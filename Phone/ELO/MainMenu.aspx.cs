using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ELO_MainMenu : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "ELO_MainMenu.PageLoad";

        if (!IsPostBack)
        {
            lblEmpName.InnerText = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);

            List<string> RptDepts = new List<string>()
                                        {
                                            "6080",
                                            "8010",
                                            "8082",
                                            "8083",
                                            "8084",
                                            "8085",
                                            "8086",
                                            "8087",
                                            "8088",
                                            "8089",
                                            "8090",
                                            "MPUSTEJOVSKY",
                                            "NAVTEST",
                                            "LTRUITT"
                                        };

            ///////////////////////////////////////////////
            // Turn on / off "Submit Report" menu Option //
            ///////////////////////////////////////////////
            SubmitJobDiv.Visible = RptDepts.Contains(SqlServer_Impl.GetEmployeeDeptByNo(BusinessLayer.UserEmpID)) || (RptDepts.Contains(BusinessLayer.UserName.ToUpper()));


            ///////////////////////////////////
            // Turn Off Time View and Delete //
            ///////////////////////////////////
            if (SqlServer_Impl.GetTimeUnapprovedDates(BusinessLayer.UserEmpID).Count == 0)
                TimeMangDiv.Visible = false;

        }
    }

}