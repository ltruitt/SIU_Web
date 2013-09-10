<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="Jobs.aspx.cs" Inherits="Forms_Jobs" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Field Job Completion Form</title>
    
    <link rel="stylesheet" href="/styles/Jobs.css">
    <script type="text/javascript" src="/Scripts/Jobs.js"></script>
    
<style>

</style> 
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
   <asp:ScriptManager ID="ScriptManager1" EnableCdn="true" Runat="Server" />
        
 <div style="background-color: #45473f;">
    <div id="FormWrapper" class="ui-widget ui-form">
        <!----------------->
        <!-- Form Header -->
        <!----------------->           
        <div class="ui-widget-header ui-corner-all">
            <h2>Job Complete</h2>
        </div>
      

       
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">             
            <ContentTemplate>
            

                <asp:Panel ID="PanelEmpID" runat="server" BackColor="Silver" BorderColor="Black" BorderStyle="Solid" BorderWidth="3">
                    

                    <div class="ui-widget-content ui-corner-all">
                    
                        <!------------------------->
                        <!-- "Reporting Employee -->
                        <!------------------------->
                        <span style="float: left; ">
                            <label style="float: left; padding: 0; width:auto; "  runat="server" ID="lblEmpName"></label>
                        </span>      
        
                        <p style="height: 10px;"></p>
                        
                        

                        <table style="padding: 0; border-width: 0; width: 270px; margin-left: 0;" summary="Emp Table">
                        
                            <!----------------------------->
                            <!-- Job Number DropDown Box -->
                            <!----------------------------->  
                            <tr style="vertical-align: top">
		                        <td class="Head" width="150" align="left" colspan="2">
                                    <asp:Label runat="server" id="lblJobSelection" text="Job:&nbsp;" Font-Bold="false" />
		                            <asp:DropDownList runat="server" ID="ddJobNo"  width="80%"  
                                                        OnSelectedIndexChanged="SelectedJobChanged" 
                                                        AutoPostBack="true"/>
		                        </td>
	                        </tr>
     

                            <!---------------------------------------------------------------->
                            <!-- Update / Content Panel Where We Will Show The Selected Job -->
                            <!---------------------------------------------------------------->
                            <tr>
                                <td nowrap="noWrap" style="vertical-align: top" colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblSelectedJob" runat="server" Font-Bold="True"/>
                                        </ContentTemplate>     
                                    </asp:UpdatePanel>         
                                </td>
                            </tr>                        
                        

                        </table>
                    </asp:Panel>
                            
                    <asp:Panel ID="PanelBasicBilling" runat="server" Visible="False">
                        <asp:CheckBox ID="cbJobComplete" runat="server"   Text="Complete"        CssClass="chkbox"/>                                    
                        <br/>
                        <asp:CheckBox ID="cbExtraBilling" runat="server"  Text="Extra Billing"  CssClass="chkbox" AutoPostBack="True" OnCheckedChanged="ScopeChanged"/>
                        
                    </asp:Panel> 
        
        

                    <asp:Panel ID="PanelDetailBilling" runat="server" Visible="False">
                        <asp:table runat="server" id="tblBillingDetail" >    
        
                            <%-----------------------------%>
                            <%-- Jobs Number Of Managers --%>
                            <%-----------------------------%> 
                            <asp:TableRow runat="server">
		                        <asp:TableCell runat="server">
                                    <asp:Label runat="server" id="l6" text="No Managers:"/>
                                </asp:TableCell>

		                        <asp:TableCell runat="server">
                                    <asp:TextBox id="txtNumMgrs" runat="server" height="20" width="50" style="text-align: center" />
<%--                                    <img id="Down" alt="dn" src="/images/dn.gif" width="20" height="20"/>
                                    <img id="Up"   alt="up" src="/images/up.gif" width="20"  height="20"/>--%>
		                        </asp:TableCell>
	                        </asp:TableRow>
                    
                                <%-------------------------%>
                                <%-- Validation Control ---%>
                                <%-------------------------%>
                                <asp:TableRow ID="TableRow3" runat="server">
                                    <asp:TableCell ID="TableCell1" runat="server" ColumnSpan="2">
                                        <asp:RangeValidator ID="valNumMgrs" runat="server" resourcekey="valNumMgrs.ErrorMessage" ControlToValidate="txtNumMgrs"
                                            CssClass="NormalRed" Display="Dynamic" ErrorMessage="<br>Number Of Managers Must be between 1 and 9" MinimumValue="1" MaximumValue="9" Type="Integer">
                                        </asp:RangeValidator>                     
                                    </asp:TableCell>
                                </asp:TableRow>  
    
    

                            <%------------------------------%>
                            <%-- Additional Material Cost --%>
                            <%------------------------------%> 
                            <asp:TableRow runat="server">
		                        <asp:TableCell ID="TableCell5"  runat="server">
                                    <asp:Label runat="server" id="l7" text="Material Cost:"/>
                                </asp:TableCell>

		                        <asp:TableCell ID="TableCell6"  runat="server">
                                    <asp:TextBox id="txtAddMaterial" runat="server" height="20" width="100" />

		                        </asp:TableCell>
	                        </asp:TableRow>
                    
                                <%-------------------------%>
                                <%-- Validation Control ---%>
                                <%-------------------------%>
                                <asp:TableRow runat="server">
                                    <asp:TableCell ID="TableCell2" runat="server" ColumnSpan="2">
                                        <asp:RangeValidator ID="valAddMaterial" runat="server" resourcekey="valAddMaterial.ErrorMessage" ControlToValidate="txtAddMaterial"
                                            CssClass="NormalRed" Display="Dynamic" ErrorMessage="<br>Additional Material Costs Must Be Between 0 and 9,999" MinimumValue="0" MaximumValue="9999" Type="Currency">
                                        </asp:RangeValidator>                     
                                    </asp:TableCell>
                                </asp:TableRow>
    
    

                            <%----------------------------%>
                            <%-- Additional Travel Cost --%>
                            <%----------------------------%> 
                            <asp:TableRow runat="server">
		                        <asp:TableCell ID="TableCell7"  runat="server">
                                    <asp:Label runat="server" id="l8" text="Travel Cost:"/>
                                </asp:TableCell>

		                        <asp:TableCell ID="TableCell8"  runat="server">
                                    <asp:TextBox id="txtAddTravel" runat="server" height="20" width="100" />

		                        </asp:TableCell>
	                        </asp:TableRow>
                    
                                <%------------------------%>
                                <%-- Validation Control --%>
                                <%------------------------%>
                                <asp:TableRow runat="server">
                                    <asp:TableCell ID="TableCell15" runat="server" ColumnSpan="2">
                                        <asp:RangeValidator ID="valAddTravel" runat="server" resourcekey="valAddTravel.ErrorMessage" ControlToValidate="txtAddTravel"
                                            CssClass="NormalRed" Display="Dynamic" ErrorMessage="<br>Travel Costs Must Be Between 0 and 9,999" MinimumValue="0" MaximumValue="9999" Type="Currency">
                                        </asp:RangeValidator>                     
                                    </asp:TableCell>
                                </asp:TableRow>
    
    
    

                            <%----------------------------%>
                            <%-- Additional Travel Cost --%>
                            <%----------------------------%> 
                            <asp:TableRow runat="server">
		                        <asp:TableCell ID="TableCell9"  runat="server">
                                    <asp:Label  runat="server" id="l9"  text="Lodgeing Cost:"/>
                                </asp:TableCell>

		                        <asp:TableCell ID="TableCell10"  runat="server">
                                    <asp:TextBox id="txtAddLodge" runat="server" height="20" width="100" />

		                        </asp:TableCell>
	                        </asp:TableRow>
                    
                                <%------------------------%>
                                <%-- Validation Control --%>
                                <%------------------------%>
                                <asp:TableRow runat="server">
                                    <asp:TableCell ID="TableCell16" runat="server" ColumnSpan="2">
                                        <asp:RangeValidator ID="valAddLodge" runat="server" resourcekey="valAddLodge.ErrorMessage" ControlToValidate="txtAddLodge"
                                            CssClass="NormalRed" Display="Dynamic" ErrorMessage="<br>Lodging Costs Must Be Between 0 and 9,999" MinimumValue="0" MaximumValue="9999" Type="Currency">
                                        </asp:RangeValidator>                     
                                    </asp:TableCell>
                                </asp:TableRow>
    
    
   
                            <%--------------------%>
                            <%-- Any Other Cost --%>
                            <%--------------------%> 
                            <asp:TableRow runat="server">
		                        <asp:TableCell ID="TableCell11"  runat="server">
                                    <asp:Label  runat="server" id="l10" text="Other Cost:"/>
                                </asp:TableCell>

		                        <asp:TableCell ID="TableCell12"  runat="server">
                                    <asp:TextBox id="txtAddOther" runat="server" height="20" width="100" />

		                        </asp:TableCell>
	                        </asp:TableRow>
                    
                                <%------------------------%>
                                <%-- Validation Control --%>
                                <%------------------------%>
                                <asp:TableRow runat="server">
                                    <asp:TableCell ID="TableCell17" runat="server" ColumnSpan="2">
                                        <asp:RangeValidator ID="valAddOther" runat="server" resourcekey="valAddOther.ErrorMessage" ControlToValidate="txtAddOther"
                                            CssClass="NormalRed" Display="Dynamic" ErrorMessage="<br>Other Costs Must Be Between 0 and 9,999" MinimumValue="0" MaximumValue="9999" Type="Currency">
                                        </asp:RangeValidator>                     
                                    </asp:TableCell>
                                </asp:TableRow>
    
    
    
    
                            <%----------------------%>
                            <%-- Jobs Total Hours --%>
                            <%----------------------%> 
                            <asp:TableRow runat="server">
		                        <asp:TableCell ID="TableCell13"  runat="server">
                                    <asp:Label runat="server" id="l11" text="Total Hours:"/>
                                </asp:TableCell>

		                        <asp:TableCell ID="TableCell14"  runat="server">
                                    <asp:TextBox id="txtTotHours" runat="server" height="20" width="50" />

		                        </asp:TableCell>
	                        </asp:TableRow>

                                <%------------------------%>
                                <%-- Validation Control --%>
                                <%------------------------%>
                                <asp:TableRow runat="server">
                                    <asp:TableCell ID="TableCell18" runat="server" ColumnSpan="2">
                                        <asp:RangeValidator ID="valTotHours" runat="server" resourcekey="valTotHours.ErrorMessage" ControlToValidate="txtTotHours"
                                            CssClass="NormalRed" Display="Dynamic" ErrorMessage="<br>Total Hours Must Be Between 0 and 9,999" MinimumValue="0" MaximumValue="9999" Type="Integer">
                                        </asp:RangeValidator>            
                                    </asp:TableCell>
                                </asp:TableRow>

                        </asp:table>
                    </asp:Panel> 
                
                
                    <%-------------------------------%>
                    <%-- Update and Cancel Buttons --%>
                    <%-------------------------------%>
                    <br/>

                    <asp:Button ID="cmdUpdate" runat="server"  Text="Update"  CssClass="UpdateBtnCSS" onclick="cmdUpdate_Click" Visible="False"/>
                    <asp:Button ID="cmdCancel" runat="server"  Text="Cancel"  CssClass="CancelBtnCSS" onclick="cmdCancel_Click"/>

                </div>

            </ContentTemplate>         
        </asp:UpdatePanel>     
        </div>
    </div>

</asp:Content>





