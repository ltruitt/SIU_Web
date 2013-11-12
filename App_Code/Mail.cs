using System;
using System.Linq;
using System.Net.Mail;
using System.Web;
using MailMessage = System.Net.Mail.MailMessage;


//using System.Net;
//using System.Net.Mail;

//var fromAddress = new MailAddress("from@gmail.com", "From Name");
//var toAddress = new MailAddress("to@example.com", "To Name");
//const string fromPassword = "fromPassword";
//const string subject = "Subject";
//const string body = "Body";

//var smtp = new SmtpClient
//           {
//               Host = "smtp.gmail.com",
//               Port = 587,
//               EnableSsl = true,
//               DeliveryMethod = SmtpDeliveryMethod.Network,
//               UseDefaultCredentials = false,
//               Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
//           };
//using (var message = new MailMessage(fromAddress, toAddress)
//                     {
//                         Subject = subject,
//                         Body = body
//                     })
//{
//    smtp.Send(message);
//}
//MailMessage message = new MailMessage("ltruitt@shermco.com", txtTo, Subject, MailMessage);
// Create mailing addresses 
// that includes a UTF8 character in the display name.
// MailAddress from = new MailAddress("jane@contoso.com",  "Jane " + (char)0xD8+ " Clayton", 
//MailAddress from = new MailAddress("ltruitt@Shermco.com");
//MailAddress to = new MailAddress("JobCompleted@Shermco.com");
//MailAddress cc = new MailAddress("ltruitt@Shermco.com");

// Specify the message content.
//MailMessage message = new MailMessage(from, to);

//message.Body = MailMessage;
//message.BodyEncoding = System.Text.Encoding.UTF8;


// Include some non-ASCII characters in body and subject.
//string someArrows = new string(new char[] {'\u2190', '\u2191', '\u2192', '\u2193'});
//message.Body += Environment.NewLine + someArrows;
//message.Subject = Subject;
//message.SubjectEncoding = System.Text.Encoding.UTF8;


// Set the method that is called back when the send operation ends.
//client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

// The userState can be any object that allows your callback 
// method to identify this send operation.
// For this example, the userToken is a string constant.
//string userState = "test message1";
//client.SendAsync(message, userState);





//string answer = Console.ReadLine();
//// If the user canceled the send, and mail hasn't been sent yet,
//// then cancel the pending operation.
//if (answer.StartsWith("c") && mailSent == false)
//{
//    client.SendAsyncCancel();
//}
// Clean up.
//message.Dispose();





public static class WebMail
{
    private static void NetMail(string txtTo, string Subject, string MailMessage)
    {
        ////////////////////////////////////
        // Use Web Config For Config Info //
        ////////////////////////////////////
        SmtpClient client = new SmtpClient {UseDefaultCredentials = false};
        if (Environment.MachineName.ToLower() == "tsdc-dev") txtTo = "ltruitt@shermco.com";
        
        try
        {
            client.Send(new MailMessage("noreply@shermco.com", txtTo, Subject, MailMessage));
        }
        catch (Exception e)
        {
            SqlServer_Impl.LogDebug("NetMail", e.Message);
        }        
    }


    private static string ToShortDate(DateTime dt)
    {
        return dt != null ? dt.ToShortDateString() : "";
    }

    private static string ToShortDate(DateTime? dt)
    {
        return (dt != null) ? ((DateTime)dt).ToShortDateString() : "";
    }

    //////////////////////////////////
    // Safety Pays Email Generation //
    //////////////////////////////////
    public static void SafetyPaysNewEmail(SIU_SafetyPaysReport Report, string UserEmail, string UserFullName)
    {
        ///////////////////////////
        // Send New-Report Email //
        ///////////////////////////
        string eMailSubject = "Safety Pays " + Report.IncTypeTxt + " Submission From: " + UserFullName;

        string emailBody = "";
        emailBody += "Report ID: " + Report.IncidentNo + Environment.NewLine;
        emailBody += "Submitted By: " + UserFullName + Environment.NewLine;
        emailBody += "Date Submitted: " + ToShortDate(Report.IncOpenTimestamp) + Environment.NewLine;
        emailBody += "Submission Type: " + Report.IncTypeTxt + Environment.NewLine;
        if (Report.JobNo.Length > 0)
            emailBody += "Job: " + Report.JobNo + Environment.NewLine;

        if ( Report.JobSite != "-")
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
        NetMail(UserEmail, eMailSubject, emailBody);
    }
    public static void SafetyPaysAcceptEMail(SIU_SafetyPaysReport UpdRcd, string ccUserEmail, string UserFullName)
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

        emailBody += "Approved and Closed By: " + UserFullName + Environment.NewLine;
        emailBody += "Closed On: " + ToShortDate(UpdRcd.IncCloseTimestamp) + Environment.NewLine + Environment.NewLine;

        emailBody += "Notes From EHS: " + Environment.NewLine + UpdRcd.ehsRepsonse + Environment.NewLine;

        ////////////////////////////
        // Send To Original  User //
        ////////////////////////////
        if (rptByEmp == null)
            return;

        NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);

        //////////////////////////////////////////////////
        // If A Courtesy Copy Was Requested, Send It On //
        //////////////////////////////////////////////////
        if ( ccUserEmail.Length > 0 )
        {
            eMailSubject = "Courtesy Copy: " + eMailSubject;
            emailBody = "The EHS Department requested that a copy of the following Email be forwarded to you." + emailBody + Environment.NewLine + Environment.NewLine;
            NetMail(ccUserEmail, eMailSubject, emailBody);
        }
    }
    public static void SafetyPaysRejectedEMail(SIU_SafetyPaysReport UpdRcd, string ccUserEmail, string UserFullName)
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
        if (UpdRcd.JobNo.Length > 0 )
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

        emailBody += "Rejected and Closed By: " + UserFullName + Environment.NewLine;
        emailBody += "Rejected On: " + ToShortDate(UpdRcd.IncCloseTimestamp) + Environment.NewLine + Environment.NewLine;

        emailBody += "Notes From EHS: " + Environment.NewLine + UpdRcd.ehsRepsonse + Environment.NewLine;

        //////////////////////////////////
        // Send Email To Original  User //
        //////////////////////////////////
        if (rptByEmp != null)
            NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);

        //////////////////////////////////////////////////
        // If A Courtesy Copy Was Requested, Send It On //
        //////////////////////////////////////////////////
        if (ccUserEmail.Length > 0)
        {
            eMailSubject = "Courtesy Copy: " + eMailSubject;
            emailBody = "The EHS Department requested that a copy of the following Email be forwarded to you." + Environment.NewLine + Environment.NewLine + emailBody;
            NetMail(ccUserEmail, eMailSubject, emailBody);
        }
    }
    public static void SafetyPaysWorkingEMail(SIU_SafetyPaysReport UpdRcd, string ccUserEmail, string UserFullName)
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

        emailBody += "Accepted for Work By: " + UserFullName + Environment.NewLine;
        emailBody += "Accepted On: " + ToShortDate(UpdRcd.IncCloseTimestamp) + Environment.NewLine + Environment.NewLine;

        emailBody += "Notes From EHS: " + Environment.NewLine + UpdRcd.ehsRepsonse + Environment.NewLine;

        ///////////////////////////
        // Send To Original  User //
        ///////////////////////////
        if (rptByEmp != null)
            NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);

        //////////////////////////////////////////////////
        // If A Courtesy Copy Was Requested, Send It On //
        //////////////////////////////////////////////////
        if (ccUserEmail.Length > 0)
        {
            eMailSubject = "Courtesy Copy: " + eMailSubject;
            emailBody = "The EHS Department requested that a copy of the following Email be forwarded to you." + Environment.NewLine + Environment.NewLine + emailBody;
            NetMail(ccUserEmail, eMailSubject, emailBody);
        }
    }
    public static void SafetyPaysClosedEMail(SIU_SafetyPaysReport UpdRcd, string ccUserEmail, string UserFullName)
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

        emailBody += "Closed By: " + UserFullName + Environment.NewLine;
        emailBody += "Closed On: " + ToShortDate(UpdRcd.IncCloseTimestamp) + Environment.NewLine + Environment.NewLine;

        emailBody += "Notes From EHS: " + Environment.NewLine + UpdRcd.ehsRepsonse + Environment.NewLine;

        ////////////////////////////
        // Send To Original  User //
        ////////////////////////////
        if (rptByEmp != null)
            NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);

        //////////////////////////////////////////////////
        // If A Courtesy Copy Was Requested, Send It On //
        //////////////////////////////////////////////////
        if (ccUserEmail.Length > 0)
        {
            eMailSubject = "Courtesy Copy: " + eMailSubject;
            emailBody = "The EHS Department requested that a copy of the following Email be forwarded to you." + Environment.NewLine + Environment.NewLine + emailBody;
            NetMail(ccUserEmail, eMailSubject, emailBody);
        }
    }
    public static void SafetyPaysTaskAssignEMail(SIU_SafetyPays_TaskList UpdRcd, string UserEmail, string UserFullName)
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
            emailBody += "Manage your task status here: http://" + System.Environment.MachineName + "/Safety/SafetyPays/SafetyPaysTasks.aspx?RptID=" + UpdRcd.IncidentNo + Environment.NewLine;
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

            emailBody += "Task Created By: " + UserFullName + Environment.NewLine;
            emailBody += "Task Due By: " + UpdRcd.DueDate + Environment.NewLine;
            emailBody += "Task Description: " + UpdRcd.TaskDefinition + Environment.NewLine + Environment.NewLine;

            ////////////////////////////
            // Send To Original  User //
            ////////////////////////////
            if (rptByEmp != null)
                NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);

            /////////////////
            // Assigned To //
            /////////////////
            rptByEmp = SqlServer_Impl.GetEmployeeByNo(UpdRcd.AssignedEmpID);
            NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);
        }
    }
    public static void SafetyPaysTaskCompleteEMail(SIU_SafetyPays_TaskList UpdRcd, string UserEmail, string UserFullName, string CloseNotes)
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
            emailBody += "A task related to the Safety Pays submission (report)"+ Environment.NewLine;
            emailBody += "shown below was closed.  This task may be one "+ Environment.NewLine;
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

            emailBody += "Task Closed By: " + UserFullName + Environment.NewLine;
            emailBody += "Task Closed On: " + UpdRcd.CompletedDate + Environment.NewLine;
            emailBody += "Task Description: " + UpdRcd.TaskDefinition + Environment.NewLine + Environment.NewLine;

            emailBody += "Closing Notes: " + CloseNotes + Environment.NewLine;


            ////////////////////////////
            // Send To Original  User //
            ////////////////////////////
            if (rptByEmp != null)
                NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);

            /////////////////
            // Assigned To //
            /////////////////
            rptByEmp = SqlServer_Impl.GetEmployeeByNo(UpdRcd.AssignedEmpID);
            NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);
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

        Shermco_Employee ObsByEmp = SqlServer_Impl.GetEmployeeByNo(UpdRcd.ObservedEmpID);
        NetMail(ObsByEmp.Company_E_Mail, eMailSubject, emailBody);
    }

    public static void JobReportIrNotice(Shermco_Job_Report Rpt)
    {
        const string eMailSubject = "I/R Alert for Reports";
        const string addressList = "SIU_IR_JOBRPT@shermco.com";

        string emailBody = "An I/R report was submitted for job: " + Rpt.Job_No_ + Environment.NewLine;
        emailBody += "Data in Dropbox: " + ((Rpt.TmpIRData == 1) ? "Yes" : "No") + Environment.NewLine;
        emailBody += "Copies Requested: " + Rpt.No__of_Copies + Environment.NewLine;
        emailBody += "Email: " + Rpt.Email + Environment.NewLine;
        emailBody += "Comment: " + Rpt.Comment + Environment.NewLine;
        emailBody += "Submitted by: " +  SqlServer_Impl.GetEmployeeNameByNo(Rpt.Turned_in_By_Emp__No_) + Environment.NewLine;
        emailBody += "Lead Tech: " + SqlServer_Impl.GetJobLeadTechName(Rpt.Job_No_) + Environment.NewLine;
        emailBody += ((Rpt.IROnly == 1) ? "This is the only report for the job." : "IR is portion of Final Report") + Environment.NewLine;

        NetMail(addressList, eMailSubject, emailBody);
    }
    public static void JobSalesNotice(Shermco_Job_Report Rpt)
    {
        const string eMailSubject = "Job Report Sales Follow-up Notice";
        string addressList = "kahrendt@shermco.com, spayne@shermco.com, rloveless@shermco.com";

        var jr = SqlServer_Impl.GetJobByNo(Rpt.Job_No_);
        var sp = SqlServer_Impl.GetEmployeeByNo(jr.Sales_Person);
        if (sp != null)
            addressList += ", " + sp.Company_E_Mail;
        

        string emailBody = "A report was submitted for job: " + Rpt.Job_No_ + " with deficiencies or sales follow-up" + Environment.NewLine + Environment.NewLine;

        emailBody += Rpt.SalesFollowUp_Comment;

        emailBody += Environment.NewLine + Environment.NewLine + "Comments:" + Environment.NewLine;
        emailBody += Rpt.Comment;
        emailBody += Environment.NewLine;

        NetMail(addressList, eMailSubject, emailBody);
    }

    public static void MovieCompletedEMail(string empID, string videoName)
    {
        const string eMailSubject = "Training Video Completed";
        string addressList = SqlServer_Impl.GetEmployeeByNo(empID).Company_E_Mail;

        if (addressList == null) return;
        if (addressList.Length == 0) return;


        string emailBody = "Thank you for viewing the following video persentation." + Environment.NewLine + Environment.NewLine;

        emailBody += "Video: " + videoName + Environment.NewLine + Environment.NewLine;

        emailBody += "There may be related documents and quizzes. "   + Environment.NewLine;
        emailBody += "These documents will be listed to the right of the"  + Environment.NewLine;
        emailBody += "video under the heading SUPPORT DOCUMENTS." + Environment.NewLine;


        NetMail(addressList, eMailSubject, emailBody);        
    }





    public static void TestEmail(string addressList)
    {
        const string eMailSubject = "SIU EMAIL TEST";
        //addressList = "ltruitt@shermco.com, truittjl@texassdc.com, truittjl@tx.rr.com";

        string emailBody = "This is an SIU Web site test message" + Environment.NewLine + Environment.NewLine;

        emailBody += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890" + Environment.NewLine + Environment.NewLine;

        NetMail(addressList, eMailSubject, emailBody);
    }










    /////////////////////////////////
    // Bug Report EMail Generation //
    /////////////////////////////////
    public static void BugReportNewEmail(SIU_TaskList Report, string UserEmail, string UserFullName)
    {

        ///////////////////////////
        // Send New-Report Email //
        ///////////////////////////
        string eMailSubject = "New ShermcoYOU Bug / Suggestion Report Submission From: " + UserFullName;

        string emailBody = Report.OpenTimeStamp + Environment.NewLine;
        emailBody += "New ShermcoYOU Bug / Suggestion From: " + UserFullName + Environment.NewLine + Environment.NewLine;
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
        NetMail(UserEmail, eMailSubject, emailBody);

        //////////////////////
        // Send Email To Me //
        //////////////////////
        NetMail("ltruitt@shermco.com", eMailSubject, emailBody);
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
        NetMail("ltruitt@shermco.com", eMailSubject, emailBody);

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
            NetMail(rptByEmp.Company_E_Mail, eMailSubject, emailBody);
    }

    ////////////////////////////////////
    // Job Completed Email Generation //
    ////////////////////////////////////
    public static void JobCompletionEmail(SIU_Complete_Job UpdRcd, bool ExtraBilling, string UserEmail, string UserFullName)
    {
        string eMailSubject = "Job: " +   UpdRcd.JobNo + " Work Completed Report";

        string emailBody = "Job: " + UpdRcd.JobNo + " Work Completed Report" + Environment.NewLine + Environment.NewLine;

        emailBody += "Reporting Employee:\t" + UserFullName + Environment.NewLine + Environment.NewLine;

        
        emailBody += "Job Scope Changed:\t" + ((ExtraBilling) ? "YES" : "NO") + Environment.NewLine + Environment.NewLine;
        if (ExtraBilling)
        {
            emailBody += "Managers:\t\t" + UpdRcd.NumMgrs + Environment.NewLine;
            emailBody += "Material Cost:\t\t" + UpdRcd.AddMaterial.ToString() + Environment.NewLine;
            emailBody += "Travel Cost:\t\t" +  UpdRcd.AddTravel.ToString() + Environment.NewLine;
            emailBody += "Lodge Cost:\t\t" +   UpdRcd.AddLodge.ToString() + Environment.NewLine;
            emailBody += "Other Cost:\t\t" +   UpdRcd.AddOther.ToString() + Environment.NewLine;

            emailBody += "" + Environment.NewLine;
            emailBody += "Total Hours:\t\t" + UpdRcd.TotHours.ToString() + Environment.NewLine;
        }


        NetMail("ltruitt@shermco.com", eMailSubject, emailBody);
    }


    public static void SafetyQomScoreEMail(ShermcoYou.DataTypes.SIU_Qom_QR UpdRcd)
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

    ///////////////////////////////////////
    // Hardware Request Email Generation //
    ///////////////////////////////////////
    public static void HardwareRequstSendNewEmail(SIU_IT_HW_Req Req, string EmailAddr)
    {
        string empNameEquipFor = SqlServer_Impl.GetEmployeeNameByNo(Req.For_EmpID);
        string empNameReq = SqlServer_Impl.GetEmployeeNameByNo(Req.Req_EmpID);
        //string SuprEmpID =  ((Shermco_Employee) (SqlServer_Impl.GetEmployeeByNo(Req.Req_EmpID)  )).Manager_No_;
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

        NetMail("ITDEPARTMENT@shermco.com", eMailSubject, emailBody);
        NetMail(EmailAddr, eMailSubject, emailBody);
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

        NetMail(EmailAddr, eMailSubject, emailBody);
    }
}