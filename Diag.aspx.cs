using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Diag : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            return;

        SessionDiag.Text += "<h3>Session Object</h2><table>";

        foreach (string SessionKey in Session.Keys)
        {
            
            var SessionVar = Session[SessionKey];


            if (SessionVar is StringCollection)
                SessionDiag.Text += "<tr><td><h4>" + SessionKey + "</h4></td>";
            else
                SessionDiag.Text += "<tr><td>" + SessionKey + "</td>";

            if ( SessionVar is string )
            {
                SessionDiag.Text += "<td>" + SessionVar + "</td>";
            }




            if (SessionVar is StringCollection)
            {
                SessionDiag.Text += "<td>" ;
                foreach (var StrColItem in (StringCollection)SessionVar)
                {
                    SessionDiag.Text += StrColItem + "<br/>";
                }
                SessionDiag.Text += "</td>";
            }

            SessionDiag.Text += "</tr>";
        }

        SessionDiag.Text += "</table>";


        HolidayCalculator hc = new HolidayCalculator(System.DateTime.Now, HttpContext.Current.Server.MapPath(@"/App_Code/Holidays.xml"));
        //Holidays.Text += TestHolidayDate(hc, "01/01/2012");
        //Holidays.Text += TestHolidayDate(hc, "01/01/2013");
        //Holidays.Text += TestHolidayDate(hc, "01/01/2014");

        //Holidays.Text += TestHolidayDate(hc, "12/25/2012");
        //Holidays.Text += TestHolidayDate(hc, "12/25/2013");
        //Holidays.Text += TestHolidayDate(hc, "12/25/2014");
        //Holidays.Text += "<br/>";

        Holidays.Text += hc.ListHolidays();
    }

    private string TestHolidayDate(HolidayCalculator hc, string DateToTest)
    {
        string result = DateToTest;
        if (!hc.IsHoldayDate(DateToTest))
            result += " Is NOT A Holiday<br/>";
        else
            result += " Is A Holiday<br/>";

        return result;
    }

    protected void TestEmailClick(object sender, EventArgs e)
    {
        WebMail.TestEmail(BusinessLayer.UserEmail);
    }
}