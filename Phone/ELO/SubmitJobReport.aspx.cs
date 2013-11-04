using System;

public partial class Reporting_SubmitJobReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string method = "Reporting_SubmitJobReport.PageLoad";

        if (!IsPostBack)
        {
            hlblEID.InnerHtml = BusinessLayer.UserEmpID;
            lblEmpName.InnerHtml = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);
            lblEmpName2.InnerHtml = lblEmpName.InnerText;
        }
    }

}
