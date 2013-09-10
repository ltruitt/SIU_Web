<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="HardwareRequestAdmin.aspx.cs" Inherits="Forms_HardwareRequestAdmin" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.60623.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Admin View For IT Hardware Request</title>
                      
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
<!----------------->
<!-- Form Header -->
<!----------------->           
    <div class="ui-widget-header ui-corner-all" >
        <div  style="text-align: center; font-size: 2em;">I. T. Hardware Request Administration</div>
    </div>
        

    <div class="ui-widget-content ui-corner-all">                 
        <asp:Panel ID="pnlGridView" runat="server">
                
        <div style="height: 65px; padding-top: 10px;">
        </div>  
            
        <div style="margin-left: auto; margin-right: auto; width: 500px; ">
        
            <asp:DataList ID="DataListCompletedJobs" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" DataSourceID="OdsList">
                <ItemTemplate>
                    <%# ShowJobHeader(Container.DataItem) %>
                    <%# ShowJobDetails(Container.DataItem) %>
                    
                </ItemTemplate>
            
                <SeparatorTemplate>
                    <hr style="border: solid 1px black; height: 0;"/>
                </SeparatorTemplate>
            </asp:DataList>                

        </div>
        </asp:Panel> 
    </div>
        
        
        

	<script type="text/javascript">
	    function ShowHide(IdId) {
	        var SorH = $('#' + IdId + "_A").val();
	        if (SorH == 'Show Request') {
	            $('#' + IdId).css('display', 'block');
	            $('#' + IdId + "_A").val('Hide Request');
	        }
	        else {
	            $('#' + IdId).css('display', 'none');
	            $('#' + IdId + "_A").val('Show Request');
	        }

	    }

        function PostRequestComplete() {
            // Create A Parameter To Send To Server

            var ButtonId = event.srcElement.id;
            var params = '{RequestID:"' + ButtonId + '"}';

            // Make Async Call To Server Showing We Are Looking At A Movie
            jQuery.ajax(
                {
                    type: "POST",
                    url: "/Forms/HardwareRequestAdmin.aspx/RequestComplete",
                    data: params,
                    contentType: "application/json",
                    dataType: "json"
                }
            );
        }

	</script>
            
    <asp:ObjectDataSource ID="OdsList" runat="server" TypeName="SqlServer_Impl" SelectMethod="GetHardwareOpenRequest"></asp:ObjectDataSource>

</asp:Content>

