using System;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Runtime.InteropServices;

// Package-Install iTextSharp

// <Optionally>  Add Table To Hold:
//      Certificate Code
//      Name Of PDF Template
//      Signature Position
//      Student Name Position
//      Course Name Position
//      Date Of Class Position
//      Instructor Name Position
//      Course Code Position
//      Student ID Position
// Lookup Above Table To Generate Custom Certificates

#region WIN API Declarations

    //used in calling WNetAddConnection2

    [StructLayout(LayoutKind.Sequential)]
    public struct NETRESOURCE
    {
        public int dwScope;
        public int dwType;  
        public int dwDisplayType;
        public int dwUsage;

        [MarshalAs(UnmanagedType.LPStr)]
        public string lpLocalName;

        [MarshalAs(UnmanagedType.LPStr)]
        public string lpRemoteName;

        [MarshalAs(UnmanagedType.LPStr)]
        public string lpComment;

        [MarshalAs(UnmanagedType.LPStr)]
        public string lpProvider;
    }






#endregion

  


public partial class Corporate_BandC_iText : System.Web.UI.Page
{

    public string StreamCertTemplate = "~/BandC/Templates/CertTemplate.pdf";
    public string StreamSignature = "~/BandC/Signatures";

    //WIN32API - WNetAddConnection2
    [DllImport("mpr.dll", CharSet = CharSet.Auto)]
    private static extern int WNetAddConnection2A(
        [MarshalAs(UnmanagedType.LPArray)] NETRESOURCE[] lpNetResource,
        [MarshalAs(UnmanagedType.LPStr)] string lpPassword,
        [MarshalAs(UnmanagedType.LPStr)] string lpUserName,
        int dwFlags);

    [DllImport("mpr.dll", CharSet = CharSet.Auto)]
    private static extern int WNetCancelConnection2A(
        [MarshalAs(UnmanagedType.LPStr)] string lpName,
        int dwFlags, int fForce);

    protected void Page_Load(object sender, EventArgs e)
    {
        string qualCode;
        string empId;

        qualCode = Request["code"];
        empId = Request["emp"];

        if (qualCode == null || empId == null)
            Response.Redirect("~/Corporate/BandC/iTextBasicLookup.aspx");

        BuildCertInMemory(qualCode, empId);

        ////////////////////////////////////////////////////
        // This Will Never Execute IF Generation Succeeds //
        ////////////////////////////////////////////////////
        Response.Redirect("~/Corporate/BandC/iTextBasicLookup.aspx");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        String htmlText = "<font  " +
         " color=\"#0000FF\"><b><i>Title One</i></b></font><font   " +
         " color=\"black\"><br><br>Some text here<br><br><br><font   " +
         " color=\"#0000FF\"><b><i>Another title here   " +
         " </i></b></font><font   " +
         " color=\"black\"><br><br>Text1<br>Text2<br><OL><LI><DIV Style='color:green'>Pham Duy Hoa</DIV></LI><LI>how are u</LI></OL><br/>"+
         "<table border='1'><tr><td style='color:red;text-align:right;width:20%'>123456</td><td style='color:green;width:60%'>78910</td><td style='color:red;width:20%'>ASFAFA</td></tr><tr><td style='color:red;text-align:right'>123456</td><td style='color:green;width:60%'>78910</td><td style='color:red;width:20%'>DAFSDGAFW</td></tr></table><br/>"+
         "<div><ol><li>123456</li><li>123456</li><li>123456</li><li>123456</li></ol></div>";
 
        HTMLToPdf(htmlText, "PDFfile.pdf");
    }

    public string SFsmsShareUserPassword = "Payday2013";
    public string _SFsmsShareUser = "ltruitt";
    public string SFsmsShare = @"\\shermco\dfs\Public\NavisionScan\Qualifications\BcDone";
    public string SFsmsShare2 = @"\\shermco\dfs\Public\NavisionScan\Qualifications\Done";

    private byte[] GetFSMSFile(string sFile)
    {
        NETRESOURCE[] nr = new NETRESOURCE[1];
        nr[0].lpRemoteName = SFsmsShare;
        nr[0].lpLocalName = ""; //mLocalName;
        nr[0].dwType = 1; //disk
        nr[0].dwDisplayType = 0;
        nr[0].dwScope = 0;
        nr[0].dwUsage = 0;
        nr[0].lpComment = "";
        nr[0].lpProvider = "";
        int iErr = WNetAddConnection2A(nr, SFsmsShareUserPassword, _SFsmsShareUser, 0);

        if (iErr > 0)
        {
            nr[0].lpRemoteName = SFsmsShare2;
            iErr = WNetAddConnection2A(nr, SFsmsShareUserPassword, _SFsmsShareUser, 0);
        }

        if (iErr > 0)
            throw new Exception("Can not open qualifications leagacy document folder");

        FileStream st = null;

        try
        {
            st = new FileStream(SFsmsShare + "\\" + sFile, FileMode.Open);
            int iLen = (int)st.Length;
            byte []b = new byte[iLen];
            st.Read(b, 0, iLen);
            return b;
        }

        finally
        {
            if( st != null )
                st.Close();

            WNetCancelConnection2A(SFsmsShare, 0, -1);

        }
    } 
    public void BuildCertInMemory(string QualCode, string EmpID)
    {

        //Document doc = new Document();
        //MemoryStream s = new MemoryStream();
        //PdfWriter writer = PdfWriter.GetInstance(doc, s);
        //doc.Open();
        //doc.Add(new Paragraph("Some Text"));
        //writer.CloseStream = false;
        //doc.Close();
        //s.Position = 0;
        //ShowPdf(s);
        //return;

        Shermco_Employee_Qualification qualRcd = SqlServer_Impl.GetEmpCertDocument(QualCode, EmpID);
        if (qualRcd == null)
            return;
        if (qualRcd.Qualification_Code != QualCode)
            return;

        /////////////////////////
        // Pdf Sitting On Disk //
        /////////////////////////
        if (qualRcd.FileName.Length > 0)
        {
            PdfReader rdr = new PdfReader(GetFSMSFile(qualRcd.FileName));
            using (MemoryStream strm = new MemoryStream())
            {
                PdfStamper stamper = new PdfStamper(rdr, strm);
                //PdfContentByte pdfContentByte = stamper.GetUnderContent(1);
                stamper.Close();
                ShowPdf(strm);
            }
            return;
        }


        ////////////////////
        // Generate A Pdf //
        ////////////////////
        PdfReader r = new PdfReader(Server.MapPath(StreamCertTemplate));
        using (MemoryStream s = new MemoryStream())
        {
            PdfStamper stamper = new PdfStamper(r, s);
            PdfContentByte pdfContentByte = stamper.GetUnderContent(1);

            /////////////////////////
            // Add Signature Image //
            /////////////////////////
            string[] sigFileList = Directory.GetFiles(HttpContext.Current.Server.MapPath(StreamSignature), qualRcd.Institution_Company + ".*");
            if (sigFileList.Length > 0)
            {
                var image = iTextSharp.text.Image.GetInstance(new FileStream(sigFileList[0], FileMode.Open));
                image.ScaleAbsolute(100, 50);
                image.SetAbsolutePosition(PageSize.A4.Width - 36f - 72f - 40f, PageSize.A4.Height - 36f - 50f - 575f);
                pdfContentByte.AddImage(image);
            }

            //////////////////////
            // Add Student Name //
            //////////////////////
            Shermco_Employee emp = SqlServer_Impl.GetEmployeeByNo(EmpID);

            pdfContentByte.BeginText();
            pdfContentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, true), 40);
            pdfContentByte.SetTextMatrix(200, 350);
            pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, emp.First_Name + " " + emp.Last_Name, 395, 350, 0);
            pdfContentByte.EndText();

            //////////////////////
            // Add Course Name //
            //////////////////////
            pdfContentByte.BeginText();
            pdfContentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, true), 30);
            pdfContentByte.SetTextMatrix(200, 350);
            pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, qualRcd.Description, 395, 290, 0);
            pdfContentByte.EndText();


            //////////////
            // Add Date //
            //////////////
            pdfContentByte.BeginText();
            pdfContentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, true), 12);
            pdfContentByte.SetTextMatrix(612, 197);
            pdfContentByte.ShowText(qualRcd.From_Date.ToShortDateString());
            pdfContentByte.EndText();


            /////////////////////////
            // Add Instructor Name //
            /////////////////////////
            Shermco_Employee instr = SqlServer_Impl.GetEmployeeByNo(qualRcd.Institution_Company);

            if (instr != null)
            {
                pdfContentByte.BeginText();
                pdfContentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, true), 12);
                pdfContentByte.SetTextMatrix(455, 185);
                pdfContentByte.ShowText(instr.First_Name + " " + instr.Last_Name);
                pdfContentByte.EndText();
            }

            /////////////////////
            // Add Course Code //
            /////////////////////
            pdfContentByte.BeginText();
            pdfContentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, true), 12);
            pdfContentByte.SetTextMatrix(525, 140);
            pdfContentByte.ShowText(QualCode);
            pdfContentByte.EndText();

            ////////////////////
            // Add Student ID //
            ////////////////////
            pdfContentByte.BeginText();
            pdfContentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, true), 12);
            pdfContentByte.SetTextMatrix(525, 122);
            pdfContentByte.ShowText(qualRcd.Employee_No_);
            pdfContentByte.EndText();


            stamper.FormFlattening = true;
            stamper.Close();
            ShowPdf(s);
        }
    }

    public void BuildCertOnDisk(string QualCode, string EmpID)
    {

        Shermco_Employee_Qualification qualRcd = SqlServer_Impl.GetEmpCertDocument(QualCode, EmpID);
        if (qualRcd == null)
            return;
        if (qualRcd.Qualification_Code != QualCode)
            return;

        using (var inputPdfStream = new FileStream(Server.MapPath(StreamCertTemplate), FileMode.Open))
        using (var outputPdfStream = new FileStream(Request.PhysicalApplicationPath + "\\TempPdf\\BCGen.pdf", FileMode.Create))
        {
            PdfReader reader = new PdfReader(inputPdfStream);
            PdfStamper stamper = new PdfStamper(reader, outputPdfStream);
            PdfContentByte pdfContentByte = stamper.GetUnderContent(1);

            ////////////////////////////////
            // Get Name Of Signature File //
            ////////////////////////////////
            string[] sigFileList = Directory.GetFiles(HttpContext.Current.Server.MapPath(StreamSignature), qualRcd.Institution_Company + ".*");

            /////////////////////////
            // Add Signature Image //
            /////////////////////////
            if (sigFileList.Length > 0)
            {
                var image = iTextSharp.text.Image.GetInstance(new FileStream(sigFileList[0], FileMode.Open));
                image.ScaleAbsolute(100, 50);
                image.SetAbsolutePosition(PageSize.A4.Width - 36f - 72f - 40f, PageSize.A4.Height - 36f - 50f - 575f);
                pdfContentByte.AddImage(image);
            }


            //////////////////////
            // Add Student Name //
            //////////////////////
            Shermco_Employee emp = SqlServer_Impl.GetEmployeeByNo(EmpID);

            pdfContentByte.BeginText();
            pdfContentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, true), 40);
            pdfContentByte.SetTextMatrix(200, 350);
            pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, emp.First_Name + " " + emp.Last_Name, 395, 350, 0);
            pdfContentByte.EndText();


            //////////////////////
            // Add Course Name //
            //////////////////////
            pdfContentByte.BeginText();
            pdfContentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, true), 30);
            pdfContentByte.SetTextMatrix(200, 350);
            pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, qualRcd.Description, 395, 290, 0);
            pdfContentByte.EndText();


            //////////////
            // Add Date //
            //////////////
            pdfContentByte.BeginText();
            pdfContentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, true), 12);
            pdfContentByte.SetTextMatrix(612, 197);
            pdfContentByte.ShowText(qualRcd.From_Date.ToShortDateString() );
            pdfContentByte.EndText();


            /////////////////////////
            // Add Instructor Name //
            /////////////////////////
            Shermco_Employee instr = SqlServer_Impl.GetEmployeeByNo(qualRcd.Institution_Company);

            pdfContentByte.BeginText();
            pdfContentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, true), 12);
            pdfContentByte.SetTextMatrix(455, 185);
            pdfContentByte.ShowText(instr.First_Name + " " + instr.Last_Name);
            pdfContentByte.EndText();

            /////////////////////
            // Add Course Code //
            /////////////////////
            pdfContentByte.BeginText();
            pdfContentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, true), 12);
            pdfContentByte.SetTextMatrix(525, 140);
            pdfContentByte.ShowText(QualCode);
            pdfContentByte.EndText();

            ////////////////////
            // Add Student ID //
            ////////////////////
            pdfContentByte.BeginText();
            pdfContentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, true), 12);
            pdfContentByte.SetTextMatrix(525, 122);
            pdfContentByte.ShowText(qualRcd.Employee_No_);
            pdfContentByte.EndText();


            stamper.Close();
        }

        ShowPdf("\\TempPdf\\BCGen.pdf");


    }
 
    public void HTMLToPdf(string HTML, string FilePath)
    {
        Document document = new Document();
        try
        {
            PdfWriter.GetInstance(document, new FileStream(Request.PhysicalApplicationPath + "\\TempPdf\\Chap0102.pdf", FileMode.Create));
            document.Open();
            iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(Server.MapPath("//Images/SiLogo.png"));

            pdfImage.ScaleToFit(100, 50);

            pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING; pdfImage.SetAbsolutePosition(180, 760);

            document.Add(pdfImage);
            //iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
            iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);
            hw.Parse(new StringReader(HTML));
            document.Close();
            ShowPdf("\\TempPdf\\Chap0102.pdf");
        }

        catch
        {
            document.Close();
        }
    }



    private void ShowPdf(MemoryStream s)
    {
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Disposition", "inline;filename=Cert.pdf");
        Response.OutputStream.Write(s.GetBuffer(), 0, s.GetBuffer().Length);
        Response.OutputStream.Flush();
        Response.OutputStream.Close();
        Response.End();
    }


    private void ShowPdf(string s)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "inline;filename=" + s);
        Response.ContentType = "application/pdf";
        Response.WriteFile(s);
        Response.Flush();
        Response.Clear();
    }
}
