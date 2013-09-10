<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="SafetyPaysTasks.aspx.cs" Inherits="Safety_SafetyPays_SafetyPaysTasks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

    <title>Safety Pays Report Task Management</title>
    
    <link href="/Styles/SubmitTimeRpt.css" rel="stylesheet"  type="text/css" />
    
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script>     
    <script type="text/javascript" src="/Scripts/JTable_Extend_ShowEditForm.js"></script>  

    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/SafetyPaysTasks.css" />

    <script type="text/javascript" src="/Scripts/SafetyPaysTasks.js"></script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <div id="FormWrapper" class="ui-widget ui-form">
                    
        <section style="visibility: hidden;">
            <span id="hlblEID"     runat="server"/>  
            <span id="hlblKey"     runat="server"/>
            <span id="hlblTaskNo"  runat="server"/>
            <span id="hlblAdmin"   runat="server"/>
        </section>            

        <!----------------->
        <!-- Form Header -->
        <!----------------->                   
        <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
            <a id="hrefBack" href="/Safety/SafetyPays/SafetyPaysWorking.aspx?isA=" style="text-decoration: none;">
                <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">Task List</span>
                <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Safety Pays Task Management</span>
            </a>
        </section>

        <section class="ui-widget-content ui-corner-all">

            <!---------------------------->
            <!-- "Viewer ID" Section -->
            <!---------------------------->
            <div style="float: left; margin-top: -20px; width: 100%;">
                <label style="float: left; padding: 0px; width:auto; "  runat="server" ID="lblEmpName"></label><br/>
            </div>

            <p style="height: 10px;"></p> 

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

                </div>

                <p></p>

                <span style="width: 80px; font-weight: bold; display:inline-block;">Comments:</span>
                <span  id="lblComments"></span><br/>

                <span style="width: 120px; font-weight: bold; display:inline-block;">Initial Response:</span>
                <span  id="lblInitialResponse"></span><br/>

            </div> 

            <!-- List Of Newly Submitted Safety Pays Reports -->
            <div class="TaskInfoBox">
                <span style="font-size: 1.5em;">C</span>
                <span style="font-size: 1.0em; margin-left: -5px;">lick On A Task To </span>
                
                <span style="font-size: 1.5em; color: blue; ">View</span>
                or 
                <span style="font-size: 1.5em; color: blue; ">Submit</span>
                Status Reports
            </div>

            <div style="width: 100%; padding-top: 0; margin-left: auto; margin-right: auto;">
                <div id="jTableTasks"></div>
            </div>

            <div id="jTableStatusDiv" style="width: 100%; margin-top: 40px; margin-left: auto; margin-right: auto; display: none;">
                <div id="jTableStatus"></div>
            </div>


        </section>
            
    </div>


    <div id="popup-box" class="popup">
        <a href="#" class="close"><img src="/Images/Delete.png" class="btn_close" title="Close Window" alt="Close" /></a>
        <div  class="popupDiv" >
            
            <label>
                <span>Provide Final Task Status Report To Close</span>
                <textarea  ID="Report"  name="RejectReason" rows="1" cols="1" ></textarea>
            </label>

            <div style="float: left; width: 130px; ">
                <input type="button" ID="btnPopupSubmit" value="Submit" Class="SearchBtnCSS" style="width: 100px; color: green; " />
            </div>
        </div>
    </div>   
</asp:Content>

