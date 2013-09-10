using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_logoff : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Web.Security.FormsAuthentication.SignOut();

        Session.Abandon();
        Session.Clear();
        Session.Timeout = 1;

        Response.Cookies.Remove(System.Web.Security.FormsAuthentication.FormsCookieName);

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.MinValue);
        Response.Cache.SetNoStore();
    }
}