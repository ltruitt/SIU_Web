<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="VestTeamHome.aspx.cs" Inherits="Teams_VEST_VestTeamHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
    <title>VPP Team Home Page</title> 
    
    <!-- Add Rotator Stuff -->
    <script type="text/javascript" src="/Scripts/jquery.anythingslider.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.easing.1.2.js"></script>
    <link rel="stylesheet" href="/Styles/anythingslider.css">
    
    <!-- Marquee Scroller Ref Scrolling.js for demo -->
    <!-- http://www.maxvergelli.com/jquery-scroller/ -->
    <script src="/Scripts/jquery-scroller-v1.min.js" type="text/javascript"></script> 
    
    <link rel="stylesheet" href="/Styles/HomePage.css" type="text/css" />
    <link rel="stylesheet" href="/Styles/VestTeamHome.css" type="text/css" />
    <link rel="stylesheet" href="/Styles/Popup.css" type="text/css" /> 

    <script type='text/javascript' src="/Scripts/Teams.js"></script>
    <script type='text/javascript' src="/Scripts/VestTeamHome.js"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
     <div style="visibility: hidden; height: 0; width: 0;">
        <span id="Sx" runat="server"/>
    </div>

    <div id="HomeWrapper" >

        <section id="sidebar">
            <div class="SideBar-Box" style="height:450px;">

			    <ul>                    
                   <li>
                       <a href="/Library/LibDocPaneMobile.aspx?VR=/Files&Path=/VEST&Sub=/Library?Back=Vest/VestTeamHome">
                           <img style="height: 90px; " alt="VEST News" src="/Images/SI-Home-Library.png"> 
                           <br/>    
                           Vest Library<br/>
                       </a>
                       <hr/>
                    </li> 
                    
				    <!--  Email Link -->
				    <li>
				        
                        <a href="mailto:VestCore@shermco.com?subject=I would like to join the team&body=Tell us something about why you would like to join....">
                        Join The Team
                        </a>
                        <hr/>
				    </li>
                    
                   <li>
                        <a href="/Files/Library/VEST/VEST Roles and Goalsl.pdf" target="_VestDoc">
                        Roles and Goals
                       </a>
                       <hr/>
                   </li>                     

                </ul>
            </div>
            
            <div class="SideBar-BoxBorder" style=" float: right; height: 600px;"/>
        </section>        
                
        <section id="HomeMain" >
        
            <section class="ui-widget-header ui-corner-all" style="height: 45px; width: 99%; margin-top: 10px; margin-bottom: 30px;" >
                    <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;">VEST</div>
            </section> 
            
         
	      <section id="rotator"  >
                <ul id="slider" >
                    <asp:Literal runat="server" ID="RotatorIMagesInsertPoint">VEST_ROTATOR</asp:Literal>
                </ul>   
                <div style="margin-bottom: 2px; margin-top: -10px;">
                     <div style="width: 150px; display: inline-block; font-weight: bold; border: 1px solid black; background-color: cadetblue;   margin-right: 1px;">Core Team Members</div>
                </div>

			    <ul class="TeamMembersUL  Core">
                    <li>Angie Schumacher</li>
                    <li>Tanner Cook</li>
                    <li>Rhonda Frieman</li>
                    <li>Bob Jett</li>
                    <li>Casey Morris</li>                     
                    <li>Paul Nelson</li>   
                    <li>John Stanfield</li>   
                    <li>Ray Weinberger</li>   
                </ul>
                
                <div style="height: 5px;"></div>
	      </section>	
      
          <div style="height: 20px;"></div>
            
           <!-- Blog  -->
          <section >
              <div style="font-size: 2em; font-weight: bold;">VEST BLOG</div>
              <ul class="TeamsNewsUL" id="TeamPageBlog" title="VEST_BLOG"></ul>
          </section>
          
          
          <div style="height: 10px;"></div>
            
        </section>
        
    </div>
          
    <!-- Marguee -->
    <div class="horizontal_scroller" style="height: 100px; width: 100%; margin: 0;" >
	<div class="scrollingtext"  id="TeamPageMarquee" title="VEST_MARQUEE">
        <span><b>Scrolling text ...</b></span>
        <img src="/Images/Si-EHS-VEST.png" alt="Scrolling Container Example 1"  />
        <span><b>Scrolling text ...</b></span>
	</div>

</div>


          
    <div id="popup-box" class="popup">
        <a href="#" class="close"><img src="/Images/Delete.png" class="btn_close" title="Close Window" alt="Close" />
            <span style="color: white; position: absolute; top:  15px; margin-left: 20px;">NOTICE.....</span>
            <span style="color: white; position: absolute; top:  15px; margin-left: 150px;">NOTICE.....</span>
            <span style="color: white; position: absolute; top:  15px; margin-left: 280px;">NOTICE.....</span>
        </a>
        

        <div  class="popupDiv" >
            
            <img src="/Images/Under-construction.jpg"  title="Close Window" alt="Notice" />

<%--            <div style="width: 130px; ">
                <input type="button" ID="btnOK" Class="SearchBtnCSS" style="width: 100px; color: red;" value="OK"/>
            </div>--%>
        </div>
    </div>  

            
</asp:Content>

