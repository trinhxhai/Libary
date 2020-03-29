<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Introduction.aspx.cs" Inherits="MyWeb.Introduction" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link href="~/Style/Layout.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
    <link href="~/Style/ContentPage.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
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
            <div id="headerLoginBox" runat="server">
                <a href="Login.aspx">Đăng nhập</a>
            </div>
            <div id="userNav"  runat="server">
                <a href="UserDetails.aspx?username=<%:username%>">Hello <%:username%></a>
                <asp:Button ID="logoutBtn" runat="server" Text="Logout" OnClick="logoutBtn_Click" />
            </div>
        </div>

        <div id="container">
            <label id="containerTitle">
                Giới thiệu :
            </label>
            <div id="content">
                <label>Title 1: </label> Some content <br />
                <label>Title 2: </label> Some content <br />
                <label>Title 3: </label> Some content
            </div>
        </div>
        <footer>1412</footer>
    </form>
</body>
</html>
