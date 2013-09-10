using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Jobs_JobTime : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "JobTime.PageLoad";

        //////////////////////////////////////////////////////////
        // But Unless This Is A New / Refresh Form, We Are Done //
        //////////////////////////////////////////////////////////
        if (IsPostBack) return;

        ///////////////////////////
        // Get The Employee Name //
        ///////////////////////////
        lblEmpName.InnerText = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);
    }
}