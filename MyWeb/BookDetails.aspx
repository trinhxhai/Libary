<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookDetails.aspx.cs" Inherits="MyWeb.BookDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Style/Layout.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
    <link href="~/Style/BookDetails.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
    <style>
        
            
    </style>
    <script type="text/javascript" language="javascript">
        function ConfirmOnDelete() {
            if (confirm("Bạn có chắc chắn muốn xóa sách này không ?") == true)
                return true;
            else return false;
            
        }
    </script>
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
                <a href="Login.aspx" ViewStateMode="Disabled">Đăng nhập</a>
            </div>
            <div id="userNav"  runat="server">
                <a href="UserDetails.aspx?username=<%:username%>">Hello <%:username%></a>
                <asp:Button ID="logoutBtn" runat="server" Text="Logout" OnClick="logoutBtn_Click" />
            </div>
        </div>
        <!-- AAAAAAAAAAAA-->
       <div id="bookContainer">
           <div id="inforBook">
               <table>
                   <tr>
                       <asp:Button ID="borrowBtn" runat="server" Text="Đặt mượn sách này " OnClick="borrowBtn_Click" />
                   </tr>
                   <tr>
                       <td ><label>Tên sách :</label> </td>
                       <td ><asp:TextBox ID="BookName" runat="server"  Enabled="False"></asp:TextBox></td>
                   </tr>
                   <tr>
                       <td><label>Loại sách :</label> </td>
                       <td><asp:TextBox ID="BookCategory" runat="server"  Enabled="False"></asp:TextBox></td>
                   </tr>
                   <tr>
                       <td><label>Giá bán :</label> </td>
                       <td><asp:TextBox ID="BookPrice" runat="server"  Enabled="False"></asp:TextBox></td>
                   </tr>
                  
                   <tr>
                       <td><label>Giới thiệu :</label> </td>
                       <td><textarea id="BookDescription" runat="server" disabled ></textarea></td>
                       
                   </tr>
                   <tr>
                       <td><asp:Label ID="BookCountLbl" runat="server" Text="Số lượng :" Visible="False"></asp:Label> </td>
                       <td><asp:TextBox ID="BookCount" runat="server"  Enabled="False" Visible="False"></asp:TextBox></td>
                   </tr>
                   
                   <tr>
                       <asp:BulletedList ID="errorBorrow" runat="server" ViewStateMode="Disabled"></asp:BulletedList>
                   </tr>
                   <tr>
                       <td> <asp:Button ID="editBtn" runat="server" Text="Edit" Visible="False" OnClick="editBtn_Click" /></td>
                       <td><asp:Button ID="saveBtn" runat="server" Text="Save" Visible="False" OnClick="saveBtn_Click" /></td>

                   </tr>
                    <tr>
                       <td><asp:Button ID="removeBtn" runat="server" Text="Remove" Visible="False"  OnClick="removeBtn_Click" /></td>
                       <td></td>
                   </tr>
                   <tr>
                       <td><asp:BulletedList ID="errorEditBook" runat="server"></asp:BulletedList></td>
                   </tr>
                   
                   </table>
           </div>
            
           <div id ="bookPicContainer">
               <asp:Image ID="bookPic" runat="server" />
           </div>
           <div id="borBooks" runat="server">
               <asp:Table ID="borBooksTable" runat="server" ViewStateMode="Disabled">
                  <asp:TableHeaderRow>
                      <asp:TableHeaderCell>Id</asp:TableHeaderCell>
                      <asp:TableHeaderCell>Trạng thái</asp:TableHeaderCell>
                      <asp:TableHeaderCell>Người mượn/đặt</asp:TableHeaderCell>
                      <asp:TableHeaderCell>Ngày mượn Trả</asp:TableHeaderCell>
                      <asp:TableHeaderCell>Hạn Trả</asp:TableHeaderCell>
                  </asp:TableHeaderRow>
            
               </asp:Table>
           </div>
        </div>
    </form>
    </div>
    </form>
    <footer>1412</footer>
</body>
</html>
