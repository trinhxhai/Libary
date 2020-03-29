<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MyWeb.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Style/Layout.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
    <style>
        #loginContainer {
            display: flex;
            flex-direction:column;
            align-items:center;
            width: 92vw;
            height: 40vw;
            border: 1px solid gray;
            margin-top: 88px;
            margin-left: auto;
            margin-right: auto;
        }

        #containerTitle {
            position: relative;
            background-color: #3af0b0;
            box-shadow: 0 0.1vw 0 #d1d1d1;
            padding-left: 2vw;
            font-size: 1.4vw;
            line-height: 6vw;
            height:6vw;
            width:100%;
        }
        #loginBox{
            margin-top:5vw;
            font-size:1.8vw;
        }
        #loginBox input{
            width:15vw;
            height:3vw;
        }
        #loginBox td{
            padding:0.5vw;
        }
        #loginBox button{
            width:8vw;
            height:3vw;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" method="post">
        <div class="navBar">
            <a href="ListBook.aspx" class="logoLink">
                <img src="Images/Logo.jpg" />
            </a>
            <ul class="navMenu">
                <li><a href="ListBook.aspx">Trang Chủ</a></li>
                <li><a href="AdminPage.aspx">Admin</a></li>
                <li><a href=""> Địa điểm </a></li>
                <li><a href="">Giới thiệu </a></li>
            </ul>
            
        </div>
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
            
    <footer>1a412</footer>
    </form>
</body>
</html>
