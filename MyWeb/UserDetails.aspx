<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="MyWeb.UserDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link href="~/Style/Layout.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
    <link href="~/Style/UserDetails.css" rel="stylesheet" type="text/css" media="screen" runat="server" />

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
            <div id="userNav"  runat="server">
                <a href="UserDetails.aspx?username=<%:username%>">Hello <%:username%></a>
                <asp:Button ID="logoutBtn" runat="server" Text="Logout" OnClick="logoutBtn_Click" />
            </div>
        </div>
        
        <div id="userContainer">
            <label id="containerTitle">
                User Details
            </label>
            <div id="userInfo">
                <label>Username : </label>
                <asp:Label ID="tbUserName" runat="server" ></asp:Label> <br />
                <label>Họ và tên : </label>
                <asp:Label ID="tbRealName" runat="server" ></asp:Label><br />
                <label>Địa chỉ : </label>
                <asp:Label ID="tbDiaChi" runat="server" ></asp:Label><br />
                <label>Số sách đã mượn : </label>
                <asp:Label ID="borBookCount" runat="server" >/20</asp:Label> <br />
                <label>Role : </label>
                <asp:Label ID="tbRole" runat="server" ></asp:Label>
          </div>
            

            <div id="borrowBooks">
            <label id="bbTitle">Sách mượn:</label>
            <%for (int i = 0; i < listBorBooks.Count; i++) %>
            <% {%>
                <div class="book">
                    <img src="Images/<%:listBorBooks[i].Book.imagePath %>" alt="" />

                    <div class="info">
                        <label>Tên sách:</label>
                        <%: listBorBooks[i].Book.bookName %><br />
                        <label>Ngày mượn:</label>
                        <%: listBorBooks[i].borrowDate %><br />
                        <label>Hạn trả:</label>
                        <%: listBorBooks[i].returnDate %>
                    </div>
                </div>
            <% }%>

            </div>


        </div>
        
        
       <footer>1412</footer>
    </form>
   
</body>
</html>
