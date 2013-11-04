using System;
using ShermcoYou.DataTypes;

public partial class ELO_VehDotEntry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string method = "ELO_VehDotEntry.PageLoad";

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

        //////////////////////////////////////////////////////////
        // Assign Each Day-Of-Week Button A Date Value So If Is //
        // Clicked, It Can Populate The Date Field              //
        //////////////////////////////////////////////////////////
        DOW weekDays = new DOW();
        btnMon.Attributes.Add("DateForThis", weekDays.MonStr);
        btnTue.Attributes.Add("DateForThis", weekDays.TueStr);
        btnWed.Attributes.Add("DateForThis", weekDays.WedStr);
        btnThu.Attributes.Add("DateForThis", weekDays.ThuStr);
        btnFri.Attributes.Add("DateForThis", weekDays.FriStr);
        btnSat.Attributes.Add("DateForThis", weekDays.SatStr);
        btnSun.Attributes.Add("DateForThis", weekDays.SunStr);

        DOW prevDays = new DOW(DateTime.Now.AddDays(-7));
        btnMon.Attributes.Add("DateForPrev", prevDays.MonStr);
        btnTue.Attributes.Add("DateForPrev", prevDays.TueStr);
        btnWed.Attributes.Add("DateForPrev", prevDays.WedStr);
        btnThu.Attributes.Add("DateForPrev", prevDays.ThuStr);
        btnFri.Attributes.Add("DateForPrev", prevDays.FriStr);
        btnSat.Attributes.Add("DateForPrev", prevDays.SatStr);
        btnSun.Attributes.Add("DateForPrev", prevDays.SunStr);

        //////////////////////////////////////////////////////
        // Highlight The Day-Of-Week Button For Todays Date //
        //////////////////////////////////////////////////////
        switch (weekDays.DTdow.ToString().ToLower().Substring(0, 3))
        {
            case "mon": btnMon.Attributes.Remove("class"); btnMon.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); txtEntryDate.InnerText = weekDays.MonStr; break;
            case "tue": btnTue.Attributes.Remove("class"); btnTue.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); txtEntryDate.InnerText = weekDays.TueStr; break;
            case "wed": btnWed.Attributes.Remove("class"); btnWed.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); txtEntryDate.InnerText = weekDays.WedStr; break;
            case "thu": btnThu.Attributes.Remove("class"); btnThu.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); txtEntryDate.InnerText = weekDays.ThuStr; break;
            case "fri": btnFri.Attributes.Remove("class"); btnFri.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); txtEntryDate.InnerText = weekDays.FriStr; break;
            case "sat": btnSat.Attributes.Remove("class"); btnSat.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); txtEntryDate.InnerText = weekDays.SatStr; break;
            case "sun": btnSun.Attributes.Remove("class"); btnSun.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); txtEntryDate.InnerText = weekDays.SunStr; break;
        }
    }
}