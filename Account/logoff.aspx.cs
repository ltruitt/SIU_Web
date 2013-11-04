using System;
using System.Web;


public partial class Account_logoff : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("/HomePage.aspx");
    }
}