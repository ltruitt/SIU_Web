using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class Safety_SafetyMeeting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) { }

    [WebMethod(EnableSession = false)]
    public static string MovieStart(string movieId, string pos, string dur)
    {
        return "";
    }





    [WebMethod(EnableSession = false)]
    public static string MovieComplete(string movieId)
    {
        return "";
    }
}