<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="EsdHome.aspx.cs" Inherits="ESD_EsdHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Engineering Department Home Page</title>  
   
    <link rel="stylesheet" type="text/css" href="/Styles/EsdHome.css"/>
    
    <script type="text/javascript" src="/Scripts/SiLibrary.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $.SiLibrary.GetDepartmentDocumentFolders("/Files", "ESD", "DirectoryDocuments");
            $.SiLibrary.GetDepartmentVideoFolders("/Videos", "ESD Production", "DirectoryVideos");
        });     
    </script>
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
						    <br/>
                            ELO
					    </a>
				    </li> 
                    
                    <!--------->
                    <!-- OWA -->  
                    <!--------->	
				    <li style="height: 90px; clear:both;">
					    <a href="https://mail01.shermco.com" target="_OWA">
						    <img style="width: 57px; height: 57px;" alt="ELO"
								    src="/Images/icon-mail.png" id="Img2" />
						    <br/>
                            Mail
					    </a>
				    </li> 
                    
				    <!-- News Articles Icon -->			
				    <li style="height: 90px; clear:both;">
					    <a href="/Library/LibDocPaneMobile.aspx?Path=/ESD/NewsLetters">
						    <img style="width: 57px; height: 57px;" alt="News" src="/Images/icon-newsletter.png"  />
						    <br/>
                            News Letters
					    </a>
				    </li>  
                
                </ul>
            </div>
                    
            <div class="SideBar-BoxBorder" style=" float: right; height: 600px;"/>
            
        </section>
        
      
      
        <section id="HomeMain" >
            <section class="ui-widget-header ui-corner-all" style="height: 45px; width: 100%; margin-bottom: 5px;" >
                    <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;">Engineering Services</div>
            </section>      
            
            <ul class="DeptGallery2">
                <li>
                        <img  alt="ESD Documents" src="/Images/Home_documents.png" />
                        <ul class="DeptGallery2Links" id="DirectoryDocuments">
                        </ul>                    
                </li>

                
                <li style="width: 200px;">
                        <img alt="ESD Videos" src="/Images/Home_videos.png" />
                        <ul class="DeptGallery2Links" id="DirectoryVideos">
                        </ul>                    
                </li>
                
                
                <li>
                        <img    alt="ESD Documents" src="/Images/Home_documents.png" />
                        <ul class="DeptGallery2Links" id="EsdReports">

                            <li><a href="/Corporate/BandC/BandC_Licenses.aspx">State Licenses</a></li>
                        </ul>                    
                </li>
                

                
            </ul>

        </section>
        

        <div style="display: block"><hr/></div>                                                         
        

		<div id="DeptGallery" style="margin-left:0; " >
		        
			<ul class="menu_gallery" style="background-color: transparent;">

                <!-- Safety Pays -->
                <li style="height: 150px; border: none; margin-right: 5px;">
                        <a href="/Library/LibDocPaneMobile.aspx?Path=/ESD/SOPs">
						    <img  style="height: 75px; margin-left: 35px; padding-top: 10px; margin-bottom: 5px;"  alt="Safety Pays" src="/Images/library.png" />
					    </a>         
                            
                        <a href="/Library/LibDocPaneMobile.aspx?Path=/ESD/SOPs">
                            <div style="font-size: .9em; float: left; font-weight: bold;  width: 100%;  text-decoration: underline; margin-bottom: 5px; padding-left: 14px; ">SOPs and Datasheets</div>
                        </a>


                        <a href="/Library/LibDocPaneMobile.aspx?Path=/ESD/Reference">
                            <div style="font-size: .9em; float: left; font-weight: bold;  width: 100%;  text-decoration: underline; margin-bottom: 5px; padding-left: 14px; ">References</div>
                        </a>                                          
                    

                </li>          
            </ul>  
        </div>          

    </div>
    
</asp:Content>
