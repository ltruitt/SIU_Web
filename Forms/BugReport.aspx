<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="BugReport.aspx.cs" Inherits="Forms_BugReport" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.60623.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Web Site Issue</title>
    
<style>
    .TaskListBtn {
	    background: whitesmoke;
	    background: black;
	    
	    padding: 3px;
        padding-right: 7px;
        padding-left: 7px;
        
	    border: 2px solid red;
	    float: right;
	    font-size: .9em;
        text-decoration: none;
        font-weight: bold;
        
	    z-index: 99999;
	    border-radius:3px 3px 3px 3px;
        -moz-border-radius: 3px;                /* Firefox */
        -webkit-border-radius: 3px;             /* Safari, Chrome */
        
	    box-shadow: 0 0 20px red;          /* CSS3 */
        -moz-box-shadow: 0 0 20px red;     /* Firefox */
        -webkit-box-shadow: 0 0 20px red;  /* Safari, Chrome */        
    }    
    
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />
    
    <div style="background-color: #45473f;">
        <div id="FormWrapper" class="ui-widget ui-form">
<!----------------->
<!-- Form Header -->
<!----------------->           
            <div class="ui-widget-header ui-corner-all" >
                <div  style="text-align: center; font-size: 2em;">Web Site Problem or Suggestion</div>
            </div>
            
            <div class="ui-widget-content ui-corner-all">
                
                
                <!---------------------------->
                <!-- "Requestor ID" Section -->
                <!---------------------------->
                <span style="float: left; ">
                    <label style="float: left; padding: 0; width:auto; "  runat="server" ID="lblEmpName"></label>
                </span>

                <p style="height: 10px;"></p>
                
                <p style="font-size: smaller;">
                    Submit this form and provide input that will improve this web site.  Suggestions will be reviewed and POSSIBLY implemented.
                </p>  
                
                <div  class="ui-helper-clearfix" >
                
                    <!----------------------->
                    <!-- Description Text Box -->
                    <!----------------------->
                    <p style="clear: both; height: 0;"></p>
                    <label style="padding: 0; padding-right: 5px; margin-top: 0; font-size: 17px;">Description: &nbsp; &nbsp;</label>
                    <asp:TextBox id="Description" runat="server" height="150" width="95%" TextMode="MultiLine" MaxLength="2400"/>
                
                        <asp:RequiredFieldValidator
                            ID="RvTxtDescription" runat="server" ErrorMessage="Please Provide Detail" ControlToValidate="Description">&nbsp;
                        </asp:RequiredFieldValidator>

                        <ajaxToolkit:ValidatorCalloutExtender 
                            runat="Server" ID="ExtRvTxtComments" TargetControlID="RvTxtDescription" Width="350px"
                            HighlightCssClass="highlight" CssClass="CustomValidatorCalloutStyle" PopupPosition="BottomLeft"
                            WarningIconImageUrl="/Images/warning.gif" CloseImageUrl="/Images//close.png" />

                    
                    <!----------------------->
                    <!-- Steps Text Box -->
                    <!----------------------->
                    <p style="clear: both; height: 0;"></p>
                    <label id="lblStepsToCreate" runat="server" style="padding: 0; padding-right: 5px; margin-top: 0; font-size: 17px;">Steps to Produce Problem:</label>
                    <asp:TextBox id="StepsToCreate" runat="server" height="150" width="95%" TextMode="MultiLine" MaxLength="2400"/>
                
                    <!----------------------->
                    <!-- Reject Notes-->
                    <!----------------------->
                    <p style="clear: both; height: 0;"></p>
                    <label id="lblRejectNotes" runat="server" style="padding: 0; padding-right: 5px; margin-top: 0; font-size: 17px;">Reject Notes:</label>
                    <asp:TextBox id="txtRejectNotes" runat="server" height="150" width="95%" TextMode="MultiLine" MaxLength="2400"/>
                    

                    <%--------------------%>                    
                    <%-- Submit Buttons --%>                    
                    <%--------------------%>
                    <div style="clear: both; height: 0;"></div>
                    <asp:Button ID="lblBtnSubmit"   runat="server"  Text="Submit Report"    CssClass="SearchBtnCSS" onclick="lblBtnSubmit_Click"/>
                    <a          id="lblBtnTaskList" runat="server" href="/Forms/BugReportList.aspx" class="TaskListBtn" ><span style="color: red;">TaskList</span></a>

                    <asp:Button ID="lblBtnAccept" runat="server"  Text="Accept"  CssClass="SearchBtnCSS" onclick="lblBtnAccept_Click"/>
                    <asp:Button ID="lblBtnReject" runat="server"  Text="Reject"  CssClass="SearchBtnCSS" onclick="lblBtnReject_Click"/>

                        
            </div>                                                         
        </div>
        </div>
    </div>

</asp:Content>

