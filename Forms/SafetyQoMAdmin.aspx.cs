using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_SafetyQoMAdmin : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "Forms_SafetyQoMAdmin.PageLoad";

        if (!Page.IsPostBack)
        {
            BindDataView();
        }
    }


#region Gridview Event Handlers
    //////////////////////////////////////////
    // "Edit" Command Called.               //
    // Either Open Detail View In Edit More //
    // Or Allow Edit In Place               //
    //////////////////////////////////////////
    protected void gvSafetyQoMList_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvSafetyQoMList.EditIndex = -1;
        gvSafetyQoMList.SelectedIndex = e.NewEditIndex; ;

        int QNo = (int)gvSafetyQoMList.SelectedDataKey.Values[0];

        //SqlServer_Impl.RecordSafetyPaysTaskComplete(IncNo, TaskNo);

        //Response.Redirect(String.Format("{0}?RptID={1}", Page.Request.Path, Session["SafetyPaysReportId"].ToString()), true);
    }

    ///////////////////////////////////////////////////////////////////
    // This Must Be HEre So That You Can Set SelectedIndex From Code //
    ///////////////////////////////////////////////////////////////////
    protected void gvSafetyQoMList_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void gvSafetyQoMList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSafetyQoMList.PageIndex = e.NewPageIndex;
        BindDataView();
    }
    protected void gvSafetyQoMList_Sorting(object sender, GridViewSortEventArgs e)
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

        gvSafetyQoMList.PageIndex = 0;
        BindDataView();
    }

    protected void NewBtn_OnClick(object sender, EventArgs e)
    {
        ///////////////////////////////////////////
        // Make And Initialize A New Data Object //
        ///////////////////////////////////////////
        SIU_Safety_MoQ newQ = SqlDataMapper<SIU_Safety_MoQ>.MakeNewDAO<SIU_Safety_MoQ>();

        ///////////////////////////////////////////////////////
        // Map Any Data FIlled In On ASP Form To Data Object //
        // Field Names Must Match                            //
        ///////////////////////////////////////////////////////
        newQ = SqlDataMapper<SIU_Safety_MoQ>.MapAspForm(newQ, Request);

        ////////////////////////////////////
        //// Write New Record To Database //
        ////////////////////////////////////
        if (SqlServer_Impl.CheckSafetyQomDateRange(newQ.StartDate, newQ.EndDate) > 0)
        {
            LblOverLappingDates.Text = "Can Not Add Overlapping Dates";
        }
        else
        {
            int NewQNo = SqlServer_Impl.RecordSafetyQomQuestion(newQ);
            Response.Redirect(Page.Request.Path, true);
        }
    }
#endregion

#region Data Binding / Data Bound Events
    protected void BindDataView()
    {
        ////////////////////////////////////////////////////////////////
        //// Get List Of Qualification Codes Based On Search Criteria //
        ////////////////////////////////////////////////////////////////
        var QomTaskList = SqlServer_Impl.GetSafetyQomQList();

        //////////////////////////////////////
        // If There Is At Least 1 Question  //
        //////////////////////////////////////
        if (QomTaskList.Count() > 0)
            gvSafetyQoMList.DataSource = QomTaskList;

        ////////////////////////////////////////////////
        // No Questions Defined, Make A Dummy Dataset //
        ////////////////////////////////////////////////
        else
        {
            IEnumerable<SIU_Safety_MoQ> item = new SIU_Safety_MoQ[] { new SIU_Safety_MoQ() };
            gvSafetyQoMList.DataSource = item;
        }

        ////////////////////
        // Bind Data Grid //
        ////////////////////
        gvSafetyQoMList.DataKeyNames = new string[] { "Q_Id" };
        gvSafetyQoMList.DataBind();

    }
    protected void gvSafetyQoMList_RowDataBound(object sender, GridViewRowEventArgs e)
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