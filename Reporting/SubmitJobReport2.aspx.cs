using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reporting_SubmitJobReport2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //////////////////////////////////////////////////////////
        // But Unless This Is A New / Refresh Form, We Are Done //
        //////////////////////////////////////////////////////////
        if (IsPostBack) return;

        hlblEID.InnerText = BusinessLayer.UserEmpID;
        lblEmpName.InnerText = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);
    }
}