<%@ Page Title="" Language="C#" MasterPageFile="/Phone/PhoneMaster.master" AutoEventWireup="true" CodeFile="SubmitJobReport.aspx.cs" Inherits="Reporting_SubmitJobReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Job Report</title>
    
    <link href="/Phone/Styles/SubmitTimeRpt.css" rel="stylesheet"  type="text/css" />
    <script type="text/javascript" src="http://code.jquery.com/ui/1.9.1/jquery-ui.min.js"></script>    
    <script type="text/javascript" src="/Scripts/SubmitJobRpt.js"></script>  
    
    <script type="text/javascript" src="/Scripts/date.format.js"></script> 
    
    <style>
        .ui-tabs .ui-tabs-nav {
            background-image: none;
        }            
        .ui-form .ui-widget-content {
            padding-left: 3px;
            padding-right: 3px;   
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    

    <div style="background-color: #45473f; ">
        <div id="FormWrapper" class="ui-widget ui-form">
                               
            <!-- Hidden Labels Storing Detailed Data -->
            <section style="visibility: hidden; height: 0; width: 0;">
                <span id="hlblJobNo"></span>
                <span runat="server" id="hlblEID"></span> 
            </section>
    
            <section class="ui-widget-content ui-corner-all" id="JobDiv">
                
                    <!---------------------------->
                    <!-- "Requestor ID" Section -->
                    <!---------------------------->
                    <div style="float: left; ">
                        <label style="float: left; padding: 0; width:auto; margin-top: -20px;"  ID="lblEmpName2" runat="server"></label><br/>
                    </div>

                    <!----------------->
                    <!-- Form Header -->
                    <!----------------->                   
                    <section class="ui-widget-header ui-corner-all" style="height: 45px; margin-top: 40px;" >
                        <a href="/Phone/ELO/MainMenu.aspx" style="text-decoration: none;">
                            <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">elo menu</span>
                            <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Submit Job Rpt</span>
                        </a>
                    </section>  
                    
                 
                    <p style="height: 0;"></p>                 
                    <!-- Job -->
                    <div class="TimeRow"    style="margin-top: 20px;">    
                        <div style="float:left;  margin-right: 15px; margin-left: 5px; padding-top: 2px; font-weight: bold; display:inline">Job:</div>
                        <input ID="ddJobNo" class="DataInputCss"   /> 
                    </div>

                
                    <div style="clear: both; width: 100%;  margin-top: 0;"/>
                </section>

                
                

            <div id="tabs">                   
                
                <!---------------------------->
                <!-- "Requestor ID" Section -->
                <!---------------------------->
                <label style="float: left; padding: 0; width:auto; margin-top: -20px;"   ID="lblEmpName" runat="server"></label>

                <%------------------------------%>                    
                <%-- Submit and Clear Buttons --%>                    
                <%------------------------------%>
                <div style="float: right; margin-right: 10px;">
                    <input type="button" ID="btnClear" value="Clear" Class="SearchBtnCSS"  />
                </div>

                <div id="Comments" style="float: right; margin-right: 20px;">
                    <input type="button" ID="btnSubmit" value="Submit" Class="SearchBtnCSS" />
                </div>


                <!----------------->
                <!-- Form Header -->
                <!----------------->                   
                <section class="ui-widget-header ui-corner-all" style="height: 45px; margin-top: 40px;" >
                    <a href="/Phone/ELO/MainMenu.aspx" style="text-decoration: none;">
                        <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">elo menu</span>
                        <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Submit Job Rpt</span>
                    </a>
                </section>  
                    
                 


                <%--Tabs--%>
                <ul>
                    <li><a href="#JobTab">Job</a></li>
                    <li><a href="#DispTab">Disp</a></li>
                    <li><a href="#FormatTab">Format</a></li>
                    <li><a href="#OtherDataTab">Other</a></li>
                    <li><a href="#TestsTab">Test Desc</a></li>
                    <li><a href="#IrTab">IR</a></li>
                    <li><a href="#SalesTab">Sales</a></li>
                </ul>

                <div id="JobTab">
                    <section id="Section4" class="ui-state-error ui-corner-all" style="margin-top: 4px; float: none; width: 97%;"   >
                        <div style="font-weight: bold; text-align: center;">Job Overview</div>
                            

                        <div style=" font-size: smaller;">
                            <span  id="lblJobSite"></span>
                            <span  id="lblJobDesc"></span>
                            <span  id="lblJobNo"></span>
                            <span  id="lblJobStatus"></span>

                            <span  id="lblDept"></span>
                            <span  id="lblTech"></span>
                            <span  id="lblRptDisp"></span>
                            
                            <span  id="lblSubmitDate"></span>
                            <span  id="lblJhaDate"></span>
                            <span  id="lblIrSubmitDate"></span>
                            <span  id="lblIrCompleteDate"></span>
                            
                            <span  id="lblCost"></span>
                            <span  id="lblLoginDate"></span>
                            <span  id="lblDataEntryDate"></span>
                            <span  id="lblProofDate"></span>
                            <span  id="lblCorrectDate"></span>
                            <span  id="lblReviewDate"></span>
                            <span  id="lblReadyDate"></span>
                            <span  id="lblErr" Class="errorTextCss"></span>

                            <span  id="lblErrServer" Class="errorTextCss"  ></span>
                            
                            <div style="height: 50px;">
                                <div style="margin-top: 15px; font-weight: bold;">Comments</div>
                                <input type="text" id="txtComments" class="DataInputCss" style="width: 250px;"    />
                            </div>


                        </div>
                    </section>                        
                </div>

                <div id="DispTab">
                    <section id="RptDisp" class="ui-state-error ui-corner-all" style="margin-top: 4px;"   >
                        
                        <div style="font-weight: bold; text-align: center;">Report Disposition</div>

                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <input type="checkbox" id="chkComplete"   class="chkRptDisp" value="Complete" />
                                <span>Complete</span>
                            </div>

                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <input type="checkbox" id="chkPartial"     class="chkRptDisp" value="Partial"/>
                                <span>Partial</span>
                            </div>                  

                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <input type="checkbox" id="chkPe"   value="PEStamp"/>
                                <span>PE Stamp Required</span>
                            </div>

                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <div>No Report (Reason Required)</div>
                                <input type="text" id="txtNoRpt" class="DataInputCss"  style="width: 80%;"     />
                            </div>
                    
                    </section>                        
                </div>
                  
                <div id="FormatTab">
                    <section id="RptFmt" class="ui-state-error ui-corner-all" style="margin-top: 4px; "   >
                            
                        <div style="font-weight: bold; text-align: center;">Report Format<br/>( Must answer Dropbox AND<br/>Select one report format )</div>
                        
                            <div style="display: inline-block; width: 100%;   margin-bottom: 0; ">
                                <span style="padding-top: 5px; padding-left: 3px; -webkit-margin-start: 3px;  margin-left: 23%;">Y</span>
                                <span style="padding-top: 5px; padding-left: 9px; -webkit-margin-start: 8px;">N</span>
                            </div>   

                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <span>All data is in Dropbox</span>
                                <input type="checkbox" id="RptDrBoxY"   class="chkDrBox" value="1"   style="margin-left: 23%;"/>
                                <input type="checkbox" id="RptDrBoxN"   class="chkDrBox" value="2"   />
                            </div>

                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <input type="checkbox" id="chkNoData"   class="chkRptFmt" value="NoData"/>
                                <span>Letter Only-No Data</span>
                            </div>

                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <input type="checkbox" id="chkPowerDB"   class="chkRptFmt" value="PowerDB"/>
                                <span>All Data is in PowerDB</span>
                            </div>

                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <input type="checkbox" id="chkScanned"   class="chkRptFmt" value="Scanned"/>
                                <span>Handwritten Data Only</span>
                            </div>

                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <input type="checkbox" id="chkPdbMaster"   class="chkRptFmt" value="LetterJobFolder"/>
                                <span>Handwritten and PowerDB data</span>
                            </div>

                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <input type="checkbox" id="chkOtherData"   class="chkRptFmt" value="LetterJobFolder"/>
                                <span><b>ONLY</b> RTS/CT/Oil/PD/Etc. - Fill out 'Other' Tab</span>
                            </div>
                        
                    </section>                 
                </div>

                <div id="OtherDataTab">
                    <section id="Section1" class="ui-state-error ui-corner-all" style="margin-top: 4px;"   >
                            
                        <div style="font-weight: bold; text-align: center;">Other Data<br/>( Must Answer Each Question )</div>

                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <div style="margin-top: 10px;">Other Data in Dropbox</div>
                                <input type="text" id="OtherData" class="DataInputCss" style="width: 80%;"    />
                            </div>
                        
                        
                        
                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <span style="padding-top: 5px; padding-left: 10px;">Y</span>
                                <span style="padding-top: 5px; padding-left: 20px;">N</span>
                                <span style="padding-top: 5px; padding-left: 12px;">N/A</span>
                            </div>                        

                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <input type="checkbox" id="CbRtsY"  class="chkRts" value="1"/>
                                <input type="checkbox" id="CbRtsN"  class="chkRts" value="2"/>
                                <input type="checkbox" id="CbRtsNA" class="chkRts" value="3" checked="checked"/>
                                <span>RTS Relay Data saved as a PDF in Dropbox</span>
                            </div>

                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <input type="checkbox" id="CbCtY"   class="chkCt" value="1"/>
                                <input type="checkbox" id="CbCtN"   class="chkCt" value="2"/>
                                <input type="checkbox" id="CbCtNA"   class="chkCt" value="3" checked="checked"/>
                                <span>CT Data Saved as a PDF in Dropbox</span>
                            </div>
                        
                        
                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <input type="checkbox" id="CbPdY"   class="chkPd" value="1"/>
                                <input type="checkbox" id="CbPdN"   class="chkPd" value="2"/>
                                <input type="checkbox" id="CbPdNA"   class="chkPd" value="3" checked="checked"/>
                                <span>Partial Discharge, Level 2 in the Dropbox</span>
                            </div>
                            
                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <input type="checkbox" id="CbOsY"   class="chkOs" value="1"/>
                                <input type="checkbox" id="CbOsN"   class="chkOs" value="2"/>
                                <input type="checkbox" id="CbOsNA"   class="chkOs" value="3" checked="checked"/>
                                <span>Oil Samples</span>
                            </div>
                            
                            <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                                <input type="checkbox" id="CbOsLtrY"   class="chkOsLtr" value="1"/>
                                <input type="checkbox" id="CbOsLtrN"   class="chkOsLtr" value="2"/>
                                <input type="checkbox" id="CbOsLtrNA"   class="chkOsLtr" value="3" checked=checked/>
                                <span>Oil Sample Deficiencies noted in the Customer Letter</span>
                            </div>                                                         
                            
                            

                    </section>
                </div>

                <div id="TestsTab">
                    <section id="Section3" class="ui-state-error ui-corner-all" style="margin-top: 4px; "   >
                            
                        <div style="font-weight: bold; width: 100%; text-align: center;">Job Testing<br/>( Must Select At Least One )</div>
                            
                            <div style="display: inline-block; width: 100%; text-align: center; margin-top: 10px;">
                                <div style="display: inline-block; ">
                                    <input type="checkbox" id="CbTestNone"   class="chkNoTest" value="NoData"/>
                                    <span>No Testing Was Performed</span>
                                </div>
                            </div>
                            
                            
                            
                            
                            

                            <div style="padding-top: 5px;">
                                <div style="display: inline-block; width: 49%; margin-top: 10px;">
                                    <input type="checkbox" id="CbTestBBT"   class="ChkTest" value="NoData"/>
                                    <span>Bus Bolt Torque</span>
                                </div>
                            
                                <div style="display: inline-block; width: 49%; margin-top: 10px;">
                                    <input type="checkbox" id="CbTestNFPA"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>NFPA-99</span>
                                </div>   
                            </div>                                                        
                        
                        

                            <div style="padding-top: 5px;">
                                <div style="display: inline-block; width: 49%; margin-top: 10px;">
                                    <input type="checkbox" id="CbTestHiPot"   class="ChkTest" value="PowerDB"/>
                                    <span>DC Hipot</span>
                                </div>      
                                <div style="display: inline-block; width: 49%; margin-top: 10px;">
                                    <input type="checkbox" id="CbTestOil"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>Oil Tests</span>
                                </div>                                                  
                            </div>                                                        
                        
                        
                        

                            <div style="padding-top: 5px;">
                                <div style="display: inline-block; width: 49%; margin-top: 10px;">
                                    <input type="checkbox" id="CbTestSonic"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>Ultrasonic</span>
                                </div> 
                                <div style="display: inline-block; width: 49%; margin-top: 10px;">
                                    <input type="checkbox" id="CbTestTTR"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>TTR</span>
                                </div>                                                                                     
                            </div>                                                        
                        
                            <div style="padding-top: 5px;">
                                <div style="display: inline-block; width: 48%; margin-top: 10px;">
                                    <input type="checkbox" id="CbTestDLRO"   class="ChkTest" value="8082Box"/>
                                    <span>DLRO</span>
                                </div>  
                                <div style="display: inline-block; width: 48%; margin-top: 10px;">
                                    <input type="checkbox" id="CbTestPCB"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>PCB Info</span>
                                </div>                                                      
                            </div>                                                        
                        
                            <div style="padding-top: 5px;">
                                <div style="display: inline-block; width: 49%; margin-top: 10px;">
                                    <input type="checkbox" id="CbTestRelay"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>Protect Relays</span>
                                </div>                                     
                                <div style="display: inline-block; width: 49%; margin-top: 10px;">
                                    <input type="checkbox" id="CbTestDoble"   class="ChkTest" value="LetterJobFolder"/>
                                    <span>Doble</span>
                                </div>         
                            </div>                                                        
                        
                            <div>
                                <div style="display: inline-block; width: 100%; margin-top: 10px;">
                                <input type="checkbox" id="CbTestPD"   class="ChkTest" value="LetterJobFolder"/>
                                <span>Partial Discharge</span>
                                </div>                                                   
                            </div>   
                        
                            <div style="display: inline-block; width: 100%; margin-top: 10px;">
                                <input type="checkbox" id="CbTestDecal"   class="ChkTest" value="Scanned"/>
                                <span>Decal Color Codes</span>
                            </div>  
                            
                            <div style="display: inline-block; width: 100%; margin-top: 10px;">
                                <input type="checkbox" id="CbTestGrdFlt"   class="ChkTest" value="LetterJobFolder"/>
                                <span>Ground Fault Systems</span>
                            </div>      
                            
                            <div style="display: inline-block; width: 100%; margin-top: 10px;">
                                <input type="checkbox" id="CbTestInslResit"   class="ChkTest" value="LetterJobFolder"/>
                                <span>Insulation Resistance</span>
                            </div>       

                            <div style="display: inline-block; width: 100%; margin-top: 10px;">
                                <input type="checkbox" id="CbTestGrdEltrode"   class="ChkTest" value="LetterJobFolder"/>
                                <span>Grounding and Ground Electrode</span>
                            </div>                                                                                                                                                                                                                                                                                                        

                            <div style="display: inline-block; width: 100%; margin-top: 10px;">
                                <input type="checkbox" id="CbTestThermo"   class="ChkTest" value="LetterJobFolder"/>
                                <span>Thermographic (IR)</span>
                            </div>                                                                             
                    </section> 
                </div>
  
                <div id="IrTab">
                        
                    <section id="Section2" class="ui-state-error ui-corner-all" style="width: 100%;  margin-top: 4px;"   >
                        
                        <div style="font-weight: bold; text-align: center;">Infrared<br/>( ONLY If You Have Infrared Data )</div>    
                        
                        <div style="display: inline-block; width: 100%;   margin-bottom: 0; ">
                            <span style="padding-top: 5px; padding-left: 3px; -webkit-margin-start: 3px;  margin-left: 23%;">Y</span>
                            <span style="padding-top: 5px; padding-left: 9px; -webkit-margin-start: 8px;">N</span>
                        </div>   

                        <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                            <span>All IR data is in Dropbox</span>
                            <input type="checkbox" id="IrDrpBoxY"   class="chkIrDrBox" value="1"   style="margin-left: 23%;"/>
                            <input type="checkbox" id="IrDrpBoxN"   class="chkIrDrBox" value="2"   />
                        </div>
                        

                        <div style="display: inline-block; width: 100%;   margin-bottom: 5px; margin-top: 10px;">
                            <input type="checkbox" id="chkIrOnly"   class="chkIR" value="IrData"/>
                            <span>IR Report Only</span>
                        </div>
                            
                        <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                            <input type="checkbox" id="chkIrPort"   class="chkIR" value="IrData"/>
                            <span>IR Portion of Final Report</span>
                        </div>
                            
                        <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                            <div>How Many Hard Copies: </div>
                            <input type="text" id="txtIrHardCnt" class="DataInputCss"  value="0"  maxlength="1"  style="width: 20px; text-align: center;"/>
                                
                        </div>
                            
                        <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                            <div>Additional Email Address: </div>
                            <input type="text" id="txtAddEmail" class="DataInputCss"  style="width: 285px;"/>
                                
                        </div>

                    </section>
                </div>
                
                    <div id="SalesTab">                        
                        <section id="Section5" class="ui-state-error ui-corner-all" style="width: 100%;  margin-top: 4px;"   >
                            <div style="font-weight: bold; text-align: center;">Optional Notes for sales follow-up</div>    
                            
                            <br/>
<%--                            <div style="display: inline-block; width: 100%;   margin-bottom: 0; ">
                                <span style="padding-top: 5px; padding-left: 3px; -webkit-margin-start: 3px;">Y</span>
                                <span style="padding-top: 5px; padding-left: 9px; -webkit-margin-start: 8px;">N</span>
                            </div>  --%>                            
                            
                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <span>Were there any defencies found OR does the Salesperson need to call the customer</span>
                                <input type="checkbox" id="chkSalesY"   class="chkSales" value="1" />
                                <%--<input type="checkbox" id="chkSalesN"   class="chkSales" value="2"   />--%>
                            </div>                             
                            
                            <div style="display: inline-block; width: 100%;   margin-bottom: 10px;">
                                <div>Notes to salesperson: </div>
                                <textarea id="txtSalesNotes" class="DataInputCss" style="width: 90%;" rows="5"></textarea>
                            </div>
                        </section>
                    </div>
                    
                <div style="clear: both; width: 100%;  margin-top: 0;"/>
            </div>    
                
         </div>
     </div>       

</asp:Content>

