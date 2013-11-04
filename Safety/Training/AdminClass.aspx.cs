using System;

public partial class Safety_Training_AdminClass : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (this.FileUpload1.HasFile)
        //{
        //    this.FileUpload1.SaveAs("c:\\" + this.FileUpload1.FileName);
        //}

        if (!IsPostBack)
            return;

        string Method = "Safety_Training_AdminClass.PageLoad";

        try
        {
            // If this is the first visit to the page, bind the role data to the datalist 
            if (Page.IsPostBack)
                return;

            //////////////////////////
            // Build Requestor Info //
            //////////////////////////
            hlblEID.InnerText = BusinessLayer.UserEmpID;
            lblEmpName.InnerText = "(" + BusinessLayer.UserEmpID + ") " + BusinessLayer.UserFullName;
        }

        catch (Exception exc)
        {
            SqlServer_Impl.LogDebug(Method, exc.Message);
        }
    }
}