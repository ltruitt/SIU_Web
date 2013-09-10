<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="UserClass.aspx.cs" Inherits="Safety_Training_UserClass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Available Safety Classes</title>
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />
    
    <!-- Include one of jTable styles. -->
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
 
    <!-- Include jTable script file. -->
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script> 

    <link rel="stylesheet" type="text/css" href="/Styles/jquery.timeentry.css"> 

    <script type="text/javascript" src="/Scripts/UserClass.js"></script> 
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<pre>
Filter Already Viewed Classes / Quizes
Link To Quiz
Record Video Watched
</pre>    

    <div id="FormWrapper" class="ui-widget ui-form">
                    
        <section style="visibility: hidden; height: 0; width: 0;">
            <span id="hlblEID" runat="server"></span>
        </section>            
            
        <!----------------->
        <!-- Form Header -->
        <!----------------->                   
        <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
            <a href="/My/MyHome.aspx" style="text-decoration: none;">
                <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">MySI</span>
                <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Available Meetings, Classes, and Quizzes</span>
            </a>
        </section>

        <section class="ui-widget-content ui-corner-all">
            <!---------------------------->
            <!-- "Viewer ID" Section -->
            <!---------------------------->
            <div style="float: left; margin-top: -20px; width: 100%;">
                <label style="float: left; padding: 0; width:auto; "  runat="server" ID="lblEmpName"></label><br/>
            </div>
            
            <div style="height: 40px;">
                <span  id="lblErrServer" Class="errorTextCss"  ></span>
            </div>

            <div id="jTableClass" style="margin-bottom: 5px;"></div>
        </section>
    </div>
</asp:Content>

