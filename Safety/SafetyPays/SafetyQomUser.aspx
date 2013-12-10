<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="SafetyQomUser.aspx.cs" Inherits="Safety_SafetyPays_SafetyQomUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Question of the Month</title>
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />
    
    <!-- Include one of jTable styles. -->
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
    
    <!-- Include jTable script file. -->
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script> 

    <script type="text/javascript" src="/Scripts/SafetyQomUser.js"></script>  
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/SafetyQomUser.css" />
      
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
 <div id="FormWrapper" class="ui-widget ui-form">
                    
        <section style="visibility: hidden; height: 0; width: 0;">
            <span id="hlblEID" runat="server"></span>
            <label id="hlblUID" runat="server"></label>
            <label id="hlblDept" runat="server"></label>
        </section>
        
        <!----------------->
        <!-- Form Header -->
        <!----------------->                   
        <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
            <a href="/MY/MyHome.aspx" style="text-decoration: none;">
                <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">MySI page</span>
                <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Question of the Month</span>
            </a>
        </section>

        <section class="ui-widget-content ui-corner-all">

            <!---------------------------->
            <!-- "Viewer ID" Section -->
            <!---------------------------->
            <div style="float: left; margin-top: -20px; width: 100%;">
                <label style="float: left; padding: 0; width:auto; "  runat="server" ID="lblEmpName"></label><br/>
        <div id="SuprArea" style="margin-top: 5px; margin-bottom: 20px; display:inline-block;" runat="server" >
            <span style="float:left; margin-right: 20px; font-size: 1.3em; width: auto;  font-weight: bold; display:inline">Select Employee:</span>
            <input ID="ddEmpIds" class="DataInputCss" runat="server"   style="width: 400px;  background-color:yellow; border-color: black;" />         
        </div>
            </div>
            
            <div style="height: 40px;">
                <span  id="lblErrServer" Class="errorTextCss"  ></span>
            </div>
                        
            <div id="container" class="holder">
                
                <div id="column-one" class="even-height">
                    <div id="jTableQomList" style="margin-bottom: 40px;"></div>
                </div>
                <div id="column-two" class="even-height">
                    
                    <div class="QomQDiv">
                        <h2 style="text-decoration: underline;">Answer the Following Question</h2>
                        <label id="QomQ"></label>
                    </div>
                </div>
            </div>
            
            <div style="clear: both;"></div>
            
            <div id="DivResponse" style="margin-left: -10px; margin-top: 10px; font-size: 1.2em;">
                <%--------------------%>                    
                <%-- Submit Buttons --%>                    
                <%--------------------%>
                <div style="margin-top: -30px; margin-left: 10px;">
                    <input type="button" ID="btnSubmit" value="Submit" Class="SearchBtnCSS" style="margin-top: 15px; margin-right: 15px;" />
                    <h3 style="color: antiquewhite;  margin-bottom: 0; float: left;">Your Response......</h3>
                </div>
                
                <textarea id="txtResponse" Class="QomRDiv"  style="width: 98%; height: 200px; margin: 0;"></textarea>
                <label id="QomR">
                    
                </label>
            </div>
            
            <div>
                <label id="ehsResponse" style="margin-top: 10px; color: antiquewhite; margin-left: 0; font-size: 1.2em; font-weight: bold;">
                </label>
            </div>

            <div style="clear: both;"></div>
        </section>
    </div>
</asp:Content>

