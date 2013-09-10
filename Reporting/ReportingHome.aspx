<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="ReportingHome.aspx.cs" Inherits="Reporting_ReportingHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Reporting Department Home Page</title>
    
    <style>
        .Box div { position:inherit; }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div id="HomeWrapper">
        
        <!-- Left Side Of Main Content Area -->		  
        <section id="sidebar">
            
         	
		    <!-- Site Shortcut Links -->		
            <div class="SideBar-Box" style="height:300px;">
                    
			    <ul>
		    				                      		             
                    <!------------------------->
                    <!-- Submit a Job Report -->  
                    <!------------------------->	
				    <li style="height: 80px; clear:both;">
					    <a href="/Reporting/SubmitJobReport.aspx">
						    <img style="width: 57px; height: 57px;" alt="Submit Job Report" src="/Images/icon-report-submit.png" id="Img1" />
						    <br/>Submit Job Report
					    </a>
				    </li> 
                    
                    <!--------------------->
                    <!-- Open Job Repots -->  
                    <!---------------------->	
				    <li style="height: 90px; clear:both;">
					    <a href="/My/MyJobRpts.aspx">
						    <img style="width: 57px; height: 57px;" alt="View Job Reports" src="/Images/icon-report-viewopen.png" id="Img2" />
						    <br/>Open Job Reports
					    </a>
				    </li> 
                
                </ul>
            </div>
                    
            <div class="SideBar-BoxBorder" style=" float: right; height: 300px;"/>
            
        </section>
        

        <section id="HomeMain" >
            <section class="ui-widget-header ui-corner-all" style="height: 45px; width: 99%; margin-top: 10px; margin-bottom: 30px;" >
                    <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;">Corporate Services</div>
            </section>     
            
                      

        </section>
        
    </div>
                        
</asp:Content>

