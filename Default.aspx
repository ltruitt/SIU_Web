<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Trace="false" %>

<!DOCTYPE html>
<html>
<head>
    <title>Web Config Encryption</title>
</head>


<body>
    <form id="form1" runat="server">
            <asp:Button ID="Button1" runat="server" Text="Generate Test Email" OnClick="TestEmailClick"  />
            <br/>
            <br/>
            <asp:Button ID="Button2" runat="server" Text="Generate Veh Insp Report Emails" OnClick="TestEmailBbClick"  />
            <br/>
            <br/>
            <asp:Button ID="Button5" runat="server" Text="Generate QTM Notice" OnClick="TestEmailQtm1stClick"  />
            <br/>
            <br/>
            <asp:Button ID="Button3" runat="server" Text="Generate QTM Reminder" OnClick="TestEmailQtm15thClick"  />
<%--            <br/>
            <br/>
            <asp:Button id="btnEncrypt" runat="server" Text="Encrypt" onclick="btnEncrypt_Click" />
            <br/>
            <br/>
            <asp:Button ID="btnDecrypt" runat="server" Text="Decrypt" onclick="btnDecrypt_Click" />--%>


    </form>


  
  
  
  
  
  

</body>
</html>