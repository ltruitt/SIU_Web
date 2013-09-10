using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShermcoYou.DataTypes;

using System.Web.Services;

public partial class ELO_TimeEntry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "ELO_TimeEntry.PageLoad";

        //////////////////////////////////////////////////////////
        // But Unless This Is A New / Refresh Form, We Are Done //
        //////////////////////////////////////////////////////////
        if (IsPostBack) return;

        ///////////////////////////
        // Get The Employee Name //
        ///////////////////////////
        lblEmpName.InnerText = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);

        //////////////////////////////////////////
        // Default The Time Entry Date To Today //
        //////////////////////////////////////////
        txtEntryDate.Value = FormatDateTime(System.DateTime.Now);

        /////////////////////////////////////////////////////////////
        // Default O/H Department To Employee Reporting Department //
        /////////////////////////////////////////////////////////////
        Shermco_Employee Emp = SqlServer_Impl.GetEmployeeByNo((string)HttpContext.Current.Session["UserEmpID"]);
        if ( Emp != null )
            hlblDeptSelection.InnerText = Emp.Global_Dimension_1_Code;
        else
            hlblDeptSelection.InnerText = "0000";

        hlblEmpDept.InnerText = hlblDeptSelection.InnerText;

        /////////////////////////////////
        // Look For Any Reject Records //
        /////////////////////////////////
        imgRejects.Visible = SqlServer_Impl.GetTimeRejectsCnt(BusinessLayer.UserEmpID) > 0;
        imgReport.Visible = (imgRejects.Visible == true) ? false : true;


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

        DOW NextDays = new DOW(DateTime.Now.AddDays(+7));
        btnMon.Attributes.Add("DateForNext", NextDays.MonStr);
        btnTue.Attributes.Add("DateForNext", NextDays.TueStr);
        btnWed.Attributes.Add("DateForNext", NextDays.WedStr);
        btnThu.Attributes.Add("DateForNext", NextDays.ThuStr);
        btnFri.Attributes.Add("DateForNext", NextDays.FriStr);
        btnSat.Attributes.Add("DateForNext", NextDays.SatStr);
        btnSun.Attributes.Add("DateForNext", NextDays.SunStr);


        btnMon.Attributes.Add("DOW", "Mon");
        btnTue.Attributes.Add("DOW", "Tue");
        btnWed.Attributes.Add("DOW", "Wed");
        btnThu.Attributes.Add("DOW", "Thu");
        btnFri.Attributes.Add("DOW", "Fri");
        btnSat.Attributes.Add("DOW", "Sat");
        btnSun.Attributes.Add("DOW", "Sun");

        //////////////////////////////////////////////////////
        // Highlight The Day-Of-Week Button For Todays Date //
        //////////////////////////////////////////////////////
        switch (WeekDays.DTdow.ToString().ToLower().Substring(0, 3))
        {
            case "mon": btnMon.Attributes.Remove("class"); btnMon.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); break;
            case "tue": btnTue.Attributes.Remove("class"); btnTue.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); break;
            case "wed": btnWed.Attributes.Remove("class"); btnWed.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); break;
            case "thu": btnThu.Attributes.Remove("class"); btnThu.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); break;
            case "fri": btnFri.Attributes.Remove("class"); btnFri.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); break;
            case "sat": btnSat.Attributes.Remove("class"); btnSat.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); break;
            case "sun": btnSun.Attributes.Remove("class"); btnSun.Attributes.Add("class", "DowBtnCSS DowBtnSelectedCss"); break;
        }

        ////////////////////////////////////////////////////////////
        // Pass Parameters For AJAX Calls To Get Sum Hours By Day //
        // And Get Accrued Time                                   //
        ////////////////////////////////////////////////////////////
        hlblEID.InnerHtml = BusinessLayer.UserEmpID;
        hlblSD.InnerHtml = WeekDays.SunDate.AddDays(-13).ToShortDateString();
        hlblED.InnerHtml = WeekDays.SunDate.AddDays(7).ToShortDateString();
    }







    



    ///////////////////////
    // Date Time Helpers //
    ///////////////////////
    private const string DATETIME_FORMAT = "MM/dd/yyyy";
    public string FormatDateTime(object dtvalue)
    {
        string sDateTime = Convert.ToString(dtvalue);

        if (IsDateTime(sDateTime) )
        {
            DateTime dt = DateTime.Parse(sDateTime);
            sDateTime = dt == new DateTime(1900, 1, 1) ? string.Empty : dt.ToString(DATETIME_FORMAT);
        }

        return sDateTime;
    }
    public static bool IsDateTime(object value)
    {
        try
        {
            if (value == null)
                return false;

            DateTime dateTime = DateTime.Parse(value.ToString());
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}






