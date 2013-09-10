<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="MyTimeStudy.aspx.cs" Inherits="Reporting_TimeReporting" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script src="/Scripts/Highcharts-2.3.5/js/highcharts.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts-2.3.5/js/modules/exporting.js" type="text/javascript"></script> 
    <script src="/Scripts/MyTimeStudy.js" type="text/javascript"></script>   
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <section style="visibility: hidden; height: 0; width: 0;">
        <span id="hlblEID"           runat="server"/>  
    </section>    
    
    <div id="SuprArea" style="margin-top: 5px;" runat="server" >
        <span style="float:left; margin-right: 20px; width: auto;  font-weight: bold; display:inline">Change Employee:</span>
        <input ID="ddEmpIds" class="DataInputCss" runat="server"  style="width: 400px;" />         
    </div>

    
    <div id="PrevMonthDaily" style="-moz-min-width: 400px; -ms-min-width: 400px; -o-min-width: 400px; -webkit-min-width: 400px; min-width: 400px; height: 200px; margin: 0 auto"></div>
    <div id="MonthDaily" style="-moz-min-width: 400px; -ms-min-width: 400px; -o-min-width: 400px; -webkit-min-width: 400px; min-width: 400px; height: 200px; margin: 0 auto"></div>
    <div id="YearWeekly" style="-moz-min-width: 400px; -ms-min-width: 400px; -o-min-width: 400px; -webkit-min-width: 400px; min-width: 400px; height: 200px; margin: 0 auto"></div>
    
    <div>
        <div style="text-align: center; ">Billable vs Non-Billable</div>
        <div style="margin-left: auto; margin-right: auto; width: 100%; text-align:center;">
            <span id="ProfitPieWeek" style="-moz-min-width: 200px; -ms-min-width: 200px; -o-min-width: 200px; -webkit-min-width: 200px; min-width: 200px; height: 200px; margin: 0 auto; display: inline-block;"></span>
            <span id="ProfitPieMonth" style="-moz-min-width: 200px; -ms-min-width: 200px; -o-min-width: 200px; -webkit-min-width: 200px; min-width: 200px; height: 200px; margin: 0 auto; display: inline-block;"></span>
            <span id="ProfitPieMonthPrev" style="-moz-min-width: 200px; -ms-min-width: 200px; -o-min-width: 200px; -webkit-min-width: 200px; min-width: 200px; height: 200px; margin: 0 auto; display: inline-block;"></span>
            <span id="ProfitPieYear" style="-moz-min-width: 200px; -ms-min-width: 200px; -o-min-width: 200px; -webkit-min-width: 200px; min-width: 200px; height: 200px; margin: 0 auto; display: inline-block;"></span>
            <div style="clear: both;"></div>
        </div>
    </div>
    <div id="JobsAccts" style="-moz-min-width: 400px; -ms-min-width: 400px; -o-min-width: 400px; -webkit-min-width: 400px; min-width: 400px; height: 400px; margin: 0 auto"></div>


</asp:Content>


