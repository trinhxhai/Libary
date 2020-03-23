<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemoveBook.aspx.cs" Inherits="MyWeb.RemoveBook" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Bạn có chắc chắn xóa sách này"></asp:Label>
            <asp:Button ID="RemoveBtn" runat="server" Text="Xóa" onclick="RemoveBtn_Click"/>
            <asp:Button ID="CancelBtn" runat="server" Text="Hủy" onclick="CancelBtn_Click"/>
        </div>
    </form>
</body>
</html>
