using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using System.IO;
using ShermcoYou.DataTypes;


using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

using SpreadsheetLight;
using SpreadsheetLight.Charts;


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
                sw.Write("<td class=\"UnderlineRow\" style=\"width: 165px; \">Name</td>");
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
                        sw.Write("<td>(" + prevRcd.Emp_No + ") " + prevRcd.EmpName + "</td>");
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
                // C'ose The Page //
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


    protected void Consolidated_Click(object sender, EventArgs e)
    {
        //////////////////////////////////
        // Build The Document Framework //
        //////////////////////////////////
        SLDocument sl = new SLDocument
        {
            DocumentProperties =
            {
                Creator = "Larry Truitt",
                ContentStatus = "Generated " + DateTime.Now,
                Title = "Safety Pays Tabulations",
                Description = "Safety Pays Tracking Report generated by SiYOU.Shermco.Com"
            }
        };        

        //////////////////////
        // Setup The Sheets //
        //////////////////////
        sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, "By Dept");
        sl.AddWorksheet("By Emp");
        sl.AddWorksheet("Bar Graph");

        sl.AddWorksheet("Prj Pts Calc");
        sl.AddWorksheet("Std Prj Pts");
        sl.AddWorksheet("Pts by Type");

        ///////////////////////////////////////////
        // Build The Incident Accident Worksheet //
        ///////////////////////////////////////////
        sl.SelectWorksheet("By Dept");
        BuildDeptData(ref sl);

        sl.SelectWorksheet("By Emp");
        BuildEmpDtlData(ref sl);

        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("Content-Disposition", "attachment; filename=spPoints.xlsx");
        sl.SaveAs(Response.OutputStream);
        Response.End();
    }

    private void BuildDeptData(ref SLDocument sl)
    {
        Dictionary<int, string> months = new Dictionary<int, string>();                 // List Of Months  < 1, "1" >

        EmpByMon sumEmpPts;                                                             // Array [14] (One For Each Month  [0] For Sum 
        Dictionary<string, EmpByMon> sumEmpMon = new Dictionary<string, EmpByMon>();    // Sum By Emp  <"empNo",  Array[14] As Above

        int startRowCnt = 1;
        int endRowCnt = 1;

        /////////////////////////
        // Build Sheet Outline //
        /////////////////////////
        BuildDeptPage(ref sl);

        if (_end.Day == 1)
            _end = _end.AddDays(-1);

        /////////////////////////////////////////////////////////////
        // Get Employee Points Detail Records For Reporting Period //
        /////////////////////////////////////////////////////////////
        var data = SqlServer_Impl.GetAdminPointsRptEmpPointsFromProd(_start, _end).OrderBy("EmpDept");

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
                sumEmpPts = sumEmpMon[rptRcd.Emp_No];
                sumEmpPts.Months[rptRcd.EventDate.Month] += rptRcd.Points;
                sumEmpPts.Months[13] += rptRcd.Points;
            }
            else
            {
                sumEmpPts = new EmpByMon();
                sumEmpPts.Months[rptRcd.EventDate.Month] = rptRcd.Points;
                sumEmpPts.Months[13] = rptRcd.Points;
                sumEmpMon.Add(rptRcd.Emp_No, sumEmpPts);
            }


            if (!months.ContainsKey(rptRcd.EventDate.Month))
                months.Add(rptRcd.EventDate.Month, rptRcd.EventDate.Month.ToString(CultureInfo.InvariantCulture));
            months = months.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

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
                        ////////////////////////////////
                        // Build Summary Row For Dept //
                        ////////////////////////////////
                        BuildDeptSum(ref sl, startRowCnt, endRowCnt, ref months);

                        //////////////////////////////////
                        // Skip 2 Rows Before Next Dept //
                        //////////////////////////////////
                        startRowCnt = endRowCnt + 2;
                        endRowCnt = startRowCnt;
                    }

                    /////////////////////////////////
                    // Header For Next Deptartment //
                    /////////////////////////////////
                    BuildDeptHeader(ref sl, endRowCnt,  rptRcd);
                    endRowCnt++;

                    //////////////////////////
                    // Table Column Headers //
                    //////////////////////////
                    BuildDeptColumnHeader(ref sl, endRowCnt, ref months);
                    endRowCnt++;

                    prevDept = rptRcd.EmpDept;
                }

                ///////////////////////////////////////
                // Write Out Employee Summary Record //
                ///////////////////////////////////////
                sumEmpPts = sumEmpMon[rptRcd.Emp_No];

                BuildDeptDtl(ref sl, endRowCnt, rptRcd.EmpName, ref sumEmpPts, deptSum, ref months);

                prevEmp = rptRcd.Emp_No;
                endRowCnt++;
            }
        }

        /////////////////////////////////////
        // Build Summary Row For Last Dept //
        /////////////////////////////////////
        BuildDeptSum(ref sl, startRowCnt, endRowCnt, ref months);
    }

    private void BuildDeptPage(ref SLDocument sl)
    {
        SLPageSettings ps = new SLPageSettings();

        SLFont ft = sl.CreateFont();
        ft.SetFont("Impact", 16);
        ps.AppendOddHeader(ft, "Points Calculated For " + _start.ToShortDateString() + " - " + _end.ToShortDateString());
        ps.AppendEvenHeader(ft, "Points Calculated For " + _start.ToShortDateString() + " - " + _end.ToShortDateString());

        ps.AppendOddFooter(ft, "page ");
        ps.AppendOddFooter(SLHeaderFooterFormatCodeValues.PageNumber);
        ps.AppendOddFooter(" of ");
        ps.AppendOddFooter(SLHeaderFooterFormatCodeValues.NumberOfPages);

        ps.AppendEvenFooter(ft, "page ");
        ps.AppendEvenFooter(SLHeaderFooterFormatCodeValues.PageNumber);
        ps.AppendEvenFooter(" of ");
        ps.AppendEvenFooter(SLHeaderFooterFormatCodeValues.NumberOfPages);

        //ps.View = SheetViewValues.PageLayout;

        sl.SetPageSettings(ps);


        sl.SetColumnWidth("A", 20);
        sl.SetColumnWidth("B", 9);
        sl.SetColumnWidth("C", 9);
    }
    private void BuildDeptHeader(ref SLDocument sl, int Row, SIU_Points_Rpt rptRcd)
    {
        SLStyle styleDeptHeader = sl.CreateStyle();
        styleDeptHeader.Font.FontName = "Calibri";
        styleDeptHeader.Font.FontSize = 18;
        styleDeptHeader.Font.Bold = true;

        sl.MergeWorksheetCells(Row, 1, Row, 2);
        sl.SetCellValue(Row, 1, "Department " + ((rptRcd.EmpDept.Length > 0) ? rptRcd.EmpDept : "Missing"));
        sl.SetCellStyle(Row, 1, styleDeptHeader);

        sl.MergeWorksheetCells(Row, 3, Row, 7);        
    }
    private void BuildDeptColumnHeader(ref SLDocument sl, int Row, ref Dictionary<int, string> months)
    {
        SLStyle styleRowHeader = sl.CreateStyle();
        styleRowHeader.Font.FontName = "Calibri";
        styleRowHeader.Font.FontSize = 11;
        styleRowHeader.Font.Bold = true;
        styleRowHeader.Alignment.Horizontal = HorizontalAlignmentValues.Center;
        styleRowHeader.SetWrapText(true);

        //////////////////////////////////////////
        // Start New Dept -- Add Column Headers //
        //////////////////////////////////////////                        
        sl.SetCellValue(Row, 1, "Name");
        sl.SetCellValue(Row, 2, "Points");
        sl.SetCellValue(Row, 3, "Planned Points");

        int col = 4;
        foreach (KeyValuePair<int, string> pair in months)
        {
            sl.SetColumnWidth(col, 11);
            sl.SetCellValue(Row, col++, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(pair.Key));
        }
        sl.SetCellValue(Row, col, "Plan Pct");

        sl.SetCellStyle(Row, 1, Row, col, styleRowHeader);        
    }
    private void BuildDeptSum(ref SLDocument sl, int startRowCnt, int endRowCnt, ref Dictionary<int, string> months)
    {
        int col = 0;

        int deptHeadRow = startRowCnt;

        SLStyle styleRowSum = sl.CreateStyle();
        styleRowSum.Font.FontName = "Calibri";
        styleRowSum.Font.FontSize = 12;
        styleRowSum.Font.Bold = true;
        styleRowSum.Font.FontColor = System.Drawing.Color.White;
        styleRowSum.Fill.SetPatternType(PatternValues.Solid);
        styleRowSum.Fill.SetPatternForegroundColor(System.Drawing.Color.Black);
        styleRowSum.Alignment.Horizontal = HorizontalAlignmentValues.Center;

        ////////////////////////////////////////////////////////////////////////////////////////////////
        // Get Projections For Department (Summed Across Reporting Period -- Applies To Each Employee //
        ////////////////////////////////////////////////////////////////////////////////////////////////

        sl.SetCellValue(endRowCnt, 1, "Total");

        ////////////////////////////////
        // Sum Column Points For Dept //
        ////////////////////////////////
        string s1 = string.Format("=SUM({0})", SLConvert.ToCellRange(startRowCnt + 2, 2, endRowCnt - 1, 2));
        sl.SetCellValue(endRowCnt, 2, s1);

        ///////////////////////////////////////
        // Sum Column Panned Points for Dept //
        ///////////////////////////////////////
        string s2 = string.Format("=SUM({0})", SLConvert.ToCellRange(startRowCnt + 2, 3, endRowCnt - 1, 3));
        sl.SetCellValue(endRowCnt, 3, s2);

        ////////////////////////////////////////////
        // Sum The By-Point-Type Columns For Dept //
        ////////////////////////////////////////////
        col = 4;
        foreach (KeyValuePair<int, string> pair in months)
        {
            string s3 = string.Format("=SUM({0})", SLConvert.ToCellRange(startRowCnt + 2, col, endRowCnt - 1, col));
            sl.SetCellValue(endRowCnt, col++, s3);
        }

        /////////////////////////////////
        // Formula For % Plan for Dept //
        /////////////////////////////////
        string s4 = "=TEXT( (B" + (endRowCnt) + "/C" + (endRowCnt) + "),\"#%\")";
        s4 = "=(B" + endRowCnt + "/C" + endRowCnt + ") * 100";
        sl.SetCellValue(endRowCnt, col, s4);

        ////////////////////////
        // Format The Sum Row //
        ////////////////////////
        sl.SetCellStyle(endRowCnt, 1, endRowCnt, col, styleRowSum);

        ///////////////////////////////////////////
        // Create Collapsed Group Of Detail Rows //
        ///////////////////////////////////////////
        sl.GroupRows(deptHeadRow + 1, endRowCnt - 1);
        sl.CollapseRows(endRowCnt);


        ////////////////////////////////////
        // Add Dept Pct of Plan Bar Chart //
        ////////////////////////////////////
        string s6 = "=G" + endRowCnt;
        sl.SetCellValue(deptHeadRow, 3, s6);

        SLConditionalFormatting cf = new SLConditionalFormatting(deptHeadRow, 3, deptHeadRow, 3);
        cf.SetCustomDataBar(true, 0, 100,
            SLConditionalFormatMinMaxValues.Number, "0",
            SLConditionalFormatMinMaxValues.Number, "100",
            System.Drawing.Color.Blue);
        sl.AddConditionalFormatting(cf);    
    }
    private void BuildDeptDtl(ref SLDocument sl, int Row, string EmpName, ref EmpByMon sumEmpPts, int deptSum, ref Dictionary<int, string> months)
    {
        SLStyle styleRow = sl.CreateStyle();
        styleRow.Font.FontName = "Calibri";
        styleRow.Font.FontSize = 11;
        styleRow.Alignment.Horizontal = HorizontalAlignmentValues.Center;

        sl.SetCellValue(Row, 1, EmpName);
        sl.SetCellValue(Row, 2, sumEmpPts.Months[13]);
        sl.SetCellValue(Row, 3, deptSum);

        int col = 4;
        foreach (KeyValuePair<int, string> pair in months)
            sl.SetCellValue(Row, col++, sumEmpPts.Months[pair.Key]);

        string s5 = "=TEXT( (B" + (Row) + "/C" + (Row) + "),\"#%\")";
        s5 = "=(B" + Row + "/C" + Row + ") * 100";
        sl.SetCellValue(Row, col, s5);
        sl.SetCellStyle(Row, 2, Row, col, styleRow);

        ///////////////////////////////////////////////
        // Add Pct of Plan Bar Chart Into Line Items //
        ///////////////////////////////////////////////
        SLConditionalFormatting cf = new SLConditionalFormatting(Row, 7, Row, 7);
        cf.SetCustomDataBar(false, 0, 100,
            SLConditionalFormatMinMaxValues.Number, "0",
            SLConditionalFormatMinMaxValues.Number, "100",
            System.Drawing.Color.Blue);
        sl.AddConditionalFormatting(cf);        
    }

    private void BuildEmpPage(ref SLDocument sl)
    {
        SLPageSettings ps = new SLPageSettings();

        SLFont ft = sl.CreateFont();
        ft.SetFont("Impact", 16);
        ps.AppendOddHeader(ft, "Points Calculated For " + _start.ToShortDateString() + " - " + _end.ToShortDateString());
        ps.AppendEvenHeader(ft, "Points Calculated For " + _start.ToShortDateString() + " - " + _end.ToShortDateString());

        ps.AppendOddFooter(ft, "page ");
        ps.AppendOddFooter(SLHeaderFooterFormatCodeValues.PageNumber);
        ps.AppendOddFooter(" of ");
        ps.AppendOddFooter(SLHeaderFooterFormatCodeValues.NumberOfPages);

        ps.AppendEvenFooter(ft, "page ");
        ps.AppendEvenFooter(SLHeaderFooterFormatCodeValues.PageNumber);
        ps.AppendEvenFooter(" of ");
        ps.AppendEvenFooter(SLHeaderFooterFormatCodeValues.NumberOfPages);

        sl.SetColumnWidth("A", 25);
    }
    private void BuildEmpColumnHeader(ref SLDocument sl, int Row)
    {
        SLStyle styleRowHeader = sl.CreateStyle();
        styleRowHeader.Font.FontName = "Calibri";
        styleRowHeader.Font.FontSize = 11;
        styleRowHeader.Font.Bold = true;
        styleRowHeader.Alignment.Horizontal = HorizontalAlignmentValues.Center;
        styleRowHeader.Alignment.Vertical = VerticalAlignmentValues.Bottom;
        styleRowHeader.SetWrapText(true);
        styleRowHeader.Border.BottomBorder.BorderStyle = BorderStyleValues.Medium;
        styleRowHeader.Border.RightBorder.Color = System.Drawing.Color.Black;

        SLStyle styleCellRBorder = sl.CreateStyle();
        styleCellRBorder.Border.RightBorder.BorderStyle = BorderStyleValues.Medium;
        styleCellRBorder.Border.RightBorder.Color = System.Drawing.Color.Black;
        
        //////////////////////////////////////////
        // Start New Dept -- Add Column Headers //
        //////////////////////////////////////////                        
        sl.SetCellValue(Row, 1, "Name");
        sl.SetCellValue(Row, 2, "Dept");
        sl.SetCellValue(Row, 3, "Eligibility");

        int col = 4;
        Dictionary<int, string> ptTypes = SqlServer_Impl.GetAutoCompletePointTypes().ToDictionary(mc => mc.UID, mc => mc.Description);
        foreach (KeyValuePair<int, string> pair in ptTypes)
        {
            sl.SetColumnWidth(col, 11);
            sl.SetCellValue(Row, col++, pair.Value);
        }
        sl.SetCellValue(Row, col, "Total");

        sl.SetCellStyle(Row, 1, Row, col, styleRowHeader);
        sl.SetCellStyle(Row, 3, styleCellRBorder);

        ///////////////////////////
        // Freeze the top 2 rows //
        ///////////////////////////
        sl.FreezePanes(1, 1);
    }
    private void BuildEmpDtl(ref SLDocument sl, int row, string EmpName, string EmpDept,  ref EmpDtl SumPtByType )
    {
        SLStyle styleRow = sl.CreateStyle();
        styleRow.Font.FontName = "Calibri";
        styleRow.Font.FontSize = 11;
        styleRow.Alignment.Horizontal = HorizontalAlignmentValues.Center;

        sl.SetCellValue(row, 1, EmpName);
        sl.SetCellValue(row, 2, EmpDept);
        sl.SetCellValue(row, 3, "-");

        int col = 4;
        foreach (var ptCnt in SumPtByType.ReasonPts)
            sl.SetCellValueNumeric(row, col++, ((ptCnt > 0) ? ptCnt.ToString(CultureInfo.InvariantCulture) : ""));

        string s1 = "=SUM(" + SLConvert.ToCellRange(row, 4, row, col - 1) + ")";
        sl.SetCellValue(row, col, s1);

        sl.SetCellStyle(row, 2, row, col, styleRow);        
    }
    private void BuildEmpSum(ref SLDocument sl, int startRowCnt, int endRowCnt, int startCol, int endCol, string totTxt)
    {
        SLStyle styleRowSum = sl.CreateStyle();
        styleRowSum.Font.FontName = "Calibri";
        styleRowSum.Font.FontSize = 12;
        styleRowSum.Font.Bold = true;
        styleRowSum.Font.FontColor = System.Drawing.Color.White;
        styleRowSum.Fill.SetPatternType(PatternValues.Solid);
        styleRowSum.Fill.SetPatternForegroundColor(System.Drawing.Color.Black);
        styleRowSum.Alignment.Horizontal = HorizontalAlignmentValues.Center;

        sl.SetCellValue(endRowCnt + 1, 1, totTxt);

        ///////////////////////
        // Sum Column Points //
        ///////////////////////
        for (int col = startCol; col < endCol; col++ )
        {
            string s1 = "=SUM(" + SLConvert.ToCellRange(startRowCnt, col, endRowCnt, col) + ")";
            sl.SetCellValue(endRowCnt + 1, col, s1);
        }

        ////////////////////////
        // Format The Sum Row //
        ////////////////////////
        sl.SetCellStyle(endRowCnt + 1, 1, endRowCnt + 1, endCol, styleRowSum);

        ///////////////////////////////////////////
        // Create Collapsed Group Of Detail Rows //
        ///////////////////////////////////////////
        sl.GroupRows(startRowCnt, endRowCnt);
    }



    protected void BuildEmpDtlData(ref SLDocument sl)
    {
        System.Text.UnicodeEncoding uniEncoding = new System.Text.UnicodeEncoding();
        if (_end.Day == 1)
            _end = _end.AddDays(-1);

        //////////////////////////////////////////////////////////////////////////////////////////
        // Buid Dict To Hold Freq Used Conversion of PointsType Index to PointsType Description //
        //////////////////////////////////////////////////////////////////////////////////////////
        Dictionary<int, string> ptTypes = SqlServer_Impl.GetAutoCompletePointTypes().ToDictionary(mc => mc.UID, mc => mc.Description);
        EmpDtl ed = new EmpDtl {ReasonPts = new int[ptTypes.Count]};
        SIU_Points_Rpt prevRcd = new SIU_Points_Rpt {Emp_No = "FIRSTRCD"};

        var data = SqlServer_Impl.GetAdminPointsRptEmpPoints(_start, _end);

        BuildEmpPage(ref sl);
        BuildEmpColumnHeader(ref sl, 1);

        int row = 2;
        int DeptStartRow = 2;

        ////////////////////////////////
        // Walk Through Eash Data Row //
        ////////////////////////////////
        foreach (SIU_Points_Rpt rptRcd in data)
        {
            /////////////////////////////////////////////////////////////////////////////
            // For Each Data Record For A Given Employee, Write The Cell To The  Table //
            /////////////////////////////////////////////////////////////////////////////
            if (prevRcd.Emp_No != rptRcd.Emp_No && prevRcd.Emp_No != "FIRSTRCD")
            {
                BuildEmpDtl(ref sl, row++, "(" + prevRcd.Emp_No + ") " + prevRcd.EmpName, prevRcd.EmpDept, ref ed);
                ed.ReasonPts = new int[ptTypes.Count];

                if (prevRcd.EmpDept != rptRcd.EmpDept)
                {
                    BuildEmpSum(ref sl, DeptStartRow, row - 1, 4, ptTypes.Count + 5, prevRcd.EmpDept + " Totals");
                    row += 2;
                    DeptStartRow = row;
                }
            }


            ed.ReasonPts[rptRcd.ReasonForPoints - 1] += rptRcd.Points;
            prevRcd = rptRcd;
        }
        BuildEmpDtl(ref sl, row, "(" + prevRcd.Emp_No + ") " + prevRcd.EmpName, prevRcd.EmpDept, ref ed);
        BuildEmpSum(ref sl, DeptStartRow, row - 1, 4, ptTypes.Count + 5, prevRcd.EmpDept + " Totals");

    }










    private void ShowExcel(MemoryStream s)
    {
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=Points.xls");
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