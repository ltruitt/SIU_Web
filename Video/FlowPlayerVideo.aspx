<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="FlowPlayerVideo.aspx.cs" Inherits="Video_FlowPlayerVideo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script src="/Scripts/fp3216/flowplayer-3.2.12.min.js" type="text/javascript"></script>
    <style>
         .aButton {
            font: bold 18px Arial;
            text-decoration: none;
            background-color: #EEEEEE;
            color: #333333;
            padding: 2px 6px 2px 6px;
            border-top: 3px solid #CCCCCC;
            border-right: 3px solid #333333;
            border-bottom: 3px solid #333333;
            border-left: 3px solid #CCCCCC;
           }        
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div style="float: left; display:block; width:500px; height:400px;" id="videoPlayer"></div>
    
    

    <div style="float: left; text-align: center; width: auto;  padding-left: 30px; padding-right: 30px;">
        <h2>Video Support Documents</h2>
        <br/>
        <div >
            <span id="SupportDocumentsInsertPoint"></span>
        </div>
    </div>
    
    <div style="clear: both;"></div>
    

    <%------------------------------------------------------%>
    <%--  Load Video Player, Video, And Support Documents --%>
    <%------------------------------------------------------%>
    <script src="/Scripts/VideoPlayers.js" type="text/javascript"></script>
    
    <!-- Hidden Fields -->
    <section style="visibility: hidden; height: 0; width: 0;">
        <span id="hlblEID"           runat="server"/>      
    </section>
</asp:Content>

