<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="NewsHome.aspx.cs" Inherits="News_NewsHome" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Corportate Home Page</title>
    
    <!-- Add Rotator Stuff -->
    <script type="text/javascript" src="/Scripts/jquery.anythingslider.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.easing.1.2.js"></script>
    <link rel="stylesheet" href="/Styles/anythingslider.css">
    
    <!-- Page Layout -->
    <link rel="stylesheet" href="/Styles/HomePage.css" type="text/css"/>
    <script type='text/javascript' src="/Scripts/HomePage.js"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
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
                    
			    </ul>	

		    </div>	 	
            
            <div class="SideBar-BoxBorder" style=" float: right; height: 600px;"/>
	    </section>
  
  
  
  
  

        <section class="ui-widget-header ui-corner-all" style="height: 45px; width: 99%; margin-top: 10px; margin-bottom: 30px;" >
                <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;">Shermco News Paper</div>
        </section> 
        
        
        <ul>
            <li style="border-bottom: 2px dashed grey;">
                <img src="/Images/WIP1.jpg" alt="WIP1" />
                We are working on building content for this page.  Please bear with us.
            </li>
            
            <li style="border-bottom: 2px dashed grey;">
                <img src="/Images/WIP2.jpg" alt="WIP2"/>
                We are working on building content for this page.  Please bear with us.
            </li>
            
            <li style="border-bottom: 2px dashed grey;">
                <img src="/Images/WIP3.jpg" alt="WIP3"/>
                We are working on building content for this page.  Please bear with us.
            </li>
        </ul>

    </div>  

</asp:Content>


