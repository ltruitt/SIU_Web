<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="CorpHome.aspx.cs" Inherits="Corporate_CorpHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Corportate Home Page</title>
    
    <!-- Add Rotator Stuff -->
    <script type="text/javascript" src="/Scripts/jquery.anythingslider.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.easing.1.2.js"></script>
    <link rel="stylesheet" href="/Styles/anythingslider.css">
    
    <!-- Page Layout -->
    <link rel="stylesheet" href="/Styles/HomePage.css" type="text/css"/>
    <script type='text/javascript' src="/Scripts/CorpHome.js"></script>
    

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div style="visibility: hidden; height: 0; width: 0;">
        <span id="Sx"  runat="server"/>
    </div>
    

    <div id="HomeWrapper">
        
        

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
                <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;">Administrative Support</div>
        </section> 

        <nav id="Corprate Home Page Navigation" style="margin-left: auto; margin-right: auto;">                   
            <!-- Corporate Icons -->  	
	        <section id="main" style="margin-left: auto; margin-right: auto; width: 98%;">
		        <div id="DeptGallery" >
			        <ul class="menu_gallery">
			    

                        <!-- HR LINK -->
				        <li>
					        <a href="/HR/HrHome.aspx">
						        <img src="/Images/SI-Corp-HR.png" alt="Human Resources" >
						        <div>Human Resources</div>
					        </a>
				        </li>
                
			            <!-- REPORTING LINK -->
				        <li>
					        <a href="/Reporting/ReportingHome.aspx" >
						        <img src="/Images/SI-Corp-Services.png" alt="Corporate Services">
						        <div>Corporate Services</div>
					        </a>
				        </li>
                        
			            <!-- Legal / Risk LINK -->
				        <li>
					        <a href="/Risk/RiskHome.aspx" >
						        <img src="/Images/SI-Corp-LegalRisk.png" alt="Legal / Risk Department">
						        <div>Legal / Risk</div>
					        </a>
				        </li>

			            <!-- Accounting LINK -->
				        <li>
					        <a href="/Accounting/AccountingHome.aspx" >
						        <img src="/Images/SI-Corp-Accounting.png" alt="Accounting Department">
						        <div>Accounting</div>
					        </a>
				        </li>
                        
			            <!-- Facilities LINK -->
				        <li>
					        <a href="/Facilities/FacilitiesHome.aspx" >
						        <img src="/Images/SI-Corp-Facilities.png" alt="Facilities Department">
						        <div>Facilities</div>
					        </a>
				        </li>                        

                
				        <!-- IT Department -->
				        <li >
					        <a href="/InformationTechnologies/ItHome.aspx">
						        <img src="/Images/SI-Corp-IT.png" alt="IT Department">
						        <div>Information Technologies</div>
					        </a>
				        </li>    
                
				        <!-- Badges and Certifications -->
				        <li >
					        <a href="BandC/BandC_Support_Links.aspx" title="Badges and Certifications Support Page">
						        <img src="/Images/SI-Corp-Certifications.png" alt="Badges and Certs">
						        <div>Badges &amp; Certifications Support Links</div>
					        </a>
				        </li>  

				        <!-- Dash Boards -->
				        <li style="display: none;">
					        <a title="DashBoards"   href="#">
						        <img src="/Images/SI-Corp-Dashboards.png"  alt="Dash Boards">
						        <div>Dash Boards</div>
					        </a>
				        </li>  
                        
				        <!-- Dash Boards -->
				        <li>
					        <a title="Reporting"   href="https://reports.shermco.com/reports/Pages/Folder.aspx" target="Reporting">
						        <img src="/Images/SI-Corp-Reporting.png"  alt="Dash Boards">
						        <div>Reporting</div>
					        </a>
				        </li>  

                
				        <!-- Completed Jobs Admin -->
				        <li style="display: none;">
					        <a title="Completed Jobs"   href="/Forms/JobsView.asp">
						        <img src="/Images/Missing.png"  alt="Completed Jobs">
						        <div>Completed Jobs</div>
					        </a>
				        </li>
                

                	
				        <!-- Manually Register Users -->
				        <li style="display: none;">
					        <a href="/Account/Register.aspx" title="Manually REgister Users">
						        <img src="/Images/Missing.png" alt="Register User">
						        <div>Register Non Employee User</div>
					        </a>
				        </li>
                    
                    		    
                    </ul>
                </div>
            </section>                
        </nav>
        
        
        
        
        <div style="display: block"><hr/></div>                                                         
        

		<div id="Div1" style="margin-left:0; " >
		        
            <ul class="DeptGallery2">
                <li>
                        
                        <ul class="DeptGallery2Links" >
                            <li style="text-align: center;">
                                <a href="/Facilities/VehInsp.aspx?d=60">
                                    <img  alt="Vehicle Inspection Report" src="/Images/VehInsp.png" style="width: 175px; padding: 0; margin-left: 25px;"/>
                                    <div style="margin-top: -20px;">Corp. Vehicle Inspection Report</div>
                                </a>
                            </li>
                        </ul>                    
                </li>                

            </ul> 
        </div> 

    </div> 

</asp:Content>

