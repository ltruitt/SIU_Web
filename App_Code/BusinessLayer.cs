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
        price += (priceMon * decimal.Parse(MonitorCnt));

        /////////////////////////////////
        // Add Cost For Monitor Stands //
        /////////////////////////////////
        decimal priceMonStd = (from mon in addOns where mon.Description == "Monitor Adj Stand" select mon.Price).SingleOrDefault();
        price += (priceMonStd * decimal.Parse(StandCnt));

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
        SIU_Incident_Accident incRcd = (SIU_Incident_Accident)SqlServer_Impl.GetIncidentAccident(_UID);
        SIU_Incident_Accident_Reports_To RC =  IncidentAccidentReportsToByEmp(incRcd.Emp_ID);

        ////////////////////////////////////
        // Now Add Already Approved Dates //
        ////////////////////////////////////
        List<SIU_Incident_Accident_Appoval> appRcds = SqlServer_Impl.GetIncidentAccidentApprovals(_UID);

        RC.DeptMgrDate = (from rcd in appRcds where rcd.EID == RC.DeptMgrEmpId select rcd.TimeStamp).SingleOrDefault();
        RC.DivMgrDate = (from rcd in appRcds where rcd.EID == RC.DivMgrEmpId select rcd.TimeStamp).SingleOrDefault();
        RC.GmDate  = (from rcd in appRcds where rcd.EID == RC.GmEmpId select rcd.TimeStamp).SingleOrDefault();
        RC.LegalMgrDate  = (from rcd in appRcds where rcd.EID == RC.LegalMgrEmpId select rcd.TimeStamp).SingleOrDefault();
        RC.SafetyMgrDate  = (from rcd in appRcds where rcd.EID == RC.SafetyMgrEmpId select rcd.TimeStamp).SingleOrDefault();
        RC.SuprDate = (from rcd in appRcds where rcd.EID == RC.SuprEmpId select rcd.TimeStamp).SingleOrDefault();
        RC.VpDate = (from rcd in appRcds where rcd.EID == RC.VpEmpId select rcd.TimeStamp).SingleOrDefault();

        return RC;
    }
    private static SIU_Incident_Accident_Reports_To IncidentAccidentReportsToByEmp(string EmpID)
    {
        //////////////////////////////////
        // Create A New Response Record //
        //////////////////////////////////
        SIU_Incident_Accident_Reports_To R2 = new SIU_Incident_Accident_Reports_To();

        /////////////////////////////////////////////
        // Load The Emp For Whom We Are Looking Up //
        /////////////////////////////////////////////
        R2.EmpId = EmpID;
        Shermco_Employee emp = SqlServer_Impl.GetEmployeeByNo(EmpID);
        if (emp == null)
            return R2;
        R2.Dept = emp.Global_Dimension_1_Code;


        ////////////////////////////////////////////////////////////////////
        // And Then The Reporting Chain Based On The Employees Department //
        ////////////////////////////////////////////////////////////////////
        SIU_ReportingChain RC = SqlServer_Impl.GetEmployeeReportingChain(R2.Dept);
        if (RC == null)
            return R2;




        /////////////////////////////////
        // Load Supervisor Chain While //
        // Also Eliminating Duplicates //
        /////////////////////////////////
        List<string> EIDs = new List<string>();

        if (!EIDs.Contains(RC.GmEmpId))
        {
            R2.GmEmpId = RC.GmEmpId;
            R2.GmName = RC.GmName;
            EIDs.Add(RC.GmEmpId);
        }

        if (!EIDs.Contains(RC.VpEmpId))
        {
            R2.VpEmpId = RC.VpEmpId;
            R2.VpName = RC.VpName;
            EIDs.Add(RC.VpEmpId);
        }

        if (!EIDs.Contains(RC.SafetyMgrEmpId))
        {
            R2.SafetyMgrEmpId = RC.SafetyMgrEmpId;
            R2.SafetyMgrName = RC.SafetyMgrName;
            EIDs.Add(RC.SafetyMgrEmpId);
        }

        if (!EIDs.Contains(RC.LegalMgrEmpId))
        {
            R2.LegalMgrEmpId = RC.LegalMgrEmpId;
            R2.LegalMgrName = RC.LegalMgrName;
            EIDs.Add(RC.LegalMgrEmpId);
        }

        if (!EIDs.Contains(RC.DivMgrEmpId))
        {
            R2.DivMgrEmpId = RC.DivMgrEmpId;
            R2.DivMgrName = RC.DivMgrName;
            EIDs.Add(RC.DivMgrEmpId);
        }


        if (!EIDs.Contains(RC.DeptMgrEmpId))
        {
            R2.DeptMgrEmpId = RC.DeptMgrEmpId;
            R2.DeptMgrName = RC.DeptMgrName;
            EIDs.Add(RC.DeptMgrEmpId);
        }

        if (!EIDs.Contains(emp.Manager_No_))
        {
            R2.SuprEmpId = emp.Manager_No_;

            /////////////////////////////////////
            // Look Up The Employee Supervisor //
            /////////////////////////////////////
            emp = SqlServer_Impl.GetEmployeeByNo(R2.SuprEmpId);
            if (emp != null)
                R2.SuprName = emp.First_Name + " " + emp.Last_Name;
        }

        return R2;
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

        foreach (SIU_DOT_Inspection Rpt in uncorrected)
        {
            string emp = (from thisEmp in emps where thisEmp.No_ == Rpt.SubmitEmpID.TrimEnd() select thisEmp.Last_Name + ", " + thisEmp.First_Name).SingleOrDefault();

            emailBody += "<tr style='color: black; text-align: center;'>";
            emailBody += "<td><a href=" + webServer + "/ELO/VehDotEntry.aspx?rpt=" + Rpt.RefID + ">" + Rpt.RefID + "</a></td>";
            emailBody += "<td>" + Rpt.Vehicle + "</td>";
            emailBody += "<td>(" + Rpt.SubmitEmpID.TrimEnd() + ") " + emp + "</td>";
            emailBody += "<td>" + Rpt.SubmitTimeStamp.ToShortDateString() + "</td>";
            emailBody += "<td style='color: black; text-align: left;'>" + Rpt.Hazard + "</td>";
            emailBody += "</tr>";
        }

        emailBody += "</table>";

        return emailBody;
    }

    public static void GenQtmNotices()
    {
        string webServer = "http://" + HttpContext.Current.Request.Url.DnsSafeHost; ;
        List<SIU_Qom_QR> qtmList = SqlServer_Impl.GetSafetyQomQRList("0");

        DateTimeFormatInfo mfi = new DateTimeFormatInfo();
        string strMonthName = mfi.GetMonthName(DateTime.Now.Month);


        foreach (SIU_Qom_QR qtm in qtmList)
        {
            string emailBody;
            emailBody = "<h1>The " + strMonthName + " " + qtm.Q_Grp + " Question of the month is:</h1>";

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

            WebMail.HtmlMail("ltruitt@shermco.com; kkirkpatrick@shermco.com", strMonthName + " " + qtm.Q_Grp + " Question of the month", emailBody);
            //WebMail.HtmlMail("allemployees@shermco.com", strMonthName + " " + qtm.Q_Grp + " Question of the month", emailBody);
        }
    }
    public static void GenQtmReminders()
    {
        try
        {
            string webServer = "http://" + HttpContext.Current.Request.Url.DnsSafeHost; ;
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

                WebMail.HtmlMail("ltruitt@shermco.com; kkirkpatrick@shermco.com", strMonthName + " " + qtm.Q_Grp + " Question of the month", emailBody);
                //WebMail.HtmlMail("allemployees@shermco.com", strMonthName + " " + qtm.Q_Grp + " Question of the month", emailBody);

            }
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("GenQtmReminders", ex.Message);
        }
    }
}