<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="MyWeb.EditUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="addUser">
            <label>username:</label>
            <asp:TextBox ID="inpUserName" runat="server"></asp:TextBox>
            <br />
            <label>password:</label>
            <asp:TextBox ID="inpPassWord" runat="server" TextMode="Password"></asp:TextBox>

            

            <label>Role:</label>
            <asp:DropDownList ID="inpRole" runat="server">
                <asp:ListItem Value="user">User</asp:ListItem>  
                <asp:ListItem Value="admin">Admin</asp:ListItem>  
            </asp:DropDownList>

            <asp:Button ID="editUserBnt" runat="server" Text="Save change" onclick="editUserBnt_Click" />
            
            <asp:BulletedList ID="validationError" runat="server">

            </asp:BulletedList>
        </div>
    </form>
</body>
</html>
