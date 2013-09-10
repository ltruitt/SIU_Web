<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="JwPlayerVideo.aspx.cs" Inherits="Video_JwPlayerVideo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script src="/Scripts/JWPlayer-5.10/jwplayer.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <!-- default link to get Adobe Flash Player If No Player Is Currentrly Installed -->
    <!-- Otherfwise Replaced By Adobe Flash Player                                   -->
    <div style="text-align: center; background-color: gray;">
        
        <!-- JW Flash Player Player -->
        <div id="mediaplayer" style="width: 500px; height: 400px; margin: 0px; padding: 0px;">
            <img alt="Get Adobe Flash player" src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif">   
                Follow The Link To Install Flash.                 
        </div>

    <script type="text/javascript">
        jwplayer("mediaplayer").setup(
        {
            flashplayer: "/Scripts/JWPlayer-5.10/player.swf",
            width: 500,
            height: 400,
            file: "http://localhost/videos/WEB/Intro/Basics.flv",
            autostart: "true"            
        });


</script>

</div>


</asp:Content>

