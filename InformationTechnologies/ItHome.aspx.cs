using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class InformationTechnologies_ItHome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "InformationTechnologies_ItHome.PageLoad";

        if ( ! IsPostBack )
        {
            StringCollection SessionVar = (StringCollection)Session["UserGroups"];
            if (SessionVar != null)
            {
                if (SessionVar.Contains("ShermcoYou_It"))
                    Sx.InnerHtml = "/InformationTechnologies/ItAdmin.aspx";

                if (SessionVar.Contains("IT Department"))
                    Sx.InnerHtml = "/InformationTechnologies/ItAdmin.aspx";
            }
        }
    }
}