<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="EhsBlog.aspx.cs" Inherits="Safety_EhsBlog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>EHS Blog</title>
	      
    <link rel="stylesheet" href="/Styles/Insurance.css" type="text/css" />
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<div style="visibility: hidden; height: 0; width: 0;">
        <span id="Sx"  runat="server"/>
    </div>
 
    <div id="HomeWrapper" >
    
               
        <section id="HomeMain" >
           
            <section class="ui-widget-header ui-corner-all" style="height: 45px; width: 99%; margin-top: 10px; margin-bottom: 30px;" >
                <a href="/Safety/SafetyHome.aspx" style="text-decoration: none;">
                    <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">Safety home</span>
                    <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;margin-top: -10px;">EHS Blog</div>
                </a>
            </section> 

            
            <ul class="DeptGallery2" style="width: 100%;">
                <li style="text-align: center; width: 50%; padding-left: 0; padding-right: 0;">
                    <h3>
                        Recent EHS Documents                    
                        <br/>
                        <img    alt="Recent EHS Documents" src="/Images/RecentDocument6.png"  />
                    </h3>                    
                        
                    <ul id="DirectoryReady">
                        <li><a  href="/Advertisements/EHS_BLOG/December EHS Bulletin.pdf"  target="_DocView"  id="A35" >December Bulletin</a></li>
                    </ul>                    
                </li>
                
                <li style="text-align: center; width: 50%; padding-left: 0; padding-right: 0;">
                    <h3>
                        Previous EHS Documents                    
                        <br/>
                        <img    alt="Previous EHS Documents" src="/Images/RecentDocument7.png"  />
                    </h3>
<%--                    <ul id="Ul1">
                        <li><a  href="/Advertisements/EHS_BLOG/December EHS Bulletin.pdf"  target="_DocView"  id="A1" >December Bulletin</a></li>
                    </ul>  --%>                  
                </li>
                
            </ul>
                                    
        </section>
    </div>    
</asp:Content>

