<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="VestTeam.aspx.cs" Inherits="Teams_VEST_VestTeam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">


</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<div id="HomeWrapper" style="height: 1320px;">
        

     <div style="visibility: hidden; height: 0; width: 0;">
        <span id="Sx"  runat="server"/>
    </div>


        <section id="sidebar">
            <div class="SideBar-Box" style="height:450px; color: black;">

			    <ul>
			        
				    <!--  Current Team -->
				    <li style="margin-bottom: 40px;">
				        <h3>Our Team Now</h3>
                        Unknown<br/>
                        Unknown<br/>
                        Unknown<br/>
                        Unknown<br/>
                        Paul Nelson<br/>
                        Angie Schumacher<br/>
                        Tanner Cook<br/>
                        Rhonda Frieman<br/>
                        Unknown<br/>
                        Unknown<br/>
				    </li>
                    
				    <!--  Recorded Safety Meeting Link -->
				    <li>
				        <hr/>
                        <a href="mailto:VestCore@shermco.com">
                        Join The Team
                        </a>
                        <hr/>
				    </li>
                   
                   <li>
                        <a href="/Blog/Blog.aspx?BlogName=VEST">
                            <img style="height: 90px; " alt="VEST News" src="/Images/icon-blog.png"> 
                            <br/>    
                            Catch Up<br/>
                            The Latest VEST News
                        </a>
                    </li> 
                </ul>
            </div>
            
            <div class="SideBar-BoxBorder" style=" float: right; height: 600px;"/>
        </section>    




        

        
                        

        <section id="SafetyMain" >
        
        <div style="text-align: center;">
           <img style="border: none; margin: 0;" alt="Vpp Team" src="/Images/Vest_TeamPhoto.jpg" /> 
        </div>

            

        </section>
    </div>
    
</asp:Content>

