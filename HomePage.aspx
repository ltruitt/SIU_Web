<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="HomePage.aspx.cs" Inherits="HomePage" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <title>Shermco YOU Home Page</title>  

    
    
    <!-- Add Rotator Stuff -->
    <script type="text/javascript" src="/Scripts/jquery.anythingslider.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.easing.1.2.js"></script>
    <link rel="stylesheet" href="/Styles/anythingslider.css">
    
    <!-- Page Layout -->
    <link rel="stylesheet" href="/Styles/HomePage.css" type="text/css" />
    <script type='text/javascript' src="/Scripts/HomePage.js?0002"></script>

    <!-- AnythingSlider optional extensions -->
    <!-- <script src="js/jquery.anythingslider.fx.js"></script> -->
    <!-- <script src="js/jquery.anythingslider.video.js"></script> -->    

</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<%--    <div id="NoJS" class="noScriptMsg">
        You do not appear to have javascript enabled.<br/>
        You need to have scripting enabled to use this web site.
        <hr style="margin: 0;"/>
        
        <div style="font-size: .8em; margin-top: 20px; color: black;">
            Please contact I.T. or follow the instructions below:
            <div style="font-size: .7em; margin-top: 10px; line-height: 25px;">
                1)  Click The "Tools" Menu, and then click on "Internet Options"<br/>
                2)  Click The "Security tab, and then click on the "Custom Level..." button<br/>
                3)  Scroll down the security sections to the "Scripting" and then "Active scripting" section<br/>
                4)  Click "Enable"
            </div>
        </div>
    </div>--%>

	<div id="HomeWrapper" style="display: none;">


<!-- Left Side Of Main Content Area -->		  
        <section id="sidebar">
            
		    <!-- Site Shortcut Links -->		
            <div class="SideBar-Box" style="height:550px;">
            

			    <ul>
			    
                    <!--------->
                    <!-- ELO -->  
                    <!--------->	
				    <li  style="height: 80px; clear:both;">
					    <a id="eloSelect" href="/ELO/MainMenu.aspx" >
						    <img style="width: 57px; height: 57px;" alt="ELO"
								    src="/Images/icon-elo.png" id="Img1" />
						
						        <br/>ELO
					    </a>
				    </li> 
                    
                    <!--------->
                    <!-- OWA -->  
                    <!--------->	
				    <li style="height: 90px; clear:both;">
                        <a id="owaSelect" href="https://Outlook.office365.com" target="_OWA">
						    <img style="width: 57px; height: 57px;" alt="ELO"
								    src="/Images/icon-mail.png" id="Img2" />
						
						        <br/>Mail
					    </a>
				    </li>                     
                    

                
                
                    <!------------------>
                    <!-- Live Meeting -->  
                    <!------------------>	
				    <li style="height: 100px; clear:both;">
					    <a id="liveSelect" href="/Safety/SafetyMeeting.aspx">
						    <img style="width: 57px; height: 57px;" alt="Meet Online"
								    src="/Images/icon-MeetingLive.png" id="LiveMeet" />
						
						        <br/>Live Meeting
					    </a>
				    </li> 
                    
                    
                    <!------------------>
                    <!-- LMS -->  
                    <!------------------>	
				    <li style="height: 100px; clear:both;">
					    <a id="A2" href="http://shermco.csod.com" target="_LMS">
						    <img style="width: 67px; height: 57px;" alt="LMS"
								    src="/Images/VideoFolder2.png" id="Img3" />
						
						        <br/>LMS
					    </a>
				    </li> 
                    


                    
                                        
                    
                    <!------------------>	
				    <!-- Apparel Icon -->			
                    <!------------------>	
				    <li style="height: 90px; clear:both;">
					    <a href="http://www.shermcogear.com" target="_Apparel">
						    <img style="width: 75px; height: 70px;" alt="Apparel" src="/Images/Apparel.png"  />
						    <br/>Apparel
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
            
            <div class="SideBar-BoxBorder" style=" float: right; height: 600px; "/>
	    </section>
	  
	  

	  <section id="rotator"  >
            <ul id="slider" >
                <asp:Literal runat="server" ID="RotatorIMagesInsertPoint">Home</asp:Literal>
            </ul>         
	  </section>	





      <!-- Home Page Icons -->
	  <section id="main"  >

		<div id="DeptGallery"  >
			<ul class="menu_gallery" >
			    
                <!--------------------->
                <!-- MY SHERMCO LINK -->
                <!--------------------->
				<li>
					<a href="/My/MyHome.aspx"  id="MySiIcon">
						<img src="/Images/SI-Home-MySi.png" alt="My Shermco Area" style="" >
						<div>MySi</div>
					</a>
                </li>
                

                <!----------------------------->
				<!-- SHERMCO UNIVERSITY LINK -->
                <!----------------------------->
				<li>
					<a href="/Univ/UnivHome.aspx" id="SiuIcon">
						<img src="/Images/SI-Home-ShermcoYOU.png" alt="Shermco Univerity">
						<div>Shermco University</div>
					</a>
				</li>
                
                
                <!----------------->
                <!-- SAFETY LINK -->
                <!----------------->
				<li>
					<a href="/Safety/SafetyHome.aspx"  id="EhsIcon">
						<img src="/Images/SI-Home-EHS.png" alt="Shermco EHS Department">
						<div>EHS</div>
					</a>
				</li>


                <!-------------->
				<!-- ESD LINK -->
                <!-------------->
				<li>
					<a href="/ESD/EsdHome.aspx" id="EsdIcon">
						<img src="/Images/SI-Home-ESD.png" alt="Engineering Department">
						<div>ESD</div>
					</a>
				</li>

                
                <!-------------->
                <!-- MSD LINK -->
                <!-------------->
				<li>
					<a href="/MSD/MsdHome.aspx" id="MsdIcon">
						<img src="/Images/SI-Home-MSD.png" alt="Motor Shop Home">
						<div>MSD</div>
					</a>
				</li>



                <!----------------------------->
                <!-- CORPORATE SERVICES LINK -->
                <!----------------------------->
				<li>
					<a href="/Corporate/CorpHome.aspx" id="CorpIcon">
						<img src="/Images/SI-Home-Corporate.png" alt="Corporate Home"/>
						<div>Corporate</div>
					</a>
				</li>
                
                
                <!-------------------------->
                <!-- TEAMS HOME PAGE LINK -->
                <!-------------------------->
				<li>
					<a href="/Teams/TeamsHome.aspx" id="TeamsIcon">
						<img src="/Images/Si-Teams.png" alt="Teams Home"/>
						<div>Shermco Teams</div>
					</a>
				</li>
                

                <!------------------>
                <!-- Library LINK -->
                <!------------------>
				<li>
					<%--<a href="/Library/LibHome.aspx" id="A1">--%>
                    <a href="/Library/LibDocPaneMobile.aspx?Back=/HomePage.aspx" id="DocumentsIcon">
						<img src="/Images/SI-Home-Library.png" alt="Document Library"/>
						<div>Document Library</div>
					</a>
                    <a  href="/Library/LibHome.aspx"    id="VideosIcon" >
                    <%--<a  href="/Library/LibDocPaneMobile.aspx?Back=HomePage&VR=/videos"    id="A4" >--%>
                        <div>Video Library</div>
                    </a>
				</li>                
                
                
                <!--------------------------->
                <!-- Web Site Intro Videos -->
                <!--------------------------->
				<li style="height: 300px;">
                    <a href="/Library/LibVideoPane.aspx?VR=/Videos&Path=/WEB/Intro" id="IntroIcon">
                        <img style="border:0; margin-left: 20px; margin-top: 20px; height: 80px;" alt="WEB INTRO" src="/Images/icon-introvideos.png"   id="mbi_mb73g0_100" />
						<div style="text-align: center;">Web Site Introduction</div>				    
					</a>
                    <div style="width: 100%; text-align: center;">
                        <a href="/video/FlowPlayerVideo.aspx?Video=ELO.mp4&Path=/Videos/WEB/Intro" id="A1" style="color: blue; font-size: .8em; text-align: center;" target="_Intro">ELO</a>
                    </div>
                    <div style="width: 100%; text-align: center;">
                        <a href="/video/FlowPlayerVideo.aspx?Video=SP.mp4&Path=/Videos/WEB/Intro" id="A8" style="color: blue; font-size: .8em; text-align: center;" target="_Intro">Safety Pays</a>
                    </div>
				</li>  

                

			</ul>
		</div>
    </section>        

    </div>
	  

                   
    <div style="position: absolute; bottom: 0; right: 10px; z-index: 100;">
        <a href="javascript:  window.location = window.location.protocol + '//' + window.location.hostname + '/' + 'phone/homepage.aspx';">
            <img src="/Images/phone6.png" alt="phone view" style="margin: 0; border: none; height: 55px;"/>
        </a>
    </div>
	

</asp:Content>


