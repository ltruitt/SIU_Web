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

        .ui-tabs-nav  {
            background-image: none;
            border: none;
            margin-top: -12px !important;
        }

        html {
            background-color: grey;
        }
</style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div>
        <div id="FormWrapper" class="ui-widget ui-form">
                    
            <section style="visibility: hidden; height: 0; width: 0;">
                <span id="hlblUID"></span>
            </section>

            <!----------------->
            <!-- Form Header -->
            <!----------------->                               
            <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
                <a href="/Safety/SafetyAdminHome.aspx" style="text-decoration: none;">
                    <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">Safety admin</span>
                    <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Incident Approval</span>
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
                        <section id="RptDisp" style="margin-top: 4px; width: 100%; margin-left: -20px;"   >
                        
                            <div style="display: inline-block;">
                                <div style=" text-align: center;">Type</div>
                                <input ID="Inc_Type" class="DataInputCss" style="width: 100px; margin-right: 10px;" />
                            </div>
                            
                            <div style="display: inline-block;">
                                <div style=" text-align: center;">Description</div>
                                <input ID="Inc_Type_Sub" class="DataInputCss" style="width: 110px;  margin-right: 10px;" /> 
                            </div>
                            
                            <div style="display: inline-block;">
                                <div style=" text-align: center;">Location</div>
                                <input ID="Inc_Loc" class="DataInputCss" style="width: 140px;  margin-right: 10px;" /> 
                            </div>
                            
                            <div style="display: inline-block;">
                                <div style=" text-align: center;">Occurance Date</div>
                                <input ID="Inc_Occur_Date" class="DataInputCss" style="width: 100px;  margin-right: 10px;" /> 
                            </div>
                            
                            <div style="display: inline-block;">
                                <div style=" text-align: center;">Unit No</div>
                                <input ID="Emp_Veh_Involved" class="DataInputCss" style="width: 100px; margin-right: 10px;" /> 
                            </div>

                            <div style="display: inline-block;">
                                <div style=" text-align: center;">Job/Prj No</div>
                                <input ID="Emp_Job_No" class="DataInputCss" style="width: 100px;  margin-right: 10px;" /> 
                            </div>

                            <div style="display: inline-block;">
                                <div style=" text-align: center;">Claim No</div>
                                <input ID="Claim_ID" class="DataInputCss" style="width: 100px;  margin-right: 10px;" /> 
                            </div>


                            <div style="display: inline-block; width: 100%; margin-top: 10px;">
                                <div style="float: left; width: 45%;">
                                    <div>Incident Description</div>
                                    <textarea id="Inc_Desc" class="DataInputCss" style="width: 100%;" rows="5"></textarea>
                                </div>
                                
                                <div style="float: left; width: 45%; padding-left: 20px;">
                                    <div>Unsafe Act or Condition</div>
                                    <textarea id="Inc_Unsafe_Act_or_Condition" class="DataInputCss" style="width: 100%;" rows="5"></textarea>
                                </div>
                            </div>
                            

                    
                        </section>                        
                    </div>
                   
                    <div id="OshaTab">
                        <section id="Section1" style="margin-top: 4px; width: 100%;  margin-left: -20px;"   >
                        
                           <div style="width: 100%; text-align: center;">
                               <div style="display: inline-block; width: 120px;  margin-bottom: 5px; text-align: center;">
                                   <span>Medical Record</span>
                                   <input type="checkbox" id="Osha_Record_Med" class="Chk" value="MedRcd" style="padding-left: 50px;"/>            
                                </div>                  
                        

                                <div style="display: inline-block;  margin-bottom: 5px; width: 250px;; margin-left: 20px;">
                                    <div style=" text-align: left;">Number of Restricted Duty Days</div>
                                    <input ID="Osha_Restrict_Days" class="DataInputCss" runat="server"   style="width: 50px; margin-left: 70px;" /> 
                                </div>
                        

                                <div style="display: inline-block;  margin-bottom: 5px; width: 250px;; margin-left: 20px;">
                                    <div style=" text-align: left;">Number of Lost Time Days</div>
                                    <input ID="Osha_Lost_Days" class="DataInputCss" runat="server"   style="width: 50px; margin-left: 70px;" /> 
                                </div>
                            </div>
                        </section>
                    </div>
                   
                    <div id="EmpTab">
                        <section id="Section2" style="margin-top: 4px; width: 100%;  margin-left: -20px;"   >
                            
                            <div style="width: 290px; margin-top: 10px; float: left;">
                                <div>
                                    <div style=" text-align: left; display: inline-block; float: left;">Employee ID</div>
                                    <input ID="ddEmpIds" class="DataInputCss"  style="width: 250px;" /> 
                                </div>
                            
                                <div style="clear: both; width: 290px; margin-top: 10px;">
                                
                                    <div style="color: white; font-size: smaller; padding-top: 15px;">
                                        <label style="width: 80px;" for="Emp_ID">Employee:</label><span id="Emp_ID">10371</span><span style="padding-left: 10px;" id="Ename"></span>
                                        <br/>
                                        <label style="width: 80px;" for="Supr">Suprervisor:</label><span   id="Supr"></span>
                                        <br/>
                                        <label style="width: 80px;" for="Edept">Deptartment:</label><span  id="Edept"></span>
                                    </div>
                                </div>
                            </div>

                            <div style="margin-left: 0; width: 50%; float: left; margin-top: 10px; ">
                                <div style="float: left; width: 230px;  margin-bottom: 5px;">
                                    <div style="display: inline-block; float: left;">Drug / Breath Alchol Test</div>
                                    <input type="checkbox" id="Emp_Drug_Alchol_Test" class="Chk" value="Emp_Drug_Alchol_Test" style="padding-left: 15px;"/>
                                </div> 

                                <div style="clear: both;"></div>

                                <div style="float: left; width: 100%; ">
                                    <div>Employee Notes</div>
                                    <textarea id="Emp_Comments" class="DataInputCss" style="width: 100%;" rows="5"></textarea>    
                                </div>

                                <div style="clear: both;"></div>

                                
                            </div>
                            

                        </section>
                    </div>
                   
                    <div id="FollowTab">
                        
                        <div style="display: inline-block; width: 100%;  margin-left: -20px;">
                            
                            <div style="float: left; width: 230px;  margin-bottom: 5px;">
                                <div style="display: inline-block; float: left;">Discipline Issued</div>
                                <input type="checkbox" id="Follow_Discipline_Issued_Flag" class="Chk" value="Follow_Discipline_Issued_Flag" style="padding-left: 15px;"/>
                            </div> 

                            <div style=" text-align: left; display: inline-block; float: left;">Responsible for Follow Up</div>
                            <input ID="Follow_Responsible" class="DataInputCss"  style="width: 250px; margin-left: 10px;" /> 
                            

                        </div>

                        <div style="display: inline-block; width: 100%;  margin-left: -20px;">
                            <div style="float: left; width: 45%;">
                                <div>Discipline Description</div>
                                <textarea id="Follow_Discipline" class="DataInputCss" style="width: 100%;" rows="5"></textarea>
                            </div>
                                
                            <div style="float: right; width: 45%; padding-right: 20px;">
                                <div>Actions Taken to Prevent Reoccurance</div>
                                <textarea id="Follow_Prevent_Reoccur" class="DataInputCss" style="width: 100%;" rows="5"></textarea>
                            </div>
                        </div>
                        
                        <div style="display: inline-block; width: 100%;  margin-left: -20px;">
                            <div style="float: left; width: 98%;">
                                <div>Additional Comments</div>
                                <textarea id="Follow_Comments" class="DataInputCss" style="width: 100%;" rows="5"></textarea>
                            </div>
                        </div>
                        
                                                
                    </div>

                    <div id="CostTab">
                        <section id="Section3" style="margin-top: 4px; width: 100%; margin-left: -20px;"   >
                            <div style="display: inline-block;">
                                <div style="margin-left: 2px;">In-House</div>
                                <input ID="Cost_inHouse" class="DataInputCss" style="width: 60px; margin-right: 20px;" /> 
                            </div>
                            
                            <div style="display: inline-block;">
                                <div style="margin-left: 5px;">Incurred</div>
                                <input ID="Cost_Incurred" class="DataInputCss" style="width: 60px; margin-right: 20px;" /> 
                            </div>
                            
                            <div style="display: inline-block;">
                                <div style="margin-left: 4px;">Reserves</div>
                                <input ID="Cost_Reserve" class="DataInputCss" style="width: 60px; margin-right: 20px;" /> 
                            </div>
                            
                            <div style="display: inline-block;">
                                <div style="margin-left: 15px;">Total</div>
                                <input ID="Cost_Total" class="DataInputCss" style="width: 60px;" /> 
                            </div>   
                        </section>                     
                    </div>
                   
                    <div style="clear: both;"></div>
                   
                    <%------------------------------%>                    
                    <%-- Submit and Clear Buttons --%>                    
                    <%------------------------------%>
                    <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                        <div style="float: left; width: 120px; ">
                            <input type="button" ID="btnSave" value="Save" Class="SearchBtnCSS" />
                        </div>
                    
                                        
                        <div style="float: left; width: 120px; ">
                            <input type="button" ID="btnClear" value="Clear" Class="SearchBtnCSS" />
                        </div>
                        
                        <div style="float: left; width: 120px; ">
                            <input type="button" ID="btnSubmit" value="Submit" Class="SearchBtnCSS" />
                        </div>

                    </div>
               </div>                   
                
               <div style="clear: both;"></div>
            </section>
            
        </div>
        
    </div>
</asp:Content>

