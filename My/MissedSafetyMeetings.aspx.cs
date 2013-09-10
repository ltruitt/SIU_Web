using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class My_MissedSafetyMeetings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "My_MissedSafetyMeetings.PageLoad";

        //////////////////////////////////////////////////////////
        // But Unless This Is A New / Refresh Form, We Are Done //
        //////////////////////////////////////////////////////////
        if (IsPostBack) return;

        ////////////////////////////
        // Show The Employee Name //
        ////////////////////////////
        lblEmpName.InnerText = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);

        ///////////////////////////////////////////
        // Bury The Employee ID For AJAX Lookups //
        ///////////////////////////////////////////
        hlblEID.InnerText = BusinessLayer.UserEmpID;

        SuprArea.Visible = (BusinessLayer.UserName.ToLower() == "ltruitt") ? true : false;
    }
}