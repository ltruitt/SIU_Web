<%@ Page Title="" Language="C#" MasterPageFile="/Phone/PhoneMaster.master" AutoEventWireup="true" CodeFile="TimeRpt.aspx.cs" Inherits="ELO_TimeRpt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Time Card Report</title>
    
    <link href="/Phone/Styles/ELO.css" rel="stylesheet"  type="text/css" />
    <script type="text/javascript" src="/Scripts/TimeRpt.js"></script> 
    
    <!-- Include one of jTable styles. -->
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
 
    <!-- Include jTable script file. -->
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script>  
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div id="SuprArea" style="margin-top: 5px;" runat="server" >
        <span style="float:left; margin-right: 20px; width: auto;  font-weight: bold; display:inline">Change Employee:</span>
        <input ID="ddEmpIds" class="DataInputCss" runat="server"  style="width: 400px;" />         
    </div>

    <div style="background-color: #45473f; ">
        <div id="FormWrapper" class="ui-widget ui-form">
        
            <section style="visibility: hidden; height: 0; width: 0;">
                <span id="hlblEID"           runat="server"></span>
            </section>            
            
            <!----------------->
            <!-- Form Header -->
            <!----------------->                   
            <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
                <a href="/Phone/ELO/MainMenu.aspx" style="text-decoration: none;">
                    <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">elo menu</span>
                    <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Day Total</span>
                </a>
            </section>
            
    
            <section class="ui-widget-content ui-corner-all">

                <!---------------------------->
                <!-- "Viewer ID" Section -->
                <!---------------------------->
                <div style="float: left; margin-top: -20px; width: 100%;">
                    <label runat="server" ID="lblEmpName" ></label><br/>
                    <span ID="imgReport" runat="server"  style="float: right; margin-left: 7px; "  >
                        <a href="/Phone/ELO/TimeEntry.aspx" style="color:white; font-weight: bold;">Time Entry</a>
                    </span> 
                </div>

                <p style="height: 5px;"></p> 
                
                <div class="TimeRow" style="display: none;">
                    
                    <div style="padding: 0; width: 107%;  margin-left: -10px;">
                        <span style="display: inline-block; margin-left: 15px; margin-right: 10px; line-height: 20px; vertical-align: top;">Show Time For:</span>
                        
                        <div style="display: inline-block;">
                            <input type="text" id="datepicker" Class="DataInputCss DateEntryCss date-pick"  runat="server" style="width: 100px;" />
                            <a></a>
                        </div>

                    </div>
                    
                 </div>
                 
                <div style="color: white; font-size: smaller;"  id="TimeRptDtlDiv">                   
                    <span style="width: 80px; font-weight: bold; display:inline-block;">Dept:</span>
                    <span id="lblDeptSelection" runat="server"></span><br/>
                    
                    <span style="width: 80px; font-weight: bold; display:inline-block;">Job:</span>
                    <span id="lblJobNoSelection" runat="server"></span><br/>
                    
                    <span style="width: 80px; font-weight: bold; display:inline-block;">Task:</span>
                    <span id="lblTaskCodeSelection" runat="server"></span><br/>
                    
                    <span style="width: 80px; font-weight: bold; display:inline-block;">O/H:</span>
                    <span id="lblOhAcctSelection"  runat="server"></span><br/>
                    
                    <span style="width: 80px; font-weight: bold; display:inline-block;">Status:</span>
                    <span  id="lblStatusSelection"  runat="server"></span><br/>
                </div> 

                 <div id="jTableContainer"></div>

             </section> 
             
                            

        </div>

    </div>    
</asp:Content>

