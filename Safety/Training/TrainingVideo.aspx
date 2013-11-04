<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="TrainingVideo.aspx.cs" Inherits="Video_FlowPlayerVideo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script src="/Scripts/fp3216/flowplayer-3.2.12.min.js" type="text/javascript"></script>
    
    <style>
        .restricted {
            overflow-y: hidden;
            overflow-x: hidden;
            width: 550px; 
            height: 550px; 
            border: 2px solid gray; 
            float: right; 
            margin-right: 3px;
        }
    </style>
    

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="videoPlayer" style="float: left; display:block; width:500px; height:400px;  border: 2px solid gray; " ></div>
    <img id='NoVideo' src="/Images/NoVideo.jpg" alt="No Video Available For This Event" style="width: 450px; height: 404px; float: left; display: none"/>
    
    <iframe name='proprofs' id='proprofs' class="restricted" ></iframe>
    <img id='NoQuiz' src="/Images/NoQuiz.jpg" alt="No Quiz Available For This Event" style="width: 450px; height: 404px; float: right; display: none; "/>
    <img id='VidFirst' src="/Images/WatchVideoFirst.jpg" alt="Watch Video First" style="width: 450px; height: 404px; float: right; display: none; "/>

    <div style="clear: both;"></div>
    

    <%------------------------------------------------------%>
    <%--  Load Video Player, Video, And Support Documents --%>
    <%------------------------------------------------------%>
    <script src="/Scripts/TrainingVideo.js" type="text/javascript"></script>
    
    <!-- Hidden Fields -->
    <section style="visibility: hidden; height: 0; width: 0;">
        <span id="hlblEID"           runat="server"/>      
        <span id="hlblName"          runat="server"/>
        <span id="hlblTlUid"         runat="server"/>
    </section>

</asp:Content>

