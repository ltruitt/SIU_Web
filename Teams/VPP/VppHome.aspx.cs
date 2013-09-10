using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Safety_VPP_VppHome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "Teams_VEST_VestTeamHome.PageLoad";

        if (IsPostBack)
            return;

        //////////////////////////////
        // Inject Ad Rotator Images //
        //////////////////////////////
        RotatorIMagesInsertPoint.Text = Blogs.Create_Injectable_Ads("/Advertisements", RotatorIMagesInsertPoint.Text);
    }
}