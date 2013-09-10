using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_SafetyPaysAdmin : System.Web.UI.Page
{
    protected ViewMode m_ViewMode = ViewMode.GridView;
    protected enum ViewMode { Unknown, GridView, DetailsView, DetailsEdit }


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
    private Label GetGridViewLabelControl(GridViewRow row, string CtlName)
    {
        Label _ctl = (Label)row.FindControl(CtlName);
        if (_ctl == null)
        {
            throw new Exception("Get Grid View Control Field: Could not find control key control");
        }

        return _ctl;
    }
    private T GetDetailViewControl<T>(DetailsViewRow row, string CtlName) where T : new()
    {
        T _ctl = (T)(object)row.FindControl(CtlName);
        if (_ctl == null)
        {
            //throw new Exception("Get Grid View Control Field: Could not find control");
            return default(T);
        }

        return _ctl;
    }
#endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "Forms_SafetyPaysAdmin.PageLoad";

        if (Request["RptID"] != null)
        {
            int RptID = int.Parse(  Request["RptID"].Trim()  );

            pnlGridView.Visible = false;

            BindDetailView(RptID);
            SetBehavior(ViewMode.DetailsView);
        }
        else
        {
            BindDataViews();    
        }
    }


    protected void SetBehavior()
    {
        bool bHasGridRows = (gvSafetyPaysAdmin.Rows.Count > 0);
        //if ((bHasGridRows == false) && (AllowRecordInserting == true))
        //{
        //    m_ViewMode = ViewMode.DetailsView;
        //}

        switch (m_ViewMode)
        {
            case ViewMode.Unknown:
                pnlGridView.Visible = bHasGridRows;
                pnlDetailsView.Visible = !bHasGridRows;
                break;

            case ViewMode.GridView:
                pnlGridView.Visible = true;
                pnlDetailsView.Visible = false;
                break;

            case ViewMode.DetailsView:
                pnlGridView.Visible = false;
                pnlDetailsView.Visible = true;
                gvSafetyPaysAdmin.EditIndex = -1;
                break;

            case ViewMode.DetailsEdit:
                pnlGridView.Visible = false;
                pnlDetailsView.Visible = true;
                gvSafetyPaysAdmin.EditIndex = -1;
                dvSafetyPaysDtl.ChangeMode(DetailsViewMode.Edit);
                break;
        }
    }
    protected void SetBehavior(ViewMode vmNewViewMode)
    {
        m_ViewMode = vmNewViewMode;
        SetBehavior();
    }



    protected void btnSearch_Click(object sender, EventArgs e)
    {

    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        string Method = "Forms_SafetyPaysAdmin.btnReject_Click";

        int RcdId = (int)dvSafetyPaysDtl.DataKey.Value;
        int RcdId2 = (int)gvSafetyPaysAdmin.SelectedDataKey.Value;

        string RejectNotes = this.RejectReason.InnerText;

        SIU_SafetyPaysReport UpdRcd = SqlServer_Impl.RecordSafetyPaysStatus(RcdId, "Reject", (string)Session["UserEmpID"], true, 0, RejectNotes);
        WebMail.SafetyPaysRejectedEMail(UpdRcd, BusinessLayer.UserEmail, BusinessLayer.UserFullName);

        Response.Redirect(Request.Path);
    }


#region Gridview Event Handlers
    //////////////////////////////////////////
    // "Edit" Command Called.               //
    // Either Open Detail View In Edit More //
    // Or Allow Edit In Place               //
    //////////////////////////////////////////
    protected void gvSafetyPaysAdmin_RowEditing(object sender, GridViewEditEventArgs e)
    {       
        gvSafetyPaysAdmin.EditIndex = -1;
        gvSafetyPaysAdmin.SelectedIndex = e.NewEditIndex;

        GridViewRow row = gvSafetyPaysAdmin.Rows[e.NewEditIndex];
        Label _lblRptStatus = (Label)row.FindControl("lblStatus");

        if ( _lblRptStatus.Text.Contains("Tasks") )
        {
            Response.Redirect("/Forms/SafetyPaysTaskList.aspx?RptId=" + gvSafetyPaysAdmin.SelectedDataKey.Value, true);
            return;
        }

        int RcdId = (int) gvSafetyPaysAdmin.SelectedDataKey.Value;
        BindDetailView(RcdId);

        SetBehavior(ViewMode.DetailsView);
        e.Cancel = true;
    }

    ///////////////////////////////////////////////////////////////////
    // This Must Be HEre So That You Can Set SelectedIndex From Code //
    ///////////////////////////////////////////////////////////////////
    protected void gvSafetyPaysAdmin_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void gvSafetyPaysAdmin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSafetyPaysAdmin.PageIndex = e.NewPageIndex;
        BindDataViews();
    }
    protected void gvSafetyPaysAdmin_Sorting(object sender, GridViewSortEventArgs e)
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

        gvSafetyPaysAdmin.PageIndex = 0;
        BindDataViews();
    }
#endregion



#region Details View Events
    protected void dvSafetyPaysDtl_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        
    }
    protected void dvSafetyPaysDtl_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
        int RcdID = (int)((DetailsView)dvSafetyPaysDtl).DataKey.Value;

        if (e.CommandName.ToLower() == "close")
        {
            SIU_SafetyPaysReport UpdRcd = SqlServer_Impl.RecordSafetyPaysStatus(RcdID, "Closed", (string)Session["UserEmpID"], true, 1);
            WebMail.SafetyPaysClosedEMail(UpdRcd, BusinessLayer.UserEmail, BusinessLayer.UserFullName);
        }

        if (e.CommandName.ToLower() == "work")
        {
            SIU_SafetyPaysReport UpdRcd = SqlServer_Impl.RecordSafetyPaysStatus(RcdID, "Working", (string)Session["UserEmpID"], false, 1);
            WebMail.SafetyPaysWorkingEMail(UpdRcd, BusinessLayer.UserEmail, BusinessLayer.UserFullName);
            Response.Redirect("~/Forms/SafetyPaysTaskList.aspx?RptId=" + UpdRcd.IncidentNo, true);
        }

        BindDataViews();
    }
#endregion




#region Data Binding / Data Bound Events
    protected void BindDataViews()
    {
        BindDataViews(ddSearchStatus.Text, ddSearchType.Text);
    }
    protected void BindDataViews(string _Status, string _Type)
    {
        ////////////////////////////////////////////////////////////////
        //// Get List Of Qualification Codes Based On Search Criteria //
        ////////////////////////////////////////////////////////////////
        var SpListList = SqlServer_Impl.GetSafetyPaysAdmin();

        gvSafetyPaysAdmin.DataSource = SpListList;
        gvSafetyPaysAdmin.DataKeyNames = new string[] {"IncidentNo"};
        gvSafetyPaysAdmin.DataBind();
        SetBehavior();
    }
    protected void BindDetailView(int _RcdID)
    {
        ////////////////////////////////////////////////////////////////
        //// Get List Of Qualification Codes Based On Search Criteria //
        ////////////////////////////////////////////////////////////////
        var SpItem = SqlServer_Impl.GetSafetyPaysReport( _RcdID );

        dvSafetyPaysDtl.DataSource = SpItem;
        dvSafetyPaysDtl.DataKeyNames = new string[] { "IncidentNo" };
        dvSafetyPaysDtl.DataBind();

        IncidentReportDetailHeader.InnerText = "Incident Report: " + _RcdID.ToString();
        SetBehavior();

    }

    protected void gvSafetyPaysAdmin_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ///////////////////////////////////
        // Ignore Header And Footer Rows //
        ///////////////////////////////////
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        ////////////////////////////////////////////
        // Replace Employee No With Employee Name //
        ////////////////////////////////////////////
        int EmpId = int.Parse(((SIU_SafetyPaysReport)e.Row.DataItem).EmpID);
        Label _lblEmpName = (Label)GetGridViewLabelControl(e.Row, "lblSubmittedBy");

        Shermco_Employee Emp = SqlServer_Impl.GetEmployeeByNo(EmpId.ToString());
        _lblEmpName.Text = Emp.Last_Name + ", " + Emp.First_Name;


        /////////////////////////////////////////////////////////////////////
        // For Reports That Are Being Worked, Show How Many Tasks Are Open //
        /////////////////////////////////////////////////////////////////////
        Label _lblWorkingCount = (Label)GetGridViewLabelControl(e.Row, "lblStatus");
        if (_lblWorkingCount.Text.ToLower() == "working")
        {
            int RcdId = ((SIU_SafetyPaysReport)e.Row.DataItem).IncidentNo;
            string Status = ((SIU_SafetyPaysReport)e.Row.DataItem).IncStatus;
            int WorkingCount = 0; // SqlServer_Impl.GetSafetyPaysOpenTaskCount(RcdId);
            _lblWorkingCount.Text = WorkingCount.ToString() + " Tasks";
        }


    }
    protected void dvSafetyPaysDtl_DataBound(object sender, EventArgs e)
    {
        if (dvSafetyPaysDtl.DataItem == null)
            return;

        ///////////////////////////////////////////
        // Show Employee Names In Lieu Of Emp ID //
        ///////////////////////////////////////////
        SIU_SafetyPaysReport rcd = (SIU_SafetyPaysReport)((DetailsView)sender).DataItem;
         
        Label _txtModEmp = (Label)dvSafetyPaysDtl.FindControl("lblLastModifyEmployee");
        Label _txtRptEmp = (Label)dvSafetyPaysDtl.FindControl("lblEmpFullName");

        Shermco_Employee Emp = (Shermco_Employee)SqlServer_Impl.GetEmployeeByNo(rcd.EmpID);
        _txtRptEmp.Text = Emp.Last_Name + ", " + Emp.First_Name;
        
        Emp = (Shermco_Employee)SqlServer_Impl.GetEmployeeByNo(rcd.IncLastTouchEmpID);
        if ( Emp == null )
            _txtModEmp.Text = "Unknown";
        else
            _txtModEmp.Text = Emp.Last_Name + ", " + Emp.First_Name;



    }

#endregion






}