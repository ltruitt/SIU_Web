<%@ Page Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="iTextBasicLookup.aspx.cs" Inherits="Corporate_BandC_iTextBasicLookup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
    <title>Certification Lookup</title>

    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />
    <script type="text/javascript" src="/Scripts/iText.js"></script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
        <section style="visibility: hidden; height: 0; width: 0;">
            <span runat="server" id="hlblCertCode"         />
            <span runat="server" id="hlblEID"              /> 
        </section>

        <div id="FormWrapper" class="ui-widget ui-form">
                    
            <!----------------->
            <!-- Form Header -->
            <!----------------->                               
            <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
                <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Print Certification Form</span>
            </section>  
            
            <section class="ui-widget-content ui-corner-all">
              
                <p style="height: 20px;"/>

                <div  style="margin-right: 330px; font-weight: bold; display:inline">Employee ID:</div>
                <div  style="                     font-weight: bold; display:inline">Certification:</div>

                <div style="margin-top: 5px;" >
                    <input ID="ddEmpIds" class="DataInputCss" runat="server"  style="width: 400px; margin-right: 20px;" />         
                    <input ID="acCertList" class="DataInputCss" runat="server"  style="width: 400px;" /> 
                </div>

            <%--------------------%>                    
            <%-- Submit Buttons --%>                    
            <%--------------------%>
            <div style="width: 100%;  display: inline-block; margin-top: 25px;">
                <asp:Button ID="Button1" runat="server" Text="Generate" ToolTip="Generate Certificate" />
            </div>


            </section>





        </div>
</asp:Content>



