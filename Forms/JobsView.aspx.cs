using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_JobsView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "Forms_JobsView.PageLoad";

        if ( !IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        DataListCompletedJobs.DataSource = SqlServer_Impl.GetSiuCompletedJobs();
        DataListCompletedJobs.DataBind();
    }
    
    public string ShowJobHeader(object DataItem)
    {
        //////////////////////
        // Type Cast Record //
        //////////////////////
        SIU_Complete_Job DataRcd = (SIU_Complete_Job) DataItem;

        //////////////////
        // Build Header //
        //////////////////
        string color = ((DataRcd.JobComplete) ? "green" : "orange");
        string Completed= ((DataRcd.JobComplete) ? "Completed" : "INCOMPLETE");
        if (DataRcd.OrigionalScope) color = "red";

        string DivID = DataRcd.JobNo.Replace('.', '_');
        string MoreScopeAnchor = String.Format("<a id=\"{0}_A\" href=\"#\" onclick=\"ShowHide('{1}');\">MORE</a> ", DivID, DivID);
        string Scope = ((DataRcd.OrigionalScope) ? MoreScopeAnchor : "");

        string RcdContentFormat = "<b style='color:{0};'>Job:&nbsp;{1}&nbsp;-&nbsp;{2}&nbsp;&nbsp;{3}</b>";

        string RcdContent = String.Format(RcdContentFormat, color, DataRcd.JobNo,  Completed, Scope);

        return RcdContent;
    }

    public string ShowJobDetails(object DataItem)
    {
        //////////////////////
        // Type Cast Record //
        //////////////////////
        SIU_Complete_Job DataRcd = (SIU_Complete_Job)DataItem;
        if (!DataRcd.OrigionalScope) return "";

        string DivID = DataRcd.JobNo.Replace('.', '_');
        string RcdContent = String.Format("<div id='{0}' style='display:none;' ", DivID);

        RcdContent += "Managers:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + DataRcd.NumMgrs + "<br/>";
        RcdContent += "Material Cost:&nbsp;" + DataRcd.AddMaterial + "<br/>";
        RcdContent += "Travel Cost:&nbsp;&nbsp;&nbsp;&nbsp;" + DataRcd.AddTravel + "<br/>";
        RcdContent += "Lodge Cost:&nbsp;&nbsp;&nbsp;&nbsp;" + DataRcd.AddLodge + "<br/>";
        RcdContent += "Other Cost:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + DataRcd.AddOther + "<br/>";

        RcdContent += "<br/>";
        RcdContent += "Total Hours:&nbsp;&nbsp;&nbsp;" + DataRcd.TotHours + "<br/>";    


        RcdContent += "</div>";

        return RcdContent;
    }
}