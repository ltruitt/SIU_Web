<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="ItHome.aspx.cs" Inherits="InformationTechnologies_ItHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Information Technologies Home Page</title>
    
    <link rel="stylesheet" href="/Styles/SafetyHome.css" type="text/css"/>  
    
        $(document).ready(function () {
            var isAdmin = $("#Sx")[0].innerHTML;
            if (isAdmin.length > 0) {
                $('#AdminIconA').prop('href', isAdmin);
                $('#AdminFooterLi').show();
            }
        });         
</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
     <div style="visibility: hidden; height: 0; width: 0;">
        <span id="Sx"  runat="server"/>
    </div>

    <section id="HomeWrapper" >

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
                    <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;">Information Technologies</div>
            </section>        
            
<%--            <ul class="DeptGallery2"  >
                <li>
                        <img    alt="EHS Documents" src="/Images/Home_documents.png" />
                        <ul class="DeptGallery2Links" >
                            <li><a href="#">Gerdau Contractor Safety</a></li>
                            <li><a href="#">Boise Contractor Safety</a></li>
                            <li><a href="#">Confined Spaces  and Dangerous Gases</a></li>
                            <li><a href="#">more....</a></li>
                        </ul>                    
                </li>
               
            </ul>--%>
  
            <hr/>

            <nav id="Corprate Home Page Navigation" style="margin-left: auto; margin-right: auto;">                   
                <!-- Corporate Icons -->  	
	            <section id="main" style="margin-left: auto; margin-right: auto; width: 98%;">
		            <div id="DeptGallery" >
			            <ul class="menu_gallery">
			    

                            <!-- HR LINK -->
				            <li>
					            <a href="/Forms/HardwareRequest.aspx" >
						            <img  style="margin: 0; margin-bottom: 5px; margin-left: auto; margin-right: auto;"  alt="Hardware Request" src="/Images/SI-IT-ComputerBuild.png" />            
						            <div>Computer Build Request</div>
					            </a>
				            </li>
                        </ul>
                    </div>
                </section>                
            </nav>
        </section>                      
                          
    </section>

</asp:Content>

