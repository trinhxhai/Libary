<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MyWeb.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link href="~/Style/Layout.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
    <link href="~/Style/Login.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
</head>
<body>
    <form id="form1" runat="server" method="post">
        <div class="navBar">
            <a href="ListBook.aspx" class="logoLink">
                <img src="Images/Logo.jpg" />
            </a>
            <ul class="navMenu">
                <li><a href="ListBook.aspx">Trang Chủ</a></li>
                <li><a href="AdminPage.aspx">Admin</a></li>
                <li><a href="Location.aspx"> Địa điểm </a></li>
                <li><a href="Introduction.aspx">Giới thiệu </a></li>
            </ul>
            
        </div>
        <div id="loginContainer">
            <label id="containerTitle">
                Login
            </label>
            <div id="loginBox">
                <table>
                    <tr>
                        <td><label>Username:</label></td>
                        <td><input type="text" name="username"/></td>
                    </tr>
                    <tr>
                        <td><label>Password:</label></td>
                        <td><input  type="password" name="password"/><br /></td>
                    </tr>
                    <tr>
                        <td><button type="submit">login</button></td>
                        <td></td>
                    </tr>
                    
                </table>
            </div>
        </div>
            
    <footer>1a412</footer>
    </form>
</body>
</html>
