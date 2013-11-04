using System;

public partial class ELO_VehDotNonCorrected : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string method = "ELO_VehDotNonCorrected.PageLoad";

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


    }
}