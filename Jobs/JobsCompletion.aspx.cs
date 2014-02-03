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

        try
        {

            // If this is the first visit to the page, bind the role data to the datalist 
            if (Page.IsPostBack == false)
            {
                //////////////////////////
                // Build Requestor Info //
                //////////////////////////
                lblEmpName.InnerText = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);
            }
        }

        catch (Exception exc)
        {
            SqlServer_Impl.LogDebug("JobsCompletion", exc.Message);
        }
    }


    /// ----------------------------------------------------------------------------- 
    /// cmdUpdate_Click runs when the update button is clicked 
    /// ----------------------------------------------------------------------------- 


    //        if (cbJobComplete.Checked)
    //        {
    //            WebMail.JobCompletionEmail(objSiHours, cbExtraBilling.Checked, BusinessLayer.UserEmail, BusinessLayer.UserFullName);
    //        }





#endregion
}