using System;
using ShermcoYou.DataTypes;

public partial class ELO_VehMileage : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {
        string method = "ELO_VehMileage.PageLoad";

        //////////////////////////////////////////////////////////
        // But Unless This Is A New / Refresh Form, We Are Done //
        //////////////////////////////////////////////////////////
        if (IsPostBack) return;

        ///////////////////////////
        // Get The Employee Name //
        ///////////////////////////
        lblEmpName.InnerHtml = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);

        ///////////////////////////////////////////
        // Bury The Employee ID For AJAX Lookups //
        ///////////////////////////////////////////
        hlblEID.InnerHtml = BusinessLayer.UserEmpID;

        //////////////////////////
        // Load Allowable Weeks //
        //////////////////////////
        DOW weekDays = new DOW();
        hlblThisWeek.InnerText = weekDays.MonStr + " - " + weekDays.MonDate.AddDays(6).ToShortDateString();
        hlblPrevWeek.InnerText = weekDays.MonDate.AddDays(-7).ToShortDateString() + " - " + weekDays.MonDate.AddDays(-1).ToShortDateString();
        hlblNextWeek.InnerText = weekDays.MonDate.AddDays(7).ToShortDateString() + " - " + weekDays.MonDate.AddDays(13).ToShortDateString();

        SuprArea.Visible = (BusinessLayer.UserName.ToLower() == "ltruitt") ? true : false;



    }
}