<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookDetails.aspx.cs" Inherits="MyWeb.BookDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
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
               <div id="borrowArea">
                   <asp:Button ID="borrowBtn" runat="server" Text="Đặt mượn sách này " OnClick="borrowBtn_Click" />
                   <asp:BulletedList ID="errorBorrow" runat="server" ViewStateMode="Disabled"></asp:BulletedList>

                   <asp:Label ID="locationLabel" runat="server" Text="Địa điểm : "></asp:Label>
                   <asp:DropDownList ID="listLocation" runat="server"></asp:DropDownList>
               </div>
               

               <asp:Table ID="Table1" runat="server">
                   <asp:TableRow>
                       <asp:TableCell>
                           <label>Tên sách :</label> 
                       </asp:TableCell>
                       <asp:TableCell>
                           <textarea id="BookName" runat="server" disabled> </textarea>
                       </asp:TableCell>
                   </asp:TableRow>
                   <asp:TableRow>
                       <asp:TableCell><label>Loại sách :</label></asp:TableCell>
                       <asp:TableCell><asp:TextBox ID="BookCategory" runat="server"  Enabled="False"></asp:TextBox></asp:TableCell>
                   </asp:TableRow>
                   <asp:TableRow>
                       <asp:TableCell><label>Giá bán :</label></asp:TableCell>
                       <asp:TableCell><asp:TextBox ID="BookPrice" runat="server"  Enabled="False"></asp:TextBox></asp:TableCell>
                   </asp:TableRow>
                   <asp:TableRow>
                       <asp:TableCell><label>Giới thiệu :</label> </asp:TableCell>
                       <asp:TableCell><textarea id="BookDescription" runat="server" disabled ></textarea></asp:TableCell>
                   </asp:TableRow>
                   <asp:TableRow>
                       <asp:TableCell><asp:Label ID="BookCountLbl" runat="server" Text="Số lượng :" Visible="False"></asp:Label></asp:TableCell>
                       <asp:TableCell><asp:TextBox ID="BookCount" runat="server"  Enabled="False" Visible="False"></asp:TextBox></asp:TableCell>
                   </asp:TableRow>
               </asp:Table>
                <asp:Button ID="editBtn" runat="server" Text="Edit" Visible="False" OnClick="editBtn_Click" />
                <asp:Button ID="saveBtn" runat="server" Text="Save" Visible="False" OnClick="saveBtn_Click" />
                <asp:Button ID="removeBtn" runat="server" Text="Remove" Visible="False"  OnClick="removeBtn_Click" />
                <asp:BulletedList ID="errorEditBook" runat="server"></asp:BulletedList>

               

           </div>
            
           <div id ="bookPicContainer">
               <asp:Image ID="bookPic" runat="server" />
           </div>
           <div id="borBooks" runat="server">
               <asp:Table ID="borBooksTable" runat="server" ViewStateMode="Disabled">
               </asp:Table>
           </div>
        </div>
    </form>
    </div>
    </form>
    <footer>1412</footer>
</body>
</html>
