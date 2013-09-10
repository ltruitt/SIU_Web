<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="PrevLibDocPane.aspx.cs" Inherits="Library_LibDocPane" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Shermco Document Library</title>
    
    <link rel="stylesheet" href="/Styles/SiLibrary.css" type="text/css"/>  
    <link rel="stylesheet" href="/Styles/jquery-breadcrumbs.css" type="text/css"/>  
    
    <script src="/Scripts/jquery-breadcrumbs.js" type="text/javascript"></script>  
    <script src="/Scripts/SiLibrary.js" type="text/javascript"></script>
    <script src="/Scripts/LibDocPane.js" type="text/javascript"></script>  
    
    
    
    <style>
        #HomeWrapper {
  	        border-left:        0 solid #D4C37B;	    /*  background: color for #sidebar  */
  	        border-right:       0 solid #D4C37B;	    /*  background: color for #aside    */
        }
    </style>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="HomeWrapper">
        
        <section id="HomeMain"  >
            
            <section class="ui-widget-header ui-corner-top" style="width: 99.0%; margin-left: .5%" >
                <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;">Shermco Document Library</div>
            </section>           
            
            
            <div style="margin-left: .5%;">
                <div id="BC"  style="background-color: gainsboro; width: 99.5%; border:black solid 1px; overflow: auto; ">
                    <ul id="breadcrumbs">
                        <%--<li id='Library'>Library</li>--%>
                    </ul>
                </div>
            
            
                                
                <div class="LibGallery ui-widget-header ui-corner-bottom" id="Folders List" style="clear: both;">
                    <div class="Icon">
                        <img src="/Images/folder_32.gif" alt="Library Documents"/>
                    </div>

                    <ul id="DocumentFolders" style="margin-top: 0; "/>
                </div>
            
                <div class="LibGallery ui-widget-header ui-corner-bottom" id="Files List" style="float: right; margin-right: .3%;" >
                    <div class="Icon">
                        <img src="/Images/Files.png" alt="Library Documents" style="height: 32px;"/>
                    </div>
                                                                
                    <ul id="FileList" style="margin-top: 0;"/>
                    
                </div>
            </div>                
        </section>
    
        <div style="clear: both;"></div>    
        
    </div>
</asp:Content>

