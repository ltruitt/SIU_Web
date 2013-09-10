<%@ Language=VBScript %>
<%
    Dim bFirst as boolean
  ' Check for a value passed on the address bar.
  if (Request.QueryString("i")) = "" then bFirst = true
  ' If we have a value for "i", we know that we can display the
  ' data in Excel.
  if (bFirst = false) then
    ' Buffer the content and send it to Excel.
    Response.Buffer = true
    Response.ContentType = "application/vnd.ms-excel" 
%>
<HTML xmlns:x="urn:schemas-microsoft-com:office:excel">
<HEAD>
<style>
  <!--table
  @page
     {mso-header-data:"&CMultiplication Table\000ADate\: &D\000APage &P";
mso-page-orientation:landscape;}
     br
     {mso-data-placement:same-cell;}

  -->
</style>
  <!--[if gte mso 9]><xml>
   <x:ExcelWorkbook>
    <x:ExcelWorksheets>
     <x:ExcelWorksheet>
      <x:Name>Sample Workbook</x:Name>
      <x:WorksheetOptions>
       <x:Print>
        <x:ValidPrinterInfo/>
       </x:Print>
      </x:WorksheetOptions>
     </x:ExcelWorksheet>
    </x:ExcelWorksheets>
   </x:ExcelWorkbook>
  </xml><![endif]--> 
</HEAD>
<BODY>
<TABLE>
<%
   ' Build a multiplication table from 1,1 to i,j.
   for i = 1 to CInt(Request.QueryString("i"))
     Response.Write ("  <TR>" + vbCrLf)
     for j = 1 to CInt(Request.QueryString("j"))
       if (j = 1) or (i = 1) then
         Response.Write ("    <TD bgcolor=""#FFF8DC"">")
       else
         Response.Write ("    <TD bgcolor=""#B0C4DE"">")
       end if
   Response.Write (CStr(i*j) + "</TD>" + vbCrLf)
     next
     Response.Write ("  </TR>" + vbCrLf)
   next
%>
</BODY>
</HTML>
<%
  else
  ' The user hasn't loaded the page yet. Prompt them for 
  ' values for the table.
%>
<HTML>
<BODY>
Please enter indices for the multiplication table:<BR>
<FORM action="/Corporate/BandC/testExcel.aspx" method=GET>  
  i = <INPUT type="text" name=i style="WIDTH: 25px"><BR>
  j = <INPUT type="text" name=j style="WIDTH: 25px"><BR><BR/>
  <INPUT type="submit" value="Submit"><BR/>
</FORM>
</BODY>
</HTML>
<%
  end if
%>