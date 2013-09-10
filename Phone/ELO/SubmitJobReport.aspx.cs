using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using AutoMapper;
using ShermcoYou.DataTypes;
using System.Reflection;

public partial class Reporting_SubmitJobReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "Reporting_SubmitJobReport.PageLoad";

        if (!IsPostBack)
        {
            hlblEID.InnerText = BusinessLayer.UserEmpID;
            lblEmpName.InnerText = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);
            lblEmpName2.InnerText = lblEmpName.InnerText;
        }
    }

}
