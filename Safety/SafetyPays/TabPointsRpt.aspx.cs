using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using ShermcoYou.DataTypes;

public partial class Safety_SafetyPays_TabPointsRpt : System.Web.UI.Page
{
    DateTime start = DateTime.Parse("1/1/2013");
    DateTime end = DateTime.Parse("2/1/2013");

    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "TabPointsRpt.PageLoad";

        //////////////////////////////////////////////////////////
        // But Unless This Is A New / Refresh Form, We Are Done //
        //////////////////////////////////////////////////////////
        if (IsPostBack)
        {
            start = DateTime.Parse(StartDate.Value);
            end = DateTime.Parse(EndDate.Value);
            return;
        }

        ////////////////////////////
        // Show The Employee Name //
        ////////////////////////////
        lblEmpName.InnerText = SqlServer_Impl.GetEmployeeNameByNo(BusinessLayer.UserEmpID);

        ///////////////////////////////////////////
        // Bury The Employee ID For AJAX Lookups //
        ///////////////////////////////////////////
        hlblEID.InnerText = BusinessLayer.UserEmpID;
    }

    ////////////////////////////////////////////////////////////
    // Type Used In Dictionary To Track Monthly Totals By Emp //
    ////////////////////////////////////////////////////////////
    private class EmpByMon
    {
        public int[] Months = new int[14];
    }

    ////////////////////////////////////////////////////////////
    // Type Used In Dictionary To Track Monthly Totals By Emp //
    ////////////////////////////////////////////////////////////
    private class EmpDtl
    {
        public int[] ReasonPts;
    }

    protected void ByDeptExportToExcelButton_Click(object sender, System.EventArgs e)
    {
        System.Text.UnicodeEncoding uniEncoding = new System.Text.UnicodeEncoding();
        if (end.Day == 1)
            end = end.AddDays(-1);

        using (MemoryStream ms = new MemoryStream())
        {

            Dictionary<int, string> Months = new Dictionary<int, string>();                 // List Of Months  < 1, "1" >

            EmpByMon SumEmpPts;                                                             // Array [14] (One For Each Month  [0] For Sum 
            Dictionary<string, EmpByMon> sumEmpMon = new Dictionary<string, EmpByMon>();    // Sum By Emp  <"empNo",  Array[14] As Above
            
            


            var sw = new StreamWriter(ms, uniEncoding);
            int StartRowCnt = 5;
            int EndRowCnt = 5;
            try
            {
                /////////////////////////////////////////////////////////////
                // Get Employee Points Detail Records For Reporting Period //
                /////////////////////////////////////////////////////////////
                var data = SqlServer_Impl.GetAdminPointsRptEmpPoints(start, end).OrderBy("EmpDept");

                /////////////////////////////
                // Build Projection Tables //
                /////////////////////////////
                _PrjPts Pts = new _PrjPts(start, end);

                ///////////////////////////////////////////////////////////
                // Build A Summary Array For Each Employee For Each Dept //
                //////////////////////////////////////////////////////////
                foreach (SIU_Points_Rpt RptRcd in data)
                {
                    if (sumEmpMon.ContainsKey(RptRcd.Emp_No))
                    {
                        SumEmpPts = sumEmpMon[RptRcd.Emp_No];
                        SumEmpPts.Months[RptRcd.EventDate.Month] += RptRcd.Points;
                        SumEmpPts.Months[13] += RptRcd.Points;
                    }
                    else
                    {
                        SumEmpPts = new EmpByMon();
                        SumEmpPts.Months[RptRcd.EventDate.Month] = RptRcd.Points;
                        SumEmpPts.Months[13] = RptRcd.Points;
                        sumEmpMon.Add(RptRcd.Emp_No, SumEmpPts);
                    }


                    if (!Months.ContainsKey(RptRcd.EventDate.Month))
                        Months.Add(RptRcd.EventDate.Month, RptRcd.EventDate.Month.ToString());
                    Months = Months.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                }

                ///////////////////////////////////////////////////
                // Write Styles To Help Format Excel Spreadsheet //
                ///////////////////////////////////////////////////
                sw.Write( BuildStyle() );

                ///////////////////
                // Report Header //
                ///////////////////
                sw.Write("<span style=\"font-size:1.7em; font-weight: bold\">Points Calculated For " + start.ToShortDateString() + " - " + end.ToShortDateString() +  "</span>");
                sw.WriteLine("<br/>");

                string PrevDept = "FIRSTRCD";
                string PrevEmp = "FIRSTRCD";
                int DeptSum = 0;


                ///////////////////////////////////////////////////////////////////
                // Walking Back Through The Data Of Emp Detail Rcds By Dept, Emp //
                ///////////////////////////////////////////////////////////////////
                foreach (SIU_Points_Rpt RptRcd in data)
                {
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // First Time We See An Emp -- Grab Summary Record From Above (sumEmpMon) And Produce A  Dept Detail Line //
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (PrevEmp != RptRcd.Emp_No)
                    {
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // But If We Switched To A New Department -- Write Out Summary Row, Then New Deaprtment Header, Then Column Headers //
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        if (PrevDept != RptRcd.EmpDept)
                        {
                            ////////////////////////////////
                            // Add Sum Row For Department //
                            ////////////////////////////////
                            if (PrevDept != "FIRSTRCD")
                            {
                                ////////////////////////////////////////////////////////////////////////////////////////////////
                                // Get Projections For Department (Summed Across Reporting Period -- Applies To Each Employee //
                                ////////////////////////////////////////////////////////////////////////////////////////////////
                                DeptSum = Convert.ToInt32(Pts.GetPrjSumForDept(RptRcd.EmpDept)[0]);

                                EndRowCnt--;
                                sw.Write("<tr>");
                                sw.Write("<td class=\"SumRow\"></td>");
                                sw.Write("<td class=\"SumRow\">=SUM(B" + StartRowCnt.ToString() + ":B" + EndRowCnt.ToString() + ")</td>");  // Points Sum
                                sw.Write("<td class=\"SumRow\">=SUM(C" + StartRowCnt.ToString() + ":C" + EndRowCnt.ToString() + ")</td>");  // Prj Pts Sum

                                int col = 'D';
                                foreach (KeyValuePair<int, string> pair in Months)
                                    sw.Write("<td class=\"SumRow\">=SUM(" + (char)col + StartRowCnt.ToString() + ":" + (char)col++ + EndRowCnt.ToString() + ")</td>");

                                sw.Write("<td class=\"SumRow\">=TEXT( (B" + (EndRowCnt + 1).ToString() + "/C" + (EndRowCnt + 1).ToString() + "),\"#%\")</td>");  // Pct Compliance

                                sw.Write("</tr>");
                                sw.Write("</table>");
                                StartRowCnt = EndRowCnt + 5;
                                EndRowCnt = StartRowCnt;
                            }

                            /////////////////////////////////
                            // Header For Next Deptartment //
                            /////////////////////////////////
                            sw.WriteLine("<br/>");
                            sw.Write("<span style=\"font-size:1.7em; font-weight: bold\">Department " + ((RptRcd.EmpDept.Length > 0) ? RptRcd.EmpDept : "Missing") + "</span>");

                            ///////////////////////////////////////////
                            // Start New Table -- Add Column Headers //
                            ///////////////////////////////////////////
                            sw.Write("<table border=\"0\">");
                            sw.Write("<tr border=\"0\"><th>Name</th><th>Points</th><th>Expected Points</th>");
                            foreach (KeyValuePair<int, string> pair in Months)
                                sw.Write("<th>" +  System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName( pair.Key ) + "</th>");
                            sw.Write("<th>Pct of Exp</th>");


                            PrevDept = RptRcd.EmpDept;
                        }

                        ///////////////////////////////////////
                        // Write Out Employee Summary Record //
                        ///////////////////////////////////////
                        SumEmpPts = sumEmpMon[RptRcd.Emp_No];

                        sw.Write("<tr style=\"text-align:center;\" >");
                        sw.Write("<td>" + RptRcd.EmpName + "</td>");
                        sw.Write("<td>" + SumEmpPts.Months[13] + "</td>");
                        sw.Write("<td>" + DeptSum.ToString() + "</td>");

                        foreach (KeyValuePair<int, string> pair in Months)
                            sw.Write("<td>" + SumEmpPts.Months[pair.Key] + "</td>");
                        sw.Write("<td >=TEXT( (B" + EndRowCnt.ToString() + "/C" + EndRowCnt.ToString() + "),\"#%\")</td>");  // Pct Compliance
                        sw.Write("</tr>");

                        PrevEmp = RptRcd.Emp_No;
                        EndRowCnt++;
                    }
                }


                /////////////////////////////////////
                // Add Sum Row To Last Deptartment //
                /////////////////////////////////////
                if (PrevDept != "FIRSTRCD")
                {
                    EndRowCnt--;
                    sw.Write("<tr style=\"text-align:center;\" >");
                    sw.Write("<td class=\"SumRow\"></td>");
                    sw.Write("<td class=\"SumRow\">=SUM(B" + StartRowCnt.ToString() + ":B" + EndRowCnt.ToString() + ")</td>");
                    sw.Write("<td class=\"SumRow\">=SUM(C" + StartRowCnt.ToString() + ":C" + EndRowCnt.ToString() + ")</td>");  // Prj Pts Sum
                    int col = 'D';
                    foreach (KeyValuePair<int, string> pair in Months)
                        sw.Write("<td class=\"SumRow\">=SUM(" + (char)col + StartRowCnt.ToString() + ":" + (char)col++ + EndRowCnt.ToString() + ")</td>");

                    sw.Write("<td class=\"SumRow\">=TEXT( (B" + (EndRowCnt + 1).ToString() + "/C" + (EndRowCnt + 1).ToString() + "),\"#%\")</td>");  // Pct Compliance
                    sw.Write("</tr>");
                }

                sw.Write("</table>");
                sw.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                ShowExcel(ms);
            }
            finally
            {
                sw.Dispose();
            }
        }
    }
    protected void EmpDtlExportToExcelButton_Click(object sender, System.EventArgs e)
    {
        System.Text.UnicodeEncoding uniEncoding = new System.Text.UnicodeEncoding();
        if (end.Day == 1)
            end = end.AddDays(-1);

        using (MemoryStream ms = new MemoryStream())
        {

            //////////////////////////////////////////////////////////////////////////////////////////
            // Buid Dict To Hold Freq Used Conversion of PointsType Index to PointsType Description //
            //////////////////////////////////////////////////////////////////////////////////////////
            Dictionary<int, string> PtTypes = SqlServer_Impl.GetAutoCompletePointTypes().ToDictionary(mc => (int)mc.UID, mc => (string)mc.Description);
            EmpDtl ed = new EmpDtl();
            ed.ReasonPts = new int[PtTypes.Count];
            SIU_Points_Rpt PrevRcd = null;

            var sw = new StreamWriter(ms, uniEncoding);
            try
            {
                var data = SqlServer_Impl.GetAdminPointsRptEmpPoints(start, end);

                ///////////////////////////////////////////////////
                // Write Styles To Help Format Excel Spreadsheet //
                ///////////////////////////////////////////////////
                sw.Write( BuildStyle() );

                sw.Write("<span style=\"font-size:1.7em; font-weight: bold\">Points Calculated For " + start.ToShortDateString() + " - " + end.ToShortDateString() + "</span>");
                sw.WriteLine("<br/>");
                sw.WriteLine("<br/>");
                

                // Table Header
                sw.Write("<table class=\"tbl\">");
                sw.Write("<tr>");
                sw.Write("<td class=\"UnderlineRow\" style=\"width: 125px; \">Name</td>");
                sw.Write("<td class=\"UnderlineRow\" style=\"width: 45px; \">Dept</td>");
                sw.Write("<td class=\"UnderlineRow\" style=\"width: 60px; border-right: 2px solid black !important;\">Eligibility</td>");
                foreach (KeyValuePair<int, string> pair in PtTypes)
                    sw.Write("<td class=\"UnderlineRow\"  style=\"width: 65px; \">" + pair.Value + "</td>");
                sw.Write("<td class=\"UnderlineRow\"  style=\"width: 40px; \">Total</td>");
                sw.Write("</tr>");

                string PrevEmp = "FIRSTRCD";
                int RowCnt = 4;
                int LastCol = 'D' + PtTypes.Count - 1;

                foreach (SIU_Points_Rpt RptRcd in data)
                {
                    if (PrevEmp != RptRcd.Emp_No && PrevEmp != "FIRSTRCD")
                    {
                        sw.Write("<tr class=\"tblRow\" style=\"text-align:center;\" >");
                        sw.Write("<td>" + PrevRcd.EmpName + "</td>");
                        sw.Write("<td>" + ((PrevRcd.EmpDept.Length > 0) ? PrevRcd.EmpDept : "----") + "</td>");
                        sw.Write("<td>" + "-" + "</td>");

                        foreach (var PtCnt in ed.ReasonPts)
                            sw.Write("<td>" +   (( PtCnt > 0 ) ? PtCnt.ToString() : "") + "</td>");

                        sw.Write("<td>=SUM(D" + RowCnt.ToString() + ":" + (char)LastCol + RowCnt.ToString() + ")</td>");


                        sw.Write("</tr>");

                        RowCnt++;
                        PrevEmp = RptRcd.Emp_No;
                        ed.ReasonPts = new int[PtTypes.Count];
                    }

                    ed.ReasonPts[RptRcd.ReasonForPoints] += RptRcd.Points;
                    PrevEmp = RptRcd.Emp_No;
                    PrevRcd = RptRcd;


                }

                ///////////////////
                // Last Employee //
                ///////////////////
                sw.Write("<tr class=\"tblRow\" style=\"text-align:center;\" >");
                sw.Write("<td>" + PrevRcd.EmpName + "</td>");
                sw.Write("<td>" + ((PrevRcd.EmpDept.Length > 0) ? PrevRcd.EmpDept : "----") + "</td>");
                sw.Write("<td>" + "-" + "</td>");

                foreach (var PtCnt in ed.ReasonPts)
                    sw.Write("<td>" + ((PtCnt > 0) ? PtCnt.ToString() : "") + "</td>");

                sw.Write("<td>=SUM(D" + RowCnt.ToString() + ":" + (char)LastCol + RowCnt.ToString() + ")</td>");

                sw.Write("</tr>");



                sw.Write("</table>");
                sw.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                ShowExcel(ms);
            }
            finally
            {
                sw.Dispose();
            }
        }
    }
    protected void PrjPtsExportToExcelButton_Click(object sender, System.EventArgs e)
    {
        System.Text.UnicodeEncoding uniEncoding = new System.Text.UnicodeEncoding();
        if (end.Day == 1)
            end = end.AddDays(-1);

        using (MemoryStream ms = new MemoryStream())
        {

            //////////////////////////////////////////////////////////////////////////////////////////
            // Buid Dict To Hold Freq Used Conversion of PointsType Index to PointsType Description //
            //////////////////////////////////////////////////////////////////////////////////////////
            Dictionary<int, string> PtTypes = SqlServer_Impl.GetAutoCompletePointTypes().ToDictionary(mc => (int)mc.UID, mc => (string)mc.Description);

            /////////////////////////////
            // Build Projection Tables //
            /////////////////////////////
            _PrjPts Pts = new _PrjPts(start, end);

            var sw = new StreamWriter(ms, uniEncoding);
            try
            {
                ///////////////////////////////////////////////////
                // Write Styles To Help Format Excel Spreadsheet //
                ///////////////////////////////////////////////////
                sw.Write( BuildStyle() );

/////////////////////////
// Start Summary Table //
/////////////////////////
                sw.Write("<span style=\"font-size:1.7em; font-weight: bold\">Projected Points Calculation For " + start.ToShortDateString() + " - " + end.ToShortDateString() + "</span>");
                sw.WriteLine("<br/><br/>");



                ///////////////////////////
                // Summary Table Header  //
                ///////////////////////////
                sw.Write("<table class=\"tbl\">");
                sw.Write("<tr>");
                sw.Write("<th class=\"UnderlineRow\">Dept</th>");
                foreach (KeyValuePair<int, string> pair in PtTypes)
                    sw.Write("<th class=\"UnderlineRow\"  >" + pair.Value + " (" + pair.Key.ToString() + ")</th>");
                sw.Write("<th class=\"UnderlineRow\"  style=\"width: 40px; \">Total</th>");
                sw.Write("</tr>");

                /////////////////////////////////////////
                // Setup To Start Summary Table Detail //
                /////////////////////////////////////////
                int RowCnt = 4;
                int LastCol = 'B' + PtTypes.Count - 1;


                //////////////////////////////
                // Write Summary Table Data //
                //////////////////////////////
                foreach (var SumRptRcd in Pts.GetPrjSumDict())
                {
                    sw.Write("<tr class=\"tblRow\" style=\"text-align:center;\" >");
                    sw.Write("<td>" + ((SumRptRcd.Key.Length > 0) ? SumRptRcd.Key : "----") + "</td>");

                    foreach (double PtCnt in SumRptRcd.Value.Skip(1) )
                        sw.Write("<td>" + ((PtCnt > 0) ? PtCnt.ToString() : "0") + "</td>");

                    sw.Write("<td>=SUM(B" + RowCnt.ToString() + ":" + (char)LastCol + RowCnt.ToString() + ")</td>");
                    sw.Write("</tr>");

                    RowCnt++;
                }
                sw.Write("</table>");
                sw.Write("<br/><br/>");


//////////////////////////
// Start Monthly Tables //
//////////////////////////

                ///////////////////////////////////////////
                // Get List Of Months Included In Report //
                ///////////////////////////////////////////
                foreach (var RptMonths in Pts.GetDatesDict())
                {
                    /////////////////////////////
                    // Adjust Detail Row Index //
                    /////////////////////////////
                    RowCnt += 4;

                    //////////////////////////
                    // Monthly Table Header //
                    //////////////////////////
                    sw.Write("<span style=\"font-size:1.7em; font-weight: bold\">Projected Points Calculation For " + RptMonths.Key + "</span>");

                    //////////////////////////////////
                    // Monthly Table Column Headers //
                    //////////////////////////////////
                    sw.Write("<table class=\"tbl\">");
                    sw.Write("<tr>");
                    sw.Write("<td class=\"UnderlineRow\" >Dept</td>");
                    foreach (KeyValuePair<int, string> pair in PtTypes)
                        sw.Write("<td class=\"UnderlineRow\"  >" + pair.Value + " (" + pair.Key.ToString() + ")</td>");
                    sw.Write("<td class=\"UnderlineRow\"  style=\"width: 40px; \">Total</td>");
                    sw.Write("</tr>");

                    //////////////////////////////
                    // Write Monthly Table Data //
                    //////////////////////////////
                    foreach (var MonRptRcd in Pts.GetMonthlyDict(RptMonths.Value))
                    {
                        sw.Write("<tr class=\"tblRow\" style=\"text-align:center;\" >");
                        sw.Write("<td>" + ((MonRptRcd.Key.Length > 0) ? MonRptRcd.Key : "----") + "</td>");

                        foreach (double PtCnt in MonRptRcd.Value.Skip(1))
                            sw.Write("<td>" + ((PtCnt > 0) ? PtCnt.ToString() : "0") + "</td>");

                        sw.Write("<td>=SUM(B" + RowCnt.ToString() + ":" + (char)LastCol + RowCnt.ToString() + ")</td>");
                        sw.Write("</tr>");

                        RowCnt++;
                    }

                    sw.Write("</table>");
                    sw.Write("<br/><br/>");
                }






                ////////////////////
                // CLose The Page //
                ////////////////////
                sw.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                ShowExcel(ms);
            }
            finally
            {
                sw.Dispose();
            }
        }
    }

    protected void StdPrjPtsExportToExcelButton_Click(object sender, System.EventArgs e)
    {
        System.Text.UnicodeEncoding uniEncoding = new System.Text.UnicodeEncoding();

        using (MemoryStream ms = new MemoryStream())
        {

            //////////////////////////////////////////////////////////////////////////////////////////
            // Buid Dict To Hold Freq Used Conversion of PointsType Index to PointsType Description //
            //////////////////////////////////////////////////////////////////////////////////////////
            Dictionary<int, string> PtTypes = SqlServer_Impl.GetAutoCompletePointTypes().ToDictionary(mc => (int)mc.UID, mc => (string)mc.Description);

            /////////////////////////////
            // Build Projection Tables //
            /////////////////////////////
            _StdPrjPts Pts = new _StdPrjPts();

            var sw = new StreamWriter(ms, uniEncoding);
            try
            {
                ///////////////////////////////////////////////////
                // Write Styles To Help Format Excel Spreadsheet //
                ///////////////////////////////////////////////////
                sw.Write(BuildStyle());

                /////////////////////////
                // Start Summary Table //
                /////////////////////////
                sw.Write("<span style=\"font-size:1.7em; font-weight: bold\">Standardized Projected Points</span>");
                sw.WriteLine("<br/><br/>");



                ///////////////////////////
                // Summary Table Header  //
                ///////////////////////////
                sw.Write("<table class=\"tbl\">");
                sw.Write("<tr>");
                sw.Write("<th class=\"UnderlineRow\">Dept</th>");
                foreach (KeyValuePair<int, string> pair in PtTypes)
                    sw.Write("<th class=\"UnderlineRow\"  >" + pair.Value + " (" + pair.Key.ToString() + ")</th>");
                sw.Write("<th class=\"UnderlineRow\"  style=\"width: 40px; \">Total</th>");
                sw.Write("</tr>");

                /////////////////////////////////////////
                // Setup To Start Summary Table Detail //
                /////////////////////////////////////////
                int RowCnt = 4;
                int LastCol = 'B' + PtTypes.Count - 1;


                //////////////////////////////
                // Write Summary Table Data //
                //////////////////////////////
                foreach (var SumRptRcd in Pts.GetPrjSumDict())
                {
                    sw.Write("<tr class=\"tblRow\" style=\"text-align:center;\" >");
                    sw.Write("<td>" + ((SumRptRcd.Key.Length > 0) ? SumRptRcd.Key : "----") + "</td>");

                    foreach (double PtCnt in SumRptRcd.Value.Skip(1))
                        sw.Write("<td>" + ((PtCnt > 0) ? PtCnt.ToString() : "0") + "</td>");

                    sw.Write("<td>=SUM(B" + RowCnt.ToString() + ":" + (char)LastCol + RowCnt.ToString() + ")</td>");
                    sw.Write("</tr>");

                    RowCnt++;
                }
                sw.Write("</table>");
                sw.Write("<br/><br/>");


            
                ////////////////////
                // CLose The Page //
                ////////////////////
                sw.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                ShowExcel(ms);
            }
            finally
            {
                sw.Dispose();
            }
        }
    }
    protected void AssignPtsExportToExcelButton_Click(object sender, System.EventArgs e)
    {
        System.Text.UnicodeEncoding uniEncoding = new System.Text.UnicodeEncoding();

        using (MemoryStream ms = new MemoryStream())
        {
            var sw = new StreamWriter(ms, uniEncoding);
            try
            {
                ///////////////////////////////////////////////////
                // Write Styles To Help Format Excel Spreadsheet //
                ///////////////////////////////////////////////////
                sw.Write(BuildStyle());

                /////////////////////////
                // Start Summary Table //
                /////////////////////////
                sw.Write("<span style=\"font-size:1.7em; font-weight: bold\">Default Point Values Assigned By Type</span>");
                sw.WriteLine("<br/><br/>");


                ///////////////////////////
                // Summary Table Header  //
                ///////////////////////////
                sw.Write("<table class=\"tbl\">");
                sw.Write("<tr>");
                sw.Write("<th class=\"UnderlineRow\">Points Type</th>");
                sw.Write("<th class=\"UnderlineRow\">Points Value</th>");
                sw.Write("</tr>");


                foreach (var PT in  SqlServer_Impl.GetAutoCompletePointTypes() )
                {
                    sw.Write("<tr>");
                    sw.Write("<td>" + PT.Description + "</td>");
                    sw.Write("<td>" + PT.PointsCount.ToString() + "</td>");
                    sw.Write("</tr>");
                }

                sw.Write("</table>");
                sw.Write("<br/><br/>");



                ////////////////////
                // CLose The Page //
                ////////////////////
                sw.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                ShowExcel(ms);
            }
            finally
            {
                sw.Dispose();
            }
        }
    }


    private void ShowExcel(MemoryStream s)
    {
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=Points.xls");
        //Response.AddHeader("Content-Disposition", "attachment;filename=Points.htm");
        Response.OutputStream.Write(s.GetBuffer(), 0, s.GetBuffer().Length);
        Response.OutputStream.Flush();
        Response.OutputStream.Close();
        Response.End();
    }


    private string BuildStyle()
    {
        string Style = string.Empty;
        Style += "<style>";
        Style += "    .tbl";
        Style += "    {";
        Style += "	    border-width: 0px;";
        Style += "	    border-spacing: 0px;";
        Style += "	    border-style: hidden;";
        Style += "	    border-collapse: collapse;";
        Style += "	    background-color: white;";
        Style += "    }";
        Style += "    .tblRow {";
        Style += "	    border-width: 2px;";
        Style += "	    padding: 2px;";
        Style += "	    border-style: solid;";
        Style += "	    border-color: rgb(219, 217, 217);";
        Style += "    }";
        Style += "    .UnderlineRow";
        Style += "    {";
        Style += "	    border-style: solid;";
        Style += "	    border-color: rgb(219, 217, 217);";
        Style += "      border-bottom: 2px solid black !important;";
        Style += "	    text-align:center;";
        Style += "	    vertical-align: middle;";
        Style += "    }";
        Style += "    .SumRow";
        Style += "    {";
        Style += "      border: none !important;";
        Style += "      border-top: 2px solid black !important;";
        Style += "	    text-align:center;";
        Style += "    }";
        Style += "    .RightBorder";
        Style += "    {";
        Style += "        border-right: 2px solid black !important;";
        Style += "    }";
        Style += "</style>";

        return Style;
    }

}