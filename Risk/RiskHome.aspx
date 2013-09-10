<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="RiskHome.aspx.cs" Inherits="Risk_RiskHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Risk and Legal Department Home Page</title>
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
                    <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;">Legal / Risk</div>
            </section>      
            
<%--            <ul class="DeptGallery2">
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
        </section>
        
    </div>  
    
           
</asp:Content>

