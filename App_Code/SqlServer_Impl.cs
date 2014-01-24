using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Transactions;
using System.Web;
using System.Globalization;
using System.Web.Configuration;
using System.Web.Script.Services;
using DocumentFormat.OpenXml.Bibliography;
using ShermcoYou.DataTypes;
using AutoMapper;
using System.Web.Services;
using System.Web.Script.Serialization;
using Calendar = System.Globalization.Calendar;
using IsolationLevel = System.Transactions.IsolationLevel;

using Microsoft.Synchronization;
using Microsoft.Synchronization.Files;
using ChangeType = Microsoft.Synchronization.Files.ChangeType;

public class DeveloperConfigurationSection : ConfigurationSection
{
    public const String ConfigurationSectionName = "DeveloperSettings";

    [ConfigurationProperty("enabled", DefaultValue = true)]
    public bool Enabled
    {
        get
        {
            return (bool)base["enabled"];
        }
        set
        {
            base["enabled"] = value;
        }
    }

    [ConfigurationProperty("useConnectString", DefaultValue = "ShermcoConnectionString")]
    public string UseConnectString
    {
        get
        {
            return (string)base["useConnectString"];
        }
        set
        {
            base["useConnectString"] = value;
        }
    }
}






[WebService(Namespace = "http://localhost/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class SiuDao : WebService
{
#region Certification and Classes 
    /////////////////////////////////////////////////////////////////////////////////
    // Build List Of All Possible Certification Codes (Classes) For AutoCompletion //
    /////////////////////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetCerts()
    {
        string certList = string.Empty;

        foreach (var cert in SqlServer_Impl.GetBandC_QualCodes())
            certList += cert.Code + " " + cert.Description + "\r";

        return certList;
    }

    ///////////////////////////////////////////////////////////
    // Return A Specific Certification Record (Non Instance) //
    ///////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetCert(string Cert)
    {
        ////////////////////////////////
        // Build Response Data Object //
        ////////////////////////////////
        List<Shermco_Qualification> jasonResp = SqlServer_Impl.GetBandC_QualCode(Cert).ToList();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(jasonResp);
    }

    /////////////////////////////////////////////////////////////////////////////
    // Return List Of Unposted Students Who COmpleted Any Class For Instructor //
    /////////////////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetUnpostedClass(string EmpID, string ClassCode, string ClassDate)
    {
        ////////////////////////////////
        // Build Response Data Object //
        ////////////////////////////////
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            List<SIU_Class_Completion> jasonResp = SqlServer_Impl.GetUnpostedClass(EmpID, ClassCode, ClassDate);
            return serializer.Serialize(new { Result = "OK", Records = jasonResp });
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetUnpostedClass", EmpID + ", " + ClassCode + ", " + ClassDate);
            SqlServer_Impl.LogDebug("SiuDao.GetUnpostedClass", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }
    }

    ///////////////////////////////////////////////////////////////////
    // Record a Student Completed A Class By An Instructor On A Date //
    ///////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string RecordUnPostedClassStudent(string QualCode, string ClassDate, string InstID, string StudentID)
    {
        try
        {
            SIU_Class_Completion newSTudentClass = new SIU_Class_Completion
            {
                emp_no = StudentID,
                qual_code = QualCode,
                class_date = DateTime.Parse(ClassDate),
                class_instructor = InstID
            };
            SIU_Class_Completion jasonResp = SqlServer_Impl.RecordUnPostedClassStudent(newSTudentClass);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(new { Result = "OK", Records = jasonResp });
        }
        catch (Exception e)
        {
            return  e.Message;
        }
    }

    ///////////////////////////////////////////////////////////////
    // Remove An Unposted Record Of A Student Completing A Class //
    ///////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string DeleteUnPostedClassStudent(string UID)
    {
        string excepMsg = SqlServer_Impl.RemoveUnPostedClassStudent(UID);
        string cc = ( excepMsg.Length > 0 ) ? "OK" : "ERROR";

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { Result = cc, Message = excepMsg});
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    // The Teacher Has Verified A Student Completed A Class.  Move Record To Emp Qual Tbl //
    ////////////////////////////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string CommitUnPostedClassStudent(string UID)
    {
        SqlServer_Impl.CommitUnPostedClassStudent(UID);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { Result = "OK" });
    }


#endregion Certification and Classes

#region ELO Time And Time Reporting
    ///////////////////////////
    // Get Jobs List         //
    // Get List Of Open Jobs //
    ///////////////////////////
    [WebMethod(EnableSession = true)]
    public string Affe31()
    {
        string jobList = string.Empty;

        foreach (var job in SqlServer_Impl.GetActiveJobs())
            jobList += job.JobNoDesc + "\r";

        return jobList;
    }

    ///////////////////////////
    // Get Job               //
    // Lookup A Specific Job //
    ///////////////////////////
    [WebMethod(EnableSession = true)]
    public string Gffeop1(string jobNo)
    {
        ////////////////////////////////
        // Build Response Data Object //
        ////////////////////////////////
        SIU_TimeSheet_Job jasonResp = new SIU_TimeSheet_Job(SqlServer_Impl.GetJobByNo(jobNo));
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        if (jasonResp.JobNo != null)
            return serializer.Serialize(jasonResp);
        return null;
    }

    ///////////////////////////////////////////////////////////
    // Lookup OverHead Accounts Available For Time Reporting //
    // Support For Time Entry Form                           //
    ///////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetTimeOHAccts()
    {
        string ohList = string.Empty;

        foreach (var oh in SqlServer_Impl.GetTimeOhAccounts())
        {
            if (ohList.Length > 0) ohList += "\r";
            ohList += oh.AccountDesc;
        }

        return ohList;
    }

    //////////////////////////////////////////////////////////////
    // Lookup OverHead Accounts Available For Expense Reporting //
    // Support For Expense Entry Form                              //
    //////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string bbb66655()
    {
        string ohList = string.Empty;

        ///////////////////////////
        // Check For Admin Priv. //
        ///////////////////////////
        StringCollection sessionVar = (StringCollection)Session["UserGroups"];

        //////////////////////////////////////
        // Add Approved GL Expense Accounts //
        // Requires Photo Proof             //
        //////////////////////////////////////
        if (sessionVar != null && sessionVar.Contains("ELO_EXP_TEST"))
        {
            foreach (var oh in SqlServer_Impl.GetExpenseOHAccts2())
            {
                if (ohList.Length > 0) ohList += "\r";
                ohList += oh.AccountDesc; // +"*";
            }
        }
        else
        {
            //////////////////////////////////////
            // Look Up Payroll Expense Accounts //
            //////////////////////////////////////
            foreach (var oh in SqlServer_Impl.GetExpenseOHAccts())
            {
                if (ohList.Length > 0) ohList += "\r";
                ohList += oh.AccountDesc;
            }            
        }

        return ohList;
    }

    ////////////////////////////////////////////////////////
    // Get OverHead Account Department For Time Reporting //
    // Support For Time Entry Form                        //
    ////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetTimeDepts()
    {
        string deptList = string.Empty;

        foreach (var dept in SqlServer_Impl.GetTimeDivDept())
        {
            if (dept.Code != "6020")
            {
                if (deptList.Length > 0) deptList += "\r";
                deptList += dept.Code_Name;
            }
        }

        return deptList;
    }

    ///////////////////////////////////////////////////////////////////
    // Get List Of Tasks Available For A Specific Job Time Reporting //
    // Support For Time Entry Form                                   //
    ///////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetTimeTasks(string JobNo)
    {
        string taskList = string.Empty;

        foreach (var task in SqlServer_Impl.GetTimeTaskCodes(JobNo))
        {
            if (taskList.Length > 0) taskList += "\r";
            taskList += task.StepnoDescription;
        }

        return taskList;
    }

    ///////////////////////////////////////////////
    // Lookup Hours Reported For A Specific Date //
    // Support For Time Entry Form               //
    ///////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetTimeTotHoursByDay(string EmpID, string StartDate, string EndDate, string T)
    {
        try
        {
            List<SIU_TimeSheet_DailyHourSum> hoursByDay = SqlServer_Impl.GetTimeSheet_PeriodDailySums(EmpID, DateTime.Parse(StartDate), DateTime.Parse(EndDate));
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string rtn = serializer.Serialize(hoursByDay);
            return rtn;    
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetTimeTotHoursByDay", EmpID + ", " + StartDate + ", " + EndDate);
            SqlServer_Impl.LogDebug("SiuDao.GetTimeTotHoursByDay", ex.Message);
            return "";
        }
    
    }

    ///////////////////////////////////////////////////
    // Supply List Of Employees For AutoComplete Box //
    ///////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetAutoCompleteActiveEmployees(string T)
    {
        string empList = string.Empty;

        foreach (var emp in SqlServer_Impl.GetAutoCompleteActiveEmployees())
            empList += emp.EmpidName + "\r";

        return empList;    
    }

    /////////////////////////////////////////////////////////////////
    // Supply List Of Safety Pays Point Types For AutoComplete Box //
    /////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetAutoCompletePointTypes(string T)
    {
        string typeList = string.Empty;

        foreach (var pt in SqlServer_Impl.GetAutoCompletePointTypes())
            typeList += pt.UID + " - "  + pt.Description + "," + pt.PointsCount + "\r";

        return typeList;
    }

    /////////////////////////////////////
    // Manually Add Safety Pays Points //
    /////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string Xxx5554S(string EmpID, string ReasonCode, string Points, string Comment, string DateOfEvent, string UID)
    {
        SIU_SafetyPays_Point pt = new SIU_SafetyPays_Point
        {
            Emp_No = EmpID,
            ReasonForPoints = int.Parse(ReasonCode),
            DatePointsGiven = DateTime.Now,
            PointsGivenBy = BusinessLayer.UserEmpID,
            Points = int.Parse(Points),
            Comments = Comment,
            EventDate = DateTime.Parse( DateOfEvent ),
            SPR_UID = 0
        };

        if (UID.Length > 0)
            pt.UID =  int.Parse(UID);
            
        SqlServer_Impl.RecordAdminPoints(pt);
    
        return "Success";
    }

    ///////////////////////////////////////
    // Lookup Safety Pays Points For Emp //
    ///////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string k00PY34(string EmpID, string jtSorting)
    {
        List<SIU_SafetyPays_Point> rpt = SqlServer_Impl.GetEmpIdPoints(EmpID).OrderBy(jtSorting).ToList();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { Result = "OK", Records = rpt});
    }

    ///////////////////////////////////////
    // Lookup Safety Pays Points For Emp //
    ///////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GhjjI0E(string UID)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            SqlServer_Impl.RemoveEmpIdPoints(UID);
            return serializer.Serialize(new { Result = "OK" });
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.RemoveEmpIdPoints", UID);
            SqlServer_Impl.LogDebug("SiuDao.RemoveEmpIdPoints", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }
    }

    //////////////////////////////////////////
    // Provide Data For YTD Expenses Report //
    //////////////////////////////////////////
    //[WebMethod(EnableSession = true)]
    //public List<SIU_Points_Rpt> GetAdminPointsRpt(string startDate, string endDate, bool _search, long nd, int rows, int page, string sidx, string sord)
    //{
    //    sord = ((sidx.Length == 0) ? "EventDate" : sidx) + " " + sord.ToUpper();
    //    return SqlServer_Impl.GetAdminPointsRpt(startDate, endDate).OrderBy(sord).ToList();
    //}

    [WebMethod(EnableSession = true)]
    public JqGridData GetAdminPointsRptEmpPoints(DateTime startDate, DateTime endDate, string empNo,  bool _search, long nd, int rows, int page, string sidx, string sord)
    {
        List<SIU_Points_Rpt> data = SqlServer_Impl.GetAdminPointsRptEmpPoints(startDate, endDate, empNo).ToList();
        return new JqGridData
        {
            total = 10,
            page = 1,
            records = 10,
            rows = data
        };
    }

    [WebMethod(EnableSession = true)]
    public JqGridData GetAdminPointsRptDepts(string startDate, string endDate, bool _search, long nd, int rows, int page, string sidx, string sord)
    {
        List<SIU_Points_Rpt> data = SqlServer_Impl.GetAdminPointsRptDepts(startDate, endDate).ToList();
        return new JqGridData
        {
            total = 10,
            page = 1,
            records = 10,
            rows = data
        };
    }

    [WebMethod(EnableSession = true)]
    public JqGridData GetAdminPointsRptEmps(string startDate, string endDate, string dept, bool _search, long nd, int rows, int page, string sidx, string sord)
    {
        List<SIU_Points_Rpt> data = SqlServer_Impl.GetAdminPointsRptEmps(startDate, endDate, dept).ToList();
        return new JqGridData
        {
            total = 10,
            page = 1,
            records = 10,
            rows = data
        };
    }

    ////////////////////////////////////////////////////
    // Lookup AVailable Paid Off Time For An Employee //
    // Support For Time Entry Form                    //
    ////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetTimeOpenBalance(string EmpID)
    {
        SIU_ELO_GetBalanceResult bal = SqlServer_Impl.GetTimeOpenBalance(EmpID);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(bal);  
    }

    /////////////////////////////////////
    // Supply Data For TimeDelete Form //
    // Not Longer In Use               //
    /////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetTimeUnapproved(string EmpID, string Date)
    {
        List<Shermco_Time_Sheet_Entry> te = SqlServer_Impl.GetTimeUnapproved(EmpID, Date);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(te);  
    }

    ///////////////////////////////
    // Get Data For Hours Report //
    ///////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetHoursRpt(string EmpID, string Date)
    {
        List<SIU_TimeSheet_HoursRpt> rpt = SqlServer_Impl.GetHoursRpt(EmpID, Date);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { Result = "OK", Records = rpt });  
    }

    [WebMethod(EnableSession = true)]
    public string rrrrrT(string EmpID, string jtSorting)
    {
        string[] sorting = jtSorting.Split(' ');

        if (sorting[0] == "workDate")
        {
            jtSorting += ",JobNo " + sorting[1];
            jtSorting += ",OhAcct " + sorting[1];
        }
        else
        {
            jtSorting += ",OhAcct " + sorting[1];
            jtSorting += ",workDate " + sorting[1];
        }


        List<SIU_TimeSheet_HoursRpt> rpt = SqlServer_Impl.GetAllHoursRptLast60(EmpID).OrderBy(jtSorting).ToList();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { Result = "OK", Records = rpt});
    }

    //////////////////////////////////////////////////
    // MyTimeStudy Data For Bar Graph Of This Month //
    //////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetEloRptPeriodHours(string EmpID, string SD)
    {
        DateTime startDate = DateTime.Parse(SD).Date;

        List<SIU_TimeSheet_HoursRpt> rpt = SqlServer_Impl.GetEloRptPeriodHours(EmpID, startDate).OrderBy("workDate").ToList();
        SIU_TimeSheet_HoursRpt_Mo dailyHoursRpt = new SIU_TimeSheet_HoursRpt_Mo(21);

        DateTime fillDate = DateTime.Parse(SD);
        for (int idx = 1; idx <= 21; idx++)
        {
            dailyHoursRpt.xlabel[idx] = fillDate.Day.ToString(CultureInfo.InvariantCulture);
            fillDate = fillDate.AddDays(1);
        }

        if (rpt.Count > 0)
        {
            int rptIdx = 0;
            foreach (var rptEntry in rpt)
            {
                int workDay = DateTime.Parse(rptEntry.workDate).Day;

                rptIdx = 0;
                while (rptIdx < dailyHoursRpt.xlabel.Length &&    dailyHoursRpt.xlabel[rptIdx] != workDay.ToString(CultureInfo.InvariantCulture)) rptIdx++;

                if (rptIdx < dailyHoursRpt.xlabel.Length)
                {
                    dailyHoursRpt.SumHours[rptIdx] += rptEntry.Total;

                    dailyHoursRpt.SumJobHours[1] += rptEntry.Total;
                    dailyHoursRpt.SumJobHours[2] += (rptEntry.JobNo.Length > 0) ? rptEntry.Total : 0;

                    dailyHoursRpt.StHours[rptIdx] += rptEntry.ST;
                    dailyHoursRpt.OtHours[rptIdx] += rptEntry.OT;
                    dailyHoursRpt.DtHours[rptIdx] += rptEntry.DT;
                    dailyHoursRpt.AbHours[rptIdx] += rptEntry.AB;
                    dailyHoursRpt.HtHours[rptIdx] += rptEntry.HT;
                }
            }
        }

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(dailyHoursRpt);
    }

    //////////////////////////////////////////////////
    // MyTimeStudy Data For Bar Graph Of This Month //
    //////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetAllHoursRptThisMo(string EmpID)
    {
        DateTime rptOnMonth = DateTime.Now;

        List<SIU_TimeSheet_HoursRpt> rpt = SqlServer_Impl.GetAllHoursRptForMonth(EmpID, rptOnMonth).OrderBy("workDate").ToList();
        SIU_TimeSheet_HoursRpt_Mo dailyHoursRpt = new SIU_TimeSheet_HoursRpt_Mo(rptOnMonth);

        foreach (var rptEntry in rpt)
        {
            int workDay = DateTime.Parse(rptEntry.workDate).Day;
            dailyHoursRpt.SumHours[workDay] += rptEntry.Total;

            dailyHoursRpt.SumJobHours[1] += rptEntry.Total;
            dailyHoursRpt.SumJobHours[2] += (rptEntry.JobNo.Length > 0) ? rptEntry.Total : 0;

            dailyHoursRpt.StHours[workDay] += rptEntry.ST;
            dailyHoursRpt.OtHours[workDay] += rptEntry.OT;
            dailyHoursRpt.DtHours[workDay] += rptEntry.DT;
            dailyHoursRpt.AbHours[workDay] += rptEntry.AB;
            dailyHoursRpt.HtHours[workDay] += rptEntry.HT;

        }

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(dailyHoursRpt);
    }

    //////////////////////////////////////////////////
    // MyTimeStudy Data For Bar Graph Of Last Month //
    //////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetAllHoursRptPrevMo(string EmpID)
    {
        DateTime rptOnMonth = DateTime.Now.AddMonths(-1);

        List<SIU_TimeSheet_HoursRpt> rpt = SqlServer_Impl.GetAllHoursRptForMonth(EmpID, rptOnMonth).OrderBy("workDate").ToList();
        SIU_TimeSheet_HoursRpt_Mo dailyHoursRpt = new SIU_TimeSheet_HoursRpt_Mo(rptOnMonth);

        foreach (var rptEntry in rpt)
        {
            int workDay = DateTime.Parse(rptEntry.workDate).Day;
            dailyHoursRpt.SumHours[workDay] += rptEntry.Total;

            dailyHoursRpt.SumJobHours[1] += rptEntry.Total;
            dailyHoursRpt.SumJobHours[2] += (rptEntry.JobNo.Length > 0) ? rptEntry.Total : 0;

            dailyHoursRpt.StHours[workDay] += rptEntry.ST;
            dailyHoursRpt.OtHours[workDay] += rptEntry.OT;
            dailyHoursRpt.DtHours[workDay] += rptEntry.DT;
            dailyHoursRpt.AbHours[workDay] += rptEntry.AB;
            dailyHoursRpt.HtHours[workDay] += rptEntry.HT;
        }

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(dailyHoursRpt);
    }

    ///////////////////////////////////////////////////
    // MyTimeStudy Data For Bar Graph Of YTD By Week //
    ///////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetWeeklyHoursThisYear(string EmpID)
    {
        ///////////////////////////////////////////////////////////////
        // Gets the Calendar instance associated with a CultureInfo. //
        ///////////////////////////////////////////////////////////////
        CultureInfo ci = new CultureInfo("en-US");
        Calendar cal = ci.Calendar;

        ////////////////////////////////////////////////////////
        // Gets the DTFI properties required by GetWeekOfYear.//
        ////////////////////////////////////////////////////////
        CalendarWeekRule cwr = ci.DateTimeFormat.CalendarWeekRule;
        const DayOfWeek firstDow = DayOfWeek.Monday;
        

        List<SIU_TimeSheet_HoursRpt> rpt = SqlServer_Impl.GetWeeklyHoursThisYear(EmpID).OrderBy("workDate").ToList();
        SIU_TimeSheet_HoursRpt_Mo dailyHoursRpt = new SIU_TimeSheet_HoursRpt_Mo(cal.GetWeekOfYear(DateTime.Now, cwr, firstDow));

        foreach (var rptEntry in rpt)
        {
            ////////////////////////////////////////////////
            // Figure Out Which Week This Time Belongs To //
            ////////////////////////////////////////////////
            int workWeek =  cal.GetWeekOfYear(DateTime.Parse(rptEntry.workDate), cwr, firstDow);

//////////////////////////////
// BUG When Hours In Future //
//////////////////////////////
            if (workWeek < dailyHoursRpt.SumJobHours.Length)
            {
                dailyHoursRpt.SumJobHours[workWeek] += (rptEntry.JobNo.Length > 0) ? rptEntry.Total : 0;
                dailyHoursRpt.SumHours[workWeek] += rptEntry.Total;
                dailyHoursRpt.StHours[workWeek] += rptEntry.ST;
                dailyHoursRpt.OtHours[workWeek] += rptEntry.OT;
                dailyHoursRpt.DtHours[workWeek] += rptEntry.DT;
                dailyHoursRpt.AbHours[workWeek] += rptEntry.AB;
            }
            if (workWeek < dailyHoursRpt.SumJobHours.Length)
                dailyHoursRpt.HtHours[workWeek] += rptEntry.HT;
        }

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(dailyHoursRpt);
    }

    ///////////////////////////////////////////////////////
    // MyTimeStudy Data For Bar Graph Of Job vs OH Hours //
    // Postive Bars For REvenue, Negative For Cost       //
    ///////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetJobOhHoursForMonth(string EmpID)
    {
        List<SIU_TimeSheet_HoursRpt> rpt = SqlServer_Impl.GetAllHoursRptForMonth(EmpID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).ToList();

        int cnt = rpt.Where(jobCnt => jobCnt.JobNo.Length > 0).Select(jobCnt => jobCnt.JobNo).Distinct().Count();
        cnt += rpt.Where(jobCnt => jobCnt.OhAcct.Length > 0).Select(jobCnt => jobCnt.OhAcct).Distinct().Count();
        cnt++;

        SIU_TimeSheet_HoursRpt_JobOh dailyHoursRpt = new SIU_TimeSheet_HoursRpt_JobOh(cnt);

        int rptIdx = 0;
        foreach (var rptEntry in rpt.Where(entry => entry.JobNo.Length > 0).OrderBy("JobNo"))
        {
            if (rptEntry.JobNo != dailyHoursRpt.JobNo[rptIdx])
                rptIdx++;

            dailyHoursRpt.SumHours[rptIdx] += rptEntry.Total;
            dailyHoursRpt.StHours[rptIdx] += rptEntry.ST;
            dailyHoursRpt.OtHours[rptIdx] += rptEntry.OT;
            dailyHoursRpt.DtHours[rptIdx] += rptEntry.DT;
            dailyHoursRpt.AbHours[rptIdx] += rptEntry.AB;
            dailyHoursRpt.HtHours[rptIdx] += rptEntry.HT;
            dailyHoursRpt.JobNo[rptIdx] = rptEntry.JobNo;
            dailyHoursRpt.AcctNo[rptIdx] = rptEntry.OhAcct;
        }

        foreach (var rptEntry in rpt.Where(entry => entry.OhAcct.Length > 0).OrderBy("OhAcct"))
        {
            if (rptEntry.OhAcct != dailyHoursRpt.AcctNo[rptIdx])
                rptIdx++;

            dailyHoursRpt.SumHours[rptIdx] += rptEntry.Total;
            dailyHoursRpt.StHours[rptIdx] += rptEntry.ST;
            dailyHoursRpt.OtHours[rptIdx] += rptEntry.OT;
            dailyHoursRpt.DtHours[rptIdx] += rptEntry.DT;
            dailyHoursRpt.AbHours[rptIdx] += rptEntry.AB;
            dailyHoursRpt.HtHours[rptIdx] += rptEntry.HT;
            dailyHoursRpt.JobNo[rptIdx] = rptEntry.JobNo;
            dailyHoursRpt.AcctNo[rptIdx] = rptEntry.OhAcct;
        }


        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(dailyHoursRpt);
    }

    //////////////////////////////////////////
    // Supply Data For Text Report Of Hours //
    //////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public List<SIU_TimeSheet_HoursRpt> GetAllHoursRptByDate(string EmpID, bool _search, long nd, int rows, int page, string sidx, string sord)
    {
        sord = ((sidx.Length == 0) ? "workDate" : sidx) + " " + sord.ToUpper();
        return SqlServer_Impl.GetAllHoursRptLast60(EmpID).OrderBy(sord).ToList();
    }

    ///////////////////////////////////////////////////////////
    // Build An Array Of Total Hours For Last/This/Next Week //
    // Support For Time Entry Form                           //
    ///////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public List<Decimal> GetTimeSheet_Sum(string EmpID, string StartDate)
    {
        DateTime sd = DateTime.Parse(StartDate);

        List<Decimal> weeklySums = new List<Decimal>
        {
            SqlServer_Impl.GetTimeSheet_Sum(EmpID, sd, sd.AddDays(+6)),
            SqlServer_Impl.GetTimeSheet_Sum(EmpID, sd.AddDays(7), sd.AddDays(13)),
            SqlServer_Impl.GetTimeSheet_Sum(EmpID, sd.AddDays(14), sd.AddDays(20))
        };


        return weeklySums;
    }

    //////////////////////////
    // Remove A Time Record //
    //////////////////////////
    [WebMethod(EnableSession = true)]
    public string hfghfg(string EntryNo)
    {
        int recordId = int.Parse(EntryNo);

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            SqlServer_Impl.DeleteTimeRecord(recordId);
            return serializer.Serialize(new { Result = "OK" });
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.DeleteTimeRecord", EntryNo);
            SqlServer_Impl.LogDebug("SiuDao.DeleteTimeRecord", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }
    }

    ///////////////////////////////////
    // Process Completed Time Record //
    ///////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string TimeSubmit(string EmpID, string JobNo, string OhAcct, string Dept, string Task, string ClassTime,
                                    string ClassDesc, string ClassLoc, string ClassInstr, string HoursType, string Hours, string workDate, string T)
    {
        SIU_TimeSheet_TimeEntryView tev = null;
        try
        {
            decimal iHours;
            string lblErr = string.Empty;

            if (EmpID.Length == 0)
            {
                SqlServer_Impl.LogDebug("SiuDao.TimeSubmit", "Invalid Emp: " + EmpID);
                return "Please Sign On." + Environment.NewLine;
            }

            if (EmpID.ToLower() == "missing" )
            {
                SqlServer_Impl.LogDebug("SiuDao.TimeSubmit", "Invalid Emp: " + EmpID);
                return "Your Navision Employee Record Was Not Located" + Environment.NewLine;
            }

            try
            {
                iHours = decimal.Parse(Hours);
            }
            catch (Exception)
            {
                SqlServer_Impl.LogDebug("SiuDao.TimeSubmit", "Hours Non Numeric: " + Hours);
                return "Hours Must Be Numeric." + Environment.NewLine;
            }

            ////////////////////////////////////////////////
            // When Holiday Time Client Forces as Absence //
            // Move It Back To Absence                    //
            ////////////////////////////////////////////////
            //if (OhAcct == "HOLIDAY") HoursType = "HT";

            tev = new SIU_TimeSheet_TimeEntryView
            {
                UserEmpID = EmpID,
                _JobNo = JobNo,
                _OhAcct = OhAcct,
                _Dept = Dept,
                _Task = Task,
                _ClassTime = ClassTime,
                _ClassDesc = ClassDesc,
                _ClassLoc = ClassLoc,
                _ClassInstr = ClassInstr,
                _HoursType = HoursType,
                _Hours = iHours,
                EntryDate = DateTime.Parse(workDate)
            };


            ///////////////////////
            // Validate The Form //
            ///////////////////////
            lblErr = BusinessLayer.ValidateTimeEntry(tev).Aggregate(lblErr, (current, err) => current + (err + "<br/>"));

            ///////////////////////////////////////////////////
            // Ignore Submission Request If There Are Errors //
            ///////////////////////////////////////////////////
            if (lblErr.Length > 0)
                return lblErr;

            ////////////////////////////
            // Init A New Data Object //
            ////////////////////////////
            Shermco_Time_Sheet_Entry tse =
                SqlDataMapper<Shermco_Time_Sheet_Entry>.MakeNewDAO<Shermco_Time_Sheet_Entry>();
            Shermco_Employee emp = SqlServer_Impl.GetEmployeeByNo(tev.UserEmpID);

            ///////////////////////////
            // Move Data Into Record //
            ///////////////////////////
            tse.Entry_Date_And_Time = DateTime.Now;
            tse.Created_By_Another_Employee = 0;
            tse.Created_by_Employee_No_ = tse.Employee_No_ = tse.Created_by_Employee_No_ = tev.UserEmpID;

            tse.Work_Date = tev.EntryDate;
            tse.User_ID = tse.Last_Modified_User_id = BusinessLayer.UserName;
            tse.Status = (int) TimeSheetEntry_Status.EmployeeApproved;
            tse.Prior_Status = (int) TimeSheetEntry_Status.New;
            tse.Shortcut_Dimension_1_Code = tev._Dept;
            tse.Task_Code = tev._Task;
            tse.Job_No_ = tev._JobNo;
            tse.Pay_Posting_Group = tev._OhAcct;

            /////////////////////////////////////////////
            // Add Class Info If The User Was In Class //
            /////////////////////////////////////////////
            //TSE.Class_Start_Time = TEV._ClassTime;
            tse.Class_Description = tev._ClassDesc;
            tse.Class_Instructor = tev._ClassInstr;
            tse.Class_Location = tev._ClassLoc;

            ////////////////////
            // Load THe Hours //
            ////////////////////
            if (tev._HoursType == "ST") tse.Straight_Time = tse.Initial_Straight_Time = tev._Hours;
            if (tev._HoursType == "OT") tse.Over_Time = tse.Initial_Over_Time = tev._Hours;
            if (tev._HoursType == "DT") tse.Double_Time = tse.Initial_Double_Time = tev._Hours;
            if (tev._HoursType == "HT") tse.Holiday_Time = tse.Initial_Holiday_Time = tev._Hours;
            if (tev._HoursType == "AB") tse.Absence_Time = tse.Initial_Absence_Time = tev._Hours;

            ///////////////////////////
            // Record Approval Level //
            ///////////////////////////
            tse.Approval_Code = emp.Time_Entry_Approval_Code;

            /////////////////////////////////////////////////
            // Show That This Record Was Added By Web Site //
            /////////////////////////////////////////////////
            tse.Web = 1;

            ////////////////////////////////
            // Write Record To Database //
            //////////////////////////////
            string sqlerr = SqlServer_Impl.RecordTimeEntry(tse);

            if (sqlerr.Length == 0)
                return "Success " + DateTime.Now;
            return "Error " + sqlerr;
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.TimeSubmit", tev.ToString());
            SqlServer_Impl.LogDebug("SiuDao.TimeSubmit", ex.Message);
            return ex.Message;
        }
    }

#endregion ELO Time And Time Reporting





    #region Sync
    [WebMethod(EnableSession = true)]
    public string DirSync(string System, string Dept, string Dir)
    {
        ApplyDirSync(@"F:\DFS\EHS\Forms", @"F:\PROD\EHS\Forms");



        return "Success";
    }


    private void ApplyDirSync(string SourcePath, string DestinationPath)
    {

        // RecycleDeletedFiles 
        // If this value is set, the provider will move files deleted during change application to the recycle bin. 
        // If this value is not set, files will be permanently deleted. 

        // RecyclePreviousFileOnUpdates 
        // If this value is set, the provider will move files overwritten during change application to the recycle bin. 
        // If this value is not set, files will be overwritten in place and any data in the old file will be lost. 

        // RecycleConflictLoserFiles 
        // If this value is set, the provider will move files that are conflict losers to the recycle bin. 
        // If this value is not set, the provider will move the files to a specified location. 
        // Or, if no location is specified, the files will be permanently deleted. 

        // CompareFileStreams 
        // If this value is set, the provider will compute a hash value for each file that is based on the contents of 
        // the whole file stream and use this value to compare files during change detection. 
        // This option is expensive and will slow synchronization, but provides more robust change detection. 
        // If this value is not set, an algorithm that compares modification times, file sizes, file names, and file 
        // attributes will be used to determine whether a file has changed. 

        try
        {
            const FileSyncOptions options = FileSyncOptions.RecycleDeletedFiles |
                                            FileSyncOptions.RecyclePreviousFileOnUpdates |
                                            FileSyncOptions.RecycleConflictLoserFiles |
                                            FileSyncOptions.CompareFileStreams |
                                            FileSyncOptions.ExplicitDetectChanges;

            // Create a filter that excludes all *.lnk files. The same filter should be used 
            // by both providers.
            FileSyncScopeFilter filter = new FileSyncScopeFilter();
            filter.FileNameExcludes.Add("*.lnk, *.config");

            // Explicitly detect changes on both replicas before syncyhronization occurs.

            string srcMeta = SourcePath.Replace('\\', '_').Replace(':', '_');
            string dstMeta = DestinationPath.Replace('\\', '_').Replace(':', '_');
            DetectChangesOnFileSystemReplica(SourcePath, filter, options, srcMeta);
            DetectChangesOnFileSystemReplica(DestinationPath, filter, options, dstMeta);

            // Synchronize the replicas in both directions. In the first session replica 1 is
            // the source, and in the second session replica 2 is the source. The third parameter
            // (the filter value) is null because the filter is specified in DetectChangesOnFileSystemReplica().
            SyncFileSystemReplicasOneWay(SourcePath, DestinationPath, options, srcMeta, dstMeta);
        }
        catch (Exception e)
        {
            SqlServer_Impl.LogDebug("ApplyDirSync", e.Message);
            throw e;
        }
        
    }

    private static void DetectChangesOnFileSystemReplica(string replicaRootPath, FileSyncScopeFilter filter, FileSyncOptions options, string syncFileName)
    {
        FileSyncProvider provider = null;

        try
        {
            provider = new FileSyncProvider(replicaRootPath, filter, options, @"F:/Files/SyncDocs", syncFileName, Path.GetTempPath(), null);
            provider.DetectChanges();
        }
        finally
        {
            // Release resources.
            if (provider != null)
                provider.Dispose();
        }
    }

    private static void SyncFileSystemReplicasOneWay(string sourceReplicaRootPath, string destinationReplicaRootPath, FileSyncOptions options, string srcMeta, string dstMeta)
    {
        FileSyncProvider sourceProvider = null;
        FileSyncProvider destinationProvider = null;

        try
        {
            // Instantiate source and destination providers, with a null filter (the filter
            // was specified in DetectChangesOnFileSystemReplica()), and options for both.
            sourceProvider = new FileSyncProvider(sourceReplicaRootPath, null, options, @"F:/Files/SyncDocs", srcMeta, Path.GetTempPath(), null);
            //sourceProvider.PreviewMode = true;
            destinationProvider = new FileSyncProvider(destinationReplicaRootPath, null, options, @"F:/Files/SyncDocs", dstMeta, Path.GetTempPath(), null);
            //destinationProvider.PreviewMode = true;

            // Register event handlers so that we can write information
            // to the console.
            destinationProvider.AppliedChange += new EventHandler<AppliedChangeEventArgs>(OnAppliedChange);
            destinationProvider.SkippedChange += new EventHandler<SkippedChangeEventArgs>(OnSkippedChange);

            // Use SyncCallbacks for conflicting items.
            SyncCallbacks destinationCallbacks = destinationProvider.DestinationCallbacks;
            destinationCallbacks.ItemConflicting += new EventHandler<ItemConflictingEventArgs>(OnItemConflicting);
            destinationCallbacks.ItemConstraint += new EventHandler<ItemConstraintEventArgs>(OnItemConstraint);

            SyncOrchestrator agent = new SyncOrchestrator();
            agent.LocalProvider = sourceProvider;
            agent.RemoteProvider = destinationProvider;
            agent.Direction = SyncDirectionOrder.Upload; // Upload changes from the source to the destination.

            SqlServer_Impl.LogDebug("SyncFileSystemReplicasOneWay", destinationProvider.RootDirectoryPath);
            agent.Synchronize();
        }
        finally
        {
            // Release resources.
            if (sourceProvider != null) sourceProvider.Dispose();
            if (destinationProvider != null) destinationProvider.Dispose();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    // Provide information about files that were affected by the synchronization session. //
    ////////////////////////////////////////////////////////////////////////////////////////
    public static void OnAppliedChange(object sender, AppliedChangeEventArgs args)
    {
        switch (args.ChangeType)
        {
            case ChangeType.Create:
                SqlServer_Impl.LogDebug("SyncFileSystemReplicasOneWay", "CREATED file " + args.NewFilePath);
                break;

            case ChangeType.Delete:
                SqlServer_Impl.LogDebug("SyncFileSystemReplicasOneWay", "DELETED file " + args.OldFilePath);
                break;

            case ChangeType.Update:
                SqlServer_Impl.LogDebug("SyncFileSystemReplicasOneWay", "OVERWRITE file " + args.OldFilePath);
                break;

            case ChangeType.Rename:
                SqlServer_Impl.LogDebug("SyncFileSystemReplicasOneWay", "RENAME file " + args.OldFilePath + " as " + args.NewFilePath);
                break;
        }
    }

    /////////////////////////////////////////////////////////////
    // Log error information for any changes that were skipped //
    /////////////////////////////////////////////////////////////
    public static void OnSkippedChange(object sender, SkippedChangeEventArgs args)
    {
        SqlServer_Impl.LogDebug("SyncFileSystemReplicasOneWay", "-- Skipped applying " + args.ChangeType.ToString().ToUpper()
              + " for " + (!string.IsNullOrEmpty(args.CurrentFilePath) ?
                            args.CurrentFilePath : args.NewFilePath) + " due to error");

        if (args.Exception != null)
            SqlServer_Impl.LogDebug("SyncFileSystemReplicasOneWay", "   [" + args.Exception.Message + "]");
    }

    //////////////////////////////////////////////////////////////////////////////////////
    // By default, conflicts are resolved in favor of the last writer. In this example, //
    // the change from the source will always win the conflict.                         //
    //////////////////////////////////////////////////////////////////////////////////////
    public static void OnItemConflicting(object sender, ItemConflictingEventArgs args)
    {
        args.SetResolutionAction(ConflictResolutionAction.SourceWins);
    }
    public static void OnItemConstraint(object sender, ItemConstraintEventArgs args)
    {
        args.SetResolutionAction(ConstraintConflictResolutionAction.SourceWins);
    }





#endregion Sync

#region Job Reports
    [WebMethod(EnableSession = true)]
    public string GetJobRptJobs()
    {
        string jobList = string.Empty;

        foreach (var job in SqlServer_Impl.GetJobRptJobs())
            jobList += job + "\r";

        return jobList;
    }

    /////////////////////////////////////////////////////
    // Get A List of Job Reports Submitted By Employee //
    /////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetSubmitJobReportNosByEmp(string EmpID)
    {
        try
        {
            List<string> rpts = SqlServer_Impl.GetSubmitJobReportNosByEmp(EmpID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(rpts);
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetSubmitJobReportNosByEmp", EmpID);
            SqlServer_Impl.LogDebug("SiuDao.GetSubmitJobReportNosByEmp", ex.Message);
            throw;
        }
    }

    //////////////////////////////////////////////
    // Lookup Disposition Of A Report For A Job //
    //////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetSubmitJobReportByNo(string jobNo)
    {
        try
        {
            Shermco_Job_Report rpt = SqlServer_Impl.GetSubmitJobReportByNo(jobNo);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(rpt);
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetSubmitJobReportByNo", jobNo);
            SqlServer_Impl.LogDebug("SiuDao.GetSubmitJobReportByNo", ex.Message);
            throw;
        }
    }

    /////////////////////////////////////////////////////////////
    // Record The Disposition Of A Reports Turned In For A Job //
    /////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public void SubmitJobRpt(   string UserEmpID, string jobNo, string Complete, string Partial, string PE, string No_Report_Reason, string comments,
                                string chkNoData, string chkPowerDB, string chkScanned, string chkPdbMaster, string chkOtherData, string RTS_Relay_Data,
                                string CT_Data_Saved, string Partial_Discharge, string Oil_Sample, string Oil_Sample_Follow_UP, string OtherText,
                                string Sonic, string TTR, string Thermo, string Relay, string PCB, string PD, string Oil, string NFPA, string InslResit,
                                string GrdEltrode, string GrdFlt, string Doble, string DLRO, string Decal, string HiPot, string BBT, string None, string IrData,
                                string chkIrDrpBox, string chkIrOnly, string chkIrPort, string txtIrHardCnt, string txtAddEmail, string chkRptDrBox, string chkRptDrBoxNo, 
                                string chkIrDrpBoxNo, string SalesFollowUp, string SalesNotes)
        
    {

        Shermco_Job_Report jobRpt = null;

        try
        {

            /////////////////////////////////
            // Look For An Existing Record //
            /////////////////////////////////
            jobRpt = SqlServer_Impl.GetSubmitJobReportByNo(jobNo);

            ////////////////////////////////////////////////////
            // If This Is A New Record, Create Default Values //
            ////////////////////////////////////////////////////
            if (jobRpt == null)
                jobRpt = SqlDataMapper<Shermco_Job_Report>.MakeNewDAO<Shermco_Job_Report>();


            //////////////////////////////////////
            // UUDecode Free Format Text Fields //
            //////////////////////////////////////
            SalesNotes = Server.UrlDecode(SalesNotes);
            int len = GetLengthLimit(jobRpt, "SalesFollowUp_Comment");
            if (SalesNotes.Length > len)
                SalesNotes = SalesNotes.Substring(0, len);

            
            comments = Server.UrlDecode(comments);
            len = GetLengthLimit(jobRpt, "Comment");
            if (comments.Length > len)
                comments = comments.Substring(0, len);

            No_Report_Reason = Server.UrlDecode(No_Report_Reason);
            len = GetLengthLimit(jobRpt, "No_Report_Required_Reason");
            if (No_Report_Reason.Length > len)
                No_Report_Reason = No_Report_Reason.Substring(0, len);

            OtherText = Server.UrlDecode(OtherText);
            len = GetLengthLimit(jobRpt, "TmpOtherText");
            if (OtherText.Length > len)
                OtherText = OtherText.Substring(0, len);

            txtAddEmail = Server.UrlDecode(txtAddEmail);
            len = GetLengthLimit(jobRpt, "Email");
            if (txtAddEmail.Length > len)
                txtAddEmail = txtAddEmail.Substring(0, len);
            
            

            ////////////
            // Job No //
            ////////////
            jobRpt.Job_No_ = jobNo;

            /////////////////////////////////////////////////
            // If I/R Report Set Logged and Received dates //
            /////////////////////////////////////////////////
            if ( chkIrOnly == "true")
            {
                jobRpt.Date_Report_Turned_In_by_Tech = DateTime.Now.Date;

                jobRpt.Turned_in_by_Tech_Date = DateTime.Now.Date;
                jobRpt.Turned_in_by_Tech_UserID = UserEmpID;
            }

            ///////////////////////////////////////////////////////
            // For I/R Portion, Only Set Received From Tech Info //
            ///////////////////////////////////////////////////////
            if ( chkIrPort == "true")
            {
                jobRpt.IR_Received_From_Tech = DateTime.Now.Date;
                jobRpt.IR_Received_From_Tech_User = BusinessLayer.UserName;                
            }

            ////////////
            // IrData //
            ////////////
            if (IrData.ToLower() == "true") jobRpt.TmpIRData = 1;
            jobRpt.Email = txtAddEmail;
            jobRpt.No__of_Copies = int.Parse(txtIrHardCnt);
            if (chkIrPort.ToLower() == "true") jobRpt.IRonFinalReport = 1;
            if (chkIrOnly.ToLower() == "true") jobRpt.IROnly = 1;
            if (chkIrDrpBox.ToLower() == "true") jobRpt.TmpIRData = 1;
            if (chkIrDrpBoxNo.ToLower() == "true") jobRpt.TmpIRData_No = 1;

            //////////////////////////////////////////
            // Who Is Touching This Rcd             //
            // Updated Every Time Record Is Touched //
            //////////////////////////////////////////
            jobRpt.Turned_in_By_Emp__No_ = UserEmpID;
            jobRpt.Turned_in_by_Tech_Date = DateTime.Now.Date;
            jobRpt.Turned_in_by_Tech_UserID = UserEmpID;
            jobRpt.Last_Date_Modified = DateTime.Now.Date;

            //////////////////
            // Comments Box //
            //////////////////
            jobRpt.Comment = comments;

            ////////////////////////
            // Report Disposition //
            ////////////////////////
            jobRpt.TmpComplete = (byte)((Complete.ToLower() == "true") ? 1 : 0);
            jobRpt.TmpPartial = (byte)((Partial.ToLower() == "true") ? 1 : 0);
            jobRpt.TmpPE = (byte)((PE.ToLower() == "true") ? 1 : 0);
            jobRpt.No_Report_Required = (byte)(No_Report_Reason.Length > 0 ? 1 : 0);
            if ( jobRpt.No_Report_Required == 1)
                jobRpt.No_Report_Required_Reason = No_Report_Reason;

            ///////////////////
            // Report Format //
            ///////////////////
            if (chkNoData.ToLower() == "true") jobRpt.Report_Data_Format = 1;
            if (chkPowerDB.ToLower() == "true") jobRpt.Report_Data_Format = 2;
            if (chkScanned.ToLower() == "true") jobRpt.Report_Data_Format = 3;
            if (chkPdbMaster.ToLower() == "true") jobRpt.Report_Data_Format = 4;
            if (chkOtherData.ToLower() == "true") jobRpt.Report_Data_Format = 5;

            if (chkRptDrBox.ToLower() == "true") jobRpt.General_Dropbox = 1;
            if (chkRptDrBoxNo.ToLower() == "true") jobRpt.General_Dropbox_No = 1;


            /////////////////////////
            // Report "Other Data" //
            /////////////////////////
            jobRpt.RTS_Relay_Data = int.Parse(RTS_Relay_Data);
            jobRpt.CT_Data_Saved = int.Parse(CT_Data_Saved);
            jobRpt.Partial_Discharge = int.Parse(Partial_Discharge);
            jobRpt.Oil_Sample = int.Parse(Oil_Sample);
            jobRpt.Oil_Sample_Follow_UP = int.Parse(Oil_Sample_Follow_UP);
            jobRpt.TmpOther = (byte)(OtherText.Length > 0 ? 1 : 0);
            if (jobRpt.TmpOther == 1)
                jobRpt.TmpOtherText = OtherText;


            ///////////////
            // Tests Run //
            ///////////////
            if (Sonic.ToLower() == "true") jobRpt.Ultrasonic_Testing = 1;
            if (TTR.ToLower() == "true") jobRpt.TTR = 1;
            if (Thermo.ToLower() == "true") jobRpt.Thermograpic_IR_ = 1;
            if (Relay.ToLower() == "true") jobRpt.RTS_Relay_Data = 1;
            if (PCB.ToLower() == "true") jobRpt.PCB_Info = 1;
            if (PD.ToLower() == "true") jobRpt.Partial_Discharge_B = 1;
            if (Oil.ToLower() == "true") jobRpt.Oil_Tests = 1;
            if (NFPA.ToLower() == "true") jobRpt.NFPA_99 = 1;
            if (InslResit.ToLower() == "true") jobRpt.Insulation_Resistance = 1;
            if (GrdEltrode.ToLower() == "true") jobRpt.Grounding___Ground_Electrode = 1;
            if (GrdFlt.ToLower() == "true") jobRpt.Ground_Fault_Systems = 1;
            if (Doble.ToLower() == "true") jobRpt.Doble = 1;
            if (DLRO.ToLower() == "true") jobRpt.DLRO = 1;
            if (Decal.ToLower() == "true") jobRpt.Decal_Color_Codes = 1;
            if (HiPot.ToLower() == "true") jobRpt.DC_Hipot = 1;
            if (BBT.ToLower() == "true") jobRpt.Bus_Bolt_Torque = 1;
            if (None.ToLower() == "true") jobRpt.No_Testing_Done = 1;

            /////////////////////
            // Sales Follow-Up //
            /////////////////////
            if (SalesFollowUp.ToLower() == "true") jobRpt.SalesFollowUp = 1;
            else jobRpt.SalesFollowUp = 0;
            jobRpt.SalesFollowUp_Comment = SalesNotes;

            jobRpt.Last_Date_Modified = DateTime.Now.Date;
            jobRpt.Received = 0;

            SqlServer_Impl.RecordSubmitJobReport(jobRpt);

            /////////////////////////////////////////////////////
            // If a New I/R Report Was Turned In Send An Email //
            /////////////////////////////////////////////////////
            if (jobRpt.IROnly == 1 || jobRpt.IRonFinalReport == 1)
                WebMail.JobReportIrNotice(jobRpt);

            if (jobRpt.SalesFollowUp == 1)
                WebMail.JobSalesNotice(jobRpt);

            WebMail.JobTechAckNotice(jobRpt);
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.SubmitJobRpt", ex.Message);
            if (jobRpt != null)
                SqlServer_Impl.LogDebug("SiuDao.SubmitJobRpt", 
                                            "sales: " + jobRpt.SalesFollowUp_Comment.Length + 
                                            " Comments: " + jobRpt.Comment.Length + 
                                            " other: " + jobRpt.TmpOtherText.Length +
                                            " email: " + jobRpt.Email.Length +
                                            " NoRpt: " + jobRpt.No_Report_Required_Reason.Length);
            throw;
        }
    }
#endregion Job Reports

#region Vehicle Personal Miles
    ////////////////////////////////////////////////////////////////////////
    // Get List Of Vehicles Available To Report Mileage For Week Selected //
    ////////////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetVehilceMileageRptAvail(string empNo, string WeekEnding)
    {
        try
        {
            List<string> rpts = SqlServer_Impl.GetVehilceMileageRptAvail(empNo, DateTime.Parse(WeekEnding)  );
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(rpts);      
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetVehilceMileageRptAvail", empNo + ", " + WeekEnding);
            SqlServer_Impl.LogDebug("SiuDao.GetVehilceMileageRptAvail", ex.Message);
            throw;
        }
    }

    ///////////////////////////////////////////////
    // Get List Of Vehicles Assigned To Employee //
    ///////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetVehicleAssigned(string empNo, string WeekEnding)
    {
        try
        {
            List<VehModelNoList> rpts = (from usersVehList in SqlServer_Impl.GetVehicleAssigned(empNo, DateTime.Parse(WeekEnding))
                                         select new VehModelNoList { VehNo = usersVehList.No_, VehModel = usersVehList.Model }
                                ).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(rpts); 
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetVehicleAssigned", empNo + ", " + WeekEnding);
            SqlServer_Impl.LogDebug("SiuDao.GetVehicleAssigned", ex.Message);
            throw;
        }
    }

    /////////////////////////////////////////////////////////////////////
    // Lookup Vehicle Mileage For An Employee For A Week For A Vehicle //
    /////////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetVehicleMileageRecord(string empNo, string WeekEnding)
    {
        try
        {
        Shermco_Assigend_Vehicle_Data rpt = SqlServer_Impl.GetVehicleMileageRecord(empNo, DateTime.Parse(WeekEnding));
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(rpt);         
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetVehicleMileageRecord", empNo + ", " + WeekEnding);
            SqlServer_Impl.LogDebug("SiuDao.GetVehicleMileageRecord", ex.Message);
            throw;
        }
    }

    /////////////////////////////////////////////////////////////////////
    // Record Vehicle Mileage For An Employee For A Week For A Vehicle //
    /////////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string SubmitVehicleMileage(string empNo, string VehNo, string Mileage, string WeekEnding)
    {
        try
        {
            Shermco_Assigend_Vehicle_Data vehRpt = SqlDataMapper<Shermco_Assigend_Vehicle_Data>.MakeNewDAO<Shermco_Assigend_Vehicle_Data>();

            vehRpt.Assigned_Vehicle_No_ = VehNo;
            vehRpt.Emp__No = empNo;
            vehRpt.Week_Ending =  DateTime.Parse(WeekEnding);
            vehRpt.Weekly_Personal_Mileage =   int.Parse(Mileage);

            return SqlServer_Impl.RecordVehicleMileage(vehRpt);
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.SubmitVehicleMileage", empNo + ", " + VehNo + ", " + Mileage + ", " + WeekEnding);
            SqlServer_Impl.LogDebug("SiuDao.SubmitVehicleMileage", ex.Message);
            throw;
        }
    }

    /////////////////////////////////////////////////////////////////////
    // Remove Vehicle Mileage For An Employee For A Week For A Vehicle //
    /////////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string RemoveVehicleMileage(string empNo, string VehNo, string Mileage, string WeekEnding)
    {
        try
        {
            Shermco_Assigend_Vehicle_Data vehRpt = SqlDataMapper<Shermco_Assigend_Vehicle_Data>.MakeNewDAO<Shermco_Assigend_Vehicle_Data>();

            vehRpt.Assigned_Vehicle_No_ = VehNo;
            vehRpt.Emp__No = empNo;
            vehRpt.Week_Ending = DateTime.Parse(WeekEnding);
            vehRpt.Weekly_Personal_Mileage = int.Parse(Mileage);
            

            return SqlServer_Impl.RemoveVehicleMileage(vehRpt);
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.RemoveVehicleMileage", empNo + ", " + VehNo + ", " +  Mileage   + ", " +  WeekEnding );
            SqlServer_Impl.LogDebug("SiuDao.RemoveVehicleMileage", ex.Message);
            throw;
        }
    }
#endregion Vehicle Personal Miles

#region Vehicle Inspection
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Get Open DOT Hazards For All Vehicles Assigned To Employee AND All Unresolved Items Submitted By Employee //
    // Called By DOT Data Entry Screen                                                                           //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetOpenDOT(string EmpID, string jtSorting)
    {
        List<SIU_DOT_Rpt> rpt = SqlServer_Impl.GetOpenDOT(EmpID).OrderBy(jtSorting).ToList();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { Result = "OK", Records = rpt });
    }

    //////////////////////////
    // Called By DOT Report //
    //////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetOpenDOT_WeekView(string EmpID, string jtSorting)
    {
        List<SIU_DOT_Rpt> rpt = SqlServer_Impl.GetOpenDOT_WeekView(EmpID).OrderBy(jtSorting).ToList();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { Result = "OK", Records = rpt });
    }

    //////////////////////////
    // Called By DOT Report //
    //////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetVehInspById(string ID)
    {
        SIU_DOT_Rpt rpt = SqlServer_Impl.GetVehInspById(ID);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(rpt);
    }

    ///////////////////////////
    // Record DOT Inspection //
    ///////////////////////////
    [WebMethod(EnableSession = true)]
    public void RecordDot(int RefID, string EmpID, string InspDate, string VehNo, string Hazard, string CorrectiveAction)
    {
        SqlServer_Impl.RecordDot(RefID, EmpID, InspDate, VehNo, Hazard, CorrectiveAction);
    }

    /////////////////////////////////////////////////////////////////
    // Record DOT Correction Made Sometime After Inspection Report //
    /////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public void RecordDotCorrection(int RefID, string EmpID, string CorrectiveAction)
    {
        SqlServer_Impl.RecordDotCorrection(RefID, EmpID, CorrectiveAction);
    }

    //////////////////////////////////////////
    // Generate Open Vehicle Issues Reports //
    //////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public void kjbbvc665()
    {
        const string eMailSubject = "Unresolved Vehicle Inspections.";

        WebMail.HtmlMail("bborowczak@shermco.com; ltruitt@shermco.com", eMailSubject, BusinessLayer.GenFleetInspRpt(""));

        foreach (var deptList in SqlServer_Impl.GetReportingChainList())
        {
            List<string> emList = SqlServer_Impl.GetEmployeeEmailByNo(new List<string>() { deptList.DeptMgrEmpId });
            if (emList.Count > 0)
            {
                string email = BusinessLayer.GenFleetInspRpt(deptList.Dept);
                if (email.Length > 0)
                    WebMail.HtmlMail(emList[0], eMailSubject, email);
            }

        }

    }

    //////////////////////////
    // Generate QOM Notices //
    //////////////////////////
    [WebMethod(EnableSession = true)]
    public void SSddf55s()
    {
        BusinessLayer.GenQtmNotices();
    }

    //////////////////////////////////////////
    // Generate 
    //////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public void p775689hh()
    {
        BusinessLayer.GenQtmReminders();
    }

    //////////////////////////////////////////
    // Generate Open Vehicle Issues Reports //
    //////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GenVehInspRpt(string Dept)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(BusinessLayer.GenFleetInspRpt(Dept));
    }


#endregion Vehicle Inspection

#region Safety Pays

    /// <span class="code-SummaryComment"><summary></span>
    /// Gets the length limit for a given field on a LINQ object ... or zero if not known
    /// <span class="code-SummaryComment"></summary></span>
    /// <span class="code-SummaryComment"><remarks></span>
    /// You can use the results from this method to dynamically 
    /// set the allowed length of an INPUT on your web page to
    /// exactly the same length as the length of the database column.  
    /// Change the database and the UI changes just by
    /// updating your DBML and recompiling.
    /// <span class="code-SummaryComment"></remarks></span>
    public static int GetLengthLimit(object obj, string field)
    {
        int dblenint = 0;   // default value = we can't determine the length

        Type type = obj.GetType();
        PropertyInfo prop = type.GetProperty(field);
        // Find the Linq 'Column' attribute
        // e.g. [Column(Storage="_FileName", DbType="NChar(256) NOT NULL", CanBeNull=false)]
        object[] info = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
        // Assume there is just one
        if (info.Length == 1)
        {
            ColumnAttribute ca = (ColumnAttribute)info[0];
            string dbtype = ca.DbType;

            if (dbtype.StartsWith("NChar") || dbtype.StartsWith("NVarChar") || dbtype.StartsWith("VarChar"))
            {
                int index1 = dbtype.IndexOf("(");
                int index2 = dbtype.IndexOf(")");
                string dblen = dbtype.Substring(index1 + 1, index2 - index1 - 1);
                int.TryParse(dblen, out dblenint);
            }
        }
        return dblenint;
    }

    /// <span class="code-SummaryComment"><summary></span>
    /// If you don't care about truncating data that you are setting on a LINQ object, 
    /// use something like this ...
    /// <span class="code-SummaryComment"></summary></span>
    public static void SetAutoTruncate(object obj, string field, string value)
    {
        int len = GetLengthLimit(obj, field);
        if (len == 0) throw new ApplicationException("Field '" + field + "'does not have length metadata");

        Type type = obj.GetType();
        PropertyInfo prop = type.GetProperty(field);
        if (value.Length > len)
        {
            prop.SetValue(obj, value.Substring(0, len), null);
        }
        else
            prop.SetValue(obj, value, null);
    } 

    [WebMethod(EnableSession = true)]
    public string RecordSafetyPays(     string IncidentNo, string EID, string JobNo, string IncTypeSafeFlag, string IncTypeUnsafeFlag, string IncTypeSuggFlag,
                                        string IncTypeTopicFlag, string IncTypeSumFlag, string IncidentDate, string ObservedEmpID,
                                        string InitialResponse, string SafetyMeetingType, string SafetyMeetingDate, string Comments,
                                        string JobSite, string IncTypeText, string QomID)
    {
        ///////////////////////////////////////////
        // Make And Initialize A New Data Object //
        ///////////////////////////////////////////
        SIU_SafetyPaysReport newReport = SqlDataMapper<SIU_SafetyPaysReport>.MakeNewDAO<SIU_SafetyPaysReport>();

        InitialResponse = Server.UrlDecode(InitialResponse);
        Comments = Server.UrlDecode(Comments);

        //////////////////
        // New Incident //
        //////////////////
        newReport.IncStatus = "New";
        newReport.EmpID = EID;
        newReport.IncLastTouchEmpID = BusinessLayer.UserEmpID;
        newReport.PointsAssigned = 0;
        newReport.IncOpenTimestamp = DateTime.Parse(DateTime.Now.ToShortDateString());
        newReport.IncLastTouchTimestamp = newReport.IncOpenTimestamp;

        newReport.IncTypeSafeFlag = (IncTypeSafeFlag == "true");
        newReport.IncTypeUnsafeFlag = (IncTypeUnsafeFlag == "true");
        newReport.IncTypeSuggFlag = (IncTypeSuggFlag == "true");
        newReport.IncTypeTopicFlag = (IncTypeTopicFlag == "true");
        newReport.IncTypeSumFlag = (IncTypeSumFlag == "true");

        if (IncidentDate.Length > 0)
            newReport.IncidentDate = DateTime.Parse(IncidentDate);
        if (SafetyMeetingDate.Length > 0)
            newReport.SafetyMeetingDate = DateTime.Parse(SafetyMeetingDate);
        newReport.ObservedEmpID = ObservedEmpID;
        newReport.InitialResponse = InitialResponse;
        newReport.SafetyMeetingType = SafetyMeetingType;

        newReport.Comments = Comments;

        newReport.JobSite = JobSite;
        newReport.JobNo = JobNo;
        newReport.IncTypeTxt = IncTypeText;

        IEnumerable<string> errorList = BusinessLayer.ValidateSafetyPays(newReport).ToList();
        if (errorList.Any())
            return "Error: " + errorList.First();


        if (newReport.IncTypeTxt == "VEST" || newReport.IncTypeTxt == "VPP")
            newReport.IncTypeTxt += " QOM";

        if (QomID.Length > 0)
            newReport.QOM_ID = int.Parse(QomID);


        //////////////////////////////////
        // Write New Record To Database //
        //////////////////////////////////
        newReport.IncidentNo = SqlServer_Impl.RecordSafetyPaysReport(newReport);

        WebMail.SafetyPaysNewEmail(newReport, BusinessLayer.UserEmail, BusinessLayer.UserFullName);

        if (BusinessLayer.UserEmpID != newReport.EmpID)
        {
            Shermco_Employee e = SqlServer_Impl.GetEmployeeByNo(newReport.EmpID);
            WebMail.SafetyPaysNewEmail(newReport, e.Company_E_Mail, e.Last_Name + ", " + e.First_Name);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // If An Existing Incident No Was Passed In Close It And Replace It With This New Record //
        ///////////////////////////////////////////////////////////////////////////////////////////
        if ( IncidentNo.Length > 0 )
        {
            if (IncidentNo != "0")
                SqlServer_Impl.RecordSafetyPaysStatus(int.Parse(IncidentNo), "Replaced", BusinessLayer.UserEmpID, true, 0, "This Incident Was Replaced By Incident " + newReport.IncidentNo);
            
        }

        return "";        
    }



    /////////////////////////////////
    // New Safety Pays Report Task //
    /////////////////////////////////
    [WebMethod(EnableSession = true)]
    public object RecordSafetyPaysRptTask(SIU_SafetyPays_TaskList record)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try 
        {
            record = SqlServer_Impl.RecordSafetyPaysTask(record);
            WebMail.SafetyPaysTaskAssignEMail(record, BusinessLayer.UserEmail, BusinessLayer.UserFullName);
            return new { Result = "OK", Record =   new SIU_SafetyPays_TaskList_Rpt( record ) };
        }

        catch (Exception ex)
        {
            string input = serializer.Serialize(record);
            SqlServer_Impl.LogDebug("SiuDao.RecordSafetyPaysRptTask", input);
            SqlServer_Impl.LogDebug("SiuDao.RecordSafetyPaysRptTask", ex.Message);
            return new { Result = "ERROR", ex.Message };
        }
    }

    ////////////////////////////////////////
    // New Safety Pays Report Task Status //
    ////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public object RecordSafetyPaysRptTaskStatus(SIU_SafetyPays_TaskStatus record)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try 
        {
            record.ResponseDate = DateTime.Now.Date;
            record = SqlServer_Impl.RecordSafetyPaysTaskStatus(record);
            return new { Result = "OK", Record = record };
        }

        catch (Exception ex)
        {
            string input = serializer.Serialize(record);
            SqlServer_Impl.LogDebug("SiuDao.RecordSafetyPaysRptTask", input);
            SqlServer_Impl.LogDebug("SiuDao.RecordSafetyPaysRptTask", ex.Message);
            return new { Result = "ERROR", ex.Message };
        }
    }

    ////////////////////////////////////////
    // New Safety Pays Report Task Status //
    ////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public object RecordSafetyPaysTaskComplete(string RptID, string TaskNo, string CloseNotes)
    {
        try 
        {
            SIU_SafetyPays_TaskList task = SqlServer_Impl.RecordSafetyPaysTaskComplete(int.Parse(RptID), int.Parse(TaskNo), CloseNotes );
            WebMail.SafetyPaysTaskCompleteEMail(task, BusinessLayer.UserEmail, BusinessLayer.UserFullName, CloseNotes);
            return task;
        }

        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.RecordSafetyPaysTaskComplete", RptID + ", " + TaskNo);
            SqlServer_Impl.LogDebug("SiuDao.RecordSafetyPaysTaskComplete", ex.Message);
            throw;
        }
    }

    


    //////////////////////////////
    // Close Safety Pays Report //
    //////////////////////////////
    [WebMethod(EnableSession = true)]
    public void RecordSafetyPaysStatusAcceptClosed(string RcdID, string EmpID, string Points, string ehsRepsonse, string SuprID, string QID)
    {
        string suprEmail = "";
        if (SuprID.Length > 0)
            suprEmail = SqlServer_Impl.GetEmployeeByNo(SuprID).Company_E_Mail;

        SIU_SafetyPaysReport rptRcd = SqlServer_Impl.RecordSafetyPaysStatus(int.Parse(RcdID), "Closed", EmpID, true, int.Parse(Points), ehsRepsonse);
        SqlServer_Impl.RecordSafetyPaysPoints(rptRcd);
        WebMail.SafetyPaysAcceptEMail(rptRcd, suprEmail, BusinessLayer.UserFullName);


    }

    /////////////////////////////////////////////////////////////
    // Set A Safety Pays Report Status to ACCEPTED and WORKING //
    /////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public void RecordSafetyPaysStatusWork(string RcdID, string EmpID, string Points, string ehsRepsonse, string SuprID, string QID)
    {
        string suprEmail = "";
        if ( SuprID.Length > 0 )
            suprEmail = SqlServer_Impl.GetEmployeeByNo(SuprID).Company_E_Mail;

        SIU_SafetyPaysReport rptRcd = SqlServer_Impl.RecordSafetyPaysStatus(int.Parse(RcdID), "Working", EmpID, false, int.Parse(Points), ehsRepsonse);
        SqlServer_Impl.RecordSafetyPaysPoints(rptRcd);
        WebMail.SafetyPaysWorkingEMail(rptRcd, suprEmail, BusinessLayer.UserFullName);
    }

    ///////////////////////////////
    // Reject Safety Pays Report //
    ///////////////////////////////
    [WebMethod(EnableSession = true)]
    public void RecordSafetyPaysStatusReject(string RcdID, string EmpID, string ehsRepsonse, string SuprID, string QID)
    {
        string suprEmail = "";
        if (SuprID.Length > 0)
            suprEmail = SqlServer_Impl.GetEmployeeByNo(SuprID).Company_E_Mail;

        SIU_SafetyPaysReport rptRcd = SqlServer_Impl.RecordSafetyPaysStatus(int.Parse(RcdID), "Reject", EmpID, true, 0, ehsRepsonse);
        WebMail.SafetyPaysRejectedEMail(rptRcd, suprEmail, BusinessLayer.UserFullName);
    }

    /////////////////////////////////
    // Complete Safety Pays Report //
    /////////////////////////////////
    [WebMethod(EnableSession = true)]
    public void RecordSafetyPaysStatusComplete(string RcdID, string EmpID)
    {
        SIU_SafetyPaysReport rptRcd = SqlServer_Impl.RecordSafetyPaysStatus(int.Parse(RcdID), "Closed", EmpID, true, 0);
        WebMail.SafetyPaysClosedEMail(rptRcd, BusinessLayer.UserEmail, BusinessLayer.UserFullName);
    }


    //////////////////////////////////////////////////////////
    // Get Summed Counts Of Safety Pays Reports By Category //
    // Presented To EHS Admin                               //
    //////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetSafetyPaysSummaryCounts()
    {
        SIU_SummaryCountsResult rpt = SqlServer_Impl.GetSafetyPaysSummaryCounts();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(rpt); 
    }


    ////////////////////////////////////////////
    // Return List Of Open Safety Pays Report //
    // Filtered by Several Options            //
    //  Or Return Just A Single Report        //
    ////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetSafetyPaysRptDataSorted(string DataFilter, string isA, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "IncidentNo")
    {
        ////////////////////////////////
        // Build Response Data Object //
        ////////////////////////////////
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            List<SIU_SafetyPaysReport_Rpt> jasonResp = null;
            int rcdCnt = 0;

            switch (DataFilter)
            {
                case "New":
                    jasonResp = SqlServer_Impl.GetNewSafetyPaysRpts(jtStartIndex, jtPageSize, jtSorting);
                    rcdCnt = (jasonResp == null) ? 0 : SqlServer_Impl.GetNewSafetyPaysRptsCount();
                    break;

                default:
                    int rptId;
                    if (int.TryParse(DataFilter, out rptId))
                        return serializer.Serialize(new { Result = "OK", Records = new SIU_SafetyPaysReport_Rpt(SqlServer_Impl.GetSafetyPaysReport(rptId).SingleOrDefault()) });
                    break;
            }
            if (jasonResp != null)
                if (isA != "1")
                    jasonResp = jasonResp.Where(c => c.EmpID == BusinessLayer.UserEmpID).ToList();

            return serializer.Serialize(new { Result = "OK", Records = jasonResp, TotalRecordCount = rcdCnt });
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetSafetyPaysRptData", DataFilter);
            SqlServer_Impl.LogDebug("SiuDao.GetSafetyPaysRptData", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }
     

    }    

    [WebMethod(EnableSession = true)]
    public string GetSafetyPaysRptData(string DataFilter, string isA, int jtStartIndex = 0, int jtPageSize = 0)
    {
        ////////////////////////////////
        // Build Response Data Object //
        ////////////////////////////////
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            List<SIU_SafetyPaysReport_Rpt> jasonResp = null;
            int rcdCnt = 0;

            switch (DataFilter)
            {
                case "New":
                    jasonResp = SqlServer_Impl.GetNewSafetyPaysRpts(jtStartIndex, jtPageSize);
                    rcdCnt = (jasonResp == null) ? 0 : SqlServer_Impl.GetNewSafetyPaysRptsCount();
                    break;

                case "NoTasks":
                    jasonResp = SqlServer_Impl.GetSafetyPaysNoTask();
                    break;

                case "LateTask":
                    jasonResp = SqlServer_Impl.GetSafetyPaysLateTask(isA);
                    break;

                case "LateStaus":
                    jasonResp = SqlServer_Impl.GetSafetyPaysLateStatus(isA);
                    break;

                case "CloseReady":
                    jasonResp = SqlServer_Impl.GetSafetyPaysCloseReadyStatus();
                    break;

                case "Current":
                    jasonResp = SqlServer_Impl.GetSafetyPaysCloseReadyStatus();
                    break;

                case "Assigned":
                    jasonResp = SqlServer_Impl.GetAssignedWorkingTasksSafetyPaysRpts();
                    break;

                case "All":
                    jasonResp = SqlServer_Impl.GetAllWorkingTasksSafetyPaysRpts();
                    break;

                default:
                    int rptId;
                    if ( int.TryParse(DataFilter, out rptId) )
                        return serializer.Serialize(new { Result = "OK", Records = new SIU_SafetyPaysReport_Rpt(SqlServer_Impl.GetSafetyPaysReport(rptId).SingleOrDefault())  });
                    break;
            }
            if ( jasonResp != null)
                if ( isA != "1")
                    jasonResp = jasonResp.Where(c => c.EmpID == BusinessLayer.UserEmpID).ToList();

            return serializer.Serialize(new { Result = "OK", Records = jasonResp, TotalRecordCount = rcdCnt });
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetSafetyPaysRptData", DataFilter);
            SqlServer_Impl.LogDebug("SiuDao.GetSafetyPaysRptData", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }
    }

    [WebMethod(EnableSession = true)]
    public string RemoveSafetyPaysRpt(string IncidentNo)
    {
        //////////////////////////////////////////////
        // Called From JTable So Need JSON Response //
        //////////////////////////////////////////////
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            SqlServer_Impl.RemoveSafetyPaysRpt(IncidentNo);
            return serializer.Serialize(new { Result = "OK" });
        }

        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("RemoveSafetyPaysRpt", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }  
    }

    ////////////////////////////////////////////
    // Return List Of Open Safety Pays Report //
    // Filtered by Several Options            //
    //  Or Return Just A Single Report        //
    ////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetSafetyPaysRptTasks(string RptID)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        try
        {
            List<SIU_SafetyPays_TaskList_Rpt> jasonResp = SqlServer_Impl.GetSafetyPaysTasksRpt( int.Parse(RptID));
            return serializer.Serialize(new { Result = "OK", Records = jasonResp });
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetSafetyPaysRptTasks", RptID);
            SqlServer_Impl.LogDebug("SiuDao.GetSafetyPaysRptTasks", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }
    }

    [WebMethod(EnableSession = true)]
    public string GetSafetyPaysRptTaskStatus(string RptID, string TaskNo)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        try
        {
            List<SIU_SafetyPays_TaskStatus> jasonResp = SqlServer_Impl.GetSafetyPaysTaskStatusRpt(int.Parse(RptID), int.Parse(TaskNo));
            return serializer.Serialize(new { Result = "OK", Records = jasonResp });
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetSafetyPaysRptStatus", RptID + ", " + TaskNo);
            SqlServer_Impl.LogDebug("SiuDao.GetSafetyPaysRptStatus", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }
    }

    [WebMethod(EnableSession = true)]
    public string GetSafetyPays(string RptID)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        try
        {
            SIU_SafetyPaysReport jasonResp = SqlServer_Impl.GetSafetyPaysReport(int.Parse(RptID)).First(); 
            return serializer.Serialize(jasonResp);
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetSafetyPays", RptID);
            SqlServer_Impl.LogDebug("SiuDao.GetSafetyPays", ex.Message);
            return serializer.Serialize(  "ERROR" + ex.Message );
        }
    }
     
#endregion Safety Pays
#region QOM
    [WebMethod(EnableSession = true)]
    public string GetSafetyQomQList()
    {
        ////////////////////////////////
        // Build Response Data Object //
        ////////////////////////////////
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            List<SIU_Safety_MoQ> jasonResp = SqlServer_Impl.GetSafetyQomQList();
            return serializer.Serialize(new { Result = "OK", Records = jasonResp });
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetSafetyQomQList", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }        
    }

    [WebMethod(EnableSession = true)]
    public string GetSafetyQomQRList(string Eid)
    {
        ////////////////////////////////
        // Build Response Data Object //
        ////////////////////////////////
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            List<SIU_Qom_QR> jasonResp = SqlServer_Impl.GetSafetyQomQRList(Eid);
            return serializer.Serialize(new { Result = "OK", Records = jasonResp });
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetSafetyQomQRList", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }
    }

    [WebMethod(EnableSession = true)]
    public string K9Kgjn(string Eid)
    {
        ////////////////////////////////
        // Build Response Data Object //
        ////////////////////////////////
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            List<SIU_Qom_QR> jasonResp = SqlServer_Impl.GetSafetyQomQHList(Eid);
            return serializer.Serialize(new { Result = "OK", Records = jasonResp });
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetSafetyQomQHList", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }
    }




    [WebMethod(EnableSession = true)]
    public object RecordSafetyQomQuestion(string _UID, string _dept, string _start, string _end, string _text, string _file, string _ans)
    {
        try
        {
            SIU_Safety_MoQ moQ = new SIU_Safety_MoQ()
            {
                QuestionGroup = _dept,
                StartDate =  DateTime.Parse(_start),
                EndDate = DateTime.Parse(_end),
                Question = Server.UrlDecode(_text),
                QuestionFile = Server.UrlDecode(_file),
                Q_Id = int.Parse(_UID),
                QuestionAns = Server.UrlDecode(_ans)
            };
            int rId = SqlServer_Impl.RecordSafetyQomQuestion(moQ);
            return new { Result = "OK", Record = rId };
        }

        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("RecordSafetyQomQuestion", ex.Message);
            return new { Result = "ERROR", ex.Message };
        }

    }

    [WebMethod(EnableSession = true)]
    public object RemoveSafetyQomQuestion(string Q_Id)
    {
        //////////////////////////////////////////////
        // Called From JTable So Need JSON Response //
        //////////////////////////////////////////////
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            SqlServer_Impl.RemoveSafetyQomQuestion(Q_Id);
            return serializer.Serialize(new { Result = "OK" });
        }

        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("RemoveSafetyQomQuestion", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }        
    }
#endregion QOM
#region Incident Accident
    [WebMethod(EnableSession = true)]
    public string GetOpenIncidentAccident()
    {
        ////////////////////////////////
        // Build Response Data Object //
        ////////////////////////////////
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            List<object> jasonResp = SqlServer_Impl.GetIncidentAccidentOpen();
            return serializer.Serialize(new { Result = "OK", Records = jasonResp[0] });
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetOpenIncidentAccident", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }         
    }

    [WebMethod(EnableSession = true)]
    public string GetSubmitIncidentAccident()
    {
        ////////////////////////////////
        // Build Response Data Object //
        ////////////////////////////////
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            List<object> jasonResp = SqlServer_Impl.GetIncidentAccidentSubmit();
            return serializer.Serialize(new { Result = "OK", Records = jasonResp[0] });
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetOpenIncidentAccident", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }
    }

    [WebMethod(EnableSession = true)]
    public string GetVehilceList()
    {
        try
        {
            List<string> rpts = SqlServer_Impl.GetVehilceList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(rpts);
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetVehilceList", ex.Message);
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public string GetIncidentAccidentApprovalNotes(string UID)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            List<object> rpts = SqlServer_Impl.GetIncidentAccidentApprovalNotes( int.Parse(UID) );
            return serializer.Serialize(new { Result = "OK", Records = rpts[0]});
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetIncidentAccidentApprovalNotes", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }
    }

    [WebMethod(EnableSession = true)]
    public string GetEmpBasic(string EID)
    {
        try
        {
            SIU_BasicEmployee emp =  new SIU_BasicEmployee( SqlServer_Impl.GetEmployeeByNo(EID) );
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(emp);
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.GetEmpBasic", "(" + EID + ") " +  ex.Message);
        }
        return null;
    }

    [WebMethod(EnableSession = true)]
    public string ReportingChain(string UID)
    {
        try
        {
            SIU_Incident_Accident_Reports_To rt = BusinessLayer.IncidentAccidentReportsToByID( int.Parse(UID) );
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(rt);
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.ReportingChain", ex.Message);
            throw;
        }
    }
    
    
    [WebMethod(EnableSession = true)]
    public string recordIncidentAccident(   string hlblUID,         string _Inc_Type,           string _Inc_Type_Sub,           string _Inc_Loc,                    string _Emp_Veh_Involved, string _Emp_Job_No,
                                            string _Claim_ID,       string _Inc_Desc,           string _Inc_Unsafe_Act_or_Condition, string _Osha_Restrict_Days,    string _Osha_Lost_Days,
                                            string _Emp_ID,         string _Emp_Comments,       string _Follow_Discipline,      string _Follow_Prevent_Reoccur,     string _Follow_Comments,
                                            string _Cost_inHouse,   string _Cost_Incurred,      string _Cost_Reserve,           string _Follow_Responsible,
                                            string _Inc_Occur_Date, string _Osha_Record_Med,    string _Emp_Drug_Alchol_Test,   string _Follow_Discipline_Issued_Flag
                                        )
    {

        try
        {
            SIU_Incident_Accident incRcd = new SIU_Incident_Accident
            {   
                Claim_ID = _Claim_ID,
                Inc_Type = _Inc_Type.TrimEnd(),
                Inc_Type_Sub = _Inc_Type_Sub.TrimEnd(),
                Inc_Desc = _Inc_Desc,
                Inc_Loc = _Inc_Loc.TrimEnd(),
                
                Inc_Unsafe_Act_or_Condition = _Inc_Unsafe_Act_or_Condition,
                Osha_Record_Med = (_Osha_Record_Med == "true"),
                Osha_Restrict_Days =  ((_Osha_Restrict_Days.Length > 0) ? int.Parse(_Osha_Restrict_Days) : 0),
                Osha_Lost_Days =      ((_Osha_Lost_Days.Length > 0) ? int.Parse(_Osha_Lost_Days) : 0),
                Follow_Discipline = _Follow_Discipline,
                Follow_Discipline_Issued_Flag = (_Follow_Discipline_Issued_Flag == "true"),
                Follow_Prevent_Reoccur = _Follow_Prevent_Reoccur,
                Follow_Comments = _Follow_Comments,
                Emp_ID = _Emp_ID.TrimEnd(),
                Emp_Drug_Alchol_Test = (_Emp_Drug_Alchol_Test == "true"),
                Emp_Veh_Involved = _Emp_Veh_Involved.TrimEnd(),
                Emp_Job_No = _Emp_Job_No,
                Emp_Comments = _Emp_Comments,
                Cost_inHouse =  (( _Cost_inHouse.Length > 0 ) ?  decimal.Parse(_Cost_inHouse) : 0),
                Cost_Incurred = ((_Cost_Incurred.Length > 0) ? decimal.Parse(_Cost_Incurred) : 0),
                Cost_Reserve = ((_Cost_Reserve.Length > 0) ? decimal.Parse(_Cost_Reserve) : 0),
                Disposition = "Open",
                UID = 0
            };

            if (hlblUID.Length > 0)
                 incRcd.UID = int.Parse(hlblUID);

            incRcd.Osha_Restrict_Duty = (incRcd.Osha_Restrict_Days > 0);
            incRcd.Osha_Lost_Time = (incRcd.Osha_Lost_Days > 0);
            if (_Inc_Occur_Date.Length >= 10)
                incRcd.Inc_Occur_Date = DateTime.Parse(_Inc_Occur_Date);

            int rcdUid = SqlServer_Impl.recordIncidentAccident(incRcd);

            ////////////////////////////////////////////////////////////
            // Remove Any Existing Approval Records.  I.E. Start Over //
            ////////////////////////////////////////////////////////////
            SqlServer_Impl.removeIncidentApproval(rcdUid);

            return rcdUid.ToString();
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("recordIncidentAccident", ex.Message);
            return ("ERROR " + ex.Message);
        }
    }

    [WebMethod(EnableSession = true)]
    public string submitIncidentAccident(string hlblUID, string _Inc_Type, string _Inc_Type_Sub, string _Inc_Loc, string _Emp_Veh_Involved, string _Emp_Job_No,
                                            string _Claim_ID, string _Inc_Desc, string _Inc_Unsafe_Act_or_Condition, string _Osha_Restrict_Days, string _Osha_Lost_Days,
                                            string _Emp_ID, string _Emp_Comments, string _Follow_Discipline, string _Follow_Prevent_Reoccur, string _Follow_Comments,
                                            string _Cost_inHouse, string _Cost_Incurred, string _Cost_Reserve, string _Follow_Responsible,
                                            string _Inc_Occur_Date, string _Osha_Record_Med, string _Emp_Drug_Alchol_Test, string _Follow_Discipline_Issued_Flag
                                        )
    {

        try
        {
            SIU_Incident_Accident incRcd = new SIU_Incident_Accident
            {
                Claim_ID = _Claim_ID,
                Inc_Type = _Inc_Type,
                Inc_Type_Sub = _Inc_Type_Sub,
                Inc_Desc = _Inc_Desc,
                Inc_Loc = _Inc_Loc,
                Inc_Unsafe_Act_or_Condition = _Inc_Unsafe_Act_or_Condition,
                Osha_Record_Med = (_Osha_Record_Med == "true"),
                Osha_Restrict_Days = int.Parse(_Osha_Restrict_Days),
                Osha_Lost_Days = int.Parse(_Osha_Lost_Days),
                Follow_Discipline = _Follow_Discipline,
                Follow_Discipline_Issued_Flag = (_Follow_Discipline_Issued_Flag == "true"),
                Follow_Prevent_Reoccur = _Follow_Prevent_Reoccur,
                Follow_Comments = _Follow_Comments,
                Emp_ID = _Emp_ID,
                Emp_Drug_Alchol_Test = (_Emp_Drug_Alchol_Test == "true"),
                Emp_Veh_Involved = _Emp_Veh_Involved,
                Emp_Job_No = _Emp_Job_No,
                Emp_Comments = _Emp_Comments,
                Cost_inHouse = decimal.Parse(_Cost_inHouse),
                Cost_Incurred = decimal.Parse(_Cost_Incurred),
                Cost_Reserve = decimal.Parse(_Cost_Reserve),
                Disposition = "Submit",
                UID = 0
            };

            if (hlblUID.Length > 0)
                incRcd.UID = int.Parse(hlblUID);

            incRcd.Osha_Restrict_Duty = (incRcd.Osha_Restrict_Days > 0);
            incRcd.Osha_Lost_Time = (incRcd.Osha_Lost_Days > 0);
            if (_Inc_Occur_Date.Length >= 10)
                incRcd.Inc_Occur_Date = DateTime.Parse(_Inc_Occur_Date);

            /////////////////////////////////////////////////
            // Save or Update The Event With SUBMIT Status //
            /////////////////////////////////////////////////
            int rcdUid  =  SqlServer_Impl.recordIncidentAccident(incRcd);

            ////////////////////////////////////////////////////////////
            // Remove Any Existing Approval Records.  I.E. Start Over //
            ////////////////////////////////////////////////////////////
            SqlServer_Impl.removeIncidentApproval(rcdUid);

            ///////////////////////////////////
            // Send Emails To Approval Chain //
            ///////////////////////////////////
            SIU_Incident_Accident_Reports_To rt = BusinessLayer.IncidentAccidentReportsToByID(incRcd.UID);
            WebMail.AccidentIncidentSubmitted(rt, incRcd);


            //////////
            // Done //
            //////////
            return rcdUid.ToString(CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("submitIncidentAccident", ex.Message);
            return ("ERROR " + ex.Message);
        }
    }


    [WebMethod(EnableSession = true)]
    public string approveIncidentAccident(string hlblUID)
    {

        try
        {

            int UID = int.Parse(hlblUID);

            /////////////////////
            // Record Approval //
            /////////////////////
            SqlServer_Impl.recordIncidentApproval( UID );

            ///////////////////////////////
            // Send Email About Approval //
            ///////////////////////////////
            SIU_Incident_Accident incRcd =  SqlServer_Impl.GetIncidentAccident(UID);
            SIU_Incident_Accident_Reports_To rt = BusinessLayer.IncidentAccidentReportsToByID(UID);
            WebMail.AccidentIncidentApproved(rt, incRcd);

            ///////////////////////////////////////////////
            // Check If All Approvals Have Been Received //
            ///////////////////////////////////////////////
            if ( rt.readyToClose ) {
                incRcd.Disposition = "Closed";
                SqlServer_Impl.recordIncidentAccident(incRcd);

                WebMail.AccidentIncidentClosed(rt, incRcd);
            }
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("SiuDao.approveIncidentAccident", ex.Message);
            throw;
        }

        return "OK";
    }

    [WebMethod(EnableSession = true)]
    public string RecordIncidentAccidentApprovalNote(string UID, string Note)
    {
        try
        {

            SIU_Incident_Accident_AppovalNote newNote = new SIU_Incident_Accident_AppovalNote()
            {
                Comments = Note,
                EID = BusinessLayer.UserEmpID,
                Ref_UID = int.Parse(UID),
                TimeStamp = DateTime.Now
            };

            SqlServer_Impl.RecordIncidentAccidentApprovalNote(ref newNote);

            ////////////////////////////////////////////////////////////
            // Remove Any Existing Approval Records.  I.E. Start Over //
            ////////////////////////////////////////////////////////////
            SqlServer_Impl.removeIncidentApproval(newNote.Ref_UID);

            ///////////////////////////////////
            // Send Emails To Approval Chain //
            ///////////////////////////////////
            var incRcd = SqlServer_Impl.GetIncidentAccident(newNote.Ref_UID);
            SIU_Incident_Accident_Reports_To rt = BusinessLayer.IncidentAccidentReportsToByID(incRcd.UID);
            WebMail.AccidentIncidentCommented(rt, incRcd, Note);

            JavaScriptSerializer serializer = new JavaScriptSerializer();            
            return serializer.Serialize(new { Result = "OK", Records = newNote});
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("RecordIncidentAccidentApprovalNote", ex.Message);
            return ("ERROR " + ex.Message);
        }
    }

#endregion Incident Accident

#region Training

    /////////////////////////////////////////////////////
    // Gets List Of Future Meetings For Administration //
    /////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetTrainingRcd(string rcdId)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            SIU_Meeting_Log_Ext rpt = SqlServer_Impl.GetTrainingLog( int.Parse(BusinessLayer.UserEmpID),  int.Parse(rcdId) );
            return serializer.Serialize(rpt);
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("GetTrainingRcd", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }

    }

    /////////////////////////////////////////////////////
    // Gets List Of Future Meetings For Administration //
    /////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetMeetingLogAdmin(string SD)
    {
        DateTime startDate = DateTime.Now;
        if (SD.Length > 0)
            startDate = DateTime.Parse(SD);

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            List<SIU_Training_Log> rpt = SqlServer_Impl.GetMeetingLogAdmin(startDate);
            return serializer.Serialize(new { Result = "OK", Records = rpt });            
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("GetMeetingLogAdmin", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }

    }

    [WebMethod(EnableSession = true)]
    public string GetMeetingLogUser(string Eid)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            List<SIU_Meeting_Log_Ext> rpt = SqlServer_Impl.GetMeetingLogUser(int.Parse(Eid));
            return serializer.Serialize(new { Result = "OK", Records = rpt });
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("GetMeetingLogAdmin", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }

    }


    [WebMethod(EnableSession = true)]
    public string RecordMeetingLogAdmin(string MeetingID, string cTopic, string cType, string cPts, string cDesc, string cInstID, string cVideo, string cQuiz, string cDate, string cStart, string cStop, string cLoc, string cReq)
    {
        try
        {
            SIU_Training_Log mtgLog = new SIU_Training_Log
            {   
                TL_UID = int.Parse(MeetingID),
                Topic = HttpUtility.UrlDecode(cTopic),
                Description = HttpUtility.UrlDecode(cDesc),
                InstructorID = int.Parse(cInstID),
                Location = HttpUtility.UrlDecode(cLoc),
                MeetingType = int.Parse(cType),
                Points = int.Parse(cPts),
                VideoFile = HttpUtility.UrlDecode(cVideo),
                QuizName = HttpUtility.UrlDecode(cQuiz)
            };

            var emp = SqlServer_Impl.GetEmployeeByNo(cInstID);
            mtgLog.Instructor = (emp != null) ? emp.Last_Name + ", " + emp.First_Name : "";
            
            if (cReq.Length > 0) mtgLog.PreReq = int.Parse(cReq);

            mtgLog.Date = DateTime.Parse(cDate.Length > 0 ? cDate : "1/1/2999 00:00");

            string mtgDate = mtgLog.Date.Value.ToShortDateString();

            cStart = mtgDate + " " + cStart;
            cStop = mtgDate + " " + cStop;

            mtgLog.StartTime = DateTime.Parse(cStart.Length > 0 ? cStart : "1/1/1753 00:00");
            mtgLog.StopTime = DateTime.Parse(cStop.Length > 0 ? cStop : "1/1/1753 00:00");

            return SqlServer_Impl.RecordMeetingLogAdmin(mtgLog);
        }
        catch (Exception ex )
        {
            SqlServer_Impl.LogDebug("RecordMeetingLogAdmin", ex.Message);
            return ("ERROR " + ex.Message );
        }
    }

    /////////////////////////////////////////////////////
    // Gets List Of Future Meetings For Administration //
    /////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string RemoveMeetingLogAdmin(int TL_UID)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            string msg = SqlServer_Impl.RemoveMeetingLogAdmin(TL_UID);
            return ((msg == "") ? serializer.Serialize(new { Result = "OK" }) : serializer.Serialize(new { Result = "ERROR", msg }));
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("RemoveMeetingLogAdmin", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }

    }

    /////////////////////////////////////////////////
    // Get Qualification Associated With a Meeting //
    /////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetMeetingQual(int MeetingID)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            List<SIU_Training_Log_Certification> rpt = SqlServer_Impl.GetMeetingQual(MeetingID);
            return serializer.Serialize(new { Result = "OK", Records = rpt });            
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("GetMeetingQual", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }

    }

    /////////////////////////////////////////////////
    // Add Qualification Associated With a Meeting //
    /////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string RecordMeetingQual(string MeetingID, string Code)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            SIU_Training_Log_Certification cert = new SIU_Training_Log_Certification
            {
                QualCode = Code,
                TL_UID = int.Parse(MeetingID),
                TLC_UID = 0
            };
            string msg = SqlServer_Impl.RecordMeetingQual(cert);

            return ((msg == "") ? serializer.Serialize(new { Result = "OK" }) :  serializer.Serialize(new { Result = "ERROR", msg }));
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("RecordMeetingQual", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }

    }

    /////////////////////////////////////////////////
    // Add Qualification Associated With a Meeting //
    /////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string RemoveMeetingQual(int TLC_UID)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            ///////////////////////////////////////////////////////////////////////
            // If the User Deletes A Certification Not Yet Added To the Database //
            // Just Say OK                                                       //
            ///////////////////////////////////////////////////////////////////////
            if (TLC_UID == 0)
                return serializer.Serialize(new { Result = "OK" });

            ///////////////////////////////////////////////////////
            // Otherwise Remove The Certification From The Class //
            ///////////////////////////////////////////////////////
            string msg = SqlServer_Impl.RemoveMeetingQual(TLC_UID);
            return ((msg == "") ? serializer.Serialize(new { Result = "OK" }) : serializer.Serialize(new { Result = "ERROR", msg }));
        }

        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("RemoveMeetingQual", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }

    }

    /////////////////////////////////////////////////////////////////
    // Supply List Of Safety Pays Point Types For AutoComplete Box //
    /////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetAutoCompleteClassTypes(string T)
    {
        string typeList = string.Empty;

        foreach (var pt in SqlServer_Impl.GetAutoCompleteClassTypes())
            typeList += pt.UID + " - " + pt.Description + "," + pt.PointsCount + "\r";

        return typeList;
    }

    ////////////////////////////
    // Record Movie Completed //
    ////////////////////////////
    [WebMethod(EnableSession = true)]
    public string TrainingMovieComplete(string empNo, string TL_UID)
    {
        return SqlServer_Impl.TrainingMovieComplete(empNo, TL_UID);
    }
#endregion Training

#region Document and Video Browsing
    ////////////////////////////////////
    // Provide Video File Directories //
    ////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string ListDocumentDirectories(string VirtualDirectory, string SubDirectory)
    {
        string virtualRoot = HttpContext.Current.Server.MapPath(VirtualDirectory);

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(PopupMenuSupport.ListDirectories(virtualRoot, SubDirectory, @"\Images\LibraryFolder.png", @"\Images\LibraryFolder-hover.png"));
    }

    ///////////////////////////////////////
    // Provide Document File Directories //
    ///////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string ListVideoDirectories(string VirtualDirectory, string SubDirectory)
    {
        string virtualRoot = HttpContext.Current.Server.MapPath(VirtualDirectory);

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string folders = serializer.Serialize(PopupMenuSupport.ListDirectories(virtualRoot, SubDirectory, @"\Images\VideoFolder2.png", @"\Images\VideoFolder2-Hover.png"));

        return folders;
    }

    ////////////////////////////////
    // Provide Library Files List //
    ////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string ListFiles(string VirtualDirectory, string SubDirectory)
    {
        string virtualRoot = HttpContext.Current.Server.MapPath(VirtualDirectory);

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string files = serializer.Serialize(PopupMenuSupport.ListFiles(virtualRoot, SubDirectory, VirtualDirectory));

        return files;
    }

    [WebMethod(EnableSession = true)]
    public string PhoneDocumentList(string VirtualDirectory, string SubDirectory)
    {
        string virtualRoot = HttpContext.Current.Server.MapPath(VirtualDirectory);

        string[] pathParts = SubDirectory.Split('/');
        string backPathStr = "";
        for (int partIdx = 0; partIdx < pathParts.Length - 1; partIdx++)
        {
            if (pathParts[partIdx].Length > 0)
                backPathStr += "/" + pathParts[partIdx];
        }
        if (backPathStr.Length == 0)
            for (int partIdx = 0; partIdx < pathParts.Length; partIdx++)
            {
                if (pathParts[partIdx].Length > 0)
                    backPathStr += "/" + pathParts[partIdx];
            }
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { BackPath = backPathStr, Docs = PopupMenuSupport.PhoneDocumentList(virtualRoot, SubDirectory, VirtualDirectory) });
    }
    

    ////////////////////////////////
    // Provide Library Files List //
    ////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string ListVideos(string VirtualDirectory, string SubDirectory)
    {
        string virtualRoot = HttpContext.Current.Server.MapPath(VirtualDirectory);

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string videos = serializer.Serialize(PopupMenuSupport.ListVideos(virtualRoot, SubDirectory, VirtualDirectory));

        return videos;
    }

    ////////////////////////////////////
    // Provide Video File Directories //
    ////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string ListVideoSupportDocuments(string Path, string File)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string supportDocuments = serializer.Serialize(PopupMenuSupport.Create_Injectable_Video_Support_Documents(Path, File));
        
        return supportDocuments;
    }

    //////////////////////////////
    // Provide Video Files List //
    //////////////////////////////
    [WebMethod(EnableSession = true)]
    public string ListVideoFiles(string path)
    {
        string videoRoot = HttpContext.Current.Server.MapPath("/Videos");

        if (path.Length > 0)
        {
            ///////////////////
            // Unmangle Path //
            ///////////////////
            path = path.Replace("**", "/");
            path = (new DirectoryInfo(path)).FullName;

            ////////////////////////////////////////////////////////////
            // Strip Physical Path Which Would Duplicate Virtual Path //
            ////////////////////////////////////////////////////////////
            if (path.Length > videoRoot.Length + 2)
                if (path.Substring(0, videoRoot.Length) == videoRoot)
                    path = path.Substring(videoRoot.Length + 1);
        }

        JavaScriptSerializer serializer = new JavaScriptSerializer();

        string files = serializer.Serialize(PopupMenuSupport.ListFiles("/Videos", path, "/Videos"));

        return files;
    }
#endregion Document and Video Browsing

#region Record Video Watching Event
    //////////////////////////////////////////////
    // Record Movie Watched OR Position Changed //
    //////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string MovieStart(string empNo, string movieId, string pos, string dur)
    {
        string videoFolder;
        string videoName;

        int splitIdx = movieId.LastIndexOf('/');

        if (splitIdx > 0)
        {
            videoName = movieId.Substring(splitIdx + 1);
            videoFolder = movieId.Substring(0, splitIdx);
        }
        else
        {
            videoName = movieId;
            videoFolder = "";
        }


        if (videoName[videoName.Length - 4] == '.')
            videoName = videoName.Substring(0, videoName.Length - 4);

        SqlServer_Impl.LogVideoWatch(empNo, videoName, videoFolder, pos, dur);

        return "";
    }

    ////////////////////////////
    // Record Movie Completed //
    ////////////////////////////
    [WebMethod(EnableSession = true)]
    public string MovieComplete(string empNo, string movieId, string pos, string dur)
    {
        int splitIdx = movieId.LastIndexOf('/');

        string videoName = movieId.Substring(splitIdx + 1);
        if (videoName[videoName.Length - 4] == '.')
            videoName = videoName.Substring(0, videoName.Length - 4);

        string videoFolder = movieId.Substring(0, splitIdx);

        SqlServer_Impl.LogVideoWatch(empNo, videoName, videoFolder, pos, dur, true);
        WebMail.MovieCompletedEMail(empNo, videoName);

        return "";
    }


#endregion Record Video Watching Event

#region MySi Reporting
    ////////////////////////////////////////////////////////////////////////
    // Run SP That Compiles Stats, Warning, and Issues For Given Employee //
    ////////////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetMySiSummaryCounts(string EmpID)
    {
        SIU_MySi_SummaryCountsResult rpt = SqlServer_Impl.GetMySiSummaryCounts(EmpID);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(rpt);        
    }

    /////////////////////////////////////////////////////////////////////////////
    // Get List Of Badges and Certifications Either Expired or About to Expire //
    /////////////////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetMyExpiringBandC(string EmpID)
    {
        List<BandC_DistinctByEmployee> rpt = SqlServer_Impl.GetMyExpiringBandC(EmpID);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { Result = "OK", Records = rpt });
    }

    //////////////////////////////////////////////////////////////
    // Get A List Of All Badges and Certifications For Employee //
    //////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public List<BandC_DistinctByEmployee> GetMyAllBandC(string EmpID, bool _search, long nd, int rows, int page, string sidx, string sord)
    {
        
        return SqlServer_Impl.GetMyAllBandC(EmpID).ToList();
        //sord = ((sidx.Length == 0) ? "category" : sidx) + " " + sord.ToUpper();
        //return SqlServer_Impl.GetMyAllBandC(EmpID).OrderBy(sord).ToList();
    }

    /////////////////////////////////////////////////////////
    // Get A List Of Missed Safety Classes For An Employee //
    /////////////////////////////////////////////////////////
    //[WebMethod(EnableSession = true)]
    //public List<SIU_MySi_MissedSafetyMeeting> GetMyMissedSafetyClasses(string EmpID, bool _search, long nd, int rows, int page, string sidx, string sord)
    //{
    //    return SqlServer_Impl.GetMyMissedSafetyClasses(EmpID).ToList();
    //}

    /////////////////////////////////////////////////////////////////////////////
    // Get List Of Badges and Certifications Either Expired or About to Expire //
    /////////////////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetMyOpenHwReq(string EmpID)
    {
        List<SIU_IT_HW_Req> rpt = SqlServer_Impl.GetMyOpenHwReq(EmpID);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { Result = "OK", Records = rpt });
    }

    /////////////////////////////////////////////////////////////////////////////
    // Get List Of Badges and Certifications Either Expired or About to Expire //
    /////////////////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetMyOpenBugs(string EmpID)
    {
        List<BugReport_Report> rpt = SqlServer_Impl.GetMyOpenBugs(EmpID);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { Result = "OK", Records = rpt });
    }

    ///////////////////////////////////////////////
    // Get List Of JOb Reports Still In Progress //
    ///////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetMyOpenJobRptsSum(string EmpID)
    {
        List<InProgressJobReport> rpt = SqlServer_Impl.GetMyOpenJobRptsSum(EmpID);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { Result = "OK", Records = rpt });
    }

    ///////////////////////////////////////////////
    // Get List Of JOb Reports Still In Progress //
    ///////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetMyPastDueJobRptsSum(string EmpID)
    {
        List<PastDueJobReport> rpt = SqlServer_Impl.GetMyPastDueJobRptsSum(EmpID);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { Result = "OK", Records = rpt });
    }

    /////////////////////////////////////////////////////////////////////
    // Lookup Vehicle Mileage For An Employee For A Week For A Vehicle //
    /////////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetMyVehicleMileageRpt(string EmpID)
    {
        List<Shermco_Assigend_Vehicle_Data> rpt = SqlServer_Impl.GetMyVehicleMileageRpt(EmpID);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { Result = "OK", Records = rpt });
    }

    //////////////////////////////////
    // Lookup Expenses For Employee //
    //////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string hhtl0(string EmpID)
    {
        List<Shermco_Employee_Expense> rpt = SqlServer_Impl.GetMyExpenses(EmpID);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { Result = "OK", Records = rpt });
    }

    //////////////////////////////////////////
    // Provide Data For YTD Expenses Report //
    //////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public List<SIU_YTD_Exp_Rpt> adf1a(string EmpID, bool LY,  bool _search, long nd, int rows, int page, string sidx, string sord)
    {
        sord = ((sidx.Length == 0) ? "WorkDate" : sidx) + " " + sord.ToUpper();
        return SqlServer_Impl.GetMyYtdExpenses(EmpID, LY).OrderBy(sord).ToList();
    }
#endregion MySi Reporting

#region ELO Expense Reporting
    //////////////////////////////////
    // Lookup Expenses For Employee //
    //////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetMealRates()
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            List<SIU_ELO_Meal_Opts.SIU_ELO_Meal_Opt> rpt = new SIU_ELO_Meal_Opts().GetList();
            return serializer.Serialize( rpt );
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("GetMealRates", ex.Message);
            return serializer.Serialize(new { Result = "ERROR", ex.Message });
        }
    }

    ///////////////////////////////////////
    // Save Miles and Meals Expense Item //
    ///////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string MileExpSubmit(string empNo, string workDate, string JobNo, string OhAcct, string Miles, string Meals, string Amount)
    {
        Shermco_Employee_Expense expRpt = SqlDataMapper<Shermco_Employee_Expense>.MakeNewDAO<Shermco_Employee_Expense>();

        if (Miles.Length == 0) Miles = "0";
        if (Meals.Length == 0) Meals = "0";
        if (JobNo.Length > 0) OhAcct = "2005";             // ??????????????????

        decimal dMiles =  Convert.ToDecimal(Miles);
        int iMiles = Convert.ToInt32(dMiles);

        expRpt.Employee_No_ = empNo;
        expRpt.Work_Date =   DateTime.Parse(workDate);
        expRpt.Job_No_ = JobNo;
        expRpt.O_H_Account_No_ = OhAcct;
        expRpt.Mileage = iMiles;
        expRpt.Meals = int.Parse(Meals);
        expRpt.Amount = decimal.Parse(Amount);

        return SqlServer_Impl.RecordExpense(expRpt);
    }

    ////////////////////////////////
    // Remove An Unposted Expense //
    ////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string zz0p(int Line_No_)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        try
        {
            SqlServer_Impl.DeleteMyExpense(Line_No_);
            return serializer.Serialize(new { Result = "OK" });
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("DeleteMyExpense", ex.Message);
            return serializer.Serialize( new { Result = "ERROR", ex.Message } );
        }
    }
#endregion ELO Expense Reporting

#region ItHwReq
    ////////////////////////////////
    // Remove An Unposted Expense //
    ////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetPriceForHardware(string Computer, string MonitorCnt, string StandCnt, bool chkCase, bool chkDock, bool chkBackPack, bool chkAdobe, bool chkCAD, bool chkMsPrj, bool chkVisio)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string priceEst = BusinessLayer.CalcHardwarePrice(Computer, MonitorCnt, StandCnt, chkCase,
                                                                 chkDock, chkBackPack, chkAdobe, chkCAD,
                                                                 chkMsPrj, chkVisio);
        return serializer.Serialize(priceEst);
    }
#endregion ItHwReq

#region Blogs
    ////////////////////////////////////
    // Return List Of Items In A Blog //
    ////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetBlogRecords(string BlogName, string jtSorting)
    {
        List<SIU_Blog> blogToc = SqlServer_Impl.GetBlogs(BlogName).ToList();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(new { Result = "OK", Records = blogToc });
    }


    //////////////////////////
    // Return List Of Blogs //
    //////////////////////////
    [WebMethod(EnableSession = true)]
    public string ListBlogs()
    {
        List<string> blogToc = SqlServer_Impl.ListBlogs();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(blogToc);
    }


    //////////////////////////
    // Return List Of Blogs //
    //////////////////////////
    [WebMethod(EnableSession = true)]
    public string GetMarquee(string BlogName)
    {
        string blogListItems = Blogs.Create_Injectable_Ads("/Advertisements", BlogName);
        //JavaScriptSerializer serializer = new JavaScriptSerializer();
        return blogListItems;
    }
#endregion Blogs




    ////////////////////////////////////////////////////////////////////
    // Get Data To Build Map Of Electrical Engineer Licenses By State //
    ////////////////////////////////////////////////////////////////////
    [WebMethod(EnableSession = true)]
    public string ESD_License_Sum()
    {
        List<BandC_License_Sum> rpt = SqlServer_Impl.ESD_License_Sum();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize (rpt);        
    }
}








public class SqlServer_Impl : WebService
{
    
#region Database Connection
    public static string SqlServerProdNvdbConnectString
    {
        get
        {
            ////////////////////////////////////////////
            // Get the application configuration file //
            ////////////////////////////////////////////
            Configuration config = WebConfigurationManager.OpenWebConfiguration("/");

            ///////////////////////////////////////
            // Get The Sql Server Connect STring //
            ///////////////////////////////////////
            string connectStringName = "ShermcoConnectionString";

            DeveloperConfigurationSection configSection = (DeveloperConfigurationSection)ConfigurationManager.GetSection(DeveloperConfigurationSection.ConfigurationSectionName);

            if ( configSection.Enabled )
                connectStringName = configSection.UseConnectString;


            ConnectionStringsSection csSection = config.ConnectionStrings;
            return csSection.ConnectionStrings[connectStringName].ConnectionString;            
        }
    }
    public static bool isProdDB
    {
        get { return SqlServerProdNvdbConnectString.Contains("si-nav"); }
    }
    public static string ForcedProductionConnectString
    {
        get
        {
            const string connectStringName = "ShermcoConnectionString";

            Configuration config = WebConfigurationManager.OpenWebConfiguration("/");
            ConnectionStringsSection csSection = config.ConnectionStrings;
            return csSection.ConnectionStrings[connectStringName].ConnectionString;
        }
    }
#endregion

#region Logon

    public static SIU_LogonProbeRcd LogonProbe(string UserID, string Pwd, string IP, int Valid)
    {
        SIU_LogonProbeRcd logonResp = new SIU_LogonProbeRcd();

        try
        {
            logonResp.Valid = Valid;
            logonResp.Mail = 0;
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);
            nvDb.SIU_Logon(UserID, Pwd, IP, ref logonResp.Valid, ref logonResp.Mail);
        }
        catch (Exception ex)
        {
            LogDebug("SIU_LogonProbeRcd", ex.Message);
            logonResp.Valid = -1;
            logonResp.Mail = 1;
        }
        return logonResp;
    }
    public static List<string> SIU_GetRoles(string UserID)
    {
        List<string> RolesList = new List<string> { };

        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);
            RolesList = (from rcds in nvDb.SIU_Roles
                         where rcds.UserID == UserID
                         select rcds.RoleID
                        ).ToList();
        }
        catch (Exception ex)
        {
            LogDebug("SIU_GetRoles", ex.Message);
        }

        return RolesList;
    }
#endregion Logon

#region Debug
    public  static void LogDebug(string Module, string Msg)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        SIU_debug debugRcd = new SIU_debug
        {
            Module = Module,
            Data = Msg,
            Server = Environment.MachineName,
            UserName = BusinessLayer.UserFullName,
            TS = DateTime.Now
        };


        nvDb.SIU_debugs.InsertOnSubmit(debugRcd);
        nvDb.SubmitChanges();
    }
#endregion

#region NTFS Cacheing
    public static void RecordCache(string PhyPath, string Markup, string MarkupType,  string VirtualDirectory = "",  int MenuID = -1, bool ListOnly = false)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        if (Markup.Length == 0) return;
        if (Environment.MachineName.ToLower() == "tsdc-dev") return;

        if (GetCache(MarkupType, PhyPath, VirtualDirectory, MenuID, ListOnly).Length == 0)
        {
            //LogDebug("RecordCache", "MUType: " + MarkupType + " Phy: " + PhyPath + " Virt: " + VirtualDirectory + " Menu: " + MenuID + " LO: " + ListOnly);

            SIU_NTFS_Cache cacheRcd = new SIU_NTFS_Cache
            {
                TimeStamp = DateTime.Now,
                Server = Environment.MachineName,
                PhysicalPath = PhyPath,
                VirtualDirectory = VirtualDirectory,
                MenuID = MenuID,
                ListOnly = ListOnly,
                Markup = Markup,
                MarkupContentType = MarkupType
            };

            nvDb.SIU_NTFS_Caches.InsertOnSubmit(cacheRcd);
            nvDb.SubmitChanges();
        }
    }
    public static string GetCache(string MarkupType, string PhyPath, string VirtualDirectory = "", int MenuID = -1, bool ListOnly = false)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        var rcd = (from cacheRcd in nvDb.SIU_NTFS_Caches
                              where
                                  cacheRcd.PhysicalPath == PhyPath && cacheRcd.VirtualDirectory == VirtualDirectory &&
                                  cacheRcd.MenuID == MenuID && cacheRcd.ListOnly == ListOnly &&
                                  cacheRcd.MarkupContentType == MarkupType &&
                                  cacheRcd.Server == Environment.MachineName
                              select cacheRcd);

        if ( ! rcd.Any() ) return "";

        if (rcd.Count() > 1)
        {
            LogDebug("GetCache", "Duplicate Cache Rcd For MUType: " + MarkupType + " Phy: " + PhyPath + " Virt: " + VirtualDirectory + " Menu: " + MenuID + " LO: " + ListOnly);
            foreach (var delRcd in rcd)
                RemoveCache(delRcd);
            return "";
        }

        if ( rcd.First().TimeStamp.AddHours(1) <   DateTime.Now)
        {
            RemoveCache(rcd.First());
            return "";
        }
        return rcd.First().Markup;
    }
    public static void RemoveCache(SIU_NTFS_Cache CacheRcd)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        nvDb.SIU_NTFS_Caches.Attach(CacheRcd);
        nvDb.SIU_NTFS_Caches.DeleteOnSubmit(CacheRcd);
        nvDb.SubmitChanges();
    }
#endregion

#region Web Login Lookups
    public static bool CreateUser_Initial(string Email, string Password)
    {
        SqlCommand cmd = new SqlCommand();

        const string sql = "INSERT INTO SIU_USERS_OUTSIDE(Email, Pwd) VALUES (@Email, @Password) ";



        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;

            SqlParameter param = new SqlParameter("@Email", SqlDbType.VarChar) {Value = Email};
            cmd.Parameters.Add(param);

            param = new SqlParameter("@Password", SqlDbType.NVarChar) {Value = Crypto.Encrypt(Password)};
            cmd.Parameters.Add(param);

            cmd.Connection = new SqlConnection(SqlServerProdNvdbConnectString);
            cmd.Connection.Open();

            cmd.ExecuteNonQuery();
        }

        catch
        {
            return false;
        }

        finally
        {
            if (cmd.Connection.State != ConnectionState.Closed)
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
            }
        }

        return true;

    }
    public static LogonRcd Validate_User(string Email, string Password)
    {
        DataSet ds = new DataSet();

        const string sql = "Select First, Last, Phone, Email, LastUsed FROM SIU_USERS_OUTSIDE WHERE EMAIL = @Email and  Pwd = @Password ";


        SqlCommand cmd = new SqlCommand(sql) {Connection = new SqlConnection(SqlServerProdNvdbConnectString)};

        SqlParameter param = new SqlParameter("@Email", SqlDbType.VarChar) {Value = Email};
        cmd.Parameters.Add(param);

        param = new SqlParameter("@Password", SqlDbType.NVarChar) {Value = Crypto.Encrypt(Password)};
        cmd.Parameters.Add(param);

        SqlDataAdapter da = new SqlDataAdapter(cmd);

        da.Fill(ds, "SIU_USERS_OUTSIDE");

        LogonRcd rcd = new LogonRcd(ds);

        return rcd;
    }
#endregion

#region  Safety Training Methods
    public static SIU_Training_Log GetMeetingLogAdmin(int rcdId)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from mtglist in nvDb.SIU_Training_Logs
                        where mtglist.TL_UID == rcdId
                        select mtglist
                    ).SingleOrDefault();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetMeetingLogAdmin: " + rcdId, ex.Message);
        }
        return new SIU_Training_Log();
    }
    public static List<SIU_Training_Log> GetMeetingLogAdmin(DateTime SD)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from mtglist in nvDb.SIU_Training_Logs
                        where mtglist.Date >= SD
                        select mtglist
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetMeetingLogAdmin", ex.Message);
        }
        return new List<SIU_Training_Log>();        
    }

    public static List<SIU_Meeting_Log_Ext> GetMeetingLogUser(int UserID)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                //////////////////////////////////////
                // Build List Of Events In Progress //
                //////////////////////////////////////
                var eventsInProgress =
                    (    from tl in nvDb.SIU_Training_Logs
                            join ta in nvDb.SIU_Training_Attendances on tl.TL_UID equals ta.TL_UID 
                            into taExt                                   
	                    from subset in taExt.DefaultIfEmpty()
                        where (tl.QuizName.Length > 0 || tl.VideoFile.Length > 0) && (subset.EmpID == UserID) &&
                                (subset.VideoEnd == null || subset.QuizPass != true || subset.QuizPass == null)
                        orderby tl.Date descending
                        select new SIU_Meeting_Log_Ext
                        {
                            TL_UID = tl.TL_UID,
                            Date = tl.Date,
                            Topic = tl.Topic,
                            Description = tl.Description,
                            Instructor = tl.Instructor,
                            MeetingType = tl.MeetingType,
                            Location = tl.Location,
                            Points = tl.Points,
                            VideoFile = tl.VideoFile,
                            QuizName = tl.QuizName,
                            StartTime = tl.StartTime,
                            StopTime = tl.StopTime,
                            InstructorID = tl.InstructorID,
                            VideoComplete = (subset.VideoEnd != null),
                            QuizComplete = TriIntFromBool(subset.QuizPass),
                            QuizPass = subset.QuizPass,
                            PreReq = tl.PreReq
                        }
                    );

                /////////////////////////////////////
                // Build List Of Completed Classes //
                /////////////////////////////////////
                List<int> listCompleteClasses =
                    (   from tl in nvDb.SIU_Training_Logs
                         join ta in nvDb.SIU_Training_Attendances on tl.TL_UID equals ta.TL_UID
                         into taExt
                         from subset in taExt.DefaultIfEmpty()
                         where (subset.EmpID == UserID) &&
                         (
                            ((tl.VideoFile.Length > 0 ) ? ((subset.VideoEnd != null)? true : false) : true) &&
                            ((tl.QuizName.Length > 0 ) ? ((subset.QuizPass == true)? true : false) : true)
                         )
                         select tl.TL_UID
                    ).ToList();


                ///////////////////////////////////////
                // Simple List Of In Progess Classes //
                ///////////////////////////////////////
                List<int> listInProgressClasses =
                    (from eip in eventsInProgress
                     select eip.TL_UID
                     ).ToList();



                ///////////////////////////////////////////
                // Build List Of Classes Not Yet Started //
                ///////////////////////////////////////////
                var eventsNotStarted =
                   (    from tl in nvDb.SIU_Training_Logs
                        where !listInProgressClasses.Contains(tl.TL_UID) && !listCompleteClasses.Contains(tl.TL_UID)
                        select new SIU_Meeting_Log_Ext
                        {
                            TL_UID = tl.TL_UID,
                            Date = tl.Date,
                            Topic = tl.Topic,
                            Description = tl.Description,
                            Instructor = tl.Instructor,
                            MeetingType = tl.MeetingType,
                            Location = tl.Location,
                            Points = tl.Points,
                            VideoFile = tl.VideoFile,
                            QuizName = tl.QuizName,
                            StartTime = tl.StartTime,
                            StopTime = tl.StopTime,
                            InstructorID = tl.InstructorID,
                            VideoComplete = false,
                            QuizComplete = -1,
                            QuizPass = false,
                            PreReq = tl.PreReq
                        }
                   );



                //////////////////////////////////////////////////////////////////////////////////////////
                // Get List Of badges And Certifications For This User That have Or Are About To Expire //
                //////////////////////////////////////////////////////////////////////////////////////////
                List<string> expiredExpiringQual =
                    (   from qc in nvDb.BandC_DistinctByEmployees
                        where qc.No_== UserID.ToString(CultureInfo.InvariantCulture) && ( qc.IsMissing == 1 || qc.To_Date > DateTime.Now.AddDays(-90) )
                        select qc.Qualification_Code
                    ).ToList();


                /////////////////////////////////////////////////////////////////////////////////////////////////
                // From List Of Expiring Badges And Certifications, Build List Of Events That Need To be Taken //
                /////////////////////////////////////////////////////////////////////////////////////////////////
                var needToCertify =
                   (    from tl in nvDb.SIU_Training_Logs
                                join tlc in nvDb.SIU_Training_Log_Certifications on tl.TL_UID equals tlc.TL_UID
                        into taExt
                        from subset in taExt.DefaultIfEmpty()
                            where expiredExpiringQual.Contains(subset.QualCode)  
                        orderby tl.Date descending
                        select new SIU_Meeting_Log_Ext
                        {
                            TL_UID = tl.TL_UID,
                            Date = tl.Date,
                            Topic = tl.Topic,
                            Description = tl.Description,
                            Instructor = tl.Instructor,
                            MeetingType = tl.MeetingType,
                            Location = tl.Location,
                            Points = tl.Points,
                            VideoFile = tl.VideoFile,
                            QuizName = tl.QuizName,
                            StartTime = tl.StartTime,
                            StopTime = tl.StopTime,
                            InstructorID = tl.InstructorID,
                            VideoComplete = false,
                            QuizComplete = -1,
                            QuizPass = false,
                            PreReq = tl.PreReq
                        }
                   );




                ///////////////////////////////
                // Build Full List Of Events //
                ///////////////////////////////
                List<SIU_Meeting_Log_Ext> allClassesList = eventsInProgress.ToList();
                allClassesList.AddRange(eventsNotStarted.ToList());
                allClassesList.AddRange(needToCertify.ToList());


                ///////////////////////////////////////////////////
                // For Events Added Because Certification Needed /
                // Add In All Pre-Requisit Classes               //
                ///////////////////////////////////////////////////
                foreach (var certItem in needToCertify.Where(c => c.PreReq != null)   )
                {
                    ////////////////////////////////////////////////////////////
                    // Is THis PreReq Already In The Master List Of Events ?? //
                    ////////////////////////////////////////////////////////////
                    var preReqRcd = (from l3 in allClassesList
                                     where l3.TL_UID == certItem.PreReq
                                     select l3).SingleOrDefault();

                    ////////////////////
                    // If Not, ADd It //
                    ////////////////////
                    if (preReqRcd == null)
                    {
                        var addPreReqRcd = (from l3 in nvDb.SIU_Training_Logs
                                            where l3.TL_UID == certItem.PreReq
                                            select new SIU_Meeting_Log_Ext
                                            {
                                                TL_UID = l3.TL_UID,
                                                Date = l3.Date,
                                                Topic = l3.Topic,
                                                Description = l3.Description,
                                                Instructor = l3.Instructor,
                                                MeetingType = l3.MeetingType,
                                                Location = l3.Location,
                                                Points = l3.Points,
                                                VideoFile = l3.VideoFile,
                                                QuizName = l3.QuizName,
                                                StartTime = l3.StartTime,
                                                StopTime = l3.StopTime,
                                                InstructorID = l3.InstructorID,
                                                VideoComplete = false,
                                                QuizComplete = -1,
                                                QuizPass = false,
                                                PreReq = l3.PreReq
                                            }).ToList();

                        allClassesList.AddRange(addPreReqRcd);
                    }
                }


                ////////////////////////////////////////
                // Now Strip Out Pre-Requisit Classes //
                ////////////////////////////////////////              
                bool more = true;
                while (more)
                {
                    more = false;
                    foreach (var notCompleteListItem in allClassesList.Where(c => c.PreReq != null).OrderByDescending(x=>x.PreReq)   )
                    {
                        // Try And Find PreReq Item To This One //
                        int? itemPreReq = notCompleteListItem.PreReq;
                        var preReqRcd = (from l3 in allClassesList
                                         where l3.TL_UID == itemPreReq
                                         select l3).SingleOrDefault();

                        // Found A PreReq Record
                        if (preReqRcd != null)
                        {
                            // If The Class Is Not Completed
                            // Remove The Dependent Record
                            if (notCompleteListItem.VideoComplete == false || notCompleteListItem.QuizPass == null || notCompleteListItem.QuizPass == false)
                                allClassesList.Remove(notCompleteListItem);
                            more = true;
                            break;
                        }
                    }
                    
                }

                return allClassesList;
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetMeetingLogUser", ex.Message);
        }
        return new List<SIU_Meeting_Log_Ext>();
    }
    public static SIU_Meeting_Log_Ext GetTrainingLog(int UserID, int TL_UID)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                //////////////////////////////////////
                // Build List Of Events In Progress //
                //////////////////////////////////////
                SIU_Meeting_Log_Ext tlPart =  
                    (from tl in nvDb.SIU_Training_Logs
                     where (tl.TL_UID == TL_UID ) 
                     select new SIU_Meeting_Log_Ext
                     {
                         TL_UID = tl.TL_UID,
                         Date = tl.Date,
                         Topic = tl.Topic,
                         Description = tl.Description,
                         Instructor = tl.Instructor,
                         MeetingType = tl.MeetingType,
                         Location = tl.Location,
                         Points = tl.Points,
                         VideoFile = tl.VideoFile,
                         QuizName = tl.QuizName,
                         StartTime = tl.StartTime,
                         StopTime = tl.StopTime,
                         InstructorID = tl.InstructorID,
                         PreReq = tl.PreReq
                     }
                    ).SingleOrDefault();


                SIU_Training_Attendance taPart = 
                    (   from ta in nvDb.SIU_Training_Attendances
                        where (ta.TL_UID == TL_UID && ta.EmpID == UserID)
                        select ta
                    ).Take(1).SingleOrDefault();
                if (taPart == null) taPart = new SIU_Training_Attendance();

                tlPart.VideoComplete = (taPart.VideoEnd != null);
                tlPart.QuizComplete = TriIntFromBool(taPart.QuizPass);
                tlPart.QuizPass = (taPart.QuizPass == null) ? false : taPart.QuizPass;

                return tlPart;
            }

        }
        catch (Exception ex)
        {
            LogDebug("GetTrainingLog", ex.Message);
        }
        return new SIU_Meeting_Log_Ext();
    }

    private static int TriIntFromBool(bool? value)
    {
        // null = -1
        // false = 0
        // true = 1
        return (value == null) ? -1 : (( value == false) ? 0 : 1);
    }


    public static string RecordMeetingLogAdmin(SIU_Training_Log MtgLog)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);
        try
        {
            if ( MtgLog.TL_UID == 0)
            {
                nvDb.SIU_Training_Logs.InsertOnSubmit(MtgLog);
                nvDb.SubmitChanges();
                return "ID:" + MtgLog.TL_UID;
            }

            SIU_Training_Log updLog = (from logRcd in nvDb.SIU_Training_Logs
                                       where logRcd.TL_UID == MtgLog.TL_UID
                                       select logRcd).Single();

            ///////////////////////////////
            // Copy Over New Data Fields //
            ///////////////////////////////
            Mapper.CreateMap<SIU_Training_Log, SIU_Training_Log>();
            Mapper.Map(MtgLog, updLog);
            nvDb.SubmitChanges();
            return "ID:" + MtgLog.TL_UID;
        }
        catch (Exception ex)
        {
            LogDebug("RecordMeetingLogAdmin", ex.Message);
            return ex.Message;
        }      
    }
    public static string RemoveMeetingLogAdmin(int TL_UID)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            var rcd = (from mtgRcd in nvDb.SIU_Training_Logs
                       where mtgRcd.TL_UID == TL_UID
                       select mtgRcd
                      ).SingleOrDefault();


            if (rcd != null)
            {
                nvDb.SIU_Training_Logs.DeleteOnSubmit(rcd);
                nvDb.SubmitChanges();
            }
            else
            {
                LogDebug("RemoveMeetingLogAdmin", "Failed To Find Data Record For " + TL_UID);
            }
            
        }
        catch (Exception ex)
        {
            LogDebug("RemoveMeetingLogAdmin", ex.Message);
            return ex.Message;
        }

        return "";        
    }
    public static List<SIU_Training_Log_Certification> GetMeetingQual(int MeetingID)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from qualList in nvDb.SIU_Training_Log_Certifications
                        where qualList.TL_UID == MeetingID
                        select qualList
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetMeetingQual", ex.Message);
        }
        return new List<SIU_Training_Log_Certification>();
    }
    public static string RecordMeetingQual(SIU_Training_Log_Certification MtgCert)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            nvDb.SIU_Training_Log_Certifications.InsertOnSubmit(MtgCert);
            nvDb.SubmitChanges();
        }
        catch (Exception ex)
        {
            LogDebug("RecordMeetingQual", ex.Message);
            return ex.Message;
        }

        return "";        
    }
    public static string RemoveMeetingQual(int TLC_UID)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            var rcd = (from certRcd in nvDb.SIU_Training_Log_Certifications
                       where certRcd.TLC_UID == TLC_UID
                       select certRcd
                      ).SingleOrDefault();

            if (rcd == null) throw ( new Exception("Unable To Locate Meeting Qualification Key: " + TLC_UID) );

            nvDb.SIU_Training_Log_Certifications.DeleteOnSubmit(rcd);
            nvDb.SubmitChanges();
        }
        catch (Exception ex)
        {
            LogDebug("RemoveMeetingQual", ex.Message);
            return ex.Message;
        }

        return "";
    }
    public static List<SIU_SafetyPays_Points_Type> GetAutoCompleteClassTypes()
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from typeList in nvDb.SIU_SafetyPays_Points_Types
                        where typeList.IsClass
                        select typeList
                       ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetAutoCompletePointTypes", ex.Message);
        }
        return new List<SIU_SafetyPays_Points_Type>();
    }


    public static bool LogVideoWatch(string empNo, string VideoName, string VideoFolder,  string Pos, string Dur, bool Completed=false)
    {
        string sql = "insert into SIU_Meeting_View_Log (EmpNo, VideoName, VideoFolder, Completed, Pos, Dur)";
        sql += " values ( @empNo, @VideoName, @VideoFolder, @Completed, @Pos, @Dur )";

        SqlCommand cmd = new SqlCommand();

        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;

            SqlParameter param = new SqlParameter("@empNo", SqlDbType.VarChar) {Value = empNo};
            cmd.Parameters.Add(param);

            param = new SqlParameter("@VideoName", SqlDbType.NVarChar) {Value = VideoName};
            cmd.Parameters.Add(param);

            param = new SqlParameter("@VideoFolder", SqlDbType.NVarChar) {Value = VideoFolder};
            cmd.Parameters.Add(param);

            param = new SqlParameter("@Completed", SqlDbType.NVarChar) {Value = Completed};
            cmd.Parameters.Add(param);

            param = new SqlParameter("@Pos", SqlDbType.NVarChar) {Value = Pos};
            cmd.Parameters.Add(param);

            param = new SqlParameter("@Dur", SqlDbType.NVarChar) {Value = Dur};
            cmd.Parameters.Add(param);

            cmd.Connection = new SqlConnection(SqlServerProdNvdbConnectString);
            cmd.Connection.Open();

            cmd.ExecuteNonQuery();
        }

        catch (Exception ex)
        {
            LogDebug("LogVideoWatch", ex.Message);
            return false;
        }

        finally
        {
            if (cmd.Connection.State != ConnectionState.Closed)
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
            }
        }

        return true;
    }
    public static string TrainingMovieComplete(string empNo, string TL_UID)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);
        try
        {
            SIU_Training_Attendance ta = (from taVideo in nvDb.SIU_Training_Attendances
                                          where taVideo.TL_UID == int.Parse(  TL_UID ) && taVideo.EmpID == int.Parse(empNo) 
                                          select taVideo
                                          ).OrderByDescending(c => c.TA_UID).Take(1).SingleOrDefault() ?? new SIU_Training_Attendance();

            /////////////////////////////////////////////////
            // If This Attendance Record Is Already Closed //
            // Open Another One                            //
            /////////////////////////////////////////////////
            if (ta.Awarded == true)
            {
                SIU_Training_Attendance_h tah = new SIU_Training_Attendance_h();

                ///////////////////////////////
                // Copy Over New Data Fields //
                ///////////////////////////////
                Mapper.CreateMap<SIU_Training_Attendance, SIU_Training_Attendance_h>();
                Mapper.Map(ta, tah);
                nvDb.SIU_Training_Attendance_hs.InsertOnSubmit(tah);

                ///////////////////////////////////
                // Start A New Attendance Record //
                ///////////////////////////////////
                ta = new SIU_Training_Attendance();

                ///////////////////////////
                // Remove The Old Record //
                ///////////////////////////
                nvDb.SIU_Training_Attendances.DeleteOnSubmit(ta);
            }


            ta.TL_UID = int.Parse( TL_UID );
            ta.EmpID =  int.Parse(  empNo );
            ta.VideoEnd = DateTime.Now;

            if (ta.TA_UID == 0)
            {
                nvDb.SIU_Training_Attendances.InsertOnSubmit(ta);
                nvDb.SubmitChanges();
                return ta.TA_UID.ToString(CultureInfo.InvariantCulture);
            }

            ///////////////////////////////
            // Copy Over New Data Fields //
            ///////////////////////////////

            nvDb.SubmitChanges();
            return ta.TA_UID.ToString(CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            LogDebug("TrainingMovieComplete", ex.Message);
            return ex.Message;
        } 

    }

    public static string RecordTest(SIU_Training_Quiz testRcd)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        ///////////////////////
        // Record Test Taken //
        ///////////////////////
        try
        {
            nvDb.SIU_Training_Quizs.InsertOnSubmit(testRcd);
            nvDb.SubmitChanges();
        }
        catch (Exception ex)
        {
            LogDebug("RecordTest TQ_Section: ", ex.Message);
            return ex.Message;
        }



        try
        {
            int tlUid = int.Parse(testRcd.User_Zipcode);

            SIU_Training_Attendance ta = (from taAttend in nvDb.SIU_Training_Attendances
                where taAttend.TL_UID == tlUid && taAttend.EmpID == int.Parse(testRcd.User_ID)
                select taAttend
                ).OrderByDescending(c =>c.TA_UID).Take(1).SingleOrDefault() ?? new SIU_Training_Attendance();

            /////////////////////////////////////////////////
            // If This Attendance Record Is Already Closed //
            // Open Another One                            //
            /////////////////////////////////////////////////
            if (ta.Awarded == true)
            {
                SIU_Training_Attendance_h tah = new SIU_Training_Attendance_h();

                ///////////////////////////////
                // Copy Over New Data Fields //
                ///////////////////////////////
                Mapper.CreateMap<SIU_Training_Attendance, SIU_Training_Attendance_h>();
                Mapper.Map(ta, tah);
                nvDb.SIU_Training_Attendance_hs.InsertOnSubmit(tah);

                ///////////////////////////////////
                // Start A New Attendance Record //
                ///////////////////////////////////
                ta = new SIU_Training_Attendance();

                ///////////////////////////
                // Remove The Old Record //
                ///////////////////////////
                nvDb.SIU_Training_Attendances.DeleteOnSubmit(ta);
            }


            ////////////////////////////////
            // Update or Insert TA Record //
            ////////////////////////////////
            ta.TL_UID = tlUid;
            ta.TQ_UID = testRcd.UID;
            ta.EmpID = int.Parse(testRcd.User_ID);
            ta.QuizDate = DateTime.Now;
            ta.QuizName = testRcd.Quiz_Name;
            ta.QuizPass = (testRcd.User_Pct_Marks > 79);
            if (ta.QuizPass == true)
                ta.Awarded = true;

            if (ta.TA_UID == 0)
            {
                nvDb.SIU_Training_Attendances.InsertOnSubmit(ta);
                nvDb.SubmitChanges();
                return testRcd.UID.ToString(CultureInfo.InvariantCulture);
            }

            ///////////////////////////
            // Commit all DB Changes //
            ///////////////////////////
            nvDb.SubmitChanges();
        }
        catch (Exception ex)
        {
            LogDebug("RecordTest TA and TAH Sections: ", ex.Message);
        }

        return testRcd.UID.ToString(CultureInfo.InvariantCulture);

    }

#endregion   Safety Training Methods

#region  Document Indexing and Content
    public static IEnumerable<SIU_File> GetFiles()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (    from fc in nvDb.SIU_Files
                    select fc
               );        
    }
    public static void ContentRecordFile(SIU_File SiuFile)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);
        nvDb.SIU_Files.InsertOnSubmit(SiuFile);
        nvDb.SubmitChanges();
    }
    public static void ContentUpdateFile(SIU_File SiuFile)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        var fileCheck = (from fc in nvDb.SIU_Files
                         where fc.FileName == SiuFile.FileName
                         select fc
                        ).FirstOrDefault();


        if (fileCheck != null)
        {
            fileCheck.LastTouch = SiuFile.LastTouch;
            fileCheck.PhyDir = SiuFile.PhyDir;
            fileCheck.VirDir = SiuFile.VirDir;
            fileCheck.FileType = SiuFile.FileType;
            nvDb.SubmitChanges();
        }
        
    }
    public static void ContentRemoveFile(SIU_File SiuFile)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        nvDb.SIU_Files.Attach(SiuFile);
        nvDb.SIU_Files.DeleteOnSubmit(SiuFile);
        nvDb.SubmitChanges();
    }
#endregion

#region Employee Lookups          Xtn Ex
    private static List<string> isDeptMgr(string emp_id)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from rptData in nvDb.SIU_ReportingChains
                        where (rptData.DeptMgrEmpId == emp_id)
                        select rptData.Dept
                       ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("IsDeptMgr", ex.Message);
        }
        return new List<string>();
    }
    public static List<Shermco_Employee> GetActiveEmployees()
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from emplist in nvDb.Shermco_Employees
                    where emplist.Status == 0 && emplist.Blocked == 0 && emplist.Temp_Block == 0
                    select emplist
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetActiveEmployees", ex.Message);
        }
        return new List<Shermco_Employee>();
    }
    public static List<Shermco_Employee> Get_Non_Employees()
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from emplist in nvDb.Shermco_Employees
                        where emplist.Status == 0 && emplist.Blocked == 0 && emplist.Temp_Block == 0 && 
                        (emplist.Contractor == 1 || emplist.Temporary_Employee == 1 || emplist.Co_Op == 1 || emplist.Part_Time == 1 || emplist.Temp_to_Perm == 1 )
                        select emplist
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetActiveEmployees", ex.Message);
        }
        return new List<Shermco_Employee>();
    }
    public static List<SIU_Emp_Ids> GetAutoCompleteActiveEmployees()
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from emplist in nvDb.Shermco_Employees
                    where emplist.Status == 0
                    select
                        new SIU_Emp_Ids { EmpID = emplist.No_, LastName = emplist.Last_Name, FirstName = emplist.First_Name}
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetAutoCompleteActiveEmployees", ex.Message);
        }
        return new List<SIU_Emp_Ids>();
    }
    public static List<SIU_SortedEmployees> GetActiveEmployeesSorted()
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from emplist in nvDb.Shermco_Employees
                    where emplist.Status == 0
                    select new SIU_SortedEmployees {sEmpNo  = emplist.No_,   EmpNo = Convert.ToInt32(emplist.No_), EmpFirstName = emplist.First_Name, EmpLastName = emplist.Last_Name, EmpDisplayNoName = emplist.No_ + " " + emplist.Last_Name + ", " + emplist.First_Name}
                    into newList

                    orderby newList.EmpNo
                    select newList
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetActiveEmployeesSorted", ex.Message);
        }
        return new List<SIU_SortedEmployees>();
    }
    public static List<string> GetActiveEmployeeNames( string NameFilter )
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from emplist in nvDb.Shermco_Employees
                    where emplist.Status == 0 && ( emplist.Last_Name.Contains(NameFilter) || emplist.First_Name.Contains(NameFilter) )
                    orderby emplist.Last_Name
                    select emplist.Last_Name + ", " + emplist.First_Name
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetActiveEmployeeNames", ex.Message);
        }
        return new List<string>();
    }
    public static string GetEmpIdFromName(string LastName, string FirstName)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from emplist in nvDb.Shermco_Employees
                    where ( emplist.Last_Name == LastName.Trim() && emplist.First_Name == FirstName.Trim() ) ||
                          ( emplist.Last_Name == FirstName.Trim() && emplist.First_Name == LastName.Trim() )
                    select emplist.EmpNo
                    ).SingleOrDefault().ToString(CultureInfo.InvariantCulture);
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetEmpIdFromName", ex.Message);
        }
        return "";
    }
    public static Shermco_Employee GetEmployeeByNo(string empNo)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                Shermco_Employee emp = (from anEmp in nvDb.Shermco_Employees
                    where anEmp.No_ == empNo
                    select anEmp).SingleOrDefault();

                if ( emp != null )
                    return emp;

                int iEmpNo;
                if (int.TryParse(empNo, out iEmpNo) == true)
                {
                    return (from anEmp in nvDb.Shermco_Employees
                            where anEmp.No_ == iEmpNo.ToString(CultureInfo.InvariantCulture)
                            select anEmp).SingleOrDefault();
                }

            }
        }
        catch (Exception ex)
        {
            LogDebug("GetEmployeeByNo", ex.Message);
        }
        return null;
    }
    public static List<string> GetEmployeeEmailByNo(List<string> empNos)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (    from anEmp in nvDb.Shermco_Employees
                            where empNos.Contains(anEmp.No_)
                            select anEmp.Company_E_Mail).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetEmployeeEmailByNo", ex.Message);
        }
        return null;
    }
    public static string GetEmployeeNameByNo(string empNo)
    {
        if (empNo == null) return "Unknown";
        if (empNo.Length == 0) return "Unknown";

        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        try
        {
            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                var nameParts  = (    from anEmp in nvDb.Shermco_Employees
                                    where anEmp.No_ == empNo
                                    select  new { anEmp.Last_Name, anEmp.First_Name } 
                                  ).SingleOrDefault();

                if (nameParts == null)
                    return "unknown";
                return "(" + empNo + ")     " + nameParts.Last_Name + ", " + nameParts.First_Name;
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetEmployeeNameByNo", ex.Message);
        }
        return "";
    }
    public static string GetEmployeeDeptByNo(string empNo)
    {
        try
        {
            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                var emp = GetEmployeeByNo(empNo);
                if (emp == null)
                    return empNo;
                return emp.Global_Dimension_1_Code;
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetEmployeeDeptByNo", ex.Message);
        }
        return "";
    }
    public static Shermco_Employee GetEmployeeByCell(string cellNo)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from anEmp in nvDb.Shermco_Employees
                    where anEmp.Shermco_Cell_Phone_No_ == cellNo
                    select anEmp).SingleOrDefault();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetEmployeeByCell", ex.Message);
        }
        return new Shermco_Employee();
    }
    public static SIU_ReportingChain GetEmployeeReportingChain(string Dept)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            return (    from chain in nvDb.SIU_ReportingChains
                        where chain.Dept == Dept
                        select chain).SingleOrDefault();
        }
        catch (Exception ex)
        {
            LogDebug("GetEmployeeReportingChain", ex.Message);
        }
        return null;        
    }
    public static List<SIU_ReportingChain> GetReportingChainList()
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            return (from chain in nvDb.SIU_ReportingChains
                    select chain).ToList();
        }
        catch (Exception ex)
        {
            LogDebug("GetEmployeeReportingChain", ex.Message);
        }
        return null;
    }
    public static List<SIU_ReportingChain> GetOshaDeptData()
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            return (from chain in nvDb.SIU_ReportingChains
                    select chain).ToList();
        }
        catch (Exception ex)
        {
            LogDebug("GetOshaDeptData", ex.Message);
        }
        return null;
    }

    public static List<Shermco_Employee> GetActiveEmployeeCellPhones(List<Shermco_Employee> Emps)
    {
        try
        {
            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from emplist in Emps
                    where emplist.Status == 0 &&
                          (emplist.Shermco_Cell_Phone_No_.Trim(' ').Length > 1) &&
                          emplist.Shermco_Cell_Phone_No_ != "N/A"
                    select emplist).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetActiveEmployeeCellPhones", ex.Message);
        }
        return new List<Shermco_Employee>();
    }
    public static List<Shermco_Employee> GetActiveEmployeeCellPhones()
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from emplist in nvDb.Shermco_Employees
                    where emplist.Status == 0 &&
                          (emplist.Shermco_Cell_Phone_No_.Trim(' ').Length > 1) &&
                          emplist.Shermco_Cell_Phone_No_ != "N/A"
                    select emplist).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetActiveEmployeeCellPhones", ex.Message);
        }
        return new List<Shermco_Employee>();
    }
    public static Shermco_Employee GetEmployeeByEmail(string Email)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from emplist in nvDb.Shermco_Employees
                    where emplist.Status == 0 &&
                          ( emplist.Company_E_Mail.ToLower() == Email.ToLower() )
                    select emplist).FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetEmployeeByEmail", ex.Message);
        }
        return new Shermco_Employee();
    }
    public static bool TestEmployeeProbation(string empNo, DateTime entryDate)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                DateTime dt = ( from anEmp in nvDb.Shermco_Employees
                    where anEmp.No_ == empNo
                    select anEmp.Employment_Date).SingleOrDefault();

                if ( entryDate > dt.AddMonths(3)  )
                    return false;

                return true;
            }
        }
        catch (Exception ex)
        {
            LogDebug("TestEmployeeProbation", ex.Message);
        }
        return false;
    }
    public static bool TestEmployeeHolidayProbation(string empNo, DateTime entryDate)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                DateTime dt = (from anEmp in nvDb.Shermco_Employees
                    where anEmp.No_ == empNo
                    select anEmp.Employment_Date).SingleOrDefault();

                if (entryDate > dt.AddDays(30))
                    return false;

                return true;
            }
        }
        catch (Exception ex)
        {
            LogDebug("TestEmployeeHolidayProbation", ex.Message);
        }
        return false;
    }
    public static bool TestEmployeeIsContractor(string empNo)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (    from anEmp in nvDb.Shermco_Employees
                    where anEmp.No_ == empNo
                    select anEmp.Contractor).SingleOrDefault() == 1; 
            }
        }
        catch (Exception ex)
        {
            LogDebug("TestEmployeeIsContractor", ex.Message);
        }
        return false;       
    }
    public static bool TestEmployeeAllowedOT(string empNo)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from anEmp in nvDb.Shermco_Employees
                    where anEmp.No_ == empNo
                    select anEmp.OT_to_Overhead).SingleOrDefault() == 1;
            }
        }
        catch (Exception ex)
        {
            LogDebug("TestEmployeeAllowedOT", ex.Message);
        }
        return false;
    }
    public static bool TestEmployeeAllowedGt8ST(string empNo)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from anEmp in nvDb.Shermco_Employees
                    where anEmp.No_ == empNo
                    select anEmp.Over_8_Hrs_ST).SingleOrDefault() == 1;
            }
        }
        catch (Exception ex)
        {
            LogDebug("TestEmployeeAllowedGt8ST", ex.Message);
        }
        return false;
    }
#endregion    

#region ESD State Licenses
    public static List<BandC_License_Sum> ESD_License_Sum()
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return  ( from sumData in nvDb.BandC_License_Sums
                    select sumData).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("ESD_License_Sum", ex.Message);
        }
        return new List<BandC_License_Sum>();        
    }
#endregion

#region ELO TIME              Xtn Ex

    public static List<SIU_Oh_Exp_Accounts> GetExpenseOHAccts()
    {

// No Indexes Suggested

        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.Required, to))
            {
                return (from accountList in nvDb.Shermco_Payroll_Posting_Groups
                        where accountList.Hover_Head == 1 && accountList.Earnings_Account.Length == 0
                    orderby accountList.Code
                        select new SIU_Oh_Exp_Accounts { Liab_Account = accountList.Liability_Account, Exp_Account   =  accountList.Expense_Account,   Code = accountList.Code,  Desc = accountList.Description }
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetExpenseOHAccts", ex.Message);
        }
        return new List<SIU_Oh_Exp_Accounts>();
    }

    public static List<SIU_Oh_Exp_Accounts> GetExpenseOHAccts2()
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.Required, to))
            {

                return ( from data in nvDb.Shermco_G_L_Accounts
                        join sList in  nvDb.SIU_Allowed_OH_Accts on data.No_ equals sList.GlAccountNo
                        orderby data.No_
                         select new SIU_Oh_Exp_Accounts { Liab_Account = data.No_, Exp_Account = data.No_, Code = data.Search_Name, Desc = data.Search_Name }
                ).ToList();                    
                    
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetExpenseOHAccts2", ex.Message);
        }
        return new List<SIU_Oh_Exp_Accounts>();
    }


    public static List<SIU_Oh_Accounts> GetTimeOhAccounts()
    {

// No Indexes Suggested

        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.Required, to))
            {
                return (from accountList in nvDb.Shermco_Payroll_Posting_Groups
                    where accountList.Hover_Head == 1
                    orderby accountList.Code
                    select new SIU_Oh_Accounts { Account = accountList.Code, Desc = accountList.Description }
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetTimeOhAccounts", ex.Message);
        }
        return new List<SIU_Oh_Accounts>();
    }
    public static List<SIU_Divs_Depts> GetTimeDivDept()
    {
// No Indexes Suggested
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from deptList in nvDb.Shermco_Dimension_Values
                    where deptList.Dimension_Code.ToLower() == "div/dep" && deptList.Blocked == 0
                    orderby deptList.Dimension_Code
                    select new SIU_Divs_Depts { Code = deptList.Code, Name = deptList.Name }
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetTimeDivDept", ex.Message);
        }
        return new List<SIU_Divs_Depts>();
    }
    public static List<SIU_Task_Codes> GetTimeTaskCodes(string JobNo)
    {
// No Indexes Suggested
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            //var iJobNo = int.Parse(new string(JobNo.Where(char.IsDigit).ToArray())).ToString();
            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.Required, to))
            {
                return (from taskList in nvDb.Shermco_Job_Specific_Routers
                    where taskList.Job_No_ == JobNo && taskList.Block == 0 &&  Convert.ToInt32(taskList.Task_Code) >= 100
                    orderby taskList.Step_No_
                    select
                        new SIU_Task_Codes { StepNo = taskList.Step_No_.ToString(CultureInfo.InvariantCulture), Description = taskList.Task_Description }
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetTimeTaskCodes", ex.Message);
        }
        return new List<SIU_Task_Codes>();
    }
    public static SIU_ELO_GetBalanceResult GetTimeOpenBalance(string EmpNo)
    {
//CREATE NONCLUSTERED INDEX [SIU_PLE_1]
//ON [dbo].[Shermco$Payroll Ledger Entry] ([Employee No_])
//INCLUDE ([Amount],[Payroll Control Code],[Payroll Control Name])
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            return (from bal in nvDb.SIU_ELO_GetBalance(EmpNo)
                    select bal
                   ).SingleOrDefault();
        }
        catch (Exception ex)
        {
            LogDebug("GetTimeOpenBalance", ex.Message);
        }
        return new SIU_ELO_GetBalanceResult();
    }
    public static int GetTimeRejectsCnt(string EmpNo)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from rejects in nvDb.Shermco_Time_Sheet_Entries
                    where rejects.Employee_No_ == EmpNo
                          && rejects.Status == (int)TimeSheetEntry_Status.Rejected
                    select rejects
                    ).Count();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetTimeRejectsCnt", ex.Message);
        }
        return 0;
    }
    public static List<SIU_TimeSheet_DailyHourSum> GetTimeSheet_PeriodDailySums(string empNo, DateTime StartDate, DateTime EndDate)
    {

// No Indexes Suggested
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from tse in nvDb.Shermco_Time_Sheet_Entries
                    where tse.Employee_No_ == empNo && (tse.Work_Date >= StartDate && tse.Work_Date <= EndDate)
                    group tse by new { tse.Work_Date }
                    into g
                    select new SIU_TimeSheet_DailyHourSum
                    {
                        workDate = FormatDateTime(g.Key.Work_Date.ToString(CultureInfo.InvariantCulture)),
                        Hours =
                            g.Sum(
                                o =>
                                    o.Straight_Time + o.Double_Time + o.Over_Time + o.Absence_Time +
                                    o.Holiday_Time),
                        DOW = int.Parse( g.Key.Work_Date.DayOfWeek.ToString() )
                    }
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetTimeSheet_PeriodDailySums", ex.Message);
        }
        return new List<SIU_TimeSheet_DailyHourSum>();
    }
    public static List<string> GetTimeUnapprovedDates(string empNo)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from ud in nvDb.Shermco_Time_Sheet_Entries
                    where ud.Status != 8 && ud.Employee_No_ == empNo
                    orderby ud.Work_Date
                    select FormatDateTime(ud.Work_Date)
                    ).Distinct().ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetTimeUnapprovedDates", ex.Message);
        }

        return new List<string>();
    }
    public static List<Shermco_Time_Sheet_Entry> GetTimeUnapproved(string empNo, string Date)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            DateTime workDate = DateTime.Now;
            if (Date.Length > 0)
                workDate = DateTime.Parse(Date);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from ud in nvDb.Shermco_Time_Sheet_Entries
                    where ud.Status != 8 && ud.Employee_No_ == empNo && ud.Work_Date == workDate
                    orderby ud.timestamp
                    select ud
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetTimeUnapproved", ex.Message);
        }

        return new List<Shermco_Time_Sheet_Entry>();

    }
    public static List<SIU_TimeSheet_HoursRpt> GetHoursRpt(string empNo, string Date)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            //////////////////////////////////////////
            // Convert Passed In Date To Date Field //
            //////////////////////////////////////////
            DateTime workDate = DateTime.Now;
            if (Date.Length > 0)
                workDate = DateTime.Parse(Date);

            ///////////////////////////////////////////////
            // Figure Out Date For Each Day Of That Week //
            ///////////////////////////////////////////////
            DOW enumWeekDays = new DOW(workDate);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from rptData in nvDb.Shermco_Time_Sheet_Entries
                    where rptData.Employee_No_ == empNo && rptData.Work_Date >= enumWeekDays.MonDate && rptData.Work_Date <= enumWeekDays.SunDate && rptData.Status != 8
                    select new SIU_TimeSheet_HoursRpt
                    {
                        EntryNo = rptData.Entry_No_,
                        AB = rptData.Absence_Time,
                        DT = rptData.Double_Time,
                        HT = rptData.Holiday_Time,
                        OT = rptData.Over_Time,
                        ST = rptData.Straight_Time,
                        workDate =  String.Format( "{0:MM/dd}",  rptData.Work_Date ),
                        Total = rptData.Absence_Time + rptData.Straight_Time + rptData.Over_Time + rptData.Double_Time + rptData.Holiday_Time,
                        Dept = rptData.Shortcut_Dimension_1_Code,
                        JobNo = rptData.Job_No_,
                        Task = rptData.Task_Code,
                        OhAcct = rptData.Pay_Posting_Group,
                        Status = rptData.Status
                    }
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetHoursRpt", ex.Message);
        }
        return new List<SIU_TimeSheet_HoursRpt>();

    }
    public static List<SIU_TimeSheet_HoursRpt> GetAllHoursRptLast60(string empNo)
    {
        
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            //////////////////////////////////////////
            // Convert Passed In Date To Date Field //
            //////////////////////////////////////////
            DateTime workDate = DateTime.Now.AddDays(-60);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from rptData in nvDb.Shermco_Time_Sheet_Entries
                    where rptData.Employee_No_ == empNo && rptData.Work_Date >= workDate && rptData.Status != 8
                    select new SIU_TimeSheet_HoursRpt
                    {
                        EntryNo = rptData.Entry_No_,
                        AB = rptData.Absence_Time,
                        DT = rptData.Double_Time,
                        HT = rptData.Holiday_Time,
                        OT = rptData.Over_Time,
                        ST = rptData.Straight_Time,
                        workDate = String.Format("{0:MM/dd}", rptData.Work_Date),
                        Total = rptData.Absence_Time + rptData.Straight_Time + rptData.Over_Time + rptData.Double_Time + rptData.Holiday_Time,
                        Dept = rptData.Shortcut_Dimension_1_Code,
                        JobNo = rptData.Job_No_,
                        Task = rptData.Task_Code,
                        OhAcct = rptData.Pay_Posting_Group,
                        Status = rptData.Status
                    }
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetAllHoursRptLast60", ex.Message);
        }
        return new List<SIU_TimeSheet_HoursRpt>();
    }
    public static List<SIU_TimeSheet_HoursRpt> GetAllHoursRptForMonth(string empNo, DateTime MonthToRptOn)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            //////////////////////////////////////////
            // Convert Passed In Date To Date Field //
            //////////////////////////////////////////
            DateTime workDate = MonthToRptOn.AddDays(-MonthToRptOn.Day + 1).Date;
            DateTime endDate = workDate.AddDays(DateTime.DaysInMonth(workDate.Year, workDate.Month) -1);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from rptData in nvDb.Shermco_Time_Sheet_Entries
                    where rptData.Employee_No_ == empNo && rptData.Work_Date >= workDate && rptData.Work_Date <= endDate
                    select new SIU_TimeSheet_HoursRpt
                    {
                        EntryNo = rptData.Entry_No_,
                        AB = rptData.Absence_Time,
                        DT = rptData.Double_Time,
                        HT = rptData.Holiday_Time,
                        OT = rptData.Over_Time,
                        ST = rptData.Straight_Time,
                        workDate = String.Format("{0:MM/dd}", rptData.Work_Date),
                        Total = rptData.Absence_Time + rptData.Straight_Time + rptData.Over_Time + rptData.Double_Time + rptData.Holiday_Time,
                        Dept = rptData.Shortcut_Dimension_1_Code,
                        JobNo = rptData.Job_No_,
                        Task = rptData.Task_Code,
                        OhAcct = rptData.Pay_Posting_Group,
                        Status = rptData.Status
                    }
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetAllHoursRptThisMo", ex.Message);
        }
        return new List<SIU_TimeSheet_HoursRpt>();
    }
    public static List<SIU_TimeSheet_HoursRpt> GetEloRptPeriodHours(string empNo, DateTime StartDate)
    {
//CREATE NONCLUSTERED INDEX [SIU_TSE_1]
//ON [dbo].[Shermco$Time Sheet Entry] ([Employee No_],[Work Date])
//INCLUDE ([Entry No_],[Status],[Shortcut Dimension 1 Code],[Job No_],[Task Code],[Pay Posting Group],[Straight Time],[Over Time],[Double Time],[Absence Time],[Holiday Time])

        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);


            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from rptData in nvDb.Shermco_Time_Sheet_Entries
                        where rptData.Employee_No_ == empNo && rptData.Work_Date >= StartDate
                        select new SIU_TimeSheet_HoursRpt
                        {
                            EntryNo = rptData.Entry_No_,
                            AB = rptData.Absence_Time,
                            DT = rptData.Double_Time,
                            HT = rptData.Holiday_Time,
                            OT = rptData.Over_Time,
                            ST = rptData.Straight_Time,
                            workDate = String.Format("{0:MM/dd}", rptData.Work_Date),
                            Total = rptData.Absence_Time + rptData.Straight_Time + rptData.Over_Time + rptData.Double_Time + rptData.Holiday_Time,
                            Dept = rptData.Shortcut_Dimension_1_Code,
                            JobNo = rptData.Job_No_,
                            Task = rptData.Task_Code,
                            OhAcct = rptData.Pay_Posting_Group,
                            Status = rptData.Status
                        }
                       ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetEloRptPeriodHours", ex.Message);
        }
        return new List<SIU_TimeSheet_HoursRpt>();
    }
    public static List<SIU_TimeSheet_HoursRpt> GetWeeklyHoursThisYear(string empNo)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            //////////////////////////////////////////
            // Convert Passed In Date To Date Field //
            //////////////////////////////////////////
            DateTime startDate = new DateTime(DateTime.Today.Year, 1, 1);


            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from rptData in nvDb.Shermco_Time_Sheet_Entries
                        where rptData.Employee_No_ == empNo && rptData.Work_Date >= startDate
                        select new SIU_TimeSheet_HoursRpt
                        {
                            EntryNo = rptData.Entry_No_,
                            AB = rptData.Absence_Time,
                            DT = rptData.Double_Time,
                            HT = rptData.Holiday_Time,
                            OT = rptData.Over_Time,
                            ST = rptData.Straight_Time,
                            workDate = String.Format("{0:MM/dd}", rptData.Work_Date),
                            Total = rptData.Absence_Time + rptData.Straight_Time + rptData.Over_Time + rptData.Double_Time + rptData.Holiday_Time,
                            Dept = rptData.Shortcut_Dimension_1_Code,
                            JobNo = rptData.Job_No_,
                            Task = rptData.Task_Code,
                            OhAcct = rptData.Pay_Posting_Group,
                            Status = rptData.Status
                        }
                       ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetWeeklyHoursThisYear", ex.Message);
        }
        return new List<SIU_TimeSheet_HoursRpt>();
    }
    
    public static void DeleteTimeRecord(int RecordId)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            var rcd = (from te in nvDb.Shermco_Time_Sheet_Entries
                       where te.Entry_No_ == RecordId
                       select te
                      ).SingleOrDefault();

            if (rcd == null) throw (new Exception(" Unable To Locate Key: " + RecordId));

            nvDb.Shermco_Time_Sheet_Entries.DeleteOnSubmit(rcd);
            nvDb.SubmitChanges(); 
        }
        catch (Exception ex)
        {
            LogDebug("DeleteTimeRecord", ex.Message);
        }
    }
    public static SIU_TimeSheet_DailyDetailSums GetTimeSheet_DailyBrokenOutSums(string empNo, DateTime entryDate)
    {
        try
        {
            SIU_TimeSheet_DailyDetailSums sums = new SIU_TimeSheet_DailyDetailSums();
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (var trans = new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                int cnt = (from tsl in nvDb.Shermco_Time_Sheet_Entries
                           where tsl.Employee_No_ == empNo && tsl.Work_Date == entryDate
                           select tsl.Straight_Time).Count();

                if (cnt == 0) return sums;

                sums.ST = (from tsl in nvDb.Shermco_Time_Sheet_Entries
                           where tsl.Employee_No_ == empNo && tsl.Work_Date == entryDate
                           select tsl.Straight_Time).Sum();

                sums.OT = (from tsl in nvDb.Shermco_Time_Sheet_Entries
                           where tsl.Employee_No_ == empNo && tsl.Work_Date == entryDate
                           select tsl.Over_Time).Sum();

                sums.DT = (from tsl in nvDb.Shermco_Time_Sheet_Entries
                           where tsl.Employee_No_ == empNo && tsl.Work_Date == entryDate
                           select tsl.Double_Time).Sum();

                sums.HT = (from tsl in nvDb.Shermco_Time_Sheet_Entries
                           where tsl.Employee_No_ == empNo && tsl.Work_Date == entryDate
                           select tsl.Holiday_Time).Sum();

                sums.AB = (from tsl in nvDb.Shermco_Time_Sheet_Entries
                           where tsl.Employee_No_ == empNo && tsl.Work_Date == entryDate
                           select tsl.Absence_Time).Sum();

                trans.Complete();

                return sums;
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetTimeSheet_DailyBrokenOutSums", ex.Message);
        }
        return new SIU_TimeSheet_DailyDetailSums();
    }
    public static string RecordTimeEntry(Shermco_Time_Sheet_Entry TSE)
    {

// No Indexes Suggested
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.Serializable
            };


            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                ///////////////////////////
                // Get Next Entry Number //
                ///////////////////////////
                int newEntryNo = (from nen in nvDb.Shermco_Time_Sheet_Entries
                                    select nen.Entry_No_
                                    ).Max();
                TSE.Entry_No_ = newEntryNo + 1;

                nvDb.Shermco_Time_Sheet_Entries.InsertOnSubmit(TSE);
                nvDb.SubmitChanges();
                tran.Complete();
            }
        }

        catch (Exception ex)
        {
            LogDebug("RecordTimeEntry", ex.Message);
            return ex.Message;
        }

        return "";
    }
    public static decimal GetTimeSheet_Sum(string empNo, DateTime StartDate, DateTime EndDate)
    {

// No Indexes Suggested
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };


            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                var rtnData = (    from tsl in nvDb.Shermco_Time_Sheet_Entries
                                   where tsl.Employee_No_ == empNo && tsl.Work_Date >= StartDate && tsl.Work_Date <= EndDate
                                   select tsl);

                if ( !rtnData.Any() ) return 0;

                return ( from sumData in rtnData
                         select sumData.Straight_Time + sumData.Over_Time + sumData.Double_Time + sumData.Holiday_Time + sumData.Absence_Time).Sum();

                
            }

        }
        catch (Exception ex)
        {
            LogDebug("GetTimeSheet_Sum", ex.Message);
        }


        return 0;
    }
#endregion

#region Hours
    public static IQueryable<SIU_Hours> GetTimesheetRawHours(DateTime _start, DateTime _end)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.Required, to))
            {
                return (
                            from siuTimeSheetEntry in nvDb.Shermco_Time_Sheet_Entries
                            join P2 in nvDb.Shermco_Payroll_Posting_Groups on new { Code = siuTimeSheetEntry.Pay_Posting_Group.ToString() } equals new { Code = P2.Code } into P2_join

                            from P2 in P2_join.DefaultIfEmpty()
                            where
                                siuTimeSheetEntry.Work_Date >= _start && siuTimeSheetEntry.Work_Date <= _end &&
                                siuTimeSheetEntry.Status == 8

                            group new {SIU_TimeSheetEntry = siuTimeSheetEntry, P2} by new {
                                siuTimeSheetEntry.Shortcut_Dimension_1_Code,
                                siuTimeSheetEntry.Work_Date,
                                siuTimeSheetEntry.Pay_Posting_Group,
                                siuTimeSheetEntry.Job_No_,
                                siuTimeSheetEntry.Task_Code,
                                siuTimeSheetEntry.Employee_No_,
                                P2.Description
                            } into g

                            select new SIU_Hours() {
                                Dept = g.Key.Shortcut_Dimension_1_Code,
                                WorkDate = (DateTime?)g.Key.Work_Date,
                                WOY = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(g.Key.Work_Date, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday),
                                EID = g.Key.Employee_No_,
                                Pay_Posting_Group = (g.Key.Pay_Posting_Group + " " + (g.Key.Description ?? "")),
                                Job = g.Key.Job_No_,
                                Task = g.Key.Task_Code,
                                ST = (decimal?)g.Sum(p => p.SIU_TimeSheetEntry.Straight_Time),
                                OT = (decimal?)g.Sum(p => p.SIU_TimeSheetEntry.Over_Time),
                                DT = (decimal?)g.Sum(p => p.SIU_TimeSheetEntry.Double_Time),
                                AB = (decimal?)g.Sum(p => p.SIU_TimeSheetEntry.Absence_Time),
                                HT = (decimal?)g.Sum(p => p.SIU_TimeSheetEntry.Holiday_Time),
                                DTL_SUM = (decimal?)g.Sum(p => p.SIU_TimeSheetEntry.Holiday_Time + p.SIU_TimeSheetEntry.Absence_Time + p.SIU_TimeSheetEntry.Double_Time + p.SIU_TimeSheetEntry.Over_Time + p.SIU_TimeSheetEntry.Straight_Time)
                            }
                    );
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetRawHours", ex.Message);
        }
        return null;
    }
    public static IQueryable<SIU_Hours> SumRawHoursByType(IQueryable<SIU_Hours> getTimesheetRawHoursData)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.Required, to))
            {
                return (
                            from List in getTimesheetRawHoursData
                            group List by new { List.WOY, List.Dept, List.EID, List.Pay_Posting_Group, List.Job, List.Task } into g
                            orderby
                            g.Key.Dept, g.Key.WOY, g.Key.EID, g.Key.Pay_Posting_Group
                            select new SIU_Hours() 
                            {
                                WOY = g.Key.WOY,
                                Dept = g.Key.Dept,
                                EID = g.Key.EID,
                                Pay_Posting_Group = g.Key.Pay_Posting_Group,
                                Job = g.Key.Job,
                                Task = g.Key.Task,
                                TYPE_SUM = g.Sum(p => p.DTL_SUM)
                            }
                        );
            }
        }
        catch (Exception ex)
        {
            LogDebug("SumRawHoursByType", ex.Message);
        }
        return null;        
    }
    public static IQueryable<SIU_Hours> SumRawHoursByDept(IQueryable<SIU_Hours> getTimesheetRawHoursData)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.Required, to))
            {
                return (
                            from List in getTimesheetRawHoursData
                            group List by new { List.Dept } into g
                            orderby g.Key.Dept
                            select new SIU_Hours()
                            {
                                Dept = g.Key.Dept,
                                DEPT_SUM = g.Sum(p => p.DTL_SUM),
                            }
                        );
            }
        }
        catch (Exception ex)
        {
            LogDebug("SumRawHoursByDept", ex.Message);
        }
        return null;  
    }
#endregion Hours
    
#region  Expenses
    public static List<Shermco_Employee_Expense> GetMyExpenses(string empNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from rptData in nvDb.Shermco_Employee_Expenses
                where rptData.Employee_No_ == empNo && rptData.Status != 2
                select rptData
               ).ToList();
    }
    public static List<SIU_YTD_Exp_Rpt> GetMyYtdExpenses(string empNo, bool LastYear)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        DateTime thisYear;
        if ( ! LastYear )
            thisYear = DateTime.Parse("01/01/" + DateTime.Now.Year);
        else
            thisYear = DateTime.Parse("01/01/" + DateTime.Now.AddYears(-1).Year);

        return (from rptData in nvDb.Shermco_Employee_Expenses
                where rptData.Employee_No_ == empNo && rptData.Status == 2 && rptData.Work_Date >= thisYear
                select ( new SIU_YTD_Exp_Rpt(rptData) )
               ).ToList();
    }
    public static decimal GetMilesRate()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from mileRate in nvDb.Shermco_Company_Informations
                select mileRate.Mileage_Reimbursement
               ).SingleOrDefault();        
    }
    public static string RecordExpense(Shermco_Employee_Expense Exp)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        ///////////////////////////
        // Get Next Entry Number //
        ///////////////////////////
        int newEntryNo = (from newExp in nvDb.Shermco_Employee_Expenses
                          select newExp.Line_No_
                         ).Max();
        Exp.Line_No_ = newEntryNo + 1;
        Exp.Status = 0;
        Exp.Approval_Code = GetEmployeeByNo(Exp.Employee_No_).Time_Entry_Approval_Code;

        nvDb.Shermco_Employee_Expenses.InsertOnSubmit(Exp);
        nvDb.SubmitChanges();

        return "";
    }
    public static void DeleteMyExpense(int Line_No_)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        var rcd = (from te in nvDb.Shermco_Employee_Expenses
                   where te.Line_No_ == Line_No_
                   select te
                  ).SingleOrDefault();

        if (rcd != null)
        {
            nvDb.Shermco_Employee_Expenses.DeleteOnSubmit(rcd);
            nvDb.SubmitChanges();
        }
    }
#endregion

#region  Vehilce    Xtn ex
    public static List<Shermco_Vehicle> GetVehicleAssigned(string empNo, DateTime WeekEnding)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions();
           to.IsolationLevel = IsolationLevel.ReadUncommitted;

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                List<string> alreadyReported = (from veh in nvDb.Shermco_Assigend_Vehicle_Datas
                                                where (veh.Emp__No == empNo && veh.Week_Ending == WeekEnding)
                                                select veh.Assigned_Vehicle_No_).ToList();

                return (from veh in nvDb.Shermco_Vehicles
                        where (veh.Driver == empNo && alreadyReported.Contains(veh.No_) == false)
                        select veh).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetVehicleAssigned", ex.Message);
        }

        return new List<Shermco_Vehicle>();
    }
    public static List<string> GetVehilceMileageRptAvail(string empNo, DateTime WeekEnding)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions();
           to.IsolationLevel = IsolationLevel.ReadUncommitted;

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                List<string> alreadyReported = (from veh in nvDb.Shermco_Assigend_Vehicle_Datas
                                                where (veh.Emp__No == empNo && veh.Week_Ending == WeekEnding)
                                                select veh.Assigned_Vehicle_No_).ToList();

                return (from veh in nvDb.Shermco_Vehicles
                        where (veh.Blocked == 0 && alreadyReported.Contains(veh.No_) == false && veh.Hide == 0)
                        select veh.No_).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetVehilceMileageRptAvail", ex.Message);
        }
        return new List<string>();
    }
    public static List<string> GetVehilceList()
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions();
            to.IsolationLevel = IsolationLevel.ReadUncommitted;

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from veh in nvDb.Shermco_Vehicles
                        where (veh.Blocked == 0 && veh.Hide == 0)
                        select veh.No_).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetVehilceList", ex.Message);
        }
        return new List<string>();
    }
    public static Shermco_Assigend_Vehicle_Data GetVehicleMileageRecord(string empNo, DateTime WeekEnding)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from veh in nvDb.Shermco_Assigend_Vehicle_Datas
                        where veh.Emp__No == empNo && veh.Week_Ending.Date == WeekEnding.Date
                        select veh
                       ).SingleOrDefault();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetVehicleMileageRecord", ex.Message);
        }
        return new Shermco_Assigend_Vehicle_Data();
    }
    public static List<Shermco_Assigend_Vehicle_Data> GetMyVehicleMileageRpt(string empNo)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from veh in nvDb.Shermco_Assigend_Vehicle_Datas
                    where
                        veh.Emp__No == empNo &&
                        veh.Week_Ending.Date > DateTime.Parse("01/01/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture))
                    select veh
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetMyVehicleMileageRpt", ex.Message);
        }
        return new List<Shermco_Assigend_Vehicle_Data>();
    }
    public static string RecordVehicleMileage(Shermco_Assigend_Vehicle_Data VehData)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.Serializable
            };

            Shermco_Assigend_Vehicle_Data newVehRcd;

            using (var trans = new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                /////////////////////////////////
                // Look For An Existing Record //
                /////////////////////////////////
                newVehRcd = GetVehicleMileageRecord(VehData.Emp__No, VehData.Week_Ending);

                /////////////////////////////////////////////
                // If This Is A New Record, Perform Insert //
                /////////////////////////////////////////////
                if (newVehRcd == null)
                {
                    
                    nvDb.Shermco_Assigend_Vehicle_Datas.InsertOnSubmit(VehData);
                    nvDb.SubmitChanges();
                    trans.Complete();
                }
            }

            return (newVehRcd == null) ? "" : "Data For This Week Already Entered";  
        }
        catch (Exception ex)
        {
            LogDebug("RecordVehicleMileage", ex.Message);
            return ex.Message;
        }
    }
    public static string RemoveVehicleMileage(Shermco_Assigend_Vehicle_Data VehData)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString)
            {
                Log = new DebugTextWriter()
            };

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using (var trans = new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                /////////////////////////////////
                // Look For An Existing Record //
                /////////////////////////////////
                var delVehRcd =  (  from veh in nvDb.Shermco_Assigend_Vehicle_Datas
                                    where veh.Emp__No == VehData.Emp__No && veh.Week_Ending.Date == VehData.Week_Ending.Date
                                    select veh
                                 ).SingleOrDefault();

                ////////////////////////////////
                // If Record Found, Remove It //
                ////////////////////////////////
                if (delVehRcd != null)
                {
                    nvDb.Shermco_Assigend_Vehicle_Datas.DeleteOnSubmit(delVehRcd);
                    nvDb.SubmitChanges();
                    trans.Complete();
                }

                return (delVehRcd != null) ? "" : "Record Not Located";
            }

        }
        catch (Exception ex)
        {
            LogDebug("RemoveVehicleMileage", ex.Message);
            return ex.Message;
        }
    }

    public static Shermco_Vehicle GetVehicleRecord(string VehicleNo)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (    from veh in nvDb.Shermco_Vehicles
                            where (veh.No_ == VehicleNo)
                            select veh).SingleOrDefault();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetVehicleRecord", ex.Message);
        }
        return new Shermco_Vehicle();        
    }
    public static SIU_DOT_Rpt GetVehInspById(string ID)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                SIU_DOT_Rpt Rpt = ( from rptData in nvDb.SIU_DOT_Inspections
                                    where rptData.RefID == int.Parse(ID)
                                    select new SIU_DOT_Rpt { Hazard = rptData.Hazard, RefID = rptData.RefID, SubmitEmpID = rptData.SubmitEmpID, SubmitTimeStamp = rptData.SubmitTimeStamp.ToShortDateString(), Vehicle = rptData.Vehicle, Correction = rptData.Correction }
                       ).SingleOrDefault();

                return Rpt;
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetVehInspById", ex.Message);
        }
        return new SIU_DOT_Rpt();
    }
    public static List<SIU_DOT_Rpt> GetOpenDOT(string empNo)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                List<String> assignedVehIds = (from veh in nvDb.Shermco_Vehicles
                                               where (veh.Driver == empNo)
                                               select veh.No_).ToList();

                List<SIU_DOT_Rpt> basicList = (from rptData in nvDb.SIU_DOT_Inspections
                        where (rptData.SubmitEmpID == empNo && rptData.CorrectionEmpID == null) || (assignedVehIds.Contains(rptData.Vehicle) && rptData.CorrectionEmpID == null)
                        select new SIU_DOT_Rpt { Hazard = rptData.Hazard, RefID = rptData.RefID, SubmitEmpID = rptData.SubmitEmpID, SubmitTimeStamp = rptData.SubmitTimeStamp.ToShortDateString(), Vehicle = rptData.Vehicle, Correction = rptData.Correction}
                       ).ToList();

                List<String> usersDepts = isDeptMgr(BusinessLayer.UserEmpID);
                if ( usersDepts.Count > 0 )
                {
                    List<SIU_DOT_Rpt> deptRpts = (from veh in nvDb.Shermco_Vehicles
                                                  join dot in nvDb.SIU_DOT_Inspections on veh.No_ equals dot.Vehicle
                                                  where veh.Blocked == 0 && veh.Driver == "" && usersDepts.Contains(veh.Global_Dimension_1_Code) && dot.CorrectionTimeStamp == null
                                                  select new SIU_DOT_Rpt { Hazard = dot.Hazard, RefID = dot.RefID, SubmitEmpID = dot.SubmitEmpID, SubmitTimeStamp = dot.SubmitTimeStamp.ToShortDateString(), Vehicle = dot.Vehicle, Correction = dot.Correction }
                                                ).ToList();
                    basicList.AddRange(deptRpts);
                }

                return basicList;
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetOpenDOT", ex.Message);
        }
        return new List<SIU_DOT_Rpt>();
    }
    public static List<SIU_DOT_Rpt> GetOpenDOT_WeekView(string empNo)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                DateTime dateRangeLimit = DateTime.Now.AddDays(-14);

                List<String> assignedVehIds = (from veh in nvDb.Shermco_Vehicles
                                               where (veh.Driver == empNo)
                                               select veh.No_).ToList();

                List<SIU_DOT_Rpt> basicList = ( from rptData in nvDb.SIU_DOT_Inspections
                                                where  ( (rptData.SubmitEmpID == empNo) || assignedVehIds.Contains(rptData.Vehicle) ) && rptData.SubmitTimeStamp > dateRangeLimit
                                                select new SIU_DOT_Rpt { Hazard = rptData.Hazard, RefID = rptData.RefID, SubmitEmpID = rptData.SubmitEmpID, SubmitTimeStamp = rptData.SubmitTimeStamp.ToShortDateString(), Vehicle = rptData.Vehicle, Correction = rptData.Correction }
                                            ).ToList();



                List<String> usersDepts = isDeptMgr(BusinessLayer.UserEmpID);
                if (usersDepts.Count > 0)
                {
                    List<SIU_DOT_Rpt> deptRpts = (from veh in nvDb.Shermco_Vehicles
                                                  join dot in nvDb.SIU_DOT_Inspections on veh.No_ equals dot.Vehicle
                                                  where veh.Blocked == 0 && veh.Driver == "" && usersDepts.Contains(veh.Global_Dimension_1_Code) && dot.CorrectionTimeStamp == null
                                                  select new SIU_DOT_Rpt { Hazard = dot.Hazard, RefID = dot.RefID, SubmitEmpID = dot.SubmitEmpID, SubmitTimeStamp = dot.SubmitTimeStamp.ToShortDateString(), Vehicle = dot.Vehicle, Correction = dot.Correction }
                                                ).ToList();
                    basicList.AddRange(deptRpts);
                }

                return basicList;
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetOpenDOT_WeekView", ex.Message);
        }
        return new List<SIU_DOT_Rpt>();
    }
    public static void RecordDotCorrection(int RefID, string EmpID, string CorrectiveAction)
    {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            SIU_DOT_Inspection inspData = (from rptData in nvDb.SIU_DOT_Inspections
                                            where rptData.RefID == RefID
                                            select rptData
                                            ).Single();

            inspData.CorrectionEmpID = EmpID;
            inspData.CorrectionTimeStamp = DateTime.Now;
            inspData.Correction = CorrectiveAction;
            nvDb.SubmitChanges();

    }
    public static void RecordDot(int RefID, string EmpID, string InspDate, string VehNo, string Hazard, string CorrectiveAction)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        if (RefID > 0)
        {
            SIU_DOT_Inspection inspData = (from rptData in nvDb.SIU_DOT_Inspections
                                           where rptData.RefID == RefID
                                           select rptData
                                          ).Single();

            inspData.CorrectionEmpID = EmpID;
            inspData.CorrectionTimeStamp = DateTime.Now;
            inspData.Correction = CorrectiveAction;
            
        }
        else
        {
            SIU_DOT_Inspection inspData = new SIU_DOT_Inspection {SubmitEmpID = EmpID, SubmitTimeStamp = DateTime.Parse(InspDate), Vehicle = VehNo, Hazard = Hazard};
            if ( CorrectiveAction.Length > 0 )
            {
                inspData.Correction = CorrectiveAction;
                inspData.CorrectionEmpID = EmpID;
                inspData.CorrectionTimeStamp = DateTime.Parse(InspDate);
            }
            nvDb.SIU_DOT_Inspections.InsertOnSubmit(inspData);
        }
        nvDb.SubmitChanges();
    }


    public static List<SIU_DOT_Inspection> getAllOpenVehInsp()
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions();
            to.IsolationLevel = IsolationLevel.ReadUncommitted;

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from veh in nvDb.SIU_DOT_Inspections
                        where (veh.CorrectionTimeStamp == null)
                        select veh).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("getAllOpenVehInsp", ex.Message);
        }
        return new List<SIU_DOT_Inspection>();        
    }
    public static List<SIU_DOT_Inspection> getAllOpenVehInsp(string likeDept)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions();
            to.IsolationLevel = IsolationLevel.ReadUncommitted;

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (    from insp in nvDb.SIU_DOT_Inspections
                            join veh in nvDb.Shermco_Vehicles on insp.Vehicle equals veh.No_
                            where ((insp.CorrectionTimeStamp == null) && veh.Global_Dimension_1_Code.StartsWith(likeDept))
                        select insp).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("getAllOpenVehInsp", ex.Message);
        }
        return new List<SIU_DOT_Inspection>();
    }
#endregion

#region Jobs
    public static List<string> GetJobRptJobs()
    {
//CREATE NONCLUSTERED INDEX [SIU_Job_1]
//ON [dbo].[Shermco$Job] ( [Status], [Is Master Job] )
//INCLUDE ( [No_], [Search Description], [Site Location])
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from jobList in nvDb.Shermco_Jobs
                where jobList.Status == (int)Job_Status.Order && jobList.Is_Master_Job == 0
                orderby jobList.No_
                select jobList.No_ + " " + jobList.Site_Location
               ).ToList();
    }
    public static List<SIU_Job> GetActiveJobs()
    {
//CREATE NONCLUSTERED INDEX [SIU_Job_1] ON [dbo].[Shermco$Job] ([Status], [Is Master Job] ) INCLUDE ( [No_], [Search Description], [Site Location])
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.Required, to))
            {
                return (from jobList in nvDb.Shermco_Jobs
                        where jobList.Status == (int)Job_Status.Order && jobList.Is_Master_Job == 0
                        orderby jobList.No_
                        select new SIU_Job { JobNo = jobList.No_, JobDesc = jobList.Search_Description, JobSite = jobList.Site_Location }
                       ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetActiveJobs", ex.Message);
        }
        return new List<SIU_Job>();
    }
    public static Shermco_Job GetJobByNo(string jobNo)
    {
// No Indexes Suggested
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        try
        {
            return (from aJob in nvDb.Shermco_Jobs
                    where aJob.No_ == jobNo
                    select aJob).Single();
        }
        catch
        {
            return null;   
        }
    }
    public static IQueryable<SIU_Complete_Job> GetSiuCompletedJobs()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from job in nvDb.SIU_Complete_Jobs
                group job by job.JobNo
                into sortGrp

                let maxdate = sortGrp.Max(g => g.TimeStamp)

                from g in sortGrp
                where g.TimeStamp == maxdate
                select g);
    }
    public static SIU_Complete_Job GetSiuCompletedJob(string jobNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from job in nvDb.SIU_Complete_Jobs
                where job.JobNo == jobNo
                group job by job.JobNo
                into sortGrp

                let maxdate = sortGrp.Max(g => g.TimeStamp)

                from g in sortGrp
                where g.TimeStamp == maxdate
                select g).SingleOrDefault();
    }
    public static bool RecordSiuCompletedJob(SIU_Complete_Job JobRcd)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        JobRcd.TimeStamp = DateTime.Now;
        nvDb.SIU_Complete_Jobs.InsertOnSubmit(JobRcd);
        nvDb.SubmitChanges();

        return true;
    }

    public static string GetJobLeadTechName(string jobNo)
    {
        if (jobNo == null) return "";
        if (jobNo.Length == 0) return "";

        try
        {
            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                var job = GetJobByNo(jobNo);
                if (job == null)
                    return "";
                return GetEmployeeNameByNo(job.Lead_Tech);
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetJobLeadTechName", ex.Message);
        }
        return "";
    }

    public static Shermco_Job_Report GetSubmitJobReportByNo(string jobNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from aJob in nvDb.Shermco_Job_Reports
                where aJob.Job_No_ == jobNo
                select aJob).SingleOrDefault();        
    }
    public static string RecordSubmitJobReport(Shermco_Job_Report JobRptRcd)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        /////////////////////////////////
        // Look For An Existing Record //
        /////////////////////////////////
        Shermco_Job_Report newRptRcd = GetSubmitJobReportByNo(JobRptRcd.Job_No_);

        /////////////////////////////////////////////
        // If This Is A New Record, Perform Insert //
        /////////////////////////////////////////////
        if (newRptRcd == null)
        {
            nvDb.Shermco_Job_Reports.InsertOnSubmit(JobRptRcd);
        }

        /////////////////////////////////
        // Otherwise Perform An Update //
        /////////////////////////////////
        else
        {
            ////////////////////////////////////////
            // Get The Origional Open Data Record //
            ////////////////////////////////////////
            nvDb.Shermco_Job_Reports.Attach(newRptRcd);

            ///////////////////////////////
            // Copy Over New Data Fields //
            ///////////////////////////////
            Mapper.CreateMap<Shermco_Job_Report, Shermco_Job_Report>()
                .ForMember(dest => dest.timestamp, opt => opt.Ignore());
            Mapper.Map(JobRptRcd, newRptRcd);
        }
        nvDb.SubmitChanges();

        return (newRptRcd == null) ? JobRptRcd.Job_No_ : newRptRcd.Job_No_;
    }
    public static List<string> GetSubmitJobReportNosByEmp(string empNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from empRpts in nvDb.Shermco_Job_Reports
                where empRpts.Turned_in_By_Emp__No_ == empNo
                select empRpts.Job_No_
               ).ToList();
    }
    public static List<InProgressJobReport> GetMyOpenJobRptsSum(string empNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from pastDue in nvDb.Shermco_Job_Reports
                where   pastDue.Turned_in_By_Emp__No_ == empNo && ( pastDue.Complete_and_Delivered ==  DateTime.Parse("1753-01-01") && pastDue.No_Report_Required == 0 )
                select new InProgressJobReport(pastDue)
               ).ToList();
    }
    public static List<PastDueJobReport> GetMyPastDueJobRptsSum(string empNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return ( from jobs in nvDb.Shermco_Jobs
                    where jobs.Lead_Tech == empNo &&
                        jobs.Date_Job_Went_to_Cost != DateTime.Parse("1753-01-01") &&
                        jobs.Reporting_Not_Necessary == 0 &&
                        ( from  rpts in nvDb.Shermco_Job_Reports select rpts.Job_No_ ).Contains(jobs.No_) == false
                  select new PastDueJobReport(jobs)
               ).ToList();

    }
    public static DateTime GetJobLastLaborDate(string JobNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (    from time in nvDb.Shermco_Time_Sheet_Entries
                    where time.Job_No_ == JobNo
                    orderby time.Work_Date descending
                    select time.Work_Date
                    
                ).Take(1).SingleOrDefault();
    }
#endregion

#region Customer
    public static string GetCustomerName(string CustomerNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from cust in nvDb.Shermco_Customers
                where cust.No_ == CustomerNo
                select cust.Name
               ).SingleOrDefault();
    }
#endregion

#region IT Hardware Request
    public static IEnumerable<SIU_IT_HW_Req> GetHardwareOpenRequest()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from openRequest in nvDb.SIU_IT_HW_Reqs
                where openRequest.CompleteDate == null
                select openRequest
               );
    }
    public static SIU_IT_HW_Req GetHardwareRequest(int ReqID)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from request in nvDb.SIU_IT_HW_Reqs
                where request.XctnID == ReqID
                select request
               ).SingleOrDefault();
    }
    public static List<SIU_IT_HW_Req_Computer> GetHardwareRequestComputers()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (    from computerList in nvDb.SIU_IT_HW_Req_Computers 
                    select computerList
               ).ToList();
    }
    public static List<SIU_IT_HW_Req_Add> GetHardwareRequestAddOns()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from computerList in nvDb.SIU_IT_HW_Req_Adds
                select computerList
               ).ToList();
    }
    public static SIU_IT_HW_Req GetHardwareOpenRequest(string EmpID)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (    from openRequest in nvDb.SIU_IT_HW_Reqs
                    where openRequest.For_EmpID == EmpID && openRequest.CompleteDate == null
                    select openRequest
               ).SingleOrDefault();
    }
    public static int RecordHardwareRequest(SIU_IT_HW_Req HwReq)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        /////////////////////////////
        // Look For An Open Record //
        /////////////////////////////
        SIU_IT_HW_Req newHwReq = GetHardwareOpenRequest(HwReq.For_EmpID);

        /////////////////////////////////////////////
        // If This Is A New Record, Perform Insert //
        /////////////////////////////////////////////
        if (newHwReq == null)
        {
            HwReq.Timestamp = DateTime.Now;
            nvDb.SIU_IT_HW_Reqs.InsertOnSubmit(HwReq);
        }

        /////////////////////////////////
        // Otherwise Perform An Update //
        /////////////////////////////////
        else
        {
            ////////////////////////////////////////
            // Get The Origional Open Data Record //
            ////////////////////////////////////////
            nvDb.SIU_IT_HW_Reqs.Attach(newHwReq);

            ///////////////////////////////
            // Copy Over New Data Fields //
            ///////////////////////////////
            Mapper.CreateMap<SIU_IT_HW_Req, SIU_IT_HW_Req>()
                .ForMember(dest => dest.XctnID, opt => opt.Ignore());
            Mapper.Map(HwReq, newHwReq);

            //////////////////////
            // Update Timestamp //
            //////////////////////
            newHwReq.Timestamp = DateTime.Now;
        }
        nvDb.SubmitChanges();

        return ( newHwReq == null) ? HwReq.XctnID : newHwReq.XctnID;
    }
    public static void CloseHardwareRequest(int ReqID, string EmpID)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        SIU_IT_HW_Req request = (  from hwReq in nvDb.SIU_IT_HW_Reqs
                                    where hwReq.XctnID == ReqID
                                    select hwReq
                                 ).SingleOrDefault();

        if (request != null)
        {
            request.CompleteDate = DateTime.Now;
            request.CompletedBy_EmpID = EmpID;
            nvDb.SubmitChanges();
        }
        
    }
    public static List<SIU_IT_HW_Req> GetMyOpenHwReq(string EmpID)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from rptData in nvDb.SIU_IT_HW_Reqs
                where rptData.Req_EmpID == EmpID
                select rptData
               ).ToList();
    }
#endregion

#region BandC Data Methods
    public static IEnumerable<Shermco_Qualification> GetBandC_QualCodes()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(ForcedProductionConnectString);
        
        return (from codes in nvDb.Shermco_Qualifications
                where codes.Do_Not_Use == 0
                select codes
               );
    }
    public static IEnumerable<Shermco_Qualification> GetBandC_QualCodes(string Qual_Code, string Category)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(ForcedProductionConnectString);

        return (from codes in nvDb.Shermco_Qualifications
                where codes.Code.Contains(Qual_Code) && codes.Do_Not_Use == 0 &&  codes.Category.Contains(Category)
                select codes
               );
    }
    public static IEnumerable<Shermco_Qualification> GetBandC_QualCode(string Qual_Code)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(ForcedProductionConnectString);

        return (from codes in nvDb.Shermco_Qualifications
                where codes.Code == Qual_Code
                select codes
               );
    }

    public static List<String> GetBandC_QualCategories()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(ForcedProductionConnectString);

            return (from categories in nvDb.Shermco_Qualifications
                where categories.Do_Not_Use == 0
                select categories.Category
               ).Distinct().ToList();
    }
    public static List<String> Please_Select_GetBandC_QualCategories()
    {
        List<string> codes = GetBandC_QualCategories();
        codes.Insert(0, "Category Selection");

        return codes;
    }
    public static List<String> GetBandC_QualCategories(string StartsWith)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(ForcedProductionConnectString);

        return (from categories in nvDb.Shermco_Qualifications
                where categories.Do_Not_Use == 0 && categories.Category.StartsWith(StartsWith)
                select categories.Category
           ).Distinct().ToList();
    }
    public static int GetBandC_EmpCntByCode(string QualCode)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(ForcedProductionConnectString);

        return (from qualEmps in nvDb.Shermco_Employee_Qualifications
                where qualEmps.Qualification_Code == QualCode
                select qualEmps.Line_No_
               ).Count();
    }

    public static void RecordBandC_QualCode(Shermco_Qualification QualCode)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(ForcedProductionConnectString);

        /////////////////////////////////
        // Look For An Existing Record //
        /////////////////////////////////
        Shermco_Qualification newQualCode = GetBandC_QualCode(QualCode.Code).SingleOrDefault();

        /////////////////////////////////////////////
        // If This Is A New Record, Perform Insert //
        /////////////////////////////////////////////
        if (newQualCode == null)
        {
            nvDb.Shermco_Qualifications.InsertOnSubmit(QualCode);
        }



        /////////////////////////////////
        // Otherwise Perform An Update //
        /////////////////////////////////
        else
        {
            //////////////////////////
            // Start Update Process //
            //////////////////////////
            nvDb.Shermco_Qualifications.Attach(newQualCode);

            ///////////////////////////////
            // Copy Over New Data Fields //
            ///////////////////////////////
            Mapper.CreateMap<Shermco_Qualification, Shermco_Qualification>()
                .ForMember(dest => dest.timestamp, opt => opt.Ignore());
            Mapper.Map(QualCode, newQualCode);


        }
        nvDb.SubmitChanges();

    }
    public static void RecordBandC_EmplCodes(IOrderedDictionary NewValues)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(ForcedProductionConnectString);

        var empQualRcds = (from qualEmps in nvDb.Shermco_Employee_Qualifications
                           where qualEmps.Qualification_Code == (string) NewValues["Code"]
                           select qualEmps
                          );

        foreach (var shermcoEmployeeQualification in empQualRcds)
        {
            shermcoEmployeeQualification.Description = NewValues["Description"].ToString();
            DateTime newToDate = shermcoEmployeeQualification.From_Date;
            newToDate = newToDate.AddMonths( int.Parse( NewValues["Duration_of_Certification"].ToString() )  );
            shermcoEmployeeQualification.To_Date = newToDate;
            shermcoEmployeeQualification.Expiration_Date = newToDate;
            shermcoEmployeeQualification.Last_Modified_Date = DateTime.Now.Date;
        }

        nvDb.SubmitChanges();
    }

    public static List<SIU_Class_Completion> GetUnpostedClass(string EmpID, string ClassCode, string ClassDate)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from students in nvDb.SIU_Class_Completions
                where students.class_instructor == EmpID // && Students.qual_code == ClassCode && Students.class_date == DateTime.Parse(ClassDate)
                select students
               ).ToList();
    }


    public static SIU_Class_Completion RecordUnPostedClassStudent(SIU_Class_Completion _StudentClass)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        string emp = (from e in nvDb.Shermco_Employees
                      where e.No_ == _StudentClass.emp_no || e.Drivers_License_No_ == _StudentClass.emp_no
                      select e.No_
                     ).SingleOrDefault();

        if (emp == null)
            throw ( new Exception("Invalid Employee ID"));
        if (emp.Length == 0)
            throw ( new Exception("Invalid Employee ID"));


        _StudentClass.emp_no = emp;
        _StudentClass.instructor_name = GetEmployeeNameByNo(_StudentClass.class_instructor);
        _StudentClass.student_name = GetEmployeeNameByNo(_StudentClass.emp_no);
        _StudentClass.qual_name = GetBandC_QualCode(_StudentClass.qual_code).Single().Description;

        nvDb.SIU_Class_Completions.InsertOnSubmit(_StudentClass);
        nvDb.SubmitChanges();
        return _StudentClass;
    }
    public static string RemoveUnPostedClassStudent(string _RcdID)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            var rcd = (from te in nvDb.SIU_Class_Completions
                       where te.UID == int.Parse(_RcdID)
                       select te
                      ).SingleOrDefault();

            if (rcd == null) throw (new Exception("Unable To Locate Key: " + _RcdID));

            nvDb.SIU_Class_Completions.DeleteOnSubmit(rcd);
            nvDb.SubmitChanges();
        }
        catch (Exception ex)
        {
            LogDebug("RemoveUnPostedClassStudent", ex.Message);
            return ex.Message;
        }

        return "OK";
    }
    public static void CommitUnPostedClassStudent(string UID)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using (var trans = new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                Int32 iUid = int.Parse(UID);
                nvDb.ExecuteCommand("exec SIU_Class_To_Qualification @UID={0}", iUid);

                trans.Complete();
            }
        }
        catch (Exception ex)
        {
            LogDebug("CommitUnPostedClassStudent", ex.Message);
        }

    }


    public static Shermco_Employee_Qualification GetEmpCertDocument(string Code, string EmpID )
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        TransactionOptions to = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadUncommitted
        };

        using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
        {
            return  (    from certsData in nvDb.Shermco_Employee_Qualifications
                                where certsData.Employee_No_ == EmpID && certsData.Qualification_Code == Code && certsData.Description != "Requirement Marker"
                                orderby certsData.From_Date descending
                                select certsData
                            ).Take(1).SingleOrDefault();
        }
    }



    public static void DeleteBandC_QualCode(string Code)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(ForcedProductionConnectString);

        var rcd = (from codes in nvDb.Shermco_Qualifications
                   where codes.Code == Code
                   select codes
                  ).SingleOrDefault();

        if (rcd != null)
        {
            rcd.Do_Not_Use = 1;
            nvDb.SubmitChanges();
        }
    }

    public static List<BandC_DistinctByEmployee> GetMyExpiringBandC(string empNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        TransactionOptions to = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted
        };

        using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
        {
            return ( from rptData in nvDb.BandC_DistinctByEmployees
                     where rptData.No_ == empNo && rptData.To_Date < DateTime.Now.AddDays(-30)
                     select rptData
                   ).ToList();

            //return (from rptData in nvDb.SIU_MySi_ExpiringBandCs
            //        where rptData.employee_no_ == empNo
            //        select rptData
            //       ).ToList();
        }
    }

    public static List<BandC_DistinctByEmployee> GetMyAllBandC(string empNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        TransactionOptions to = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadUncommitted
        };

        using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
        {
            return (from rptData in nvDb.BandC_DistinctByEmployees
                    where rptData.No_ == empNo
                    select rptData
                   ).ToList();
        }
    }


    //public static List<SIU_MySi_MissedSafetyMeeting> GetMyMissedSafetyClasses(string empNo)
    //{
    //    SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

    //    TransactionOptions to = new TransactionOptions
    //    {
    //        IsolationLevel = IsolationLevel.ReadUncommitted
    //    };

    //    using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
    //    {
    //        return (from rptData in nvDb.SIU_MySi_MissedSafetyMeetings
    //                where rptData.EmpNo == empNo
    //                select rptData
    //               ).ToList();
    //    }
    //}

    
#endregion

#region Safety Pays Methods

    // Called By Legacy Safety Pays Task List Page //
    // Called By Legacy Safety Pays Task List Page //
    // Called By Legacy Safety Pays Task List Page //
    // Called By Legacy Safety Pays Task List Page //
    public static IEnumerable<SIU_SafetyPaysReport> GetSafetyPaysAdmin()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from spList in nvDb.SIU_SafetyPaysReports
                where spList.IncStatus != "Closed" && spList.IncStatus != "Reject"
                select spList
               );
    }


    //////////////////////////////////
    // Add A New Safety Pays Report //
    //////////////////////////////////
    public static int RecordSafetyPaysReport(SIU_SafetyPaysReport _report)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);
        SIU_SafetyPaysReport newReport = null;

        try
        {


            /////////////////////////////////////////////
            // If This Is A New Record, Perform Insert //
            /////////////////////////////////////////////
            if (_report.IncidentNo == 0)
            {
                nvDb.SIU_SafetyPaysReports.InsertOnSubmit(_report);
            }



            /////////////////////////////////
            // Otherwise Perform An Update //
            /////////////////////////////////
            else
            {
                /////////////////////////////////
                // Look For An Existing Record //
                /////////////////////////////////
                newReport = GetSafetyPaysReport(_report.IncidentNo).SingleOrDefault();

                if (newReport == null) throw (new Exception("Unable To Locate Key: " + _report.IncidentNo));

                //////////////////////////
                // Start Update Process //
                //////////////////////////
                nvDb.SIU_SafetyPaysReports.Attach(newReport);

                ///////////////////////////////
                // Copy Over New Data Fields //
                ///////////////////////////////
                Mapper.CreateMap<SIU_SafetyPaysReport, SIU_SafetyPaysReport>();
                Mapper.Map(_report, newReport);


            }
            nvDb.SubmitChanges();

            return (newReport == null) ? _report.IncidentNo : newReport.IncidentNo;
        }

        catch (Exception ex)
        {
            LogDebug("RecordSafetyPaysReport", ex.Message);
            throw;
        }
    }
    public static void RemoveSafetyPaysRpt(string IncNo)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            var rcd = (from spRcd in nvDb.SIU_SafetyPaysReports
                       where spRcd.IncidentNo == int.Parse(IncNo)
                       select spRcd
                      ).SingleOrDefault();

            if (rcd == null) throw (new Exception("Unable To Locate Key: " + IncNo));

            nvDb.SIU_SafetyPaysReports.DeleteOnSubmit(rcd);
            nvDb.SubmitChanges();
        }
        catch (Exception ex)
        {
            LogDebug("RemoveSafetyPaysRpt", ex.Message);
        }        
    }


    /////////////////////////////////////////////////////////////
    // Data Support For EHS Admin To Sort And Mark Each Safety //
    // Pays Admin As Either ACCPET, WORK, or REJECT            //
    /////////////////////////////////////////////////////////////
    public static List<SIU_SafetyPaysReport_Rpt> GetSafetyPaysNoTask()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        List<int> incNo = (from spList in nvDb.SIU_SafetyPays_TaskLists
                              select spList.IncidentNo).Distinct().ToList();

        return (from spList in nvDb.SIU_SafetyPaysReports
                where spList.IncStatus.ToLower() == "working" && (!incNo.Contains(spList.IncidentNo))
                select new SIU_SafetyPaysReport_Rpt(spList)
               ).ToList();
    }
    public static List<SIU_SafetyPaysReport_Rpt> GetSafetyPaysLateTask(string isAdmin)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        List<int> incNo;
        if (isAdmin == "0")
            incNo = (from spList in nvDb.SIU_SafetyPays_TaskLists
                           where spList.CompletedDate == null && spList.DueDate < DateTime.Now && spList.AssignedEmpID == BusinessLayer.UserEmpID
                           select spList.IncidentNo).Distinct().ToList();
        else
            incNo = (from spList in nvDb.SIU_SafetyPays_TaskLists
                     where spList.CompletedDate == null && spList.DueDate < DateTime.Now
                     select spList.IncidentNo).Distinct().ToList();        


        return (from spList in nvDb.SIU_SafetyPaysReports
                where spList.IncStatus.ToLower() == "working" && (incNo.Contains(spList.IncidentNo))
                select new SIU_SafetyPaysReport_Rpt(spList)
               ).ToList();
    }
    public static List<SIU_SafetyPaysReport_Rpt> GetSafetyPaysLateStatus(string isAdmin)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        List<int> incNo;
        if (isAdmin == "0")
        {
            List<int> incNoSta = (from spList2 in nvDb.SIU_SafetyPays_TaskStatus
                                  where spList2.ResponseDate == null
                                  select spList2.IncidentNo).Distinct().ToList();

            incNo = (from spList in nvDb.SIU_SafetyPays_TaskLists
                     where spList.CompletedDate == null && spList.AssignedEmpID == BusinessLayer.UserEmpID
                     && (incNoSta.Contains(spList.IncidentNo))
                     select spList.IncidentNo).Distinct().ToList();
        }
        else
            incNo = (from spList in nvDb.SIU_SafetyPays_TaskStatus
                     where spList.ResponseDate == null
                     select spList.IncidentNo).Distinct().ToList();

        return (from spList in nvDb.SIU_SafetyPaysReports
                where spList.IncStatus.ToLower() == "working" && (incNo.Contains(spList.IncidentNo))
                select new SIU_SafetyPaysReport_Rpt(spList)
               ).ToList();
    }
    public static List<SIU_SafetyPaysReport_Rpt> GetSafetyPaysCloseReadyStatus()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        List<int> incNoNotComplete = (  from spList in nvDb.SIU_SafetyPays_TaskLists
                                        where spList.CompletedDate == null 
                                        select spList.IncidentNo).Distinct().ToList();

        List<int> incNoStarted = (      from spList in nvDb.SIU_SafetyPays_TaskLists
                                        select spList.IncidentNo).Distinct().ToList();

        List<int> incNoLateTask = (     from spList in nvDb.SIU_SafetyPays_TaskLists
                                        where spList.CompletedDate == null && spList.DueDate < DateTime.Now
                                        select spList.IncidentNo).Distinct().ToList();



        return (from spList in nvDb.SIU_SafetyPaysReports
                where spList.IncStatus.ToLower() == "working" &&
                    (incNoStarted.Contains(spList.IncidentNo)) &&
                     ( ! incNoNotComplete.Contains(spList.IncidentNo)) &&
                     ( ! incNoLateTask.Contains(spList.IncidentNo)) 
                select new SIU_SafetyPaysReport_Rpt(spList)
               ).ToList();
    }
    public static List<SIU_SafetyPaysReport_Rpt> GetSafetyPaysCurrentStatus()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        List<int> incNoNotComplete = (from spList in nvDb.SIU_SafetyPays_TaskLists
                                      where spList.CompletedDate == null
                                      select spList.IncidentNo).Distinct().ToList();

        List<int> incNoStarted = (from spList in nvDb.SIU_SafetyPays_TaskLists
                                  select spList.IncidentNo).Distinct().ToList();

        return (from spList in nvDb.SIU_SafetyPaysReports
                where spList.IncStatus.ToLower() == "working" &&
                    (incNoStarted.Contains(spList.IncidentNo)) &&
                     (!incNoNotComplete.Contains(spList.IncidentNo))
                select new SIU_SafetyPaysReport_Rpt(spList)
               ).ToList();
    }
    public static List<SIU_SafetyPaysReport_Rpt> GetNewSafetyPaysRpts(int startIndex, int count, string SortBy)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from spList in nvDb.SIU_SafetyPaysReports
                  where spList.IncStatus == "New"
                  select new SIU_SafetyPaysReport_Rpt(spList)

               ).ToList().OrderBy(SortBy).Skip(startIndex).Take(count).ToList();
    }
    public static List<SIU_SafetyPaysReport_Rpt> GetNewSafetyPaysRpts(int startIndex, int count)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from spList in nvDb.SIU_SafetyPaysReports
                where spList.IncStatus == "New"
                select new SIU_SafetyPaysReport_Rpt(spList)
               ).Skip(startIndex).Take(count).ToList();
    }

    public static int GetNewSafetyPaysRptsCount()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from spList in nvDb.SIU_SafetyPaysReports
                where spList.IncStatus == "New"
                select spList
               ).Count();
    }

    public static List<SIU_SafetyPaysReport_Rpt> GetAllWorkingTasksSafetyPaysRpts()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from spList in nvDb.SIU_SafetyPaysReports
                where spList.IncStatus.ToLower() == "working"
                select new SIU_SafetyPaysReport_Rpt(spList)
               ).ToList();
    }
    public static List<SIU_SafetyPaysReport_Rpt> GetAssignedWorkingTasksSafetyPaysRpts()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        List<int> incNo = (from spList in nvDb.SIU_SafetyPays_TaskLists
                           where spList.CompletedDate == null && spList.AssignedEmpID == BusinessLayer.UserEmpID
                           select spList.IncidentNo).Distinct().ToList();

        return (from spList in nvDb.SIU_SafetyPaysReports
                where spList.IncStatus.ToLower() == "working" && (incNo.Contains(spList.IncidentNo))
                select new SIU_SafetyPaysReport_Rpt(spList)
               ).ToList();
    }
    
    /////////////////////////////////////////////////////////////////
    // Get A Safety Pays Report                                    // 
    // Called Internally For Adding New Report                     //
    // Called When Building Emails                                 //
    // Called Internally And Converted To SIU_SafetyPaysReport_Rpt //
    //     For Managing Safety Pays Tasks                          //
    /////////////////////////////////////////////////////////////////
    public static IEnumerable<SIU_SafetyPaysReport> GetSafetyPaysReport(int _incidentNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from reports in nvDb.SIU_SafetyPaysReports
                where reports.IncidentNo == _incidentNo
                select reports
               );
    }

    //////////////////////////////////////////////////////
    // Data Support For Management Of Safety Pays Tasks //
    //////////////////////////////////////////////////////
    public static IEnumerable<SIU_SafetyPays_TaskList> GetSafetyPaysTasks(int _incidentNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from spTaskList in nvDb.SIU_SafetyPays_TaskLists
                where spTaskList.IncidentNo == _incidentNo
                select spTaskList
               );
    }
    public static List<SIU_SafetyPays_TaskList_Rpt> GetSafetyPaysTasksRpt(int _incidentNo, string isAdmin="1")
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        if ( isAdmin == "1")
            return (from spTaskList in nvDb.SIU_SafetyPays_TaskLists
                    where spTaskList.IncidentNo == _incidentNo
                    select new SIU_SafetyPays_TaskList_Rpt( spTaskList )
                   ).ToList();

        return (from spTaskList in nvDb.SIU_SafetyPays_TaskLists
                where spTaskList.IncidentNo == _incidentNo && spTaskList.AssignedEmpID == BusinessLayer.UserEmpID
                select new SIU_SafetyPays_TaskList_Rpt(spTaskList)
               ).ToList();
    }
    public static IEnumerable<SIU_SafetyPays_TaskStatus> GetSafetyPaysTaskStatus(int _incidentNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from spTaskList in nvDb.SIU_SafetyPays_TaskStatus
                where spTaskList.IncidentNo == _incidentNo
                select spTaskList
               );
    }
    public static List<SIU_SafetyPays_TaskStatus> GetSafetyPaysTaskStatusRpt(int _incidentNo, int _TaskNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from spTaskList in nvDb.SIU_SafetyPays_TaskStatus
                where spTaskList.IncidentNo == _incidentNo && spTaskList.TaskNo == _TaskNo
                select spTaskList
               ).ToList();
    }


    public static SIU_SafetyPaysReport RecordSafetyPaysStatus(int RcdID, string NewStatus, string ModEmpID, bool Closed, int Points, string ehsRepsonse = "")
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        SIU_SafetyPaysReport spRcd = GetSafetyPaysReport(RcdID).First();
        nvDb.SIU_SafetyPaysReports.Attach(spRcd);

        spRcd.IncLastTouchEmpID = ModEmpID;
        spRcd.IncLastTouchTimestamp = DateTime.Now;
        spRcd.IncStatus = NewStatus;


        if (spRcd.PointsAssigned == null) 
            spRcd.PointsAssigned = 0;

        if ( spRcd.PointsAssigned == 0 && Points > 0)
        {
            spRcd.PointsAssignedTimeStamp = DateTime.Now;
            spRcd.PointsAssigned = Points;
        }

        if ( Closed )
            spRcd.IncCloseTimestamp = DateTime.Now;

        if (ehsRepsonse.Length > 0)
        {
            spRcd.ehsRepsonse = ehsRepsonse;
        }

        nvDb.SubmitChanges();

        return spRcd;
    }
    public static SIU_SafetyPays_TaskList RecordSafetyPaysTask(SIU_SafetyPays_TaskList _Task)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        nvDb.SIU_SafetyPays_TaskLists.InsertOnSubmit(_Task);
        nvDb.SubmitChanges();

        return _Task;
    }
    public static SIU_SafetyPays_TaskStatus RecordSafetyPaysTaskStatus(SIU_SafetyPays_TaskStatus _Status)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        if (_Status.UID == 0)
        {
            nvDb.SIU_SafetyPays_TaskStatus.InsertOnSubmit(_Status);
            nvDb.SubmitChanges();
            return _Status;
        }

        SIU_SafetyPays_TaskStatus spStatus = (  from spStat in nvDb.SIU_SafetyPays_TaskStatus
            where spStat.UID == _Status.UID
            select spStat
            ).Single();
        spStatus.Response = _Status.Response;
        spStatus.ResponseDate = _Status.ResponseDate;
        nvDb.SubmitChanges();
        return spStatus;
    }
    public static SIU_SafetyPays_TaskList RecordSafetyPaysTaskComplete(int IncNo, int TaskNo, string CloseNotes)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        ////////////////////////////////////
        // Add Task Closing Status Report //
        ////////////////////////////////////
        SIU_SafetyPays_TaskStatus finalStatus = new SIU_SafetyPays_TaskStatus
        {
            IncidentNo = IncNo,
            TaskNo = TaskNo,
            ReqDate1 = DateTime.Now,
            Email1 = BusinessLayer.UserEmail,
            Response = CloseNotes,
            ResponseDate = DateTime.Now
        };
        nvDb.SIU_SafetyPays_TaskStatus.InsertOnSubmit(finalStatus);


        SIU_SafetyPays_TaskList task = (from updTask in nvDb.SIU_SafetyPays_TaskLists
                                         where updTask.IncidentNo == IncNo && updTask.TaskNo == TaskNo
                                         select updTask).SingleOrDefault();

        if (task != null)
        {
            task.CompletedDate = DateTime.Now;
            nvDb.SubmitChanges();
        }

        return task;
    }


    struct RecordSafetyPaysPointsObsData
    {
        public int PtCnt;
        public int UID;
    }
    public static void RecordSafetyPaysPoints(SIU_SafetyPaysReport rptRcd)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        if (rptRcd.PointsAssigned != null)
        {
            SIU_SafetyPays_Point newPts = new SIU_SafetyPays_Point
            {
                Emp_No = rptRcd.EmpID,
                DatePointsGiven = DateTime.Now,
                PointsGivenBy = rptRcd.IncLastTouchEmpID,
                Comments = "Awarded for " + rptRcd.IncTypeTxt,
                Points = (int)rptRcd.PointsAssigned,
                SPR_UID = rptRcd.IncidentNo,
                QOM_ID = rptRcd.QOM_ID
            };

            ///////////////////////////////////////////////////////////////
            // Figure Out Which Date To Use As The Event Date            //
            // The Data that Decided Which Period the Points Are Awarded //
            ///////////////////////////////////////////////////////////////
            if ( rptRcd.IncidentDate != null )
                newPts.EventDate = (DateTime)rptRcd.IncidentDate;
            else
                if ( rptRcd.SafetyMeetingDate != null )
                    newPts.EventDate = (DateTime)rptRcd.SafetyMeetingDate;
                else
                    newPts.EventDate = (DateTime)rptRcd.IncOpenTimestamp;

            ///////////////////////////////////////////////
            // Manually Convert SP Form Type to Rpt Type //
            ///////////////////////////////////////////////
            if ( rptRcd.IncTypeSafeFlag ) newPts.ReasonForPoints = 1;
            if ( rptRcd.IncTypeUnsafeFlag ) newPts.ReasonForPoints = 9;
            if ( rptRcd.IncTypeSuggFlag ) newPts.ReasonForPoints = 8;
            if ( rptRcd.IncTypeTopicFlag ) newPts.ReasonForPoints = 14;

            /////////////////////////////////////////////////////////////////
            // This is a hole. The Safety Form Does Not Make A distinction //
            // Between Admin and Non-Admin Meetings                        //
            /////////////////////////////////////////////////////////////////
            if ( rptRcd.IncTypeSumFlag )
            {
                var emp = GetEmployeeByNo(rptRcd.EmpID);
                if ( emp.Global_Dimension_1_Code.StartsWith("60") )
                    newPts.ReasonForPoints = 18;
                else
                    newPts.ReasonForPoints = 5;
                
            }



            //////////////////////////////////////////////////////////
            // If This is a QOM Record, Make Some Minor Adjustments //
            //////////////////////////////////////////////////////////
            if (rptRcd.QOM_ID > 0 )
            {
                if ( rptRcd.IncTypeTxt.ToLower().Contains("vest") )
                    newPts.ReasonForPoints = 22;
                else
                    newPts.ReasonForPoints = 6;
            }



            nvDb.SIU_SafetyPays_Points.InsertOnSubmit(newPts);

            ///////////////////////////////////////////////////////////////////////
            // If An Employee Was Identified In An "I Saw Something Safe" report //
            // Award that employee points as well                                //
            ///////////////////////////////////////////////////////////////////////
            if (rptRcd.IncTypeSafeFlag && rptRcd.ObservedEmpID.Length > 0)
            {

                RecordSafetyPaysPointsObsData obsData = (
                    from pc in nvDb.SIU_SafetyPays_Points_Types 
                    where pc.Description.Contains("Identified")
                    select new RecordSafetyPaysPointsObsData{ PtCnt = pc.PointsCount, UID = pc.UID }
                ).Take(1).SingleOrDefault();

                SIU_SafetyPays_Point newPtsObserved = new SIU_SafetyPays_Point
                {
                    Emp_No = rptRcd.ObservedEmpID,
                    DatePointsGiven = DateTime.Now,
                    PointsGivenBy = rptRcd.IncLastTouchEmpID,
                    Comments = "Awarded for Safety Pays Submission (Observed)",
                    Points = obsData.PtCnt,
                    EventDate = rptRcd.IncidentDate ?? rptRcd.IncOpenTimestamp,
                    ReasonForPoints = obsData.UID,
                    SPR_UID = rptRcd.IncidentNo
                };

                nvDb.SIU_SafetyPays_Points.InsertOnSubmit(newPtsObserved);

                WebMail.SafetyPaysObservedEMail(rptRcd, obsData.PtCnt);
            }

            nvDb.SubmitChanges();
        }

        
        
    }


    ////////////////////////////////////////////////////////////////////////
    // Data Support For EHS Admin manual Management Of Safety Pays Points //
    ////////////////////////////////////////////////////////////////////////
    //public static List<String> GetList_SafetyPaysStatuses()
    //{
    //    SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

    //    var s = (from statuses in nvDb.SIU_SafetyPaysReports
    //            select statuses.IncStatus
    //       ).Distinct().ToList();

    //    s.Insert(0, "");
    //    return s;
    //}
   
    public static int RecordAdminPoints(SIU_SafetyPays_Point _Pts)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        if (_Pts.UID > 0)
        {
            SIU_SafetyPays_Point changePtRcd = (from ptRcd in nvDb.SIU_SafetyPays_Points
                                                where ptRcd.UID == _Pts.UID
                                                select ptRcd).SingleOrDefault();

            if (changePtRcd == null) throw (new Exception("Failed To Locate Update Record for " + _Pts.UID));

            changePtRcd.Points = _Pts.Points;
            changePtRcd.ReasonForPoints = _Pts.ReasonForPoints;

            if (changePtRcd.SPR_UID != null)
            {
                if (changePtRcd.SPR_UID > 0)
                {
                    SIU_SafetyPaysReport changeRptRcd = (from rptRcd in nvDb.SIU_SafetyPaysReports
                                                         where rptRcd.IncidentNo == changePtRcd.SPR_UID
                                                         select rptRcd).SingleOrDefault();

                    changeRptRcd.PointsAssigned = _Pts.Points;
                    changeRptRcd.IncLastTouchTimestamp = DateTime.Now;
                    changeRptRcd.IncLastTouchEmpID = BusinessLayer.UserEmpID;
                    changeRptRcd.IncTypeTxt = Get_SafetyPaysType(_Pts.ReasonForPoints);
                }
            }

        }

        else
        {
            nvDb.SIU_SafetyPays_Points.InsertOnSubmit(_Pts);
        }

        nvDb.SubmitChanges();
        return _Pts.UID;
    }
    public static List<SIU_SafetyPays_Point> GetEmpIdPoints(string EmpID)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from rpt in nvDb.SIU_SafetyPays_Points
                where rpt.Emp_No == EmpID
                select rpt).ToList();
    }
    public static void RemoveEmpIdPoints(string UID)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            var rcd = (from ptRcd in nvDb.SIU_SafetyPays_Points
                       where ptRcd.UID == int.Parse(UID)
                       select ptRcd
                      ).SingleOrDefault();

            if (rcd == null) throw (new Exception("Unable To Locate Key: " + UID));

            nvDb.SIU_SafetyPays_Points.DeleteOnSubmit(rcd);
            nvDb.SubmitChanges();
        }
        catch (Exception ex)
        {
            LogDebug("RemoveEmpIdPoints", ex.Message);
        }
    }


    /////////////////////////////////////////////////////////////
    // Data Support For EHS Admin Safety Pays Points Reporting //
    /////////////////////////////////////////////////////////////
    public static List<SIU_SafetyPays_Points_Type> GetAutoCompletePointTypes()
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from typeList in nvDb.SIU_SafetyPays_Points_Types
                        select typeList
                       ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetAutoCompletePointTypes", ex.Message);
        }
        return new List<SIU_SafetyPays_Points_Type>();
    }
    public static List<SIU_SafetyPays_Points_Prjd_Std> GetStdProjectedPoints()
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from prjPts in nvDb.SIU_SafetyPays_Points_Prjd_Stds
                        orderby prjPts.Dept, prjPts.PtsType
                        select prjPts
                       ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetStdProjectedPoints", ex.Message);
        }
        return new List<SIU_SafetyPays_Points_Prjd_Std>();
    }
    public static List<SIU_SafetyPays_Points_Prjd> GetProjectedPoints(DateTime start, DateTime end)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {

                return (from prjPts in nvDb.SIU_SafetyPays_Points_Prjds
                        where prjPts.yr >= start.Year && prjPts.mon >= start.Month &&
                              prjPts.yr <= end.Year && prjPts.mon <= end.Month
                        orderby prjPts.Dept, prjPts.yr, prjPts.mon, prjPts.PtsType
                        select prjPts
                       ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetProjectedPoints", ex.Message);
        }
        return new List<SIU_SafetyPays_Points_Prjd>();
    }
    public static string Get_SafetyPaysType(int ReasonForPoints)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from types in nvDb.SIU_SafetyPays_Points_Types
                where types.UID == ReasonForPoints
                select types.Description
               ).Single();
    }
    public static List<SIU_Points_Rpt> GetAdminPointsRptEmpPoints(DateTime start, DateTime end, string EmpNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (    from rptData in nvDb.SIU_SafetyPays_Points
                    join emp in nvDb.Shermco_Employees on rptData.Emp_No equals emp.No_
                    join ptType in nvDb.SIU_SafetyPays_Points_Types on rptData.ReasonForPoints equals ptType.UID
                    where rptData.EventDate >= start && rptData.EventDate <= end && emp.No_ == EmpNo
                    orderby emp.Global_Dimension_1_Code descending
                    select (new SIU_Points_Rpt(rptData, emp.No_, emp.Global_Dimension_1_Code, emp.Last_Name + ", " + emp.First_Name, ptType.Description))
                ).ToList();
    }
    public static List<SIU_Points_Rpt> GetAdminPointsRptEmpPoints(DateTime start, DateTime end)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        //return (from rptData in nvDb.SIU_SafetyPays_Points
        //        join emp in nvDb.Shermco_Employees on rptData.Emp_No equals emp.No_
        //        join ptType in nvDb.SIU_SafetyPays_Points_Types on rptData.ReasonForPoints equals ptType.UID
        //        where rptData.EventDate >= start && rptData.EventDate <= end
        //        orderby emp.Global_Dimension_1_Code ascending, emp.No_ ascending, emp.Last_Name ascending
        //        select (new SIU_Points_Rpt(rptData, emp.No_, emp.Global_Dimension_1_Code, emp.Last_Name + ", " + emp.First_Name, ptType.Description))
        //        ).ToList();

        return (from t1 in nvDb.Shermco_Employees
                join t0 in nvDb.SIU_SafetyPays_Points on new { No_ = t1.No_ } equals new { No_ = t0.Emp_No } into t0_join
                where t1.Status == 0 && t1.Blocked == 0

                from t0 in t0_join.DefaultIfEmpty()
                join t2 in nvDb.SIU_SafetyPays_Points_Types on new { ReasonForPoints = t0.ReasonForPoints } equals new { ReasonForPoints = t2.UID } into t2_join

                from t2 in t2_join.DefaultIfEmpty()
                where (t0.EventDate >= start && t0.EventDate <= end) || t0.EventDate == null

                orderby t1.Global_Dimension_1_Code ascending, t1.No_ ascending, t1.Last_Name ascending
                select (new SIU_Points_Rpt(t0, t1.No_, t1.Global_Dimension_1_Code, t1.Last_Name + ", " + t1.First_Name, t2.Description))
                 ).ToList();
    }
    public static List<SIU_Points_Rpt> GetAdminPointsRptEmpPointsFromProd(DateTime start, DateTime end)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(ForcedProductionConnectString);

        //return (from rptData in nvDb.SIU_SafetyPays_Points
        //        join emp in nvDb.Shermco_Employees on rptData.Emp_No equals emp.No_
        //        join ptType in nvDb.SIU_SafetyPays_Points_Types on rptData.ReasonForPoints equals ptType.UID
        //        where rptData.EventDate >= start && rptData.EventDate <= end
        //        orderby emp.Global_Dimension_1_Code ascending, emp.No_ ascending, emp.Last_Name ascending
        //        select (new SIU_Points_Rpt(rptData, emp.Global_Dimension_1_Code, emp.Last_Name + ", " + emp.First_Name, ptType.Description))
        //        ).ToList();

       return ( from t1 in nvDb.Shermco_Employees
                join t0 in nvDb.SIU_SafetyPays_Points on new { No_ = t1.No_ } equals new { No_ = t0.Emp_No } into t0_join
                where t1.Status == 0 && t1.Blocked == 0

                from t0 in t0_join.DefaultIfEmpty()
                join t2 in nvDb.SIU_SafetyPays_Points_Types on new { ReasonForPoints = t0.ReasonForPoints } equals new { ReasonForPoints = t2.UID } into t2_join

                from t2 in t2_join.DefaultIfEmpty()
                where (t0.EventDate >= start && t0.EventDate <= end) || t0.EventDate == null

                orderby t1.Global_Dimension_1_Code ascending, t1.No_ ascending, t1.Last_Name ascending
                select (new SIU_Points_Rpt(t0, t1.No_, t1.Global_Dimension_1_Code, t1.Last_Name + ", " + t1.First_Name, t2.Description))
                ).ToList();

    }
    public static List<SIU_Points_Rpt> GetAdminPointsRptDepts(string startDate, string endDate)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        DateTime start = DateTime.Parse(startDate);
        DateTime end = DateTime.Parse(endDate);

        return (    from rptData in nvDb.SIU_SafetyPays_Points
                    join emp in nvDb.Shermco_Employees on rptData.Emp_No equals emp.No_
                    where rptData.EventDate >= start && rptData.EventDate <= end

                    group rptData.Points by emp.Global_Dimension_1_Code
                    into deptSums
                    select new SIU_Points_Rpt
                                {
                                    EmpDept = deptSums.Key,
                                    Points =  deptSums.Sum()
                                }
                ).ToList();
    }
    public static List<SIU_Points_Rpt> GetAdminPointsRptEmps(string startDate, string endDate, string dept)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        DateTime start = DateTime.Parse(startDate);
        DateTime end = DateTime.Parse(endDate);

        return (    from rptData in nvDb.SIU_SafetyPays_Points
                    join emp in nvDb.Shermco_Employees on rptData.Emp_No equals emp.No_
                    where rptData.EventDate >= start && rptData.EventDate <= end && emp.Global_Dimension_1_Code == dept

                    group rptData.Points by new { emp.No_, emp.Last_Name, emp.First_Name, emp.Global_Dimension_1_Code }
                    into empSums
                    select new SIU_Points_Rpt
                                {
                                    Emp_No = empSums.Key.No_,
                                    EmpName = empSums.Key.Last_Name + ", " + empSums.Key.First_Name,
                                    EmpDept = empSums.Key.Global_Dimension_1_Code,
                                    Points = empSums.Sum()
                                }
                ).ToList();
    }
    public static List<SIU_Points_Rpt> GetAdminPointsRptEmps(string startDate, string endDate)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        DateTime start = DateTime.Parse(startDate);
        DateTime end = DateTime.Parse(endDate);

        return (from rptData in nvDb.SIU_SafetyPays_Points
                join emp in nvDb.Shermco_Employees on rptData.Emp_No equals emp.No_
                where rptData.EventDate >= start && rptData.EventDate <= end
                
                group rptData.Points by new { emp.No_, emp.Last_Name, emp.First_Name, emp.Global_Dimension_1_Code }
                    into empSums
                    select new SIU_Points_Rpt
                    {
                        Emp_No = empSums.Key.No_,
                        EmpName = empSums.Key.Last_Name + ", " + empSums.Key.First_Name,
                        EmpDept = empSums.Key.Global_Dimension_1_Code,
                        Points = empSums.Sum()
                    }
                ).ToList();
    }

    public static List<SIU_SafetyPays_AcceptReject_Rpt_SPResult> GetSafetyPaysDeptRpt(DateTime startDate, DateTime endDate)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from cntrs in nvDb.SIU_SafetyPays_AcceptReject_Rpt_SP(startDate, endDate)
                        select cntrs
                       ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetSafetyPaysDeptRpt", ex.Message);
        }

        return new List<SIU_SafetyPays_AcceptReject_Rpt_SPResult>();

    }
    public static List< Tuple<SIU_SafetyPays_Point, SIU_SafetyPaysReport>> GetSafetyPaysRawDataRpt(DateTime startDate, DateTime endDate)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return new List<Tuple<SIU_SafetyPays_Point, SIU_SafetyPaysReport>>
                     (from p in nvDb.SIU_SafetyPays_Points
                        where p.EventDate >= startDate && p.EventDate <= endDate
                        join r in nvDb.SIU_SafetyPaysReports on new { SPR_UID = Convert.ToInt32(p.SPR_UID) } equals new { SPR_UID = r.IncidentNo } into r_join
                        from r in r_join.DefaultIfEmpty()
                        select Tuple.Create(p,r)); //.ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetSafetyPaysRawDataRpt", ex.Message);
        }

        return new List<Tuple<SIU_SafetyPays_Point, SIU_SafetyPaysReport>>();

    }
#endregion

#region Safety Question Of the Month Methods

    ////////////////////////////////////
    // Called by JTable on Admin Page //
    ////////////////////////////////////
    public static List<SIU_Safety_MoQ> GetSafetyQomQList()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        DateTime periodEnd = DateTime.Now.AddDays(1);

        return (from qomList in nvDb.SIU_Safety_MoQs
                where qomList.EndDate >= periodEnd
                select qomList
               ).ToList();        
    }

    ////////////////////////////////////////////////////////////////
    // Lookup Active Qom Questions and Combine With And Responses //
    // Submitted by User Combined With Any Awarded Points         //
    ////////////////////////////////////////////////////////////////
    public static List<SIU_Qom_QR> GetSafetyQomQRList(string Eid)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        DateTime periodEnd = DateTime.Now;

        ///////////////////////////
        // Check For Admin Priv. //
        ///////////////////////////
        StringCollection sessionVar = (StringCollection)HttpContext.Current.Session["UserGroups"];
        if (sessionVar != null)
            if (sessionVar.Contains("ShermcoYou_Safety_Pays") || sessionVar.Contains("EHS_TEST"))
                periodEnd = DateTime.Now.AddDays(-30);

        var responses =
            from respData in nvDb.SIU_SafetyPaysReports
            where respData.EmpID == Eid && respData.IncTypeTxt.Contains("QOM")
            select respData;

        var questionResponse =
            from qomQ in nvDb.SIU_Safety_MoQs
            join empRespData in responses on qomQ.Q_Id equals empRespData.QOM_ID into oj
            from joinedData in oj.DefaultIfEmpty()
            where DateTime.Now >= qomQ.StartDate && DateTime.Now <= qomQ.EndDate.AddDays(1)
            join spp in nvDb.SIU_SafetyPays_Points on joinedData.IncidentNo equals spp.SPR_UID into oj2
            from allData in oj2.DefaultIfEmpty()
            select new SIU_Qom_QR(qomQ, joinedData, allData);

        return questionResponse.ToList();
    }
    public static List<SIU_Qom_QR> GetSafetyQomQHList(string Eid)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        DateTime periodEnd = DateTime.Now.AddDays(-180);

        var responses =
            from respData in nvDb.SIU_SafetyPaysReports
            where respData.EmpID == BusinessLayer.UserEmpID && respData.IncTypeTxt.Contains("QOM")
            select respData;

        var questionResponse =
            from qomQ in nvDb.SIU_Safety_MoQs
            join empRespData in responses on qomQ.Q_Id equals empRespData.QOM_ID into oj
            from joinedData in oj.DefaultIfEmpty()
            where qomQ.StartDate <= DateTime.Now && qomQ.EndDate.AddDays(1) >= periodEnd
            join spp in nvDb.SIU_SafetyPays_Points on joinedData.IncidentNo equals spp.SPR_UID into oj2
            from allData in oj2.DefaultIfEmpty()
            select new SIU_Qom_QR(qomQ, joinedData, allData);

        return questionResponse.ToList();
    }
    public static SIU_Safety_MoQ GetSafetyQomQ(int QiD)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        if (QiD > 0)
        {
            return (from qomList in nvDb.SIU_Safety_MoQs
                    where qomList.Q_Id == QiD
                    select qomList
                   ).SingleOrDefault();
        }


        DateTime cd = DateTime.Now;
        SIU_Safety_MoQ q = null;
        try
        {
            q = (   from qomList in nvDb.SIU_Safety_MoQs
                    where qomList.StartDate.DayOfYear <= cd.DayOfYear && qomList.EndDate.DayOfYear >= cd.DayOfYear
                    select qomList
                ).SingleOrDefault();
        }
        catch (Exception ex)
        {
            LogDebug("GetSafetyQomQ", ex.Message);
        }

        return q;
    }
    public static int CheckSafetyQomDateRange(DateTime Start, DateTime End)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (    from qomList in nvDb.SIU_Safety_MoQs
                    where (Start >= qomList.StartDate && Start <= qomList.EndDate) ||
                          (End   >= qomList.StartDate && End   <= qomList.EndDate)
                    select qomList.Q_Id
               ).Count();
    }
    public static int RecordSafetyQomQuestion(SIU_Safety_MoQ _question)
    {
        SIU_Safety_MoQ newQomQ = null;

        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);
            
            /////////////////////////////////////////////
            // If This Is A New Record, Perform Insert //
            /////////////////////////////////////////////
            if (_question.Q_Id == 0)
            {
                nvDb.SIU_Safety_MoQs.InsertOnSubmit(_question);
            }

            /////////////////////////////////
            // Otherwise Perform An Update //
            /////////////////////////////////
            else
            {
                /////////////////////////////////
                // Look For An Existing Record //
                /////////////////////////////////
                newQomQ = GetSafetyQomQ(_question.Q_Id);

                //////////////////////////
                // Start Update Process //
                //////////////////////////
                nvDb.SIU_Safety_MoQs.Attach(newQomQ);

                ///////////////////////////////
                // Copy Over New Data Fields //
                ///////////////////////////////
                Mapper.CreateMap<SIU_Safety_MoQ, SIU_Safety_MoQ>();
                Mapper.Map(_question, newQomQ);


            }
            nvDb.SubmitChanges();

            
        }
        catch (Exception ex)
        {
            LogDebug("RecordSafetyQomQuestion", ex.Message);
        }

        return (newQomQ == null) ? _question.Q_Id : newQomQ.Q_Id;
    }
    public static void RemoveSafetyQomQuestion(string _UID)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            var rcd = (from te in nvDb.SIU_Safety_MoQs
                       where te.Q_Id == int.Parse(_UID)
                       select te
                      ).SingleOrDefault();

            if (rcd == null) throw (new Exception(" Unable To Locate Key: " + _UID));

            nvDb.SIU_Safety_MoQs.DeleteOnSubmit(rcd);
            nvDb.SubmitChanges();
        }
        catch (Exception ex)
        {
            LogDebug("RemoveSafetyQomQuestion", ex.Message);
        }
    }
#endregion

#region Incident Accident
    public static IEnumerable<object> GetIncidentAccident()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        var rcds = (    from iaList in nvDb.SIU_Incident_Accidents
                        join emp in nvDb.Shermco_Employees on iaList.Emp_ID equals emp.No_ into outerJoin

                        from iaList2 in outerJoin.DefaultIfEmpty() 
                        join supr in nvDb.Shermco_Employees on iaList2.Manager_No_ equals supr.No_ into outerJoin2

                        from iaList3 in outerJoin2.DefaultIfEmpty() 
                        select new { 	iaList, 
				                        EmpNo = iaList2.No_, EmpLast = iaList2.Last_Name, EmpFirst = iaList2.First_Name, EmpDept = iaList2.Global_Dimension_1_Code,
				                        SuprNo = iaList3.No_, SuprLast = iaList3.Last_Name, SuprFirst = iaList3.First_Name
			                        }
                    );

        return rcds;
    }

    public static List<object> GetIncidentAccidentOpen()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return new List<object>
        {(  from iaList in nvDb.SIU_Incident_Accidents
            join emp in nvDb.Shermco_Employees on iaList.Emp_ID equals emp.No_ into outerJoin
            
            from iaList2 in outerJoin.DefaultIfEmpty()  // Left Outer Join
            where (new List<string>(){ "Open", "Submit"}).Contains(iaList.Disposition)

            select new { iaList, iaList2.Last_Name, iaList2.Global_Dimension_1_Code, iaList2.First_Name }
            ).ToList()};
    }

    public static List<object> GetIncidentAccidentSubmit()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return new List<object>
        {(  from iaList in nvDb.SIU_Incident_Accidents
            join emp in nvDb.Shermco_Employees on iaList.Emp_ID equals emp.No_ into outerJoin
            
            from iaList2 in outerJoin.DefaultIfEmpty()  // Left Outer Join
            where iaList.Disposition == "Submit"

            select new { iaList, iaList2.Last_Name, iaList2.Global_Dimension_1_Code, iaList2.First_Name }
            ).ToList()};
    }


    public static List<object> GetIncidentAccidentApprovalNotes(int UID)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return new List<object>
        {
            (   from incNotes in nvDb.SIU_Incident_Accident_AppovalNotes
                join emp in nvDb.Shermco_Employees on incNotes.EID equals emp.No_

                where incNotes.Ref_UID == UID 

				select new
				{
					Comment = incNotes.Comments,
					CommentDate = incNotes.TimeStamp, 
                    CommentsBy = emp.Last_Name + ", " + emp.First_Name,
                    incNotes.Ref_UID
				}	
            ).ToList()
        };
    }
    public static void RecordIncidentAccidentApprovalNote(ref SIU_Incident_Accident_AppovalNote ApprovalNote)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        try
        {
            nvDb.SIU_Incident_Accident_AppovalNotes.InsertOnSubmit(ApprovalNote);
            nvDb.SubmitChanges();
        }
        catch (Exception ex)
        {
            LogDebug("SIU_DAO.RecordIncidentAccidentApprovalNote", ex.Message);
            throw;
        }       
    }

    public static SIU_Incident_Accident GetIncidentAccident(int _UID)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return ( from rcd in nvDb.SIU_Incident_Accidents
                 where rcd.UID == _UID
                 select rcd).SingleOrDefault();
    }
    public static int recordIncidentAccident(SIU_Incident_Accident _IncRcd)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);
        SIU_Incident_Accident newRcd = null;

        /////////////////////////////////////////////
        // If This Is A New Record, Perform Insert //
        /////////////////////////////////////////////
        if (_IncRcd.UID == 0)
        {
            nvDb.SIU_Incident_Accidents.InsertOnSubmit(_IncRcd);
        }



        /////////////////////////////////
        // Otherwise Perform An Update //
        /////////////////////////////////
        else
        {
            /////////////////////////////////
            // Look For An Existing Record //
            /////////////////////////////////
            newRcd = GetIncidentAccident(_IncRcd.UID);

            if (newRcd == null) throw (new Exception("Unable To Locate Key: " + _IncRcd.UID));

            //////////////////////////
            // Start Update Process //
            //////////////////////////
            nvDb.SIU_Incident_Accidents.Attach(newRcd);

            ///////////////////////////////
            // Copy Over New Data Fields //
            ///////////////////////////////
            Mapper.CreateMap<SIU_Incident_Accident, SIU_Incident_Accident>();
            Mapper.Map(_IncRcd, newRcd);


        }
        nvDb.SubmitChanges();

        return (newRcd == null) ? _IncRcd.UID : newRcd.UID;
    }
    public static void removeIncidentApproval(int _UID)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        try
        {
            nvDb.ExecuteCommand("DELETE FROM SIU_Incident_Accident_Appoval WHERE Ref_UID = {0}", _UID);
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("submitIremoveIncidentApprovalncidentAccident", ex.Message);
            throw;
        }
    }
    public static void recordIncidentApproval(int _UID)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        try
        {

            SIU_Incident_Accident_Appoval appRcd = new SIU_Incident_Accident_Appoval
            {
                EID = BusinessLayer.UserEmpID,
                Ref_UID = _UID,
                TimeStamp = DateTime.Now
            };

            nvDb.SIU_Incident_Accident_Appovals.InsertOnSubmit(appRcd);
            nvDb.SubmitChanges();        
        }
        catch (Exception ex)
        {
            LogDebug("SIU_DAO.recordIncidentApproval", ex.Message);
            throw;
        }
    }
    public static List<SIU_Incident_Accident_Appoval> GetIncidentAccidentApprovals(int _UID)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            return (from appRcds in nvDb.SIU_Incident_Accident_Appovals
                    where appRcds.Ref_UID == _UID
                    select appRcds
                    ).ToList();
        }
        catch (Exception ex)
        {
            LogDebug("GetIncidentAccidentApprovals", ex.Message);
            throw;
        }
    }
#endregion Incident Accident

#region Summary Counts  Xtn Ex
    public static SIU_SummaryCountsResult GetSafetyPaysSummaryCounts()
    {
// No Indexes Suggested
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from cntrs in nvDb.SIU_SummaryCounts()
                        select cntrs
                       ).SingleOrDefault();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetSafetyPaysSummaryCounts", ex.Message);
        }

        return new SIU_SummaryCountsResult();

    }
    public static SIU_MySi_SummaryCountsResult GetMySiSummaryCounts(string EmpID)
    {
//CREATE NONCLUSTERED INDEX [SIU_Job_1]
//ON [dbo].[Shermco$Job] ( [Status], [Is Master Job] )
//INCLUDE ( [No_], [Search Description], [Site Location])
//GO

//CREATE NONCLUSTERED INDEX [SIU_TSE_1]
//ON [dbo].[Shermco$Time Sheet Entry] ([Employee No_],[Work Date])
//INCLUDE ([Entry No_],[Status],[Shortcut Dimension 1 Code],[Job No_],[Task Code],[Pay Posting Group],[Straight Time],[Over Time],[Double Time],[Absence Time],[Holiday Time])
//GO

//CREATE NONCLUSTERED INDEX [SIU_PLE_1]
//ON [dbo].[Shermco$Payroll Ledger Entry] ([Employee No_])
//INCLUDE ([Amount],[Payroll Control Code],[Payroll Control Name])
//GO

//CREATE NONCLUSTERED INDEX [SIU_VEH_1]
//ON [dbo].[Shermco$Vehicle] ([Driver])
//GO

//CREATE NONCLUSTERED INDEX [SIU_JOBRPT_1]
//ON [dbo].[Shermco$Job Report] ([Complete and Delivered],[Turned in By Emp_ No_])
//GO

//CREATE NONCLUSTERED INDEX [SIU_JOB_2]
//ON [dbo].[Shermco$Job] ([Reporting Not Necessary],[Lead Tech],[Date Job Went to Cost])
//INCLUDE ([No_])
//GO

//CREATE NONCLUSTERED INDEX [SIU_MeetingLog_1]
//ON [dbo].[Z_MeetingLog] ([EmpNo],[DateCompleted],[Topic],[Date])
//INCLUDE ([DivDep])
//GO

//CREATE NONCLUSTERED INDEX [SIU_Employee_1]
//ON [dbo].[Shermco$Employee] ([Termination Date],[Temporary Employee],[Contractor],[Safety Meeting])
//GO
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from counts in nvDb.SIU_MySi_SummaryCounts(EmpID)
                        select counts
                       ).SingleOrDefault();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetMySiSummaryCounts", ex.Message);
        }

        return new SIU_MySi_SummaryCountsResult();
    }
#endregion

#region Bug Report 
    public static List<SIU_TaskList> GetBugReportList()
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (    from reports in nvDb.SIU_TaskLists
                    where reports.CloseTimeStamp >= DateTime.Now.AddMonths(-1) || reports.CloseTimeStamp == null
                    select reports
               ).ToList();
    }
    public static IEnumerable<SIU_TaskList> GetBugReport(int _incidentNo)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (    from reports in nvDb.SIU_TaskLists
                    where reports.IncidentNo == _incidentNo
                    select reports
               );
    }
    public static int RecordBugReport(SIU_TaskList _report)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);
        SIU_TaskList newReport = null;

        /////////////////////////////////////////////
        // If This Is A New Record, Perform Insert //
        /////////////////////////////////////////////
        if (_report.IncidentNo == 0)
        {
            nvDb.SIU_TaskLists.InsertOnSubmit(_report);
        }



        /////////////////////////////////
        // Otherwise Perform An Update //
        /////////////////////////////////
        else
        {
            /////////////////////////////////
            // Look For An Existing Record //
            /////////////////////////////////
            newReport = GetBugReport(_report.IncidentNo).SingleOrDefault();

            //////////////////////////
            // Start Update Process //
            //////////////////////////
            nvDb.SIU_TaskLists.Attach(newReport);

            ///////////////////////////////
            // Copy Over New Data Fields //
            ///////////////////////////////
            Mapper.CreateMap<SIU_SafetyPaysReport, SIU_SafetyPaysReport>();
            Mapper.Map(_report, newReport);


        }
        nvDb.SubmitChanges();

        return (newReport == null) ? _report.IncidentNo : newReport.IncidentNo;
    }
    public static SIU_TaskList RecordBugStatus(char Status, int IncidentNo, string OptRejectNotes = "")
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);
        SIU_TaskList updBug;

        /////////////////////////////////
        // Get Existing Record //
        /////////////////////////////////
        updBug = GetBugReport(IncidentNo).SingleOrDefault();
        if (updBug == null)
            return null;

        ////////////////////////////
        //// Start Update Process //
        ////////////////////////////
        nvDb.SIU_TaskLists.Attach(updBug);

        switch (Status)
        {
            case 'A':
                updBug.AcceptTimeStamp = DateTime.Now;
                break;

            case 'R':
                updBug.CloseTimeStamp = DateTime.Now;
                break;

            case 'W':
                updBug.WorkTimeStamp = DateTime.Now;
                break;

            case 'T':
                updBug.TestingTimeStamp = DateTime.Now;
                updBug.TestRejectionNotes = null;
                updBug.TestRejectionTimeStamp = null;
                break;

            case 'X':
                updBug.TestRejectionTimeStamp = DateTime.Now;
                updBug.TestRejectionNotes = OptRejectNotes;
                break;

            case 'C':
                updBug.CloseTimeStamp = DateTime.Now;
                break;
        }

        nvDb.SubmitChanges();

        return updBug;
    }
    public static List<BugReport_Report> GetMyOpenBugs(string EmpID)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from rptData in nvDb.SIU_TaskLists
                where rptData.EmpID == EmpID
                select new BugReport_Report(rptData)
               ).ToList();        
    }
#endregion

#region Blogs
    public static IEnumerable<SIU_Blog> GetBlogsAdvertised(string BlogName)
    {

// No Indexes Sugegsted

        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from blogs in nvDb.SIU_Blogs
                    where
                        blogs.BlogName == BlogName &&
                        blogs.Advertise && blogs.Advertise_End > DateTime.Now &&
                        (blogs.Advertise_Start == null || blogs.Advertise_Start < DateTime.Now)
                    select blogs
                    );
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetBlogs", ex.Message);
        }

        return new BindingList<SIU_Blog>();

    }

    ////////////////////////////////////
    // Return List Of Items In A Blog //
    ////////////////////////////////////
    public static List<SIU_Blog> GetBlogs(string BlogName)
    {

// No Indexes Suggested

        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from blogs in nvDb.SIU_Blogs
                    where
                        blogs.BlogName == BlogName
                    select blogs
                    ).ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("GetBlogs", ex.Message);
        }

        return new List<SIU_Blog>();

    }

    //////////////////////////
    // Return List If Blogs //
    //////////////////////////
    public static List<string> ListBlogs()
    {
// No Indexes Suggested
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                return (from blogs in nvDb.SIU_Blogs
                    select blogs.BlogName
                    ).Distinct().ToList();
            }
        }
        catch (Exception ex)
        {
            LogDebug("ListBlogs", ex.Message);
        }

        return new List<string>();
    }

    public static int RecordBlog(SIU_Blog BlogEntry)
    {
        try
        {
            SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

            TransactionOptions to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using (new TransactionScope(TransactionScopeOption.RequiresNew, to))
            {
                nvDb.SIU_Blogs.InsertOnSubmit(BlogEntry);
                nvDb.SubmitChanges();
                return BlogEntry.RefID;
            }
        }
        catch (Exception ex)
        {
            LogDebug("RecordBlog", ex.Message);
        }

        return -1;
    }
#endregion

#region PayStub
    public static List<DateTime> GetPayrollDates(string EmpID)
    {
        SIU_ORM_LINQDataContext nvDb = new SIU_ORM_LINQDataContext(SqlServerProdNvdbConnectString);

        return (from postDates in nvDb.Shermco_Payroll_Ledger_Entries
                where postDates.Employee_No_ == EmpID && postDates.Posting_Date > DateTime.Now.AddMonths(-12) && postDates.Display == 1
                group postDates by postDates.Posting_Date 
                into g

                from groupPostingDates in g
                    orderby groupPostingDates.Posting_Date descending
                    select groupPostingDates.Posting_Date
               ).Distinct().ToList();

    }
#endregion

#region    Date Time Helpers
    private const string DatetimeFormat = "MM/dd/yyyy";
    public static string FormatDateTime(object dtvalue)
    {
        string sDateTime = Convert.ToString(dtvalue);

        if (IsDateTime(sDateTime))
        {
            DateTime dt = DateTime.Parse(sDateTime);
            sDateTime = dt == new DateTime(1900, 1, 1) ? string.Empty : dt.ToString(DatetimeFormat);
        }

        return sDateTime;
    }
    public static bool IsDateTime(object value)
    {
        try
        {
            if (value == null)
                return false;

            DateTime dateTime = DateTime.Parse(value.ToString());
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
#endregion



}





#region Projection Classes
public class _PrjPts
{

    ////////////////////////////////////////////////////////
    // Array Of Summarized Points...                      //
    // Stored In _PrjPtsDeptSumDict with a Dept No as Key //
    ////////////////////////////////////////////////////////
    readonly double[] _prjPtsMonPts;
    readonly double[] _prjPtsSumPts;

    //////////////////////////////////////////////////////////////
    // Dictionary Instance Of A Summary Array For A Dept        //
    // One for each month and one for summary                   //
    // Stored in _PrjPtsDeptSumDict with Date (Mon / Yr) as Key //
    //////////////////////////////////////////////////////////////
    readonly Dictionary<DateTime, double[]> _prjPtsDeptSumDict;

    ///////////////////////////////////////////////////////////////////////////////////////
    // Dictionary That Holds A Summary Array For Each Month Requested Plus A Total Array //
    ///////////////////////////////////////////////////////////////////////////////////////
    readonly Dictionary<string, Dictionary<DateTime, double[]> > _prjPtsRptDict;

    /////////////////////////////////////////////////////////////////////////////
    // Dictionary Returned When Summary Over Entire Reporting Period Requested //
    /////////////////////////////////////////////////////////////////////////////
    readonly Dictionary<string, double[]> _sumDict;

    /////////////////////////////////////////////////////////////////
    // Dictionary Returned When List Of Reporting Months Requested //
    /////////////////////////////////////////////////////////////////
    readonly Dictionary<string, DateTime> _monList = new Dictionary<string,DateTime>();

    //int _NoPtTypes;

    ////////////////////////////////////////////////////////////////////////
    // Build Dictionaries With Summary Values                             //
    ////////////////////////////////////////////////////////////////////////
    public _PrjPts(DateTime start, DateTime end)
    {
        // Presume that and end date of 3/1/2013 is really 2/28/2013
        if (end.Day == 1)
            end = end.AddDays(-1);

        ////////////////////////
        // Lookup Point Types //
        ////////////////////////
        var ptTypes = SqlServer_Impl.GetAutoCompletePointTypes();
        int noPtTypes = ptTypes.Count() + 1;
        int noMonths = (((end.Year - start.Year) * 12) + end.Month - start.Month) + 1;

        //////////////////////////////////
        // Build Months List Dictionary //
        //////////////////////////////////
        DateTime monListItem = DateTime.Parse(start.Month + @"/1/" + start.Year );
        if (noMonths > 1)
        {
            for (int monCtr = 0; monCtr < noMonths; monCtr++)
            {
                _monList.Add(monListItem.Month + @"/1/" + monListItem.Year, monListItem);
                monListItem = monListItem.AddMonths(1);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Start New Month to Month and Summary Over Entire Reporting Period Dictionaries //
        ////////////////////////////////////////////////////////////////////////////////////
        _prjPtsRptDict = new Dictionary<string, Dictionary<DateTime, double[]>>();
        _sumDict = new Dictionary<string, double[]>();

        /////////////////////////////////
        // New Dept and New Date Flags //
        /////////////////////////////////
        string prevDept = string.Empty;
        string prevMon = string.Empty;
        string prevYr = string.Empty;

        foreach (var prjPts in SqlServer_Impl.GetProjectedPoints(start, end))
        {
            ///////////////////////////////////////////////
            // New Month -- Add Summary Points For Month //
            ///////////////////////////////////////////////
            if ( prjPts.mon + prjPts.yr.ToString() != prevMon + prevYr )
            {
                /////////////////////////////////
                // Add Sum Array To Dictionary //
                /////////////////////////////////
                if (_prjPtsSumPts != null)
                    _prjPtsDeptSumDict.Add(DateTime.Parse(prevMon + @"/1/" + prevYr), _prjPtsMonPts);

                ////////////////////////////////
                // Reset Monthly Points Array //
                ////////////////////////////////
                _prjPtsMonPts = new double[noPtTypes];
            }

            /////////////////////////////////////////////
            // End Of Dept -- Add Total Summary Points //
            /////////////////////////////////////////////
            if (prjPts.Dept != prevDept)
            {
                /////////////////////////////////
                // Add Sum Array To Dictionary //
                /////////////////////////////////
                if (_prjPtsSumPts != null)
                {
                    ///////////////////////////////////////////
                    // Add Summary Points Tp Dept Dictionary //
                    ///////////////////////////////////////////
                    _sumDict.Add(prevDept, _prjPtsSumPts);

                    //////////////////////////////////////////////////////
                    // Add Dept Dictionary To Monthly Report Dictionary //
                    //////////////////////////////////////////////////////
                    _prjPtsRptDict.Add(prevDept, _prjPtsDeptSumDict);
                }

                /////////////////////////////////
                // Reset Department Dictionary //
                /////////////////////////////////
                _prjPtsDeptSumDict = new Dictionary<DateTime, double[]>();

                ////////////////////////////
                // Reset Sum Points Array //
                ////////////////////////////
                _prjPtsSumPts = new double[noPtTypes];

                ////////////////////////////////
                // Reset Monthly Points Array //
                ////////////////////////////////
                _prjPtsMonPts = new double[noPtTypes];
            }


            /////////////////////////////////////////////////////
            // Add Projected (Required) Points To Dept Summary //
            /////////////////////////////////////////////////////
            if (prjPts.MonPrjPts > 0)
                _prjPtsMonPts[prjPts.PtsType] += prjPts.MonPrjPts;
            else
                _prjPtsMonPts[prjPts.PtsType] += prjPts.WklyPrjPts * DOW.Weeks((int)prjPts.yr, (int)prjPts.mon);
            _prjPtsSumPts[prjPts.PtsType] += _prjPtsMonPts[prjPts.PtsType];

            //////////////////////////////
            // Reset Report Break Flags //
            //////////////////////////////
            prevDept = prjPts.Dept;
            prevMon = prjPts.mon.ToString();
            prevYr = prjPts.yr.ToString();
        }




        if ( !_sumDict.ContainsKey(prevDept))
        {
            ///////////////////////////////////////////
            // Add Summary Points Tp Dept Dictionary //
            ///////////////////////////////////////////
            _sumDict.Add(prevDept, _prjPtsSumPts);

            //////////////////////////////////////////////////////
            // Add Dept Dictionary To Monthly Report Dictionary //
            //////////////////////////////////////////////////////
            _prjPtsDeptSumDict.Add(DateTime.Parse(prevMon + @"/1/" + prevYr), _prjPtsMonPts);
            _prjPtsRptDict.Add(prevDept, _prjPtsDeptSumDict);
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    // Dictionary Returned When Summary Over Entire Reporting Period Requested //
    /////////////////////////////////////////////////////////////////////////////    
    public Dictionary<string, double[]> GetPrjSumDict()
    {
        return _sumDict;
    }

    public double[] GetPrjSumForDept(string Dept)
    {
        double[] deptPrtSums;
        if ( _sumDict.ContainsKey(Dept) )
        {
            deptPrtSums = _sumDict[Dept];

            if ( deptPrtSums[0] == 0 )
                foreach (double DeptSum in deptPrtSums)
                    deptPrtSums[0] += DeptSum;
        }
        else
        {
            deptPrtSums = new double[] { 0 };
        }
        return deptPrtSums;
    }


    /////////////////////////////////////////////////////////////////////////////
    // Dictionary Returned When Summary Over Entire Reporting Period Requested //
    /////////////////////////////////////////////////////////////////////////////    
    public Dictionary<string, double[]> GetMonthlyDict(DateTime RptMonYr)
    {
        Dictionary<string, double[]> deptMonDic = new Dictionary<string, double[]>();
        RptMonYr = RptMonYr.AddDays(RptMonYr.Day - 1);

        foreach (var deptRcd in _prjPtsRptDict)
        {
            if (deptRcd.Value.ContainsKey(RptMonYr))
                deptMonDic.Add(deptRcd.Key, deptRcd.Value[RptMonYr]);
        }

        return deptMonDic;
    }


    /////////////////////////////////////////////////////////////////
    // Dictionary Returned When List Of Reporting Months Requested //
    /////////////////////////////////////////////////////////////////
    public Dictionary<string, DateTime> GetDatesDict()
    {
        return _monList;
    }

}
public class _StdPrjPts
{

    ////////////////////////////////////////////////////////
    // Array Of Summarized Points...                      //
    // Stored In _PrjPtsDeptSumDict with a Dept No as Key //
    ////////////////////////////////////////////////////////
    readonly double[] _prjPtsMonPts;

    ///////////////////////////////////////////////////////////////////////////////////////
    // Dictionary That Holds A Summary Array FOr Each Month Requested Plus A Total Array //
    ///////////////////////////////////////////////////////////////////////////////////////
    readonly Dictionary<string, double[]> _prjPtsRptDict = new Dictionary<string, double[]>();

    int _NoPtTypes;

    ////////////////////////////////////////////////////////////////////////
    // Build Dictionaries With Summary Values                             //
    ////////////////////////////////////////////////////////////////////////
    public _StdPrjPts()
    {
        ////////////////////////
        // Lookup Point Types //
        ////////////////////////
        var ptTypes = SqlServer_Impl.GetAutoCompletePointTypes();
        _NoPtTypes = ptTypes.Count() + 1;

        /////////////////////////////////
        // New Dept and New Date Flags //
        /////////////////////////////////
        string prevDept = string.Empty;

        foreach (var prjPts in SqlServer_Impl.GetStdProjectedPoints() )
        {
            /////////////////////////////////////////////
            // End Of Dept -- Add Total Summary Points //
            /////////////////////////////////////////////
            if (prjPts.Dept != prevDept)
            {
                /////////////////////////////////
                // Add Sum Array To Dictionary //
                /////////////////////////////////
                if (_prjPtsMonPts != null)
                {
                    //////////////////////////////////////////////////////
                    // Add Dept Dictionary To Monthly Report Dictionary //
                    //////////////////////////////////////////////////////
                    _prjPtsRptDict.Add(prevDept, _prjPtsMonPts);
                }

                ////////////////////////////////
                // Reset Monthly Points Array //
                ////////////////////////////////
                _prjPtsMonPts = new double[_NoPtTypes];

            }


            /////////////////////////////////////////////////////
            // Add Projected (Required) Points To Dept Summary //
            /////////////////////////////////////////////////////
            if (prjPts.MonPrjPts > 0)
                _prjPtsMonPts[prjPts.PtsType] += prjPts.MonPrjPts;
            else
                _prjPtsMonPts[prjPts.PtsType] += prjPts.WklyPrjPts * DOW.Weeks(DateTime.Now.Year, DateTime.Now.Month);

            //////////////////////////////
            // Reset Report Break Flags //
            //////////////////////////////
            prevDept = prjPts.Dept;
        }

        //////////////////////////////////////////////////////
        // Add Dept Dictionary To Monthly Report Dictionary //
        //////////////////////////////////////////////////////
        _prjPtsRptDict.Add(prevDept, _prjPtsMonPts);
    }

    public Dictionary<string, double[]> GetPrjSumDict()
    {
        return _prjPtsRptDict;
    }
}
#endregion Projection Classes

/////////////////////////////////////
// SQL Data Mapper -- Helper Class //
/////////////////////////////////////
public static class SqlDataMapper<T>
{
    public static T MakeNewDAO<T>() where T : new()
    {
        T dao = new T();

        DateTime minNavDate = DateTime.Parse("1/1/1753");

        ////////////////////////////////////////////
        //     Initialize Data Access Object      //
        // Build List Of Fields In Database Table //
        ////////////////////////////////////////////
        var siuTblFields =  ( from dataTblFlds in typeof(T).GetProperties()
                                        select dataTblFlds );

        foreach (var field in siuTblFields)
        {
            PropertyInfo pi = dao.GetType().GetProperty(field.Name);

            if ( pi.PropertyType == typeof(string) )
                pi.SetValue(dao, "", null);
                
            if (pi.PropertyType == typeof(bool) )
                pi.SetValue(dao, false, null);

            if (pi.PropertyType == typeof(DateTime) )
                pi.SetValue(dao, minNavDate, null);

            if (pi.PropertyType == typeof(int))
                pi.SetValue(dao, 0, null);
        }

        return dao;
    }
    public static T MapAspForm<T>(T MapToDAO, HttpRequest PageRequest)
    {
        //////////////////////////////////////////
        // Map Form Fields To Data Table Fields //
        //////////////////////////////////////////
        for (int x = 1; x < PageRequest.Form.Count; x++)
        {
//            System.Diagnostics.Debug.WriteLine(PageRequest.Form.GetKey(x).ToString());
            if (PageRequest.Form.GetKey(x).Contains("$"))
            {
                string fieldName =
                    PageRequest.Form.GetKey(x).Substring(PageRequest.Form.GetKey(x).LastIndexOf('$') + 1);

                PropertyInfo pi = MapToDAO.GetType().GetProperty(fieldName);
                if (pi != null)
                {
//                  System.Diagnostics.Debug.WriteLine("SIU FIELD: " + PI.Name + "  Form Field: " + fieldName);

                    /////////////////////////
                    // Process Text Fields //
                    /////////////////////////
                    if (pi.PropertyType == typeof(string))
                        pi.SetValue(MapToDAO, String.Join(" ", String.Join(" ", PageRequest.Form.GetValues(x))), null);

                    ////////////////////////
                    // Process Bit Fields //
                    ////////////////////////
                    if (pi.PropertyType == typeof(bool) || pi.PropertyType == typeof(bool?))

                        ////////////////////////////////////////////////////////////////////////////
                        // This Is A Trick That Works Only Because Uncheck Boxes Are Not Returned //
                        ////////////////////////////////////////////////////////////////////////////
                        pi.SetValue(MapToDAO, true, null);


                    /////////////////////////
                    // Process Date Fields //
                    /////////////////////////
                    if ( pi.PropertyType == typeof(DateTime) || pi.PropertyType == typeof( DateTime?) )
                    {
                        try
                        {
                            pi.SetValue(MapToDAO, DateTime.Parse(PageRequest.Form.GetValues(x)[0]), null);
                        }
                        catch {}
                        
                    }
                }
            }
        }

        return MapToDAO;
    }
}


///////////////////////////////////////////////////
// Debugger Attaches To Connection ANd Shows SQL //
///////////////////////////////////////////////////
public class DebugTextWriter : TextWriter
{
    public override void Write(char[] buffer, int index, int count)
    {
        Debug.Write(new String(buffer, index, count));
    }
    public override void Write(string value)
    {
        Debug.Write(value);
    }
    public override Encoding Encoding
    {
        get { return Encoding.Default; }
    }
}

//////////////////////////////////////////////////////////////////////////////////////
// Stolen From:                                                                     //
// http://aonnull.blogspot.com/2010/08/dynamic-sql-like-linq-orderby-extension.html //
//////////////////////////////////////////////////////////////////////////////////////
 public static class OrderByHelper
{
    public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> enumerable, string orderBy)
    {
        return enumerable.AsQueryable().OrderBy(orderBy).AsEnumerable();
    }

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> collection, string orderBy)
    {
        foreach(OrderByInfo orderByInfo in ParseOrderBy(orderBy))
            collection = ApplyOrderBy<T>(collection, orderByInfo);

        return collection;
    }

    private static IQueryable<T> ApplyOrderBy<T>(IQueryable<T> collection, OrderByInfo orderByInfo)
    {
        string[] props = orderByInfo.PropertyName.Split('.');
        Type type = typeof(T);

        ParameterExpression arg = Expression.Parameter(type, "x");
        Expression expr = arg;
        foreach (string prop in props)
        {
            // use reflection (not ComponentModel) to mirror LINQ
            PropertyInfo pi = type.GetProperty(prop);
            expr = Expression.Property(expr, pi);
            type = pi.PropertyType;
        }
        Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
        LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
        string methodName;

        if (!orderByInfo.Initial && collection is IOrderedQueryable<T>)
        {
            methodName = orderByInfo.Direction == SortDirection.Ascending ? "ThenBy" : "ThenByDescending";
        }
        else
        {
            if (orderByInfo.Direction == SortDirection.Ascending)
                    methodName = "OrderBy";
            else
                    methodName = "OrderByDescending";
        }

        return (IOrderedQueryable<T>)typeof(Queryable).GetMethods().Single(
            method => method.Name == methodName
                    && method.IsGenericMethodDefinition
                    && method.GetGenericArguments().Length == 2
                    && method.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), type)
            .Invoke(null, new object[] { collection, lambda });

    }

    private static IEnumerable<OrderByInfo> ParseOrderBy(string orderBy)
    {
        if (String.IsNullOrEmpty(orderBy))
            yield break;

        string[] items = orderBy.Split(',');
        bool initial = true;
        foreach(string item in items)
        {
            string[] pair = item.Trim().Split(' ');

            if (pair.Length > 2)
                throw new ArgumentException(String.Format("Invalid OrderBy string '{0}'. Order By Format: Property, Property2 ASC, Property2 DESC",item));

            string prop = pair[0].Trim();

            if(String.IsNullOrEmpty(prop))
                throw new ArgumentException("Invalid Property. Order By Format: Property, Property2 ASC, Property2 DESC");
                
            SortDirection dir = SortDirection.Ascending;
                
            if (pair.Length == 2)
                dir = ("desc".Equals(pair[1].Trim(), StringComparison.OrdinalIgnoreCase) ? SortDirection.Descending : SortDirection.Ascending);

            yield return new OrderByInfo { PropertyName = prop, Direction = dir, Initial = initial };

            initial = false;
        }

    }

    private class OrderByInfo
    {
        public string PropertyName { get; set; }
        public SortDirection Direction { get; set; }
        public bool Initial { get; set; }
    }

    private enum SortDirection
    {
        Ascending = 0,
        Descending = 1
    }
}
