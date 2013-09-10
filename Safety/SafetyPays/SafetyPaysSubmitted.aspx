<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="SafetyPaysSubmitted.aspx.cs" Inherits="Safety_SafetyPays_SafetyPaysSubmitted" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

    <title>Submitted Safety Pays Reports</title>
    
    <link href="/Styles/SubmitTimeRpt.css" rel="stylesheet"  type="text/css" />
    
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script>     
    
    <script type="text/javascript" src="/Scripts/SafetyPaysSubmitted.js"></script>

    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/SafetyPaysSubmitted.css" />

</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="FormWrapper" class="ui-widget ui-form">
                    
        <section style="visibility: hidden; height: 0; width: 0;">
            <span id="hlblEID"           runat="server"/>  
            <span id="hlblKey"           runat="server"/>
        </section>            
            
        <!----------------->
        <!-- Form Header -->
        <!----------------->                   
        <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
            <a href="/Safety/SafetyAdminHome.aspx" style="text-decoration: none;">
                <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">admin page</span>
                <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Submitted Safety Pays</span>
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


            <div style="color: white; font-size: smaller;"  id="DtlDiv">  
                    
                <div style="display: inline-block; width: 100%;">
                    <div style="width: 300px; display: inline-block;">
                        <span style="width: 80px;  font-weight: bold; display:inline-block;">Rpt Type:</span>
                        <span id="lblIncTypeTxt"></span>
                    </div>
                    <div style=" display: inline-block;">
                        <span style="width: 80px; font-weight: bold; display:inline-block;">Location:</span>
                        <span style="color: yellow;" id="lblJobSite"></span>
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
                    <span style="width: 80px; font-weight: bold; display:inline-block;">Mtg Date:</span>
                    <span id="lblSafetyMeetingDate"></span>
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
                    <span style="width: 80px; font-weight: bold; display:inline-block;">Mtg Type:</span>
                    <span id="lblSafetyMeetingType"></span>
                </div>

                <p></p>

                <span style="width: 80px; font-weight: bold; display:inline-block;">Comments:</span>
                <span  id="lblComments" ></span><br/>

                <span style="width: 180px; font-weight: bold; display:inline-block;">Initial Employee Response:</span>
                <span  id="lblInitialResponse"></span><br/>
                

            </div> 



            <div id="cmdDiv" style="display: inline-block; margin-top: 5px;">
                
                <br/>

                <div style="display: inline-block; margin-bottom: 0; float: left; ">
                    <div style="display: inline-block; background-color:  rgb(35, 135, 16); width: 350px; height: 5px; "></div>
                    <div style="display: inline-block; background-color:  red; width: 150px; height: 5px; margin-left: 45px;"></div>
                </div>
                <div style="clear: both;"></div>

                <div style="float: left; width: 130px;  ">
                    <input type="button" ID="btnAccept" value="Approved" Class="SearchBtnCSS" style="width: 100px; color: green;" />
                </div>

                <div style="float: left; width: 70px;">
                    <input id="txtPts" Class="DataInputCss" style="float: left; width: 35px; margin-right: 5px; background-color: rgb(180, 248, 177); font-weight: bold; text-align: center; font-size: 1.5em; padding: 0; margin-top: -3px;" />
                </div>

                <div style="float: left; width: 200px; ">
                    <input type="button" ID="btnWork" value="Approved w/ Task" Class="SearchBtnCSS" style="width: 150px; color: green;"/>
                </div>
                                                                            
                <div style="float: left; width: 150px; ">
                    <input type="button" ID="btnReject" value="Not Approved" Class="SearchBtnCSS"  style="width: 150px;" />
                </div>
                
                <br/>
                <div style="margin-top: 60px;">
                    <div style="background-color: khaki; width: 750px; height: 5px; "></div>
                    <span style="width: 120px; font-weight: bold; display:inline-block; color: khaki; vertical-align: top;">EHS Response:</span>
                    <textarea ID="ehsRepsonse"  name="ehsResponse"  cols="1" style="width: 620px; height: 60px;" Class="DataInputCss" maxlength="200" ></textarea>
                </div>
                                
            </div>


            <!-- List Of Newly Submitted Safety Pays Reports -->
            <div style="width: 100%; padding-top: 20px; margin-left: auto; margin-right: auto;">
                <div id="jTableContainer"></div>
            </div>
        </section>
            
    </div>

</asp:Content>

