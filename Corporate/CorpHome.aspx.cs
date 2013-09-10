using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Corporate_CorpHome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "Corporate_CorpHome.PageLoad";

        //////////////////////
        // Inject News Blog //
        //////////////////////
        BlogInsertPoint.Text = Blogs.Create_Injectable_Blog_Advertisements(BlogInsertPoint.Text);

        //////////////////////////////////////////
        // Inject The Directory Popup File List //
        // Attach to Manually Placed Icon       //
        //////////////////////////////////////////
        //PopupMenusDocumentInsertPoint.Text = PopupMenuSupport.Create_Icon_Attached_Injectable_Directory_Popup_Menu(PopupMenusDocumentInsertPoint.Text);


        StringCollection sessionVar = (StringCollection)Session["UserGroups"];
        if (sessionVar != null)
        {
            if (sessionVar.Contains("ShermcoYou_BandC"))
                Sx.InnerHtml = "/Corporate/CorpAdmin.aspx";

            if (sessionVar.Contains("ShermcoYou_Corp"))
                Sx.InnerHtml = "/Corporate/CorpAdmin.aspx";
        }
    }
}