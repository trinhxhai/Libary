<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditBook.aspx.cs" Inherits="MyWeb.EditBook" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style> 
        inp
        #bookPic{
            display:block;
            max-height:50vh;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="newBook">
            <h2>Edit sách</h2>
            <label>Tên Sách</label> <br />
            <asp:TextBox ID="BookName" runat="server"></asp:TextBox><br />
            <label>Loại sách :</label> <br />
            <asp:TextBox ID="BookCategory" runat="server"></asp:TextBox><br />
            <label>Giới thiệu :</label> <br />
            <asp:TextBox ID="BookDescription" runat="server"></asp:TextBox><br />
            <label>Giá bán :</label> <br />
            <asp:TextBox ID="BookPrice" runat="server"></asp:TextBox><br />
            <asp:FileUpload ID="ImageUpload" runat="server" />
            <asp:Button ID="editBookBnt" runat="server" Text="Save Change" onclick="editBookBnt_Click"/>
            <asp:Image ID="bookPic" runat="server" />
            <%// Danh sách error validation %>
            <asp:BulletedList ID="validBookErrors" runat="server" >
            </asp:BulletedList>
        </div>
    </form>
</body>
</html>
