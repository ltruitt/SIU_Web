<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterMobile.master" AutoEventWireup="true" CodeFile="SearchDocPaneMobile.aspx.cs" Inherits="Library_SearchDocPaneMobile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

    <title>
        Document Search Page
    </title>
                
    <link rel="stylesheet" href="/Styles/LibDocPaneMobile.css" />
    <script type="text/javascript"  src="/Scripts/SearchDocPaneMobile.js" ></script>
    
        <style>
            #footer {
                display: none;
            }
        </style>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <a href="/HomePage.aspx" rel="external" style="text-decoration: none;" id="hrefBack">
        <div data-role="header" class="ui-widget-header ui-corner-all" style="height: 45px; width: 100%; margin-bottom: 5px;" data-theme="x" >
            <div id="hrefBackText" style="position: relative; left: 5px; vertical-align: top; font-size: 1em; line-height: 10px; padding-top: 5px;">Home...</div>            
            <div id="headerText" style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto; margin-top: -10px; line-height: 20px;">Document Search</div>
            <div style="width: 100%;  display:inline-block; font-size: 1em; text-align: center; font-weight: normal; color: white;">First 200 documents returned</div> 
        </div> 
    </a>

    <div id="SearchArea" style="margin-top: 5px;" >
        <span style="width: auto;  font-weight: bold; display:inline; font-size: 1.5em;">Filename Text To Search:</span> 
        <input ID="txtSearch" class="DataInputCss" style="width: 50%; display: inline-block; margin-left: 10px;" /> 

        <a href="#SearchNow"  id="searchBtn" style="margin-top: 20px;">
            <img src="/images/search_button.png" style="width: 30px;"  />
        </a>
    </div>
    
    <div id="loading" style="text-align: center; display: none;">
        <img src="/images/Loading3.gif" style="width: 80px; height: 60px;" />
    </div>
                     
    <div style="clear: both;"></div>
    <!-- Navagation Objects -->   
    <div class="NavSet" data-role="collapsible-set" data-theme="b" data-content-theme="c" >  <!-- Start NavSet -->
                        


    </div>  <!-- End NavSet -->

</asp:Content>

