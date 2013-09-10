using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_LibHome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "Library_LibHome.PageLoad";

        //////////////////////////////////////////////////////////
        // But Unless This Is A New / Refresh Form, We Are Done //
        //////////////////////////////////////////////////////////
        if (IsPostBack) return;

        //////////////////////////////////////////
        // Inject The Directory Popup File List //
        // Attach to Manually Placed Icon       //
        //////////////////////////////////////////
        PopupMenusDocumentInsertPoint.Text = PopupMenuSupport.Create_Icon_Attached_Injectable_Directory_Popup_Menu(PopupMenusDocumentInsertPoint.Text);

        //////////////////////
        // Inject News Blog //
        //////////////////////
        //BlogInsertPoint.Text = Blogs.Create_Injectable_Blog_Advertisements(BlogInsertPoint.Text);
    }
}