<%@ Page Title="Logon Failed" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="logoff.aspx.cs" Inherits="Account_logoff" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <title>Login Failed</title>
    
    <style>
        body,html{
          height:90%;
            background-color: yellow;
        }
        
        #mainarea {
            height: expression( this.scrollHeight < document.body.clientHeight ? "100%" : "auto" );
            min-height: 100%;
            background-color: yellow;
        }
        
    </style>
    
    
    <script type="text/javascript">
        $(document).ready(function () {

            window.onclick = function () {
                window.location.href = window.location.href.replace('logoff.aspx', 'SessionAbandon.aspx');
            };

            try {
                window.onbeforeunload = function () {
                    window.location.href = window.location.href.replace('logoff.aspx', 'SessionAbandon.aspx');
                };
            }

            catch (err) { }





            window.onclick = function () {
                window.close();
                window.location.href = window.location.href;
            };

            try {
                window.onbeforeunload = function () {
                    window.close();
                    return false;
                };
            }

            catch (err) { }



        });        
    </script>    
    

</asp:Content>    
    
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <div style="margin-top: 10px;" id="mainarea">
        <img src="/Images/Stop.png" alt="STOP" style="float: left; margin-right: 20px;"/>
        <img src="/Images/Stop.png" alt="STOP" style="float: right; margin-left: 20px;"/>
        <h2 style="color: red;">You have been signed off from the Shermco YOU! web site..</h2>
        <div style="clear: both;"></div>
    </div>

</asp:Content>        

