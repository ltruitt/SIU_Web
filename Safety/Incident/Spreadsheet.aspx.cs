using System;
using ShermcoYou;


public partial class Safety_Incident_Spreadsheet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btn_SSL_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("Content-Disposition", "attachment; filename=IncidentAccident.xlsx");
        SIU_Spreadsheets.IncidentAccidentGenerate(Response.OutputStream);
        Response.End();

    }


}