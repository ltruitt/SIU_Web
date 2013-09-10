using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Corporate_BandC_iTextBasicLookup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string QualCode = null;
        string EmpID = null;

        if (IsPostBack)
        {
            EmpID = ddEmpIds.Value;
            QualCode = acCertList.Value;
        }

        if (QualCode == null || EmpID == null) return;
        string[] codeParts = QualCode.Split(' ');
        if (codeParts.Length == 0) return;

        string _target = "/Corporate/BandC/iText.aspx?code=" + codeParts[0] + "&emp=" + EmpID;

        Response.Write("<script>window.open('" + _target + "','_blank');</script>");
    }
}