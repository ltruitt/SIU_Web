using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

public partial class Blog_Blog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "InformationTechnologies_ItHome.PageLoad";

        if (!IsPostBack)
        {
            StringCollection SessionVar = (StringCollection)Session["UserGroups"];
            if (SessionVar != null)
            {
                if (SessionVar.Contains("ShermcoYou_It"))
                    Sx.InnerHtml = "/Blog/BlogAdmin.aspx";

                if (SessionVar.Contains("ShermcoYou_Safety_Pays"))
                    Sx.InnerHtml = "/Blog/BlogAdmin.aspx";
            }
        }
    }
}