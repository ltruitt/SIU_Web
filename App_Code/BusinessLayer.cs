using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using ShermcoYou.DataTypes;

public static class BusinessLayer
{
    public static IEnumerable<string> ValidateTimeEntry(SIU_TimeSheet_TimeEntryView TEV)
    {
        List<string> errors = new List<string>();
        HolidayCalculator hc = new HolidayCalculator(DateTime.Now, HttpContext.Current.Server.MapPath(@"/App_Code/Holidays.xml"));


        if ( TEV._OhAcct.Length == 0 && TEV._JobNo.Length == 0 )
            errors.Add("You Must Provide A Job Or O/H Account.");

        //////////////////////////////////
        // No Absence Time For Holidays //
        //////////////////////////////////
        if (TEV._OhAcct == "HOLIDAY")
        {
            if (!hc.IsHoldayDate(TEV.EntryDate))
                errors.Add("Holiday Time Must Fall On A Holiday.");
        }

        ////////////////////////////////////////
        // No Holidays For 30 Days After Hire //
        ////////////////////////////////////////
        if (TEV._OhAcct == "HOLIDAY")
            if (SqlServer_Impl.TestEmployeeHolidayProbation(TEV.UserEmpID, TEV.EntryDate))
                errors.Add("No Holiday Time First 30 Days.");

        //////////////////////////////////////////////////////////
        // Make Sure They Are Past Probation Period Of 3 Months //
        //////////////////////////////////////////////////////////
        if (TEV._OhAcct == "SICK")
            if (SqlServer_Impl.TestEmployeeProbation(TEV.UserEmpID, TEV.EntryDate))
                errors.Add("No Sick Time During Probabtion.");
        
        ///////////////////////////////////////////
        // Verify They Have Enough Vacation Time //
        ///////////////////////////////////////////
        if (TEV._OhAcct == "HOLIDAY")
            if (SqlServer_Impl.TestEmployeeIsContractor(TEV.UserEmpID))
                errors.Add("Holiday Time Not Available For Contract Employees.");

        ////////////////////////////////////////
        // No Vacation Time or ST On Weekends //
        ////////////////////////////////////////
        if (TEV.EntryDate.DayOfWeek == DayOfWeek.Saturday || TEV.EntryDate.DayOfWeek == DayOfWeek.Sunday)
        {
            if (TEV._OhAcct == "VACATION")
                errors.Add("No Vacation On Weekend.");

            //if ( TEV._HoursType == "ST")
            //    errors.Add("No ST On Weekend.");
        }

        ///////////////////////////////////////////////////////////////////////
        // If The Employee Is Not A Contractor But The Time Entry Date Falls //
        // On A Holiday, The Time Entered Must Be Marked Holiday Time        //
        ///////////////////////////////////////////////////////////////////////
        if (SqlServer_Impl.TestEmployeeIsContractor(TEV.UserEmpID))
            if (hc.IsHoldayDate(TEV.EntryDate))
                if (TEV._OhAcct != "HOLIDAY")
                    errors.Add("Holiday Time Required For This Date.");

        //////////////////////////////////////////////
        // Verify Employee Allowed to Post OverTime //
        //////////////////////////////////////////////
        if (TEV._HoursType == "OT" || TEV._HoursType == "DT")
            if (TEV._OhAcct.Length > 0)
                if (!SqlServer_Impl.TestEmployeeAllowedOT(TEV.UserEmpID))
                    errors.Add("Overtime Not Permitted.");

        ////////////////////////////////////////////////////////////
        // Lookup Previously Eneterd Time For The Time Entry Date //
        ////////////////////////////////////////////////////////////
        SIU_TimeSheet_DailyDetailSums sums = SqlServer_Impl.GetTimeSheet_DailyBrokenOutSums(TEV.UserEmpID, TEV.EntryDate);

        /////////////////////////////////
        // No More THan 24 Hours A Day //
        /////////////////////////////////
        if ( sums.AB + sums.DT + sums.HT + sums.OT + sums.ST > 24)
            errors.Add("Only 24 Hours In A Day.");

        //////////////////////////////////////////////////////////
        // Verify Employee Allowed To Work More Than 8 Hours ST //
        //////////////////////////////////////////////////////////
        if (sums.ST + TEV._Hours > 8)
            if (TEV._HoursType == "ST")
                if (!SqlServer_Impl.TestEmployeeAllowedGt8ST(TEV.UserEmpID))
                    errors.Add("Not Permitted More Than 8 Hours ST A Day.");

        //////////////////////////////////////////////////////////
        // Verify Employee Allowed To Work More Than 8 Hours ST //
        //////////////////////////////////////////////////////////
        //if (TEV._HoursType == "OT")
        //{
        //    if (TEV.EntryDate.DayOfWeek != DayOfWeek.Saturday && TEV.EntryDate.DayOfWeek != DayOfWeek.Sunday)
        //        if (sums.OT + TEV._Hours > 4)
        //            errors.Add("Not Permitted More Than 4 Hours OT A Day.");
        //}


        ////////////////////////
        // Max 12 hours of OT //
        ////////////////////////
        if (TEV._HoursType == "OT" && sums.OT + TEV._Hours > 12)
        {
            errors.Add("Max 12 Hours OT.");
        }


        ////////////////////
        // Only DT On Sun //
        ////////////////////
        //if (TEV.EntryDate.DayOfWeek == DayOfWeek.Sunday)
        //{
        //    if (TEV._HoursType != "DT")
        //        errors.Add("DT Only On Sunday.");
        //}


        return errors;
    }
    public static IEnumerable<string> ValidateSafetyPays(SIU_SafetyPaysReport rpt)
    {
        List<string> errors = new List<string>();
        DateTime minNavDate = DateTime.Parse("1/1/1753");

        /////////////////////////////////////////////////////
        // If 
        /////////////////////////////////////////////////////
        if (rpt.JobSite.Length == 0) 
            errors.Add("Job Site Is Missing");
        
        ///////////////////////////////////////////////////
        // Make Sure Comments Has More Than Just A Space //
        ///////////////////////////////////////////////////
        string splessComments = rpt.Comments.Replace('\n', ' ').Replace(" ", "");
            
        if (splessComments.Length == 0)
            errors.Add("Description / Comments Missing");
        
        //////////////////////////////////////////////
        // Get The Text Off the Checked Report Type //
        //////////////////////////////////////////////
        if ( rpt.IncTypeSafeFlag )
            if (rpt.IncidentDate == minNavDate )
                errors.Add("Incident Date Missing");

        if ( rpt.IncTypeUnsafeFlag )
            if (rpt.IncidentDate == minNavDate )
                errors.Add( "Incident Date Missing");


        if ( rpt.IncTypeSumFlag )
            if (rpt.SafetyMeetingDate == minNavDate)
                errors.Add( "Safety Meeting Date Missing");

        return errors;
                
    }

    public static string UserName { get { return (((string)HttpContext.Current.Session["UserUser"]) == null ) ? "" : ((string)HttpContext.Current.Session["UserUser"]).ToUpper(); } }
    public static string UserEmail { get { return (((string)HttpContext.Current.Session["UserUser"]) == null) ? "" : ((string)HttpContext.Current.Session["UserEmail"]).ToUpper(); } }
    public static string UserDomain { get { return (((string)HttpContext.Current.Session["UserDomain"]) == null) ? "" : (string)HttpContext.Current.Session["UserDomain"]; } }
    public static string UserFullName
    {
        get
        {
            return (((string)HttpContext.Current.Session["UserFullName"]) == null) ? "" : ((string)HttpContext.Current.Session["UserFullName"]).ToUpper();
        }
    }
    public static string UserEmpID
    {
        get
        {
            return (((string)HttpContext.Current.Session["UserEmpID"]) == null) ? "" : (string)HttpContext.Current.Session["UserEmpID"];
        }
    }
    public static string UserSuprFullName { get { return (((string)HttpContext.Current.Session["UserSuprFullName"]) == null) ? "" : (string)HttpContext.Current.Session["UserSuprFullName"]; } }
    public static string UserSuprEmpId { get { return (((string)HttpContext.Current.Session["UserSuprEmpID"]) == null) ? "" : (string)HttpContext.Current.Session["UserSuprEmpID"]; } }
    public static string UserDept { get { return (((string)HttpContext.Current.Session["UserDept"]) == null) ? "" : (string)HttpContext.Current.Session["UserDept"]; } }

    public static string CalcHardwarePrice(string Computer, string MonitorCnt, string StandCnt, bool chkCase, bool chkDock, bool chkBackPack, bool chkAdobe, bool chkCAD, bool chkMsPrj, bool chkVisio)
    {
        //const string method = "Forms_HardwareRequest.CalcPrice";

        ///////////////////////////////////
        // Get Cost Of Selected Computer //
        ///////////////////////////////////
        decimal price = (from aComputer in SqlServer_Impl.GetHardwareRequestComputers()
            where aComputer.Computer == Computer
            select aComputer.Price
            ).Take(1).SingleOrDefault();

        ///////////////////////
        // Get Add-Ons Table //
        ///////////////////////
        List<SIU_IT_HW_Req_Add> addOns = SqlServer_Impl.GetHardwareRequestAddOns();

        //////////////////////////
        // Add Cost Of Monitors //
        //////////////////////////
        decimal priceMon = (from mon in addOns where mon.Description == "Monitor" select mon.Price).SingleOrDefault();
        price += (priceMon * Decimal.Parse(MonitorCnt));

        /////////////////////////////////
        // Add Cost For Monitor Stands //
        /////////////////////////////////
        decimal priceMonStd = (from mon in addOns where mon.Description == "Monitor Adj Stand" select mon.Price).SingleOrDefault();
        price += (priceMonStd * Decimal.Parse(StandCnt));

        ////////////////////////////////
        // Add Cost For Computer Case //
        ////////////////////////////////
        if (chkCase)
            price += (from mon in addOns where mon.Description == "Case" select mon.Price).SingleOrDefault();

        //////////////////////////////////
        // Add Cost For Docking Station //
        //////////////////////////////////
        if (chkDock)
            price += (from mon in addOns where mon.Description == "Dock" select mon.Price).SingleOrDefault();


        ///////////////////////////
        // Add Cost For BackPack //
        ///////////////////////////
        if (chkBackPack)
            price += (from mon in addOns where mon.Description == "BackPack" select mon.Price).SingleOrDefault();



        //////////////////////////
        // Add Software Pricing //
        //////////////////////////
        // Adobe
        if (chkAdobe)
            price += (from mon in addOns where mon.Description == "Adobe Acrobat" select mon.Price).SingleOrDefault();

        // AutoCAD
        if (chkCAD)
            price += (from mon in addOns where mon.Description == "AutoCAD LT" select mon.Price).SingleOrDefault();

        // Microsoft Project
        if (chkMsPrj)
            price += (from mon in addOns where mon.Description == "Project" select mon.Price).SingleOrDefault();


        // Visio
        if (chkVisio)
            price += (from mon in addOns where mon.Description == "Visio" select mon.Price).SingleOrDefault();


        return price.ToString("C2");
    }

    public static SIU_Incident_Accident_Reports_To IncidentAccidentReportsToByID(int _UID)
    {
        SIU_Incident_Accident incRcd = SqlServer_Impl.GetIncidentAccident(_UID);
        SIU_Incident_Accident_Reports_To rc =  IncidentAccidentReportsToByEmp(incRcd.Emp_ID);

        ////////////////////////////////////
        // Now Add Already Approved Dates //
        ////////////////////////////////////
        List<SIU_Incident_Accident_Appoval> appRcds = SqlServer_Impl.GetIncidentAccidentApprovals(_UID);

        rc.DeptMgrDate = (from rcd in appRcds where rcd.EID == rc.DeptMgrEmpId select rcd.TimeStamp).SingleOrDefault();
        rc.DivMgrDate = (from rcd in appRcds where rcd.EID == rc.DivMgrEmpId select rcd.TimeStamp).SingleOrDefault();
        rc.GmDate  = (from rcd in appRcds where rcd.EID == rc.GmEmpId select rcd.TimeStamp).SingleOrDefault();
        rc.LegalMgrDate  = (from rcd in appRcds where rcd.EID == rc.LegalMgrEmpId select rcd.TimeStamp).SingleOrDefault();
        rc.SafetyMgrDate  = (from rcd in appRcds where rcd.EID == rc.SafetyMgrEmpId select rcd.TimeStamp).SingleOrDefault();
        rc.SuprDate = (from rcd in appRcds where rcd.EID == rc.SuprEmpId select rcd.TimeStamp).SingleOrDefault();
        rc.VpDate = (from rcd in appRcds where rcd.EID == rc.VpEmpId select rcd.TimeStamp).SingleOrDefault();

        return rc;
    }
    private static SIU_Incident_Accident_Reports_To IncidentAccidentReportsToByEmp(string EmpID)
    {
        //////////////////////////////////
        // Create A New Response Record //
        //////////////////////////////////
        SIU_Incident_Accident_Reports_To r2 = new SIU_Incident_Accident_Reports_To {EmpId = EmpID};

        /////////////////////////////////////////////
        // Load The Emp For Whom We Are Looking Up //
        /////////////////////////////////////////////
        Shermco_Employee emp = SqlServer_Impl.GetEmployeeByNo(EmpID);
        if (emp == null)
            return r2;
        r2.Dept = emp.Global_Dimension_1_Code;


        ////////////////////////////////////////////////////////////////////
        // And Then The Reporting Chain Based On The Employees Department //
        ////////////////////////////////////////////////////////////////////
        SIU_ReportingChain rc = SqlServer_Impl.GetEmployeeReportingChain(r2.Dept);
        if (rc == null)
            return r2;




        /////////////////////////////////
        // Load Supervisor Chain While //
        // Also Eliminating Duplicates //
        /////////////////////////////////
        List<string> eiDs = new List<string>();

        if (!eiDs.Contains(rc.GmEmpId))
        {
            r2.GmEmpId = rc.GmEmpId;
            r2.GmName = rc.GmName;
            eiDs.Add(rc.GmEmpId);
        }

        if (!eiDs.Contains(rc.VpEmpId))
        {
            r2.VpEmpId = rc.VpEmpId;
            r2.VpName = rc.VpName;
            eiDs.Add(rc.VpEmpId);
        }

        if (!eiDs.Contains(rc.SafetyMgrEmpId))
        {
            r2.SafetyMgrEmpId = rc.SafetyMgrEmpId;
            r2.SafetyMgrName = rc.SafetyMgrName;
            eiDs.Add(rc.SafetyMgrEmpId);
        }

        if (!eiDs.Contains(rc.LegalMgrEmpId))
        {
            r2.LegalMgrEmpId = rc.LegalMgrEmpId;
            r2.LegalMgrName = rc.LegalMgrName;
            eiDs.Add(rc.LegalMgrEmpId);
        }

        if (!eiDs.Contains(rc.DivMgrEmpId))
        {
            r2.DivMgrEmpId = rc.DivMgrEmpId;
            r2.DivMgrName = rc.DivMgrName;
            eiDs.Add(rc.DivMgrEmpId);
        }


        if (!eiDs.Contains(rc.DeptMgrEmpId))
        {
            r2.DeptMgrEmpId = rc.DeptMgrEmpId;
            r2.DeptMgrName = rc.DeptMgrName;
            eiDs.Add(rc.DeptMgrEmpId);
        }

        if (!eiDs.Contains(emp.Manager_No_))
        {
            r2.SuprEmpId = emp.Manager_No_;

            /////////////////////////////////////
            // Look Up The Employee Supervisor //
            /////////////////////////////////////
            emp = SqlServer_Impl.GetEmployeeByNo(r2.SuprEmpId);
            if (emp != null)
                r2.SuprName = emp.First_Name + " " + emp.Last_Name;
        }

        return r2;
    }

    public static string GenFleetInspRpt(string Dept)
    {
        string webServer = "http://" + HttpContext.Current.Request.Url.DnsSafeHost;

        List<SIU_DOT_Inspection> uncorrected;

        if (Dept.Length > 0)
            uncorrected = SqlServer_Impl.getAllOpenVehInsp(Dept);
        else
            uncorrected = SqlServer_Impl.getAllOpenVehInsp();

        if (uncorrected.Count == 0)
            return ("");

        List<Shermco_Employee> emps = SqlServer_Impl.GetActiveEmployees();

        string emailBody;
        if (Dept.Length > 0)
            emailBody = "<h1>The following is a list of uncorrected vehicle inspections for department \"" + Dept + "\".</h1>";
        else
            emailBody = "<h1>The following is a list of ALL uncorrected vehicle Inspections.</h1>";

        

        emailBody += "<table style='width: 100%'>";

        emailBody += "<tr style='color: white; background-color: blue; font_size: 1.2em; font-weight: bold;'>";
        emailBody += "<th>ID</td>";
        emailBody += "<th>Vehicle</td>";
        emailBody += "<th>Submit Name</td>";
        emailBody += "<th>Submit Date</td>";
        emailBody += "<th>Hazard</td>";
        emailBody += "</tr>";

        foreach (SIU_DOT_Inspection rpt in uncorrected)
        {
            string emp = (from thisEmp in emps where thisEmp.No_ == rpt.SubmitEmpID.TrimEnd() select thisEmp.Last_Name + ", " + thisEmp.First_Name).SingleOrDefault();

            emailBody += "<tr style='color: black; text-align: center;'>";
            emailBody += "<td><a href=" + webServer + "/ELO/VehDotEntry.aspx?rpt=" + rpt.RefID + ">" + rpt.RefID + "</a></td>";
            emailBody += "<td>" + rpt.Vehicle + "</td>";
            emailBody += "<td>(" + rpt.SubmitEmpID.TrimEnd() + ") " + emp + "</td>";
            emailBody += "<td>" + rpt.SubmitTimeStamp.ToShortDateString() + "</td>";
            emailBody += "<td style='color: black; text-align: left;'>" + rpt.Hazard + "</td>";
            emailBody += "</tr>";
        }

        emailBody += "</table>";

        return emailBody;
    }

    public static void GenQtmNotices()
    {
        string webServer = "http://" + HttpContext.Current.Request.Url.DnsSafeHost;
        List<SIU_Qom_QR> qtmList = SqlServer_Impl.GetSafetyQomQRList("0");

        DateTimeFormatInfo mfi = new DateTimeFormatInfo();
        string strMonthName = mfi.GetMonthName(DateTime.Now.Month);

        

        foreach (SIU_Qom_QR qtm in qtmList)
        {
            string emailBody = "<!DOCTYPE html><html><head><title>QTM Email</title></head><body>";

            if ( qtm.Q_Grp.ToLower() == "vest" )
                emailBody += @"<img alt='VEST IMAGE' src='" + webServer + "/images/Si-EHS-VEST.png'  width='200'  />";

            if (qtm.Q_Grp.ToLower() == "vpp")
                emailBody += @"<img alt='VPP IMAGE' src='" + webServer + "/images/Si-VPP.png'  width='200'  />";

            emailBody += "<h1>The " + strMonthName + " " + qtm.Q_Grp + " Question of the month is:</h1>";

            emailBody += "<b>" + qtm.Question + "</b>";
            emailBody += "<br/><br/>";
            emailBody += "<a href=" + webServer + "/Safety/SafetyPays/SafetyQomUser.aspx?id=" +  qtm.Q_Id  +  ">Click here to respond to this or other open Questions of the Month</a>";

            emailBody += "<br/><br/>";

            emailBody += "You may review your history of Questions and Answers ";
            emailBody += "<a href=" + webServer + "/Safety/SafetyPays/SafetyQomHistory.aspx>here</a>.";

            emailBody += "<br/><br/>";

            emailBody += "<b>If you have questions or comments, please ";
            emailBody += "<a href=mailto:aschumacher@shermco.com?subject=" + strMonthName + "%20" + qtm.Q_Grp + "%20Question%20of%20the%20Month%20message>email EHS</a></b>";

            emailBody += "<br/><br/>";
            emailBody += "</body>";

            WebMail.HtmlMail("ltruitt@shermco.com", strMonthName + " " + qtm.Q_Grp + " Question of the month", emailBody);
            //WebMail.HtmlMail("ltruitt@shermco.com; allemployees@shermco.com", strMonthName + " " + qtm.Q_Grp + " Question of the month", emailBody);
        }
    }
    public static void GenQtmReminders()
    {
        try
        {
            string webServer = "http://" + HttpContext.Current.Request.Url.DnsSafeHost;
            List<SIU_Qom_QR> qtmList = SqlServer_Impl.GetSafetyQomQRList("0");

            DateTimeFormatInfo mfi = new DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(DateTime.Now.Month);

            foreach (SIU_Qom_QR qtm in qtmList)
            {
                string emailBody = "<!DOCTYPE html><html><head><title>QTM Email</title></head><body>";

                emailBody += "<div style='border: 2 solid blue; font-size: 1.3em; font-weight: bold; margin: 10px; color: blue; padding: 3px;'>";
                emailBody += "The following email is a reminder to respond to any currently unanswered Questions of the Month</div>";

                emailBody += "<br/>";

                if (qtm.Q_Grp.ToLower() == "vest")
                    emailBody += @"<img alt='VEST IMAGE' src='" + webServer + "/images/Si-EHS-VEST.png'  width='200'  />";

                if (qtm.Q_Grp.ToLower() == "vpp")
                    emailBody += @"<img alt='VPP IMAGE' src='" + webServer + "/images/Si-VPP.png'  width='200'  />";



                emailBody += "<h1>The " + strMonthName + " " + qtm.Q_Grp + " Question of the month is:</h1>";

                emailBody += "<b>" + qtm.Question + "</b>";
                emailBody += "<br/><br/>";
                emailBody += "<a href=" + webServer + "/Safety/SafetyPays/SafetyQomUser.aspx?id=" + qtm.Q_Id + ">Click here to respond to this or other open Questions of the Month</a>";

                emailBody += "<br/><br/>";

                emailBody += "You may review your history of Questions and Answers ";
                emailBody += "<a href=" + webServer + "/Safety/SafetyPays/SafetyQomHistory.aspx>here</a>.";

                emailBody += "<br/><br/>";

                emailBody += "<b>If you have questions or comments, please ";
                emailBody += "<a href=mailto:aschumacher@shermco.com?subject=" + strMonthName + "%20" + qtm.Q_Grp + "%20Question%20of%20the%20Month%20message>email EHS</a></b>";

                emailBody += "<br/><br/>";
                emailBody += "</body>";

                WebMail.HtmlMail("ltruitt@shermco.com", strMonthName + " " + qtm.Q_Grp + " Question of the month", emailBody);
                //WebMail.HtmlMail("ltruitt@shermco.com; allemployees@shermco.com", strMonthName + " " + qtm.Q_Grp + " Question of the month", emailBody);

            }
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("GenQtmReminders", ex.Message);
        }
    }

    public static void GenQtmTest()
    {
        try
        {
            string webServer = "http://" + HttpContext.Current.Request.Url.DnsSafeHost;
            List<SIU_Qom_QR> qtmList = SqlServer_Impl.GetSafetyQomQRList("0");

            DateTimeFormatInfo mfi = new DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(DateTime.Now.Month);

            foreach (SIU_Qom_QR qtm in qtmList)
            {
                string emailBody = "<div style='border: 2 solid blue; font-size: 1.3em; font-weight: bold; margin: 10px; color: blue; padding: 3px;'>";
                emailBody += "The following email is a reminder to respond to any currently unanswered Questions of the Month</div>";

                emailBody += "<br/>";

                emailBody += "<h1>The " + strMonthName + " " + qtm.Q_Grp + " Question of the month is:</h1>";

                emailBody += "<b>" + qtm.Question + "</b>";
                emailBody += "<br/><br/>";
                emailBody += "<a href=" + webServer + "/Safety/SafetyPays/SafetyQomUser.aspx>Click here to respond to this or other open Questions of the Month</a>";

                emailBody += "<br/><br/>";

                emailBody += "You may review your history of Questions and Answers ";
                emailBody += "<a href=" + webServer + "/Safety/SafetyPays/SafetyQomHistory.aspx>here</a>.";

                emailBody += "<br/><br/>";

                emailBody += "<b>If you have questions or comments, please ";
                emailBody += "<a href=mailto:aschumacher@shermco.com?subject=" + strMonthName + "%20" + qtm.Q_Grp + "%20Question%20of%20the%20Month%20message>email EHS</a></b>";

                emailBody += "<br/><br/>";

                WebMail.HtmlMail("ltruitt@shermco.com", strMonthName + " " + qtm.Q_Grp + " Question of the month", emailBody);
            }
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("GenQtmReminders", ex.Message);
        }
    }



    //////////////////////////////////
    // Safety Pays Email Generation //
    //////////////////////////////////
    public static void SafetyPaysNewEmail(SIU_SafetyPaysReport Report, string userEmail, string userFullName)
    {
        ///////////////////////////
        // Send New-Report Email //
        ///////////////////////////
        string eMailSubject = "Safety Pays " + Report.IncTypeTxt + " Submission From: " + userFullName;

        string emailBody = "";
        emailBody += "Report ID: " + Report.IncidentNo + Environment.NewLine;
        emailBody += "Submitted By: " + userFullName + Environment.NewLine;
        emailBody += "Date Submitted: " + ToShortDate(Report.IncOpenTimestamp) + Environment.NewLine;
        emailBody += "Submission Type: " + Report.IncTypeTxt + Environment.NewLine;
        if (Report.JobNo.Length > 0)
            emailBody += "Job: " + Report.JobNo + Environment.NewLine;

        if (Report.JobSite != "-")
            emailBody += "Location: " + Report.JobSite + Environment.NewLine;

        emailBody += Environment.NewLine;

        if (Report.IncTypeTopicFlag || Report.IncTypeSumFlag)
            emailBody += "Meeting: " + Report.SafetyMeetingType + Environment.NewLine;

        if (Report.IncTypeSumFlag)
            emailBody += "Meeting Date: " + ToShortDate(Report.SafetyMeetingDate) + Environment.NewLine + Environment.NewLine;

        if (Report.IncTypeUnsafeFlag || Report.IncTypeSafeFlag)
            emailBody += "Event Date: " + ToShortDate(Report.IncidentDate) + Environment.NewLine;

        if (Report.IncTypeSafeFlag)
            emailBody += "Involving Employee: " + Report.ObservedEmpID + Environment.NewLine + Environment.NewLine;

        emailBody += "Original Remarks:" + Environment.NewLine;
        emailBody += Report.Comments + Environment.NewLine + Environment.NewLine;

        if (Report.IncTypeUnsafeFlag)
        {
            emailBody += "Corrective Action:" + Environment.NewLine;
            emailBody += Report.InitialResponse + Environment.NewLine + Environment.NewLine;
        }

        //////////////////////////////////
        // Send Email To Original  User //
        //////////////////////////////////
        WebMail.NetMail(userEmail, eMailSubject, emailBody);
    }
    public static void SafetyPaysAcceptEMail(SIU_SafetyPaysReport UpdRcd, string ccUserEmail, string userFullName)
    {
        Shermco_Employee rptByEmp = SqlServer_Impl.GetEmployeeByNo(UpdRcd.EmpID);

        //////////////////////////////////
        // Send Closed / Accepted Email //
        //////////////////////////////////
        string eMailSubject = "Approved and Closed Safety Pays Report " + UpdRcd.IncTypeTxt;

        string emailBody = "";
        emailBody += "Report ID: " + UpdRcd.IncidentNo + Environment.NewLine;
        emailBody += "Submitted By: " + rptByEmp.Last_Name + ", " + rptByEmp.First_Name + Environment.NewLine;
        emailBody += "Date Submitted: " + ToShortDate(UpdRcd.IncOpenTimestamp) + Environment.NewLine;
        emailBody += "Submission Type: " + UpdRcd.IncTypeTxt + Environment.NewLine;
        if (UpdRcd.JobNo.Length > 0)
            emailBody += "Job: " + UpdRcd.JobNo + Environment.NewLine;
        emailBody += "Location: " + UpdRcd.JobSite + Environment.NewLine + Environment.NewLine;

        if (UpdRcd.IncTypeTopicFlag || UpdRcd.IncTypeSumFlag)
            emailBody += "Meeting: " + UpdRcd.SafetyMeetingType + Environment.NewLine;

        if (UpdRcd.IncTypeSumFlag)
            emailBody += "Meeting Date: " + ToShortDate(UpdRcd.SafetyMeetingDate) + Environment.NewLine + Environment.NewLine;

        if (UpdRcd.IncTypeUnsafeFlag || UpdRcd.IncTypeSafeFlag)
            emailBody += "Event Date: " + ToShortDate(UpdRcd.IncidentDate) + Environment.NewLine;

        if (UpdRcd.IncTypeSafeFlag)
            emailBody += "Involving Employee: " + UpdRcd.ObservedEmpID + Environment.NewLine + Environment.NewLine;

        emailBody += "Original  Submission:" + Environment.NewLine;
        emailBody += UpdRcd.Comments + Environment.NewLine + Environment.NewLine;

        if (UpdRcd.IncTypeUnsafeFlag)
        {
            emailBody += "Corrective Action:" + Environment.NewLine;
            emailBody += UpdRcd.InitialResponse + Environment.NewLine + Environment.NewLine;
        }

        emailBody += "Points Awarded: " + UpdRcd.PointsAssigned.ToString() + "  On  " + ToShortDate(UpdRcd.PointsAssignedTimeStamp) + Environment.NewLine + Environment.NewLine;

        emailBody += "Approved and Closed By: " + userFullName + Environment.NewLine;
        emailBody += "Closed On: " + ToShortDate(UpdRcd.IncCloseTimestamp) + Environment.NewLine + Environment.NewLine;

        emailBody += "Notes From EHS: " + Environment.NewLine + UpdRcd.ehsRepsonse + Environment.NewLine;

        ////////////////////////////
        // Send To Original  User //
        ////////////////////////////
// ReSharper disable once ConditionIsAlwaysTrueOrFalse
// ReSharper disable once HeuristicUnreachableCode
        if (rptByEmp == null) return;

        WebMail.NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);

        //////////////////////////////////////////////////
        // If A Courtesy Copy Was Requested, Send It On //
        //////////////////////////////////////////////////
        if (ccUserEmail.Length > 0)
        {
            eMailSubject = "Courtesy Copy: " + eMailSubject;
            emailBody = "The EHS Department requested that a copy of the following Email be forwarded to you." + emailBody + Environment.NewLine + Environment.NewLine;
            WebMail.NetMail(ccUserEmail, eMailSubject, emailBody);
        }
    }
    public static void SafetyPaysRejectedEMail(SIU_SafetyPaysReport UpdRcd, string ccUserEmail, string userFullName)
    {
        Shermco_Employee rptByEmp = SqlServer_Impl.GetEmployeeByNo(UpdRcd.EmpID);

        //////////////////////////////////
        // Send Closed / Accepted Email //
        //////////////////////////////////
        string eMailSubject = "Not Approved and Closed Safety Pays Report " + UpdRcd.IncTypeTxt;

        string emailBody = "";
        emailBody += "Report ID: " + UpdRcd.IncidentNo + Environment.NewLine;
        emailBody += "Submitted By: " + rptByEmp.Last_Name + ", " + rptByEmp.First_Name + Environment.NewLine;
        emailBody += "Date Submitted: " + ToShortDate(UpdRcd.IncOpenTimestamp) + Environment.NewLine;
        emailBody += "Submission Type: " + UpdRcd.IncTypeTxt + Environment.NewLine;
        if (UpdRcd.JobNo.Length > 0)
            emailBody += "Job: " + UpdRcd.JobNo + Environment.NewLine;
        emailBody += "Location: " + UpdRcd.JobSite + Environment.NewLine + Environment.NewLine;

        if (UpdRcd.IncTypeTopicFlag || UpdRcd.IncTypeSumFlag)
            emailBody += "Meeting: " + UpdRcd.SafetyMeetingType + Environment.NewLine;

        if (UpdRcd.IncTypeSumFlag)
            emailBody += "Meeting Date: " + ToShortDate(UpdRcd.SafetyMeetingDate) + Environment.NewLine + Environment.NewLine;


        if (UpdRcd.IncTypeUnsafeFlag || UpdRcd.IncTypeSafeFlag)
            emailBody += "Event Date: " + ToShortDate(UpdRcd.IncidentDate) + Environment.NewLine;

        if (UpdRcd.IncTypeSafeFlag)
            emailBody += "Involving Employee: " + UpdRcd.ObservedEmpID + Environment.NewLine + Environment.NewLine;

        emailBody += "Original  Submission:" + Environment.NewLine;
        emailBody += UpdRcd.Comments + Environment.NewLine + Environment.NewLine;

        if (UpdRcd.IncTypeUnsafeFlag)
        {
            emailBody += "Corrective Action:" + Environment.NewLine;
            emailBody += UpdRcd.InitialResponse + Environment.NewLine + Environment.NewLine;
        }

        emailBody += "Rejected and Closed By: " + userFullName + Environment.NewLine;
        emailBody += "Rejected On: " + ToShortDate(UpdRcd.IncCloseTimestamp) + Environment.NewLine + Environment.NewLine;

        emailBody += "Notes From EHS: " + Environment.NewLine + UpdRcd.ehsRepsonse + Environment.NewLine;

        //////////////////////////////////
        // Send Email To Original  User //
        //////////////////////////////////
        if (rptByEmp != null)
            WebMail.NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);

        //////////////////////////////////////////////////
        // If A Courtesy Copy Was Requested, Send It On //
        //////////////////////////////////////////////////
        if (ccUserEmail.Length > 0)
        {
            eMailSubject = "Courtesy Copy: " + eMailSubject;
            emailBody = "The EHS Department requested that a copy of the following Email be forwarded to you." + Environment.NewLine + Environment.NewLine + emailBody;
            WebMail.NetMail(ccUserEmail, eMailSubject, emailBody);
        }
    }
    public static void SafetyPaysWorkingEMail(SIU_SafetyPaysReport UpdRcd, string ccUserEmail, string userFullName)
    {
        Shermco_Employee rptByEmp = SqlServer_Impl.GetEmployeeByNo(UpdRcd.EmpID);

        //////////////////////////////////
        // Send Closed / Accepted Email //
        //////////////////////////////////
        string eMailSubject = "Accepted with Task Safety Pays Report " + UpdRcd.IncTypeTxt;

        string emailBody = "";
        emailBody += "Report ID: " + UpdRcd.IncidentNo + Environment.NewLine;
        emailBody += "Submitted By: " + rptByEmp.Last_Name + ", " + rptByEmp.First_Name + Environment.NewLine;
        emailBody += "Date Submitted: " + ToShortDate(UpdRcd.IncOpenTimestamp) + Environment.NewLine;
        emailBody += "Submission Type: " + UpdRcd.IncTypeTxt + Environment.NewLine;
        if (UpdRcd.JobNo.Length > 0)
            emailBody += "Job: " + UpdRcd.JobNo + Environment.NewLine;
        emailBody += "Location: " + UpdRcd.JobSite + Environment.NewLine + Environment.NewLine;

        if (UpdRcd.IncTypeTopicFlag || UpdRcd.IncTypeSumFlag)
            emailBody += "Meeting: " + UpdRcd.SafetyMeetingType + Environment.NewLine;

        if (UpdRcd.IncTypeSumFlag)
            emailBody += "Meeting Date: " + ToShortDate(UpdRcd.SafetyMeetingDate) + Environment.NewLine + Environment.NewLine;

        if (UpdRcd.IncTypeUnsafeFlag || UpdRcd.IncTypeSafeFlag)
            emailBody += "Event Date: " + ToShortDate(UpdRcd.IncidentDate) + Environment.NewLine;

        if (UpdRcd.IncTypeSafeFlag)
            emailBody += "Involving Employee: " + UpdRcd.ObservedEmpID + Environment.NewLine + Environment.NewLine;

        emailBody += "Original  Submission:" + Environment.NewLine;
        emailBody += UpdRcd.Comments + Environment.NewLine + Environment.NewLine;

        if (UpdRcd.IncTypeUnsafeFlag)
        {
            emailBody += "Corrective Action:" + Environment.NewLine;
            emailBody += UpdRcd.InitialResponse + Environment.NewLine + Environment.NewLine;
        }

        emailBody += "Points Awarded: " + UpdRcd.PointsAssigned.ToString() + "  On  " + UpdRcd.PointsAssignedTimeStamp + Environment.NewLine + Environment.NewLine;

        emailBody += "Accepted for Work By: " + userFullName + Environment.NewLine;
        emailBody += "Accepted On: " + ToShortDate(UpdRcd.IncCloseTimestamp) + Environment.NewLine + Environment.NewLine;

        emailBody += "Notes From EHS: " + Environment.NewLine + UpdRcd.ehsRepsonse + Environment.NewLine;

        ///////////////////////////
        // Send To Original  User //
        ///////////////////////////
        if (rptByEmp != null)
            WebMail.NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);

        //////////////////////////////////////////////////
        // If A Courtesy Copy Was Requested, Send It On //
        //////////////////////////////////////////////////
        if (ccUserEmail.Length > 0)
        {
            eMailSubject = "Courtesy Copy: " + eMailSubject;
            emailBody = "The EHS Department requested that a copy of the following Email be forwarded to you." + Environment.NewLine + Environment.NewLine + emailBody;
            WebMail.NetMail(ccUserEmail, eMailSubject, emailBody);
        }
    }
    public static void SafetyPaysClosedEMail(SIU_SafetyPaysReport UpdRcd, string ccUserEmail, string userFullName)
    {
        Shermco_Employee rptByEmp = SqlServer_Impl.GetEmployeeByNo(UpdRcd.EmpID);

        //////////////////////////////////
        // Send Closed / Accepted Email //
        //////////////////////////////////
        string eMailSubject = "Closed Safety Pays Report " + UpdRcd.IncTypeTxt;

        string emailBody = "";
        emailBody += "Report ID: " + UpdRcd.IncidentNo + Environment.NewLine;
        emailBody += "Submitted By: " + rptByEmp.Last_Name + ", " + rptByEmp.First_Name + Environment.NewLine;
        emailBody += "Date Submitted: " + ToShortDate(UpdRcd.IncOpenTimestamp) + Environment.NewLine;
        emailBody += "Submission Type: " + UpdRcd.IncTypeTxt + Environment.NewLine;
        if (UpdRcd.JobNo.Length > 0)
            emailBody += "Job: " + UpdRcd.JobNo + Environment.NewLine;
        emailBody += "Location: " + UpdRcd.JobSite + Environment.NewLine + Environment.NewLine;

        if (UpdRcd.IncTypeTopicFlag || UpdRcd.IncTypeSumFlag)
            emailBody += "Meeting: " + UpdRcd.SafetyMeetingType + Environment.NewLine;

        if (UpdRcd.IncTypeSumFlag)
            emailBody += "Meeting Date: " + ToShortDate(UpdRcd.SafetyMeetingDate) + Environment.NewLine + Environment.NewLine;

        if (UpdRcd.IncTypeUnsafeFlag || UpdRcd.IncTypeSafeFlag)
            emailBody += "Event Date: " + ToShortDate(UpdRcd.IncidentDate) + Environment.NewLine;

        if (UpdRcd.IncTypeSafeFlag)
            emailBody += "Involving Employee: " + UpdRcd.ObservedEmpID + Environment.NewLine + Environment.NewLine;

        emailBody += "Original  Submission:" + Environment.NewLine;
        emailBody += UpdRcd.Comments + Environment.NewLine + Environment.NewLine;

        if (UpdRcd.IncTypeUnsafeFlag)
        {
            emailBody += "Corrective Action:" + Environment.NewLine;
            emailBody += UpdRcd.InitialResponse + Environment.NewLine + Environment.NewLine;
        }

        emailBody += "Closed By: " + userFullName + Environment.NewLine;
        emailBody += "Closed On: " + ToShortDate(UpdRcd.IncCloseTimestamp) + Environment.NewLine + Environment.NewLine;

        emailBody += "Notes From EHS: " + Environment.NewLine + UpdRcd.ehsRepsonse + Environment.NewLine;

        ////////////////////////////
        // Send To Original  User //
        ////////////////////////////
        if (rptByEmp != null)
            WebMail.NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);

        //////////////////////////////////////////////////
        // If A Courtesy Copy Was Requested, Send It On //
        //////////////////////////////////////////////////
        if (ccUserEmail.Length > 0)
        {
            eMailSubject = "Courtesy Copy: " + eMailSubject;
            emailBody = "The EHS Department requested that a copy of the following Email be forwarded to you." + Environment.NewLine + Environment.NewLine + emailBody;
            WebMail.NetMail(ccUserEmail, eMailSubject, emailBody);
        }
    }
    public static void SafetyPaysTaskAssignEMail(SIU_SafetyPays_TaskList UpdRcd, string userEmail, string userFullName)
    {
        SIU_SafetyPaysReport report = SqlServer_Impl.GetSafetyPaysReport(UpdRcd.IncidentNo).SingleOrDefault();
        if (report != null)
        {
            Shermco_Employee rptByEmp = SqlServer_Impl.GetEmployeeByNo(report.EmpID);

            //////////////////////////////////
            // Send Closed / Accepted Email //
            //////////////////////////////////
            string eMailSubject = "New Safety Pays Task Assigned To " + SqlServer_Impl.GetEmployeeNameByNo(UpdRcd.AssignedEmpID);

            string emailBody = "";
            emailBody += "Report ID: " + UpdRcd.IncidentNo + Environment.NewLine;
            emailBody += "Manage your task status here: http://" + Environment.MachineName + "/Safety/SafetyPays/SafetyPaysTasks.aspx?RptID=" + UpdRcd.IncidentNo + Environment.NewLine;
            emailBody += "Submit status reports weekly." + Environment.NewLine;
            emailBody += "Close the task when you complete the work." + Environment.NewLine + Environment.NewLine;


            emailBody += "Submitted By: " + rptByEmp.Last_Name + ", " + rptByEmp.First_Name + Environment.NewLine;
            emailBody += "Date Submitted: " + ToShortDate(report.IncOpenTimestamp) + Environment.NewLine;
            emailBody += "Submission Type: " + report.IncTypeTxt + Environment.NewLine;
            if (report.JobNo.Length > 0)
                emailBody += "Job: " + report.JobNo + Environment.NewLine;
            emailBody += "Location: " + report.JobSite + Environment.NewLine + Environment.NewLine;

            if (report.IncTypeTopicFlag || report.IncTypeSumFlag)
                emailBody += "Meeting: " + report.SafetyMeetingType + Environment.NewLine;

            if (report.IncTypeSumFlag)
                emailBody += "Meeting Date: " + ToShortDate(report.SafetyMeetingDate) + Environment.NewLine + Environment.NewLine;

            if (report.IncTypeUnsafeFlag || report.IncTypeSafeFlag)
                emailBody += "Event Date: " + ToShortDate(report.IncidentDate) + Environment.NewLine;

            if (report.IncTypeSafeFlag)
                emailBody += "Involving Employee: " + report.ObservedEmpID + Environment.NewLine + Environment.NewLine;

            emailBody += "Original  Submission:" + Environment.NewLine;
            emailBody += report.Comments + Environment.NewLine + Environment.NewLine;

            if (report.IncTypeUnsafeFlag)
            {
                emailBody += "Corrective Action:" + Environment.NewLine;
                emailBody += report.InitialResponse + Environment.NewLine + Environment.NewLine;
            }

            emailBody += "Task Created By: " + userFullName + Environment.NewLine;
            emailBody += "Task Due By: " + UpdRcd.DueDate + Environment.NewLine;
            emailBody += "Task Description: " + UpdRcd.TaskDefinition + Environment.NewLine + Environment.NewLine;

            ////////////////////////////
            // Send To Original  User //
            ////////////////////////////
            if (rptByEmp != null)
                WebMail.NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);

            /////////////////
            // Assigned To //
            /////////////////
            rptByEmp = SqlServer_Impl.GetEmployeeByNo(UpdRcd.AssignedEmpID);
            WebMail.NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);
        }
    }
    public static void SafetyPaysTaskCompleteEMail(SIU_SafetyPays_TaskList UpdRcd, string userEmail, string userFullName, string CloseNotes)
    {
        SIU_SafetyPaysReport report = SqlServer_Impl.GetSafetyPaysReport(UpdRcd.IncidentNo).SingleOrDefault();
        if (report != null)
        {
            Shermco_Employee rptByEmp = SqlServer_Impl.GetEmployeeByNo(report.EmpID);

            //////////////////////////////////
            // Send Closed / Accepted Email //
            //////////////////////////////////
            const string eMailSubject = "Safety Pays Task Assigned Completed";

            string emailBody = "";
            emailBody += "A task related to the Safety Pays submission (report)" + Environment.NewLine;
            emailBody += "shown below was closed.  This task may be one " + Environment.NewLine;
            emailBody += "of many tasks related to the report. A seperate" + Environment.NewLine;
            emailBody += "notice will be sent when all tasks are complete " + Environment.NewLine;
            emailBody += "and the report is closed." + Environment.NewLine + Environment.NewLine;

            emailBody += "Report ID: " + UpdRcd.IncidentNo + Environment.NewLine;
            emailBody += "Submitted By: " + rptByEmp.Last_Name + ", " + rptByEmp.First_Name + Environment.NewLine;
            emailBody += "Date Submitted: " + ToShortDate(report.IncOpenTimestamp) + Environment.NewLine;
            emailBody += "Submission Type: " + report.IncTypeTxt + Environment.NewLine;
            if (report.JobNo.Length > 0)
                emailBody += "Job: " + report.JobNo + Environment.NewLine;
            emailBody += "Location: " + report.JobSite + Environment.NewLine + Environment.NewLine;

            if (report.IncTypeTopicFlag || report.IncTypeSumFlag)
                emailBody += "Meeting: " + report.SafetyMeetingType + Environment.NewLine;

            if (report.IncTypeSumFlag)
                emailBody += "Meeting Date: " + ToShortDate(report.SafetyMeetingDate) + Environment.NewLine + Environment.NewLine;

            if (report.IncTypeUnsafeFlag || report.IncTypeSafeFlag)
                emailBody += "Event Date: " + ToShortDate(report.IncidentDate) + Environment.NewLine;

            if (report.IncTypeSafeFlag)
                emailBody += "Involving Employee: " + report.ObservedEmpID + Environment.NewLine + Environment.NewLine;

            emailBody += "Original  Submission:" + Environment.NewLine;
            emailBody += report.Comments + Environment.NewLine + Environment.NewLine;

            if (report.IncTypeUnsafeFlag)
            {
                emailBody += "Corrective Action:" + Environment.NewLine;
                emailBody += report.InitialResponse + Environment.NewLine + Environment.NewLine;
            }

            emailBody += "Task Closed By: " + userFullName + Environment.NewLine;
            emailBody += "Task Closed On: " + UpdRcd.CompletedDate + Environment.NewLine;
            emailBody += "Task Description: " + UpdRcd.TaskDefinition + Environment.NewLine + Environment.NewLine;

            emailBody += "Closing Notes: " + CloseNotes + Environment.NewLine;


            ////////////////////////////
            // Send To Original  User //
            ////////////////////////////
            if (rptByEmp != null)
                WebMail.NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);

            /////////////////
            // Assigned To //
            /////////////////
            rptByEmp = SqlServer_Impl.GetEmployeeByNo(UpdRcd.AssignedEmpID);
            WebMail.NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);
        }
    }
    public static void SafetyPaysObservedEMail(SIU_SafetyPaysReport UpdRcd, int Points)
    {
        Shermco_Employee rptByEmp = SqlServer_Impl.GetEmployeeByNo(UpdRcd.EmpID);

        /////////////////////////////////////
        // Send Observed and Awarded Email //
        /////////////////////////////////////
        string eMailSubject = "You Were Observed In A Safe Act And Awarded One Or More Points" + UpdRcd.IncTypeTxt;

        string emailBody = "";

        emailBody += "Submitted By: " + rptByEmp.Last_Name + ", " + rptByEmp.First_Name + Environment.NewLine;
        emailBody += "Date Submitted: " + ToShortDate(UpdRcd.IncOpenTimestamp) + Environment.NewLine;
        emailBody += "Submission Type: " + UpdRcd.IncTypeTxt + Environment.NewLine;
        if (UpdRcd.JobNo.Length > 0)
            emailBody += "Job: " + UpdRcd.JobNo + Environment.NewLine;
        emailBody += "Location: " + UpdRcd.JobSite + Environment.NewLine + Environment.NewLine;

        if (UpdRcd.IncTypeTopicFlag || UpdRcd.IncTypeSumFlag)
            emailBody += "Meeting: " + UpdRcd.SafetyMeetingType + Environment.NewLine;

        if (UpdRcd.IncTypeSumFlag)
            emailBody += "Meeting Date: " + ToShortDate(UpdRcd.SafetyMeetingDate) + Environment.NewLine + Environment.NewLine;

        if (UpdRcd.IncTypeUnsafeFlag || UpdRcd.IncTypeSafeFlag)
            emailBody += "Event Date: " + ToShortDate(UpdRcd.IncidentDate) + Environment.NewLine;

        if (UpdRcd.IncTypeSafeFlag)
            emailBody += "Involving Employee: " + UpdRcd.ObservedEmpID + Environment.NewLine + Environment.NewLine;

        emailBody += "Original  Submission:" + Environment.NewLine;
        emailBody += UpdRcd.Comments + Environment.NewLine + Environment.NewLine;

        if (UpdRcd.IncTypeUnsafeFlag)
        {
            emailBody += "Corrective Action:" + Environment.NewLine;
            emailBody += UpdRcd.InitialResponse + Environment.NewLine + Environment.NewLine;
        }

        emailBody += "Points Awarded: " + Points + "  On  " + ToShortDate(UpdRcd.PointsAssignedTimeStamp) + Environment.NewLine + Environment.NewLine;

        emailBody += "Closed On: " + ToShortDate(UpdRcd.IncCloseTimestamp) + Environment.NewLine + Environment.NewLine;

        emailBody += "Notes From EHS: " + Environment.NewLine + UpdRcd.ehsRepsonse + Environment.NewLine;

        Shermco_Employee obsByEmp = SqlServer_Impl.GetEmployeeByNo(UpdRcd.ObservedEmpID);
        WebMail.NetMail(obsByEmp.Company_E_Mail, eMailSubject, emailBody);
    }

    ///////////////////////
    // Job Report Emails //
    ///////////////////////
    public static void JobReportIrNotice(Shermco_Job_Report Rpt)
    {
        const string eMailSubject = "I/R Alert for Reports";
        const string addressList = "SIU_IR_JOBRPT@shermco.com";

        string emailBody = "An I/R report was submitted for job: " + Rpt.Job_No_ + Environment.NewLine;
        emailBody += "Data in Dropbox: " + ((Rpt.TmpIRData == 1) ? "Yes" : "No") + Environment.NewLine;
        emailBody += "Copies Requested: " + Rpt.No__of_Copies + Environment.NewLine;
        emailBody += "Email: " + Rpt.Email + Environment.NewLine;
        emailBody += "Comment: " + Rpt.Comment + Environment.NewLine;
        emailBody += "Submitted by: " + SqlServer_Impl.GetEmployeeNameByNo(Rpt.Turned_in_By_Emp__No_) + Environment.NewLine;
        emailBody += "Lead Tech: " + SqlServer_Impl.GetJobLeadTechName(Rpt.Job_No_) + Environment.NewLine;
        emailBody += ((Rpt.IROnly == 1) ? "This is the only report for the job." : "IR is portion of Final Report") + Environment.NewLine;

        WebMail.NetMail(addressList, eMailSubject, emailBody);
    }
    public static void JobSalesNotice(Shermco_Job_Report Rpt)
    {
        string addressList = "kahrendt@shermco.com, spayne@shermco.com, rloveless@shermco.com";
        const string eMailSubject = "Priority - Contact Customer Regarding Work";

        var jr = SqlServer_Impl.GetJobByNo(Rpt.Job_No_);
        var sp = SqlServer_Impl.GetEmployeeByNo(jr.Sales_Person);
        var cn = SqlServer_Impl.GetCustomerName(jr.Bill_to_Customer_No_);
        if (sp != null)
            addressList += ", " + sp.Company_E_Mail;

        string emailBody = "Job No.: " + Rpt.Job_No_ + Environment.NewLine;
        emailBody += "Customer: (" + jr.Bill_to_Customer_No_ + ") " + cn + Environment.NewLine;
        emailBody += "Job Description: " + jr.Description + Environment.NewLine;
        emailBody += "DivDep: " + jr.Global_Dimension_1_Code + Environment.NewLine;
        emailBody += "Tech Submitted: " + SqlServer_Impl.GetEmployeeNameByNo(Rpt.Turned_in_By_Emp__No_) + Environment.NewLine;
        emailBody += "Comment for Sales: " + Rpt.SalesFollowUp_Comment + Environment.NewLine + Environment.NewLine;

        emailBody += "Site Location: " + jr.Site_Location + Environment.NewLine;
        emailBody += "Site Address: " + jr.Site_Address + Environment.NewLine;
        if (jr.Site_Address_2.Length > 0)
            emailBody += "Site Address: " + jr.Site_Address_2 + Environment.NewLine;
        emailBody += "Site City: " + jr.Site_City + Environment.NewLine;
        emailBody += "Site State: " + jr.Site_County + Environment.NewLine;
        emailBody += "Site Zip: " + jr.Site_Post_Code + Environment.NewLine + Environment.NewLine;

        emailBody += "Sell-To Contact: " + jr.Sell_To_Contact + Environment.NewLine;
        emailBody += "Sell-To Contact Phone: " + jr.Sell_To_Contact_Phone_No_ + Environment.NewLine;
        emailBody += "Sell-To Contact Mobile: " + jr.Sell_To_Contact_Mobile_No_ + Environment.NewLine;
        emailBody += "Sell-To Contact Email: " + jr.Sell_To_Contact_Email + Environment.NewLine + Environment.NewLine;

        emailBody += "Report Comments:" + Environment.NewLine;
        emailBody += Rpt.Comment;
        emailBody += Environment.NewLine;

        WebMail.NetMail(addressList, eMailSubject, emailBody);
    }
    public static void JobSalesNotice7071(Shermco_Job_Report Rpt)
    {
        string addressList = "SIU7071SubmitJobReport@shermco.com";
        const string eMailSubject = "Priority - Contact Customer Regarding Work";

        var jr = SqlServer_Impl.GetJobByNo(Rpt.Job_No_);
        var sp = SqlServer_Impl.GetEmployeeByNo(jr.Sales_Person);
        var cn = SqlServer_Impl.GetCustomerName(jr.Bill_to_Customer_No_);
        if (sp != null)
            addressList += ", " + sp.Company_E_Mail;

        string emailBody = "<div style='border: 2 solid blue; font-size: 1.3em; font-weight: bold; margin: 10px; color: white; background-color: blue; padding: 3px;'>";
        emailBody += "<h1> The following email is a request to contact.... " + cn + " (" + jr.Bill_to_Customer_No_ + ")  </h1></div>";

        emailBody += "<div style='margin-left: 20px; font-size: 1.5em;'><b>Regarding</b></div><br/>";

        emailBody += "<table style='margin-left: 20px;'><tr>";
        emailBody += "<td style='width: 200px;'><b>Job No.:</b></td><td>" + Rpt.Job_No_   + "</td></tr>";


        emailBody += "<tr><td><b>Job Description:</b></td><td>" + jr.Description + "</td></tr>";
        emailBody += "<tr><td><b>DivDep:</b></td><td>" + jr.Global_Dimension_1_Code + "</td></tr>";
        emailBody += "<tr><td><b>Tech Submitted:</b></td><td>" + SqlServer_Impl.GetEmployeeNameByNo(Rpt.Turned_in_By_Emp__No_) + "</td></tr>";
        emailBody += "<tr><td><b>Comment for Sales:</b></td><td style='background-color: yellow; color: red;'><b>" + Rpt.SalesFollowUp_Comment + "</b></td></tr>";
        emailBody += "<tr></tr>";

        emailBody += "<tr><td><b>Site Location:</b></td><td>" + jr.Site_Location + "</td></tr>";
        emailBody += "<tr><td><b>Site Address:</b></td><td>" + jr.Site_Address + "</td></tr>";

        if (jr.Site_Address_2.Length > 0)
            emailBody += "<tr><td><b>Site Address:</b></td><td>" + jr.Site_Address_2 + "</td></tr>";

        emailBody += "<tr><td><b>Site City:</b></td><td>" + jr.Site_City + "</td></tr>";
        emailBody += "<tr><td><b>Site State:</b></td><td>" + jr.Site_County + "</td></tr>";
        emailBody += "<tr><td><b>Site Zip:</b></td><td>" + jr.Site_Post_Code + "</td></tr>";
        emailBody += "<tr></tr>";

        emailBody += "<tr><td><b>Sell-To Contact:</b></td><td>" + jr.Sell_To_Contact + "</td></tr>";
        emailBody += "<tr><td><b>Sell-To Contact Phone:</b></td><td>" + jr.Sell_To_Contact_Phone_No_ + "</td></tr>";
        emailBody += "<tr><td><b>Sell-To Contact Mobile:</b></td><td>" + jr.Sell_To_Contact_Mobile_No_ + "</td></tr>";
        emailBody += "<tr><td><b>Sell-To Contact Email:</b></td><td>" + jr.Sell_To_Contact_Email + "</td></tr>";
        emailBody += "<tr></tr>";

        emailBody += "<tr><td><b>Report Comments:</b></td><td style='background-color: yellow; color: red;'><b>";
        emailBody += Rpt.Comment ;
        emailBody += "</b></td></tr></table>";

        WebMail.HtmlMail(addressList, eMailSubject, emailBody);
    }
    public static void JobTechAckNotice(Shermco_Job_Report Rpt)
    {
        try
        {
            string addressList = SqlServer_Impl.GetEmployeeEmailByNo(new List<string> { Rpt.Turned_in_by_Tech_UserID })[0];

            const string eMailSubject = "Job Report Submission Acknowledgement";

            var jr = SqlServer_Impl.GetJobByNo(Rpt.Job_No_);
            var cn = SqlServer_Impl.GetCustomerName(jr.Bill_to_Customer_No_);

            string emailBody = "<b>Job No.: </b>" + Rpt.Job_No_ + "<br/>";
            emailBody += "<b>Customer: </b>(" + jr.Bill_to_Customer_No_ + ") " + cn + "<br/>";
            emailBody += "<b>Job Description: </b>" + jr.Description + "<br/>";
            emailBody += "<b>DivDep: </b>" + jr.Global_Dimension_1_Code + "<br/>";
            emailBody += "<b>Tech Submitted: </b>" + SqlServer_Impl.GetEmployeeNameByNo(Rpt.Turned_in_By_Emp__No_) + "<br/>";
            emailBody += "<b>Comment for Sales: </b>" + Rpt.SalesFollowUp_Comment + "<br/>" + "<br/>";

            emailBody += "<b>Site Location: </b>" + jr.Site_Location + "<br/>";
            emailBody += "<b>Site Address: </b>" + jr.Site_Address + "<br/>";
            if (jr.Site_Address_2.Length > 0)
                emailBody += "<b>Site Address: </b>" + jr.Site_Address_2 + "<br/>";
            emailBody += "<b>Site City: </b>" + jr.Site_City + "<br/>";
            emailBody += "<b>Site State: </b>" + jr.Site_County + "<br/>";
            emailBody += "<b>Site Zip: </b>" + jr.Site_Post_Code + "<br/>" + "<br/>";

            emailBody += "<b>Sell-To Contact: </b>" + jr.Sell_To_Contact + "<br/>";
            emailBody += "<b>Sell-To Contact Phone: </b>" + jr.Sell_To_Contact_Phone_No_ + "<br/>";
            emailBody += "<b>Sell-To Contact Mobile: </b>" + jr.Sell_To_Contact_Mobile_No_ + "<br/>";
            emailBody += "<b>Sell-To Contact Email: </b>" + jr.Sell_To_Contact_Email + "<br/>" + "<br/>";

            emailBody += "<b>Report Comments: </b>" + "<br/>";
            emailBody += Rpt.Comment;
            emailBody += "<br/>";

            WebMail.HtmlMail(addressList, eMailSubject, emailBody);
        }

        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("WebMail.JobTechAckNotice", ex.Message);
            if (Rpt != null)
                SqlServer_Impl.LogDebug("WebMail.JobTechAckNotice", "JobNo: " + Rpt.Job_No_);
        }
    }

    //////////////////////
    // No Longer In Use //
    //////////////////////
    public static void MovieCompletedEMail(string empID, string videoName)
    {
        const string eMailSubject = "Training Video Completed";
        string addressList = SqlServer_Impl.GetEmployeeByNo(empID).Company_E_Mail;

        if (addressList == null) return;
        if (addressList.Length == 0) return;


        string emailBody = "Thank you for viewing the following video persentation." + Environment.NewLine + Environment.NewLine;

        emailBody += "Video: " + videoName + Environment.NewLine + Environment.NewLine;

        emailBody += "There may be related documents and quizzes. " + Environment.NewLine;
        emailBody += "These documents will be listed to the right of the" + Environment.NewLine;
        emailBody += "video under the heading SUPPORT DOCUMENTS." + Environment.NewLine;


        WebMail.NetMail(addressList, eMailSubject, emailBody);
    }

    //////////////////////
    // No Longer In Use //
    //////////////////////
    public static void AccidentIncidentSubmitted(SIU_Incident_Accident_Reports_To RT, SIU_Incident_Accident incRcd)
    {
        const string eMailSubject = "Accident or Incident Event Submitted For Completion Approval";

        string addressList = String.Empty;

        List<string> empNos = new List<string>
        {
            RT.DeptMgrEmpId,
            RT.DivMgrEmpId,
            RT.GmEmpId,
            RT.LegalMgrEmpId,
            RT.SafetyMgrEmpId,
            RT.SuprEmpId,
            RT.VpEmpId
        };

        foreach (string emailAddr in SqlServer_Impl.GetEmployeeEmailByNo(empNos))
        {
            if (emailAddr.Length > 0)
            {
                if (addressList.Length == 0)
                    addressList = emailAddr;
                else
                    addressList += ", " + emailAddr;
            }
        }

        if (addressList.Length == 0) return;


        string emailBody = "An Accident or Incident event was submitted for your approval." + Environment.NewLine + Environment.NewLine;

        emailBody += "Your Approval is requested." + Environment.NewLine + Environment.NewLine;

        emailBody += "Event Description: " + incRcd.Inc_Desc + Environment.NewLine;
        emailBody += "Involved Employee: " + SqlServer_Impl.GetEmployeeNameByNo(incRcd.Emp_ID) + Environment.NewLine + Environment.NewLine;

        emailBody += "You may view the details of the incident " + Environment.NewLine;
        emailBody += "as well as view and submit progress notes" + Environment.NewLine;
        emailBody += "here: http://" + Environment.MachineName + "/Safety/Incident/Approve.aspx?RptID=" + incRcd.UID + Environment.NewLine;


        WebMail.NetMail(addressList, eMailSubject, emailBody);
    }
    public static void AccidentIncidentApproved(SIU_Incident_Accident_Reports_To RT, SIU_Incident_Accident incRcd)
    {
        string eMailSubject = "Accident or Incident Event Approved By " + UserFullName;
        string addressList = String.Empty;

        List<string> empNos = new List<string>
        {
            RT.DeptMgrEmpId,
            RT.DivMgrEmpId,
            RT.GmEmpId,
            RT.LegalMgrEmpId,
            RT.SafetyMgrEmpId,
            RT.SuprEmpId,
            RT.VpEmpId
        };

        foreach (string emailAddr in SqlServer_Impl.GetEmployeeEmailByNo(empNos))
        {
            if (emailAddr.Length > 0)
            {
                if (addressList.Length == 0)
                    addressList = emailAddr;
                else
                    addressList += ", " + emailAddr;
            }
        }

        if (addressList.Length == 0) return;

        string emailBody = eMailSubject + Environment.NewLine + Environment.NewLine;

        emailBody += "Event Description: " + incRcd.Inc_Desc + Environment.NewLine;
        emailBody += "Involved Employee: " + SqlServer_Impl.GetEmployeeNameByNo(incRcd.Emp_ID) + Environment.NewLine + Environment.NewLine;

        emailBody += "You may view the details of the incident " + Environment.NewLine;
        emailBody += "as well as view and submit progress notes" + Environment.NewLine;
        emailBody += "here: http://" + Environment.MachineName + "/Safety/Incident/Approve.aspx?RptID=" + incRcd.UID + Environment.NewLine;



        WebMail.NetMail(addressList, eMailSubject, emailBody);
    }
    public static void AccidentIncidentCommented(SIU_Incident_Accident_Reports_To RT, SIU_Incident_Accident incRcd, string newComment)
    {
        string eMailSubject = "Accident or Incident Event Commented By " + UserFullName;
        string addressList = String.Empty;

        List<string> empNos = new List<string>
        {
            RT.DeptMgrEmpId,
            RT.DivMgrEmpId,
            RT.GmEmpId,
            RT.LegalMgrEmpId,
            RT.SafetyMgrEmpId,
            RT.SuprEmpId,
            RT.VpEmpId
        };

        foreach (string emailAddr in SqlServer_Impl.GetEmployeeEmailByNo(empNos))
        {
            if (emailAddr.Length > 0)
            {
                if (addressList.Length == 0)
                    addressList = emailAddr;
                else
                    addressList += ", " + emailAddr;
            }
        }

        if (addressList.Length == 0) return;

        string emailBody = eMailSubject + Environment.NewLine + Environment.NewLine;

        emailBody += "Event Description: " + incRcd.Inc_Desc + Environment.NewLine;
        emailBody += "Involved Employee: " + SqlServer_Impl.GetEmployeeNameByNo(incRcd.Emp_ID) + Environment.NewLine + Environment.NewLine;

        emailBody += "*** If you previously approved this event you will need to do so again using the link below..............." + Environment.NewLine + Environment.NewLine;

        emailBody += "Notation or Comment added: " + newComment + Environment.NewLine + Environment.NewLine;

        emailBody += "You may view the details of the incident " + Environment.NewLine;
        emailBody += "as well as view and submit progress notes" + Environment.NewLine;
        emailBody += "here: http://" + Environment.MachineName + "/Safety/Incident/Approve.aspx?RptID=" + incRcd.UID + Environment.NewLine;

        WebMail.NetMail(addressList, eMailSubject, emailBody);
    }
    public static void AccidentIncidentClosed(SIU_Incident_Accident_Reports_To RT, SIU_Incident_Accident incRcd)
    {
        const string eMailSubject = "Accident or Incident Event Completed.";
        string addressList = String.Empty;

        List<string> empNos = new List<string>
        {
            RT.DeptMgrEmpId,
            RT.DivMgrEmpId,
            RT.GmEmpId,
            RT.LegalMgrEmpId,
            RT.SafetyMgrEmpId,
            RT.SuprEmpId,
            RT.VpEmpId
        };

        foreach (string emailAddr in SqlServer_Impl.GetEmployeeEmailByNo(empNos))
        {
            if (emailAddr.Length > 0)
            {
                if (addressList.Length == 0)
                    addressList = emailAddr;
                else
                    addressList += ", " + emailAddr;
            }
        }

        if (addressList.Length == 0) return;

        string emailBody = "Each manager approved the completion of this Accident or Incident event and the event is now closed." + Environment.NewLine + Environment.NewLine;

        emailBody += "Event Description: " + incRcd.Inc_Desc + Environment.NewLine;
        emailBody += "Involved Employee: " + SqlServer_Impl.GetEmployeeNameByNo(incRcd.Emp_ID) + Environment.NewLine + Environment.NewLine;

        WebMail.NetMail(addressList, eMailSubject, emailBody);
    }

    ////////////////////////////////////
    // Job Completed Email Generation //
    // Not In Use                     //
    ////////////////////////////////////
    public static void JobCompletionEmail(SIU_Complete_Job UpdRcd, bool ExtraBilling, string userEmail, string userFullName)
    {
        string eMailSubject = "Job: " + UpdRcd.JobNo + " Work Completed Report";

        string emailBody = "Job: " + UpdRcd.JobNo + " Work Completed Report" + Environment.NewLine + Environment.NewLine;

        emailBody += "Reporting Employee:\t" + userFullName + Environment.NewLine + Environment.NewLine;


        emailBody += "Job Scope Changed:\t" + ((ExtraBilling) ? "YES" : "NO") + Environment.NewLine + Environment.NewLine;
        if (ExtraBilling)
        {
            emailBody += "Managers:\t\t" + UpdRcd.NumMgrs + Environment.NewLine;
            emailBody += "Material Cost:\t\t" + UpdRcd.AddMaterial.ToString() + Environment.NewLine;
            emailBody += "Travel Cost:\t\t" + UpdRcd.AddTravel.ToString() + Environment.NewLine;
            emailBody += "Lodge Cost:\t\t" + UpdRcd.AddLodge.ToString() + Environment.NewLine;
            emailBody += "Other Cost:\t\t" + UpdRcd.AddOther.ToString() + Environment.NewLine;

            emailBody += "" + Environment.NewLine;
            emailBody += "Total Hours:\t\t" + UpdRcd.TotHours.ToString() + Environment.NewLine;
        }


        WebMail.NetMail("ltruitt@shermco.com", eMailSubject, emailBody);
    }

    //////////////////////
    // No Longer In Use //
    //////////////////////
    public static void SafetyQomScoreEMail(SIU_Qom_QR UpdRcd)
    {
        //SIU_Safety_MoQ question = SqlServer_Impl.GetSafetyQomQ(UpdRcd.Q_Id);

        ////////////////////////////////////
        //// Send Closed / Accepted Email //
        ////////////////////////////////////
        //const string eMailSubject = "Safety Question Of The Month Scored";

        //string emailBody = "";
        //emailBody += "Question ID: " + UpdRcd.Q_Id + Environment.NewLine;
        //emailBody += "Question Text " + question.Question + Environment.NewLine + Environment.NewLine;

        //emailBody += "Response Date: " + UpdRcd.ResponseDate + Environment.NewLine;
        //emailBody += "Response Text: " + UpdRcd.Response + Environment.NewLine + Environment.NewLine;

        //        emailBody += "Score Date: " + UpdRcd.ScoreDate + Environment.NewLine;
        //        emailBody += "Scored By: " + SqlServer_Impl.GetEmployeeNameByNo(UpdRcd.Score_Emp_ID) + Environment.NewLine;
        //        emailBody += "Score: " + UpdRcd.Score + Environment.NewLine;

        ///////////////////////////
        // And To Original  User //
        ///////////////////////////
        //Shermco_Employee emp = SqlServer_Impl.GetEmployeeByNo(UpdRcd.Emp_ID);
        //NetMail(emp.Company_E_Mail, eMailSubject, emailBody);
    }

    //////////////////////
    // No Longer In Use //
    //////////////////////
    public static void BugReportNewEmail(SIU_TaskList Report, string userEmail, string userFullName)
    {

        ///////////////////////////
        // Send New-Report Email //
        ///////////////////////////
        string eMailSubject = "New ShermcoYOU Bug / Suggestion Report Submission From: " + userFullName;

        string emailBody = Report.OpenTimeStamp + Environment.NewLine;
        emailBody += "New ShermcoYOU Bug / Suggestion From: " + userFullName + Environment.NewLine + Environment.NewLine;
        emailBody += "Remarks:" + Environment.NewLine;
        emailBody += Report.Description + Environment.NewLine + Environment.NewLine;

        if (Report.StepsToCreate.Length > 0)
        {
            emailBody += "Steps to Repoduce Problem:" + Environment.NewLine;
            emailBody += Report.StepsToCreate + Environment.NewLine + Environment.NewLine;
        }

        //////////////////////////////////
        // Send Email To Original  User //
        //////////////////////////////////
        WebMail.NetMail(userEmail, eMailSubject, emailBody);

        //////////////////////
        // Send Email To Me //
        //////////////////////
        WebMail.NetMail("ltruitt@shermco.com", eMailSubject, emailBody);
    }
    public static void BugReportSendStatusEmail(SIU_TaskList UpdRcd, char CmdBtn)
    {
        Shermco_Employee rptByEmp = SqlServer_Impl.GetEmployeeByNo(UpdRcd.EmpID);
        string newStatus = "Unknown";

        switch (CmdBtn)
        {
            case 'A':
                newStatus = "Accepted";
                break;

            case 'R':
                newStatus = "Rejected";
                break;

            case 'W':
                newStatus = "In Progress";
                break;

            case 'C':
                newStatus = "Completed / Closed";
                break;

            case 'T':
                newStatus = "Ready For User Testing";
                break;

            case 'X':
                newStatus = "User Testing Failed";
                break;
        }


        //////////////////////////////////
        // Send Closed / Accepted Email //
        //////////////////////////////////
        string eMailSubject = newStatus + " Bug / Suggestion <" + UpdRcd.IncidentNo + ">";

        string emailBody = "";
        if (rptByEmp == null)
            emailBody += "Submitted By: UNKNOWN" + Environment.NewLine;
        else
            emailBody += "Submitted By: " + rptByEmp.Last_Name + ", " + rptByEmp.First_Name + Environment.NewLine;
        emailBody += "Submitted On: " + UpdRcd.OpenTimeStamp + Environment.NewLine;
        emailBody += "Accepted On: " + UpdRcd.AcceptTimeStamp + Environment.NewLine;
        emailBody += "Started Work On: " + UpdRcd.WorkTimeStamp + Environment.NewLine;
        emailBody += "Submitted For Testing On: " + UpdRcd.TestingTimeStamp + Environment.NewLine;
        emailBody += "Work Rejected On: " + UpdRcd.TestRejectionTimeStamp + Environment.NewLine;
        emailBody += "Closed Work On: " + UpdRcd.CloseTimeStamp + Environment.NewLine + Environment.NewLine;

        emailBody += "New Status: " + newStatus + Environment.NewLine + Environment.NewLine;

        emailBody += "Description: " + Environment.NewLine + UpdRcd.Description + Environment.NewLine + Environment.NewLine;

        emailBody += "Step to Reporoduce Problem: " + Environment.NewLine + UpdRcd.StepsToCreate + Environment.NewLine + Environment.NewLine;

        emailBody += "Test Rejection Notes: " + Environment.NewLine + UpdRcd.TestRejectionNotes + Environment.NewLine + Environment.NewLine;


        /////////////////////////////
        // Send Email To Developer //
        /////////////////////////////
        WebMail.NetMail("ltruitt@shermco.com", eMailSubject, emailBody);

        ////////////////////////////////
        // Send Email To Safety Group //
        ////////////////////////////////
        if (CmdBtn == 'T')
        {
            emailBody += "Accept or Reject Work HERE" + Environment.NewLine + Environment.NewLine;
            emailBody += @"http://" + HttpContext.Current.Server.MachineName + @"/Forms/BugReport.aspx" + "?RptID=" + UpdRcd.IncidentNo;
            emailBody += Environment.NewLine;
        }



        ///////////////////////////
        // And To Original  User //
        ///////////////////////////
        if (rptByEmp != null)
            WebMail.NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);
    }

    //////////////////////
    // No Longer In Use //
    //////////////////////
    public static void HardwareRequstSendNewEmail(SIU_IT_HW_Req Req, string EmailAddr)
    {
        string empNameEquipFor = SqlServer_Impl.GetEmployeeNameByNo(Req.For_EmpID);
        string empNameReq = SqlServer_Impl.GetEmployeeNameByNo(Req.Req_EmpID);
        string empNameReqSupr = SqlServer_Impl.GetEmployeeNameByNo(Req.Req_EmpID);

        string eMailSubject = "Equipment Request For Employee " + empNameEquipFor;


        string emailBody = "Equipment Request For Employee " + empNameEquipFor + Environment.NewLine + Environment.NewLine;

        emailBody += "Requestor:\t\t" + empNameReq + Environment.NewLine;
        emailBody += "Supervisor:\t\t" + empNameReqSupr + Environment.NewLine;

        emailBody += "User Role Is:\t\t" + Req.Role + Environment.NewLine + Environment.NewLine;

        emailBody += "Selected Computer: \t" + Req.Hardware + Environment.NewLine;
        emailBody += "Total Estimated Price: " + Req.Price + Environment.NewLine + Environment.NewLine;

        emailBody += "Requested Options:" + Environment.NewLine;
        emailBody += "-----------------------------------------------------------------------------------" + Environment.NewLine;
        emailBody += "Monitors:       " + Req.MonitorCount + "                     Monitor Stands: " + Req.MonitorStandCount + Environment.NewLine + Environment.NewLine;

        if (Req.ComputerCase) emailBody += "   Case" + Environment.NewLine;
        if (Req.ComputerDock) emailBody += "   Dock" + Environment.NewLine;
        if (Req.BackPack) emailBody += "   BackPack" + Environment.NewLine + Environment.NewLine;

        if (Req.Adobe) emailBody += "   Adobe" + Environment.NewLine;
        if (Req.CAD) emailBody += "   CAD" + Environment.NewLine;
        if (Req.MsPrj) emailBody += "   MsPrj" + Environment.NewLine;
        if (Req.Visio) emailBody += "   Visio" + Environment.NewLine;

        WebMail.NetMail("ITDEPARTMENT@shermco.com", eMailSubject, emailBody);
        WebMail.NetMail(EmailAddr, eMailSubject, emailBody);
    }
    public static void HardwareRequstSendCompleteEmail(string EmailAddr, SIU_IT_HW_Req Req)
    {
        string forEmpName = SqlServer_Impl.GetEmployeeNameByNo(Req.For_EmpID);
        string byEmpName = SqlServer_Impl.GetEmployeeNameByNo(Req.Req_EmpID);


        string eMailSubject = "Completed Equipment Request For Employee " + forEmpName;

        string emailBody = "Completed Equipment Request For Employee " + forEmpName + Environment.NewLine + Environment.NewLine;

        emailBody += "Requestor:\t\t" + byEmpName + Environment.NewLine;

        emailBody += "User Role Is:\t\t" + Req.Role + Environment.NewLine + Environment.NewLine;

        emailBody += "Selected Computer: \t" + Req.ComputerDesc + Environment.NewLine;
        emailBody += "Total Estimated Price: " + Req.Price + Environment.NewLine + Environment.NewLine;

        emailBody += "Requested Options:" + Environment.NewLine;
        emailBody += "-----------------------------------------------------------------------------------" + Environment.NewLine;
        emailBody += "Monitors:       " + Req.MonitorCount + "                     Monitor Stands: " + Req.MonitorStandCount + Environment.NewLine + Environment.NewLine;

        if (Req.ComputerCase) emailBody += "   Case" + Environment.NewLine;
        if (Req.ComputerDock) emailBody += "   Dock" + Environment.NewLine;
        if (Req.BackPack) emailBody += "   BackPack" + Environment.NewLine + Environment.NewLine;

        if (Req.Adobe) emailBody += "   Adobe" + Environment.NewLine;
        if (Req.CAD) emailBody += "   CAD" + Environment.NewLine;
        if (Req.MsPrj) emailBody += "   MsPrj" + Environment.NewLine;
        if (Req.Visio) emailBody += "   Visio" + Environment.NewLine;

        WebMail.NetMail(EmailAddr, eMailSubject, emailBody);
    }

    private static string ToShortDate(DateTime dt)
    {
// ReSharper disable once ConditionIsAlwaysTrueOrFalse
        return dt != null ? dt.ToShortDateString() : "";
    }
    private static string ToShortDate(DateTime? dt)
    {
        return (dt != null) ? ((DateTime)dt).ToShortDateString() : "";
    }
}