using System;
using System.Web;


public partial class SessionAbandon : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Web.Security.FormsAuthentication.SignOut();
        

        Response.ClearHeaders();
        Response.Clear();

        Response.StatusCode = 401;
        Response.Status = "401 Unauthorized";
        Response.StatusDescription = "Authentication required";
        Response.End();

        Session.Abandon();
        Session.Clear();
        Session.Timeout = 1;

        Response.Cookies.Remove(System.Web.Security.FormsAuthentication.FormsCookieName);

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.MinValue);
        Response.Cache.SetNoStore();

        string realm = "Shermco";
        Response.AddHeader("WWW-Authenticate", string.Format(@"BASIC Realm={0} ({1})", realm, DateTime.Now.ToString()));


    }
}