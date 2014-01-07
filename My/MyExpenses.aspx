<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="MyExpenses.aspx.cs" Inherits="My_MyExpenses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Expenses</title>

    <!-- jqGrid Reference -->
    <link rel="stylesheet" type="text/css" media="screen" href="/jqGrid-4.4.4/css/ui.jqgrid.css" />
    <script src="/jqGrid-4.4.4/js/i18n/grid.locale-en.js" type="text/javascript"></script>
    <script type="text/javascript">jQuery.jgrid.no_legacy_api = false;</script>
    <script src="/jqGrid-4.4.4/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    
    
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />
    <script type="text/javascript" src="/Scripts/MyExpenses.js"></script> 
</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        
        <div id="SuprArea" style="margin-top: 5px;" runat="server">
            <span style="float:left; margin-right: 20px; width: auto;  font-weight: bold; display:inline">Change Employee:</span>
            <input ID="ddEmpIds" class="DataInputCss" runat="server" style="width: 400px;" />         
        </div>
                
        <!----------------->
        <!-- Form Header -->
        <!----------------->                   
        <section class="ui-widget-header ui-corner-all" style="height: 45px; margin-top: 15px; margin-bottom: 5px;" >
            <a href="/ELO/MainMenu.aspx" style="text-decoration: none;">
                <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">elo menu</span>
                <span  style="text-align: center; font-size: 2em; width: 100%; margin-top: 7px; position:absolute; left: 0; margin-left: auto; margin-right: auto;">YTD Expenses</span>
            </a>
        </section>  

        <section style="visibility: hidden; height: 0; width: 0;">
            <span id="hlblEID"     runat="server"/> 
            <span ID="lblEmpName"  runat="server"/>
        </section>      
    
        <div style="margin-top: -5px; width: 100%; background-color: gainsboro;">
            <div style="display: inline-block;">
                <input type="checkbox" id="chkLY" style="color: black; margin-left: 10px; margin-top: 2px;" value="chkLY"/>
            </div>
            <div style="clear: both; margin-left: 5px; font-size: 1.4em; margin-top: -27px; display:inline-block;">Last Year</div>
        </div>          
            
        <div id="jqwrapper" style="width: 100%; height: 100%; text-align: center;">
            <table id="ExpTbl"></table>
        </div>
        
    

</asp:Content>



