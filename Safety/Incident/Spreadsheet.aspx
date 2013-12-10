<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="Spreadsheet.aspx.cs" Inherits="Safety_Incident_Spreadsheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <br/>
    <asp:Button ID="btn_SSL" runat="server" Text="Build SpreadSheet" OnClick="btn_SSL_Click"  />
</asp:Content>

