using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HomePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //////////////////////
            // Inject News Blog //
            //////////////////////
            BlogInsertPoint.Text = Blogs.Create_Injectable_Blog_Advertisements(BlogInsertPoint.Text);

            //////////////////////////////
            // Inject Ad Rotator Images //
            //////////////////////////////
            RotatorIMagesInsertPoint.Text = Blogs.Create_Injectable_Ads("/Advertisements", RotatorIMagesInsertPoint.Text);          
        }
    }
}