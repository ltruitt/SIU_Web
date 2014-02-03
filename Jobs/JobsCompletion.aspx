<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="JobsCompletion.aspx.cs" Inherits="Forms_Jobs" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Field Job Completion Form</title>
    
    <link rel="stylesheet" href="/styles/Jobs.css">
    <script type="text/javascript" src="/Scripts/Jobs.js"></script>
    
    <script src="/Scripts/InputTypes/ToggleSwitch/ToggleSwitch.js" type="text/javascript"></script>
    <link rel="stylesheet" href="/Scripts/InputTypes/ToggleSwitch/ToggleSwitch.css">
    
    <script src="/Scripts/InputTypes/autoNumeric/autoNumeric.js" type=text/javascript> </script>
  

    
    <style>
        span[id^='SwitcherLabel'] {
             color: white !important;
             font-weight: bold !important;
             font-size: 1.5em !important;
             background-image: none !important;
             border: none !important;
             width: 105% !important;
             margin-right: 10px !important;
             -webkit-box-shadow: none !important;
             -ms-box-shadow: none !important;
             box-shadow: none !important;
         }
    </style>
<style>

    </style> 
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        
 <div style="background-color: #45473f;">
   
    <div id="FormWrapper" class="ui-widget ui-form">
        

            <!----------------->
            <!-- Form Header -->
            <!----------------->                   
            <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
                <a href="/ELO/MainMenu.aspx" style="text-decoration: none;">
                    <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">elo menu</span>
                    <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Job Completion Record</span>
                </a>
            </section>        
        
            <section class="ui-widget-content ui-corner-all">
                
                <!------------------------->
                <!-- "Reporting Employee -->
                <!------------------------->
                <span style="float: left; ">
                    <label style="float: left; padding: 0; width:auto; "  runat="server" ID="lblEmpName"></label>
                </span>      
        
                <p style="height: 10px;"></p>

                <!-- Job Charge DD -->
                <div class="TimeRow JobAndOh" id="JobDiv"  style="margin-top: 20px;">    
                    <span style="float:left; margin-right: 15px; font-weight: bold; display:inline">Job No:</span>
                    <input ID="ddJobNo" class="DataInputCss" style="width: 300px;"  /> 
                    <img id="ddJobNoWait" src="/Images/Loading2.gif"  style="height: 21px;"/>
                </div>        
                
                <div style="text-align: center; margin-top: 25px; clear: both;">
                    <span  id="lblJobNoSelection"></span>
                    <span  id="lblJobSiteSelection"></span>
                    <span  id="lblJobDescSelection"></span>
                    <span  id="lblDeptSelection"></span>
                </div>
        
                <div style="clear: both; height: 15px;"></div>
                
                <div ID="basicBillingDiv" style="display: none; margin-left: auto; margin-right: auto; width: 250px; " >
                    <div id="SwitcherBilling"></div>
                </div>
                
                <div style="clear: both; height: 10px;"></div>

                <div ID="DetailBillingDiv" style="display: none; clear: both;"  >
                    <h3 style="text-align: center; color: crimson;">Cost Above Origional Agreement</h3>
                    <div>
                        <div style="float: left; width: 53%;">
                            <span style="margin-left: 63px;">No Managers</span>
                            <input  id="txtNumMgrs" type="text" class="DataInputCss" style="text-align: center; height: 20px; width: 40px;"/> 
                        </div>
                        
                        <div style="float: right; width: 47%;">
                            <span style="margin-left: 5px;">Total Hours</span>
                            <input  id="txtTotHours" type="text" class="DataInputCss" style="text-align: center; height: 20px; width: 100px;"/> 
                        </div>
                    </div>
                    
                    <div style="clear: both; height: 4px;"></div>
                    
                    <div>
                        <div style="float: left; width: 53%;">
                            <span style="margin-left: 5px;">Material Costs</span>
                            <input  id="txtAddMaterial" type="text" class="DataInputCss" style="text-align: center; height: 20px; width: 100px;"/> 
                        </div>
                    
                        <div style="float: right; width: 47%;">
                            <span style="margin-left: 5px;">Travel Costs</span>
                            <input  id="txtAddTravel" type="text" class="DataInputCss" style="text-align: center; height: 20px; width: 100px;"/> 
                        </div>
                    </div>
                    
                    <div style="clear: both; height: 4px;"></div>
                          
                    <div>
                        <div style="float: left; width: 53%;">
                            <span style="margin-left: 5px;">Lodgeing Costs</span>
                            <input  id="txtAddLodge" type="text" class="DataInputCss" style="text-align: center; height: 20px; width: 100px;"/> 
                        </div>

                        <div style="float: right; width: 47%;">
                            <span style="margin-left: 5px;">Other Costs</span>
                            <input  id="txtAddOther" type="text" class="DataInputCss" style="text-align: center; height: 20px; width: 100px;"/> 
                        </div>
                    </div>
                    
                    <div style="clear: both; height: 20px;"></div>

                    <div style="margin-bottom: 7px;">
                        <div>
                            <div>Customer Approval Name: </div>
                            <input  id="AppName" type="text" class="DataInputCss" style="text-align: center; height: 20px; width: 370px;"/> 
                        </div>
                    </div> 
                    
                    <div id="SwitcherSales"></div>                   

                </div>
                
                <div style="clear: both; height: 15px;"></div>
                

                

                <%--------------------%>                    
                <%-- Submit Buttons --%>                    
                <%--------------------%>
                <div style="width: 100%;  display: inline-block;">
                    <div style="float: left; width: 30%; ">
                        <input type="button" ID="btnSubmit" value="Submit" Class="SearchBtnCSS" />
                    </div>
                    
                    <div style="float: right;  text-align: right;">
                        <input type="button" ID="btnClear" value="Clear" Class="SearchBtnCSS"   />
                    </div>
                    
                </div>

           </section>
        
            
        
        </div>
        
    </div>
</asp:Content>





