using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web.UI.WebControls;

using Microsoft.Reporting.WebForms;

public partial class My_MyPayStubs : System.Web.UI.Page
{
    private const string DATETIME_FORMAT = "MM/dd/yyyy";

    private int _rowIdx;

#region GridView Helper Method
    private T GetGridViewControl<T>(GridViewRow row, string CtlName) where T : new()
    {
        T ctl = (T)(object)row.FindControl(CtlName);
        if (ctl == null)
        {
            throw new Exception("Get Grid View Control Field: Could not find control");
        }

        return ctl;
    }
#endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindDataView();
        }

    }


    protected void Gv1PasswordEntered(object sender, EventArgs e)
    {
        int rowId =  int.Parse(   ((TextBox) sender).Attributes["Row"]   );
        GridViewRow gr = gvPayStubs.Rows[rowId];

        string postingDate =  GetGridViewControl<Label>(gr, "lblPostingDate").Text;
        string txtRunPwd = ((TextBox)sender).Text;

        ShowReport(txtRunPwd, postingDate);
    }



    protected void Gv2PasswordEntered(object sender, EventArgs e)
    {
        int rowId = int.Parse(((TextBox)sender).Attributes["Row"]);
        GridViewRow gr = gvPayStubs2.Rows[rowId];

        string postingDate = GetGridViewControl<Label>(gr, "lblPostingDate").Text;
        string txtRunPwd = ((TextBox)sender).Text;

        ShowReport(txtRunPwd, postingDate);
    }

    private void ShowReport(string Pwd, string PostDate)
    {
        try
        {
            MyReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;

            MyReportViewer.ServerReport.ReportServerUrl = new Uri("https://reports.shermco.com/reportserver"); // Report Server URL 
            MyReportViewer.ServerReport.ReportPath = "/IT/PPSS"; // Report Name 
            MyReportViewer.ShowParameterPrompts = true;
            MyReportViewer.ShowParameterPrompts = false;
            MyReportViewer.ShowPrintButton = true;
            MyReportViewer.ShowExportControls = false;
            MyReportViewer.ShowRefreshButton = false;
            MyReportViewer.ShowPageNavigationControls = false;
            MyReportViewer.ShowFindControls = false;
            MyReportViewer.ShowZoomControl = false;
            MyReportViewer.SizeToReportContent = true;

            //if (Environment.MachineName.ToLower() == "tsdc-dev")
            //    MyReportViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials("ltruitt", Pwd, "Shermco");
            //else
                MyReportViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials(BusinessLayer.UserName, Pwd, "Shermco");
        

            // Below code demonstrate the Parameter passing method. User only if you have parameters into the reports.  
            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[2];

            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter();
            reportParameterCollection[0].Name = "EmployeeCode";
            //reportParameterCollection[0].Values.Add("1075");
            reportParameterCollection[0].Values.Add(BusinessLayer.UserEmpID);
        

            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter();
            reportParameterCollection[1].Name = "PostingDate";
            reportParameterCollection[1].Values.Add(PostDate);

            MyReportViewer.ServerReport.SetParameters(reportParameterCollection);

            MyReportViewer.ServerReport.Refresh();
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("MyPayStub.ShowReport", PostDate);
            SqlServer_Impl.LogDebug("MyPayStub.ShowReport", ex.Message);
            MyReportViewer.Reset();
        }
    }

    protected void ClearRpt_Clicked(object sender, EventArgs e)
    {
        MyReportViewer.Reset();
    }


#region Data Binding / Data Bound Events
    protected void BindDataView()
    {
        ////////////////////////////////////////////////////////////////
        //// Get List Of Qualification Codes Based On Search Criteria //
        ////////////////////////////////////////////////////////////////
        List<DateTime> postDates = SqlServer_Impl.GetPayrollDates(BusinessLayer.UserEmpID);
        //List<DateTime> PostDates = SqlServer_Impl.GetPayrollDates("1075");

        ///////////////////////////////////////
        // Split The List Into 2 Equal Parts //
        ///////////////////////////////////////
        List<DateTime> postDates1 = (from p1 in postDates
                                     select p1
                                    ).Take(postDates.Count/2).ToList();

        List<DateTime> postDates2 = (from p1 in postDates
                                     select p1
                                    ).Skip(postDates1.Count).ToList();


        //////////////////////////////////////////
        // Assign List Part 1 To Left Side Grid //
        //////////////////////////////////////////
        _rowIdx = 0;
        if (postDates1.Any())
            gvPayStubs.DataSource = postDates1;
        else
        {
            IEnumerable<Shermco_Payroll_Ledger_Entry> item = new[] { new Shermco_Payroll_Ledger_Entry() };
            gvPayStubs.DataSource = item;
        }
        gvPayStubs.DataBind();

        //////////////////////////////////////
        // Assign List Part 2 To Right Side //
        //////////////////////////////////////
        _rowIdx = 0;
        if (postDates2.Any())
            gvPayStubs2.DataSource = postDates2;
        else
        {
            IEnumerable<Shermco_Payroll_Ledger_Entry> item = new[] { new Shermco_Payroll_Ledger_Entry() };
            gvPayStubs2.DataSource = item;
        }
        gvPayStubs2.DataBind();


    }

    protected void gvPayStubs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ///////////////////////////////////
        // Ignore Header And Footer Rows //
        ///////////////////////////////////
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;


        //////////////////////////////////////////////
        // Show A Condensed Version Of The Due Date //
        //////////////////////////////////////////////
        Label lblPostingDate = GetGridViewControl<Label>(e.Row, "lblPostingDate");
        try
        {
            lblPostingDate.Text = FormatDateTime(((DateTime)e.Row.DataItem));
        }
        catch (Exception)
        {
            if (lblPostingDate.Text == "01/01/0001")
            {
                lblPostingDate.Text = "No Records";
                return;
            }
        }
        



        TextBox txtPwd = GetGridViewControl<TextBox>(e.Row, "txtRunPwd");
        txtPwd.Attributes.Add("Row", _rowIdx.ToString(CultureInfo.InvariantCulture));
        _rowIdx++;

    }

    protected void gvPayStubs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPayStubs.PageIndex = e.NewPageIndex;
    }
#endregion


    public string FormatDateTime(object dtvalue)
    {
        string sDateTime = Convert.ToString(dtvalue);

        if (IsDateTime(sDateTime))
        {
            DateTime dt = DateTime.Parse(sDateTime);
            if (dt == new DateTime(1900, 1, 1))
                sDateTime = string.Empty;
            else
                sDateTime = dt.ToString(DATETIME_FORMAT);
        }

        return sDateTime;
    }
    public static bool IsDateTime(object value)
    {
        try
        {
            if (value == null)
                return false;

            DateTime dateTime = DateTime.Parse(value.ToString());
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

}

[Serializable] 
public class ReportServerCredentials : IReportServerCredentials
{
    private string _reportServerUserName;
    private string _reportServerPassword;
    private string _reportServerDomain;

    public ReportServerCredentials(string userName, string password, string domain)
    {
        _reportServerUserName = userName;
        _reportServerPassword = password;
        _reportServerDomain = domain;
    }

    public WindowsIdentity ImpersonationUser
    {
        get
        {
            // Use default identity.
            return null;
        }
    }

    public ICredentials NetworkCredentials
    {
        get
        {
            // Use default identity.
            return new NetworkCredential(_reportServerUserName, _reportServerPassword, _reportServerDomain);
        }
    }

    public void New(string userName, string password, string domain)
    {
        _reportServerUserName = userName;
        _reportServerPassword = password;
        _reportServerDomain = domain;
    }

    public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
    {
        // Do not use forms credentials to authenticate.
        authCookie = null;
        user = null;
        password = null;
        authority = null;

        return false;
    }
}