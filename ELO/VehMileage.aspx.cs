using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShermcoYou.DataTypes;

public partial class ELO_VehMileage : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "ELO_VehMileage.PageLoad";

        //////////////////////////////////////////////////////////
        // But Unless This Is A New / Refresh Form, We Are Done //
        //////////////////////////////////////////////////////////
        if (IsPostBack) return;

        ///////////////////////////
        // Get The Employee Name //
        ///////////////////////////
        lblEmpName.InnerText = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);

        ///////////////////////////////////////////
        // Bury The Employee ID For AJAX Lookups //
        ///////////////////////////////////////////
        hlblEID.InnerText = BusinessLayer.UserEmpID;

        //////////////////////////
        // Load Allowable Weeks //
        //////////////////////////
        DOW WeekDays = new DOW();
        hlblThisWeek.InnerText = WeekDays.MonStr + " - " + WeekDays.MonDate.AddDays(6).ToShortDateString();
        hlblPrevWeek.InnerText = WeekDays.MonDate.AddDays(-7).ToShortDateString() + " - " + WeekDays.MonDate.AddDays(-1).ToShortDateString();
        hlblNextWeek.InnerText = WeekDays.MonDate.AddDays(7).ToShortDateString() + " - " + WeekDays.MonDate.AddDays(13).ToShortDateString();

        SuprArea.Visible = (BusinessLayer.UserName.ToLower() == "ltruitt") ? true : false;



    }
}