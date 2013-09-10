<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="SafetyMeeting.aspx.cs" Inherits="Safety_SafetyMeeting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Safety Meeting Live Video Feed</title>    
    <link rel="stylesheet" href="/Styles/SafetyHome.css" type="text/css"/>      
    
    <script type='text/javascript' src='/Scripts/SafetyMeeting.js'></script>
    <script type='text/javascript' src='http://shermco.gdlcdn.com/mediaplayer/jwplayer.js'></script>
     
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
            
    <div id='mediaspace'>Live Meeting</div>

    <script type="text/javascript">

        jwplayer('mediaspace').setup({
            'flashplayer': 'http://shermco.gdlcdn.com/mediaplayer/player.swf',
            'file': 'shermco.flv',
            'provider': 'rtmp',
            'streamer': 'rtmp://streamcast.gdlcdn.com/2039A4',
            'rtmp.subscribe': 'true',
            'controlbar': 'bottom',
            'playlist': 'none',
            'dock': 'true',
            'autostart': 'true',
            'icons': 'true',
            'quality': 'true',
            'width': '640',
            'height': '480'
        });
        

    </script>

</asp:Content>

