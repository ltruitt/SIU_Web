<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="FlowPlayerVideo.aspx.cs" Inherits="Video_FlowPlayerVideo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script src="/Scripts/fp3216/flowplayer-3.2.12.min.js" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div style="float: left; display:block; width:500px; height:400px;" id="videoPlayer"></div>
    
    

    <div style="float: left; text-align: center; width: auto;  padding-left: 30px; padding-right: 30px;">
        <h3>Video Support Documents</h3>
        <div style="text-align: center; width: 100%; background-color: yellow;">
            <span id="SupportDocumentsInsertPoint"></span>
        </div>
    </div>
    
    <div style="clear: both;"></div>
    

    <%------------------------------------------------------%>
    <%--  Load Video Player, Video, And Support Documents --%>
    <%------------------------------------------------------%>
    <script src="/Scripts/VideoPlayers.js" type="text/javascript"></script>
    
    <!-- Hidden Fields -->
    <section style="visibility: hidden; height: 0px; width: 0px;">
        <span id="hlblEID"           runat="server"/>      
    </section>
</asp:Content>

