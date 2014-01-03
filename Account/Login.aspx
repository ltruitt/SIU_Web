<%@ Page Title="Log In" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="Login.aspx.cs"     Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
    <title>Shermco You Login</title>

    <link rel="stylesheet" href="/Scripts/JqueryUI/development-bundle/themes/trontastic/jquery.ui.all.css" />
    <link rel="stylesheet" href="/styles/jquery.ui.form.css">
    
    <link href="/Styles/login.css" rel="stylesheet"  type="text/css" />
    <script>
        $(document).ready(function () {
            $('#UserName').focus();
        });
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <div style="background-color: #45473f;">
        
        <%--Put Here To Define Default Button--%>
        <asp:Panel ID="Panel1" runat="server" DefaultButton="LoginButton">
    
            <div id="FormWrapper" class="ui-widget ui-form" style="width: 320px;">
    
                <p></p>

                <div class="ui-widget-content ui-corner-all" >
                
                    <div class="failureNotification" style="margin-top: -10px;" >
                        <asp:Literal ID="FailureText" runat="server" ></asp:Literal>
                    </div>

                    <div style="margin-top: 35px;"/>
            
                            <div class="accountInfo">
                                <fieldset class="login">
                                    <legend style="color: white;">Account Information</legend>

                                    <p id="eidBlk">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Employee ID</asp:Label>
                                        <asp:TextBox ID="UserName" type="number" ClientIDMode="Static" runat="server" CssClass="DataInputCss DateEntryCss" MaxLength="5"></asp:TextBox>
                                    </p>
                                
                                    <p  id="pwdBlk" runat="server">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                        <asp:TextBox ID="Password" type="number" ClientIDMode="Static" runat="server" CssClass="DataInputCss DateEntryCss" TextMode="Password"></asp:TextBox>
                                    </p>

                                </fieldset>
                            
                            
                                <p class="submitButton">
                                    <asp:Button ID="LoginButton" ClientIDMode="Static" runat="server" Text="Log In" OnClick="LoginButton_Click"/>
                                </p>

                            
                                <div style="clear: both;"></div>
                            </div>
                
                </div>
            </div>
        </asp:Panel>
    </div>    

</asp:Content>
