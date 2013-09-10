using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HomePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string Method = "HomePage.PageLoad";

        if (!IsPostBack)
        {
            //ContentUpd Updater = new ContentUpd();
            //Updater.ContentScan("/Files");

            //////////////////////////////////////////
            // Inject The Directory Popup File List //
            // Attach to Manually Placed Icon       //
            //////////////////////////////////////////
            //PopupMenusDocumentInsertPoint.Text = PopupMenuSupport.Create_Icon_Attached_Injectable_Directory_Popup_Menu(PopupMenusDocumentInsertPoint.Text);

            //////////////////////
            // Inject News Blog //
            //////////////////////
            BlogInsertPoint.Text = Blogs.Create_Injectable_Blog_Advertisements(BlogInsertPoint.Text);

            //////////////////////////////
            // Inject Ad Rotator Images //
            //////////////////////////////
            RotatorIMagesInsertPoint.Text = Blogs.Create_Injectable_Ads("/Advertisements", RotatorIMagesInsertPoint.Text);

            ///////////////////////////////////////////////
            // Inject Video Folders And Popup File Lists //
            ///////////////////////////////////////////////
            PopupMenusVideoInsertPoint.Text = PopupMenuSupport.Create_Icon_Attached_Injectable_Video_Popup_Menu(PopupMenusVideoInsertPoint.Text);
        }


    }


}