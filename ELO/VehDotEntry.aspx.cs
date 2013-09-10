using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShermcoYou.DataTypes;

public partial class ELO_VehDotEntry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "ELO_VehDotEntry.PageLoad";

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

        //////////////////////////////////////////////////////////
        // Assign Each Day-Of-Week Button A Date Value So If Is //
        // Clicked, It Can Populate The Date Field              //
        //////////////////////////////////////////////////////////
        DOW WeekDays = new DOW();
        btnMon.Attributes.Add("DateForThis", WeekDays.MonStr);
        btnTue.Attributes.Add("DateForThis", WeekDays.TueStr);
        btnWed.Attributes.Add("DateForThis", WeekDays.WedStr);
        btnThu.Attributes.Add("DateForThis", WeekDays.ThuStr);
        btnFri.Attributes.Add("DateForThis", WeekDays.FriStr);
        btnSat.Attributes.Add("DateForThis", WeekDays.SatStr);
        btnSun.Attributes.Add("DateForThis", WeekDays.SunStr);

        DOW PrevDays = new DOW(DateTime.Now.AddDays(-7));
        btnMon.Attributes.Add("DateForPrev", PrevDays.MonStr);
        btnTue.Attributes.Add("DateForPrev", PrevDays.TueStr);
        btnWed.Attributes.Add("DateForPrev", PrevDays.WedStr);
        btnThu.Attributes.Add("DateForPrev", PrevDays.ThuStr);
        btnFri.Attributes.Add("DateForPrev", PrevDays.FriStr);
        btnSat.Attributes.Add("DateForPrev", PrevDays.SatStr);
        btnSun.Attributes.Add("DateForPrev", PrevDays.SunStr);

        //////////////////////////////////////////////////////
        // Highlight The Day-Of-Week Button For Todays Date //
        //////////////////////////////////////////////////////
        switch (WeekDays.DTdow.ToString().ToLower().Substring(0, 3))
        {
            case "mon": btnMon.Attributes.Remove("class"); btnMon.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); txtEntryDate.InnerText = WeekDays.MonStr; break;
            case "tue": btnTue.Attributes.Remove("class"); btnTue.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); txtEntryDate.InnerText = WeekDays.TueStr; break;
            case "wed": btnWed.Attributes.Remove("class"); btnWed.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); txtEntryDate.InnerText = WeekDays.WedStr; break;
            case "thu": btnThu.Attributes.Remove("class"); btnThu.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); txtEntryDate.InnerText = WeekDays.ThuStr; break;
            case "fri": btnFri.Attributes.Remove("class"); btnFri.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); txtEntryDate.InnerText = WeekDays.FriStr; break;
            case "sat": btnSat.Attributes.Remove("class"); btnSat.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); txtEntryDate.InnerText = WeekDays.SatStr; break;
            case "sun": btnSun.Attributes.Remove("class"); btnSun.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); txtEntryDate.InnerText = WeekDays.SunStr; break;
        }
    }
}