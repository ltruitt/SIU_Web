<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="MyJobRpts.aspx.cs" Inherits="Reporting_SubmitRptDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Open Job Reports</title>
      
    <!-- Include one of jTable styles. -->
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
 
    <!-- Include jTable script file. -->
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script> 
    
    <script type="text/javascript" src="/Scripts/MyJobRpts.js"></script>

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

    <div id="SuprArea" style="margin-top: 5px;" runat="server" >
        <span style="float:left; margin-right: 20px; width: auto;  font-weight: bold; display:inline">Change Employee:</span>
        <input ID="ddEmpIds" class="DataInputCss" runat="server"  style="width: 400px;" />         
    </div>

    <div style="background-color: black;  ">
        <div id="FormWrapper" class="ui-widget ui-form">
        
            <section style="visibility: hidden; height: 0; width: 0;">
                <span id="hlblEID"           runat="server"/>      
            </section>            
            

            <section class="ui-widget-header ui-corner-all"  >
                <div  style="text-align: center; font-size: 2em; color: white; margin-bottom: 5px;">Open Job Reports</div>               
               
            
    
                <section class="ui-widget-content ui-corner-all">

                    <!---------------------------->
                    <!-- "Viewer ID" Section -->
                    <!---------------------------->
                    <div style="float: left; ">
                        <label style="float: left; padding: 0; width:auto; margin-top: -20px;"  runat="server" ID="lblEmpName"></label><br/>
                    </div>

                    <p style="height: 5px;"></p> 
                                 
                     <div id="jTableContainer"></div>


                     <section  class="ui-helper-clearfix" >
                    
                         <!-- CLass Taken Data Capture -->
                         <div id="DetailDiv"  style="width: 260px; float: left; margin-top: 8px; clear: both;" runat="server" >
                             <div style="width: 105%; font-weight: bold; text-align: center; color:dodgerblue; ">Job Report Detail</div>
                        
                       
                             <div style="color: white; font-size: smaller;">
                                 
                                 <span style="width: 125px; display: inline-block;">Job Number: </span>
                                 <span runat="server" id="lblJobNo"  /><br/>

                                 <span style="width: 125px; display: inline-block;">Req. Sales: </span>
                                 <span runat="server" id="lblReqSales"  /><br/>

                                 <span style="width: 125px; display: inline-block;">Follow Up: </span>
                                 <span runat="server" id="lblSalesDate"  /><br/><br/>
                                 
                                 <span style="width: 125px; display: inline-block;">Submit: </span>
                                 <span runat="server" id="lblSubmitDate"  /><br/>

                                 <span style="width: 125px; display: inline-block;">JHA Submit: </span>
                                 <span runat="server" id="lblJhaDate"  /><br/>
                                 
                                 <span style="width: 125px; display: inline-block;">IR Submit: </span>
                                 <span runat="server" id="lblIrSubmitDate"  /><br/>
                                                                                             
                                 <span style="width: 125px; display: inline-block;">IR Complete: </span>
                                 <span runat="server" id="lblIrCompleteDate"  /><br/>                                                                                             
                                                                                                                                                               
                                 <span style="width: 125px; display: inline-block;">Cost Date: </span>
                                 <span runat="server" id="lblCost"  /><br/>

                                 <span style="width: 125px; display: inline-block;">Logged In: </span>
                                 <span runat="server" id="lblLoginDate"  /><br/>

                                 <span style="width: 125px; display: inline-block;">Data Entry: </span>
                                 <span runat="server" id="lblDataEntryDate"  /><br/> 

                                 <span style="width: 125px; display: inline-block;">Proof: </span>
                                 <span runat="server" id="lblProofDate"  /><br/>                                                                 

                                 <span style="width: 125px; display: inline-block;">Correct: </span>
                                 <span runat="server" id="lblCorrectDate"  /><br/>

                                 <span style="width: 125px; display: inline-block;">Tech Review: </span>
                                 <span runat="server" id="lblReviewDate"  /><br/>

                                 <span style="width: 125px; display: inline-block;">Ready: </span>
                                 <span runat="server" id="lblReadyDate"  /><br/>
                             </div>
                         </div>
                    
                     </section>
                        
                        
         
                </section>                                   
            </section>
        </div>
        
    </div>     
</asp:Content>

