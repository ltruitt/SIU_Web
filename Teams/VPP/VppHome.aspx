<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="VppHome.aspx.cs" Inherits="Safety_VPP_VppHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
    <title>VPP Team Home Page</title> 
    
    <!-- Add Rotator Stuff -->
    <script type="text/javascript" src="/Scripts/jquery.anythingslider.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.easing.1.2.js"></script>
    <link rel="stylesheet" href="/Styles/anythingslider.css">
    
    <!-- Marquee Scroller Ref Scrolling.js for demo -->
    <!-- http://www.maxvergelli.com/jquery-scroller/ -->
    <script src="/Scripts/jquery-scroller-v1.min.js" type="text/javascript"></script> 

	<!-- Page Layout -->
	<link rel="stylesheet" href="/Styles/VppHome.css" type="text/css" />  
    <link rel="stylesheet" href="/Styles/HomePage.css" type="text/css" />    
    <link rel="stylesheet" href="/Styles/Popup.css" type="text/css" />    
    
    <script type='text/javascript' src="/Scripts/Teams.js"></script>
    <script type='text/javascript' src="/Scripts/VppTeamHome.js"></script>
    
   
        
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
     <div style="visibility: hidden; height: 0; width: 0;">
        <span id="Sx" runat="server"/>
    </div>

    <div id="SafetyWrapper" >
        
        <section id="sidebar" style="margin-top: -20px;">
            <div class="SideBar-Box" style="height:450px;">

			    <ul>
                    
                   <li>
                        <a href="/Library/LibDocPaneMobile.aspx?Path=/VPP?Back=VPP/VppHome">
                            <img style="height: 90px; " alt="VPP News" src="/Images/SI-Home-Library.png"> 
                            <br/>    
                            VPP Library<br/>
                        </a>
                       <hr/>
                    </li> 
                    
                   <li>
                        <a href="/Files/Library/VPP/VPP Team Roles and Goals.pdf" target="_VppDoc">
                        Roles and Goals
                       </a>
                       <hr/>
                   </li>                     

                   <li>
                        <a href="/Teams/VPP/Elements.aspx">
                        Elements of OSHA Challenge
                       </a>
                       <hr/>
                   </li>  
                    
                   <li>
                        <a href="/Teams/VPP/HealthPolicyStatement.aspx" >
                        Health Policy
                       </a>
                       <hr/>
                   </li>  
                    

                </ul>
            </div>
            
            <div class="SideBar-BoxBorder" style=" float: right; height: 800px;"/>
        </section>  
        
        <section id="HomeMain" >
           
        <section id="aside" style="margin-top: -20px;">
            <div class="box BoxText" style="height:500px">
                <h1 class="BoxHeader">Safety and Health Mission Statement</h1>
                
                <div style="height: 4px;"></div> 
                <span class="em0">The mission of</span> 
                <span class="em1">Shermco Industries</span> 
                <span class="em0">is to</span>
                <span class="em2">provide</span> 
                <span class="em0">a level of service that </span>
                <span class="em3">exceeds</span> 
                <span class="em0">our customers expectation. We will continue to </span>
                <span class="em2">maintain</span> 
                <span class="em0">a</span> 
                <span class="em3">healthy</span> 
                <span class="em0">work environment and enhance a culture of </span>
                <span class="em3">safe</span> 
                <span class="em0">work practices wherein management and employees may work </span>
                <span class="em2">together</span> 
                <span class="em0">as a </span>
                <span class="em2">team</span> 
                <span class="em0">seeking </span>
                <span class="em2">excellence</span> 
                <span class="em0">in </span>                
                <span class="em3">safety.</span> 
            </div>          
        </section>
        
        <section class="ui-widget-header ui-corner-all" style="height: 45px; width: 99%; margin-top: 10px; margin-bottom: 10px;" >
                <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;">VPP</div>
        </section>         
        
            
	      <section id="rotator"  >
                <ul id="slider" style="width: 250px; height: 200px; margin-top: 20px;" >
                    <asp:Literal runat="server" ID="RotatorIMagesInsertPoint">VPP_ROTATOR</asp:Literal>
                </ul> 
              
                <div style="margin-bottom: 2px; margin-top: -10px;">
                     <div style="width: 150px; display: inline-block; font-weight: bold; border: 1px solid black; background-color: cadetblue;   margin-left: 0;">Team Members</div>
                </div>

			    <ul class="TeamMembersUL  Core">
                    <li>Jason Henry</li>
                    <li>John Clough</li>
                    <li>Casey Morris</li>
                    <li>Jennifer Colleps</li>
                    <li>Toni Nagle</li>
                    <li>Rhonda Frieman</li>
                    <li>Tanner Cook</li>
                    <li>Angie Schumacher</li>
                    <li>Brian Borowczak</li>
                    <li>Preston Mullen</li>
                    <li>Karl Stephens</li>
                </ul>                      
	      </section>	


          <div style="height: 20px;"></div>
            
           <!-- Blog  -->
          <section>
              <div style="font-size: 2em; font-weight: bold;">VPP BLOG</div>
              <ul class="TeamsNewsUL" id="TeamPageBlog" title="VPP_BLOG"></ul>
          </section>
          
          
          <div style="height: 10px;"></div>
          
        </section>
    </div>

    <!-- Marguee -->
    <div class="horizontal_scroller" style="height: 100px; width: 100%; margin: 0;" >
	    <div class="scrollingtext"  id="TeamPageMarquee" title="VPP_MARQUEE">
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

