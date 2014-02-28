using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_BugReport : System.Web.UI.Page
{
    private static int IncNo;

    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "Forms_BugReport.PageLoad";

        try
        {
            // If this is the first visit to the page, bind the role data to the datalist 
            if (Page.IsPostBack == false)
            {
                //////////////////////////
                // Build Requestor Info //
                //////////////////////////
                lblEmpName.InnerText = BusinessLayer.UserEmail;
            }

            if (Request["RptID"] != null)
            {
                IncNo = int.Parse(Request["RptID"].Trim());
                lblBtnSubmit.Visible = false;

                SIU_TaskList Task = SqlServer_Impl.GetBugReport(IncNo).SingleOrDefault();
                this.Description.Text = Task.Description;
                this.StepsToCreate.Visible = false;
                this.lblStepsToCreate.Visible = false;
                this.lblRejectNotes.Visible = true;
                this.txtRejectNotes.Visible = true;

                this.lblBtnTaskList.Visible = false;
            }

            else
            {
                lblBtnAccept.Visible = false;
                lblBtnReject.Visible = false;

                this.lblRejectNotes.Visible = false;
                this.txtRejectNotes.Visible = false;
            }
        }

        catch (Exception exc)
        {
            //Module failed to load 
            string err = exc.Message;
        }
    }

    protected void lblBtnSubmit_Click(object sender, EventArgs e)
    {
        string Method = "Forms_BugReport.lblBtnSubmit_Click";

        ///////////////////////////////////////////
        // Make And Initialize A New Data Object //
        ///////////////////////////////////////////
        SIU_TaskList newReport = SqlDataMapper<SIU_TaskList>.MakeNewDAO<SIU_TaskList>();

        ///////////////////////////////////////////////////////
        // Map Any Data FIlled In On ASP Form To Data Object //
        // Field Names Must Match                            //
        ///////////////////////////////////////////////////////
        newReport = SqlDataMapper<SIU_TaskList>.MapAspForm(newReport, Request);

        //////////////////
        // New Incident //
        //////////////////
        newReport.EmpID = BusinessLayer.UserEmpID;
        newReport.OpenTimeStamp = System.DateTime.Now;
        newReport.EmpEmail = BusinessLayer.UserEmail;

        //////////////////////////////////
        // Write New Record To Database //
        //////////////////////////////////
        int NewIncidentNo = SqlServer_Impl.RecordBugReport(newReport);
        BusinessLayer.BugReportNewEmail(newReport, BusinessLayer.UserEmail, BusinessLayer.UserFullName);

        Response.Redirect("/", true);

    }

    protected void lblBtnAccept_Click(object sender, EventArgs e)
    {
        string Method = "Forms_BugReport.lblBtnAccept_Click";

        BusinessLayer.BugReportSendStatusEmail(SqlServer_Impl.RecordBugStatus('C', IncNo), 'C');
        Response.Redirect( @"http://" + Server.MachineName + @"/Forms/BugReportList.aspx");

    }

    protected void lblBtnReject_Click(object sender, EventArgs e)
    {
        string Method = "Forms_BugReport.lblBtnReject_Click";

        BusinessLayer.BugReportSendStatusEmail(SqlServer_Impl.RecordBugStatus('X', IncNo, this.txtRejectNotes.Text), 'X');
        Response.Redirect(@"http://" + Server.MachineName + @"/Forms/BugReportList.aspx");
    }
}