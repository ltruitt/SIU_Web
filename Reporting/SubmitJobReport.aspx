<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="SubmitJobReport.aspx.cs" Inherits="Reporting_SubmitJobReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Job Report</title>
    
    <link href="/Styles/SubmitTimeRpt.css" rel="stylesheet"  type="text/css" />
    <script type="text/javascript" src="http://code.jquery.com/ui/1.9.1/jquery-ui.min.js"></script>    
    <script type="text/javascript" src="/Scripts/SubmitJobRpt.js?0000"></script>  

    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />
    <script type="text/javascript" src="/Scripts/date.format.js"></script>  
    <script type="text/javascript" src="/Scripts/jquery.textchange.js"></script>  
    
    <style>
        .ui-tabs .ui-tabs-nav {
            background-image: none;
            margin-top: -15px;
            border: none;  
        }            
        .ui-form .ui-widget-content {
            padding-left: 3px;
            padding-right: 3px; 

        }

        .chkRptDisp .chkRptFmt .chkRts .chkPd .chkCt .chkOs .chkOsLtr .chkNoTest .ChkTest .chkIR .chkDrBox .chkIrDrBox .chkSales .chkIrData {
            border: none;  
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
                <%--<a href="/ELO/MainMenu.aspx" style="text-decoration: none;">--%>
                    <%--<span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">elo menu</span>--%>
                    <span id="PageTitle"  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Job Report
                        <span id="NotReady" style="font-size: .7em; position: relative; top: -2px;" ></span>
                    </span>
                <%--</a>--%>
            </section>  
            
            

    
            <section class="ui-widget-content ui-corner-all">

                <!---------------------------->
                <!-- "Requestor ID" Section -->
                <!---------------------------->
                <div style="float: left; ">
                    <label style="float: left; padding: 0; width:auto; margin-top: -20px;"  runat="server" ID="lblEmpName"></label><br/>
                </div>
                <a href="/My/MyPastDueJobRpts.aspx">
                    <div id="PastDueCnt" style="color: red; font-weight: bold; margin-top: 0; text-align: center; width: 90%; font-size: 1.5em;"></div>
                </a>

                <p style="height: 20px;"></p> 
                
                <div style="float: left; width: 320px; ">
                

                <!-- Job -->
                <div class="TimeRow" id="JobDiv"   style="margin-top: 20px;">    
                    <div style="float:left;  margin-right: 15px; padding-top: 5px; font-weight: bold; display:inline">Job:</div>
                    <input ID="ddJobNo" class="DataInputCss"   /> 
                    <div style="float:left;  margin-right: 15px; padding-top: 5px; font-weight: bold; display:inline; color: red; display: none;" ID="NoJobError">The Job Number Was Not Found</div>
                </div>

                    <!-- Hidden Labels Storing Detailed Data -->
               <section style="visibility: hidden;">
                    <span runat="server" id="hlblJobNo"        />
                    <span runat="server" id="hlblEID"          /> 
                </section>                
                
                    <!-- Display Labels Showing Selected Report Details -->
                    <!-- Comments -->
                    <!-- Submit and Clear Buttons -->
                    <div style="float: left; margin-right: 20px; width: 300px;">
                        <div style="color: white; font-size: smaller;">
                            <span  id="lblJobSite"></span>
                            <span  id="lblJobDesc"></span>
                            <span  id="lblJobNo"></span>
                            <span  id="lblJobStatus"></span>

                            <span  id="lblDept"></span>
                            <span  id="lblTech"></span>
                            <span  id="lblRptDisp"></span>
                            <br/>
                            <span  id="lblSubmitDate"></span>
                            <span  id="lblJhaDate"></span>
                            <span  id="lblIrSubmitDate"></span>
                            <span  id="lblIrCompleteDate"></span>
                            <br/>
                            <span  id="lblCost"></span>
                            <span  id="lblLoginDate"></span>
                            <span  id="lblDataEntryDate"></span>
                            <span  id="lblProofDate"></span>
                            <span  id="lblCorrectDate"></span>
                            <span  id="lblReviewDate"></span>
                            <span  id="lblReadyDate"></span>
                            <span  id="lblErr" Class="errorTextCss"  ></span>

                            <span  id="lblErrServer" Class="errorTextCss"  ></span>
                        </div>
                        
                           
                           
                        <%--------------------------------------------%>                    
                        <%-- Comment Text, Submit and Clear Buttons --%>                    
                        <%--------------------------------------------%>
                        <div id="Comments" style="float: left; margin-right: 20px; ">
                            <div  style="float: left;  ">
                                <div style="margin-top: 15px;">Comments (Max 250 Characters)</div>
                                <textarea id="txtComments" class="DataInputCss" style="width: 120%;" rows="5" maxlength="250"></textarea>
                            </div>
                    

                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <div style="float: left; width: 30%; ">
                                    <input type="button" ID="btnSubmit" value="Submit" Class="SearchBtnCSS" />
                                </div>
                    
                                        
                                <div style="float: left; width: 25%; ">
                                    <input type="button" ID="btnClear" value="Clear" Class="SearchBtnCSS" />
                                </div>
                            </div>
                        </div>   
                    </div>                
                

                </div>           

                <div id="tabs"style="float: right; width: 60%;">                   

                    <ul>
                        <li><a href="#DispTab">Disposition</a></li>
                        <li><a href="#FormatTab">Format</a></li>
                        <li><a href="#OtherDataTab">Other Data</a></li>
                        <li><a href="#TestsTab">Test Description</a></li>
                        <li><a href="#IrTab">IR</a></li>
                        <li><a href="#SalesTab">Customer Follow Up</a></li>
                    </ul>

                    <div id="DispTab">
                        <section id="RptDisp" class="ui-state-error ui-corner-all" style="margin-top: 4px; width: 100%;"   >
                        
                            <div style="font-weight: bold; text-align: center;">Report Disposition</div>

                            <div style="display: inline-block; width: 100%; margin-bottom: 5px;">
                                <input type="checkbox" id="chkComplete" class="chkRptDisp" value="Complete" />
                                <span>Complete</span>
                            </div>

                            <div style="display: inline-block; width: 100%;  margin-bottom: 5px;">
                                <input type="checkbox" id="chkPartial" class="chkRptDisp" value="Partial"/>
                                <span>Partial</span>
                            </div>                  

                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <input type="checkbox" id="chkPe" value="PEStamp"/>
                                <span>PE Stamp Required</span>
                            </div>

                            <div style="display: inline-block; width: 100%;">
                                <div>No Report (Reason Required)</div>
                                <input type="text" id="txtNoRpt" class="DataInputCss" style="width: 80%;" maxlength="100"/>
                            </div>
                    
                        </section>                        
                    </div>
                  
                    <div id="FormatTab">
                        <section id="RptFmt" class="ui-state-error ui-corner-all" style="margin-top: 4px; "   >
                            
                            <div style="font-weight: bold; text-align: center;">Report Format<br/>( Must answer Dropbox AND Select one report format )</div>

                            <div style="display: inline-block; width: 100%;   margin-bottom: 0; ">
                                <span style="padding-top: 5px; padding-left: 3px; -webkit-margin-start: 3px;  margin-left: 35%;">Y</span>
                                <span style="padding-top: 5px; padding-left: 9px; -webkit-margin-start: 8px;">N</span>
                            </div>                        
                                                        
                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <span>All data is in Dropbox</span>
                                <input type="checkbox" id="RptDrBoxY"   class="chkDrBox" value="1"   style="margin-left: 35%;"/>
                                <input type="checkbox" id="RptDrBoxN"   class="chkDrBox" value="2"   />
                            </div>
                            

                            
                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <input type="checkbox" id="chkNoData" class="chkRptFmt" value="NoData"/>
                                <span>Letter Report Only-No Data</span>
                            </div>

                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <input type="checkbox" id="chkPowerDB" class="chkRptFmt" value="PowerDB"/>
                                <span>All Data has been entered in PowerDB</span>
                            </div>

                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <input type="checkbox" id="chkScanned" class="chkRptFmt" value="Scanned"/>
                                <span>Only Handwritten (Scanned in Job Folder OR Placed in the 8082 inbox)</span>
                            </div>

                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <input type="checkbox" id="chkPdbMaster" class="chkRptFmt" value="LetterJobFolder"/>
                                <span>Combination of handwritten data and typed data in PowerDB</span>
                            </div>

                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <input type="checkbox" id="chkOtherData" class="chkRptFmt" value="LetterJobFolder"/>
                                <span><b>ONLY</b> RTS/CT/Oil/PD/Etc. - Fill out 'Other' Tab</span>
                            </div>
                        
                        </section>                 
                    </div>

                    <div id="OtherDataTab">
                        <section id="Section1" class="ui-state-error ui-corner-all" style="margin-top: 4px;"   >
                            
                            <div style="font-weight: bold; text-align: center;">Other Data<br/>(Must Answer Each Question)</div>

                            <div style="display: inline-block; width: 100%;">
                                <div style="margin-top: 10px;">Other Data in Dropbox (Max 100 Characters)</div>
                                <input type="text" id="OtherData" class="DataInputCss" style="width: 80%;" maxlength="100"    />
                            </div>
                        
                        
                        
                            <div style="display: inline-block; width: 100%;   margin-bottom: 0;">
                                <span style="padding-top: 5px; padding-left: 3px; -webkit-margin-start: 3px;">Y</span>
                                <span style="padding-top: 5px; padding-left: 9px; -webkit-margin-start: 8px;">N</span>
                                <span style="padding-top: 5px; padding-left: 4px; -webkit-margin-start: 6px;">N/A</span>
                            </div>                        

                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <input type="checkbox" id="CbRtsY"   class="chkRts" value="1"/>
                                <input type="checkbox" id="CbRtsN"   class="chkRts" value="2"/>
                                <input type="checkbox" id="CbRtsNA"   class="chkRts" value="3"/>
                                <span>RTS Relay Data saved as a PDF in Dropbox</span>
                            </div>

                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <input type="checkbox" id="CbCtY"   class="chkCt" value="1"/>
                                <input type="checkbox" id="CbCtN"   class="chkCt" value="2"/>
                                <input type="checkbox" id="CbCtNA"   class="chkCt" value="3" />
                                <span>CT Data Saved as a PDF in Dropbox</span>
                            </div>
                        
                        
                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <input type="checkbox" id="CbPdY"   class="chkPd" value="1"/>
                                <input type="checkbox" id="CbPdN"   class="chkPd" value="2"/>
                                <input type="checkbox" id="CbPdNA"   class="chkPd" value="3" />
                                <span>Partial Discharge, Level 2 in the Dropbox</span>
                            </div>
                            
                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <input type="checkbox" id="CbOsY"   class="chkOs" value="1"/>
                                <input type="checkbox" id="CbOsN"   class="chkOs" value="2"/>
                                <input type="checkbox" id="CbOsNA"   class="chkOs" value="3" />
                                <span>Oil Samples</span>
                            </div>
                            
                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <input type="checkbox" id="CbOsLtrY"   class="chkOsLtr" value="1"/>
                                <input type="checkbox" id="CbOsLtrN"   class="chkOsLtr" value="2"/>
                                <input type="checkbox" id="CbOsLtrNA"   class="chkOsLtr" value="3" />
                                <span>Oil Sample Deficiencies noted in the Customer Letter</span>
                            </div>                                                        
                        </section>
                    </div>
                    
                    
                    

                    <div id="TestsTab">
                        <section id="Section3" class="ui-state-error ui-corner-all" style="margin-top: 4px; "   >
                            
                            <div style="font-weight: bold;  width: 100%; text-align: center;">Job Testing<br/>(Must Select At Least One)</div>
                            
                            <div style="display: inline-block; width: 100%; text-align: center;  margin-bottom: 10px;">
                                <div style="display: inline-block; ">
                                    <input type="checkbox" id="CbTestNone"   class="chkNoTest" value="NoData"/>
                                    <span>No Testing Was Performed</span>
                                </div>
                            </div>
                            
                            

                            <div style="width: 48%; display: inline-block;">
                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestBBT"   class="ChkTest" value="NoData"/>
                                    <span>Bus Bolt Torque</span>
                                </div>

                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestHiPot"   class="ChkTest" value="PowerDB"/>
                                    <span>DC Hipot</span>
                                </div>

                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestDecal"   class="ChkTest" value="Scanned"/>
                                    <span>Decal Color Codes</span>
                                </div>

                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestDLRO"   class="ChkTest" value="8082Box"/>
                                    <span>DLRO</span>
                                </div>

                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestDoble"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>Doble</span>
                                </div>

                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestGrdFlt"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>Ground Fault Systems</span>
                                </div>

                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestGrdEltrode"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>Grounding and Ground Electrode</span>
                                </div>

                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestInslResit"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>Insulation Resistance</span>
                                </div>
                            </div>
                            
                            <div style="width: 48%; display: inline-block;">
                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestNFPA"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>NFPA-99 Testing</span>
                                </div>                                
                                
                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestOil"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>Oil Tests</span>
                                </div>
                                
                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestPD"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>Partial Discharge</span>
                                </div>                                
                                
                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestPCB"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>PCB Info</span>
                                </div>
                                
                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestRelay"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>Protective Relays</span>
                                </div>                                
                                
                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestThermo"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>Thermographic (IR)</span>
                                </div>
                                
                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestTTR"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>TTR</span>
                                </div>

                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="CbTestSonic"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>Ultrasonic</span>
                                </div>

                            </div>
                        
                        </section> 
                    </div>
  
                    <div id="IrTab">                        
                        <section id="Section2" class="ui-state-error ui-corner-all" style="width: 100%;  margin-top: 4px;"   >
                        
                            <div style="font-weight: bold; text-align: center;">Infrared<br/>(ONLY If You Have Infrared Data)</div>  
                            <br/>                        

                            <div id="irQ" style="text-align: center;">    
                                <div>Are you turning in I/R data?</div>
                            
                                <div style="display: inline-block; width: 100%;   margin-bottom: 0; ">
                                    <span style="padding-top: 5px; padding-left: 3px; -webkit-margin-start: 3px;">Y</span>
                                    <span style="padding-top: 5px; padding-left: 9px; -webkit-margin-start: 8px;">N</span>
                                </div>    

                                <div style="display: inline-block; width: 100%; margin-left: 48.5%; margin-bottom: 10px; ">
                                    <input type="checkbox" id="chkIrDataY"   class="chkIrData" value="1" />
                                    <input type="checkbox" id="Checkbox2"   class="chkIrData" value="2" />
                                </div>                                  
                            </div>
                            
                            
                            

                              
                            <div id="irDataDiv">
                                <div style="display: inline-block; width: 100%;   margin-bottom: 0; ">
                                    <span style="padding-top: 5px; padding-left: 3px; -webkit-margin-start: 3px;  margin-left: 35%;">Y</span>
                                    <span style="padding-top: 5px; padding-left: 9px; -webkit-margin-start: 8px;">N</span>
                                </div>   
                            
                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <span>All IR data is in Dropbox</span>
                                    <input type="checkbox" id="IrDrpBoxY"   class="chkIrDrBox" value="1"   style="margin-left: 35%;"/>
                                    <input type="checkbox" id="IrDrpBoxN"   class="chkIrDrBox" value="2"   />
                                </div>

                                <div style="display: inline-block; width: 100%;   margin-bottom: 5px; margin-top: 10px;">
                                    <input type="checkbox" id="chkIrOnly"   class="chkIR" value="IrData"/>
                                    <span>IR Report Only</span>
                                </div>
                            
                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <input type="checkbox" id="chkIrPort"   class="chkIR" value="IrData"/>
                                    <span>IR is portion of Final Report</span>
                                </div>
                            
                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <div>How Many Hard Copies: </div>
                                    <input type="text" id="txtIrHardCnt" class="DataInputCss"  value="0"  maxlength="1"  style="width: 20px; text-align: center;"  />
                                
                                </div>
                            
                                <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                    <div>Additional Email Address: </div>
                                    <input type="text" id="txtAddEmail" class="DataInputCss"  style="width: 400px;" maxlength="50"/>
                                </div>
                            </div>


                        </section>
                    </div>
                    
                    <div id="SalesTab">                        
                        <section id="Section4" class="ui-state-error ui-corner-all" style="width: 100%;  margin-top: 4px;"   >
                            <div style="font-weight: bold; text-align: center;">Optional Notes for sales follow-up</div>    
                            
                            <br/>                        
                            <div>Were there any defencies found OR does the Salesperson need to call the customer ?</div>
                            
                            <div style="display: inline-block; width: 100%;   margin-bottom: 0; ">
                                <span style="padding-top: 5px; padding-left: 3px; -webkit-margin-start: 3px;">Y</span>
                                <span style="padding-top: 5px; padding-left: 9px; -webkit-margin-start: 8px;">N</span>
                            </div>    

                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <input type="checkbox" id="chkSalesY"   class="chkSales" value="1" />
                                <input type="checkbox" id="chkSalesN"   class="chkSales" value="2" />
                            </div>                             
                            
                            <div id="saleNotesDiv" style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <div>Notes to salesperson: (Max 100 characters)</div>
                                <textarea id="txtSalesNotes" class="DataInputCss" style="width: 90%;" rows="5" maxlength="100"></textarea>
                            </div>
                        </section>
                    </div>
                                        
                </div>

                <div style="clear: both; width: 100%;  margin-top: 0;"/>
                                
                

            </section>      
         </div>
     </div>       
     
</asp:Content>

