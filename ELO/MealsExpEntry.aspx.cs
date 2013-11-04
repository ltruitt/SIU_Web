using System;
using ShermcoYou.DataTypes;

public partial class ELO_MealsExpEntry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string method = "ELO_MealsExpEntry.PageLoad";

        //////////////////////////////////////////////////////////
        // But Unless This Is A New / Refresh Form, We Are Done //
        //////////////////////////////////////////////////////////
        if (IsPostBack) return;

        ////////////////////////////
        // Show The Employee Name //
        ////////////////////////////
        lblEmpName.InnerHtml = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);
        hlblMileRate.InnerHtml = SqlServer_Impl.GetMilesRate().ToString();

        ///////////////////////////////////////////
        // Bury The Employee ID For AJAX Lookups //
        ///////////////////////////////////////////
        hlblEID.InnerHtml = BusinessLayer.UserEmpID;

        ////////////////////
        // Setup Calendar //
        ////////////////////
        DOW WeekDays = new DOW();
        hlblSD.InnerHtml = WeekDays.SunDate.AddDays(-14).ToShortDateString();
        hlblED.InnerHtml = WeekDays.SunDate.AddDays(6).ToShortDateString();

        SuprArea.Visible = (BusinessLayer.UserName.ToLower() == "ltruitt") ? true : false;

    }
}