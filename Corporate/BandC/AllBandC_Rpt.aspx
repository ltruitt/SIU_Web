<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="AllBandC_Rpt.aspx.cs" Inherits="Corporate_BandC_AllBandC_Rpt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
        <title>All BandC Documents</title>

    <!-- jqGrid Reference -->
    <link rel="stylesheet" type="text/css" media="screen" href="/jqGrid-4.4.4/css/ui.jqgrid.css" />
    <script src="/jqGrid-4.4.4/js/i18n/grid.locale-en.js" type="text/javascript"></script>
    <script type="text/javascript">        jQuery.jgrid.no_legacy_api = true;</script>
    <script src="/jqGrid-4.4.4/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    
    
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />
    <script type="text/javascript" src="/Scripts/AllBandC_Rpt.js"></script> 
    
    <style type="text/css">
        .myAltRowClass { background-color: hotpink; background-image: none; color: black; font-weight: bold;  }
    </style>
</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div id="SuprArea" style="margin-top: 5px;" runat="server" >
        <span style="float:left; margin-right: 20px; width: auto;  font-weight: bold; display:inline">Change Employee:</span>
        <input ID="ddEmpIds" class="DataInputCss" runat="server"  style="width: 400px;" />         
    </div>
    
        
    <div style="background-color: #45473f;">
        
        
            <!----------------->
            <!-- Form Header -->
            <!----------------->                   
            <section class="ui-widget-header ui-corner-all" style="height: 45px; margin-top: 15px; margin-bottom: 5px;" >
                <a href="/My/MyHome.aspx" style="text-decoration: none;">
                    <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">MySi</span>
                    <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Badges and Certifications Report</span>
                </a>
            </section>  

            <section style="visibility: hidden; height: 0; width: 0;">
                <span id="hlblEID"           runat="server"/> 
                <span ID="lblEmpName"        runat="server"/>
            </section>            
            
            <div id="jqwrapper" style="margin-left: auto; margin-right: auto; width: 100%; height: auto;">
                <table id="BandC_Tbl"></table>
            </div>
        
    </div>        
       
</asp:Content>

