<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="BlogAdmin.aspx.cs" Inherits="Blog_BlogAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Blog Admin</title> 
    
    <!-- Include one of jTable styles. -->
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
 
    <!-- Include jTable script file. -->
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script>  

    <script type="text/javascript" src="/Scripts/BlogAdmin.js"></script>     
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <span id="BlogTOC" runat="server"  style="display: none;">???</span>

    <section id="HomeMain" style="margin-top: 10px;" >
        <section class="ui-widget-header ui-corner-all" style="height: 45px; width: 99.8%;" >
                <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;">Blog Administration</div>
        </section>      
            
            
        <table style="width: 99.8%;">
            <tr>

                <td style="width: 30%; height: 100%;  vertical-align : top;">
                    <h1 style="background-color: rgb(11, 103, 205); color: white; margin-top:  25px; height: 60px;">Select A Blog To Administer</h1>
                    <hr/>                
                    <ul class="DeptGallery2Links" id="BlogsUl" style="margin-left: 30px;">
                    </ul>   
                </td>   
            
                <td style="width: 5%;"></td>
            
                <td style="width: 49%; height: 100%;  vertical-align : top;">
                    <div id="jTableBlogItems"></div>
                </td>
            </tr>
        </table>

    </section>

</asp:Content>

