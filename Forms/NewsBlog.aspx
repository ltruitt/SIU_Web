<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="NewsBlog.aspx.cs" Inherits="Forms_Blog" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.60623.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>News Blog Management</title>
    
    <style>
        .chkbox label {
            padding-top:        0;
            background-color:   transparent;
            color:              white;
            font-size:          18px;
            margin-top:         -3px;
            margin-left: 5px;                
        }
        
        .ui-form {
            width: 90%;
        }
    </style>
    
    <script type="text/javascript">
 
        $(document).ready(function () {
            $('#Advertise_Start').datepicker();
            $('#Advertise_End').datepicker({
                minDate: $('#Advertise_Start')[0].innerHTML
            });            
        })
        
    </script>
</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />

    <div style="background-color: #45473f;">
        <div id="FormWrapper" class="ui-widget ui-form">
<!----------------->
<!-- Form Header -->
<!----------------->           
            <div class="ui-widget-header ui-corner-all" >
                <div  style="text-align: center; font-size: 2em;">News Blog Post</div>
            </div>
            
            <div class="ui-widget-content ui-corner-all">
            
                <!---------------->
                <!-- "Blog Name -->
                <!---------------->
                <span style="float: left; ">
                    <label style="float: left; padding: 0; width:auto; "  runat="server" ID="lblBlogName"></label>
                </span>

                <p style="height: 15px;"></p> 
                
                <div  class="ui-helper-clearfix" >
                    
                    <asp:CheckBox ID="Advertise"      runat="server"  Text="Advertise on Home Page"        CssClass="chkbox" />
                    
                    <label style="padding: 0; padding-right: 5px; margin-top: 0; font-size: 17px; margin-left: 70px;">Date To Start Advertising</label> 
                    <asp:TextBox ID="Advertise_Start" runat="server" MaxLength="10" Width="100px" CssClass="DataInputCss" ToolTip="Date To Start Advertising" />                                 
                            
                        <asp:RegularExpressionValidator ID="RevAdvertise_Start" runat="server" EnableClientScript="True" Enabled="False"
                                                        ErrorMessage="Provide a Date To Start Advertising" 
                                                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"  
                                                        ControlToValidate="Advertise_Start">&nbsp;
                        </asp:RegularExpressionValidator>
                            
                            <ajaxToolkit:ValidatorCalloutExtender 
                                runat="Server" ID="RevAdvertise_StartExt" TargetControlID="RevAdvertise_Start" Width="350px"
                                HighlightCssClass="highlight" CssClass="CustomValidatorCalloutStyle" PopupPosition="BottomLeft"
                                WarningIconImageUrl="/Images/warning.gif" CloseImageUrl="/Images//close.png" />    
                                
                                
                    
                    <label style="padding: 0; padding-right: 5px; margin-top: 0; font-size: 17px; margin-left: 70px;">Date To End Advertising</label>                                 
                    <asp:TextBox ID="Advertise_End" runat="server" MaxLength="10" Width="100px" CssClass="DataInputCss" ToolTip="Date To End Advertising"   />  
                               
                            
                        <asp:RegularExpressionValidator ID="RevAdvertise_End" runat="server" EnableClientScript="True" Enabled="False"
                                                        ErrorMessage="Provide a Date To Start Advertising" 
                                                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"  
                                                        ControlToValidate="Advertise_End">&nbsp;
                        </asp:RegularExpressionValidator>
                            
                            <ajaxToolkit:ValidatorCalloutExtender 
                                runat="Server" ID="RevAdvertise_EndExt" TargetControlID="RevAdvertise_End" Width="350px"
                                HighlightCssClass="highlight" CssClass="CustomValidatorCalloutStyle" PopupPosition="BottomLeft"
                                WarningIconImageUrl="/Images/warning.gif" CloseImageUrl="/Images//close.png" />                                
                                

                                        
                    
                    

                    

                    <!----------------------->
                    <!-- Title Text Box -->
                    <!----------------------->
                    <p style="clear: both; height: 10px;"></p>
                    <label style="padding: 0; padding-right: 5px; margin-top: 0; font-size: 17px;">Title: &nbsp;</label>
                    <asp:TextBox id="BlogTitle" runat="server"  width="90%"  MaxLength="200" CssClass="DataInputCss" />
                
                        <asp:RequiredFieldValidator
                            ID="RfvBlogTitle" runat="server" ErrorMessage="Provide A Blog Title" ControlToValidate="BlogTitle">&nbsp;
                        </asp:RequiredFieldValidator>

                        <ajaxToolkit:ValidatorCalloutExtender 
                            runat="Server" ID="RfvBlogTitleExt" TargetControlID="RfvBlogTitle" Width="350px"
                            HighlightCssClass="highlight" CssClass="CustomValidatorCalloutStyle" PopupPosition="BottomLeft"
                            WarningIconImageUrl="/Images/warning.gif" CloseImageUrl="/Images//close.png" />
                            
                    <div style="clear: both;  ">      
                        <p style="font-size: smaller; padding: 0;  color: red;  margin-top: -20px; text-align: center;">
                            ONLY This Text Will Appear On The Home Page If ADVERTISED
                        </p>   
                    </div> 
                
                
                    <p style="clear: both; height: 5px;"></p>
                    <label style="padding: 0; padding-right: 5px; margin-top: 0; font-size: 17px;">Title Link 
                        <span style="font-size: smaller; margin-left: 20px;"> (OPTIONAL. Link to a Web Page)</span></label>
                    <asp:TextBox id="BlogTitleURL" runat="server" width="95%" CssClass="DataInputCss"/>
                    <div style="clear: both; height: 20px; "></div>                                                     
                
                
                
                

                            
                                         
                
                
                
                    

                            
                    <div style="clear: both; height: 4px; "></div>                    
                
                    <!---------------------->
                    <!-- Content Text Box -->
                    <!---------------------->
                    <p style="clear: both; height: 0;"></p>
                    <label style="padding: 0; padding-right: 5px; margin-top: 0; font-size: 17px;">Content: &nbsp; &nbsp;</label>
                    <asp:TextBox id="BlogText" runat="server" height="300" width="95%" TextMode="MultiLine" CssClass="DataInputCss" />
                
                        <asp:RequiredFieldValidator
                            ID="RfvBlogText" runat="server" ErrorMessage="Provide Blog Content" ControlToValidate="BlogText">&nbsp;
                        </asp:RequiredFieldValidator>

                        <ajaxToolkit:ValidatorCalloutExtender 
                            runat="Server" ID="RfvBlogTextExt" TargetControlID="RfvBlogText" Width="350px"
                            HighlightCssClass="highlight" CssClass="CustomValidatorCalloutStyle" PopupPosition="BottomLeft"
                            WarningIconImageUrl="/Images/warning.gif" CloseImageUrl="/Images//close.png" />                                                           


                    <%--------------------%>                    
                    <%-- Submit Buttons --%>                    
                    <%--------------------%>
                    <div style="clear: both; height: 5px;"></div>
                    <asp:Button ID="lblBtnSubmit"   runat="server"  Text="Save Blog"    CssClass="SearchBtnCSS" onclick="lblBtnSubmit_Click"/>


                </div>           
            </div>                                                         
        </div>
    </div>
</asp:Content>

