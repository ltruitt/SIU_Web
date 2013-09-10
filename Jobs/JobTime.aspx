<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="JobTime.aspx.cs" Inherits="Jobs_JobTime" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Customer Job Cost Reporting</title>

    <!-- jqGrid Reference -->
    <link rel="stylesheet" type="text/css" media="screen" href="/jqGrid-4.4.4/css/ui.jqgrid.css" />
    <script src="/jqGrid-4.4.4/js/i18n/grid.locale-en.js" type="text/javascript"></script>
    <script type="text/javascript">jQuery.jgrid.no_legacy_api = true;</script>
    <script src="/jqGrid-4.4.4/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    

    <script type="text/javascript" src="/Scripts/JobTime.js"></script> 

    <style>     
        .jTableTD
        {
            text-align: center;
            
        }   
        
        .jTableTD_ChkBox {
            text-align: center;
            font-size: 2em;
        }     
    </style>     
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <!-- Hidden Labels Keeping Selected Charge Account From DD Selections -->
    <section style="visibility: hidden; height: 0; width: 0;">
        <span runat="server" id="hlblJobNoSelection"        />
        <span runat="server" id="hlblEID"                   /> 
                        
    </section>

    <div style="background-color: #45473f; ">
        <div id="FormWrapper" class="ui-widget ui-form">
        
            
            
            <!----------------->
            <!-- Form Header -->
            <!----------------->                   
            <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
                <%--<a href="/ELO/MainMenu.aspx" style="text-decoration: none;">--%>
                    <%--<span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">elo menu</span>--%>
                    <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Job Time Sheet</span>
                <%--</a>--%>
            </section>  
            
            <section class="ui-widget-content ui-corner-all">

                <!---------------------------->
                <!-- "Requestor ID" Section -->
                <!---------------------------->
                <div style="float: left; margin-left: -15px;  width: 115%;">
                    
                    <div style="margin-top: -20px;">
                        <label style="float: none; padding: 0; width:auto; margin-top: -20px;"  runat="server" ID="lblEmpName"></label>
                    </div>
                    <div style="height: 20px;"></div>
                    


                </div>
                
                <p style="height: 5px;"></p> 
                

                <!-- Job Charge AutoComplete-->
                <div class="TimeRow" id="JobDiv"  runat="server">    
                    <span style="float:left;   width: 50px;  font-weight: bold; display:inline">Job:</span>
                    <input ID="acJobNo" class="DataInputCss" runat="server"  /> 
                </div>
                
                
                <!-- Display Labels Showing Selected Charge Account From DD Selections -->
                <div style="color: white; font-size: smaller; ">
                    <span runat="server" id="lblJobNoSelection"  />
                    <span runat="server" id="lblJobDescSelection"  />
                    <span runat="server" id="lblJobSiteSelection" />

                    <span runat="server" id="lblJobContactSelection"   />
                    <span runat="server" id="lblJobEmailSelection"   />  

                    <span runat="server" id="lblErr" Class="errorTextCss"  ></span>
                    <span runat="server" id="lblErrServer" Class="errorTextCss"  ></span>
                </div>                   
                
                <section id="AddInfoSec" class="ui-state-error ui-corner-all" style="height: 12px; width: 260px; float: left; margin-top: 12px; padding-top: 3px; clear: both;" runat="server"  >
                    <span style="float:left; width: auto;  margin-top: -6px; font-weight: bold; display:inline">Additional Info</span>
                    <img id="AddInfoExpand" src="/Images/Expand.png" style="float: right; margin-top: -3px;" alt="Expand"/>
                    <img id="AddInfoCollapse" src="/Images/Collapse.png" style="float: right; margin-top: -3px; display: none;" alt="Collapse"/>
                    <table id="AddInfo"></table>
                </section>
                
                <section id="AddDescSec" class="ui-state-error ui-corner-all" style="height: 12px; width: 260px; float: left; margin-top: 4px; padding-top: 3px; clear: both;" runat="server"  >
                    <span style="float:left; width: auto;  margin-top: -6px; font-weight: bold; display:inline">Work Descriptions</span>
                    <img id="AddDescExpand" src="/Images/Expand.png" style="float: right; margin-top: -3px;" alt="Expand"/>
                    <img id="AddDescCollapse" src="/Images/Collapse.png" style="float: right; margin-top: -3px; display: none;" alt="Collapse"/>
                    <table id="AddDescTbl"></table>
                </section>
                
                <section id="JobTimeSec" class="ui-state-error ui-corner-all" style="height: 12px; width: 260px; float: left; margin-top: 4px; padding-top: 3px; clear: both;" runat="server"  >
                    <span style="float:left; width: auto;  margin-top: -6px; font-weight: bold; display:inline">Time</span>
                    <img id="JobTimeExpand" src="/Images/Expand.png" style="float: right; margin-top: -3px;" alt="Expand"/>
                    <img id="JobTimeCollapse" src="/Images/Collapse.png" style="float: right; margin-top: -3px; display: none;" alt="Collapse"/>
                    <table id=""></table>
                </section>
                
                
                <section id="JobExpSec" class="ui-state-error ui-corner-all" style="height: 12px; width: 260px; float: left; margin-top: 4px; padding-top: 3px; clear: both;" runat="server"  >
                    <span style="float:left; width: auto;  margin-top: -6px; font-weight: bold; display:inline">Expenses</span>
                    <img id="JobExpExpand" src="/Images/Expand.png" style="float: right; margin-top: -3px;" alt="Expand"/>
                    <img id="JobExpCollapse" src="/Images/Collapse.png" style="float: right; margin-top: -3px; display: none;" alt="Collapse"/>
                    <table id="JobExp"></table>
                </section>                
                
                
                <%-------------------%>
                <%-- Report Button --%>
                <%-------------------%>
                <div style="width: 100%;  display: inline-block; margin-top: 10px;">
                    <div style="float: left; width: 30%; ">
                        <input type="button" ID="btnReport" value="View Report" Class="SearchBtnCSS" />
                    </div>
                </div>                
                
                

            </section>            
        </div>
        
    </div>  
    
    

</asp:Content>

