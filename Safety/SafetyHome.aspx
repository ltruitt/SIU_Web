<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="SafetyHome.aspx.cs" Inherits="Safety_SafetyHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>EHS Department Home Page</title>  
    
    <!-- Marquee Scroller Ref Scrolling.js for demo -->
    <!-- http://www.maxvergelli.com/jquery-scroller/ -->
<%--    <script src="/Scripts/jquery-scroller-v1.min.js" type="text/javascript"></script> --%>

    <script type="text/javascript" src="/Scripts/SiLibrary.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $.SiLibrary.GetDepartmentDocumentFolders("/Files", "EHS", "DirectoryDocuments");
            $.SiLibrary.GetDepartmentVideoFolders("/Videos", "EHS", "DirectoryVideos");
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

    <div id="HomeWrapper" >

        <section id="sidebar">
            <div class="SideBar-Box" style="height:450px;">

			    <ul>
				    <!--  Live Safety Meeting Link -->
				    <li style="margin-bottom: 40px;">
					    <a href="SafetyMeeting.aspx" >
						    <img  alt="Live Safety Meeting" src="/Images/icon-MeetingLive.png" style="height: 57px; width: 57px;"/>
                                    <br/>Live Meeting
					    </a>  
				    </li>
                    
				    <!--  Recorded Safety Meeting Link -->
<%--				    <li>
					    <a href="/Library/LibVideoPane.aspx?VR=/Videos&Path=Safety" >
						    <img  alt="Previous Safety Meeting" src="/Images/icon-MeetingPrevious.png" style="height: 57px; width: 57px"/>
                                    <br/>Recorded Meetings
					    </a>  
				    </li>--%>
                    
				    <!--  Safety Blogs -->
				    <li style="padding-top: 20px;">
						<img  alt="Safety Blogs" src="/Images/icon-blog.png" style="height: 57px; width: 57px"/>
                        <br/>
                        
                        <a href="/Teams/VPP/VppHome.aspx">VPP News</a><br/>  
                        <a href="/Teams/VEST/VestTeamHome.aspx">VEST News</a>                        

<%--                        <a href="/Blog/Blog.aspx?BlogName=VPP">VPP News</a><br/>  
                        <a href="/Blog/Blog.aspx?BlogName=VPP">VEST News</a>--%>  
				    </li>
                    

                </ul>
            </div>
            
            <div class="SideBar-BoxBorder" style=" float: right; height: 600px;"/>
        </section>        
                
        <section id="HomeMain" >
            
            <section class="ui-widget-header ui-corner-all" style="height: 45px; width: 99%; margin-top: 10px; margin-bottom: 30px;" >
                <a href="/HomePage.aspx" style="text-decoration: none;">
                    <span style="text-decoration: underline;  font-style:italic;   position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">Home</span>
                    <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;margin-top: -10px;">Environmental Health and Safety</div>
                </a>
            </section> 
                
        

            <ul class="DeptGallery2" >                                
                <li>
                    <img    alt="EHS Documents" src="/Images/Home_documents.png" />
                    <ul class="DeptGallery2Links" id="DirectoryDocuments">
                    </ul>
                </li>
                
                <li>
                        <img    alt="EHS Videos" src="/Images/Home_videos.png" />
                        <ul class="DeptGallery2Links" id="DirectoryVideos">
                        </ul>                        
                </li>
                
                
                <li>
                    <img  alt="EHS Documents" src="/Images/Home_teams.png" />
                    <ul class="DeptGallery2Links" id="Ul1">
                        <li>
                            <a href="/Teams/VPP/VppHome.aspx">VPP Team</a>
                        </li>
                        <li>
                            <a href="#">VEST Team</a>
                        </li>
                    </ul>
                </li>
            </ul>
            
                             
            <div style="display: block"><hr/></div>                                                         
        

		    <div id="DeptGallery" style="margin-left:0; " >
		        
			    <ul class="menu_gallery" style="background-color: transparent;">

                    <!-- Safety Pays -->
                    <li style="height: 150px; border: none; margin-right: 5px;">
                            <a href="/Safety/SafetyPays/SubmitSafetyPays.aspx" >
						        <img  style="width: 150px; height: 75px; margin: 0; padding-top: 10px; margin-bottom: 5px;"  alt="Safety Pays" src="/Images/SI-SafetyPays.png" />
					        </a>         
                            
                            <a href="/Safety/SafetyPays/SubmitSafetyPays.aspx" >
                                <div style="font-size: .8em; float: left; font-weight: bold;  width: 100%;  text-decoration: underline; margin-bottom: 5px; padding-left: 14px; ">Submit Report</div>
                            </a>
                        
                            <a href="/Safety/SafetyPays/TabPointsRpt.aspx" >
                                <div style="font-size: .8em; float: left; font-weight: bold;  width: 100%;  text-decoration: underline; margin-bottom: 5px; padding-left: 14px;">Tabulated Points</div>
                            </a>

<%--                            <a href="/Safety/SafetyPays/PointsChart.aspx" >
                                <div style="font-size: .8em; float: left; font-weight: bold;  width: 100%;  text-decoration: underline; margin-bottom: 5px; padding-left: 14px; ">Bar Chart</div>
                            </a>--%>
                        
                                                                  
                    </li>          
                    
                    
                    
                    <!-- Print Badge Or Certification -->
<%--                    <li style="height: 150px; border: none; margin-left: 35px;">
                            <a href="/Corporate/BandC/iTextBasicLookup.aspx" >
                                <img src="/Images/SI-Corp-Certifications.png" alt="Badges and Certs">
					        </a>         
                            
                            <a href="/Corporate/BandC/iTextBasicLookup.aspx" >
                                <div style="font-size: .8em; float: left; font-weight: bold;  width: 50px; width: 100%;  text-decoration: underline; margin-bottom: 5px; ">Get Badge<br /> or Certification</div>
                            </a>

                            <div style="clear: both;"></div>             
                    </li> --%>    

                    
                    <!-- Safety Pays QOM -->
                    <li id="QomLI" runat="server" style="height: 150px; border: none; margin-left: 35px; margin-top: 12px;">
                           
                            <a href="/Safety/SafetyPays/SafetyQomUser.aspx" >
                                <img src="/Images/QOM.png" style="width: 110px; height: 100px;  padding-bottom: 8px; margin-top: 0;" alt="QOM">
                                <div style="font-size: .8em; float: left; font-weight: bold;  width: 100%; width: 100%;  text-decoration: underline; margin-bottom: 5px; ">Submit Response</div>
                            </a>
                            <a href="/Safety/SafetyPays/SafetyQomHistory.aspx" style="font-size: .8em; float: left; font-weight: bold;  width: 100%; width: 100%;  text-decoration: underline !important; margin-bottom: 5px;">Previous Submissions</a>

                            <div style="clear: both;"></div>
                    </li>
                    
                    <!-- MSDS Online -->
                    <li id="Li1" runat="server" style="height: 150px; border: none; margin-left: 35px; margin-top: 12px;">
                           
                            <a href="https://msdsmanagement.msdsonline.com/?ID=3f089f59-50a0-48d9-afdf-4b2ea14c32f2" target="_MSDS">
                                <img src="/Images/msds225.png" style="padding-bottom: 8px; margin-top: 10px;" alt="MSDS Online">
                                <div style="font-size: .8em; float: left; font-weight: bold;  width: 100%; width: 100%;  text-decoration: underline; margin-bottom: 5px; ">MSDS Online</div>
                            </a>

                            <div style="clear: both;"></div>
                    </li>
                                                                  
                    <!-- EHS Document "Blog" -->
                    <li id="Li2" runat="server" style="height: 150px; border: none; margin-left: 0; margin-top: 12px;">
                           
                            <a href="/Safety/EhsBlog.aspx" >
                                <%--<img src="/Images/News2.png" style="padding-bottom: 8px; margin-top: 10px;" alt="EHS Blog">--%>
                                <img src="/Images/Ehs_blog.png" style="padding-bottom: 8px; margin-top: 10px;" alt="EHS Blog">
                                <div style="font-size: .8em; float: left; font-weight: bold;  width: 120%; width: 100%;  text-decoration: underline; margin-bottom: 5px; ">EHS Blog</div>
                            </a>

                            <div style="clear: both;"></div>
                    </li>                   
                            
                </ul>
            </div>

         </section>
        

<%--        <!-- Q. O. M.  -->
        <div class="horizontal_scroller" style="height: 100px; width: 100%; margin: 0;" >
            <div class="scrollingtext"  id="TeamPageMarquee" title="VEST_MARQUEE">
                <span><b>Scrolling text ...</b></span>
                <img src="/Images/Si-EHS-VEST.png" alt="Scrolling Container Example 1" height="50px;" />
                <span><b>Scrolling text ...</b></span>
            </div>
        </div>--%>

    </div>   
    


   <script type="text/javascript">
       var $ = jQuery;
       
       //$('.horizontal_scroller').SetScroller({
       //    velocity: 89,
       //    direction: 'horizontal',
       //    startfrom: 'right',
       //    loop: 'infinite',
       //    movetype: 'linear',
       //    onmouseover: 'play',
       //    onmouseout: 'play',
       //    onstartup: 'play',
       //    cursor: 'default'
       //});
   </script>       

      

</asp:Content>

