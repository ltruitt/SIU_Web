<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="LibHome.aspx.cs" Inherits="Library_LibHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Shermco Library</title>
    
    <!-- Generic Library Scripting -->
    <script src="/Scripts/SiLibrary.js" type="text/javascript"></script>
    
    <!-- Library Home Page Scripts and Styles -->
    <link rel="stylesheet" href="/Styles/SiLibrary.css" type="text/css"/>  
    <script src="/Scripts/LibHome.js" type="text/javascript"></script>
    
    
    <!-- Add Rotator Scripts And Style -->
<%--    <script type="text/javascript" src="/Scripts/jquery.anythingslider.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.easing.1.2.js"></script>
    <link rel="stylesheet" href="/Styles/anythingslider.css">--%>
    
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server" EnableViewState="False">

    <div id="HomeWrapper">
        
        <!-- Left Side Of Main Content Area -->		  
        <section id="sidebar">
            
         	
		    <!-- Site Shortcut Links -->		
            <div class="SideBar-Box" style="height:550px;">


            <!-- Hovering Over The News Letter Icon Reveals A List Of Gallery Objects Presented In A Popup Window -->
            <!-- On Page Load, Those Popup Window Definitions Are Generated ANd Insetred Here Dynamically         -->
            <asp:Literal ID="PopupMenusDocumentInsertPoint" runat="server" Text="Corporate\News Letters"></asp:Literal>    
            

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
                    

                    <!-------------------------------------->
                    <!-- Manually Derived Popup Menu Icon -->  
                    <!-------------------------------------->
				    <!-- News Letters Icon -->			
				    <li style="height: 90px; clear:both;">
					    <a href="#">
						    <img style="width: 57px; height: 57px;" alt="News Letters"
								    src="/Images/icon-newsletter.png" 
                                    id="mbi_mb73g0_1" />
						
						        <br/>News
					    </a>
				    </li>  
			    </ul>	
            
                    
                                					
		</div>	
        
        <div class="SideBar-BoxBorder" style=" float: right; height: 600px;"/> 	
	</section>
    


        <section id="HomeMain" >
            
            <section class="ui-widget-header ui-corner-top" style="width: 98.5%;" >
                    <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;">Shermco Video Library</div>
            <%--</section>    --%>           
<%--            
            <div class="LibGallery ui-widget-header ui-corner-bottom" id="Shermco Document Library" >
                <div class="Icon">
                    <img src="/Images/folder_32.gif" alt="Library Documents"/>
                </div>

                <ul id="DocumentFolders" style="margin-top: 0; "/>
            </div>
            --%>
            

            <div class="LibGallery HomeLibGallery ui-widget-header ui-corner-bottom" id="Shermco Video Library" style="xfloat: right; xmargin-right: 1.2%;">
                <div class="Icon">
                    <img src="/Images/ReelBlue.png" alt="Video Documents" style="height: 35px;"/>
                </div>
                                
                <ul id="VideoFolders" style="margin-top: 0;"/>
            </div>
                
</section>           
        </section>
    </div>
    
</asp:Content>

