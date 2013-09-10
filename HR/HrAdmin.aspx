<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="HrAdmin.aspx.cs" Inherits="HR_HrAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
     <title>HR Adminisration Page</title>
     <link rel="stylesheet" href="/Scripts/JqueryUI/development-bundle/themes/trontastic/jquery.ui.all.css" />
     <link rel="stylesheet" href="/Forms/styles/jquery.ui.form.css">       
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <!-- Admin Tasks -->
        <div id="SafertAdmin" class="AdminMenu"  style="background-color: transparent; margin-top: -0px; ">
            
            <div class="ui-widget-header ui-corner-all" >
                <div  style="text-align: center; font-size: 2em;">Human Resources Administration</div>
            </div>
        

			<ul>
				<li class="l2"><a href="/Forms/QualCodes.aspx" id="BandCCodesLink">&nbsp;Badge and Certification Codes</a></li>
                <li class="l2"><a href="/Forms/NewsBlog.aspx?BlogID=HR" id="BlogLink">Home Page Blog</a></li>
			</ul>
        </div>      
</asp:Content>

