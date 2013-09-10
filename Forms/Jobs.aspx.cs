using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShermcoYou.DataTypes;

public partial class Forms_Jobs : System.Web.UI.Page
{

#region "Private Members"
    private List<SIU_Job> Jobs;
    private List<Shermco_Employee> Emps;
    private List<Shermco_Employee> CellPhones;
#endregion



#region "Event Handlers"

    /// ----------------------------------------------------------------------------- 
    /// Page_Load runs when the control is loaded 
    /// ----------------------------------------------------------------------------- 
    protected void Page_Load(object sender, System.EventArgs e)
    {
        string Method = "Forms_Jobs.PageLoad";

        try
        {

            // If this is the first visit to the page, bind the role data to the datalist 
            if (Page.IsPostBack == false)
            {
                try
                {
                    //////////////////////////
                    // Build Requestor Info //
                    //////////////////////////
                    lblEmpName.InnerText = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);

                    //////////////////////////////////////
                    // Get Available Jobs And Employees //
                    //////////////////////////////////////
                    Jobs = SqlServer_Impl.GetActiveJobs();

                    ////////////////////////////
                    // Bind List Of Open Jobs //
                    ////////////////////////////
                    ddJobNo.DataSource = Jobs;
                    ddJobNo.DataTextField = "JobNoDesc";
                    ddJobNo.DataValueField = "JobNo";
                    ddJobNo.DataBind();
                }

                catch (Exception exc)
                {
                    //Module failed to load 
                    string err = exc.Message;
                }

                lblSelectedJob.Text = "";
                lblSelectedJob.Visible = false;
            }
        }

        catch (Exception exc)
        {
            //Module failed to load 
            string err = exc.Message;
        }
    }

    /// ----------------------------------------------------------------------------- 
    /// cmdCancel_Click runs when the cancel button is clicked 
    /// ----------------------------------------------------------------------------- 
    protected void cmdCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("/", false);
    }

    /// ----------------------------------------------------------------------------- 
    /// cmdUpdate_Click runs when the update button is clicked 
    /// ----------------------------------------------------------------------------- 
    protected void cmdUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            SIU_Complete_Job objSiHours = new SIU_Complete_Job();

            objSiHours.EmpNo = int.Parse(BusinessLayer.UserEmpID);
            objSiHours.JobNo = ddJobNo.Text;
            objSiHours.JobComplete = cbJobComplete.Checked;
            objSiHours.OrigionalScope = cbExtraBilling.Checked;
            objSiHours.NumMgrs = int.Parse(txtNumMgrs.Text);
            objSiHours.AddMaterial = int.Parse(txtAddMaterial.Text);
            objSiHours.AddTravel = int.Parse(txtAddTravel.Text);
            objSiHours.AddLodge = int.Parse(txtAddLodge.Text);
            objSiHours.AddOther = int.Parse(txtAddOther.Text);
            objSiHours.TotHours = int.Parse(txtTotHours.Text);

            SqlServer_Impl.RecordSiuCompletedJob(objSiHours);

            if (cbJobComplete.Checked)
            {
                WebMail.JobCompletionEmail(objSiHours, cbExtraBilling.Checked, BusinessLayer.UserEmail, BusinessLayer.UserFullName);
            }



        // Redirect back to the portal home page 
            Response.Redirect("/", true);
        }
        catch
        {
            //Module failed to load 
        }
    }

    protected void SelectedJobChanged(object sender, EventArgs e)
    {
        var Job = SqlServer_Impl.GetJobByNo(ddJobNo.Text);
        var SiuJob = SqlServer_Impl.GetSiuCompletedJob(ddJobNo.Text);

        lblJobSelection.Text = "JOB: " + ddJobNo.Text;
        ddJobNo.Visible = false;

        lblSelectedJob.Visible = true;
        if (Job.Description.Length > 0)
            lblSelectedJob.Text = Job.Description;
        else
            lblSelectedJob.Text = "No Job Description Provided";

        PanelBasicBilling.Visible = true;
        cmdUpdate.Visible = true;

        if ( SiuJob != null)
        {
            cbJobComplete.Checked = SiuJob.JobComplete;
            cbExtraBilling.Checked = SiuJob.OrigionalScope;

            if (SiuJob.OrigionalScope == true)
            {
                PanelDetailBilling.Visible = true;

                txtNumMgrs.Text = SiuJob.NumMgrs.ToString();
                txtAddMaterial.Text = SiuJob.AddMaterial.ToString();
                txtAddTravel.Text = SiuJob.AddTravel.ToString();
                txtAddLodge.Text = SiuJob.AddLodge.ToString();
                txtAddOther.Text = SiuJob.AddOther.ToString();
                txtTotHours.Text = SiuJob.TotHours.ToString();
            }
        }
    }

    protected void ScopeChanged(object sender, EventArgs e)
    {
        if (cbExtraBilling.Checked == true)
            PanelDetailBilling.Visible = true;
        else
            PanelDetailBilling.Visible = false;
    }
#endregion
}