using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class My_MyHome : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "MyHome.Page_Load";

        //////////////////////////////////////////////////////////
        // But Unless This Is A New / Refresh Form, We Are Done //
        //////////////////////////////////////////////////////////
        if (IsPostBack) return;

        /////////////////////
        // Save The Emp ID //
        /////////////////////
        hlblEID.InnerText = BusinessLayer.UserEmpID;

        SuprArea.Visible = (BusinessLayer.UserName.ToLower() == "ltruitt") ? true : false;
        SuprArea.Visible = (BusinessLayer.UserName.ToLower() == "ltruitt" || BusinessLayer.UserName.ToLower() == "sstrong") ? true : false;
    }


    protected void EmpChange(object sender, EventArgs e)
    {
        Session["UserEmpID"] = ((Button) sender).Text;
        Session["UserFullName"] = "USERID SET TO " + ((Button)sender).Text; ;
        Response.Redirect(Request.Path);
    }
}