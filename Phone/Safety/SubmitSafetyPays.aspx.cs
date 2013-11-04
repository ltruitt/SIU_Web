using System;
using System.Collections.Specialized;

public partial class Forms_SafetyIssue : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "SubmitSafetyPays.PageLoad";

        try
        {
            // If this is the first visit to the page, bind the role data to the datalist 
            if (Page.IsPostBack)
                return;

            //////////////////////////
            // Build Requestor Info //
            //////////////////////////
            hlblEID.InnerHtml = BusinessLayer.UserEmpID;
            lblEmpName.InnerHtml = "(" + BusinessLayer.UserEmpID + ") " + BusinessLayer.UserFullName;

            ///////////////////////////
            // Check For Admin Priv. //
            ///////////////////////////
            StringCollection sessionVar = (StringCollection)Session["UserGroups"];
            if (sessionVar != null)
                if (SuprArea != null) 
                    SuprArea.Visible = (sessionVar.Contains("ShermcoYou_Safety_Pays"));
        }

        catch (Exception exc)
        {
            //Module failed to load 
            string err = exc.Message;
        }
    }




        

    //////    ///////////////////////////////////////////////////////
    //////    // Map Any Data FIlled In On ASP Form To Data Object //
    //////    // Field Names Must Match                            //
    //////    ///////////////////////////////////////////////////////
    //////    newReport = SqlDataMapper<SIU_SafetyPaysReport>.MapAspForm(newReport, Request);



    //////    ////////////////////////////////////////////////
    //////    // Fill Out Text Description Of Incident Type //
    //////    ////////////////////////////////////////////////
    //////    SetIncidentTypeText(newReport, (Button) sender);

    //////    return;       

    //////    WebMail.SafetyPaysNewEmail(newReport, BusinessLayer.UserEmail, BusinessLayer.UserFullName);


    //////    Response.Redirect("/", true);
    //////}

}