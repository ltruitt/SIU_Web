<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="TeamsHome.aspx.cs" Inherits="Teams_TeamsHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Teams Home Page</title>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
        <!-- Left Side Of Main Content Area -->		  
        <section id="sidebar">
            
		    <!-- Site Shortcut Links -->		
            <div class="SideBar-Box" style="height:550px;">  
            
			    <ul>
			    
                    <!--------->
                    <!-- ELO -->  
                    <!--------->	
				    <li style="height: 80px; clear:both;">
					    <a href="/ELO/MainMenu.aspx">
						    <img style="width: 57px; height: 57px;" alt="ELO" src="/Images/icon-elo.png" id="Img1" />
						    <br/>ELO
					    </a>
				    </li> 
                    
                    <!--------->
                    <!-- OWA -->  
                    <!--------->	
				    <li style="height: 90px; clear:both;">
					    <a href="https://mail01.shermco.com" target="_OWA"> 
						    <img style="width: 57px; height: 57px;" alt="OWA" src="/Images/icon-mail.png" id="Img2" />
						    <br/>Mail
					    </a>
				    </li>                     
                    
				    <!-- News Letters Icon -->			
				    <li style="height: 90px; clear:both;">
					    <a href="/News/NewsHome.aspx">
						    <img style="width: 57px; height: 57px;" alt="News" src="/Images/icon-newsletter.png"  />
						    <br/>News
					    </a>
				    </li>  
			    </ul>	
            
                <hr/>

                <!-------------------->
                <!-- Scrolling News --> 
                <!-------------------->
                <ul id="BlogSlider" style="display: none;">
                    <asp:Literal runat="server" ID="BlogInsertPoint">Home</asp:Literal>
                </ul>                 					
		    </div>	 	
            
            <div class="SideBar-BoxBorder" style=" float: right; height: 600px;"/>
	    </section>
  
  
  
  
  

        <section class="ui-widget-header ui-corner-all" style="height: 45px; width: 99%; margin-top: 10px; margin-bottom: 30px;" >
                <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;">Shermco Teams</div>
        </section> 
        
        
        <!-- Links To Individual Team Pages -->  	
        <nav id="Corprate Home Page Navigation" style="margin-left: auto; margin-right: auto;">
                               
            
	        <section id="main" style="margin-left: auto; margin-right: auto; width: 98%;">
		        <div id="DeptGallery" >
			        <ul class="menu_gallery">
			    
                        <!-- VPP Team -->
                        <li style="height: 150px; border: none; margin-right: 35px;">
					        <a href="/Teams/VPP/VppHome.aspx" >
					            <img  style="width: 150px; height: 75px; margin: 0px; padding-top: 10px; margin-bottom: 5px;"  alt="Shermco's VPP Program" src="/Images/Si-VPP.png" />
                                <div style="font-size: .8em; float: left; text-decoration: underline; font-weight: bold;   width: 50px; width: 100%;">VPP</div>
					        </a>
                        </li>
                        
                        <!-- VPP Team -->
                        <li style="height: 150px; border: none; margin-right: 5px;">
					        <a href="/Teams/VEST/VestTeamHome.aspx" >
					            <img  style="width: 75px; height: 75px; margin: 0px; padding-top: 10px; margin-bottom: 5px;"  alt="Shermco's VEST Program" src="/Images/Si-EHS-VEST.png" />
                                <div style="font-size: .8em; float: left; text-decoration: underline; font-weight: bold;   width: 75px;">VEST</div>
					        </a>
                        </li>

                        <!-- VPP Team -->
                        <li style="height: 150px; border: none; margin-right: 5px;">
					        <a href="#" >
					            <img  style="width: 75px; height: 75px; margin: 0px; padding-top: 10px; margin-bottom: 5px;"  alt="This Web Site Team" src="/Images/Si-Teams.png" />
                                <div style="font-size: .8em; float: left; text-decoration: underline; font-weight: bold;   width: 75px;">This Web Site Advisory</div>
					        </a>
                        </li>                    
                    		    
                    </ul>
                </div>
            </section>                
        </nav>
</asp:Content>

