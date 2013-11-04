using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SiteMaster2 : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( Request.IsAuthenticated )
        {
            //LitUserFullName.Text = "Welcome... " + SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);
            LitUserFullName.Text = "Welcome... " + BusinessLayer.UserFullName;
        }
        else
        {
            LitUserFullName.Text = "Please Logon...";
        }

        if (!SqlServer_Impl.isProdDB)
        {
            MasterHeaderTxt.InnerText = "DEVELOPMENT...";
            MasterHeaderArea.Attributes["Class"] = "TitleRectangle Development";
        }
    }
}
