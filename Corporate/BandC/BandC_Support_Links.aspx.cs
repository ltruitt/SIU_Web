using System;
using System.Collections.Specialized;


public partial class Corporate_BandC_BandC_Support_Links : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        /////////////////////////////////////////////////
        // Check If User Gets Tset Version Of Expenses //
        /////////////////////////////////////////////////
        StringCollection sessionVar = (StringCollection)Session["UserGroups"];

        BncAdmin.Visible = false;
        if (sessionVar != null && sessionVar.Contains("BNC_ADMIN"))
        {
            BncAdmin.Visible = true;
        }        
    }
}