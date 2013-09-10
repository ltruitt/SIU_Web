﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="ReplacingSafetyPaysAdmin.aspx.cs" Inherits="Forms_SafetyPaysAdmin" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.60623.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Admin View For Safety Pays Reports</title>
    
    <link type="text/css" rel="stylesheet" href="/Styles/SafetyPays.css"/>
    <script type="text/javascript" src="/Scripts/SafetyPaysAdmin.js"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />
    
<!----------------->
<!-- Form Header -->
<!----------------->           
    <div class="ui-widget-header ui-corner-all" >
        <div  style="text-align: center; font-size: 2em;">Safety Pays Administration</div>
    </div>
        

    <div class="ui-widget-content ui-corner-all">                 
        <asp:Panel ID="pnlGridView" runat="server">

                
<!------------------>
<!-- Search Panel -->
<!------------------>
            <div style="background-color: black; border: 2px solid red; margin-top: -0px; margin-bottom: 20px;">   
                <div style="text-align: center;">
                    <asp:Button ID="btnSearch" runat="server"  Text="Search"  CssClass="SearchBtnCSS" onclick="btnSearch_Click"/>

                    <span style=" color: white; font-size: 1.4em; padding-left: 15px; padding-right: 5px; "> Status</span>
                    <asp:DropDownList runat="server" ID="ddSearchStatus" CssClass="SearchTextBoxCSS"  DataSourceID="OdsStatus"></asp:DropDownList>

                    <span style="width: 10px;">&nbsp;</span>
                    <span style=" color: white; font-size: 1.4em; padding-left: 15px; padding-right: 5px; "> Type</span>
                    <asp:DropDownList runat="server" ID="ddSearchType" CssClass="SearchTextBoxCSS"  DataSourceID="OdsType"></asp:DropDownList>
                </div>
            
            </div>
        
        
            <div style="height: 15px;"></div>    

            <table style="margin-left: auto; margin-right: auto;">
                    <tr>
                        <td>
                            <SIU:SelfSortingGrid ID="gvSafetyPaysAdmin" runat="server"
                    
                                AutoGenerateColumns="False" 
                                AutoGenerateSelectButton="False" 
                                AutoGenerateEditButton="False"
                                AutoGenerateDeleteButton="False"
                     
                                AllowSorting="True"
                                AllowPaging="True" 
                                EmptyDataText="No Open Reports or Data Access Problem." 
                                ShowFooter="False"
                    
                                PageSize="15" 
                                CellPadding="2" 
                                CellSpacing="2" 

                                BorderStyle="None"
                                     
                                OnRowEditing="gvSafetyPaysAdmin_RowEditing" 
                                OnSelectedIndexChanged="gvSafetyPaysAdmin_SelectedIndexChanged" 
                                OnRowDataBound="gvSafetyPaysAdmin_RowDataBound"
                                    
                                OnPageIndexChanging="gvSafetyPaysAdmin_PageIndexChanging" 
                                OnSorting="gvSafetyPaysAdmin_Sorting" 
                                >
                                    
                                <RowStyle CssClass="TableRowCss"></RowStyle>

                                <AlternatingRowStyle BackColor="#333333" />
                                <Columns>
                                        
                                    <%-- Edit Command --%>
                                    <asp:TemplateField HeaderStyle-BorderStyle="None" 
                                        HeaderText="Edit&nbsp;&nbsp;&nbsp;">
                                        <HeaderStyle BorderStyle="None" />
                                        <ItemStyle CssClass="TableRowCss" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" Runat="server" CausesValidation="False" CommandName="Edit">
                                                <img src="/Images/edit.png" alt="edit" style="border: 0; height: 24px; width: 24px; margin-right: 15px; margin-left: 0;"/>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%-- Report ID --%>
                                    <asp:TemplateField HeaderStyle-BorderStyle="None" HeaderText="ID" 
                                        SortExpression="TaskNo">
                                        <HeaderStyle BorderStyle="None" />
                                        <ItemStyle CssClass="TableRowCss2" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("IncidentNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%-- Report Status Value --%>
                                    <asp:TemplateField HeaderStyle-BorderStyle="None" HeaderText="Status" 
                                        SortExpression="IncStatus">
                                        <HeaderStyle BorderStyle="None" />
                                        <ItemStyle CssClass="TableRowCss2" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("IncStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%-- Report Opened Date --%>
                                    <asp:TemplateField HeaderStyle-BorderStyle="None" HeaderText="Open Date" 
                                        SortExpression="IncOpenTimestamp">
                                        <HeaderStyle BorderStyle="None" />
                                        <ItemStyle CssClass="TableRowCss" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Bind("IncOpenTimestamp") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%-- Report Type --%>
                                    <asp:TemplateField HeaderStyle-BorderStyle="None" HeaderText="Type" 
                                        SortExpression="IncTypeTxt">
                                        <HeaderStyle BorderStyle="None" />
                                        <ItemStyle CssClass="TableRowCss" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text='<%# Bind("IncTypeTxt") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%-- Reported By  --%>
                                    <asp:TemplateField HeaderStyle-BorderStyle="None" HeaderText="Submitted By">
                                        <HeaderStyle BorderStyle="None" />
                                        <ItemStyle CssClass="TableRowCss" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubmittedBy" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                                <PagerStyle HorizontalAlign="Left"/>
                            </SIU:SelfSortingGrid>
                        </td>
                    </tr>
            </table>

        </asp:Panel> 
            
            
            
<!------------------------->
<!-- Edit / Insert Panel -->
<!------------------------->
        <asp:Panel ID="pnlDetailsView" runat="server" Width="100%" >

            <div style="height: 60px; margin-top: 15px;">
                <div  style="text-align: center; font-size: 1.8em; ">
                    <span runat="server" id="IncidentReportDetailHeader"></span>
                </div>
            </div> 
                    
            <table style="margin-left: auto; margin-right: auto;">
                <tr>
                    <td>
                        <asp:DetailsView ID="dvSafetyPaysDtl" runat="server" 
                            AllowPaging="False" 
                            GridLines="None"
                      
                            AutoGenerateDeleteButton="False" 
                            AutoGenerateEditButton="False" 
                            AutoGenerateInsertButton="False"  
                            AutoGenerateRows="False" 

                            DefaultMode="ReadOnly"

                            BorderStyle="Solid"
                            BorderWidth="0px"  
                            CellPadding="00" 
                            CaptionAlign="Left" 
                                     
                            EmptyDataText="Error Retrieving Report." 
                            ondatabound="dvSafetyPaysDtl_DataBound" 
                            onmodechanging="dvSafetyPaysDtl_ModeChanging" 
                            onitemcommand="dvSafetyPaysDtl_ItemCommand" 
                        >
                                    

                            <fieldheaderstyle CssClass="DtlViewHeaderCss" />
                                    
                            <Fields>
                           
                                <%-- Report Status --%>
                                <asp:TemplateField HeaderText="Report Status">
                                    <ItemTemplate>
				                        <asp:Label ID="IncStatus" runat="server" Text='<%# Bind("IncStatus") %>'></asp:Label>
				                    </ItemTemplate>
                                </asp:TemplateField> 
                                        
                                <%-- Open Date --%>
                                <asp:TemplateField HeaderText="Open Date">
                                    <ItemTemplate>
				                        <asp:Label ID="IncOpenTimestamp" runat="server" Text='<%# Bind("IncOpenTimestamp") %>'></asp:Label>
				                    </ItemTemplate>
                                </asp:TemplateField> 

                                <%-- Reporting Employee --%>
                                <asp:TemplateField HeaderText="Reporting Employee">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpFullName" runat="server" ></asp:Label>
				                    </ItemTemplate>
                                </asp:TemplateField>  
                            
                                <%-- Job Site --%>
                                <asp:TemplateField HeaderText="Job Site" >
                                    <ItemTemplate>
				                        <asp:Label ID="JobSite" runat="server" Text='<%# Bind("JobSite") %>' ></asp:Label>
				                    </ItemTemplate>
                                </asp:TemplateField>  
                            
                                <%-- Report Type --%>
                                <asp:TemplateField HeaderText="Incident Type" >
                                    <ItemTemplate>
				                        <asp:Label ID="IncTypeTxt" runat="server" Text='<%# Bind("IncTypeTxt") %>' ></asp:Label>
				                    </ItemTemplate>
                                </asp:TemplateField> 
                            
                                <%-- Report Text --%>
                                <asp:TemplateField HeaderText="Report Text" >
                                    <ItemTemplate>
				                        <asp:Label ID="Comments" runat="server" Text='<%# Bind("Comments") %>' ></asp:Label>
				                    </ItemTemplate>
                                </asp:TemplateField> 
                                        
                                <%-- Todo List Length --%>
                                <asp:TemplateField HeaderText="Todo List Length">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTodoListLen" runat="server" ></asp:Label>
				                    </ItemTemplate>
                                </asp:TemplateField>  
                                        
                                <%-- Next Due Todo List Item --%>
                                <asp:TemplateField HeaderText="Next To Do List Item">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTodoListItem" runat="server" ></asp:Label>
				                    </ItemTemplate>
                                </asp:TemplateField> 
                                        
                                <%-- Last Modified Date --%>
                                <asp:TemplateField HeaderText="Last Modified Date" >
                                    <ItemTemplate>
				                        <asp:Label ID="IncLastTouchTimestamp" runat="server" Text='<%# Bind("IncLastTouchTimestamp") %>' ></asp:Label>
				                    </ItemTemplate>
                                </asp:TemplateField> 
                                        
                                <%-- Last Modified By --%>
                                <asp:TemplateField HeaderText="Last Modified By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLastModifyEmployee" runat="server" ></asp:Label>
				                    </ItemTemplate>
                                </asp:TemplateField> 
                                
                                <%-- Reject Reason --%>
                                <asp:TemplateField HeaderText="Reject Notes" >
                                    <ItemTemplate>
                                        <asp:Label ID="xxx" runat="server" Text="KLSJFHKLJDHKLJH"></asp:Label>
				                    </ItemTemplate>
                                                                        
                                    <EditItemTemplate>
                                        <asp:TextBox ID="RejectReason" runat="server"  ></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>                                                                         
                                        
                            </Fields> 
                                    

                            <FooterTemplate>
                                    <table width="100%">            
                                    <tr>
                                    <td align="left">
                                        <asp:Button ID="DtlCmdWork"     runat="server"  Text="Work"     CssClass="SearchBtnCSS"  CommandName="Work"/>
                                    </td>
                                    <td align="center">
                                        <asp:Button ID="DtlCmdClose"    runat="server"  Text="Close"    CssClass="SearchBtnCSS"  CommandName="Close"/>
                                    </td>
                                    <td align="Right">
                                        <button class="SearchBtnCSS RejectScript" type="button" >
                                            Reject
                                        </button>
                                    </td>  
                                </table>                                      
                            </FooterTemplate>

                                    
                                                                                     
                        </asp:DetailsView>

                    </td>
                </tr>
            </table>
            
        </asp:Panel> 
    </div>
                
               
                
                
    <asp:ObjectDataSource ID="OdsStatus"    runat="server" TypeName="SqlServer_Impl" SelectMethod="GetList_SafetyPaysStatuses"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OdsType"      runat="server" TypeName="SqlServer_Impl" SelectMethod="GetList_SafetyPaysTypes"></asp:ObjectDataSource>                
    

    <div id="popup-box" class="popup">
        <a href="#" class="close"><img src="/Images/Delete.png" class="btn_close" title="Close Window" alt="Close" /></a>
        <div  class="popupDiv" >
            
            <label >
                <span>Reason for Reject</span>
                <textarea runat="server" id="RejectReason"  name="RejectReason" placeholder="Reject Notes"  rows="1" cols="1"></textarea>
            </label>

            <asp:Button ID="DtlCmdReject"   runat="server"  Text="Reject"   CssClass="SearchBtnCSS"  CommandName="Reject" OnClick="btnReject_Click"/>
        </div>
    </div>    
</asp:Content>

