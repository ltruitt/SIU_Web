<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="HrHome.aspx.cs" Inherits="HR_HrHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
	<title>Human Resources Home Page</title>
	 
    <!-- Add Rotator Stuff -->
    <script type="text/javascript" src="/Scripts/jquery.anythingslider.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.easing.1.2.js"></script>
    <link rel="stylesheet" href="/Styles/anythingslider.css">
       
    <link rel="stylesheet" href="/Styles/HomePage.css" type="text/css" />
    <script type='text/javascript' src="/Scripts/HomePage.js"></script>
    
    <script type="text/javascript" src="/Scripts/SiLibrary.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $.SiLibrary.GetDepartmentDocumentFolders("/Files", "HR", "DirectoryDocuments");
            $.SiLibrary.GetDepartmentVideoFolders("/Videos", "HR", "DirectoryVideos");
        });

        $(document).ready(function () {
            var isAdmin = $("#Sx")[0].innerHTML;
            if (isAdmin.length > 0) {
                $('#AdminIconA').prop('href', isAdmin);
                $('#AdminFooterLi').show();
            }
        });    
    </script>
    

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div style="visibility: hidden; height: 0; width: 0;">
        <span id="Sx"  runat="server"/>
    </div>
    

  
   <%--<a href="http://www.gwrs.com/" style="margin-right: 35px; margin-left: 35px;">Click Here for Additional Information and Documents<br/>on the Great West Retirement Services web site</a>--%>   
   

    <div id="HomeWrapper" >
    
        <!-- Left Side Of Main Content Area -->		  
        <section id="sidebar">
            
         	
		    <!-- Site Shortcut Links -->		
            <div class="SideBar-Box" style="height:300px;">
                    
			    <ul>
		    				                      		                             
                    <!---------->
                    <!-- Jobs -->  
                    <!---------->	
				    <li style="height: 90px; clear:both;">
					    <a href="Corporate/Documents/PolicyManualJanuary2012.pdf" target="newton">
						    <img style="width: 57px; height: 57px;" alt="Jobs" src="/Images/icon-jobs.png" id="Img6" />
						
						        <br/>Career
					    </a>
				    </li> 
                
			        <!-- 2012 Health Fair -->			
			        <li style="height: 70px; clear: both; padding-top: 20px; padding-bottom: 20px;" >
				        <a href="/Library/LibVideoPane.aspx?VR=/Videos&Sub=HR/&Path=2012 Health Fair">
					        <img style="width: 75px; height: 50px;" alt="Health Fair Videos" 
							        src="/Images/HealthFairSign.png"  id="mbi_mb73g0_100" />
                                <br/>2012 Videos
				        </a>
			        </li> 
                    
                     
                    
                    

                    <!-------------------------->
                    <!-- Personal Vehical Use -->  
                    <!-------------------------->	
				    <li style="height: 100px; clear:both;">
					    <a href="/My/PersonalUseInfo.aspx">
						    <img style="width: 127px; height: 57px; " alt="Personal Use" src="/Images/icon-vehicle2.png" id="Img5" />
						    Personal Use of Vehicles FAQ
					    </a>
				    </li> 
                    
                    <hr/>

                    <!-------------------->
                    <!-- Scrolling News --> 
                    <!-------------------->
                    <ul id="BlogSlider" style="display: none;">
                        <asp:Literal runat="server" ID="BlogInsertPoint">HR</asp:Literal>
                    </ul>  

                </ul>
            </div>
                    
            <div class="SideBar-BoxBorder" style=" float: right; height: 400px;"/>
            
        </section>
        

        
        <section id="HomeMain" >
            <section class="ui-widget-header ui-corner-all" style="height: 45px; width: 99%; margin-top: 10px; margin-bottom: 30px;" >
                <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;">Human Resources</div>
            </section>        
            
            <ul class="DeptGallery2">
                <li>
                        <img    alt="HR Documents" src="/Images/Home_documents.png" />
                        <ul class="DeptGallery2Links" id="DirectoryDocuments">
                        </ul>                    
                </li>
                
                <li>
                        <img    alt="HR Videos" src="/Images/Home_videos.png" />
                        <ul class="DeptGallery2Links" id="DirectoryVideos">
                        </ul>                    
                </li>
                
            </ul>
                                    
        </section>
    </div>


</asp:Content>

