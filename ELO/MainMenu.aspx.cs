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


            ///////////////////////////
            // Check For Admin Priv. //
            ///////////////////////////
            StringCollection sessionVar = (StringCollection)Session["UserGroups"];
            if (sessionVar != null && sessionVar.Contains("ELO_EXP_TEST"))
            {
                A7.HRef = "/ELO/MealsExpEntry2.aspx";
            }
        }
    }

}