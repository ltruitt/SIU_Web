﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ELO_TimeRpt : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "ELO_TimeRpt.PageLoad";

        //////////////////////////////////////////////////////////
        // But Unless This Is A New / Refresh Form, We Are Done //
        //////////////////////////////////////////////////////////
        if (IsPostBack) return;

        ////////////////////////////
        // Show The Employee Name //
        ////////////////////////////
        lblEmpName.InnerText = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);

        ///////////////////////////////////////////
        // Bury The Employee ID For AJAX Lookups //
        ///////////////////////////////////////////
        hlblEID.InnerText = BusinessLayer.UserEmpID;

        //////////////////////////////////////////
        // Default The Time Entry Date To Today //
        //////////////////////////////////////////
        datepicker.Value = System.DateTime.Now.ToShortDateString();

        SuprArea.Visible = (BusinessLayer.UserName.ToLower() == "ltruitt") ? true : false;
    }
}