<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookDetails.aspx.cs" Inherits="MyWeb.BookDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style> 
        #bookPic{
            display:block;
            max-height:50vh;
        }
        #borBooks{
            display:<%=adminMode%>;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
       <div class="Book">
            <label>Tên Sách</label> <br />
            <asp:TextBox ID="BookName" runat="server"></asp:TextBox><br />
            <label>Loại sách :</label> <br />
            <asp:TextBox ID="BookCategory" runat="server"></asp:TextBox><br />
            <label>Giới thiệu :</label> <br />
            <asp:TextBox ID="BookDescription" runat="server"></asp:TextBox><br />
            <label>Giá bán :</label> <br />
            <asp:TextBox ID="BookPrice" runat="server"></asp:TextBox><br />
            <asp:Image ID="bookPic" runat="server" />
        </div>
        <asp:Table ID="borBooks" runat="server" Visible="">
             <asp:TableRow>
                <asp:TableCell>Id</asp:TableCell>
                <asp:TableCell>Trạng thái</asp:TableCell>
                 <asp:TableCell>Người mượn</asp:TableCell>
                <asp:TableCell>Hạn Trả</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </form>
</body>
</html>
