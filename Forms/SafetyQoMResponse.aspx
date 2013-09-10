<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="SafetyQoMResponse.aspx.cs" Inherits="Forms_SafetyQoMResponse" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.60623.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
   <title>Question of the Month</title>
   
   <style>
        .LblScoreCss {
            font-size: 1.2em;
            text-align: center;
            font-style: italic;
            color: red;
            background-color: yellow;
            border: 2px solid red;
        }       
   </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />
    

    <div style="background-color: #45473f; ">
        <div id="FormWrapper" class="ui-widget ui-form">        
<!----------------->
<!-- Form Header -->
<!----------------->                   
            <div class="ui-widget-header ui-corner-all"  >
                <div  style="text-align: center; font-size: 2em;">Safety Question Response</div>
            </div>  
    
            <div class="ui-widget-content ui-corner-all">

                <!---------------------------->
                <!-- "Requestor ID" Section -->
                <!---------------------------->
                <div style="float: left; ">
                    <label style="float: left; padding: 0; width:auto; "  runat="server" ID="lblEmpName"></label>
                </div>
                <div style="float: right; width: 95px; ">
                    <label style="float: right;  padding: 0; width: auto;" runat="server" ID="EmpID"></label>
                    <label style="float: right;  padding: 0; width: 20px;  margin-right: 5px;">ID:</label>
                </div>

                <p style="height: 5px;"></p>   
                    
                <label style="padding: 0; font-weight: bold;  ">This Months Question</label>
                <label style="padding: 0; width: auto; font-style: italic;" runat="server" ID="Qom"></label>
                
                <div style="clear: both;"></div>

                <!----------------------->
                <!-- Response Text Box -->
                <!----------------------->
                <label style="padding: 0; padding-right: 5px; margin-top: 0; font-size: 17px; font-weight: bold; margin-top: 10px;">Your Response</label>
                <asp:TextBox id="Response" runat="server" height="200" width="95%"   TextMode="MultiLine" />
                
                    <asp:RequiredFieldValidator
                        ID="rvJobSite" runat="server" ErrorMessage="Response Can Not Be Blank" ControlToValidate="Response">&nbsp;
                    </asp:RequiredFieldValidator>
                                

                <%--------------------%>                    
                <%-- Submit Buttons --%>                    
                <%--------------------%>
                <div style="clear: both; height: 0;">
                    <label runat="server" ID="Q_Id" hidden="hidden"></label>
                </div>
                <asp:Button ID="lblBtnSubmit" runat="server"  Text="Submit Response"  
                    CssClass="SearchBtnCSS" onclick="lblBtnSubmit_Click"/>                
                    
                <div style=" text-align: center;">
                    <asp:Label runat="server"  ID="LblScore"  CssClass="LblScoreCss" BorderWidth="0"></asp:Label>
                </div>

                <div>&nbsp;</div>
            </div>    
    
        </div>
  
    </div>
    

</asp:Content>

