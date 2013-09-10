using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_SafetyQoMResponse : System.Web.UI.Page
{
    private const string DATETIME_FORMAT = "MM/dd/yyyy";

    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "Forms_SafetyQoMResponse.PageLoad";

        try
        {
            // If this is the first visit to the page, bind the role data to the datalist 
            if (Page.IsPostBack == false)
            {
                //////////////////////////
                // Build Requestor Info //
                //////////////////////////
                lblEmpName.InnerText = BusinessLayer.UserFullName;
                EmpID.InnerText = BusinessLayer.UserEmpID;

                SIU_Safety_MoQ CurQ = SqlServer_Impl.GetSafetyQomQ(-1);
                if (CurQ == null)
                    Qom.InnerText = "There is not a question defined for today...";
                else
                    Qom.InnerText = CurQ.Question;

                Q_Id.InnerText = CurQ.Q_Id.ToString();


                SIU_Safety_MoQ_Response QomR = SqlServer_Impl.GetSafetyQomR(CurQ.Q_Id, BusinessLayer.UserEmpID);
                Response.Text = QomR.Response;

                if (QomR.ScoreDate != null)
                {
                    LblScore.Text = "Scored: " + QomR.Score.ToString() + " on " + FormatDateTime(QomR.ScoreDate);
                    lblBtnSubmit.Visible = false;
                }
            }
        }

        catch (Exception exc)
        {
            //Module failed to load 
            string err = exc.Message;
        }
    }

    protected void lblBtnSubmit_Click(object sender, EventArgs e)
    {
        ///////////////////////////////////////////
        // Make And Initialize A New Data Object //
        ///////////////////////////////////////////
        SIU_Safety_MoQ_Response Resp = SqlDataMapper<SIU_Safety_MoQ_Response>.MakeNewDAO<SIU_Safety_MoQ_Response>();

        ///////////////////////////////////////////////////////
        // Map Any Data FIlled In On ASP Form To Data Object //
        // Field Names Must Match                            //
        ///////////////////////////////////////////////////////
        Resp = SqlDataMapper<SIU_SafetyPaysReport>.MapAspForm(Resp, Request);
        Resp.ResponseDate = System.DateTime.Now;
        Resp.Emp_ID = BusinessLayer.UserEmpID;
        Resp.Q_Id = int.Parse( Q_Id.InnerText );

        
        SqlServer_Impl.RecordSafetyQomResponse(Resp);
        WebMail.SafetyQomResponseEMail(Resp);


        Page.Response.Redirect("/", true);
    }

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