using System;
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



    protected void btn_SSL_Click(object sender, EventArgs e)
    {
        SLDocument sl = new SLDocument();

        sl.SetCellValue("C2", "Apple");
        sl.SetCellValue("D2", "Banana");
        sl.SetCellValue("E2", "Cherry");
        sl.SetCellValue("F2", "Durian");
        sl.SetCellValue("G2", "Elderberry");
        sl.SetCellValue("B3", "North");
        sl.SetCellValue("B4", "South");
        sl.SetCellValue("B5", "East");
        sl.SetCellValue("B6", "West");

        Random rand = new Random();
        for (int i = 3; i <= 6; ++i)
        {
            for (int j = 3; j <= 7; ++j)
            {
                sl.SetCellValue(i, j, 9000 * rand.NextDouble() + 1000);
            }
        }

        SLChart chart;

        chart = sl.CreateChart("B2", "G6");
        chart.SetChartType(SLBarChartType.ClusteredBar);
        // the chartsheet name should not clash with any existing sheet names
        // including worksheet names.
        sl.InsertChart(chart, "Chart1");

        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("Content-Disposition", "attachment; filename=WebStreamDownload.xlsx");
        sl.SaveAs(Response.OutputStream);
        Response.End();

    }
}
