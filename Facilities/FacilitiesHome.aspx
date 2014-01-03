<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="FacilitiesHome.aspx.cs" Inherits="Facilities_FacilitiesHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Facilities Department Home Page</title>
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
                <a href="/" style="text-decoration: none; font-weight: bold; width: 100%;">
                    <span style="text-decoration: underline; font-style: italic; position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">home</span>
                    <div  style="width: 70%; text-align: center; font-size: 2em;  position:absolute;  margin-left: auto; margin-right: auto; margin-top: -10px">Facilities</div>
                </a>
            </section>        
            

            <ul class="DeptGallery2">
                <li>
                        
                        <ul class="DeptGallery2Links" >
                            <li style="text-align: center;">
                                <a href="/Facilities/VehInsp.aspx">
                                    <img  alt="Vehicle Inspection Report" src="/Images/VehInsp.png" style="width: 175px; padding: 0; margin-left: 25px;"/>
                                    <div style="margin-top: -20px;">Full Vehicle Inspection Report</div>
                                </a>
                                <a href="/Facilities/VehInsp.aspx?d=60">Corp. Vehicle Inspection Report</a><br/>
                                <a href="/Facilities/VehInsp.aspx?d=70">MSD Vehicle Inspection Report</a><br/>
                                <a href="/Facilities/VehInsp.aspx?d=80">ESD Vehicle Inspection Report</a>
                            </li>
                        </ul>                    
                </li>                

            </ul>  

        </section>
        
    </div>    
    
</asp:Content>

