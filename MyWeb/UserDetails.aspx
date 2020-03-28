<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="MyWeb.UserDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Style/Layout.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
    <style>
        *{
            margin:0;
            padding:0;
        }
        #userContainer{
            display: grid;
            grid-template-columns: 20% 20% 20% 20% 20%;
            width: 80%;
            height:40vw;
            border: 2px solid green;
            margin-top:5vw;
            margin-left:auto;
            margin-right:auto;

        }
            #userContainer #userInfo{
                grid-column:1 / span 2;
                height:100%;
                border: 1px solid red;
                padding:1vw;
            }

            #userContainer #borrowBooks{
                border: 1px solid blue;
                grid-column:3 / span 3;
                height:100%;
                display:flex;
                flex-direction:column;
                overflow:scroll;
                padding:1vw;
            }

                .book{
                    width:35vw;
                    height:20vw;
                    border:1px solid black;
                    padding:1vw;
                    margin-bottom:0.5vw;
                }
                .book .info{
                    display:inline-block;
                    width:20vw;
                    height:20vw;
                    padding-left:0.5vw;
                }
                .book img{
                    float:left;
                    max-width:10vw;
                    max-height:20vw;
                }
                .book{
            
                }
        /*book  container*/
        
    </style>
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
                <li><a href=""> Địa điểm </a></li>
                <li><a href="">Giới thiệu </a></li>
            </ul>
            <div id="userNav"  runat="server">
                <a href="UserDetails.aspx?username=<%:username%>">Hello <%:username%></a>
                <asp:Button ID="logoutBtn" runat="server" Text="Logout" OnClick="logoutBtn_Click" />
            </div>
        </div>
        
        <div id="userContainer">
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
            <h2>Sách mượn:</h2>
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
        
        
       
    </form>
   <footer>1a412</footer>
</body>
</html>
