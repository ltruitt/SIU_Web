<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="Blog.aspx.cs" Inherits="Blog_Blog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"> 
    
    <!-- Include one of jTable styles. -->
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
 
    <!-- Include jTable script file. -->
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script>  
    
    <script type="text/javascript" src="/Scripts/Blog.js"></script>    

    <script type="text/javascript"    >
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
    

    <span id="BlogTOC" runat="server"  style="display: none;">???</span>

    <table style="width: 100%;">
        <tr>
            <td style="width: 400px; height: 100%;  vertical-align : top;">
                <div id="jTableContainer"></div>
            </td>
            <td style="background-color: whitesmoke;">
                <span id="BlogItem" runat="server" ></span>    
            </td>   
        </tr>
    </table>


</asp:Content>

