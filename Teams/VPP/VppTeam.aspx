<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="VppTeam.aspx.cs" Inherits="Safety_VPP_VppTeam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <%--<link rel="stylesheet" href="/Styles/VppHome.css" type="text/css" />  --%>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<div id="HomeWrapper" style="height: 1320px;">
        

     <div style="visibility: hidden; height: 0; width: 0;">
        <span id="Sx" runat="server"/>
    </div>


        <section id="sidebar">
            <div class="SideBar-Box" style="height:450px; color: black;">

			    <ul>
				    <!--  Current Team -->
				    <li style="margin-bottom: 40px;">
				        <h3>Our Team Now</h3>
                        Jason Henry<br/>
                        John Clough<br/>
                        Casey Morris<br/>
                        Jennifer Colleps<br/>
                        Toni Nagle<br/>
                        Rhonda Frieman<br/>
                        Tanner Cook<br/>
                        Angie Schumacher<br/>
                        Brian Borowczak<br/>
                        John McCurley<br/>
                        Preston Mullen<br/>
                        Karl Stephens<br/>
				    </li>
                    
				    <!--  Email Link -->
				    <li>
				        <hr/>
                        <a href="mailto:VppCore@shermco.com?subject=I would like to join the team&body=Tell us something about why you would like to join....">
                        Join The Team
                        </a>
                        <hr/>
				    </li>
                    
                   <li>
                        <a href="/Files/Library/VEST/VEST%20Roles%20and%20Goalsl.pdf" target="_VppDoc">
                           Roles and Goals<br/>
                       </a>
                       <hr/>
                   </li>                     
                    
                </ul>
            </div>
            
            <div class="SideBar-BoxBorder" style=" float: right; height: 600px;"/>
        </section>    
        
        <div style="text-align: center;">
           <img style="border: none; margin: 0;" alt="Vpp Team" src="/Images/VPP Team Poster.jpg" /> 
        </div>

    </div>
</asp:Content>

