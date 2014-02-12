using System;
using System.Collections.Specialized;
using ShermcoYou;


public partial class Safety_SafetyPays_EmpTabPointsRpt : System.Web.UI.Page
{
    DateTime _start = DateTime.Parse("1/1/2013");
    DateTime _end = DateTime.Parse("2/1/2013");

    protected void Page_Load(object sender, EventArgs e)
    {
        //////////////////////////////////////////////////////////
        // But Unless This Is A New / Refresh Form, We Are Done //
        //////////////////////////////////////////////////////////
        if (IsPostBack)
        {
            _start = DateTime.Parse(StartDate.Value);
            _end = DateTime.Parse(EndDate.Value);
            return;
        }

        ////////////////////////////
        // Show The Employee Name //
        ////////////////////////////
        lblEmpName.InnerText = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);

        ///////////////////////////////////////////
        // Bury The Employee ID For AJAX Lookups //
        ///////////////////////////////////////////
        hlblEID.InnerText = BusinessLayer.UserEmpID;
    }

    protected void Consolidated_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("Content-Disposition", "attachment; filename=spPoints" + BusinessLayer.UserEmpID + ".xlsx");
        SIU_Spreadsheets.BuildSpWorkbook(Response.OutputStream, _start, _end, (StringCollection)Session["UserGroups"]);
        Response.End();
    }
}