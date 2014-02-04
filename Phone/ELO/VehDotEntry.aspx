<%@ Page Title="" Language="C#" MasterPageFile="/Phone/PhoneMaster.master" AutoEventWireup="true" CodeFile="VehDotEntry.aspx.cs" Inherits="ELO_VehDotEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>D. O. T. Inspection Entry</title>
    

    <script type="text/javascript" src="/Scripts/VehDotEntry.js"></script> 
    

    <!-- Include one of jTable styles. -->
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
 
    <!-- Include jTable script file. -->
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script> 

    <link href="/Phone/Styles/ELO.css" rel="stylesheet"  type="text/css" />    
    <link href="/Phone/Styles/VehDotEntry.css" rel="stylesheet"  type="text/css" />    
    
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div style="background-color: #45473f; ">
        <div id="FormWrapper" class="ui-widget ui-form">
            
            <!------------------------->
            <!-- Hidden Data Fields --->
            <!------------------------->
            <section style="visibility: hidden; height: 0; width: 0;">
                <span id="hlblEID"             runat="server"></span>  
                <span id="hlblRefID"           runat="server"></span> 
                <span id="hlblPrevWeek"        runat="server"></span> 
                <span id="hlblThisWeek"        runat="server"></span> 
                <span id="hlblWeekIdx"         runat="server"></span>
            </section>   
                                          
            <!----------------->
            <!-- Form Header -->
            <!----------------->                   
            <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
                <a href="/Phone/ELO/MainMenu.aspx" style="text-decoration: none;">
                    <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">elo menu</span>
                    <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Veh. Inspection</span>
                </a>
            </section>
    
            <section class="ui-widget-content ui-corner-all">
            
                <!------------------------->
                <!-- "Viewer ID" Section -->
                <!------------------------->
                <div style="float: left; margin-top: -20px; width: 100%; margin-bottom: 20px;">
                    <label runat="server" ID="lblEmpName" ></label><br/>
                    <span ID="imgReport" runat="server"  style="float: right; margin-left: 7px; "  >
                        <a href="/Phone/ELO/VehDotNonCorrected.aspx" style="color:white; font-weight: bold; position: relative; z-index: 100;">Report</a>
                    </span> 
                </div>
    
        
                <div id="jTableContainer" style="z-index: 0; clear: both; color: black;"></div>
                
                <!--------------------------------->
                <!-- Week For Time Being Entered -->
                <!--------------------------------->
                <div class="TimeRow" style="margin-top: 10px;">
                    
                    <div style="padding: 0; width: 107%;  margin-left: -10px;">
                        
                        <div style="display: inline-block; width: 100%; text-align: center; ">
                            <span runat="server" id="txtEntryDate"  class="ThisWeekCss"  ></span>
                        </div>
                    </div>
                    
                 </div>
                 <div style="clear: both;"></div>
                
                <!--------------------------->
                <!-- Date Shortcut Buttons -->
                <!--------------------------->
                <div style="margin-bottom: 3px; margin-top: -6px;">
                
                     <div style="float: left;">
                         <span class="prevarrow"><a href="#" ></a></span>
                     </div>

                    <div style="float: left;">
                        <table style=" margin-left: 15px; width: 200px; margin-right: 15px;">
                            <tr >
                                <td><input id="btnMon" type="button" class="DowBtnCSS" value="M"  runat="server" style="width: 40px;" /></td>
                                <td><input id="btnTue" type="button" class="DowBtnCSS" value="T"  runat="server" style="width: 40px;" /></td>
                                <td><input id="btnWed" type="button" class="DowBtnCSS" value="W"  runat="server" style="width: 40px;" /></td>
                                <td><input id="btnThu" type="button" class="DowBtnCSS" value="T"  runat="server" style="width: 40px;" /></td>
                            </tr>
                        </table>
                        <table style="margin-left: 40px; width: 150px; margin-right: 15px;">
                            <tr>
                                <td><input id="btnFri" type="button" class="DowBtnCSS" value="F"  runat="server" style="width: 40px;"/></td>
                                <td ><input id="btnSat" type="button" class="DowBtnCSS" value="S"  runat="server" style="width: 40px;"/></td>
                                <td><input id="btnSun" type="button" class="DowBtnCSS" value="S"  runat="server" style="width: 40px;"/></td>
                            </tr>
                        </table>
                    </div>
                    
                     <div style="float: left;">
                         <span class="nextarrow"><a href="#"></a></span>
                     </div>

                </div>
                <div style="clear: both;"></div>
                

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
                        </div>  
                        <div class="TimeRow" id="Div1"  style="margin-top: 0;">                             
                            <span style="float:left; font-weight: bold; display:inline">Reported By:</span>
                            <span id="reportedBy" style="float:left; margin-left: 5px;  font-weight: bold; display:inline"></span>
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
                        <input type="button" ID="btnNoUse" value="Vehicle Not Used" Class="SearchBtnCSS"  style="width: 50%; padding-left: 0; padding-right: 0; margin-left: 0; display: inline-block; float: right; text-align: center;" />
                        <input type="button" ID="btnNoHaz" value="No Hazards"       Class="SearchBtnCSS"  style="width: 50%; padding-left: 0; padding-right: 0; margin-left: 0; display: inline-block; float: left; text-align: center;"/>

                        <span style="font-weight: bold; margin-top: 10px; display:inline-block;">Hazard:</span>

                        


                        <div class="TimeRow" id="txtHazardDiv" style=" height: 80px;">
                            <textarea  id="txtHazard"  Class="DataInputCss" title="Corrective Action" style="width: 100%;" rows="3" cols="20" runat="server" ></textarea>
                        </div>
                        
                        <!------------------------>
                        <!--  Corrective Action -->
                        <!------------------------>
                        <span style="width: 100%; font-weight: bold; margin-top: 10px; display:inline-block;">Corrective Action:</span>
                        <div class="TimeRow" id="txtCorrectionDiv" style=" height: 80px;">
                            <textarea  id="txtCorrection"  Class="DataInputCss" title="Corrective Action" style="width: 100%;" rows="3" cols="20" runat="server" ></textarea>
                        </div>

                                                 
                     </div>
                     


                 </div>
                 
                 
                <%--------------------%>                    
                <%-- Submit Buttons --%>                    
                <%--------------------%>
                <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                    <div style="float: left; width: 30%; ">
                        <input type="button" ID="btnSubmit" value="Submit" Class="SearchBtnCSS" />
                    </div>
                                        
                    <div style="float: right; width: 25%; ">
                        <input type="button" ID="btnClear" value="Clear" Class="SearchBtnCSS"  autofocus="False" />
                    </div>
                </div>

            </section>
        </div>
    </div>
</asp:Content>

