<%@ Page Title="" Language="C#" MasterPageFile="/Phone/PhoneMaster.master" AutoEventWireup="true" CodeFile="SafetyQomUser.aspx.cs" Inherits="Safety_SafetyPays_SafetyQomUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Question of the Month</title>
    <link rel="stylesheet" href="/Phone/Styles/Phone.css"/>
    
    <!-- Include one of jTable styles. -->
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
    
    <!-- Include jTable script file. -->
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script> 

    <script type="text/javascript" src="/Scripts/SafetyQomUser.js?0001"></script>  
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/SafetyQomUser.css" />
      
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
 <div id="FormWrapper" class="ui-widget ui-form">
                    
        <section style="visibility: hidden; height: 0; width: 0;">
            <span id="hlblEID" runat="server"></span>
            <label id="hlblUID" runat="server"></label>
            <label id="hlblDept" runat="server"></label>
            <label id="hlblIncNo" runat="server"></label>
        </section>
        
        <!----------------->
        <!-- Form Header -->
        <!----------------->                   
        <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
            <a href="/Phone/HomePage.aspx" style="text-decoration: none;">                    
                <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 15px;">home</span>
                <span  style="text-align: center; font-size: 1.7em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto; margin-top: 5px;">Question of the Month</span>
            </a>
        </section>

        <section class="ui-widget-content ui-corner-all">

            <!---------------------------->
            <!-- "Viewer ID" Section -->
            <!---------------------------->
            <div style="float: left; margin-top: -20px; width: 100%; height: 20px;">
                <label style="float: left; padding: 0; width:auto; "  runat="server" ID="lblEmpName"></label><br/>
            </div>
            
            <div style="height: 40px;">
                <span  id="lblErrServer" Class="errorTextCss"  ></span>
            </div>
                    
            
            <div id="jTableQomList" style="margin-bottom: 10px;"></div>
                                
            <div style="text-decoration: underline; font-size: 1.2em; text-align: center; font-weight: bold;">Answer the Following Question</div>
            <label id="QomQ" style="font-weight: bold;"></label>
            
            <div style="clear: both; height: 20px;"></div>

            <div id="DivResponse" style="margin-left: -10px; margin-top: 0; font-size: 1em;">
                
                <div style="margin-top: -30px; margin-left: 10px;">
                    <h3 style="color: rgb(45, 137, 239);  margin-bottom: 0; ">Your Response......</h3>
                    <h3 style="color: whitesmoke;         margin-bottom: 0; margin-top: -22px; margin-left: 2.1px; ">Your Response......</h3>
                    
                    <%--------------------%>                    
                    <%-- Submit Buttons --%>                    
                    <%--------------------%>
                    <div style="margin-top: -30px; margin-left: 10px; margin-bottom: 30px;">
                        <input type="button" ID="btnSubmit" value="Submit" Class="SearchBtnCSS" style="float: right;" />
                    </div>
                </div>
                                
                <textarea id="txtResponse" Class="QomRDiv"  style="width: 93%; height: 100px; margin: 0; border-color: rgb(45, 137, 239) !important; background-color: whitesmoke !important; "></textarea>
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

