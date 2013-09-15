<%@ Page Language="C#" AutoEventWireup="true" CodeFile="submit.aspx.cs" Inherits="ProProfs_submit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <%--//localhost/ProProfs/submit.aspx?quiz_id=1&quiz_name=test--%>
    

    <title>ShermcoYOU! ProProfs Interface</title>
    
    <!------------------------------ JQUERY --------------------------------->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="http://code.jquery.com/jquery-1.8.3.min.js"></script>--%>

    <script type="text/javascript" src="/ProProfs/Submit.js"></script>     
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <span  id="Status" ></span>
    </div>
    </form>
</body>
</html>
