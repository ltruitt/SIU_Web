<%@ Page Title="" Language="C#" MasterPageFile="/Phone/PhoneMaster.master" AutoEventWireup="true" CodeFile="TimeEntry.aspx.cs" Inherits="ELO_TimeEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Time Card</title>
    
    <link href="/Phone/Styles/ELO.css" rel="stylesheet"  type="text/css" />
    <link href="/Phone/Styles/TimeEntry.css" rel="stylesheet"  type="text/css" />
    <script type="text/javascript" src="/Scripts/TimeEntry.js"></script>     
    
</asp:Content>






<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

        <div id="FormWrapper" class="ui-widget ui-form">        
            <!----------------->
            <!-- Form Header -->
            <!----------------->                   
            <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
                <a href="/Phone/ELO/MainMenu.aspx" style="text-decoration: none;">
                    <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">elo menu</span>
                    <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Time Card</span>
                </a>
            </section>  
    
            <section class="ui-widget-content ui-corner-all">

                <!---------------------------->
                <!-- "Requestor ID" Section -->
                <!---------------------------->
                <div style="float: left; margin-left: -15px;  width: 115%;">
                    
                    <div style="margin-top: -20px;">
                        <label runat="server" ID="lblEmpName" ></label>
                        <span ID="imgRejects" runat="server" class="VHcss" style="float: none; margin-left: 7px; background-color: red;"  >
                            <a href="/ELO/TimeRpt.aspx" style="color:white; font-weight: bold;">Rejected</a>
                        </span>
                        <span ID="imgReport" runat="server"  style="float: right; margin-right: 21px; "  >
                            <a href="/Phone/ELO/TimeRpt.aspx" style="color:white; font-weight: bold;">Report</a>
                        </span>
                    </div>
                    <div style="height: 45px;"></div>
                    

                    <span class="VHcss" style="margin-left: -5px;"  >Vac:
                        <span style="float: right; margin-left: 7px; "  runat="server" ID="lblVac" ></span>
                    </span>

                    <span class="VHcss" style="margin-left: -5px;"  >Per:
                        <span style="float: right; margin-left: 7px; "  runat="server" ID="lblHol" ></span>
                    </span>
                    
                    <span class="VHcss" style="margin-left: -5px;  display: none;">Sick:
                        <span style="float: right; margin-left: 7px;"  runat="server" ID="lblSick" ></span>
                    </span>
                </div>


                <p style="height: 10px;"></p> 


                <!-- Date For Time Being Entered -->
		            

                <div class="TimeRow">
                    
                    <div style="padding: 0; width: 107%;  margin-left: -10px;">
                        <span style="display: inline-block; margin-left: 25px; margin-right: 10px; line-height: 20px; vertical-align: top;">Time For:</span>

                        <div style="display: inline-block;">
                            <input type="text" id="txtEntryDate" Class="DataInputCss DateEntryCss"  runat="server" style="width: 100px;" />   
                        </div>
                    </div>
                 </div>

                 
                 
                 <!-- Show Days For This Week Where Time Enetered -->
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
                
                
                <!-- Job Charge DD -->
                <div class="TimeRow" id="JobDiv"  runat="server">    
                    <span style="float:left;   width: 50px;  font-weight: bold; display:inline">Job:</span>
                    <input ID="ddJobNo" class="DataInputCss" /> 
                </div>

                <!-- OverHead Account Time Being Charged To DD -->  
                <div class="TimeRow" id="OhAcctDiv"    runat="server">
                    <span style="float:left;   width: 50px;  font-weight: bold; display:inline">O/H:</span>
                    <input ID="ddOhAcct" class="DataInputCss" /> 
                </div>  
                

                
                <!-- Task Code Time Being Charged To -->  
                <div  id="TaskDiv" class="TimeRow"   runat="server" style="display: none;">
                    <span style="float:left;   width: 50px;  font-weight: bold; display:inline">Task:</span>
                    <input ID="ddTaskNo" class="DataInputCss" runat="server"  />
                </div>                              
    
                <!-- Hidden Labels Keeping Selected Charge Account From DD Selections -->
                <section style="visibility: hidden; height: 0; width: 0;">
                    <span id="hlblJobNoSelection"></span>
                    <span id="hlblOhAcctSelection"></span>
                    <span id="hlblDeptSelection" runat="server"></span>
                    <span id="hlblTaskCodeSelection"></span>                   
                    <span id="hlblClassTimeSelection"></span>  
                    <span id="hlblClassDescSelection"></span>  
                    <span id="hlblClassLocSelection"></span>  
                    <span id="hlblClassInstrSelection"></span>  
                    <span id="hlblDailyHours"></span> 

                    <span id="hlblEmpDept" runat="server"></span>
                    <span id="hlblWeekIdx"></span>
                    <span id="hldlOhAcct"></span>
                    <span id="hlblEID" runat="server"></span> 
                    <span id="hlblSD" runat="server"></span> 
                    <span id="hlblEndD" runat="server"></span> 
                </section>
                

                <!-- Display Labels Showing Selected Charge Account From DD Selections -->
                <div style="color: white; font-size: smaller;">
                    <span runat="server" id="lblJobNoSelection"></span>
                    <span runat="server" id="lblOhAcctSelection"></span>
                    <span runat="server" id="lblJobDescSelection"></span>
                    <span runat="server" id="lblJobSiteSelection"></span>

                    
                    <!-- Dept Code Time Being Charged To    --> 
                    <div id="DepDiv" class="TimeRow"  style="display: none;">
                        <span runat="server" id="lblDeptSelection"></span>
                        <input ID="ddDept" class="DataInputCss" runat="server"  style="width: 60px; float: none; margin-left: 10px;" />
                    </div>                                                          

                    <span runat="server" id="lblTaskCodeSelection"></span>
                    <span runat="server" id="lblClassTimeSelection"></span>  
                    <span runat="server" id="lblClassLocSelection"></span>  
                    <span runat="server" id="lblClassInstrSelection"></span>  
                    <span runat="server" id="lblClassDescSelection"></span>  
                    <div></div>
                    <span runat="server" id="lblErr" Class="errorTextCss"  ></span>
                    <span runat="server" id="lblErrServer" Class="errorTextCss"  ></span>
                </div>               

                <!---------------------------->
                <!-- Details Of Class Taken -->
                <!-- Submit Button          -->
                <!----------------------------->
                <section  class="ui-helper-clearfix" >
                    
                    <!-- CLass Taken Data Capture -->
                    <div id="ClassDataCollectionDiv"  style="width: 260px; float: left; margin-top: 4px; clear: both; display: none;" runat="server" >
                        <div style="width: 105%; font-weight: bold; text-align: center; color:dodgerblue; ">Provide Class Detail</div>

                        <div class="TimeRow" id="ClassTimeDiv" style="display: inline-block;" >    
                            <span style="float:left;   width: 110px;  font-weight: bold; display:inline">Start Time:</span>
                            <input id="txtClassTime"  class="DataInputCss" title="Class Start Time"  style="width: 55%; display:inline;" runat="server" />
                        </div>
                        
                        <div class="TimeRow" id="ClassDescDiv" style="display: inline-block; height: 80px;">
                            <span style="float:left;   width: 50px;  font-weight: bold; display:inline">Desc:</span>    
                            <textarea  id="txtClassDesc"  Class="DataInputCss" title="Class Description" style="width: 78%;" rows="3" cols="20" runat="server" ></textarea>
                        </div>
                        
                        <div class="TimeRow" id="ClassLocDiv" style="display: block;">
                            <span style="float:left;   width: 50px;  font-weight: bold; display:inline">Loc:</span>   
                            <input     id="txtClassLoc"   Class="DataInputCss" title="Class Location"    style="width: 70%;" runat="server" />
                        </div>
                        
                        <div class="TimeRow" id="ClassInstrDiv" style="display: block;">
                            <span style="float:left;   width: 50px;  font-weight: bold; display:inline">Instr:</span>   
                            <input     id="TxtClassInstr" Class="DataInputCss" title="Class Instructor"  style="width: 70%;" runat="server" />
                        </div>
                        
                        <div class="submitButton">
                            <input type="button" ID="btnClassComplete" value="Class Detail Complete" Class="SearchBtnCSS" />
                        </div>
                    </div>
                    
                </section>
                    
                    



                <section id="TimeCheckBoxes" class="ui-state-error ui-corner-all" style="width: 260px; float: left; margin-top: 4px; clear: both; display: none;" runat="server"  >
                    <div>
                    <div style="float: left;">
                        <span>ST</span><br/>
                        <input type="checkbox" id="chkST" runat="server" Class="chkbox"  value="ST"/>
                    </div>

                    <div style="float: left; margin-left: 15px;">
                        <span>OT</span><br/>
                        <input type="checkbox" id="chkOT" Class="chkbox"  value="OT"/>
                    </div>                    

                    <div style="float: left; margin-left: 15px;">
                        <span>DT</span><br/>
                        <input type="checkbox" id="chkDT" Class="chkbox"  value="DT"/>
                    </div>  
                    
                    <div style="float: left; margin-left: 15px;">
                        <span>Abs</span><br/>
                        <div style=" display: inline-block; ">
                            <input type="checkbox" id="chkAB"  Class="chkbox" value="AB"/>
                        </div>
                    </div>  
                    
                    <div style="float: left; margin-left: 15px;">
                        <span>Hol</span><br/>
                        <input type="checkbox" id="chkHT" runat="server" Class="chkbox"  value="HT"/>
                    </div>  
                    </div>
                   
                    <%--Hours Input TextBox--%>
                    <div style="clear: both;  float: left;">
                        <span style="float: left; margin-right: 15px;">Hours</span>
                        <input id="txtTime" class="DataInputCss" style="width: 60px;" />              
                    </div>
                        


                </section>
                
                <%--------------------%>                    
                <%-- Submit Buttons --%>                    
                <%--------------------%>
                <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                    <div style="float: left; width: 30%; ">
                        <input type="button" ID="btnSubmit" value="Submit" Class="SearchBtnCSS" />
                    </div>
                    
                    <div style="float: left;  width: 35%; margin: 0; padding: 0; text-align: center; line-height: 14px;">
                        <span style="font-size: .9em;">Today:</span>
                        <span id="lblHoursThisDay" style="font-size: 1.1em; float: right;"></span>
                        <br/>
                        <span style="font-size: .9em;">Week:</span>
                        <span id="lblHoursThisWeek" style="font-size: 1.1em; float: right;"></span>
                    </div>
                    

                       
                       
                                        
                    <div style="float: right; width: 25%; ">
                        <input type="button" ID="btnClear" value="Clear" Class="SearchBtnCSS"  autofocus="False" />
                    </div>
                </div>
                    

            </section>
        </div>

</asp:Content>

