<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="VehInsp.aspx.cs" Inherits="Facilities_VehInsp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Facilities Department Home Page</title>
    <script type="text/javascript" src="/Scripts/FacilitiesVehInsp.js"></script> 
    <style>
        #VehInspRpt a {
            color: blue;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div id="HomeWrapper">
            
        <!-- Left Side Of Main Content Area -->		  
        <section id="sidebar">
            
         	
		    <!-- Site Shortcut Links -->		
            <div class="SideBar-Box" style="height:300px;">
                    
			    <ul>
		    				                      		             
                    <!--------->
                    <!-- ELO -->  
                    <!--------->	
				    <li style="height: 80px; clear:both;">
					    <a href="/ELO/MainMenu.aspx">
						    <img style="width: 57px; height: 57px;" alt="ELO"
								    src="/Images/icon-elo.png" id="Img1" />
						
						        <br/>ELO
					    </a>
				    </li> 
                    
                    <!--------->
                    <!-- OWA -->  
                    <!--------->	
				    <li style="height: 90px; clear:both;">
					    <a href="https://mail01.shermco.com" target="_OWA">
						    <img style="width: 57px; height: 57px;" alt="ELO"
								    src="/Images/icon-mail.png" id="Img2" />
						
						        <br/>Mail
					    </a>
				    </li> 
                
                </ul>
            </div>
                    
            <div class="SideBar-BoxBorder" style=" float: right; height: 300px;"/>
            
        </section>
        <section id="HomeMain" >
            <section class="ui-widget-header ui-corner-all" style="height: 45px; width: 99%; margin-top: 10px; margin-bottom: 30px;" >
                <a href="/Facilities/FacilitiesHome.aspx" style="text-decoration: none;">
                    <span style="text-decoration: underline; font-style: italic;   position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">Facilities menu</span>
                    <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto; margin-top: -10px;">Unresolved Vehicle Inspections</div>   
                </a>
            </section>        
            <div style="width: 100%; text-align: center;">
                <img id="PlsWait" src="/Images/Please_wait1.gif"/>
            </div>
            <div id="VehInspRpt" style="margin-bottom: 20px;"></div>
        </section>
        
    </div>  
</asp:Content>

