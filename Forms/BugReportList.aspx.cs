using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_BugReportList : System.Web.UI.Page
{
    private const string DATETIME_FORMAT = "MM/dd/yyyy";
    private char CmdBtn;
    
#region Public Properties
    private string SortDirection
    {
        get
        {
            if (ViewState["SortDirection"] != null)
                return (string)ViewState["SortDirection"];
            else
                return "ASC";
        }
        set
        {
            if (ViewState["SortDirection"] == null)
            {
                ViewState.Add("SortDirection", value);
            }
            else
            {
                ViewState["SortDirection"] = value;
            }
        }
    }
    private string SortExpression
    {
        get
        {
            if (ViewState["SortExpression"] != null)
                return (string)ViewState["SortExpression"];
            else
                return string.Empty;
        }
        set
        {
            if (ViewState["SortExpression"] == null)
            {
                ViewState.Add("SortExpression", value);
            }
            else
            {
                ViewState["SortExpression"] = value;
            }
        }
    }
#endregion

#region GridView Helper Method
    private T GetGridViewLabelControl<T>(GridViewRow row, string CtlName) where T : new()
    {
        T _ctl = (T)(object)row.FindControl(CtlName);
        if (_ctl == null)
        {
            throw new Exception("Get Grid View Control Field: Could not find control");
        }

        return _ctl;
    }
#endregion

    protected void Page_Load(object sender, EventArgs e)
    {

    }

#region Gridview Event Handlers
    //////////////////////////////////////////
    // "Edit" Command Called.               //
    // Either Open Detail View In Edit More //
    // Or Allow Edit In Place               //
    //////////////////////////////////////////
    protected void gvBugReportAdmin_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvBugReportAdmin.EditIndex = -1;
        gvBugReportAdmin.SelectedIndex = e.NewEditIndex; ;

        int IncNo = (int)gvBugReportAdmin.SelectedDataKey.Values[0];

        WebMail.BugReportSendStatusEmail( SqlServer_Impl.RecordBugStatus(CmdBtn, IncNo), CmdBtn );
        Response.Redirect(Page.Request.Path);
    }

    ///////////////////////////////////////////////////////////////////
    // This Must Be HEre So That You Can Set SelectedIndex From Code //
    ///////////////////////////////////////////////////////////////////
    protected void gvBugReportAdmin_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void gvBugReportAdmin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBugReportAdmin.PageIndex = e.NewPageIndex;
    }
    protected void gvBugReportAdmin_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (SortExpression != e.SortExpression)
        {
            SortExpression = e.SortExpression;
            SortDirection = "ASC";
        }
        else
        {
            if (SortDirection == "ASC")
            {
                SortDirection = "DESC";
            }
            else
            {
                SortDirection = "ASC";
            }
        }

        gvBugReportAdmin.PageIndex = 0;
    }

    protected void lblBtnAccept_Click(object sender, EventArgs e)
    {
        CmdBtn = 'A';
    }
    protected void lblBtnWork_Click(object sender, EventArgs e)
    {
        CmdBtn = 'W';
    }
    protected void lblBtnReject_Click(object sender, EventArgs e)
    {
        CmdBtn = 'R';
    }
    protected void lblBtnTest_Click(object sender, EventArgs e)
    {
        CmdBtn = 'T';
    }
#endregion

#region Data Binding / Data Bound Events
    protected void gvBugReportAdmin_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ///////////////////////////////////
        // Ignore Header And Footer Rows //
        ///////////////////////////////////
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        //////////////////////////////////////////////
        // Show A Condensed Version Of The Due Date //
        //////////////////////////////////////////////
        Label _lblOpenDate = (Label)GetGridViewLabelControl<Label>(e.Row, "lblOpenDate");
        _lblOpenDate.Text = "<b>O</b> " + FormatDateTime(((SIU_TaskList)e.Row.DataItem).OpenTimeStamp);

        if (((SIU_TaskList)e.Row.DataItem).AcceptTimeStamp != null)
            _lblOpenDate.Text += "<br/>" + "<b>A</b> " + FormatDateTime(((SIU_TaskList)e.Row.DataItem).OpenTimeStamp);

        if (((SIU_TaskList)e.Row.DataItem).WorkTimeStamp != null)
            _lblOpenDate.Text += "<br/>" + "<b>W</b> " + FormatDateTime(((SIU_TaskList)e.Row.DataItem).WorkTimeStamp);

        if (((SIU_TaskList)e.Row.DataItem).TestingTimeStamp != null)
            _lblOpenDate.Text += "<br/>" + "<b>T</b> " + FormatDateTime(((SIU_TaskList)e.Row.DataItem).TestingTimeStamp);

        if (((SIU_TaskList)e.Row.DataItem).TestRejectionTimeStamp != null)
            _lblOpenDate.Text += "<br/>" + "<b>R</b> " + FormatDateTime(((SIU_TaskList)e.Row.DataItem).TestRejectionTimeStamp);

        if (((SIU_TaskList)e.Row.DataItem).CloseTimeStamp != null)
            _lblOpenDate.Text += "<br/>" + "<b>C</b> " + FormatDateTime(((SIU_TaskList)e.Row.DataItem).CloseTimeStamp);


        /////////////////////////////////////////////////////
        // Show A Condensed Version Of THe Completion Date //
        /////////////////////////////////////////////////////
        Label _lblStatus = (Label)GetGridViewLabelControl<Label>(e.Row, "lblStatus");
        LinkButton _BtnMarkTest = (LinkButton)GetGridViewLabelControl<LinkButton>(e.Row, "LinkTest");
        LinkButton _BtnMarkWork = (LinkButton)GetGridViewLabelControl<LinkButton>(e.Row, "LinkWork");
        LinkButton _BtnMarkReject = (LinkButton)GetGridViewLabelControl<LinkButton>(e.Row, "LinkReject");
        LinkButton _BtnMarkAccept = (LinkButton)GetGridViewLabelControl<LinkButton>(e.Row, "LinkAccept");


        if (((SIU_TaskList)e.Row.DataItem).AcceptTimeStamp == null && ((SIU_TaskList)e.Row.DataItem).CloseTimeStamp == null)
        {
            _lblStatus.Text = "Open";
            _BtnMarkWork.Visible = false;
            _BtnMarkTest.Visible = false;
            _BtnMarkReject.Visible = true;
            _BtnMarkAccept.Visible = true;
        }


        if (((SIU_TaskList)e.Row.DataItem).AcceptTimeStamp != null)
        {
            _lblStatus.Text = "Accepted";
            _BtnMarkTest.Visible = false;
            _BtnMarkReject.Visible = false;
            _BtnMarkAccept.Visible = false;
            _BtnMarkWork.Visible = true;
        }


        if (((SIU_TaskList)e.Row.DataItem).WorkTimeStamp != null)
        {
            _lblStatus.Text = "Working";
            _BtnMarkReject.Visible = false;
            _BtnMarkAccept.Visible = false;
            _BtnMarkWork.Visible = false;
            _BtnMarkTest.Visible = true;
        }

        if (((SIU_TaskList)e.Row.DataItem).TestingTimeStamp != null)
        {
            _lblStatus.Text = "Testing";
            if ((string)Session["UserUser"].ToString().ToLower() == "ltruitt")
            {
                int RcdID = ((SIU_TaskList)e.Row.DataItem).IncidentNo;
                string href = @"http://" + Server.MachineName + @"/Forms/BugReport.aspx" + "?RptID=" + RcdID;
                _lblStatus.Text = "<a href=" + href + ">Testing</a>";
            }
            _BtnMarkReject.Visible = false;
            _BtnMarkAccept.Visible = false;
            _BtnMarkWork.Visible = false;
            _BtnMarkTest.Visible = false;
        }

        if (((SIU_TaskList)e.Row.DataItem).TestRejectionTimeStamp != null)
        {
            _lblStatus.Text = "Failed";
            _BtnMarkReject.Visible = false;
            _BtnMarkAccept.Visible = false;
            _BtnMarkWork.Visible = false;
            _BtnMarkTest.Visible = true;
        }


        if (((SIU_TaskList)e.Row.DataItem).CloseTimeStamp != null)
        {
            if (((SIU_TaskList)e.Row.DataItem).AcceptTimeStamp != null)
                _lblStatus.Text = "Closed";

            else
                _lblStatus.Text = "Rejected";

            _BtnMarkReject.Visible = false;
            _BtnMarkAccept.Visible = false;
            _BtnMarkWork.Visible = false;
            _BtnMarkTest.Visible = false;
        }

        if ( (string)Session["UserUser"].ToString().ToLower() != "ltruitt" )
        {
            _BtnMarkReject.Visible = false;
            _BtnMarkAccept.Visible = false;
            _BtnMarkWork.Visible = false;
            _BtnMarkTest.Visible = false;            
        }
    }
#endregion

    public string FormatDateTime(object dtvalue)
    {
        string sDateTime = Convert.ToString(dtvalue);

        if (IsDateTime(sDateTime) == true)
        {
            System.DateTime dt = System.DateTime.Parse(sDateTime);
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