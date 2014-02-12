using System;
using System.Collections.Generic;
using System.Collections.Specialized;

public partial class ELO_MainMenu : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string method = "ELO_MainMenu.PageLoad";

        if (!IsPostBack)
        {
            lblEmpName.InnerText = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);


            ///////////////////////////////////
            // Turn Off Time View and Delete //
            ///////////////////////////////////
            if (SqlServer_Impl.GetTimeUnapprovedDates(BusinessLayer.UserEmpID).Count == 0)
                TimeMangDiv.Visible = false;


            /////////////////////////////////////////////////
            // Check If User Gets Tset Version Of Expenses //
            /////////////////////////////////////////////////
            StringCollection sessionVar = (StringCollection)Session["UserGroups"];

            TestExp.Visible = false;
            if (sessionVar != null && sessionVar.Contains("ELO_EXP_TEST"))
            {
                TestExp.Visible = true;
            }

            TestJobCompletion.Visible = false;
            TestJobCompletionReport.Visible = false;
            if (sessionVar != null && sessionVar.Contains("JOB_COMPLETION"))
            {
                TestJobCompletion.Visible = true;
                TestJobCompletionReport.Visible = true;
            }


            TestJobReport.Visible = false;
            if (sessionVar != null && sessionVar.Contains("JOB_REPORT_TEST"))
            {
                TestJobReport.Visible = true;
            }

        }
    }

}