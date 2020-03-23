<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MyWeb.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" method="post">
            <label>Username:</label> <input type="text" name="username"/> <br />
            <label>Password:</label><input  type="password" name="password"/><br />
            <button type="submit">login</button>
    </form>
</body>
</html>
