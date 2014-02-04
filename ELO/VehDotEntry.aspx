<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="VehDotEntry.aspx.cs" Inherits="ELO_VehDotEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>D. O. T. Inspection Entry</title>

    <!-- Include one of jTable styles. -->
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
 
    <!-- Include jTable script file. -->
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script> 

    <link href="/Styles/ELO.css" rel="stylesheet"  type="text/css" />    
    <link href="/Styles/VehDotEntry.css" rel="stylesheet"  type="text/css" />    
    <script type="text/javascript" src="/Scripts/VehDotEntry.js?0001"></script> 
    
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div style="background-color: #45473f; ">
        <div id="FormWrapper" class="ui-widget ui-form">
            
            <!------------------------->
            <!-- Hidden Data Fields --->
            <!------------------------->
            <section style="visibility: hidden; height: 0; width: 0;">
                <span id="hlblEID"             runat="server"/>  
                <span id="hlblRefID"           runat="server"/> 
                <span id="hlblPrevWeek"        runat="server"/> 
                <span id="hlblThisWeek"        runat="server"/> 
                <span id="hlblWeekIdx"         runat="server"/>
            </section>   
                                          
            <!----------------->
            <!-- Form Header -->
            <!----------------->                   
            <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
                <a href="/ELO/MainMenu.aspx" style="text-decoration: none;">
                    <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">elo menu</span>
                    <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Vehicle Inspection</span>
                </a>
            </section>
    
            <section class="ui-widget-content ui-corner-all">
            
                <!------------------------->
                <!-- "Viewer ID" Section -->
                <!------------------------->
                <div style="float: left; margin-top: -20px; width: 100%; margin-bottom: 20px;">
                    <label style="float: left; padding: 0; width:auto; "  runat="server" ID="lblEmpName"></label><br/>
                    <span ID="imgReport" runat="server"  style="float: right; margin-left: 7px; "  >
                        <a href="/ELO/VehDotNonCorrected.aspx" style="color:white; font-weight: bold; position: relative; z-index: 100;">Hazard History Report</a>
                    </span> 
                </div>
    
                <div id="jTableContainer" style="z-index: 0; clear: both;"></div>
                
                <!--------------------------------->
                <!-- Week For Time Being Entered -->
                <!--------------------------------->
                <div class="TimeRow" style="margin-top: 10px;">
                    
                    <div style="padding: 0; width: 100%;  margin-left: 0;">
                        
                        <div style="display: inline-block; width: 100%; text-align: center; ">
                            Submitted On: 
                            <span runat="server" id="txtEntryDate"  class="ThisWeekCss"  ></span>
                        </div>
                    </div>
                    <div style="clear: both;"></div>   
                 </div>
                 
                
                <!--------------------------->
                <!-- Date Shortcut Buttons -->
                <!--------------------------->
                <div id="dateBtnsDiv" style="margin-bottom: 3px; margin-top: -6px; margin-left: auto; margin-right: auto; width: 400px;">
                
                     <div style="float: left;">
                         <span class="prevarrow"><a href="#" ></a></span>
                     </div>

                    <div style="float: left;">
                        <table style=" margin-left: 15px; width: 200px; margin-right: 15px; padding-top: 5px;">
                            <tr >
                                <td><input id="btnMon" type="button" class="DowBtnCSS" value="M"  runat="server" style="width: 40px;" /></td>
                                <td><input id="btnTue" type="button" class="DowBtnCSS" value="T"  runat="server" style="width: 40px;" /></td>
                                <td><input id="btnWed" type="button" class="DowBtnCSS" value="W"  runat="server" style="width: 40px;" /></td>
                                <td><input id="btnThu" type="button" class="DowBtnCSS" value="T"  runat="server" style="width: 40px;" /></td>
                                <td style="width: 75px;"><input id="btnFri" type="button" class="DowBtnCSS" value="F"  runat="server" style="width: 40px;"/></td>
                                <td ><input id="btnSat" type="button" class="DowBtnCSS" value="S"  runat="server" style="width: 40px;"/></td>
                                <td style="padding-right: 5px;  width: 40px;"><input id="btnSun" type="button" class="DowBtnCSS" value="S"  runat="server" style="width: 40px;"/></td>
                            </tr>
                        </table>
                    </div>
                    
                     <div style="float: left;">
                         <span class="nextarrow"><a href="#"></a></span>
                     </div>
                     
                     <div style="clear: both;"></div>
                </div>
                
                

                 <!----------------------->
                 <!-- Data Entry Fields -->
                 <!----------------------->   
                 <div style="display: inline-block; margin-top: 20px; width: 100%; ">
                     <div>
                         
                         <!--------------------->
                         <!-- Vehicle Number  -->
                         <!--------------------->
                        <div class="TimeRow" id="ClassTimeDiv"  style="margin-top: 0;">    
                            <span style="float:left;   width: 110px;  font-weight: bold; display:inline">Vehicle No:</span>
                            <input id="txtVehicleNo"  class="DataInputCss" style="width: 80px; display:inline; margin-top: -3px;" runat="server" />
                            
                            <span style="float:left; margin-left: 30px;  width: 110px;  font-weight: bold; display:inline">Reported By:</span>
                            <span id="reportedBy" style="float:left; font-weight: bold; display:inline"></span>
                            <br/>
                        </div>  
                        
                        <!------------------------------------->
                        <!-- Assigned Vehicle List Shortcuts -->
                        <!------------------------------------->
                        <div>
                            <span id="AssignedVehicleList"  runat="server"></span>
                        </div>
                        
                        <!----------------------------------------------------------------------->
                        <!-- Display Labels Showing Selected Charge Account From DD Selections -->
                        <!----------------------------------------------------------------------->
                        <div style="color: white; font-size: smaller;">
                            <span runat="server" id="lblErr" Class="errorTextCss"  ></span>
                        </div> 

                        <!------------->
                        <!--  Hazard -->
                        <!------------->
                         <div style="width: 605px;">
                            <span style="font-weight: bold; margin-top: 10px; display:inline-block;">Hazard:</span>
                            <input type="button" ID="btnNoUse" value="Vehicle Not Used" Class="SearchBtnCSS"   style="width: 200px; margin-left: 10px; display: inline-block; float: right;" />
                            <input type="button" ID="btnNoHaz" value="No Hazards"       Class="SearchBtnCSS"   style="width: 200px;                    display: inline-block; float: right;" />
                        </div>

                        <div class="TimeRow" id="txtHazardDiv" style=" height: 80px;">
                            <textarea  id="txtHazard"  Class="DataInputCss" title="Corrective Action" style="width: 600px;" rows="3" cols="20" runat="server" ></textarea>
                        </div>
                        
                        <!------------------------>
                        <!--  Corrective Action -->
                        <!------------------------>
                        <span style="width: 100%; font-weight: bold; margin-top: 30px; display:inline-block;">Corrective Action: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  (Fill this field in ONLY after a repair is complete)</span>
                        <div class="TimeRow" id="txtCorrectionDiv" style=" height: 80px;">
                            <textarea  id="txtCorrection"  Class="DataInputCss" title="Corrective Action" style="width: 600px;" rows="3" cols="20" runat="server" ></textarea>
                        </div>                      
                     </div>
                 </div>
                 
                 
                <%--------------------%>                    
                <%-- Submit Buttons --%>                    
                <%--------------------%>
                <div style="width: 100%;  display: inline-block; margin-top: 25px;">
                    <div style="float: left; width: 100px; margin-right: 40px;">
                        <input type="button" ID="btnSubmit" value="Submit" Class="SearchBtnCSS" />
                    </div>
                                        
                    <div style="float: left;   width: 100px; ">
                        <input type="button" ID="btnClear" value="Clear" Class="SearchBtnCSS" />
                    </div>
                </div>

            </section>
        </div>
    </div>
    
</asp:Content>

