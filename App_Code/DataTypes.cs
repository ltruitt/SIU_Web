using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AutoMapper;

using System.Globalization;

public partial class SIU_Training_Quiz
{
    public override string ToString()
    {
        return (    "TL_UID: " + User_Zipcode +
                    " " + Quiz_Name +
                    " (" + User_ID + ") " + User_Name +
                    "pct: " + User_Pct_Marks +
                    "+ " + Total_Correct +
                    "- " + Total_Wrong
               );
    }
}

namespace ShermcoYou.DataTypes
{
    public enum TimeSheetEntry_Status
    {
        New = 0,
        PendingEmployeeApproval = 1,
        EmployeeApproved = 2,
        PendingVerification = 3,
        BeingReviewedByVerifier = 4,
        PendingManagerApproval = 5,
        ManagerApproved = 6,
        Rejected = 7,
        Posted = 8
    }
    public enum Job_Status
    {
        Planning = 0,
        Quote = 1,
        Order = 2,
        Completed = 3
    }
    public enum Employee_Status
    {
        Active = 0,
        Inactive = 1,
        Terminated = 2
    }
    public enum LogonValid
    {
        Fail = 0,
        Success = 1,
        LockedOut = -99
    }

    public class EmpByMon
    {
        public int[] Months = new int[14];
    }
    public class TypSumByMon
    {
        public string Month;
        public int Type;
        public int Points;
        public int Submitted;
        public int Rejected;
        public int Open;
        public int Working;
        public int Killed;
        public int Closed;
    }
    public class EmpDtl
    {
        public int[] ReasonPts;
    }
    public class sumRcd
    {
        public string Dept;
        public string DeptName;
        public decimal TotHours = 2000;
        public int TotFirAidClasses;
        public int TotMedRecordable;
        public int TotRestrictDays;
        public int TotLostDays;
        public int TotVehIncidents;

        public float TotInjInHouseCost;
        public float TotInjIncurredCost;
        public float TotInjReservedCost;

        public float TotVehInHouseCost;
        public float TotVehIncurredCost;
        public float TotVehReservedCost;

        public string OSHA_NAICS;
        public float OSHA_TRIR_NAT_AVG;
        public float OSHA_RIR_NAT_AVG;
        public float OSHA_LTIR_NAT_AVG;
        public float OSHA_DART_NAT_AVG;
    }
    public class SIU_Hours
    {
        public string Dept;
        public DateTime? WorkDate;
        public int WOY;
        public string EID;
        public string Pay_Posting_Group;
        public string Job;
        public string Task;
        public decimal? ST;
        public decimal? OT;
        public decimal? DT;
        public decimal? AB;
        public decimal? HT;
        public decimal? DTL_SUM;
        public decimal? TYPE_SUM;
        public decimal? DEPT_SUM;
    }
    public class SIU_LogonProbeRcd
    {
        public int? Valid;
        public int? Mail;
    }
    public class SIU_SortedEmployees
    {
        public string sEmpNo { get; set; }
        public int EmpNo { get; set; }
        public string EmpLastName { get; set; }
        public string EmpFirstName { get; set; }
        public string EmpDisplayNoName { get; set; }
    }
    public class SIU_DOT_Rpt
    {
        private string _vehicle;
        private string _empId;
        private string _make;
        private string _model;
        private string _plate;
        private string _empName;

        public int RefID { get; set;  }
        public string Vehicle
        {
            get { return _vehicle; }
            set 
            {
                if (value == null)
                    _vehicle = "";
                else
                {
                    _vehicle = value.Trim();
                    Shermco_Vehicle veh = SqlServer_Impl.GetVehicleRecord(_vehicle);
                    if (veh != null)
                    {
                        _make = veh.Make;
                        _model = veh.Model;
                        _plate = veh.License;
                    }
                }

            }
        }
        public string Make { get { return _make;  } }
        public string Model { get { return _model; } }
        public string Plate { get { return _plate; } }
        public string SubmitTimeStamp { get; set; }
        public string SubmitEmpID
        {
            get { return _empId; } 
            set
            {
                if (value == null)
                    _vehicle = "";
                else
                {
                    _empId = value.Trim();
                    _empName = SqlServer_Impl.GetEmployeeNameByNo(_empId);
                }
            }
        }

        public string SubmitEmpName { get { return _empName; } }
        public string Hazard { get; set; }
        public string Correction { get; set; }
    }
    public class SIU_BasicEmployee
    {
        public string Name;
        public string EID;
        public string Super;
        public string Dept;

        public SIU_BasicEmployee()
        {
            Name = BusinessLayer.UserFullName;
            EID = BusinessLayer.UserEmpID;
            Super = BusinessLayer.UserSuprEmpId + "  " + BusinessLayer.UserSuprFullName;
            Dept = BusinessLayer.UserDept;
        }

        public SIU_BasicEmployee(Shermco_Employee e)
        {
            Name = e.Last_Name + ", " + e.First_Name;
            EID = e.No_;
            Dept = e.Global_Dimension_1_Code;

            Super = SqlServer_Impl.GetEmployeeNameByNo(e.Manager_No_);
        }
    }
    public class SIU_Incident_Accident_Reports_To
    {
        public string EmpId;
        public string Dept;
        public string SuprEmpId;
        public string DeptMgrEmpId;
        public string DivMgrEmpId;
        public string VpEmpId;
        public string GmEmpId;
        public string PresEmpId;

        public string SafetyMgrEmpId;
        public string RiskMgrEmpId;
        public string LegalMgrEmpId;

        public string EmpName;
        public string SuprName;
        public string DeptMgrName;
        public string DivMgrName;
        public string VpName;
        public string GmName;
        public string PresName;

        public string SafetyMgrName;
        public string RiskMgrName;
        public string LegalMgrName;

        public DateTime SuprDate;
        public DateTime DeptMgrDate;
        public DateTime DivMgrDate;
        public DateTime VpDate;
        public DateTime GmDate;
        public DateTime PresDate;

        public DateTime SafetyMgrDate;
        public DateTime RiskMgrDate;
        public DateTime LegalMgrDate;

        public bool readyToClose
        {
            get
            {
                if (SuprEmpId != null && SuprDate == DateTime.MinValue) return false;
                if (DeptMgrEmpId != null && DeptMgrDate == DateTime.MinValue) return false;
                if (DivMgrEmpId != null && DivMgrDate == DateTime.MinValue) return false;
                if (VpEmpId != null && VpDate == DateTime.MinValue) return false;
                if (GmEmpId != null && GmDate == DateTime.MinValue) return false;
                if (PresEmpId != null && PresDate == DateTime.MinValue) return false;
                if (SafetyMgrEmpId != null && SafetyMgrDate == DateTime.MinValue) return false;
                if (RiskMgrEmpId != null && RiskMgrDate == DateTime.MinValue) return false;
                if (LegalMgrEmpId != null && LegalMgrDate == DateTime.MinValue) return false;
                return true;
            }
        }
    }
    public class SIU_SafetyPays_TaskList_Rpt
    {
        public int IncidentNo;
        public int TaskNo;
        public string TaskDefinition;
        public DateTime DueDate;
        public string AssignedEmpID;
        public DateTime CompletedDate;
        public string AssignedEmpName;

        public SIU_SafetyPays_TaskList_Rpt()
        {
        }

        public SIU_SafetyPays_TaskList_Rpt(SIU_SafetyPays_TaskList Rpt)
        {
            IncidentNo = Rpt.IncidentNo;
            TaskNo = Rpt.TaskNo;
            TaskDefinition = Rpt.TaskDefinition;
            DueDate = Rpt.DueDate;
            AssignedEmpID = Rpt.AssignedEmpID;
            if (Rpt.CompletedDate != null)
                CompletedDate = (DateTime)Rpt.CompletedDate;
            else
                Rpt.CompletedDate = null;
            AssignedEmpName = SqlServer_Impl.GetEmployeeNameByNo(Rpt.AssignedEmpID);
        }
    }
    public class SIU_Qom_QR
    {
        public int Q_Id;
        public DateTime StartDate;
        public DateTime EndDate;

        public string Question = "";
        public string Response = "";
        public string EhsResponse = "";
        public int SPP_UID;
        public string Q_Grp = "";
        public string Status = "No Response";
        public string Points = "";
        public string Q_Ans;

        private static readonly string RootFilesPath = HttpContext.Current.Server.MapPath("/Files");

        public SIU_Qom_QR()
        {
        }

        public SIU_Qom_QR(SIU_Safety_MoQ Q, SIU_SafetyPaysReport R, SIU_SafetyPays_Point P)
        {
            Q_Id = Q.Q_Id;
            StartDate = Q.StartDate;
            EndDate = Q.EndDate;
            Q_Grp = Q.QuestionGroup;

            Q_Ans = "";
            if ( Q.EndDate < DateTime.Now )
                Q_Ans = Q.QuestionAns;
            
            if ( Q.Question != null )
                Question = Q.Question + Environment.NewLine;
            if (  Q.QuestionFile != null )
                if ( Q.QuestionFile.Length > 0)
                {
                    string filePath = RootFilesPath + @"\UPLOADS\" + Q.QuestionFile;

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.StreamReader qomFile = new System.IO.StreamReader(filePath);
                        Question += qomFile.ReadToEnd();
                        qomFile.Close();
                    }
                }

            if (R != null)
            {
                Status = R.IncStatus;
                Response = R.Comments;
                EhsResponse = R.ehsRepsonse;

                if ( P != null) 
                    if ( P.Points > 0 )
                        Points = P.Points.ToString(CultureInfo.InvariantCulture);
            }

            if (Status == "New") Status = "Submitted";
            if (Status == "Closed") Status = "Accepted";
            if (Status == "Reject") Status = "Decline";
        }

    }
    public class SIU_SafetyPaysReport_Rpt
    {
        private int _incidentNo;
        public string IncStatus;
        public string JobSite;
        public string Comments;


        private string _incTypeTxt;
        public bool IncTypeSafeFlag;
        public bool IncTypeUnsafeFlag;
        public bool IncTypeSuggFlag;
        public bool IncTypeTopicFlag;
        public bool IncTypeSumFlag;

        public string SafetyMeetingType;

        public string EmpID;                public string ReportedByEmpName;
        public string IncLastTouchEmpID;    public string LastModifedByEmpName;
        public string ObservedEmpID;        public string ObservedEmpName;

        private DateTime _incOpenTimestamp;
        public DateTime IncCloseTimestamp;
        public DateTime IncLastTouchTimestamp;
        public DateTime PointsAssignedTimeStamp;
        private DateTime _incidentDate;
        public DateTime SafetyMeetingDate;

        public int PointsAssigned;
        public string EhsRepsonse;
        public string InitialResponse;

        public int defaultPoints = 0;
        public int TotalTasks = 0;
        public int OpenTasks = 0;
        public int LateTasks = 0;
        public int LateStatus = 0;

        public int QOM_ID;

        public SIU_SafetyPaysReport_Rpt(SIU_SafetyPaysReport Rpt)
        {
            _incidentNo = Rpt.IncidentNo;
            IncStatus = Rpt.IncStatus;
            JobSite  = Rpt.JobSite;
            Comments  = Rpt.Comments;

            _incTypeTxt  = Rpt.IncTypeTxt;
            IncTypeSafeFlag  = Rpt.IncTypeSafeFlag;
            IncTypeUnsafeFlag  = Rpt.IncTypeUnsafeFlag;
            IncTypeSuggFlag  = Rpt.IncTypeSuggFlag;
            IncTypeTopicFlag  = Rpt.IncTypeTopicFlag;
            IncTypeSumFlag  = Rpt.IncTypeSumFlag;

            SafetyMeetingType  = Rpt.SafetyMeetingType;

            EmpID  = Rpt.EmpID;                
            IncLastTouchEmpID  = Rpt.IncLastTouchEmpID;    
            ObservedEmpID  = Rpt.ObservedEmpID;

            if (Rpt.IncOpenTimestamp != null)
                _incOpenTimestamp  = (System.DateTime)Rpt.IncOpenTimestamp;

            if ( Rpt.IncCloseTimestamp != null)
                  IncCloseTimestamp = (System.DateTime)Rpt.IncCloseTimestamp;

            if (Rpt.IncLastTouchTimestamp != null)
                IncLastTouchTimestamp = (System.DateTime)Rpt.IncLastTouchTimestamp;

            if (Rpt.PointsAssignedTimeStamp != null)
                PointsAssignedTimeStamp = (System.DateTime)Rpt.PointsAssignedTimeStamp;

            if (Rpt.IncidentDate != null)
                _incidentDate = (System.DateTime)Rpt.IncidentDate;

            if (Rpt.SafetyMeetingDate != null)
                SafetyMeetingDate = (System.DateTime)Rpt.SafetyMeetingDate;

            PointsAssigned  = (int)Rpt.PointsAssigned;
            EhsRepsonse = Rpt.ehsRepsonse;
            InitialResponse = Rpt.InitialResponse;

            ReportedByEmpName = SqlServer_Impl.GetEmployeeNameByNo(EmpID);
            LastModifedByEmpName = SqlServer_Impl.GetEmployeeNameByNo(IncLastTouchEmpID);
            ObservedEmpName = SqlServer_Impl.GetEmployeeNameByNo(ObservedEmpID);

            defaultPoints = SqlServer_Impl.GetAutoCompletePointTypes().Where(c => c.Description.ToLower() == _incTypeTxt.ToLower()).Select(c => c.PointsCount).SingleOrDefault();

            TotalTasks = 0;
            OpenTasks = 0;
            LateTasks = 0;
            LateStatus = 0;

            if (IncStatus.ToLower() == "working")
            {
                var tasks = SqlServer_Impl.GetSafetyPaysTasks(_incidentNo);
                TotalTasks = tasks.Count();
                OpenTasks = tasks.Where(c => c.CompletedDate == null).Count();
                LateTasks = tasks.Where(c => c.CompletedDate == null && c.DueDate < DateTime.Now).Count();
                LateStatus = SqlServer_Impl.GetSafetyPaysTaskStatus(_incidentNo).Where(c => c.ResponseDate == null).Count();
            }

            if ( Rpt.QOM_ID != null )
                QOM_ID = (int)Rpt.QOM_ID;
        }

        public SIU_SafetyPaysReport_Rpt(SIU_SafetyPaysReport Rpt, string ReportingEmployee, string ObservedEmployee, string LastTouchedEmployee, string Points )
        {
            _incidentNo = Rpt.IncidentNo;
            IncStatus = Rpt.IncStatus;
            JobSite = Rpt.JobSite;
            Comments = Rpt.Comments;

            _incTypeTxt = Rpt.IncTypeTxt;
            IncTypeSafeFlag = Rpt.IncTypeSafeFlag;
            IncTypeUnsafeFlag = Rpt.IncTypeUnsafeFlag;
            IncTypeSuggFlag = Rpt.IncTypeSuggFlag;
            IncTypeTopicFlag = Rpt.IncTypeTopicFlag;
            IncTypeSumFlag = Rpt.IncTypeSumFlag;

            SafetyMeetingType = Rpt.SafetyMeetingType;

            EmpID = Rpt.EmpID;
            IncLastTouchEmpID = Rpt.IncLastTouchEmpID;
            ObservedEmpID = Rpt.ObservedEmpID;

            if (Rpt.IncOpenTimestamp != null)
                _incOpenTimestamp = (System.DateTime)Rpt.IncOpenTimestamp;

            if (Rpt.IncCloseTimestamp != null)
                IncCloseTimestamp = (System.DateTime)Rpt.IncCloseTimestamp;

            if (Rpt.IncLastTouchTimestamp != null)
                IncLastTouchTimestamp = (System.DateTime)Rpt.IncLastTouchTimestamp;

            if (Rpt.PointsAssignedTimeStamp != null)
                PointsAssignedTimeStamp = (System.DateTime)Rpt.PointsAssignedTimeStamp;

            if (Rpt.IncidentDate != null)
                _incidentDate = (System.DateTime)Rpt.IncidentDate;

            if (Rpt.SafetyMeetingDate != null)
                SafetyMeetingDate = (System.DateTime)Rpt.SafetyMeetingDate;

            PointsAssigned = (int)Rpt.PointsAssigned;
            EhsRepsonse = Rpt.ehsRepsonse;
            InitialResponse = Rpt.InitialResponse;

            //ReportedByEmpName = SqlServer_Impl.GetEmployeeNameByNo(EmpID);
            //LastModifedByEmpName = SqlServer_Impl.GetEmployeeNameByNo(IncLastTouchEmpID);
            //ObservedEmpName = SqlServer_Impl.GetEmployeeNameByNo(ObservedEmpID);

            ReportedByEmpName = ReportingEmployee;
            LastModifedByEmpName = LastTouchedEmployee;
            ObservedEmpName = ObservedEmployee;


            //defaultPoints = SqlServer_Impl.GetAutoCompletePointTypes().Where(c => c.Description.ToLower() == _incTypeTxt.ToLower()).Select(c => c.PointsCount).SingleOrDefault();
            if ( Points != null)
            defaultPoints =  int.Parse(Points);

            TotalTasks = 0;
            OpenTasks = 0;
            LateTasks = 0;
            LateStatus = 0;

            if (IncStatus.ToLower() == "working")
            {
                var tasks = SqlServer_Impl.GetSafetyPaysTasks(_incidentNo);
                TotalTasks = tasks.Count();
                OpenTasks = tasks.Where(c => c.CompletedDate == null).Count();
                LateTasks = tasks.Where(c => c.CompletedDate == null && c.DueDate < DateTime.Now).Count();
                LateStatus = SqlServer_Impl.GetSafetyPaysTaskStatus(_incidentNo).Where(c => c.ResponseDate == null).Count();
            }

            if (Rpt.QOM_ID != null)
                QOM_ID = (int)Rpt.QOM_ID;
        }

        public int IncidentNo
        {
            get { return _incidentNo; }
            set { _incidentNo = value; }
        }

        public DateTime IncidentDate
        {
            get { return _incidentDate; }
            set { _incidentDate = value; }
        }

        public DateTime IncOpenTimestamp
        {
            get { return _incOpenTimestamp; }
            set { _incOpenTimestamp = value; }
        }

        public string IncTypeTxt
        {
            get { return _incTypeTxt; }
            set { _incTypeTxt = value; }
        }
    }
    public class SIU_Meeting_Log_Ext
    {
        public int TL_UID;
        public DateTime? Date;
        public string Topic;
        public string Description;
        public string Instructor;
        public int MeetingType;
        public string Location;
        public int? Points;
        public string VideoFile;
        public string QuizName;
        public DateTime? StartTime;
        public DateTime? StopTime;
        public int? InstructorID;
        public bool? VideoComplete;
        public int QuizComplete;
        public bool? QuizPass;
        public int? PreReq;
    }
    public class SIU_ELO_Meal_Opts
    {
        public class SIU_ELO_Meal_Opt
        {
            public string DisplayString;
            public decimal Amount;
        }

        public List<SIU_ELO_Meal_Opt> GetList()
        {
            List<SIU_ELO_Meal_Opt> MealOpts = new List<SIU_ELO_Meal_Opt>();
            MealOpts.Add(new SIU_ELO_Meal_Opt() { Amount = 10, DisplayString = "4-10" });
            MealOpts.Add(new SIU_ELO_Meal_Opt() { Amount = 20, DisplayString = "10.25-12" });
            MealOpts.Add(new SIU_ELO_Meal_Opt() { Amount = 35, DisplayString = "12.24-24" });

            return MealOpts;
        }
    }
    public class SIU_YTD_Exp_Rpt
    {
        public DateTime WorkMonth { get; set; }
        public DateTime WorkDate { get; set; }
        public string JobNo { get; set; }
        public string OH_AccountNo { get; set; }
        public int Miles { get; set; }
        public int Meals { get; set; }
        public decimal Amount { get; set; }

        public SIU_YTD_Exp_Rpt()
        {
        }

        public SIU_YTD_Exp_Rpt(Shermco_Employee_Expense Rpt)
        {
            WorkMonth = WorkDate = Rpt.Work_Date;
            WorkMonth = WorkMonth.AddDays(-WorkMonth.Day + 1);
            JobNo = Rpt.Job_No_;
            OH_AccountNo = Rpt.O_H_Account_No_;
            Miles = Rpt.Mileage;
            Meals = Rpt.Meals;
            Amount = Rpt.Amount;
        }

    }
    public class JqGridData
    {
        public int total { get; set; }
        public int page { get; set; }
        public int records { get; set; }
        public List<SIU_Points_Rpt> rows { get; set; }
    }
    public class SIU_Points_Rpt
    {
        public string Emp_No            { get; set; }
        public string EmpName           { get; set; }
        public string EmpDept           { get; set; }

        public DateTime EventDate       { get; set; }
        public DateTime DatePointsGiven { get; set; }
        
        public int ReasonForPoints      { get; set; }
        public string PointsType        { get; set; }

        public string PointsGivenBy     { get; set; }
        public string PointsGivenByName { get; set; }

        public string Comments          { get; set; }
        public int Points               { get; set; }
        public int UID                  { get; set; }

        public SIU_Points_Rpt() { }

        public SIU_Points_Rpt(SIU_SafetyPays_Point Rpt, string EmpNo,  string Dept, string Name, string PtType)
        {
            //Emp_No = Rpt.Emp_No;
            Emp_No = EmpNo;
            EmpName = Name;
            EmpDept = Dept;

            if (Rpt != null)
            {
                EventDate = Rpt.EventDate;
                DatePointsGiven = Rpt.DatePointsGiven;

                ReasonForPoints = Rpt.ReasonForPoints;
                PointsType = PtType;

                PointsGivenBy = Rpt.PointsGivenBy;

                Comments = Rpt.Comments;
                Points = Rpt.Points;
                UID = Rpt.UID;
            }
            else
            {
                Points = 0;
                EventDate = DateTime.MinValue;
            }
        }

    }
    public class SIU_TimeSheet_Job
    {
        public string JobNo;
        public string JobDesc;
        public string JobDept;
        public string JobCust;
        public string JobContact;
        public string JobEmail;
        public string JobStatus;
        public string LeadTech;
        public DateTime JobCostDate;

        public SIU_TimeSheet_Job(Shermco_Job init)
        {
            if (init == null)
                return;

            JobNo = init.No_;
            JobDesc = init.Description;
            JobDept = init.Global_Dimension_1_Code;
            JobCust = SqlServer_Impl.GetCustomerName(init.Sell_To_Customer_No_);
            LeadTech = init.Lead_Tech;
            JobCostDate = init.Date_Job_Went_to_Cost;
            switch (init.Status)
            {
                case (int)Job_Status.Planning: JobStatus = "Planning"; break;
                case (int)Job_Status.Quote: JobStatus = "Quote"; break;
                case (int)Job_Status.Order: JobStatus = "Order"; break;
                case (int)Job_Status.Completed: JobStatus = "Complete"; break;
                default: JobStatus = init.Status.ToString(CultureInfo.InvariantCulture); break;
            }
            
        }
    }
    public class SIU_TimeSheet_Oh
    {
        public string AcctNo;
        public string EmpDept = "0000";

        public SIU_TimeSheet_Oh(string OhAcct, string EmpID)
        {
            AcctNo = OhAcct;
            Shermco_Employee Emp = null;

            ////////////////////////////
            // Lookup Employee Record //
            ////////////////////////////
            if (HttpContext.Current.Session != null)
                Emp = SqlServer_Impl.GetEmployeeByNo(EmpID);

            //////////////////////////////////
            // Get The Employees Department //
            //////////////////////////////////
            if (Emp != null)
                EmpDept = Emp.Global_Dimension_1_Code;
        }
    }
    public class SIU_TimeSheet_DailyDetailSums
    {
        public decimal ST { get; set; }
        public decimal OT { get; set; }
        public decimal DT { get; set; }
        public decimal HT { get; set; }
        public decimal AB { get; set; }
    }
    public class SIU_TimeSheet_DailyHourSum
    {
        public string workDate { get; set; }
        public Decimal Hours { get; set; }
        public int DOW { get; set; }
    }
    public class SIU_TimeSheet_HoursRpt
    {
        public int EntryNo { get; set; }
        public string workDate { get; set; }
        public decimal ST { get; set; }
        public decimal OT { get; set; }
        public decimal DT { get; set; }
        public decimal AB { get; set; }
        public decimal HT { get; set; }
        public decimal Total { get; set; }

        public string Dept { get; set; }
        public string JobNo { get; set; }
        public string Task { get; set; }
        public string OhAcct { get; set; }
        public int Status { get; set; }
    }
    public class SIU_TimeSheet_HoursRpt_Mo
    {

        public decimal[] HtHours;
        public decimal[] AbHours;
        public decimal[] DtHours;
        public decimal[] OtHours;
        public decimal[] StHours;
        public decimal[] SumHours;
        public decimal[] SumJobHours;
        public string[] xlabel;
        

        public SIU_TimeSheet_HoursRpt_Mo()
        {

            int NumDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            Initializer(NumDays);
        }

        public SIU_TimeSheet_HoursRpt_Mo(DateTime MonToRptOn)
        {
            int NumDays = DateTime.DaysInMonth(MonToRptOn.Year, MonToRptOn.Month);
            Initializer(NumDays);
        }


        public SIU_TimeSheet_HoursRpt_Mo(int Length)
        {
            Initializer(Length);
        }

        private void Initializer(int RptPeriods)
        {
            RptPeriods++;  // Because We Ignore the 0 Index

            HtHours = new decimal[RptPeriods];
            AbHours = new decimal[RptPeriods];
            DtHours = new decimal[RptPeriods];
            OtHours = new decimal[RptPeriods];
            StHours = new decimal[RptPeriods];
            SumHours = new decimal[RptPeriods];
            SumJobHours = new decimal[RptPeriods];
            xlabel = new string[RptPeriods];

            for (int day = 0; day < RptPeriods; day++)
            {
                HtHours[day] = 0;
                AbHours[day] = 0;
                DtHours[day] = 0;
                OtHours[day] = 0;
                StHours[day] = 0;
                SumHours[day] = 0;
                SumJobHours[day] = 0;
                xlabel[day] = "";
            }
        }


    }
    public class SIU_TimeSheet_HoursRpt_JobOh
    {

        public decimal[] HtHours;
        public decimal[] AbHours;
        public decimal[] DtHours;
        public decimal[] OtHours;
        public decimal[] StHours;
        public decimal[] SumHours;
        public string[] JobNo;
        public string[] AcctNo;

        public SIU_TimeSheet_HoursRpt_JobOh(int Length)
        {
            HtHours = new decimal[Length];
            AbHours = new decimal[Length];
            DtHours = new decimal[Length];
            OtHours = new decimal[Length];
            StHours = new decimal[Length];
            SumHours = new decimal[Length];
            JobNo = new string[Length];
            AcctNo = new string[Length];

            for (int day = 0; day < Length; day++)
            {
                HtHours[day] = 0;
                AbHours[day] = 0;
                DtHours[day] = 0;
                OtHours[day] = 0;
                StHours[day] = 0;
                SumHours[day] = 0;
            }
        }
    }
    public class InProgressJobReport
    {
        public string JobNo;
        public string JobNo2;
        public string JobDesc;
        public string JobDept;
        public string JobCust;
        public string CostingDate;
        public string TurnInTech;
        public string ReqSalesFollowUp;
        public string SalesFollupDate;
        public string SubmitDate;
        public string JhaDate;
        public string IrSubmitDate;
        public string IrCompleteDate;
        public string LoggedInDate;
        public string DataEntryDate;
        public string ProofDate;
        public string CorrectDate;
        public string ReviewDate;
        public string ReadyDate;

        public InProgressJobReport(Shermco_Job_Report init)
        {
            JobNo = JobNo2 = init.Job_No_;

            Shermco_Job job = SqlServer_Impl.GetJobByNo(JobNo);
            JobDesc = job.Description;
            JobDept = job.Global_Dimension_1_Code;
            JobCust = SqlServer_Impl.GetCustomerName(job.Bill_to_Customer_No_);
            CostingDate = job.Date_Job_Went_to_Cost.ToShortDateString();

            TurnInTech = init.Turned_in_By_Emp__No_ + " ";
            ReqSalesFollowUp = (init.SalesFollowUp == '1') ? "YES" : "No";

            SalesFollupDate =  ( init.SalesFollowUp_Ack_Date == DateTime.Parse("1753-01-01") )  ? "" : init.SalesFollowUp_Ack_Date.ToShortDateString();
            SubmitDate = (init.Turned_in_by_Tech_Date == DateTime.Parse("1753-01-01"))          ? "" : init.Turned_in_by_Tech_Date.ToShortDateString();
            JhaDate = (init.JHA_Submitted_Date == DateTime.Parse("1753-01-01"))                 ? "" : init.JHA_Submitted_Date.ToShortDateString();

            IrSubmitDate = (init.IR_Received_From_Tech == DateTime.Parse("1753-01-01"))         ? "" : init.IR_Received_From_Tech.ToShortDateString();
            IrCompleteDate = (init.IR_Complete_and_Delivered == DateTime.Parse("1753-01-01"))   ? "" : init.IR_Complete_and_Delivered.ToShortDateString();

            CostingDate = (job.Date_Job_Went_to_Cost == DateTime.Parse("1753-01-01"))           ? "" : job.Date_Job_Went_to_Cost.ToShortDateString();
            LoggedInDate = (init.Logged_and_Scanned == DateTime.Parse("1753-01-01"))            ? "" : init.Logged_and_Scanned.ToShortDateString();
            DataEntryDate = (init.Start_Data_Sheet_Entry == DateTime.Parse("1753-01-01"))       ? "" : init.Start_Data_Sheet_Entry.ToShortDateString();
            ProofDate = (init.Start_Proofread == DateTime.Parse("1753-01-01"))                  ? "" : init.Start_Proofread.ToShortDateString();
            CorrectDate = (init.Received_for_Corrections == DateTime.Parse("1753-01-01"))       ? "" : init.Received_for_Corrections.ToShortDateString();
            ReviewDate = (init.Sent_for_Tech_Review == DateTime.Parse("1753-01-01"))            ? "" : init.Sent_for_Tech_Review.ToShortDateString();
            ReadyDate = (init.Tech_Review_Completed == DateTime.Parse("1753-01-01"))            ? "" : init.Tech_Review_Completed.ToShortDateString();
        }
    };
    public class PastDueJobReport
    {
        public string JobNo;
        public string JobDesc;
        public string JobDept;
        public string JobCust;
        public string JobStatus;

        public string LeadTechNo;
        public string LeadTechName;
        public string CostingDate;
        public string LastLaborDate;
        public int DaysLate;

        public PastDueJobReport(Shermco_Job init)
        {
            DateTime LLD = DateTime.MinValue;

            JobNo = init.No_;
            JobDesc = init.Description;
            JobDept = init.Global_Dimension_1_Code;
            JobCust = SqlServer_Impl.GetCustomerName(init.Bill_to_Customer_No_);
            JobStatus = init.Job_status;

            LeadTechNo = init.Lead_Tech;
            LeadTechName = SqlServer_Impl.GetEmployeeNameByNo(init.Lead_Tech);
            CostingDate =   init.Date_Job_Went_to_Cost.ToShortDateString();

            LLD = SqlServer_Impl.GetJobLastLaborDate(JobNo);
            LastLaborDate = LLD.ToShortDateString();

            DaysLate = (System.DateTime.Now - LLD).Days;
        }
    }
    public class BugReport_Report : SIU_TaskList
    {
        public bool Accepted;
        public bool Working;
        public bool Testing;

        public BugReport_Report ( SIU_TaskList Task )
        {
            Mapper.CreateMap<SIU_TaskList, BugReport_Report>();
            Mapper.Map(Task, this);
           
            Accepted = (Task.AcceptTimeStamp != null) ? true : false;
            Working = (Task.WorkTimeStamp != null) ? true : false;
            Testing = (Task.TestingTimeStamp != null) ? true : false;
        }
    }
    public class DirectoryListing
    {
        public string DirectoryName;
        public string Html;
        public int MenuItemNo;
    }
    public class VehModelNoList
    {
        public string VehNo;
        public string VehModel;
    }
    public class SIU_TimeSheet_TimeEntryView
    {
        public string UserEmpID { get; set; }
        public DateTime EntryDate { get; set; }
        public string _JobNo { get; set; }
        public string _OhAcct { get; set; }
        public string _Dept { get; set; }
        public string _Task { get; set; }
        public string _ClassTime { get; set; }
        public string _ClassDesc { get; set; }
        public string _ClassLoc { get; set; }
        public string _ClassInstr { get; set; }
        public string _HoursType { get; set; }
        public decimal _Hours { get; set; }

        public override string ToString()
        {
            return ( "Emp:"     +   UserEmpID + "  " +
                     "Dt:"      +   EntryDate.ToShortDateString() + "  " +
                     "Job:"     +   _JobNo + "  " +
                     "Acct:"    +   _OhAcct + "  " +
                     "Dept:"    +   _Dept + "  " +
                     "Tsk:"     +   _Task + "  " +
                     "cTim:"    +   _ClassTime + "  " +
                     "cDsc:"    +   _ClassDesc + "  " +
                     "cLoc"     +   _ClassLoc + "  " +
                     "cInstr:"  +   _ClassInstr + "  " +
                     "Typ:"     +   _HoursType + "  " +
                     "Hrs:"     +   _Hours
                   );
        }
    }
    public class SIU_SubmitJobRptView
    {
        public string jobNo { get; set; }
        public string rptDisp { get; set; }
        public string chkNoData { get; set; }
        public string chkPowerDB { get; set; }
        public string chkScanned { get; set; }
        public string chk8082Box { get; set; }
        public string chkLetterJobFolder { get; set; }
        public string chkRptMaster { get; set; }
        public string chkIrData { get; set; }
        public string chkCallCust { get; set; }
        public string comments { get; set; }
    }
    public class SIU_Oh_Accounts
    {
        public string Account { get; set; }
        public string Desc { get; set; }

        public string AccountDesc
        {
            get { return Account + " " + Desc; }
        }
    }
    public class SIU_Oh_Exp_Accounts
    {
        public string Liab_Account { get; set; }
        public string Exp_Account { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }

        public string AccountDesc
        {
            get
            {
                if ( Liab_Account.Length > 0 )
                    return Liab_Account + " " + Code;
                return Exp_Account + " " + Code;
            }
        }
    }
    public class SIU_Divs_Depts
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public string Code_Name
        {
            get { return Code + " " + Name; }
        }
    }
    public class SIU_Task_Codes
    {
        public string StepNo { get; set; }
        public string Description { get; set; }

        public string StepnoDescription
        {
            get { return StepNo + " " + Description;  }
        }
    }
    public class SIU_Job
    {
        public string EmpPhoNo { get; set; }
        public int EmpNo { get; set; }
        public string JobNo { get; set; }
        public string JobDesc { get; set; }
        public string JobSite { get; set; }
        public bool JobComplete { get; set; }
        public bool OrigionalJobScope { get; set; }
        public int NumMgrs { get; set; }
        public int AddMaterial { get; set; }
        public int AddTravel { get; set; }
        public int AddLodge { get; set; }
        public int AddOther { get; set; }
        public int TotHours { get; set; }
        public string LeadTech { get; set; }

        public string JobNoDesc
        {
            get { return JobNo + " " + JobSite; }
        }

        // initialization 
        public SIU_Job()
        {
            EmpPhoNo = "";
            EmpNo = 0;
            JobNo = "";
            JobDesc = "";
            LeadTech = "";
            NumMgrs = 1;
            AddMaterial = 0;
            AddTravel = 0;
            AddLodge = 0;
            AddOther = 0;
            TotHours = 0;
        }

        public bool CanAdd()
        {
            if (EmpPhoNo.Length == 0 && EmpNo == 0) return false;
            if (JobNo.Length == 0) return false;
            if (NumMgrs == 0) return false;
            if (AddMaterial == 0) return false;
            if (AddTravel == 0) return false;
            if (AddLodge == 0) return false;
            if (AddOther == 0) return false;
            if (TotHours == 0) return false;
            return true;
        }

        public override string ToString()
        {
            return ( EmpPhoNo + "  " +
                     EmpNo + "  " +
                     JobNo + "  " +
                     NumMgrs + "  " +
                     AddMaterial + "  " +
                     AddTravel + "  " +
                     AddLodge + "  " +
                     AddOther + "  " +
                     TotHours
                   );
        }
    }
    public class SIU_Emp_Ids
    {
        public string EmpID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public string EmpidName
        {
            get { return EmpID + " " + FirstName + " " + LastName + " " + FirstName; }
        }
    }
    public class LogonRcd
    {
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _phone;
        private readonly string _email;
        private readonly string _lastUsed;

        public LogonRcd(DataSet ds)
        {
            if (ds.Tables.Count == 0) return;
            DataTable SIU_USERS_OUTSIDE = ds.Tables["SIU_USERS_OUTSIDE"];

            if (SIU_USERS_OUTSIDE == null) return;
            if (SIU_USERS_OUTSIDE.Rows.Count == 0) return;

            DataRow dr = SIU_USERS_OUTSIDE.Rows[0];
            _firstName = dr["First"].ToString();
            _lastName = dr["Last"].ToString();
            _phone = dr["Phone"].ToString();
            _email = dr["Email"].ToString();
            _lastUsed = dr["LastUsed"].ToString();

        }

        public string LastUsed { get { return _lastUsed; } }
        public string Email { get { return _email; } }
        public string Phone { get { return _phone; } }
        public string LastName { get { return _lastName; } }
        public string FirstName { get { return _firstName; } }
    }
    public class DirectoryList
    {
        public string DisplayName;
        public string ShortName;
        public string LongName;
        public string Icon;
        public string Href;
        public string Ext;
        public string SubPath;  //Full Path less Virt Root and Shortname
    }
    public class PhoneDirectoryList
    {
        public string RootDirName;
        public string RootSubDirCnt;
        public string RootFileCnt;

        public string SubDirName;
        public string SubDirDirCnt;
        public string SubFileCnt;
        public string SubDirPath;

        public string FileName;
        public string FileType;
        public string FilePath;
    }

    //////////////////////////////////
    // TimeStamp toString Extension //
    //////////////////////////////////
    public static class TimeStamp
    {
        public static string TimestampToString(System.Data.Linq.Binary binary)
        {
            byte[] binarybytes = binary.ToArray();

            string result = "";
            foreach (byte b in binarybytes)
            {
                result += b + "|";
            }


            result = result.Substring(0, result.Length - 1);
            return result;
        }

        public static System.Data.Linq.Binary StringToTimestamp(string s)
        {
            string[] arr = s.Split('|');
            byte[] result = new byte[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                result[i] = Convert.ToByte(arr[i]);
            }
            return result;
        }         
    }


    ////////////////////////////////////////////////////////////////
    // Enumerate All Days Of A Given Week (defaults To This Week) //
    ////////////////////////////////////////////////////////////////
    public class DOW
    {
        private DateTime _MonDT;
        private DateTime _TueDT;
        private DateTime _WedDT;
        private DateTime _ThuDT;
        private DateTime _FriDT;
        private DateTime _SatDT;
        private DateTime _SunDT;

        public string MonStr { get { return _MonDT.ToShortDateString(); } }
        public string TueStr { get { return _TueDT.ToShortDateString(); } }
        public string WedStr { get { return _WedDT.ToShortDateString(); } }
        public string ThuStr { get { return _ThuDT.ToShortDateString(); } }
        public string FriStr { get { return _FriDT.ToShortDateString(); } }
        public string SatStr { get { return _SatDT.ToShortDateString(); } }
        public string SunStr { get { return _SunDT.ToShortDateString(); } }

        public DateTime MonDate { get { return _MonDT; } }
        public DateTime TueDate { get { return _TueDT; } }
        public DateTime WedDate { get { return _WedDT; } }
        public DateTime ThuDate { get { return _ThuDT; } }
        public DateTime FriDate { get { return _FriDT; } }
        public DateTime SatDate { get { return _SatDT; } }
        public DateTime SunDate { get { return _SunDT; } }

        public DayOfWeek DTdow;

        public DOW()
        {
            Calc_DOW(System.DateTime.Now);
        }

        public DOW(DateTime _date)
        {
            Calc_DOW(_date);
        }

        private void Calc_DOW(DateTime DTnow)
        {
            DTdow = DTnow.DayOfWeek;

            ////////////////////////////
            // Set The Date To Monday //
            ////////////////////////////
            switch (DTdow.ToString().ToLower().Substring(0, 3))
            {
                case "tue": DTnow = DTnow.AddDays(-1); break;
                case "wed": DTnow = DTnow.AddDays(-2); break;
                case "thu": DTnow = DTnow.AddDays(-3); break;
                case "fri": DTnow = DTnow.AddDays(-4); break;
                case "sat": DTnow = DTnow.AddDays(-5); break;
                case "sun": DTnow = DTnow.AddDays(-6); break;
            }

            ////////////////////////////////
            // Build Date For Next 7 Days //
            ////////////////////////////////
            for (int x = 0; x++ < 7; DTnow = DTnow.AddDays(1))
                SetDayProp(DTnow);
        }

        //////////////////////////////////////
        // Set A Day-Of-Week Property Value //
        //////////////////////////////////////
        private void SetDayProp(DateTime DT)
        {
            switch (DT.DayOfWeek.ToString().ToLower().Substring(0, 3))
            {
                case "mon": _MonDT = DT; break;
                case "tue": _TueDT = DT; break;
                case "wed": _WedDT = DT; break;
                case "thu": _ThuDT = DT; break;
                case "fri": _FriDT = DT; break;
                case "sat": _SatDT = DT; break;
                case "sun": _SunDT = DT; break;
            }
        }


        public static int Weeks(int year, int month)
        {
            const DayOfWeek wkstart = DayOfWeek.Monday;

            DateTime first = new DateTime(year, month, 1);
            int firstwkday = (int)first.DayOfWeek;
            int otherwkday = (int)wkstart;

            int offset = ((otherwkday + 7) - firstwkday) % 7;

            double weeks = (double)(DateTime.DaysInMonth(year, month) - offset) / 7d;

            return (int)Math.Ceiling(weeks);
        }

        public static int GetWeekInMonth(DateTime date)
        {
            DateTime tempdate = date.AddDays(-date.Day + 1);

            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNumStart = ciCurr.Calendar.GetWeekOfYear(tempdate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            return weekNum - weekNumStart + 1;

        }
    }



}