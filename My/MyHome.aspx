<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="MyHome.aspx.cs" Inherits="My_MyHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
     <title>My Shermco Home Page</title>

     <link rel="stylesheet" href="/styles/MyHome.css">
     <script type="text/javascript" src="/Scripts/MyHome.js"></script>

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div id="SuprArea" style="margin-top: 5px;" runat="server" >
        <span style="float:left; margin-right: 20px; width: auto;  font-weight: bold; display:inline">Change Employee:</span>
        <input ID="ddEmpIds" class="DataInputCss" runat="server" style="width: 400px;" />         
    </div>
    

    <div id="HomeWrapper">
    
        <!-- Hidden Labels Keeping Selected Charge Account From DD Selections -->
        <section style="visibility: hidden;">
            <span runat="server" id="hlblEID"/> 
        </section>

        <!-- Left Side Of Main Content Area -->		  
        <section id="sidebar">
            
         	
		    <!-- Site Shortcut Links -->		
            <div class="SideBar-Box" style="height:450px;">
                    
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
						    <img style="width: 57px; height: 57px;" alt="ELO" src="/Images/icon-mail.png" id="Img2" />
						
						        <br/>Mail
					    </a>
				    </li> 
                
                    <!------------->
                    <!-- PayStub -->  
                    <!------------->	
				    <li style="height: 90px; clear:both;">
					    <%--<a href="/My/MyPayStubs.aspx">--%>
					    <a href="http://portal.shermco.com" target="_portal">    
						    <img style="width: 77px; height: 57px;" alt="Paystub" src="/Images/icon-paystub.png" id="Img3" />
						        <br/>PayStubs
					    </a>
				    </li> 
                    
                    <!-------->
                    <!-- HR -->  
                    <!-------->	
				    <li style="height: 90px; clear:both;">
					    <a href="/HR/HrHome.aspx">
						    <img style="width: 57px; height: 57px;" alt="HR Home" src="/Images/icon-hr-sidebar.png" id="Img4" />
						        <br/>Human Resources
					    </a>
				    </li>                     
                                    
                    <!-------------------------->
                    <!-- Personal Vehical Use -->  
                    <!-------------------------->	
				    <li style="height: 60px; clear:both;">
					    <a href="/My/PersonalUseInfo.aspx">
						    <img style="width: 127px; height: 57px; " alt="Personal Use" src="/Images/icon-vehicle2.png" id="Img5" />
						    Personal Use of Vehicles FAQ
					    </a>
				    </li> 
                </ul>
            </div>
                    
            <div class="SideBar-BoxBorder" style=" float: right; height: 600px;"/>
            
        </section>

        <!-- Menu -->
        <div id="MyHomeNotifications" class="AdminMenu"  style="background-color: white; margin-top: -0px; ">
            
            <div class="ui-widget-header ui-corner-all" >
                <div  style="text-align: center; font-size: 2em;">My Shermco</div>
            </div>      
            
            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                <img id="WaitImg" src="/Images/slider-loading.gif" alt="Please Wait" />
            </div>

            <div class="toolbar">
			    <ul>
                    
                    <li class="l8" id="LiSafetyPays" >
                        <span id="Span2"  runat="server" style="text-align: center; color: black;">Safety Pays<br/></span>
                        
                        <div style="text-align: left; margin-left: 20px; ">
                            
                            <div>
                                <a href="/Safety/SafetyPays/SafetyPaysWorking.aspx?isA=0" ID="A17" style="height: 20px;">
                                    <span style="width: 100px; font-weight: normal; font-size: 1em; display: inline-block; margin-left: -5px;">Opened By Me: </span>
                                    <span id="SpMyOpen" style="text-align: center; width: 20px; display: inline-block;"/>
                                </a>
                            </div>
                            
                            <div>
                                <a href="/Safety/SafetyPays/SafetyPaysWorking.aspx?s=ass&isA=0" ID="A1"  style="height: 18px;">
                                   <span style="width: 100px; font-weight: normal; font-size: 1em; display: inline-block; margin-left: -5px;">Assigned to Me: </span>
                                    <span id="SpMyAssigned" style="text-align: center; width: 20px; display: inline-block;" /><br/>
                                </a>
                            </div>
                            
                            <div>
                                <a href="/Safety/SafetyPays/SafetyPaysWorking.aspx?s=tsk&isA=0" ID="A18"  style="height: 18px;">
                                    <span style="width: 100px; font-weight: normal; font-size: 1em; display: inline-block; margin-left: -5px;">Late Task: </span>
                                    <span id="SpMyLateTask" style="text-align: center; color: red; width: 20px; display: inline-block;"/>
                                </a>
                            </div>
                            
                            <div>
                                <a href="/Safety/SafetyPays/SafetyPaysWorking.aspx?s=sta&isA=0" ID="A19"  style="height: 18px;">
                                    <span style="width: 100px; font-weight: normal; font-size: 1em; display: inline-block; margin-left: -5px;">Late Status: </span>
                                    <span id="SpMyLateStatus" style="text-align: center;  color: red; width: 20px; display: inline-block;"/>
                                </a>
                            </div>
                        </div>
                        
                    </li>
                    
                                        

                    <li id="l1AccruedTime">
                        <span id="Span1"  runat="server" style="text-align: center; ">Accrued Time<br/></span>
                        
                        <div style="text-align: left; margin-left: 20px;">
                            <div style="width: 75px; font-weight: normal; font-size: 1em; display: inline-block; margin-left: 10px;">Holiday: </div>
                            <span id="PHoliday"  runat="server" style="text-align: center; width: 20px; display: inline-block;"/><br/>
                        
                            <div style="width: 75px; font-weight: normal; font-size: 1em; display: inline-block;  margin-left: 10px;">Vacation: </div>
                            <span id="Vacation"  runat="server" style="text-align: center; width: 20px; display: inline-block;"/><br/>

                            <div style="width: 75px; font-weight: normal; font-size: 1em; display: inline-block;  margin-left: 10px;">Sick: </div>
                            <span id="Sick"  runat="server" style="text-align: center; width: 20px; display: inline-block;"/>
                        </div>
                    </li>
                				
                    <li class="l1" id="liTimeStudy" style=" font-weight: bold;">
                        <a href="/My/MyTimeStudy.aspx" ID="A3"  runat="server" style="font-weight: bold; text-align: center; width: 100%; display: inline-block; margin-top: 40px;">
                         View Time Study
                        </a>
                    </li> 
                    
                    <li class="l1" id="liBandC" style=" font-weight: bold;">
                        <a href="/Corporate/BandC/AllBandC_Rpt.aspx" ID="A2"  runat="server" style="font-weight: bold; text-align: center; width: 100%; display: inline-block; margin-top: 40px;">
                         All My Badges and Certifications
                        </a>
                    </li>
                                    
                    <li class="l1" id="liMyYtdExp" style=" font-weight: bold;">
                        <a href="/My/MyExpenses.aspx" ID="A16"  runat="server" style="font-weight: bold; text-align: center; width: 100%; display: inline-block; margin-top: 40px;">
                            View Y.T.D. Expenses
                        </a>
                    </li>    
                
                    <li class="l1" id="liVehicleMileageReport">
                        <a href="/My/MyVehMileage.aspx" ID="A14"  runat="server">
                            Reported Vehicle Miles For This Year
                        </a>
                    </li>

                    <li class="l1" id="liExpCntExpAmt">
                        <a href="/ELO/MealsExpEntry.aspx" ID="A15"  runat="server">
                            You have 
                            <span id="ExpCnt"  runat="server"/> 
                            open expenses for
                            <span id="ExpAmt"  runat="server"/> 
                        </a>
                    </li> 

                    <li class="l8" id="liHoursThisWeek_HoursToday">
                        <a href="/ELO/TimeEntry.aspx" ID="A4"  runat="server">
                            You Reported
                            <span id="HoursThisWeek"  runat="server"/>
                            this week.
                            <span id="HoursToday"  runat="server" style="font-style:italic; color: red; font-size: 1.1em;"/> 
                         
                        </a>
                    </li>  




                    <li class="l8" id="liVehIssues">
                        <a href="/ELO/VehDotEntry.aspx" ID="A13"  runat="server">
                            There are 
                            <span id="VehIssues"  runat="server"/>
                            Open Issues With Your Assigned Vehicle.
                        </a>
                    </li>  




                    <li class="l8" id="liOpenBugReq">
                        <a href="/My/MyBugReport.aspx" ID="A5"  runat="server">
                            You have 
                            <span id="OpenBugReq"  runat="server"/>  
                            open
                        </a>
                    </li>
                

                    <li class="l8" id="liOpenHwReq">
                        <a href="/My/MyHwReq.aspx" ID="A6"  runat="server">
                            You have 
                            <span id="OpenHwReq"  runat="server"/> 
                            open
                        </a>
                    </li>
                

                    <li class="l8" id="liExpiringBandC">
                        <a href="/My/MyExpiringBandC.aspx" ID="A7"  runat="server">
                            You have 
                            <span id="ExpiringBandC"  runat="server"/> 
                            expiring soon
                        </a>
                    </li>
                    
                    <li class="l8" id="liMissedMeetings">
                        <a href="/My/MissedSafetyMeetings.aspx" ID="A12"  runat="server">
                            You Missed 
                            <span id="MissedMeetings"  runat="server"/> 
                            Safety Meetings
                        </a>
                    </li>
                
                    <li class="l8" id="liVehicleMileageReported">
                        <a href="/ELO/VehMileage.aspx" ID="A10"  runat="server">
                            You 
                            <span id="VehicleMileageReported"  runat="server"/> 
                            Report Vehicle Mileage Last Week
                        </a>
                    </li>
                
                    <li class="l8" id="liRejectedTime">
                        <a href="/ELO/TimeRpt.aspx" ID="A8"  runat="server">
                            You Have 
                            <span id="RejectedTime"  runat="server"/> 
                        </a>
                    </li>
                
			        <li class="l8" id="liJobRptsInProgress">
			            <a href="/My/MyJobRpts.aspx" ID="A11"  runat="server">
			                You Have 
                            <span id="JobRptsInProgress"  runat="server"/> 
                            In Process
			            </a>
			        </li>

                    <li class="l8" id="liJobRptsPastDue" >
                        <a href="/My/MyPastDueJobRpts.aspx" ID="A9"  runat="server">
                            You Owe 
                            <span id="JobRptsPastDue"  runat="server"/> 
                        </a>
                    </li>
                
<%--                
                <li class="l30"><a href="#" ID="A2"  runat="server">You have 7 Safety Pays Reports Open and 0 Working</a></li>
                <li class="l30"><a href="#" ID="A3"  runat="server">Your EHS Monthly Question Scored 1 Point</a></li>
                <li class="l30"><a href="#" ID="A12"  runat="server">You Have 3 IT Sysaid Tasks Open</a></li>
                <li class="l30"><a href="#" ID="A13"  runat="server">You Have 1 Facilities Sysaid Tasks Open</a></li>
                --%>

			    </ul>
            </div>      
        </div>
        
        
        <%--------------------%>                    
        <%-- Submit Buttons --%>                    
        <%--------------------%>
        <div style="width: 100%;  display: inline-block; margin-top: 10px;">
            <div style="float: left; width: 10%; ">
                <input type="button" ID="btnShowAll" value="Show All" Class="SearchBtnCSS" />
            </div>
            
        </div>
        
        

    </div>   

</asp:Content>

