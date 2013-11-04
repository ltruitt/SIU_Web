using System;
using System.Collections.Generic;

public partial class ELO_MainMenu : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string method = "ELO_MainMenu.PageLoad";

        if (!IsPostBack)
        {
            lblEmpName.InnerText = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);

            List<string> rptDepts = new List<string>
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
                                            "LTRUITT",
                                            "AJONES"
                                        };

            ///////////////////////////////////////////////
            // Turn on / off "Submit Report" menu Option //
            ///////////////////////////////////////////////
            SubmitJobDiv.Visible = rptDepts.Contains(SqlServer_Impl.GetEmployeeDeptByNo(BusinessLayer.UserEmpID)) || (rptDepts.Contains(BusinessLayer.UserName.ToUpper()));


            ///////////////////////////////////
            // Turn Off Time View and Delete //
            ///////////////////////////////////
            if (SqlServer_Impl.GetTimeUnapprovedDates(BusinessLayer.UserEmpID).Count == 0)
                TimeMangDiv.Visible = false;

        }
    }

}