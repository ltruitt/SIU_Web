<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="Approve.aspx.cs" Inherits="Safety_Incident_Approve" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script>  

    <script type="text/javascript" src="/Scripts/SafetyIncidentApprove.js"></script> 
    
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" /> 
    <link href="/Styles/SafetyIncidentApprove.css" rel="stylesheet"  type="text/css" />
    

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div>
        <div id="FormWrapper" class="ui-widget ui-form">
                    
            <section style="visibility: hidden; height: 0; width: 0;">
                <span id="hlblUID"></span>
                <span id="hlblEID" runat="server"></span>
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
            
            <!---------->
            <!-- Form -->
            <!---------->
            <section class="ui-widget-content ui-corner-all">

                <!---------------------------->
                <!-- "Requestor ID" Section -->
                <!---------------------------->
                <div style="float: left; ">
                    <label style="float: left; padding: 0; width:auto; margin-top: -20px;"  runat="server" ID="lblEmpName"></label><br/>
                </div>

                <p style="height: 20px;"></p> 
                
                <%---------------------------------------%>
                <!-- List Of Open Incidents / Accidents -->
                <%---------------------------------------%>
                <div style="float: left; margin-right: 0; width: 40%; padding: 0;">
                    <div style="width: 100%; padding-top: 0; margin-left: auto; margin-right: auto;">
                        <div id="jTableContainer"></div>
                    </div>
                </div> 
                
                
                <%---------%>
                <!-- Tabs -->
                <%---------%>
               <div id="tabs"style="float: right; width: 55%; margin-right: 0; padding-left: 0;">

                    <ul>
                        <li><a href="#ClassTab">Classification</a></li>
                        <li><a href="#OshaTab">OSHA Log</a></li>
                        <li><a href="#EmpTab">Employee</a></li>
                        <li><a href="#FollowTab">Follow Up</a></li>
                        <li><a href="#CostTab">Cost</a></li>
                        <li><a href="#ApprTab">Approval</a></li>
                    </ul>


                    <%-----------------------%>
                    <!-- Classification Tab -->
                    <%-----------------------%>
                    <div id="ClassTab">
                        <section id="RptDisp" style="margin-top: 4px; width: 100%;"   >
                        
                            <div>
                                <div style="display: inline-block; width: 100px; margin-right: 10px; padding-bottom: 15px;">
                                    <div class="lblHead">Type</div>
                                    <span ID="Inc_Type" class="lbl">none</span>
                                </div>
                            
                                <div style="display: inline-block; width: 110px;  margin-right: 10px;">
                                    <div class="lblHead">Description</div>
                                    <span ID="Inc_Type_Sub" class="lbl">none</span>
                                </div>
                            
                                <div style="display: inline-block; width: 140px;  margin-right: 10px;">
                                    <div class="lblHead">Location</div>
                                    <span ID="Inc_Loc" class="lbl">none</span>
                                </div>
                            
                                <div style="display: inline-block; width: 100px;  margin-right: 10px;">
                                    <div class="lblHead">Occurance Date</div>
                                    <span ID="Inc_Occur_Date" class="lbl">none</span>
                                </div>
                            
                                <div style="display: inline-block; width: 100px;  margin-right: 10px;">
                                    <div class="lblHead">Unit No</div>
                                    <span ID="Emp_Veh_Involved" class="lbl">none</span>
                                </div>

                                <div style="display: inline-block; width: 100px;  margin-right: 10px;">
                                    <div class="lblHead">Job/Prj No</div>
                                    <span ID="Emp_Job_No" class="lbl">none</span> 
                                </div>

                                <div style="display: inline-block; width: 100px;  margin-right: 10px;">
                                    <div class="lblHead">Claim No</div>
                                    <div ID="Claim_ID" class="lbl">none</div>
                                </div>
                            </div>

                                    

                                     

                                     

                                     

                                    



                            

                            <div style="display: inline-block; width: 100%; margin-top: 15px;">
                                <div style="float: left; width: 45%;">
                                    <div class="lblHead">Incident Description</div>
                                    <span ID="Inc_Desc" class="lbl">none</span> 
                                </div>
                                
                                <div style="float: left; width: 45%; padding-left: 20px;">
                                    <div class="lblHead">Unsafe Act or Condition</div>
                                    <span ID="Inc_Unsafe_Act_or_Condition" class="lbl">none</span> 
                                </div>
                            </div>
                            

                    
                        </section>                        
                    </div>
                   

                    <%-------------%>
                    <!-- OSHA Tab -->
                    <%-------------%>
                    <div id="OshaTab">
                        <section id="Section1" style="margin-top: 4px; width: 100%;"   >
                        
                           <div style="width: 100%;">
                               <div style="margin-bottom: 5px; width: 100%;">
                                   <span class="lblHead2">Medical Record</span>
                                   <input type="checkbox" id="Osha_Record_Med" class="Chk" value="MedRcd" style="float: none; margin-left: 5px;" disabled="disabled"/>            
                                </div>                  
                        

                                <div style="margin-bottom: 5px; width: 100%;">
                                    <span class="lblHead2">Number of Restricted Duty Days:</span>
                                    <span ID="Osha_Restrict_Days"  runat="server"  style="width: 50px; float: none; margin-left: 5px;"></span>
                                </div>
                        

                                <div style="margin-bottom: 5px; width: 100%;">
                                    <span class="lblHead2">Number of Lost Time Days:</span>
                                    <span ID="Osha_Lost_Days" runat="server"   style="width: 50px; float: none; margin-left: 5px;"></span> 
                                </div>
                            </div>
                        </section>
                    </div>


                    <%-----------------%>
                    <!-- Employee Tab -->
                    <%-----------------%>                   
                    <div id="EmpTab">
                        <section id="Section2" style="margin-top: 4px; width: 100%;"   >
                            
                            <div style="width: 290px; margin-top: 10px; float: left; margin-right: 5px;">

                                <div style="clear: both; width: 290px; margin-top: -10px; ">
                                
                                    <div style="color: white; font-size: smaller; padding-top: 15px;">
                                        <label style="width: 80px; font-weight: bold;" for="Emp_ID">Employee:</label><span id="Emp_ID"></span><span style="padding-left: 10px;" id="Ename"></span>
                                        <br/>
                                        <label style="width: 80px; font-weight: bold;" for="Supr">Suprervisor:</label><span   id="Supr"></span>
                                        <br/>
                                        <label style="width: 80px; font-weight: bold;" for="Edept">Deptartment:</label><span  id="Edept"></span>
                                    </div>
                                </div>
                            </div>

                            <div style="margin-left: 0; width: 50%; float: left; margin-top: 10px; ">
                                <div style="float: left; width: 230px;  margin-bottom: 5px;">
                                    <div class="lblHead">Drug / Breath Alchol Test</div>
                                    <input type="checkbox" id="Emp_Drug_Alchol_Test" class="Chk" value="Emp_Drug_Alchol_Test" style="padding-left: 40%;" disabled="disabled"/>
                                </div> 

                                <div style="clear: both;"></div>                                
                            </div>
                            

                            <div style="float: left; width: 100%; margin-top: 10px; ">
                                <div class="lblHead">Employee Notes</div>
                                <span id="Emp_Comments" class="lbl" style="width: 100%;">none</span>    
                            </div>


                        </section>
                    </div>
                   

                    <%------------------%>
                    <!-- Follow-Up Tab -->
                    <%------------------%>
                    <div id="FollowTab">
                        
                        <div style="width: 100%; margin-left: -20px;">
                            <div style="width: 180px; float: left;">
                                <div style="height: 12px;"></div>
                                <span style="font-weight: bold;" >Discipline Issued</span>
                                <input type="checkbox" id="Follow_Discipline_Issued_Flag" class="Chk" value="Follow_Discipline_Issued_Flag" style="padding-left: 15px; display: inline-block;" disabled="disabled"/>
                            </div> 

                            <div style="float: left; margin-left: 30px;">
                                <div class="lblHead">Responsible for Follow Up</div>
                                <span ID="Follow_Responsible"  style="width: 250px;"></span>
                            </div>

                        </div>

                        <div style="clear: both;"></div>

                        <div style="margin-left: -10px;">
                            <div style="float: left; width: 300px; padding-right: 10px;">
                                <div class="lblHead" style="height: 40px; position: relative;  ">
                                    <div style=" bottom: 0; position: absolute; width: 100%;">Discipline Description</div>
                                </div>
                                <span id="Follow_Discipline" style="width: 100%;"></span>
                            </div>
                                
                            <div style="float: left; width: 300px; padding-right: 10px; padding-bottom: 15px;">
                                <div class="lblHead" style="height: 40px; position: relative;">
                                    <div style=" bottom: 0; position: absolute; width: 100%;">Actions Taken to Prevent Reoccurance</div>
                                </div>
                                <span id="Follow_Prevent_Reoccur" style="width: 100%;"></span>
                            </div>
                        
                            <div style="float: left; width: 300px;">
                                <div class="lblHead" style="height: 40px; position: relative;">
                                    <div style=" bottom: 0; position: absolute; width: 100%;">Additional Comments</div>
                                </div>
                                <span id="Follow_Comments" style="width: 100%;"></span>
                            </div>
                        </div>
                                             
                    </div>


                    <%----------------%>
                    <!-- Costing Tab -->
                    <%----------------%>
                    <div id="CostTab">
                        <section id="Section3" style="margin-top: 4px; width: 100%;"   >

                            <div>
                                <div style="display: inline-block; width: 100px; margin-right: 10px;">
                                    <div class="lblHead">In-House</div>
                                    <span ID="Cost_inHouse" class="lblCtr">0</span>
                                </div>

                                <div style="display: inline-block; width: 100px; margin-right: 10px;">
                                    <div class="lblHead">Incurred</div>
                                    <span ID="Cost_Incurred" class="lblCtr">0</span>
                                </div>

                                <div style="display: inline-block; width: 100px; margin-right: 10px;">
                                    <div class="lblHead">Reserves</div>
                                    <span ID="Cost_Reserve" class="lblCtr">0</span>
                                </div>

                                <div style="display: inline-block; width: 100px; margin-right: 10px;">
                                    <div class="lblHead">Total</div>
                                    <span ID="Cost_Total" class="lblCtr">0</span>
                                </div>

                            </div> 
                        </section>                     
                    </div>
                   

                    <%-----------------%>
                    <!-- Approval Tab -->
                    <%-----------------%>
                    <div id="ApprTab">
                        <section id="Section4" style="margin-top: 4px; width: 100%;"   >

                            <%----------------------%>
                            <!-- Approval Comments -->
                            <%-----------------------%>
                            <div style="float: left; margin-right: 0; width: 100%; padding: 0;">
                                <div style="width: 100%; padding-top: 0; margin-left: auto; margin-right: auto;">
                                    <div id="JTableIncidentComments"></div>
                                </div>
                            </div>   
                            
                            <%----------------------%>
                            <!-- Approval Buttons -->
                            <%-----------------------%>
                            <div style="clear: both; margin-top: -10px; ">
                                <div style="color: white; font-size: smaller; padding-top: 15px;">
                                    <div id="AppSuprDiv">
                                        <label style="width: 160px; font-weight: bold;">Supervisor:</label>
                                        <span class="lblEID" id="AppSuprID"></span>
                                        <span class="lblName" id="AppSuprName"></span>
                                        <span class="lblName" id="AppSuprDate"></span>
                                        <div style="height: 5px;"></div>
                                    </div>
                                    
                                    <div id="AppDeptDiv">
                                        <label style="width: 160px; font-weight: bold;">Department Manager:</label>
                                        <span class="lblEID"  id="AppDeptId"></span>
                                        <span class="lblName"  id="AppDeptName"></span>
                                        <span class="lblName"  id="AppDeptDate"></span>
                                        <div style="height: 5px;"></div>
                                    </div>
                                    
                                    <div id="AppDivDiv">
                                        <label style="width: 160px; font-weight: bold;">Division Manager:</label>
                                        <span class="lblEID"  id="AppDivId"></span>
                                        <span class="lblName"  id="AppDivName"></span>
                                        <span class="lblName"  id="AppDivDate"></span>
                                        <div style="height: 5px;"></div>
                                    </div>
                                    
                                    <div id="AppVpDiv">
                                        <label style="width: 160px; font-weight: bold;">Vice President:</label>
                                        <span class="lblEID"  id="AppVpId"></span>
                                        <span class="lblName"  id="AppVpName"></span>
                                        <span class="lblName"  id="AppVpDate"></span>
                                        <div style="height: 5px;"></div>
                                    </div>
                                    
                                    <div id="AppGmDiv">
                                        <label style="width: 160px; font-weight: bold;">GM:</label>
                                        <span class="lblEID"  id="AppGmId"></span>
                                        <span class="lblName"  id="AppGmName"></span>
                                        <span class="lblName"  id="AppGmDate"></span>
                                        <div style="height: 5px;"></div>
                                    </div>
                                        
                                    <div id="AppLegalDiv">
                                        <label style="width: 160px; font-weight: bold;">Legal:</label>
                                        <span class="lblEID"  id="AppLegalId"></span>
                                        <span class="lblName"  id="AppLegalName"></span>
                                        <span class="lblName"  id="AppLegalDate"></span>
                                        <div style="height: 5px;"></div>
                                    </div>

                                    <div id="AppEhsDiv">
                                        <label style="width: 160px; font-weight: bold;">EHS:</label>
                                        <span class="lblEID"  id="AppEhsId"></span>
                                        <span class="lblName"  id="AppEhsName"></span>
                                        <span class="lblName"  id="AppEhsDate"></span>
                                    </div>

                                    <div style="height: 5px;"></div>
                                    <input type="button" ID="btnApp" value="Approve" Class="SearchBtnCSS" />
                                    <input type="button" ID="btnCls" value="Close" Class="SearchBtnCSS" />
                                </div>
                            </div>
                        </section>                                         
                    </div>

                    <div style="clear: both;"></div>
                   
                
                </div>
                
                <div style="clear: both;"></div>
            </section>
        </div>
        
    </div>
</asp:Content>

