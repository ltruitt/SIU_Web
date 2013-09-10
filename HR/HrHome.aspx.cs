using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

public partial class HR_HrHome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "HR_HrHome.PageLoad";

        //////////////////////
        // Inject News Blog //
        //////////////////////
        BlogInsertPoint.Text = Blogs.Create_Injectable_Blog_Advertisements(BlogInsertPoint.Text);

        StringCollection SessionVar = (StringCollection)Session["UserGroups"];
        if (SessionVar != null)
        {
            if (SessionVar.Contains("ShermcoYou_BandC"))
                Sx.InnerHtml = "/Hr/HrAdmin.aspx";

            if (SessionVar.Contains("ShermcoYou_Corp"))
                Sx.InnerHtml = "/Hr/HrAdmin.aspx";
        }
    }
}