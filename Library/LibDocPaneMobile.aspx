<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterMobile.master" AutoEventWireup="true" CodeFile="LibDocPaneMobile.aspx.cs" Inherits="Library_LibDocPaneMobile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
    <title>
        Mobile Look Library Interface
    </title>
                
    <link rel="stylesheet" href="/Styles/LibDocPaneMobile.css" />
    <script type="text/javascript"  src="/Scripts/LibDocPaneMobile.js" ></script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <a href="#" rel="external" style="text-decoration: none;" id="hrefBack">
        <div data-role="header" class="ui-widget-header ui-corner-all" style="height: 45px; width: 100%; margin-bottom: 5px;" data-theme="x" >
            <div id="hrefBackText" style="position: relative; left: 5px; vertical-align: top; font-size: 1em; line-height: 10px; padding-top: 5px;">back...</div>            
            <div id="headerText" style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto; margin-top: -10px;">Document Library</div>
        </div> 
    </a>
                     
    <a id="BackPath" href="#" data-role="button" style='width: 70px;' data-mini="true" data-theme="a" data-icon="arrow-l">UP</a>
                 
    <!-- Navagation Objects -->   
    <div class="NavSet" data-role="collapsible-set" data-theme="b" data-content-theme="c" >  <!-- Start NavSet -->
                        
        <div style="text-align: center;">
            <img src="/images/slider-loading.gif" />
            <h3>Loading...</h3>
        </div>

    </div>  <!-- End NavSet -->

</asp:Content>

