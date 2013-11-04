using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using System.IO;
using ShermcoYou.DataTypes;

public partial class Safety_SafetyPays_TabPointsRpt : System.Web.UI.Page
{
    DateTime _start = DateTime.Parse("1/1/2013");
    DateTime _end = DateTime.Parse("2/1/2013");

    protected void Page_Load(object sender, EventArgs e)
    {
        //////////////////////////////////////////////////////////
        // But Unless This Is A New / Refresh Form, We Are Done //
        //////////////////////////////////////////////////////////
        if (IsPostBack)
        {
            _start = DateTime.Parse(StartDate.Value);
            _end = DateTime.Parse(EndDate.Value);
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

    protected void ByDeptExportToExcelButton_Click(object sender, EventArgs e)
    {
        System.Text.UnicodeEncoding uniEncoding = new System.Text.UnicodeEncoding();
        if (_end.Day == 1)
            _end = _end.AddDays(-1);

        using (MemoryStream ms = new MemoryStream())
        {

            Dictionary<int, string> months = new Dictionary<int, string>();                 // List Of Months  < 1, "1" >

            EmpByMon SumEmpPts;                                                             // Array [14] (One For Each Month  [0] For Sum 
            Dictionary<string, EmpByMon> sumEmpMon = new Dictionary<string, EmpByMon>();    // Sum By Emp  <"empNo",  Array[14] As Above
            
            
            var sw = new StreamWriter(ms, uniEncoding);
            int startRowCnt = 5;
            int endRowCnt = 5;
            try
            {
                /////////////////////////////////////////////////////////////
                // Get Employee Points Detail Records For Reporting Period //
                /////////////////////////////////////////////////////////////
                var data = SqlServer_Impl.GetAdminPointsRptEmpPoints(_start, _end).OrderBy("EmpDept");

                /////////////////////////////
                // Build Projection Tables //
                /////////////////////////////
                _PrjPts pts = new _PrjPts(_start, _end);

                ///////////////////////////////////////////////////////////
                // Build A Summary Array For Each Employee For Each Dept //
                //////////////////////////////////////////////////////////
                foreach (SIU_Points_Rpt rptRcd in data)
                {
                    if (sumEmpMon.ContainsKey(rptRcd.Emp_No))
                    {
                        SumEmpPts = sumEmpMon[rptRcd.Emp_No];
                        SumEmpPts.Months[rptRcd.EventDate.Month] += rptRcd.Points;
                        SumEmpPts.Months[13] += rptRcd.Points;
                    }
                    else
                    {
                        SumEmpPts = new EmpByMon();
                        SumEmpPts.Months[rptRcd.EventDate.Month] = rptRcd.Points;
                        SumEmpPts.Months[13] = rptRcd.Points;
                        sumEmpMon.Add(rptRcd.Emp_No, SumEmpPts);
                    }


                    if (!months.ContainsKey(rptRcd.EventDate.Month))
                        months.Add(rptRcd.EventDate.Month, rptRcd.EventDate.Month.ToString(CultureInfo.InvariantCulture));
                    months = months.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                }

                ///////////////////////////////////////////////////
                // Write Styles To Help Format Excel Spreadsheet //
                ///////////////////////////////////////////////////
                sw.Write( BuildStyle() );

                ///////////////////
                // Report Header //
                ///////////////////
                sw.Write("<span style=\"font-size:1.7em; font-weight: bold\">Points Calculated For " + _start.ToShortDateString() + " - " + _end.ToShortDateString() +  "</span>");
                sw.WriteLine("<br/>");

                string prevDept = "FIRSTRCD";
                string prevEmp = "FIRSTRCD";
                int deptSum = 0;


                ///////////////////////////////////////////////////////////////////
                // Walking Back Through The Data Of Emp Detail Rcds By Dept, Emp //
                ///////////////////////////////////////////////////////////////////
                foreach (SIU_Points_Rpt rptRcd in data)
                {
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // First Time We See An Emp -- Grab Summary Record From Above (sumEmpMon) And Produce A  Dept Detail Line //
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (prevEmp != rptRcd.Emp_No)
                    {
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // But If We Switched To A New Department -- Write Out Summary Row, Then New Deaprtment Header, Then Column Headers //
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        if (prevDept != rptRcd.EmpDept)
                        {
                            deptSum = Convert.ToInt32(pts.GetPrjSumForDept(rptRcd.EmpDept)[0]);


                            ////////////////////////////////
                            // Add Sum Row For Department //
                            ////////////////////////////////
                            if (prevDept != "FIRSTRCD")
                            {
                                ////////////////////////////////////////////////////////////////////////////////////////////////
                                // Get Projections For Department (Summed Across Reporting Period -- Applies To Each Employee //
                                ////////////////////////////////////////////////////////////////////////////////////////////////
                                //DeptSum = Convert.ToInt32(Pts.GetPrjSumForDept(RptRcd.EmpDept)[0]);

                                endRowCnt--;
                                sw.Write("<tr>");
                                sw.Write("<td class=\"SumRow\"></td>");
                                sw.Write("<td class=\"SumRow\">=SUM(B" + startRowCnt + ":B" + endRowCnt + ")</td>");  // Points Sum
                                sw.Write("<td class=\"SumRow\">=SUM(C" + startRowCnt + ":C" + endRowCnt + ")</td>");  // Prj Pts Sum

                                int col = 'D';
                                foreach (KeyValuePair<int, string> pair in months)
                                    sw.Write("<td class=\"SumRow\">=SUM(" + (char)col + startRowCnt + ":" + (char)col++ + endRowCnt + ")</td>");

                                sw.Write("<td class=\"SumRow\">=TEXT( (B" + (endRowCnt + 1) + "/C" + (endRowCnt + 1) + "),\"#%\")</td>");  // Pct Compliance

                                sw.Write("</tr>");
                                sw.Write("</table>");
                                startRowCnt = endRowCnt + 5;
                                endRowCnt = startRowCnt;
                            }

                            /////////////////////////////////
                            // Header For Next Deptartment //
                            /////////////////////////////////
                            sw.WriteLine("<br/>");
                            sw.Write("<span style=\"font-size:1.7em; font-weight: bold\">Department " + ((rptRcd.EmpDept.Length > 0) ? rptRcd.EmpDept : "Missing") + "</span>");

                            ///////////////////////////////////////////
                            // Start New Table -- Add Column Headers //
                            ///////////////////////////////////////////
                            sw.Write("<table border=\"0\">");
                            sw.Write("<tr border=\"0\"><th>Name</th><th>Points</th><th>Expected Points</th>");
                            foreach (KeyValuePair<int, string> pair in months)
                                sw.Write("<th>" +  CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName( pair.Key ) + "</th>");
                            sw.Write("<th>Pct of Exp</th>");


                            prevDept = rptRcd.EmpDept;
                        }

                        ///////////////////////////////////////
                        // Write Out Employee Summary Record //
                        ///////////////////////////////////////
                        SumEmpPts = sumEmpMon[rptRcd.Emp_No];

                        sw.Write("<tr style=\"text-align:center;\" >");
                        sw.Write("<td>" + rptRcd.EmpName + "</td>");
                        sw.Write("<td>" + SumEmpPts.Months[13] + "</td>");
                        sw.Write("<td>" + deptSum + "</td>");

                        foreach (KeyValuePair<int, string> pair in months)
                            sw.Write("<td>" + SumEmpPts.Months[pair.Key] + "</td>");
                        sw.Write("<td >=TEXT( (B" + endRowCnt + "/C" + endRowCnt + "),\"#%\")</td>");  // Pct Compliance
                        sw.Write("</tr>");

                        prevEmp = rptRcd.Emp_No;
                        endRowCnt++;
                    }
                }


                /////////////////////////////////////
                // Add Sum Row To Last Deptartment //
                /////////////////////////////////////
                if (prevDept != "FIRSTRCD")
                {
                    endRowCnt--;
                    sw.Write("<tr style=\"text-align:center;\" >");
                    sw.Write("<td class=\"SumRow\"></td>");
                    sw.Write("<td class=\"SumRow\">=SUM(B" + startRowCnt + ":B" + endRowCnt + ")</td>");
                    sw.Write("<td class=\"SumRow\">=SUM(C" + startRowCnt + ":C" + endRowCnt + ")</td>");  // Prj Pts Sum
                    int col = 'D';
                    foreach (KeyValuePair<int, string> pair in months)
                        sw.Write("<td class=\"SumRow\">=SUM(" + (char)col + startRowCnt + ":" + (char)col++ + endRowCnt + ")</td>");

                    sw.Write("<td class=\"SumRow\">=TEXT( (B" + (endRowCnt + 1) + "/C" + (endRowCnt + 1) + "),\"#%\")</td>");  // Pct Compliance
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
    protected void EmpDtlExportToExcelButton_Click(object sender, EventArgs e)
    {
        System.Text.UnicodeEncoding uniEncoding = new System.Text.UnicodeEncoding();
        if (_end.Day == 1)
            _end = _end.AddDays(-1);

        using (MemoryStream ms = new MemoryStream())
        {

            //////////////////////////////////////////////////////////////////////////////////////////
            // Buid Dict To Hold Freq Used Conversion of PointsType Index to PointsType Description //
            //////////////////////////////////////////////////////////////////////////////////////////
            Dictionary<int, string> ptTypes = SqlServer_Impl.GetAutoCompletePointTypes().ToDictionary(mc => mc.UID, mc => mc.Description);
            EmpDtl ed = new EmpDtl();
            ed.ReasonPts = new int[ptTypes.Count];
            SIU_Points_Rpt prevRcd = null;

            var sw = new StreamWriter(ms, uniEncoding);
            try
            {
                var data = SqlServer_Impl.GetAdminPointsRptEmpPoints(_start, _end);

                ///////////////////////////////////////////////////
                // Write Styles To Help Format Excel Spreadsheet //
                ///////////////////////////////////////////////////
                sw.Write( BuildStyle() );

                sw.Write("<span style=\"font-size:1.7em; font-weight: bold\">Points Calculated For " + _start.ToShortDateString() + " - " + _end.ToShortDateString() + "</span>");
                sw.WriteLine("<br/>");
                sw.WriteLine("<br/>");
                

                // Table Header
                sw.Write("<table class=\"tbl\">");
                sw.Write("<tr>");
                sw.Write("<td class=\"UnderlineRow\" style=\"width: 125px; \">Name</td>");
                sw.Write("<td class=\"UnderlineRow\" style=\"width: 45px; \">Dept</td>");
                sw.Write("<td class=\"UnderlineRow\" style=\"width: 60px; border-right: 2px solid black !important;\">Eligibility</td>");
                foreach (KeyValuePair<int, string> pair in ptTypes)
                    sw.Write("<td class=\"UnderlineRow\"  style=\"width: 65px; \">" + pair.Value + "</td>");
                sw.Write("<td class=\"UnderlineRow\"  style=\"width: 40px; \">Total</td>");
                sw.Write("</tr>");

                string prevEmp = "FIRSTRCD";
                int rowCnt = 4;


                //////////////////////////////////////////////////////////////////////////////////////////
                // The First Column Is Always D.  The Last Column Is Based On The Number Of Point Types //
                // Columns Are Base 26 (A - Z, AA - AZ, Ba - BZ, ... )                                  //
                // Calculate The Last Column So We Can Build A Sum Formula                              //
                //////////////////////////////////////////////////////////////////////////////////////////
                int lastColP1 = 0;
                if (ptTypes.Count > 23)
                    lastColP1 = ((ptTypes.Count + 3) / 26);      // Convert to Base 26 (A - Z) Skipping First 3 Columns

                int lastColP2 = (ptTypes.Count % 26) - (lastColP1 * 26);
                lastColP2 += 'D' - 1;

                string lastCol = "";
                if (lastColP1 > 0)
                {
                    lastColP1--;
                    lastColP1 += 'A';                      // Convert to Alpha
                    lastCol = ((char)lastColP1).ToString(CultureInfo.InvariantCulture);
                }
                lastCol += ((char)lastColP2).ToString(CultureInfo.InvariantCulture);

                ////////////////////////////////
                // Walk Through Eash Data Row //
                ////////////////////////////////
                foreach (SIU_Points_Rpt rptRcd in data)
                {
                    /////////////////////////////////////////////////////////////////////////////
                    // For Each Data Record For A Given Employee, Write The Cell To The  Table //
                    /////////////////////////////////////////////////////////////////////////////
                    if (prevEmp != rptRcd.Emp_No && prevEmp != "FIRSTRCD")
                    {
                        sw.Write("<tr class=\"tblRow\" style=\"text-align:center;\" >");
                        sw.Write("<td>" + prevRcd.EmpName + "</td>");
                        sw.Write("<td>" + ((prevRcd.EmpDept.Length > 0) ? prevRcd.EmpDept : "----") + "</td>");
                        sw.Write("<td>" + "-" + "</td>");

                        foreach (var ptCnt in ed.ReasonPts)
                            sw.Write("<td>" +   (( ptCnt > 0 ) ? ptCnt.ToString(CultureInfo.InvariantCulture) : "") + "</td>");

                        sw.Write("<td>=SUM(D" + rowCnt + ":" + lastCol + rowCnt + ")</td>");


                        sw.Write("</tr>");

                        rowCnt++;
                        prevEmp = rptRcd.Emp_No;
                        ed.ReasonPts = new int[ptTypes.Count];
                    }


                    
                    ed.ReasonPts[rptRcd.ReasonForPoints - 1] += rptRcd.Points;
                    prevEmp = rptRcd.Emp_No;
                    prevRcd = rptRcd;
                }

                ///////////////////
                // Last Employee //
                ///////////////////
                sw.Write("<tr class=\"tblRow\" style=\"text-align:center;\" >");
                sw.Write("<td>" + prevRcd.EmpName + "</td>");
                sw.Write("<td>" + ((prevRcd.EmpDept.Length > 0) ? prevRcd.EmpDept : "----") + "</td>");
                sw.Write("<td>" + "-" + "</td>");

                foreach (var ptCnt in ed.ReasonPts)
                    sw.Write("<td>" + ((ptCnt > 0) ? ptCnt.ToString(CultureInfo.InvariantCulture) : "") + "</td>");

                sw.Write("<td>=SUM(D" + rowCnt + ":" + lastCol + rowCnt + ")</td>");

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
    protected void PrjPtsExportToExcelButton_Click(object sender, EventArgs e)
    {
        System.Text.UnicodeEncoding uniEncoding = new System.Text.UnicodeEncoding();
        if (_end.Day == 1)
            _end = _end.AddDays(-1);

        using (MemoryStream ms = new MemoryStream())
        {

            //////////////////////////////////////////////////////////////////////////////////////////
            // Buid Dict To Hold Freq Used Conversion of PointsType Index to PointsType Description //
            //////////////////////////////////////////////////////////////////////////////////////////
            Dictionary<int, string> ptTypes = SqlServer_Impl.GetAutoCompletePointTypes().ToDictionary(mc => mc.UID, mc => mc.Description);

            /////////////////////////////
            // Build Projection Tables //
            /////////////////////////////
            _PrjPts pts = new _PrjPts(_start, _end);

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
                sw.Write("<span style=\"font-size:1.7em; font-weight: bold\">Projected Points Calculation For " + _start.ToShortDateString() + " - " + _end.ToShortDateString() + "</span>");
                sw.WriteLine("<br/><br/>");



                ///////////////////////////
                // Summary Table Header  //
                ///////////////////////////
                sw.Write("<table class=\"tbl\">");
                sw.Write("<tr>");
                sw.Write("<th class=\"UnderlineRow\">Dept</th>");
                foreach (KeyValuePair<int, string> pair in ptTypes)
                    sw.Write("<th class=\"UnderlineRow\"  >" + pair.Value + " (" + pair.Key + ")</th>");
                sw.Write("<th class=\"UnderlineRow\"  style=\"width: 40px; \">Total</th>");
                sw.Write("</tr>");

                /////////////////////////////////////////
                // Setup To Start Summary Table Detail //
                /////////////////////////////////////////
                int rowCnt = 4;
                int lastCol = 'B' + ptTypes.Count - 1;


                //////////////////////////////
                // Write Summary Table Data //
                //////////////////////////////
                foreach (var sumRptRcd in pts.GetPrjSumDict())
                {
                    sw.Write("<tr class=\"tblRow\" style=\"text-align:center;\" >");
                    sw.Write("<td>" + ((sumRptRcd.Key.Length > 0) ? sumRptRcd.Key : "----") + "</td>");

                    foreach (double ptCnt in sumRptRcd.Value.Skip(1) )
                        sw.Write("<td>" + ((ptCnt > 0) ? ptCnt.ToString(CultureInfo.InvariantCulture) : "0") + "</td>");

                    sw.Write("<td>=SUM(B" + rowCnt + ":" + (char)lastCol + rowCnt + ")</td>");
                    sw.Write("</tr>");

                    rowCnt++;
                }
                sw.Write("</table>");
                sw.Write("<br/><br/>");


//////////////////////////
// Start Monthly Tables //
//////////////////////////

                ///////////////////////////////////////////
                // Get List Of Months Included In Report //
                ///////////////////////////////////////////
                foreach (var rptMonths in pts.GetDatesDict())
                {
                    /////////////////////////////
                    // Adjust Detail Row Index //
                    /////////////////////////////
                    rowCnt += 4;

                    //////////////////////////
                    // Monthly Table Header //
                    //////////////////////////
                    sw.Write("<span style=\"font-size:1.7em; font-weight: bold\">Projected Points Calculation For " + rptMonths.Key + "</span>");

                    //////////////////////////////////
                    // Monthly Table Column Headers //
                    //////////////////////////////////
                    sw.Write("<table class=\"tbl\">");
                    sw.Write("<tr>");
                    sw.Write("<td class=\"UnderlineRow\" >Dept</td>");
                    foreach (KeyValuePair<int, string> pair in ptTypes)
                        sw.Write("<td class=\"UnderlineRow\"  >" + pair.Value + " (" + pair.Key + ")</td>");
                    sw.Write("<td class=\"UnderlineRow\"  style=\"width: 40px; \">Total</td>");
                    sw.Write("</tr>");

                    //////////////////////////////
                    // Write Monthly Table Data //
                    //////////////////////////////
                    foreach (var monRptRcd in pts.GetMonthlyDict(rptMonths.Value))
                    {
                        sw.Write("<tr class=\"tblRow\" style=\"text-align:center;\" >");
                        sw.Write("<td>" + ((monRptRcd.Key.Length > 0) ? monRptRcd.Key : "----") + "</td>");

                        foreach (double ptCnt in monRptRcd.Value.Skip(1))
                            sw.Write("<td>" + ((ptCnt > 0) ? ptCnt.ToString(CultureInfo.InvariantCulture) : "0") + "</td>");

                        sw.Write("<td>=SUM(B" + rowCnt + ":" + (char)lastCol + rowCnt + ")</td>");
                        sw.Write("</tr>");

                        rowCnt++;
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

    protected void StdPrjPtsExportToExcelButton_Click(object sender, EventArgs e)
    {
        System.Text.UnicodeEncoding uniEncoding = new System.Text.UnicodeEncoding();

        using (MemoryStream ms = new MemoryStream())
        {

            //////////////////////////////////////////////////////////////////////////////////////////
            // Buid Dict To Hold Freq Used Conversion of PointsType Index to PointsType Description //
            //////////////////////////////////////////////////////////////////////////////////////////
            Dictionary<int, string> ptTypes = SqlServer_Impl.GetAutoCompletePointTypes().ToDictionary(mc => mc.UID, mc => mc.Description);

            /////////////////////////////
            // Build Projection Tables //
            /////////////////////////////
            _StdPrjPts pts = new _StdPrjPts();

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
                foreach (KeyValuePair<int, string> pair in ptTypes)
                    sw.Write("<th class=\"UnderlineRow\"  >" + pair.Value + " (" + pair.Key + ")</th>");
                sw.Write("<th class=\"UnderlineRow\"  style=\"width: 40px; \">Total</th>");
                sw.Write("</tr>");

                /////////////////////////////////////////
                // Setup To Start Summary Table Detail //
                /////////////////////////////////////////
                int rowCnt = 4;
                int lastCol = 'B' + ptTypes.Count - 1;


                //////////////////////////////
                // Write Summary Table Data //
                //////////////////////////////
                foreach (var sumRptRcd in pts.GetPrjSumDict())
                {
                    sw.Write("<tr class=\"tblRow\" style=\"text-align:center;\" >");
                    sw.Write("<td>" + ((sumRptRcd.Key.Length > 0) ? sumRptRcd.Key : "----") + "</td>");

                    foreach (double ptCnt in sumRptRcd.Value.Skip(1))
                        sw.Write("<td>" + ((ptCnt > 0) ? ptCnt.ToString(CultureInfo.InvariantCulture) : "0") + "</td>");

                    sw.Write("<td>=SUM(B" + rowCnt + ":" + (char)lastCol + rowCnt + ")</td>");
                    sw.Write("</tr>");

                    rowCnt++;
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
    protected void AssignPtsExportToExcelButton_Click(object sender, EventArgs e)
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


                foreach (var pt in  SqlServer_Impl.GetAutoCompletePointTypes() )
                {
                    sw.Write("<tr>");
                    sw.Write("<td>" + pt.Description + "</td>");
                    sw.Write("<td>" + pt.PointsCount + "</td>");
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
        string style = string.Empty;
        style += "<style>";
        style += "    .tbl";
        style += "    {";
        style += "	    border-width: 0px;";
        style += "	    border-spacing: 0px;";
        style += "	    border-style: hidden;";
        style += "	    border-collapse: collapse;";
        style += "	    background-color: white;";
        style += "    }";
        style += "    .tblRow {";
        style += "	    border-width: 2px;";
        style += "	    padding: 2px;";
        style += "	    border-style: solid;";
        style += "	    border-color: rgb(219, 217, 217);";
        style += "    }";
        style += "    .UnderlineRow";
        style += "    {";
        style += "	    border-style: solid;";
        style += "	    border-color: rgb(219, 217, 217);";
        style += "      border-bottom: 2px solid black !important;";
        style += "	    text-align:center;";
        style += "	    vertical-align: middle;";
        style += "    }";
        style += "    .SumRow";
        style += "    {";
        style += "      border: none !important;";
        style += "      border-top: 2px solid black !important;";
        style += "	    text-align:center;";
        style += "    }";
        style += "    .RightBorder";
        style += "    {";
        style += "        border-right: 2px solid black !important;";
        style += "    }";
        style += "</style>";

        return style;
    }

}