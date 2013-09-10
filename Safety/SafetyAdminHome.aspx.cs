using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Safety_SafetyAdmin : System.Web.UI.Page
{
    private const string DATETIME_FORMAT = "MM/dd/yyyy";

    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "Safety_SafetyAdmin.PageLoad";

        if (!IsPostBack)
        {
            return;


            //SafetyPaysQomAdminLink.InnerHtml = "The current Q.O.M. Ends " + FormatDateTime(Counts.QomCurrentEnd) + ".";
            //if (Counts.QomNextStart == null)
            //    SafetyPaysQomAdminLink.InnerHtml += "<b>THIS IS THE LAST DEFINED QUESTION...</b>";
            //else
            //    SafetyPaysQomAdminLink.InnerHtml += " The Next Q.O.M. runs " +
            //                                   FormatDateTime(Counts.QomNextStart) + " to " + FormatDateTime(Counts.QomNextEnd);
        }
    }


    public string FormatDateTime(object dtvalue)
    {
        string sDateTime = Convert.ToString(dtvalue);

        if (IsDateTime(sDateTime) == true)
        {
            System.DateTime dt = System.DateTime.Parse(sDateTime);
            if (dt == new DateTime(1900, 1, 1))
                sDateTime = string.Empty;
            else
                sDateTime = dt.ToString(DATETIME_FORMAT);
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