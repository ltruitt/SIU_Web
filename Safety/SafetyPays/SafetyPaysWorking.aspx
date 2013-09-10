<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="SafetyPaysWorking.aspx.cs" Inherits="Safety_SafetyPays_SafetyPaysWorking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>In-Process Safety Pays Reports</title>
    
    <link href="/Styles/SubmitTimeRpt.css" rel="stylesheet"  type="text/css" />
    
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script>     
    
    <script type="text/javascript" src="/Scripts/SafetyPaysWorking.js"></script>

    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />

    <style>
         .footer_gallery { display: none; }
         div.jtable-main-container table.jtable thead th { text-align:  center;  }
        .center { text-align: center; }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <div id="FormWrapper" class="ui-widget ui-form">
                    
        <section style="visibility: hidden;">
            <span id="hlblEID"    runat="server"/>  
            <span id="hlblKey"    runat="server"/>
            <span id="hlblAdmin"  runat="server"/>
        </section>            
            
        <!----------------->
        <!-- Form Header -->
        <!----------------->                   
        <section id="AdminHeader" class="ui-widget-header ui-corner-all" style="height: 45px; display: none;" >
            <a href="/Safety/SafetyAdminHome.aspx" style="text-decoration: none;">
                <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">admin page</span>
                <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">(Administrators) In Progress Safety Pays Reports</span>
            </a>
        </section>
        
        <section id="Userheader" class="ui-widget-header ui-corner-all" style="height: 45px; display: none;" >
            <a href="/My/MyHome.aspx" style="text-decoration: none;">
                <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">MySI</span>
                <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">In Progress Safety Pays Reports</span>
            </a>
        </section>


        <section class="ui-widget-content ui-corner-all">

            <!---------------------------->
            <!-- "Viewer ID" Section -->
            <!---------------------------->
            <div style="float: left; margin-top: -20px; width: 100%;">
                <label style="float: left; padding: 0; width:auto; "  runat="server" ID="lblEmpName"></label><br/>
            </div>

            <p style="height: 5px;"></p> 



            <!-- Filter Reports Showing -->
            <div id="searchDiv" style="display: inline-block; margin-top: 5px;">

                <div style="float: left; width: 170px;  ">
                    <input type="button" ID="btnNoTaks" value="No Task Assigned" Class="SearchBtnCSS" style="width: 165px; color: blue;" />
                </div>
                                                                            
                <div style="float: left; width: 170px; ">
                    <input type="button" ID="btnLateTasks" value="Late Tasks" Class="SearchBtnCSS"  style="width: 165px; color: red;" />
                </div>

                <div style="float: left; width: 170px; ">
                    <input type="button" ID="btnLateSta" value="Late Status" Class="SearchBtnCSS"  style="width: 165px; color: red;" />
                </div>

                <div style="float: left; width: 170px; ">
                    <input type="button" ID="btnRdyToCloseTasks" value="Ready To Close" Class="SearchBtnCSS"  style="width: 165px; color: blue;" />
                </div>


                <div style="float: left; width: 170px; ">
                    <input type="button" ID="btnAllCurrenttasks" value="Current Status" Class="SearchBtnCSS" style="width: 165px; color: green;"/>
                </div>
                
                <div style="float: left; width: 170px;">
                    <input type="button" ID="btnAssigned" value="Assigned" Class="SearchBtnCSS"  style="width: 165px; color: green;" />
                </div>

                <div style="float: left; ">
                    <input type="button" ID="btnAllTasks" value="All" Class="SearchBtnCSS"  style="width: 165px; color: green;" />
                </div>
            </div>


            <p style="height: 5px;"></p> 

            <!-- Show Report Detail -->
            <div style="color: white; font-size: smaller;"  id="DtlDiv">  
                    
                <div style="display: inline-block; width: 100%;">
                    <div style="width: 300px; display: inline-block;">
                        <span style="width: 80px;  font-weight: bold; display:inline-block;">Rpt Type:</span>
                        <span id="lblIncTypeTxt"></span>
                    </div>
                    <div style=" display: inline-block;">
                        <span style="width: 80px; font-weight: bold; display:inline-block;">Location:</span>
                        <span  id="lblJobSite"></span>
                    </div>
                </div>


                <div style="display: inline-block; width: 100%">
                    <div style="width: 300px; display: inline-block;">
                        <span style="width: 80px; font-weight: bold; display:inline-block;">Opened:</span>
                        <span id="lblIncOpenTimestamp"></span>
                    </div>
                    <div style="width: 300px; display: inline-block;">                    
                        <span style="width: 80px; font-weight: bold; display:inline-block;">Occured:</span>
                        <span id="lblIncidentDate"></span>
                    </div>
<%--                    <span style="width: 80px; font-weight: bold; display:inline-block;">Last Status Req.:</span>
                    <span id="lblSafetyMeetingDate" runat="server" />--%>
                </div>


                <div style="display: inline-block; width: 100%;">
                    <div style="width: 300px; display: inline-block;">
                        <span style="width: 80px; font-weight: bold; display:inline-block;">Reporter:</span>
                        <span id="lblReportedByEmpName"></span>
                    </div>
                    <div style="width: 300px; display: inline-block;">
                        <span style="width: 80px; font-weight: bold; display:inline-block;">Observed:</span>
                        <span id="lblObservedEmpName"></span>
                    </div>

<%--                    <span style="width: 80px; font-weight: bold; display:inline-block;">Last Status Rpt:</span>
                    <span id="lblSafetyMeetingType"  runat="server" />--%>
                </div>

                <p></p>
                
                <div>
                    <span style="width: 80px; font-weight: bold; display:inline-block;">Comments:</span>
                    <span  id="lblComments"> </span><br/>
                    <span style="width: 120px; font-weight: bold; display:inline-block;">Initial Response:</span>
                    <span  id="lblInitialResponse"></span><br/>
                </div>

            </div> 



            <!-- Open Task Management Screen -->
            <div id="cmdDiv" style="display: inline-block; margin-top: 5px;">

                <div style="float: left; width: 175px;  ">
                    <input type="button" ID="btnMngTasks" value="Manage Tasks" Class="SearchBtnCSS" style="width: 170px; color: blue;" />
                </div>

                <div style="float: left; width: 175px;">
                    <input type="button" id="btnCloseRpt" value="Close Report" Class="SearchBtnCSS" style="width: 170px; color: red;"  />
                </div>                                                           
            </div>



            <!-- List Of In Process Safety Pays Reports -->
            <div style="width: 100%; padding-top: 20px; margin-left: auto; margin-right: auto;">
                <div id="jTableContainer"></div>
            </div>
        </section>
            
    </div>


</asp:Content>

