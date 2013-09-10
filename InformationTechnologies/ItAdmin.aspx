<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="ItAdmin.aspx.cs" Inherits="InformationTechnologies_ItAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
     <title>Information Technologies Administration Page</title>
     <link rel="stylesheet" href="/Scripts/JqueryUI/development-bundle/themes/trontastic/jquery.ui.all.css" />
     <link rel="stylesheet" href="/Forms/styles/jquery.ui.form.css">    
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
        <!-- Admin Tasks -->
        <div id="ItAdmin" class="AdminMenu"  style="background-color: white; margin-top: -0px; ">
            
            <div class="ui-widget-header ui-corner-all" >
                <div  style="text-align: center; font-size: 2em;">I. T. Administration</div>
            </div>
        

			<ul>
				<li class="l2">
				    <a href="/Forms/HardwareRequestAdmin.aspx" id="SafetyPaysAdminLink">
                        <img src="/Images/NewComputer2.png" style="border: none; height: 40px; float: left;"/>
                        Computer Requests
                    </a>
				</li>
			</ul>
            
			<ul>
				<li class="l2">
				    <a href="/Forms/BugReportList.aspx" id="A1">
				        <img src="/Images/bug1.png" style="border: none; height: 40px; float: left;"/>
				                   Bugs / Suggestions
				    </a>
				</li>
			</ul>

        </div>

</asp:Content>

