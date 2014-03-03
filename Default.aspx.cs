using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

using SpreadsheetLight;
using SpreadsheetLight.Charts;


public partial class _Default : System.Web.UI.Page
{


    void Page_Load(object sender, EventArgs e)
    {
        Response.Write(System.Security.Principal.WindowsIdentity.GetCurrent().Name);


        //try
        //{
        //    if (IsPostBack)
        //    {

        //        switch (Request.Form["__EVENTTARGET"])
        //        {
        //            case "WarnLink":
        //                throw new ArgumentException("Trace warn.");
        //                break;
        //            case "WriteLink":
        //                throw new InvalidOperationException("Trace write.");
        //                break;
        //            default:
        //                throw new ArgumentException("General exception.");
        //                break;
        //        }
        //    }
        //}
        //catch (ArgumentException ae)
        //{
        //    Trace.Warn("Exception Handling", "Warning: Page_Load.", ae);
        //}
        //catch (InvalidOperationException ioe)
        //{
        //    Trace.Write("Exception Handling", "Exception: Page_Load.", ioe);
        //}
    }

    string provider = "RSAProtectedConfigurationProvider";
    string section = "connectionStrings";

    protected void btnEncrypt_Click(object sender, EventArgs e)
    {
        Configuration confg = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
        ConfigurationSection configSect = confg.GetSection(section);

        if (configSect != null)
        {
            configSect.SectionInformation.ProtectSection(provider);
            confg.Save();
        }
    }

    protected void btnDecrypt_Click(object sender, EventArgs e)
    {
        Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
        ConfigurationSection configSect = config.GetSection(section);
        if (configSect.SectionInformation.IsProtected)
        {
            configSect.SectionInformation.UnprotectSection();
            config.Save();
        }
    }


    protected void TestEmailClick(object sender, EventArgs e)
    {
        WebMail.TestEmail(BusinessLayer.UserEmail);
    }


    protected void TestEmailBbClick(object sender, EventArgs e)
    {
        const string eMailSubject = "Unresolved Vehicle Inspections.";

        WebMail.HtmlMail("bborowczak@shermco.com", eMailSubject, BusinessLayer.GenFleetInspRpt(""));

        foreach (var deptList in SqlServer_Impl.GetReportingChainList())
        {
            List<string> emList = SqlServer_Impl.GetEmployeeEmailByNo(new List<string>() { deptList.DeptMgrEmpId });
            if (emList.Count > 0)
            {
                string email = BusinessLayer.GenFleetInspRpt(deptList.Dept);
                if ( email.Length > 0 )
                    WebMail.HtmlMail(emList[0], eMailSubject, email);
            }
                
        }
    }

    protected void TestEmailQtm1stClick(object sender, EventArgs e)
    {
        BusinessLayer.GenQtmNotices();
    }

    protected void TestEmailQtm15thClick(object sender, EventArgs e)
    {
        BusinessLayer.GenQtmReminders();
    }

    protected void DocSearch(object sender, EventArgs e)
    {
        PopupMenuSupport.FileNameSearch("pdf");
    }
}
