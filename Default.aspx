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
            <asp:Button id="btnEncrypt" runat="server" Text="Encrypt" onclick="btnEncrypt_Click" />
            <br/>
            <br/>
            <asp:Button ID="btnDecrypt" runat="server" Text="Decrypt" onclick="btnDecrypt_Click" />
            <br/>
            <br/>        
            <asp:Button ID="btn_SSL" runat="server" Text="Build Sample SpreadSheet" OnClick="btn_SSL_Click"  />

    </form>


  
  
  
  
  
  

</body>
</html>