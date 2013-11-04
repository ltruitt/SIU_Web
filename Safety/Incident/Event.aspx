<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="Event.aspx.cs" Inherits="Safety_Incident_Event" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script>  

    <script type="text/javascript" src="/Scripts/Event.js"></script> 
    
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" /> 
    <link href="/Styles/Event.css" rel="stylesheet"  type="text/css" />
    
<style>
            .Chk  {
            border: none;  
            padding-right: 10px;
        }
</style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div >
        <div id="FormWrapper" class="ui-widget ui-form">
                    
            <!----------------->
            <!-- Form Header -->
            <!----------------->                               
            <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
                <a href="/Safety/SafetyAdminHome.aspx" style="text-decoration: none;">
                    <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">Safety admin</span>
                    <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Incident Recorder</span>
                </a>
            </section> 
            
            <section class="ui-widget-content ui-corner-all">

                <!---------------------------->
                <!-- "Requestor ID" Section -->
                <!---------------------------->
                <div style="float: left; ">
                    <label style="float: left; padding: 0; width:auto; margin-top: -20px;"  runat="server" ID="lblEmpName"></label><br/>
                </div>

                <p style="height: 20px;"></p> 
                
                <div style="float: left; margin-right: 5px; width: 40%;">
                    
                    <%---------------------------------------%>
                    <!-- List Of Open Incidents / Accidents -->
                    <%---------------------------------------%>
                    <div style="width: 100%; padding-top: 20px; margin-left: auto; margin-right: auto;">
                        <div id="jTableContainer"></div>
                    </div>
                    
                </div> 
                
                
               <div id="tabs"style="float: right; width: 55%;">                   

                    <ul>
                        <li><a href="#ClassTab">Classification</a></li>
                        <li><a href="#OshaTab">OSHA Log</a></li>
                        <li><a href="#EmpTab">Employee</a></li>
                        <li><a href="#FollowTab">Follow Up</a></li>
                        <li><a href="#CostTab">Cost</a></li>
                    </ul>

                    <div id="ClassTab">
                        <section id="RptDisp" class="xui-state-error ui-corner-all" style="margin-top: 4px; width: 100%;"   >
                        
                            <div style="display: inline-block;  margin-bottom: 5px; width: 90px;">
                                <div style=" text-align: center;">Type</div>
                                <input ID="acType" class="DataInputCss" runat="server"   style="width: 90px;" /> 
                            </div>
                            
                            <div style="display: inline-block;  margin-bottom: 5px; width: 120px; margin-left: 20px;">
                                <div style=" text-align: center;">Description</div>
                                <input ID="acTypeDesc" class="DataInputCss" runat="server"   style="width: 110px;" /> 
                            </div>
                            
                            <div style="display: inline-block;  margin-bottom: 5px; width: 140px; margin-left: 10px;">
                                <div style=" text-align: center;">Location</div>
                                <input ID="acLocation" class="DataInputCss" runat="server"   style="width: 140px;" /> 
                            </div>
                            
                            <div style="display: inline-block;  margin-bottom: 5px; width: 120px; margin-left: 20px;">
                                <div style=" text-align: center;">Occurance Date</div>
                                <input ID="DateOccured" class="DataInputCss" runat="server"   style="width: 100px; margin-left: 10px;" /> 
                            </div>
                            
                            <div style="display: inline-block;  margin-bottom: 5px; width: 120px; margin-left: 20px;">
                                <div style=" text-align: center;">Unit No</div>
                                <input ID="Text2" class="DataInputCss" runat="server"   style="width: 100px; margin-left: 10px;" /> 
                            </div>

                            <div style="display: inline-block;  margin-bottom: 5px; width: 120px; margin-left: 20px;">
                                <div style=" text-align: center;">Job No</div>
                                <input ID="Text3" class="DataInputCss" runat="server"   style="width: 100px; margin-left: 10px;" /> 
                            </div>


                            <div style="display: inline-block; width: 100%;">
                                <div style="float: left; width: 45%;">
                                    <div>Incident Description</div>
                                    <textarea id="txtIncDesc" class="DataInputCss" style="width: 100%;" rows="5"></textarea>
                                </div>
                                
                                <div style="float: right; width: 45%; padding-right: 20px;">
                                    <div>Unsafe Act or Condition</div>
                                    <textarea id="Textarea1" class="DataInputCss" style="width: 100%;" rows="5"></textarea>
                                </div>
                            </div>
                            

                    
                        </section>                        
                    </div>
                   
                   
                   

                    <div id="OshaTab">
                        <section id="Section1" class="xui-state-error ui-corner-all" style="margin-top: 4px; width: 100%;"   >
                        
                           <div style="width: 100%; text-align: center;">
                               <div style="display: inline-block; width: 120px;  margin-bottom: 5px; text-align: center;">
                                   <span>Medical Record</span>
                                   <input type="checkbox" id="chkMedRcd" class="Chk" value="MedRcd" style="padding-left: 50px;"/>            
                                </div>                  
                        

                                <div style="display: inline-block;  margin-bottom: 5px; width: 250px;; margin-left: 20px;">
                                    <div style=" text-align: left;">Number of Restricted Duty Days</div>
                                    <input ID="txtRestrictDays" class="DataInputCss" runat="server"   style="width: 50px; margin-left: 70px;" /> 
                                </div>
                        

                                <div style="display: inline-block;  margin-bottom: 5px; width: 250px;; margin-left: 20px;">
                                    <div style=" text-align: left;">Number of Lost Time Days</div>
                                    <input ID="txtLostDays" class="DataInputCss" runat="server"   style="width: 50px; margin-left: 70px;" /> 
                                </div>
                            </div>
                        </section>
                    </div>
                   
                   
                   
                   

                    <div id="EmpTab">
                        <section id="Section2" class="xui-state-error ui-corner-all" style="margin-top: 4px; width: 100%;"   >
                            <div style="margin-left: 20px; width: 300px; margin-top: 10px; float: left;">
                                <div>
                                    <div style=" text-align: left; display: inline-block; float: left;">Employee ID</div>
                                    <input ID="Text1" class="DataInputCss"  style="width: 50px; margin-left: 10px; " /> 
                                </div>
                            
                                <div style="clear: both; width: 300px; margin-top: 10px;">
                                
                                    <div style="color: white; font-size: smaller; padding-top: 15px;">
                                        <label style="width: 80px;" for="EID">Employee:</label><span id="EID">10371</span><span style="padding-left: 10px;" id="Ename">Truitt, John</span>
                                        <br/>
                                        <label style="width: 80px;" for="sEID">Suprervisor:</label><span  id="sEID">200</span><span style="padding-left: 10px;"  id="sName">Mullen, Lonnie</span>
                                        <br/>
                                        <label style="width: 80px;" for="Edept">Deptartment:</label><span  id="Edept">6060</span>
                                    </div>
                                </div>
                            </div>

                            <div style="margin-left: 20px; width: 200px; float: left; margin-top: 10px;">
                                <div style="display: inline-block; width: 230px;  margin-bottom: 5px;">
                                    <div style="display: inline-block; float: left;">Drug / Breath Alchol Test</div>
                                    <input type="checkbox" id="Checkbox1" class="Chk" value="MedRcd" style="xfloat: left; padding-left: 15px;"/>
                                </div> 
                            
<%--                                <div style=" width: 100%; margin-top: 10px;">
                                    <div style=" text-align: left; display: inline-block; text-align: center; width: 150px;">Job Number</div>
                                    <input ID="Text4" class="DataInputCss"  style="width: 150px; " />
                                </div>--%>
                            </div>
                        </section>
                    </div>
                   
                   

                    <div id="FollowTab">
                        <div style="display: inline-block; width: 100%;">
                            <div style="float: left; width: 45%;">
                                <div>Discipline Description</div>
                                <textarea id="Textarea2" class="DataInputCss" style="width: 100%;" rows="5"></textarea>
                            </div>
                                
                            <div style="float: right; width: 45%; padding-right: 20px;">
                                <div>Actions Taken to Prevent Reoccurance</div>
                                <textarea id="Textarea3" class="DataInputCss" style="width: 100%;" rows="5"></textarea>
                            </div>
                        </div>
                        
                        <div style="display: inline-block; width: 100%;">
                            <div style="float: left; width: 98%;">
                                <div>Additional Comments</div>
                                <textarea id="Textarea4" class="DataInputCss" style="width: 100%;" rows="5"></textarea>
                            </div>
                        </div>
                        
                                                
                    </div>

                    <div id="CostTab">
                        <div style="display: inline-block;  margin-bottom: 5px; width: 90px;">
                            <div style=" text-align: center;">In-House</div>
                            <input ID="Text4" class="DataInputCss" runat="server"   style="width: 90px;" /> 
                        </div>
                            
                        <div style="display: inline-block;  margin-bottom: 5px; width: 120px; margin-left: 20px;">
                            <div style=" text-align: center;">Incurred</div>
                            <input ID="Text5" class="DataInputCss" runat="server"   style="width: 110px;" /> 
                        </div>
                            
                        <div style="display: inline-block;  margin-bottom: 5px; width: 140px; margin-left: 10px;">
                            <div style=" text-align: center;">Reserves</div>
                            <input ID="Text6" class="DataInputCss" runat="server"   style="width: 140px;" /> 
                        </div>
                            
                        <div style="display: inline-block;  margin-bottom: 5px; width: 120px; margin-left: 20px;">
                            <div style=" text-align: center;">Total</div>
                            <input ID="Text7" class="DataInputCss" runat="server"   style="width: 100px; margin-left: 10px;" /> 
                        </div>                        
                    </div>
                   
                   <div style="clear: both;"></div>
                   
                    <%------------------------------%>                    
                    <%-- Submit and Clear Buttons --%>                    
                    <%------------------------------%>
                    <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                        <div style="float: left; width: 120px;; ">
                            <input type="button" ID="btnSubmit" value="Submit" Class="SearchBtnCSS" />
                        </div>
                    
                                        
                        <div style="float: left; width: 25%; ">
                            <input type="button" ID="btnClear" value="Clear" Class="SearchBtnCSS" />
                        </div>
                    </div>
               </div>                   
                
               <div style="clear: both;"></div>
            </section>
            
        </div>
        
    </div>
</asp:Content>

