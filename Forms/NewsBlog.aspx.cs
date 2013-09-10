using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_Blog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "Forms_Blog.PageLoad";

        if (Request["BlogID"] != null)
        {
            lblBlogName.InnerHtml = Request["BlogID"].ToString() + " Blog Management";
        }
    }


    protected void lblBtnSubmit_Click(object sender, EventArgs e)
    {
        /////////////////////////////////////////////
        //// Make And Initialize A New Data Object //
        /////////////////////////////////////////////
        SIU_Blog newBlog = SqlDataMapper<SIU_Blog>.MakeNewDAO<SIU_Blog>();

        /////////////////////////////////////////////////////////
        //// Map Any Data FIlled In On ASP Form To Data Object //
        //// Field Names Must Match                            //
        /////////////////////////////////////////////////////////
        newBlog = SqlDataMapper<SIU_Blog>.MapAspForm(newBlog, Request);

        ////////////////////
        //// New Incident //
        ////////////////////
        newBlog.BlogName = lblBlogName.InnerText.Replace(" Blog Management", "");

        ////////////////////////////////////
        //// Write New Record To Database //
        ////////////////////////////////////
        SqlServer_Impl.RecordBlog(newBlog);


        Response.Redirect("/", true);

    }
}