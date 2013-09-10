<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="VehDotNonCorrected.aspx.cs" Inherits="ELO_VehDotNonCorrected" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>D. O. T. Uncorrected Hazards</title>
    
    <script type="text/javascript" src="/Scripts/VehDotNonCorrected.js"></script> 
    

    <!-- Include one of jTable styles. -->
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
 
    <!-- Include jTable script file. -->
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script> 
    
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />
</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div style="background-color: #45473f; ">
        <div id="FormWrapper" class="ui-widget ui-form">
        
            <section style="visibility: hidden; height: 0; width: 0;">
                <span id="hlblEID"           runat="server"/>  
                <span id="lblRefID"          runat="server"/> 
            </section>            
            
            <!----------------->
            <!-- Form Header -->
            <!----------------->                   
            <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
                <a href="/ELO/MainMenu.aspx" style="text-decoration: none;">
                    <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">elo menu</span>
                    <span  style="text-align: center; font-size: 1.7em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Vehicle Inspections</span>
                </a>
            </section>
            
    
            <section class="ui-widget-content ui-corner-all">

                <!---------------------------->
                <!-- "Viewer ID" Section -->
                <!---------------------------->
                <div style="float: left; margin-top: -20px; width: 100%;">
                    <label style="float: left; padding: 0; width:auto; "  runat="server" ID="lblEmpName"></label><br/>
                    <span ID="imgReport" runat="server"  style="float: right; margin-left: 7px; "  >
                        <a href="/ELO/VehDotEntry.aspx" style="color:white; font-weight: bold;">New Inspection</a>
                    </span> 
                </div>

                <p style="height: 5px;"></p> 
                                
                <div style="color: white; font-size: smaller;  display: none;"  id="DotRptDtlDiv">                   
                    <span style="width: 80px; font-weight: bold; display:inline-block;">Vehicle:</span>
                    <span id="lblVehicle" runat="server" />
                    
                    <span style="width: 15px; font-weight: bold; display:inline-block;"></span>
                    <span id="lblPlate" runat="server" /><br/>
                    
                    <span style="width: 80px; font-weight: bold; display:inline-block;"></span>
                    <span id="lblMake" runat="server" />
                    <span id="lblModel" runat="server" /><br/>
                    
                                        
                    <span style="width: 80px; font-weight: bold; display:inline-block;">Date:</span>
                    <span id="lblSubmitTimeStamp" runat="server" /><br/>
                    
                    <span style="width: 80px; font-weight: bold; display:inline-block;">By:</span>
                    <span id="lblSubmitEmpID" runat="server" /><br/>
                    
                    <span style="width: 80px; font-weight: bold; display:inline-block;">Hazard:</span>
                    <span id="lblHazard"  runat="server" /><br/>

                    <span style="width: 100%; font-weight: bold; margin-top: 10px; display:inline-block;">Corrective Action:</span>
                    <span id="lblCorrection"  runat="server" /><br/>
                    <div id="uncorrectedDiv" style="display: none;">
                    
                    <div class="TimeRow" id="ClassDescDiv" style=" height: 79px;">
                        <textarea  id="txtCorrection"  Class="DataInputCss" title="Corrective Action" style="width: 100%;" rows="3" cols="20" runat="server" ></textarea>
                    </div>
                        
                    <%--------------------%>                    
                    <%-- Submit Buttons --%>                    
                    <%--------------------%>
                    <div style="width: 100%;  display: inline-block; margin-top: 5px;">
                        <div style="float: left; width: 30%; ">
                            <input type="button" ID="btnSubmit" value="Submit" Class="SearchBtnCSS" />
                        </div>
                    </div>
</div>
                </div> 

                 <div id="jTableContainer"></div>

             </section> 
             
        </div>

    </div>    
    
</asp:Content>

