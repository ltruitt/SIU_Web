<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="HardwareRequest.aspx.cs" Inherits="Forms_HardwareRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>I. T. Request For Hardware Form</title>
    
   <style>
       .chkbox {
           float: left;
        }
        
    .ui-form {
        width: 100%;
    }        
   </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
<script type="text/javascript" src="/Scripts/HardwareRequest.js"></script>
    <asp:ScriptManager ID="ScriptManager1" EnableCdn="true" Runat="Server" />



 <div style="background-color: #45473f; font-size: .9em;">
    <div id="FormWrapper" class="ui-widget ui-form">
        <!----------------->
        <!-- Form Header -->
        <!----------------->           
        <div class="ui-widget-header ui-corner-all">
            <h2>I. T. Hardware Request</h2>
        </div>
        
        
        
        <!------------------>
        <!-- Form Content -->
        <!------------------>  
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">             
            <ContentTemplate>
                <asp:Panel ID="PanelEmpID" runat="server">

                    <div class="ui-widget-content ui-corner-all">
     
     

                        <!----------------------------->
                        <!-- "Computed Cost" Section -->
                        <!----------------------------->     
                         <b>
                            <label style="float: left; width: 49%; text-align: right; padding: 0;">Estimated Cost:</label>    
                            <label style="float: right; width: 49%; text-align: left; padding: 0;" runat="server" ID="lblPrice">$00.00</label>
                         </b>
     

                        <p style="height: 10px;"></p>
            
            

                        <div class="ui-helper-clearfix" style=" margin-left: 33%; margin-right: 33%;">
        
                            <!---------------------------->
                            <!-- "Requestor ID" Section -->
                            <!---------------------------->
                            <span style="float: left; ">
                                <label style="float: left; padding: 0; width:170px;">Requestor</label>
                                <label style="float: left; padding: 0;" runat="server" ID="lblEmpName"></label>
                                <label style="float: left;  padding: 0;  display: none;" runat="server" ID="lblEmpID"></label>
                            </span>

                            <br/>

                            <span style="float: left; ">
                                <label style="float: left;  padding: 0; width: 170px;">Supr:</label>
                                <label style="float: left;  padding: 0;" runat="server" ID="lblEmpSuprName"></label>
                                <label style="float: left;  padding: 0; display: none;" runat="server"  ID="lblEmpSuprID"></label>
                            </span>                                           

                            <br/>
                            <div style=" height: 10px;"/>
                               

                            <!---------->
                            <!-- Form -->
                            <!---------->
                            <div class="ui-state-error ui-corner-all" style="width: 300px; ">
                                
                                <!----------------------------->
                                <!-- "Equipment For" Section -->
                                <!----------------------------->
                                <label style="float: left;   margin-top: 0; width: auto; padding-top: 0;">Equipment For:</label>
                                <label style="float: right;  padding: 0;" runat="server" ID="lblEquipForName"></label>                                
                                <asp:DropDownList ID="ddEmpIdEquipFor" Width="200px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="EquipForSelected"/>
        
                                <div style="float: right;">
                                    <asp:RequiredFieldValidator ID="EquipFor" runat="server" ErrorMessage="Required Field" ControlToValidate="ddEmpIdEquipFor"> 
                                        <div class="ui-icon ui-icon-alert"></div>
                                        <p class="ui-helper-reset ui-state-error-text"> Required field</p>                                    
                                    </asp:RequiredFieldValidator>
                                </div>
                                                
                
                
                                <!------------------------>
                                <!-- "Job Role" Section -->
                                <!------------------------>
                                <label style="clear: left; margin-top: 0; width: 200px; padding-top: 0;">Job Role:</label>
                                    <asp:DropDownList ID="ddUserRole" Width="200px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RoleSelected" />
                            
                                <div style="float: right;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required Field" ControlToValidate="ddUserRole"> 
                                        <div class="ui-icon ui-icon-alert"></div>
                                        <p class="ui-helper-reset ui-state-error-text"> Required field</p>                                    
                                    </asp:RequiredFieldValidator>
                                </div>
                                     
                                             
        
        
                            <!-------------------------------->
                            <!-- "Computer Options" Section -->
                            <!-------------------------------->
                            <label style="clear: left; margin-top: 0; width: 200px; padding-top: 0;">Computer Options:</label>
                            <asp:DropDownList ID="ddCompOpts" Width="200px" runat="server" Enabled="False" AutoPostBack="True" OnSelectedIndexChanged="UpdatePrice"/>
        

                            <div style="float: right;">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required Field" ControlToValidate="ddCompOpts"> 
                                    <div class="ui-icon ui-icon-alert"></div>
                                    <p class="ui-helper-reset ui-state-error-text">Required field</p>                                    
                                </asp:RequiredFieldValidator>
                            </div>

           
        
                            <!------------------------------------>
                            <!-- "Computer Accessories" Section -->
                            <!------------------------------------>
                            <label style="clear: left; margin-top: 0; width: 200px; padding-top: 0;">Computer Accessories:</label>
                            <div class="ui-state-error ui-corner-all"  style="width: 280px;" >

            
                                <div style="float: left;">
                                    <asp:CheckBox ID="chkDock"     runat="server" AutoPostBack="True" OnCheckedChanged="UpdatePrice" Text="Dock"      CssClass="chkbox" /><br/>
                                    <asp:CheckBox ID="chkCase"     runat="server" AutoPostBack="True" OnCheckedChanged="UpdatePrice" Text="Case"      CssClass="chkbox" /><br/>
                                    <asp:CheckBox ID="chkBackPack" runat="server" AutoPostBack="True" OnCheckedChanged="UpdatePrice" Text="BackPack"  CssClass="chkbox" />
                                </div>
                                
                                

                                <div style="float: right;">
                                   
                                    <div style="clear:both"></div>


                                    <div>
                                        <label style="padding: 0; padding-right: 5px; margin-top: 0; width: 65px; ">Monitors</label>
                                        <asp:TextBox Enabled="True" id="txtMonitorCnt"  runat="server" height="20" width="25" style="text-align: center" Text="0" AutoPostBack="True" OnTextChanged="UpdatePrice"/>
                                        
                                        <img alt="Add Monitor" id="MonUp" src="/images/up.gif"   width="20" height="20" style="margin-left: 2px; margin-top: 2px; float: right; "/>
                                        <img alt="Sub Monitor" id="MonDown" src="/images/dn.gif" width="20" height="20" style="margin-left: 2px; margin-top: 2px; float: right; "/>
                                    </div>


                                    <div>
                                        <p style="clear:both"></p>
                                        <label style="padding: 0; margin: 0; width: 60px;">Stands</label>
                                        <asp:TextBox Enabled="True" id="txtStandCnt"  runat="server" height="20" width="25" style="text-align: center" Text="0" AutoPostBack="True" OnTextChanged="UpdatePrice"/>
                                        
                                        <img alt="Add Stand" id="StdUp"   src="/images/up.gif" width="20" height="20" style="margin-left: 2px; margin-top: 6px; float: right; "/>
                                        <img alt="Sub Stand" id="StdDown" src="/images/dn.gif" width="20" height="20" style="margin-left: 2px; margin-top: 6px; float: right; "/>
                                    </div>
                                </div>
            
     
                            </div>
        
        
        
                            <!-------------------------------------->
                            <!-- "Additional Accessories" Section -->
                            <!-------------------------------------->
                            <label style="clear: left; margin-top: 0; width: 200px; padding-top: 15px;">Additional Software:</label>
                            <div class="ui-state-error ui-corner-all" style="width: 280px;">
            
                                <asp:CheckBox ID="chkAdobe"  runat="server" AutoPostBack="True" OnCheckedChanged="UpdatePrice" Text="Adobe Acrobat"       CssClass="chkbox" />
                                <div style="height: 30px;"></div>
                                <asp:CheckBox ID="chkCAD"    runat="server" AutoPostBack="True" OnCheckedChanged="UpdatePrice" Text="AutoCAD LT"          CssClass="chkbox" />
                                <div style="height: 30px;"></div>
                                <asp:CheckBox ID="chkVisio"  runat="server" AutoPostBack="True" OnCheckedChanged="UpdatePrice" Text="Visio"               CssClass="chkbox" />
                                <div style="height: 30px;"></div>
                                <asp:CheckBox ID="chkMsPrj"  runat="server" AutoPostBack="True" OnCheckedChanged="UpdatePrice" Text="Microsoft Project"   CssClass="chkbox" />       
                            </div>
        
                        
                        
                        
                            <%-------------------------------%>
                            <%-- Update and Cancel Buttons --%>
                            <%-------------------------------%>
                            <div style="height: 20px; clear: both;" ></div>
                            <div style="width: 300px; ">
                            
                                <div style="float: left; width: 30%; ">
                                    <asp:Button cssclass="SearchBtnCSS" id="cmdUpdate" resourcekey="cmdUpdate" runat="server" text="&nbsp;&nbsp;Update&nbsp;&nbsp;" OnClick="UpdateCmd" Visible="True"></asp:Button>
                                </div>
                                
                                <div style="float: right; width: 30%; ">
                                    <asp:Button cssclass="SearchBtnCSS" id="cmdCancel" resourcekey="cmdCancel" runat="server" text="&nbsp;&nbsp;Cancel&nbsp;&nbsp;" OnClick="CancelCmd" causesvalidation="False"  ></asp:Button>
                                </div>
                            </div>

</div>                            


                    </div>
                </asp:Panel> 

            </ContentTemplate>         
        </asp:UpdatePanel> 
    </div>
</div>    


        
</asp:Content>

