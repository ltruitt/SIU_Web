<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="SubmitSafetyPays.aspx.cs" Inherits="Forms_SafetyIssue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Report Safety Issue</title>
    
    <link rel="stylesheet" href="/Styles/SafetyPays.css"/>
    <script type="text/javascript" src="/Scripts/SubmitSafetyPays.js" ></script>
    
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />
</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
     
    <div id="FormWrapper" class="ui-widget ui-form">

        <section style="visibility: hidden; height: 0; width: 0;">
            <span id="hlblEID"     runat="server"></span>
            <span id="hlblObsEID"  runat="server"></span>  
            <span id="hlblJobNo"   runat="server"></span>
            <span id="hlblIncidentNo"   runat="server"></span>
        </section>  

        <section id="HomeMain" >
        
            <!----------------->
            <!-- Form Header -->
            <!----------------->           
            <div class="ui-widget-header ui-corner-all" style="height: 60px;" >
                <img src="/Images/SafetyPaysBannerBlack.png" alt="Safety Pays Banner" style="float: left; height: 55px; margin-left: 45px; margin-right: auto; "/>
                <img src="/Images/SafetyPaysBannerBlack.png" alt="Safety Pays Banner" style="float: right; height: 55px; margin-left: 45px; margin-right: auto; "/>
                <div  style="text-align: center; font-size: 2em;">Safety Pays Submission Form</div>
            </div>
            
            <div class="ui-widget-content ui-corner-all">
                
                <!---------------------------->
                <!-- "Requestor ID" Section -->
                <!---------------------------->
                <span style="float: left; ">
                    <label style="float: left; padding: 0; margin-left: 5px;  width:auto; "  runat="server" ID="lblEmpName"></label>
                </span>

                <p style="height: 10px;"></p>
                
                <p style="font-size: smaller; text-align: center;">
                    Submit this form and provide input that will improve <span style="font-size:larger; font-weight: bold; font-style: italic;">Shermco's</span> safety culture.
                </p>
                
                <div id="SuprArea" style="margin-top: 5px; margin-bottom: 20px; display:inline-block;" runat="server" >
                    <span style="float:left; margin-right: 20px; font-size: 1.3em; width: auto;  font-weight: bold; display:inline">Select Employee:</span>
                    <input ID="ddEmpIds" class="DataInputCss" runat="server"   style="width: 400px;  background-color:yellow; border-color: black;" />         
                    
                    <span style="float:left; margin-left: 20px;  margin-right: 20px; font-size: 1.3em; width: auto;  font-weight: bold; display:inline">Lookup Record:</span>
                    <input ID="RcdId" class="DataInputCss" runat="server"   style="width: 40px; background-color:yellow; border-color: black;" />   
                </div>

                <!----------------------->
                <!-- Job Site Text Box -->
                <!----------------------->
                <div>

                    <div style="width: 350px;  display:inline-block; height: 55px;">
                        <div style="font-size: 1.3em; font-weight: bold">(Optional) Job Number</div>
                        <input type="text" id="JobNo" Class="DataInputCss DateEntryCss"   runat="server" style="width: 300px;" />  
                    </div>

                    <div style="width: 350px; display:inline-block; height: 55px;">
                        <div style="font-size: 1.3em; font-weight: bold">(REQUIRED) Location</div>
                        <input type="text" id="JobSite" Class="DataInputCss DateEntryCss"   runat="server" style="width: 300px;" /> 
                    </div>

                    <div style="float:right;  max-width: 300px; text-align: left; display:inline-block; -moz-text-overflow:clip; text-overflow:clip; height: 55px;">
                        <div runat="server" id="lblJobSiteSelection"  ></div>
                        <div runat="server" id="lblJobDescSelection"   ></div>
                        <div runat="server" id="lblDeptSelection"   ></div>
                    </div>
                </div>


                <div style="height: 20px;"></div>
                <div style="font-size: 1.3em; font-weight: bold">Check the appropiate reporting category</div>


                <div  class="ui-helper-clearfix" />

                    <!-------------------------------------->
                    <!-- Report Type Section -->
                    <!-------------------------------------->
                    <div class="ui-state-error ui-corner-all " style="width: 100%; clear: both;">
                        <div style="display: inline-block; width: 19%;">
                            <input type="checkbox" id="IncTypeSafeFlag" runat="server"   class="chkbox" value="I Saw Something Safe" />
                            <div style="padding-top: 5px;">I Saw Something Safe</div>
                        </div>

                        <div style="display: inline-block; width: 19%;">
                            <input type="checkbox" id="IncTypeUnsafeFlag" runat="server"   class="chkbox" value="Unsafe Act" />
                            <div style="padding-top: 5px;">Unsafe Act</div>
                        </div>

                        <div style="display: inline-block; width: 19%;">
                            <input type="checkbox" id="IncTypeSuggFlag" runat="server"   class="chkbox" value="Safety Suggestion" checked="checked" />
                            <div style="padding-top: 5px;">Safety Suggestion</div>
                        </div>

                        <div style="display: inline-block; width: 19%;">
                            <input type="checkbox" id="IncTypeTopicFlag" runat="server"   class="chkbox" value="EHS Topic" />
                            <div style="padding-top: 5px;">EHS Topic</div>
                        </div>

                        <div style="display: inline-block; width: 19%;">
                            <input type="checkbox" id="IncTypeSumFlag" runat="server"   class="chkbox" value="EHS Summary" />
                            <div style="padding-top: 5px;">EHS Summary</div>
                        </div>

                    <div style="height: 10px; clear: both;"></div>

                    <!-------------------->
                    <!-- Safe Act Block -->
                    <!-------------------->
                    <div id="SafeActFields" class="ui-state-error ui-corner-all" style="width: 600px; float: left; margin-top: 10px; clear: both; border-width: 5px;">    
                        <div style="font-size: 1em; font-weight: bold">Date Observed</div>
                        <input type="text" id="IncidentDate" Class="DataInputCss DateEntryCss"   runat="server" style="width: 123px;" />  
                                                      
                        <div style="clear: both; height: 4px; "></div>    
                        
                        <div id="ObservedEmpIDDiv">
                            <div style="font-size: 1em; font-weight: bold">Employee who was OBSERVED</div>    
                            <input type="text" id="ObservedEmpID" Class="DataInputCss DateEntryCss"   runat="server" style="width: 300px;" />                                             
                        </div>
                        
                                           
                        <div style="clear: both; height: 4px; "></div>  
                        
                        <div id="InitialResponseDiv">
                            <div style="font-size: 1em; font-weight: bold">Corrective Action Already Taken</div>                        
                            <textarea id="InitialResponse" Class="DataInputCss DateEntryCss"   runat="server" style="width: 95%; height: 100px; " maxlength="2048"  />                                             
                        </div>

                    </div>
                

                    <!------------------------>
                    <!-- Meeting Type Block -->
                    <!------------------------>
                    <div id="MeetingTypeFields" class="ui-state-error ui-corner-all" style="width: 260px; float: left; margin-top: 10px; clear: both; border-width: 5px; ">    
                    
                        <div style="font-size: 1em; font-weight: bold; display: inline-block;">Meeting Type:&nbsp;</div>              
                        <select id="SafetyMeetingType" class="DataInputCss" style="width: 125px" >
                            <option value="Monday">Monday</option>
                            <option value="Friday">Friday</option>
                            <option value="Admin">Admin</option>                            
                        </select>   
                        
                        <div style="clear: both; height: 4px; "></div>  

                        <div id="SafetyMeetingDateDiv">
                            <div style="font-size: 1em; font-weight: bold; display: inline-block; float:left; padding-right: 7px; ">Meeting Date:</div> 
                            <input type="text" id="SafetyMeetingDate" Class="DataInputCss DateEntryCss"   runat="server" style="width: 123px;" />                                     
                        </div>

                    </div>

                    <!----------------------->
                    <!-- Comments Text Box -->
                    <!----------------------->
                    <div style="clear: both; height: 10px; "></div> 
                    <div style="clear:both; ">
                        <div id="CommentLbl" style="font-size: 1.3em; font-weight: bold">Suggestion</div>
                        <textarea id="Comments" Class="DataInputCss DateEntryCss" style="width: 99%; height: 100px; " maxlength="2400"></textarea>
                    </div>        

                </div>




                <%--------------------%>                    
                <%-- Submit Buttons --%>                    
                <%--------------------%>
                <div style="width: 100%;  display: inline-block; margin-top: 25px;">
                    <div style="float: left; width: 30%; ">
                        <input type="button" ID="btnSubmit" value="Submit" Class="SearchBtnCSS" />
                    </div>
                                        
                    <div style="float: left; margin-left: 35px;  width: 25%; ">
                        <input type="button" ID="btnClear" value="Clear" Class="SearchBtnCSS" />
                    </div>
                </div>

            </div>
        </section>

    </div>

    <%--Footer Fix--%>
    <div style="height: 40px; clear: both;"></div>


</asp:Content>

