<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="TimeEntry.aspx.cs" Inherits="ELO_TimeEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>ELO Time Entry</title>
    
    <link href="/Styles/ELO.css" rel="stylesheet"  type="text/css" />
    <link href="/Styles/TimeEntry.css" rel="stylesheet"  type="text/css" />
    <script type="text/javascript" src="/Scripts/TimeEntry.js"></script>     
    <script type="text/javascript" src="/Scripts/TimeEntryDeskTopAddon.js"></script>     
    <script src="/Scripts/Highcharts-3.0.2/js/highcharts.js" type="text/javascript"></script> 
    
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />

</asp:Content>






<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <div >
        <div id="FormWrapper" class="ui-widget ui-form">        
            <!----------------->
            <!-- Form Header -->
            <!----------------->                   
            <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
                <a href="/ELO/MainMenu.aspx" style="text-decoration: none;">
                    <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">elo menu</span>
                    <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Time Card</span>
                </a>
            </section>  
    
            <section class="ui-widget-content ui-corner-all">

                <!---------------------------->
                <!-- "Requestor ID" Section -->
                <!---------------------------->
                <div style="margin-left: -15px;  width: auto;">
                    
                    <div style="margin-top: -20px;">
                        <label style="float: none; padding: 0; width:auto; margin-top: -20px;"   ID="lblEmpName" runat="server"></label>
                        <span ID="imgRejects"  class="VHcss" style="float: none; margin-left: 7px; background-color: red;" runat="server"  >
                            <a href="/ELO/TimeRpt.aspx" style="color:white; font-weight: bold;">Rejected</a>
                        </span>
                        <span ID="imgReport"   style="float: right; margin-right: 21px;" runat="server" >
                            <a href="/ELO/TimeRpt.aspx" style="color:white; font-weight: bold;">Day Total</a>
                        </span>
                        
                        <span ID="Span1"   style="float: right; margin-right: 31px; "  >
                            <a href="/My/MyTimeStudy.aspx" style="color:white; font-weight: bold;">Time Study</a>
                        </span>
                    </div>
                    
                    <div style="height: 20px;"></div>

                    <div style="height: 20px; float: right; ">                    
                        <span class="VHcss" style="margin-left: -5px;"  >Vac:
                            <span style="float: right; margin-left: 7px; "   ID="lblVac" ></span>
                        </span>

                        <span class="VHcss" style="margin-left: -5px;"  >Per:
                            <span style="float: right; margin-left: 7px; "   ID="lblHol" ></span>
                        </span>
                    
                        <span class="VHcss" style="margin-left: -5px; display: none;">Sick:
                            <span style="float: right; margin-left: 7px;"   ID="lblSick" ></span>
                        </span>
                    </div>
                </div>


                <p style="height: 5px;"></p> 
                



                <div id="MonthDaily" style="-moz-min-width: 400px; -ms-min-width: 400px; -o-min-width: 400px; -webkit-min-width: 400px; min-width: 400px; height: 200px; margin: 0 auto"></div>
                



                <!-- Date For Time Being Entered -->
                <div class="TimeRow" style="margin-left: auto; margin-right: auto;  margin-top: 25px; width: 100%; text-align: center;">
                    <span style="display: inline-block; margin-left: 0; margin-right: 10px; line-height: 20px; vertical-align: top;">Time For:</span>

                    <div style="display: inline-block;">
                        <input type="text" id="txtEntryDate" Class="DataInputCss DateEntryCss"   style="width: 100px;" runat="server" />   
                    </div>
                 </div>

                 
                 
                 <!-- Show Days For This Week Where Time Enetered -->
                 <div style="margin-bottom: 3px; margin-top: -6px;  width: 100%; text-align: center;">
                         
                    <table style=" width: auto;  margin-left: auto; margin-right: auto; ">
                        <tr >
                            <td><span class="prevarrow" style="top: 100px;"><a href="#" ></a></span></td>
                            <td style="padding: 0 2px 0 10px;"><input id="btnMon" runat="server" type="button" class="DowBtnCSS" value="M"   style="width: 40px; margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnTue" runat="server" type="button" class="DowBtnCSS" value="T"   style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnWed" runat="server" type="button" class="DowBtnCSS" value="W"   style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnThu" runat="server" type="button" class="DowBtnCSS" value="T"   style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnFri" runat="server" type="button" class="DowBtnCSS" value="F"   style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnSat" runat="server" type="button" class="DowBtnCSS" value="S"   style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 10px 0 0;"><input id="btnSun" runat="server" type="button" class="DowBtnCSS" value="S"   style="width: 40px;  margin-top: -5px;" /></td>
                            <td><span class="nextarrow"><a href="#"></a></span></td>
                        </tr>
                    </table>
                     
                 </div>


                <div style="clear: both;"></div>
                
                
                <!-- Job Charge DD -->
                <div class="TimeRow" id="JobDiv"  >    
                    <span style="float:left;   width: 50px;  font-weight: bold; display:inline">Job:</span>
                    <input ID="ddJobNo" class="DataInputCss"   /> 
                </div>

                <!-- OverHead Account Time Being Charged To DD -->  
                <div class="TimeRow" id="OhAcctDiv"    >
                    <span style="float:left;   width: 50px;  font-weight: bold; display:inline">O/H:</span>
                    <input ID="ddOhAcct" class="DataInputCss"   /> 
                </div>  
                
<%--                <div style="font-size:x-large; float: left;"  >
                    <span  id="lblErr" Class="errorTextCss"  ></span>
                    <span  id="lblErrServer" Class="errorTextCss"  ></span>
                </div>  --%>
                
                
                <div style="text-align: center; margin-top: 25px; margin-bottom: 25px;">
                    <span  id="lblJobNoSelection"></span>
                    <span  id="lblOhAcctSelection"></span>
                    <span  id="lblJobDescSelection"></span>
                    <span  id="lblJobSiteSelection"></span>

                    
                    <!-- Dept Code Time Being Charged To    --> 
                    <div id="DepDiv" class="TimeRow" style="display: none;">
                        <span  id="lblDeptSelection"></span>
                        <input ID="ddDept" class="DataInputCss" style="width: 60px; float: none; margin-left: 10px;" />
                    </div>                                                          

                    <span  id="lblTaskCodeSelection"></span>
                    <span  id="lblClassTimeSelection"></span>  
                    <span  id="lblClassLocSelection"></span>  
                    <span  id="lblClassInstrSelection"></span>  
                    <span  id="lblClassDescSelection"></span>  
                </div>

                
                <!-- Task Code Time Being Charged To -->  
                <div  id="TaskDiv" class="TimeRow"    style="display: none;">
                    <span style="float:left;   width: 50px;  font-weight: bold; display:inline">Task:</span>
                    <input ID="ddTaskNo" class="DataInputCss"   />
                </div>                              
    
                <!-- Hidden Labels Keeping Selected Charge Account From DD Selections -->
                <section style="visibility: hidden; height: 0; width: 0;">
                    <span  id="hlblJobNoSelection"></span>
                    <span  id="hlblOhAcctSelection"></span>
                    <span  id="hlblDeptSelection" runat="server"></span>
                    <span  id="hlblTaskCodeSelection"></span>                   
                    <span  id="hlblClassTimeSelection"></span>  
                    <span  id="hlblClassDescSelection"></span>  
                    <span  id="hlblClassLocSelection"></span>  
                    <span  id="hlblClassInstrSelection"></span>  
                    <span  id="hlblDailyHours"></span> 
                    <span  id="hlblEID" runat="server"></span> 
                    <span  id="hlblSD" runat="server"></span> 
                    <span  id="hlblED" runat="server"></span> 
                    <span  id="hlblEmpDept" runat="server"></span>
                    <span  id="hlblWeekIdx"></span>
                    <span  id="hldlOhAcct"></span>
                </section>
                
                
                <div style="height: 20px;"></div>

                <!-- Display Labels Showing Selected Charge Account From DD Selections -->
                <div style="font-size:large;"  >
                    <span  id="lblErr" Class="errorTextCss"  ></span>
                    <span  id="lblErrServer" Class="errorTextCss"  ></span>
                    
                </div>               

                <!---------------------------->
                <!-- Details Of Class Taken -->
                <!-- Submit Button          -->
                <!----------------------------->
                <section  class="ui-helper-clearfix" >
                    
                    <!-- CLass Taken Data Capture -->
                    <div id="ClassDataCollectionDiv"  style="width: 260px; float: left; margin-top: 4px; clear: both; display: none;"  >
                        <div style="width: 105%; font-weight: bold; text-align: center; color:dodgerblue; ">Provide Class Detail</div>

                        <div class="TimeRow" id="ClassTimeDiv" style="display: inline-block;" >    
                            <span style="float:left;   width: 110px;  font-weight: bold; display:inline">Start Time:</span>
                            <input id="txtClassTime"  class="DataInputCss" title="Class Start Time"  style="width: 55%; display:inline;"  />
                        </div>
                        
                        <div class="TimeRow" id="ClassDescDiv" style="display: inline-block; height: 80px;">
                            <span style="float:left;   width: 50px;  font-weight: bold; display:inline">Desc:</span>    
                            <textarea  id="txtClassDesc"  Class="DataInputCss" title="Class Description" style="width: 78%;" rows="3" cols="20"  ></textarea>
                        </div>
                        
                        <div class="TimeRow" id="ClassLocDiv" style="display: block;">
                            <span style="float:left;   width: 50px;  font-weight: bold; display:inline">Loc:</span>   
                            <input     id="txtClassLoc"   Class="DataInputCss" title="Class Location"    style="width: 70%;"  />
                        </div>
                        
                        <div class="TimeRow" id="ClassInstrDiv" style="display: block;">
                            <span style="float:left;   width: 50px;  font-weight: bold; display:inline">Instr:</span>   
                            <input     id="TxtClassInstr" Class="DataInputCss" title="Class Instructor"  style="width: 70%;"  />
                        </div>
                        
                        <div class="submitButton">
                            <input type="button" ID="btnClassComplete" value="Class Detail Complete" Class="SearchBtnCSS" />
                        </div>
                    </div>
                    
                </section>
                    
                    

                <div style="margin-bottom: 3px; margin-top: -6px;  width: 100%; text-align: center;">
                         
                    
                    <section    id="TimeCheckBoxes" 
                                class="ui-state-error ui-corner-all" 
                                style="width: 100%; margin-top: 4px; display: none; "   >
                                       

                            <div style="display: inline-block;">
                                <span>Straight</span><br/>
                                <span>Time</span><br/>
                                <input  style="margin-left: 15px;" type="checkbox" id="chkST"  Class="chkbox"  value="ST"/>
                            </div>

                            <div style="margin-left: 20px; display: inline-block;">
                                <span>Over</span><br/>
                                <span>Time</span><br/>
                                <input  style="margin-left: 5px;" type="checkbox" id="chkOT"  Class="chkbox"  value="OT"/>
                            </div>                    

                            <div style="margin-left: 20px; display: inline-block;">
                                <span>Double</span><br/>
                                <span>Time</span><br/>
                                <input  style="margin-left: 10px;" type="checkbox" id="chkDT"  Class="chkbox"  value="DT"/>
                            </div>  
                    
                            <div style="margin-left: 20px; display: inline-block;">
                                <span>Absent</span><br/>
                                <span>Time</span><br/>
                                <input  style="margin-left: 10px;" type="checkbox" id="chkAB"   Class="chkbox" value="AB"/>
                            </div>  
                    
                            <div style="margin-left: 20px; display: inline-block;">
                                <span>Holiday</span><br/>
                                <span>Time</span><br/>
                                <input  style="margin-left: 10px;" type="checkbox" id="chkHT"  Class="chkbox"  value="HT"/>
                            </div>  

                        <div style="clear: both; height: 20px;"></div>
                   
                        <!-- Hours Input TextBox -->
                        <div style=" ;">
                            <span style="margin-right: 15px;">Hours</span>
                            <input id="txtTime" class="DataInputCss" style="width: 60px; float: none;"   />              
                        </div>
                        


                    </section>
                </div>
                
                <%--------------------%>                    
                <%-- Submit Buttons --%>                    
                <%--------------------%>
                <div style="width: 100%;  display: inline-block; margin-top: 25px;">
                    <div style="float: left; width: 30%; ">
                        <input type="button" ID="btnSubmit" value="Submit" Class="SearchBtnCSS" />
                    </div>
                    
                    <div style="float: right; width: 25%; ">
                        <input type="button" ID="btnClear" value="Clear" Class="SearchBtnCSS"   />
                    </div>
                    
                    <div style="margin-left: auto; margin-right: auto; width: 150px; height: 40px; padding: 0; text-align: center; line-height: 14px; vert-align: bottom;">
                        <div style="width: auto; margin-left: auto; margin-right: auto;">
                            <span style="width: 70px; display: inline-block;">Today:</span>
                            <span id="lblHoursThisDay" style="font-size: 1.1em;"></span>

                            <br/>

                            <span style="width: 70px; display: inline-block;">Week:</span>
                            <span id="lblHoursThisWeek" style="font-size: 1.1em;"></span>
                        </div>
                    </div>
                </div>
                    

            </section>
        </div>
    </div> 
    
</asp:Content>

