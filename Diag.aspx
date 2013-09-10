<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="Diag.aspx.cs" Inherits="Diag" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <asp:Button ID="Button1" runat="server" Text="Generate Test Email" OnClick="TestEmailClick"  />
    <br/><br/><br/>

    <a href="/trace.axd" target="_Trace"><b>Click Here For  Trace Viewer.......</b></a><br/>
    <asp:Label runat="server" ID="SessionDiag"></asp:Label>
    
    
    
    <b> Holidays </b><br/>
    <asp:Label runat="server" ID="Holidays"></asp:Label>
    
</asp:Content>

