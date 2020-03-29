<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Location.aspx.cs" Inherits="MyWeb.Location" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
                Địa điểm :
            </label>
            <div id="content">
                <label>Nhà Sách Đại Kim : </label> 369 Thư viện phô mai 69, Hành tinh xanh số 6 <br />
                <label>Nhà Sách Hải Nha : </label> 2027 Đinh Nam, Thành phố Honda, Hành tinh song song <br />
                <label>Nhà Sách Phương Nam : </label> quầy số 6, thành phố dưới nước,
            </div>
        </div>
        <footer>1412</footer>
    </form>
</body>
</html>
