﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="MealsExpEntry2.aspx.cs" Inherits="ELO_MealsExpEntry" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Meals and Expenses Entry</title>
    
    <%--ELO Look and Feel--%>
    <link href="/Styles/ELO.css" rel="stylesheet"  type="text/css" />
    
    <%--Meals And Expenses Specific Look and Feel--%>
    <link href="/Styles/MealsExpEntry.css" rel="stylesheet"  type="text/css" />
    
    <%--Import JQuery Theme--%>
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
    
    <%--Jquery Based CRUD Table--%>
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script> 
     
    <%--Form Management Code--%>
    <script type="text/javascript" src="/Scripts/MealsExpEntry2.js?0000"></script> 
    
    <%--Code To Catch Changes To Text Boxes--%>    
    <%--<script type="text/javascript" src="/Scripts/jquery.textchange.js"></script>--%>
    
    <%--Desktop Look And Feel Base--%>
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />  
    
    <%--File Upload Code     --%>
    <script src="/Scripts/EloUpload.js"></script>
    
    <%--Numeric Formating Validation--%>
    <script src="/Scripts/InputTypes/autoNumeric/autoNumeric.js" type=text/javascript> </script>
    
    <style type="text/css">
        .JobAndOh { }
        .MilesAndMeals { }
    </style>

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
                    <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Expense Card</span>
                </a>
            </section>
    
            <section class="ui-widget-content ui-corner-all">

                <!---------------------------->
                <!-- "Requestor ID" Section -->
                <!---------------------------->
                <div style="float: left; ">
                    <label style="float: left; padding: 0; width:auto; margin-top: -20px;"  runat="server" ID="lblEmpName"></label><br/>
                </div>


                <p style="height: 5px;"></p> 
                

                <!-- Display Open Expenses -->
                <div id="jTableContainer"></div>

                



                <!-- Date For Time Being Entered -->
                <div class="TimeRow" id="DateDiv"  runat="server" style="margin-top: 20px;">    
                    <span style="float:left;   width: 120px;  font-weight: bold; display:inline">Work Date:</span>
                    <input type="text" id="txtWorkDate" Class="DataInputCss DateEntryCss"  runat="server" style="width: 100px;" /> 
                </div>                 
                 

                
                
                <!-- Job Charge DD -->
                <div class="TimeRow JobAndOh" id="JobDiv"  style="margin-top: 20px;">    
                    <span style="float:left;   width: 50px;  font-weight: bold; display:inline">Job:</span>
                    <input ID="ddJobNo" class="DataInputCss" style="width: 200px;"  /> 
                </div>

                <!-- OverHead Account Time Being Charged To DD -->  
                <div class="TimeRow JobAndOh" id="OhAcctDiv" >
                    <span style="float:left;   width: 50px;  font-weight: bold; display:inline">O/H:</span>
                    <input ID="ddOhAcct" class="DataInputCss"  style="width: 200px;" /> 
                </div>  
                
                <!--  -->  
                <div class="TimeRow MilesAndMeals" id="MilesDiv"  style="margin-top: 20px;">
                    <span style="float:left;   width: 157px;  font-weight: bold; display:inline-block; margin-top: 2px; text-align: right;  padding-right: 20px;">Reimbursment Miles:</span>
                    <input ID="txtMiles" class="DataInputCss" style="width: 100px; display: inline-block; float: left;"/> 
                </div>  
                <div style="height: 12px;"></div>
                       
                <!--  -->  
                <div class="TimeRow MilesAndMeals" id="MealsDiv" >
                    <span style="float:left;   width: 157px;  font-weight: bold; display:inline-block; margin-top: 2px; text-align: right;  padding-right: 20px;">Reimbursment Meals:</span>
                    <select ID="txtMeals"  class="DataInputCss" style="width: 100px; "> 
                        <option selected="selected" value="0">Pick  One</option>
                    </select>
                </div>  
                
                <!-- Starting Date For 4291 OH Account -->
                <div class="TimeRow" id="Div4291"  style="margin-top: 20px;">    
                    <span style="float:left;   margin-right: 20px;  font-weight: bold; display:inline">First of 5 Consequtive Workdays:</span>
                    <input type="text" id="txt4291Day" Class="DataInputCss DateEntryCss"  runat="server" style="width: 100px;" /> 
                </div>  
                
                                 
                
                <div id="FileUploadDiv">
                    <span id="UploadHead" style="width: 100%;  font-weight: bold; display:inline-block; margin-top: 2px;  padding-right: 20px;">Upload Image of Receipt:</span>
                    <input class="DataInputCss"  style="width: 500px;" id="txtFile" name="txtFile" type="file" accept="image/*" capture="camera" onchange="fileSelected();"> 
                    <span id="lblFile" style="width: 100%;"></span>         
                    
                    <div id="UploadStats">
                        <div id="progress_info">
                            <span id="progress"></span>
                            <span id="progress_percent"></span>
                            <div class="clear_both"></div>
                            <div>
                                <span id="speed"></span>
                                <span id="remaining"></span>
                                <span id="b_transfered"></span>
                                <span class="clear_both"></span>
                            </div>
                            <div id="upload_response"></div>
                        </div>
                        <div id="error">Unsupported files type!</div>
                        <div id="error2">An error occurred while uploading the file</div>
                        <div id="abort">The upload has been canceled by the user or the browser dropped the connection</div>
                        <div id="warnsize">Your file is very big. We can't accept it. Please select a smaller file</div>
                    </div>
                    <p></p>                                                 
                </div>
                
                <div style="clear: both; margin-bottom: 5px;"></div>

                

                <!-- Display Labels Showing Selected Charge Account From DD Selections -->
                <div style="color: white; font-size: smaller;">
                    <span id="lblDate"></span>
                    <span id="lblJobNo"></span>
                    <span id="lblOhAcct"></span>
                    <span id="lblJobDesc"></span>
                    <span id="lblJobSite"></span>
                    <span id="lblMiles"></span>
                    <span id="lblMeals"></span>
                                   
                    
                    <br/>

                    <!-- Dept Code Time Being Charged To    --> 
                    <div id="DepDiv" class="TimeRow"   runat="server">
                        <span runat="server" id="lblAmount"   />
                        <input ID="ovrAmount" class="DataInputCss" runat="server"  style="width: 60px; float: none; margin-left: 10px;" />
                    </div>   
                                        



                    
                                         

                    <span runat="server" id="lblErr" Class="errorTextCss"  ></span>
                    <span runat="server" id="lblErrServer" Class="errorTextCss"  ></span>
                </div>               

                <!--  -->  
                <div class="TimeRow MilesAndMeals" id="ExpAmountDiv" >
                    <span style="float:left;   width: 157px;  font-weight: bold; display:inline-block; margin-top: 2px; text-align: right;  padding-right: 20px;">Expense Amount:</span>
                    <input id="txtExpAmount"   type="text" class="DataInputCss" style="width: 100px; display: inline-block; float: left;"/> 
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
            
                <!-- Hidden Labels Keeping Selected Charge Account From DD Selections -->
                <section style="visibility: hidden; height: 0; width: 0;">
                   job <span runat="server" id="hlblJobNo"            />
                   oh <span runat="server" id="hlblOhAcct"            />
                   miles <span runat="server" id="hlblMiles"          />
                   meals <span runat="server" id="hlblMeals"          />
                   Idx <span runat="server" id="hlblMealsIdx"         />
                   amount <span runat="server" id="hlblAmount"        />
                   eid <span runat="server" id="hlblEID"              />
                   sd <span runat="server" id="hlblSD"                />
                   ed <span runat="server" id="hlblED"                />
                   rate <span runat="server" id="hlblMileRate"        />
                    
                </section>
        </div>
    </div> 
    
</asp:Content>



