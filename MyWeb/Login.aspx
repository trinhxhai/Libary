<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MyWeb.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Style/Login.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div id="loginContainer">
            <label id="containerTitle">
                Login
            </label>
            <div id="loginBox">
                <table>
                    <tr>
                        <td><label>Username:</label></td>
                        <td><input type="text" name="username"/></td>
                    </tr>
                    <tr>
                        <td><label>Password:</label></td>
                        <td><input  type="password" name="password"/><br /></td>
                    </tr>
                    <tr>
                        <td><button type="submit">login</button></td>
                        <td></td>
                    </tr>
                    
                </table>
            </div>
        </div>
         
</asp:Content>
