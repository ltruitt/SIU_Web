<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="JobsView.aspx.cs" Inherits="Forms_JobsView" ViewStateMode="Disabled" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Corporate Job Completion Review</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div style="margin-left: auto; margin-right: auto; width: 300px;" >
        <asp:DataList ID="DataListCompletedJobs" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
            <ItemTemplate>
                <%--<b>Job Number: </b> <%# ((SIU_Complete_Job)Container.DataItem).EmpNo  %>--%>
                <%# ShowJobHeader(Container.DataItem) %>
                <%# ShowJobDetails(Container.DataItem) %>
            </ItemTemplate>
            
            <SeparatorTemplate>
                <hr style="border: solid 1px black; height: 0;"/>
            </SeparatorTemplate>
        </asp:DataList>
    </div>
    
	<script type="text/javascript">
		function ShowHide(IdId) {
		    var MorL = $('#' + IdId + "_A").text();
		    if (MorL == 'MORE') {
		        $('#' + IdId).css('display', 'block');
		        $('#' + IdId + "_A").text('LESS');
		    }
		    else {
		        $('#' + IdId).css('display', 'none');
		        $('#' + IdId + "_A").text('MORE');
		    }

		}
	</script>
    

</asp:Content>

