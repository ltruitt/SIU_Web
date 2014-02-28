using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class Forms_HardwareRequestAdmin : System.Web.UI.Page
{
    private const string DATETIME_FORMAT = "MM/dd/yyyy";

    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "Forms_HardwareRequestAdmin.PageLoad";

        if (IsPostBack)
        {
            Response.Redirect(Page.Request.Path);
        }
    }

    public string ShowJobHeader(object DataItem)
    {
        //////////////////////
        // Type Cast Record //
        //////////////////////
        SIU_IT_HW_Req DataRcd = (SIU_IT_HW_Req)DataItem;

        //////////////////
        // Build Header //
        //////////////////
        string ReqID = DataRcd.XctnID.ToString();

        string RcdContentFormat = "&nbsp;&nbsp;&nbsp;For&nbsp; {1} &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;{2}";
        string HideShowBtn = string.Format("<input type='button' id=\"{0}_A\" value='Show Request' class='ShowBtnCSS' onclick=\"ShowHide('{1}');\" />", ReqID, ReqID);

        string ForEmpName = SqlServer_Impl.GetEmployeeNameByNo( DataRcd.For_EmpID.ToString()  );
        string ReqDate = FormatDateTime(DataRcd.Timestamp);

        string RcdContent = String.Format(RcdContentFormat, ReqID, ForEmpName, ReqDate);

        return HideShowBtn + RcdContent;
    }

    public string ShowJobDetails(object DataItem)
    {
        ////////////////////////
        //// Type Cast Record //
        ////////////////////////
        SIU_IT_HW_Req DataRcd = (SIU_IT_HW_Req)DataItem;


        string ReqID = DataRcd.XctnID.ToString();
        string FromEmpName = SqlServer_Impl.GetEmployeeNameByNo(DataRcd.Req_EmpID.ToString());




        string RcdContent = String.Format("<div id='{0}' style='display:none;'> ", ReqID);

        RcdContent += "<br/>&nbsp;&nbsp;Requested By:&nbsp;" + FromEmpName + "<br/>";

        RcdContent += "<br/>&nbsp;&nbsp;Role:&nbsp;" + DataRcd.Role + "<br/><br/>";

        RcdContent += "&nbsp;&nbsp;<b><span style='color: yellow'>" + DataRcd.ComputerDesc + "</span></b><br/>";

        if (DataRcd.MonitorCount > 0 )
            RcdContent += "&nbsp;&nbsp;&nbsp;&nbsp;" + DataRcd.MonitorCount.ToString() + "&nbsp;Monitors<br/>";

        if (DataRcd.MonitorStandCount > 0)
            RcdContent += "&nbsp;&nbsp;&nbsp;&nbsp;" + DataRcd.MonitorStandCount.ToString()  + "&nbsp;Monitor Stands<br/>";

        if (DataRcd.ComputerCase)
            RcdContent += "&nbsp;&nbsp;&nbsp;&nbsp;A Computer Case<br/>";

        if (DataRcd.ComputerDock)
            RcdContent += "&nbsp;&nbsp;&nbsp;&nbsp;A Computer Dock<br/>";

        if (DataRcd.BackPack)
            RcdContent += "&nbsp;&nbsp;&nbsp;&nbsp;A Backpack<br/>";

        if (DataRcd.Adobe)
            RcdContent += "&nbsp;&nbsp;&nbsp;&nbsp;Loaded With Adobe<br/>";

        if (DataRcd.CAD)
            RcdContent += "&nbsp;&nbsp;&nbsp;&nbsp;Loaded With CAD<br/>";

        if (DataRcd.MsPrj)
            RcdContent += "&nbsp;&nbsp;&nbsp;&nbsp;Loaded With Microsoft Project<br/>";

        if (DataRcd.Visio)
            RcdContent += "&nbsp;&nbsp;&nbsp;&nbsp;Loaded With Visio<br/><br/>";

        RcdContent += "Estimated Total Price&nbsp;$" + DataRcd.Price.ToString() + "<br/>";

        RcdContent +=
            "<input type='submit' runat='server' value='Complete' id='SubmitButton" + ReqID + ":" + (string)Session["UserEmpID"] + "' class='SearchBtnCSS CloseRequest'  onclick=\"PostRequestComplete()\"  />";

        RcdContent += "</div>";

        return RcdContent;
    }


    public string FormatDateTime(object dtvalue)
    {
        string sDateTime = Convert.ToString(dtvalue);

        if (IsDateTime(sDateTime) == true)
        {
            System.DateTime dt = System.DateTime.Parse(sDateTime);
            if (dt == new DateTime(1900, 1, 1))
                sDateTime = string.Empty;
            else
                sDateTime = dt.ToString(DATETIME_FORMAT);
        }

        return sDateTime;
    }
    public static bool IsDateTime(object value)
    {
        try
        {
            if (value == null)
                return false;

            DateTime dateTime = DateTime.Parse(value.ToString());
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    [WebMethod(EnableSession = false)]
    public static string RequestComplete(string RequestID)
    {
        if ( RequestID.Substring(0,12) != "SubmitButton"   )
            return "";

        string[] RequestIdParts = RequestID.Substring(12).Split(':');
        if (RequestIdParts.Length != 2)
            return RequestID;

        int ReqID =  int.Parse( RequestIdParts[0]  );
        string EmpID =  RequestIdParts[1];

        SqlServer_Impl.CloseHardwareRequest(ReqID, EmpID);

        SIU_IT_HW_Req Req = SqlServer_Impl.GetHardwareRequest(ReqID);
        Shermco_Employee CloserEmp = SqlServer_Impl.GetEmployeeByNo(EmpID);
        BusinessLayer.HardwareRequstSendCompleteEmail(CloserEmp.Company_E_Mail, Req);
        
        Shermco_Employee ReqEmp = SqlServer_Impl.GetEmployeeByNo(Req.Req_EmpID);
        BusinessLayer.HardwareRequstSendCompleteEmail(ReqEmp.Company_E_Mail, Req);

        return RequestID;
    }


    

}