using System;
using System.Collections.Specialized;


public partial class Safety_SafetyHome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string method = "Safety_SafetyHome.PageLoad";

        if (!IsPostBack)
        {
            ///////////////////////////
            // Check For Admin Priv. //
            ///////////////////////////
            StringCollection sessionVar = (StringCollection)Session["UserGroups"];
            if (sessionVar != null)
                if (sessionVar.Contains("ShermcoYou_Safety_Pays"))
                    Sx.InnerHtml = "/Safety/SafetyAdminHome.aspx";

            ////////////////////////////////
            // Show Question Of The Month //
            ////////////////////////////////
            //SIU_Safety_MoQ Q = SqlServer_Impl.GetSafetyQomQ(-1);
            //if (Q != null)
            //    QomMarquee.InnerHtml = "<b>Question Of The Month: </b>" + Q.Question;
            //else
            //    QomMarquee.Visible = false;
        }
            
    }
}