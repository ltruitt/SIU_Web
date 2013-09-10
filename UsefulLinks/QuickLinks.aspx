<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="QuickLinks.aspx.cs" Inherits="UsefulLinks_QuickLinks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <center>
        <br/>
        <div id="PhoneMenu" class="PhoneMenu" style="display:inline; ">
			<ul>
				<li class="l3"><a href="/Forms/Jobs.aspx" id="PhoneJobsIcon">Complete Job</a></li>
				<li class="l3"><a href="/Forms/HardwareRequest.aspx">Computer Request</a></li>
                <li class="l3"><a href="/Forms/SafetyPays.aspx">Safety Pays Report</a></li>
			</ul>
        </div>
        <br/>
        </center>    
</asp:Content>

