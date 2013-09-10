<%@ Page Title="" Language="C#" MasterPageFile="/Phone/PhoneMaster.master" AutoEventWireup="true" CodeFile="VehMileage.aspx.cs" Inherits="ELO_VehMileage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Vehicle Mileage</title>
    
    <link href="/Phone/Styles/ELO.css" rel="stylesheet"  type="text/css" />
    <link href="/Phone/Styles/VehMileage.css" rel="stylesheet"  type="text/css" />
    <script type="text/javascript" src="/Scripts/VehicleMileage.js"></script>    
    
    
</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div id="SuprArea" style="margin-top: 5px;" runat="server" >
        <span style="float:left; margin-right: 20px; width: auto;  font-weight: bold; display:inline">Change Employee:</span>
        <input ID="ddEmpIds" class="DataInputCss" runat="server"  style="width: 400px;" />         
    </div>

    <div id="FormWrapper" class="ui-widget ui-form">        
            
        <!----------------->
        <!-- Form Header -->
        <!----------------->                   
        <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
            <a href="/Phone/ELO/MainMenu.aspx" style="text-decoration: none;">
                <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">elo menu</span>
                <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Vehicle Mileage</span>
            </a>
        </section>             
            
            
            
        <!-- Hidden Labels Keeping Selected Charge Account From DD Selections -->
        <section style="visibility: hidden; height: 0; width: 0;">
            <span runat="server" id="hlblEID"></span> 
            <span runat="server" id="hlblPrevWeek"></span> 
            <span runat="server" id="hlblThisWeek"></span> 
            <span runat="server" id="hlblNextWeek"></span> 
        </section>
    

        <section class="ui-widget-content ui-corner-all">

            <!---------------------------->
            <!-- "Requestor ID" Section -->
            <!---------------------------->
            <div style="margin-top: -20px;">
                <label runat="server" ID="lblEmpName" ></label>
                <div ID="imgReport" runat="server"  style="float: right; margin-right: 21px; "  >
                    <a href="/Phone/my/MyVehMileage.aspx" style="color:white; font-weight: bold;">Report</a>
                </div>
            </div>
            <p style="height: 25px;"></p> 
                
                
            <!-- Date For Time Being Entered -->
            <div class="TimeRow">
                    
                <div style="padding: 0; width: 107%;  margin-left: -10px;">
                        
                    <span class="prevarrow"><a href="#"></a></span>
                        
                    <div style="display: inline-block; width: 240px; text-align: center; ">
                        <span style="display: inline-block; line-height: 20px; vertical-align: top; ">Entering Miles For Week Of:</span><br/>
                        <span runat="server" id="txtEntryDate"  class="ThisWeekCss"></span>
                    </div>
                    <span class="nextarrow"><a href="#"></a></span>
                </div>
                    
                </div>
                 
                <div style="display: inline-block; margin-top: 20px; width: 100%; ">
                    <div>
                        <!--------------------->
                        <!-- Vehicle Number  -->
                        <!--------------------->
                    <div class="TimeRow" id="ClassTimeDiv"  style="margin-top: 0;">    
                        <span style="float:left;   width: 130px;  font-weight: bold; display:inline">Vehicle No:</span>
                        <input id="txtVehicleNo"  class="DataInputCss" style="width: 80px; display:inline; margin-top: -3px;" runat="server" />
                    </div>                 
                 

                    <!---------------------->
                    <!--  Vehicle Mileage -->
                    <!---------------------->
                    <div class="TimeRow" id="Div1"  style="margin-top: 0;">    
                        <span style="float:left;   width: 130px;  font-weight: bold; display:inline">Vehicle Miles:</span>
                        <input id="txtVehicleMileage"  class="DataInputCss" style="width: 80px; display:inline; margin-top: -3px;" runat="server" />
                    </div>                          
                </div>
                    
                <!------------------------------------->
                <!-- Assigned Vehicle List Shortcuts -->
                <!------------------------------------->
                <div>
                    <span id="AssignedVehicleList"  runat="server">
                    </span>
                </div>
                </div>

            <!-- Display Labels Showing Selected Charge Account From DD Selections -->
            <div style="color: white; font-size: smaller;">
                <span runat="server" id="lblErr" Class="errorTextCss"  ></span>
                <span runat="server" id="lblErrServer" Class="errorTextCss"  ></span>
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
                    
                <div style="float: Left; width: 35%; text-align: center; margin-left: 5%; ">
                    <input type="button" ID="btnRemove" value="Remove" Class="SearchBtnCSS"  autofocus="False" />
                </div>
            </div>

        </section>  
    </div>
</asp:Content>

