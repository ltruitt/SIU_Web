using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_SafetyQomAnsAdmin : System.Web.UI.Page
{
    private const string DATETIME_FORMAT = "MM/dd/yyyy";


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

    protected void Page_Load(object sender, EventArgs e) { }

#region Gridview Event Handlers
    //////////////////////////////////////////
    // "Edit" Command Called.               //
    // Either Open Detail View In Edit More //
    // Or Allow Edit In Place               //
    //////////////////////////////////////////
    protected void gvSafetyQomAnsAdminSelect_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvSafetyQomAnsAdminSelect.EditIndex = -1;
        gvSafetyQomAnsAdminSelect.SelectedIndex = e.NewEditIndex; ;

        int QNo = (int)gvSafetyQomAnsAdminSelect.SelectedDataKey.Values[0];

        //SqlServer_Impl.RecordSafetyPaysTaskComplete(IncNo, TaskNo);

        //Response.Redirect(String.Format("{0}?RptID={1}", Page.Request.Path, Session["SafetyPaysReportId"].ToString()), true);
    }
    protected void gvSafetyQomAnsAdmin_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvSafetyQomAnsAdmin.EditIndex = -1;
        gvSafetyQomAnsAdmin.SelectedIndex = e.NewEditIndex; ;

        int QNo = (int)gvSafetyQomAnsAdmin.SelectedDataKey.Values[0];
        int QAns = (int)gvSafetyQomAnsAdmin.SelectedDataKey.Values[1];

        GridViewRow row = gvSafetyQomAnsAdmin.SelectedRow;
        string _NewScore =  ((TextBox) gvSafetyQomAnsAdmin.Rows[e.NewEditIndex].Cells[4].FindControl("NewScore")).Text;

        if ( _NewScore.Length > 0 )
            WebMail.SafetyQomScoreEMail(SqlServer_Impl.RecordSafetyQomScore(QNo, QAns, _NewScore, BusinessLayer.UserEmpID));

        gvSafetyQomAnsAdmin.DataBind();
    }

    ///////////////////////////////////////////////////////////////////
    // This Must Be Here So That You Can Set SelectedIndex From Code //
    ///////////////////////////////////////////////////////////////////
    protected void gvSafetyQomAnsAdminSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void gvSafetyQomAnsAdmin_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void gvSafetyQomAnsAdminSelect_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSafetyQomAnsAdminSelect.PageIndex = e.NewPageIndex;
        //BindDataView();
    }
    protected void gvSafetyQomAnsAdmin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSafetyQomAnsAdmin.PageIndex = e.NewPageIndex;
        //BindDataView();
    }

    protected void gvSafetyQomAnsAdminSelect_Sorting(object sender, GridViewSortEventArgs e)
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

        gvSafetyQomAnsAdminSelect.PageIndex = 0;
        //BindDataView();
    }
    protected void gvSafetyQomAnsAdmin_Sorting(object sender, GridViewSortEventArgs e)
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

        gvSafetyQomAnsAdmin.PageIndex = 0;
        //BindDataView();
    }

#endregion

#region Bata Bound Events
    protected void gvSafetyQomAnsAdminSelect_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ///////////////////////////////////
        // Ignore Header And Footer Rows //
        ///////////////////////////////////
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        ////////////////////////////////////////////////
        // Show A Condensed Version Of The Start Date //
        ////////////////////////////////////////////////
        Label _lblStartDate = (Label)GetGridViewLabelControl<Label>(e.Row, "lblStartDate");
        _lblStartDate.Text = FormatDateTime(((SIU_Safety_MoQ)e.Row.DataItem).StartDate);


        /////////////////////////////////////////////////////
        // Show A Condensed Version Of The Completion Date //
        /////////////////////////////////////////////////////
        Label _lblCompletedDate = (Label)GetGridViewLabelControl<Label>(e.Row, "lblEndDate");
        _lblCompletedDate.Text = FormatDateTime(((SIU_Safety_MoQ)e.Row.DataItem).EndDate);

    }
    protected void gvSafetyQomAnsAdmin_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ///////////////////////////////////
        // Ignore Header And Footer Rows //
        ///////////////////////////////////
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        ///////////////////////////////////////////
        // Show The Employee Submitting Response //
        ///////////////////////////////////////////
        Label _lblEmpName = (Label)GetGridViewLabelControl<Label>(e.Row, "lblEmpName");
        _lblEmpName.Text = SqlServer_Impl.GetEmployeeNameByNo(  ((SIU_Safety_MoQ_Response)e.Row.DataItem).Emp_ID);


        /////////////////////////////////////////////////////
        // Show A Condensed Version Of The Completion Date //
        /////////////////////////////////////////////////////
        Label _lblResponseDate = (Label)GetGridViewLabelControl<Label>(e.Row, "lblResponseDate");
        _lblResponseDate.Text = FormatDateTime(((SIU_Safety_MoQ_Response)e.Row.DataItem).ResponseDate);

        if ( ((SIU_Safety_MoQ_Response)e.Row.DataItem).ScoreDate != null )
        {
            ((LinkButton)GetGridViewLabelControl<LinkButton>(e.Row, "BtnScore")).Visible = false;
            ((TextBox)GetGridViewLabelControl<TextBox>(e.Row, "NewScore")).Visible = false;
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