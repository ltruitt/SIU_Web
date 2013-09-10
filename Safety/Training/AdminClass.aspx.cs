using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Safety_Training_AdminClass : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "Safety_Training_AdminClass.PageLoad";

        try
        {
            // If this is the first visit to the page, bind the role data to the datalist 
            if (Page.IsPostBack == true)
                return;

            //////////////////////////
            // Build Requestor Info //
            //////////////////////////
            hlblEID.InnerText = BusinessLayer.UserEmpID;
            lblEmpName.InnerText = "(" + BusinessLayer.UserEmpID.ToString() + ") " + BusinessLayer.UserFullName;
        }

        catch (Exception exc)
        {
            //Module failed to load 
            string err = exc.Message;
        }
    }
}