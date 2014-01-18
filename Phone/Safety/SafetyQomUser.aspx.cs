using System;
using System.Collections.Specialized;

public partial class Safety_SafetyPays_SafetyQomUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        const string method = "Safety_SafetyQomUser";

        try
        {
            // If this is the first visit to the page, bind the role data to the datalist 
            if (Page.IsPostBack)
                return;

            //////////////////////////
            // Build Requestor Info //
            //////////////////////////
            hlblEID.InnerText = BusinessLayer.UserEmpID;
            lblEmpName.InnerText = "(" + BusinessLayer.UserEmpID + ") " + BusinessLayer.UserFullName;
        }

        catch (Exception exc)
        {
            SqlServer_Impl.LogDebug(method, exc.Message);
        }
    }
}