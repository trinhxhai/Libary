<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JustForTest.aspx.cs" Inherits="MyWeb.JustForTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #RadioButtonList1{
            background-color:forestgreen;
            height:50px;
            overflow:scroll;
        }
        #ListBox1{
            width:400px;
            height:200px;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:ListBox ID="ListBox1" runat="server" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" AutoPostBack="True" OnTextChanged="ListBox1_TextChanged">
            <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
           <asp:ListItem>A</asp:ListItem>
            <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
           <asp:ListItem>A</asp:ListItem>
            <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
           <asp:ListItem>A</asp:ListItem>
            <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
           <asp:ListItem>A</asp:ListItem>
        </asp:ListBox>
        <asp:RadioButtonList ID="RadioButtonList1" runat="server">
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
           <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
           <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
           <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
           <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
          <asp:ListItem>A</asp:ListItem>
        </asp:RadioButtonList>
    </form>

</body>

</html>
