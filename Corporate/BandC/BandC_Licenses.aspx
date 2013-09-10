<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="BandC_Licenses.aspx.cs" Inherits="Corporate_BandC_BandC_Licenses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script type='text/javascript' src='https://www.google.com/jsapi'></script>
    <script type='text/javascript' src='/Scripts/BandC_Licenses2.js'></script>    
    
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<%--    <div id="HomeWrapper">--%>
    
        <section id="HomeMain" >
            
            <section class="ui-widget-header ui-corner-all" style="height: 45px; width: 100%;" >
                <a href="/ELO/MainMenu.aspx" style="text-decoration: none;">
                    <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;">State License Overview</div>
                </a>
            </section>  
            
          
            <div style="width: 900px; height: 500px; margin-left: auto; margin-right: auto;" id="Google_Map_Div"></div>
            
        </section>
    <%--</div>--%>
</asp:Content>

