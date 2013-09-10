using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Safety_SafetyPays_SafetyPaysTasks : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "SafetyPaysTasks.PageLoad";

        try
        {
            // If this is the first visit to the page, bind the role data to the datalist 
            if (Page.IsPostBack == true)
                return;

            //////////////////////////
            // Build Requestor Info //
            //////////////////////////
            //lblEmpName.InnerText = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);
            hlblEID.InnerText = BusinessLayer.UserEmpID;
            lblEmpName.InnerText = "(" + BusinessLayer.UserEmpID.ToString() + ") " + BusinessLayer.UserFullName;

            ///////////////////////////
            // Check For Admin Priv. //
            // Set Default Value     //
            // Override In JS        //
            ///////////////////////////
            hlblAdmin.InnerText = "0";
            StringCollection SessionVar = (StringCollection)Session["UserGroups"];
            if (SessionVar != null)
                if (SessionVar.Contains("ShermcoYou_Safety_Pays"))
                    hlblAdmin.InnerText = "1";
        }

        catch (Exception exc)
        {
            //Module failed to load 
            string err = exc.Message;
        }
    }
}