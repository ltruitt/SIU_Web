using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Safety_Incident_Approve : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string method = "Safety_Incident_Event.PageLoad";

        //////////////////////////////////////////////////////////
        // But Unless This Is A New / Refresh Form, We Are Done //
        //////////////////////////////////////////////////////////
        if (IsPostBack) return;

        ////////////////////////////
        // Show The Employee Name //
        ////////////////////////////
        lblEmpName.InnerHtml = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);

        ///////////////////////////////////////////
        // Bury The Employee ID For AJAX Lookups //
        ///////////////////////////////////////////
        hlblEID.InnerHtml = BusinessLayer.UserEmpID;
        //hlblEID.InnerHtml = "177";


        //  SuprArea.Visible = (BusinessLayer.UserName.ToLower() == "ltruitt") ? true : false;
    }
}