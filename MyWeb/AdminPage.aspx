<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="MyWeb.ListUser"  %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Style/Layout.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
    <style>
        
        #viewContainer{
            display:grid;
            grid-template-columns:45vw 45vw;
            grid-template-rows:5vw 40vw;
            margin-top:100px;
            margin-bottom:8vw;
            width:90vw;
            height:45vw;
            margin-left:auto;
            margin-right:auto;
            border-radius:25px;
            background-color: antiquewhite;

        }
        #menuBar {
            display: flex;
            align-items: flex-end;
            background-color: cadetblue;
            grid-column: 1 / span 2;
            grid-row: 1 / span 1;
            padding-left: 2vw;
            border-radius: 25px 25px 0 0;
        }

            #menuBar input {
                display: block;
                height: 68%;
                width: 10vw;
                border: none;
                border: 1px solid green;
                border-bottom: none;
                margin-right: 0.5vw;
                font-size:18px;
                transition-duration:0.5s;
            }
            #menuBar input:hover {
                cursor: pointer;
                font-size:20px;
                
            }
                #menuBar input:focus {
                    background-color: green;
                }

        .info {
            padding: 2vw;
            grid-column: 1 / span 1;
            grid-row: 2 / span 1;
        }

        .preview {
            padding: 2vw;
            background-color: antiquewhite;
            grid-column: 2 / span 1;
            grid-row: 2 / span 1;
            border-left: 2px dotted gray;
        }

        /*USER LIST*/ 
        #listUserView{
            padding:20px;
            grid-row: 2 / span 1;
        }
        #listBoxUser{
            display:block;
            width:85%;
            margin-left:auto;
            margin-right:auto;
            margin-top:5vw;
            padding:0.5vw;
            height:200px;
        }
        #removeUserBtn{
            display:block;
            width:85%;
            margin-left:auto;
            margin-right:auto;
            margin-top:1vw;
        }
        /*PRE VIEW BOOK*/
        #previewBook {
            display: grid;
            grid-template-columns: 50% 50%; /* containter's col 2 45vw*/
            grid-template-rows: 15% 85%;
        }

            #previewBook h2 {
                grid-column: 1/span 2;
                grid-row: 1 / span 1;
            }
            #previewBook #inforPreviewBook{
                grid-column: 1/ span 1;
                grid-row: 2 / span 1;
            }
            #previewBook #previewPicBook {
                max-height: 85%;
                grid-column: 2/ span 1;
                grid-row: 2 / span 1;
            }
            #previewBook #borBookMessage{
                grid-column: 1/ span 2;
                grid-row: 3 / span 1;
            }
        /*PREVIEW User*/
        #previewUser{
            border:2px solid black;

        }
            #previewUser #preUserInfo{
                border : 1px solid gray;
                font-size:1.25vw;
                width:85%;
                height:45%;
                padding-top:1.5vw;
                margin-left:auto;
                margin-right:auto;
            }
            #previewUser #preUserInfo{
                border : 1px solid gray;
                font-size:1.25vw;
                width:85%;
                height:45%;
                padding-top:1.5vw;
                margin-left:auto;
                margin-right:auto;
            }
                    #previewUser #editUser,#saveUser {
                        display:inline-block;
                        background-color:aqua;
                        margin-left:0.75vw;
                        width:6vw;
                    }
            #previewUser #last-row {
                border: 1px solid pink;
                background-color: aquamarine;
                height:4.25vw;
            }
            #previewUserBook{
                display: grid;
                grid-template-columns: 50% 50%;
                grid-template-rows: 85% 15%;
                width:85%;
                height:55%;
                margin-left:auto;
                margin-right:auto;
            }
        #previewUser #previewUserBook #userListBorBook {
            grid-column: 1/ span 1;
            grid-row: 1/ span 1;
            border: 1px solid pink;
        }
        #previewUser #previewUserBook #returnBookBtn {
            grid-column: 1/ span 1;
            grid-row: 2/ span 1;
            display: block;
        }
        /*Prebook right side*/
        #previewUser #previewUserBook #previewUserBookPic {
            grid-column:2/ span 1;
            grid-row:1/ span 2;
            display: block;
            max-height:100%;
            max-width:100%;
            margin-left:auto;
            margin-right:auto;
        }

        /*ADD USER*/

    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="navBar">
            <a href="ListBook.aspx" class="logoLink">
                <img src="Images/Logo.jpg" />
            </a>
            <ul class="navMenu">
                <li><a href="AdminPage.aspx">Admin</a></li>
                <li><a href=""> Trả sách </a></li>
                <li><a href="Login.aspx">Đăng nhập</a></li>
                <li><a href="">Đăng kí </a></li>
            </ul>
        </div>

        <label id ="curentAdmin">
        </label>
        <div id="viewContainer">
        <div id="menuBar">   
            <asp:Button ID="viewListUser" runat="server" Text="Users"  OnClick="viewListUser_Click" />
            <asp:Button ID="viewAddUser" runat="server" Text="Add User" OnClick="viewAddUser_Click" />
            <asp:Button ID="viewNewBook" runat="server" Text="New Book" OnClick="viewNewBook_Click" />
            <asp:Button ID="viewBorBook" runat="server" Text="Borrow Book" OnClick="viewBorBook_Click" />
        </div>
        <asp:MultiView ID="inforMView" runat="server" ActiveViewIndex="0">
            <asp:View ID="UserView" runat="server">
                <div id ="listUserView">

                
                <asp:ListBox ID="listBoxUser" runat="server" OnSelectedIndexChanged="listBoxUser_SelectedIndexChanged" AutoPostBack="True">
                </asp:ListBox>
                <asp:Button ID="removeUserBtn" runat="server" Text="Xóa User" OnClick="removeUserBtn_Click" />
                </div>
            </asp:View>
            <asp:View ID="addUserView" runat="server">
                <div id="addUser"  class="info" >
                    <h2> Thêm user</h2>

                    <table>
                        <tr>
                            <td>
                                <label>User name:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="inpUserName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Password:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="inpPassWord" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>CMND:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="inpCMND" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Real name:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="inpRealName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Dia chi:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="inpDchi" runat="server"></asp:TextBox>
                            </td>
                        </tr><tr>
                            <td>
                                <label>Role:</label>
                            </td>
                            <td>
                                <asp:DropDownList ID="inpRole" runat="server">
                                    <asp:ListItem Value="user">User</asp:ListItem>  
                                    <asp:ListItem Value="admin">Admin</asp:ListItem>  
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="addUserBnt" runat="server" Text="Add User" onclick="addUserBnt_Click" />

                            </td>
                        </tr>
                        
                    </table>

                    <asp:BulletedList ID="validUserErrors" runat="server">
                    </asp:BulletedList>
                </div>
            </asp:View>
            <asp:View ID="newBookView" runat="server">
                <div id="newBook"  class="info" >
                    <h2>Thêm sách</h2>
                    <label>Tên Sách</label> <br />
                    <asp:TextBox ID="BookName" runat="server"></asp:TextBox><br />
                    <label>Loại sách :</label> <br />
                    <asp:TextBox ID="BookCategory" runat="server"></asp:TextBox><br />
                    <label>Giới thiệu :</label> <br />
                    <asp:TextBox ID="BookDescription" runat="server"></asp:TextBox><br />
                    <label>Giá bán :</label> <br />
                    <asp:TextBox ID="BookPrice" runat="server"></asp:TextBox><br />
                    <label>Số Lượng :</label> <br />
                    <asp:TextBox ID="Amount" runat="server"></asp:TextBox><br />
                    <asp:FileUpload ID="ImageUpload" runat="server" />

                    <asp:Button ID="addBookBnt" runat="server" Text="Add Books" onclick="addBookBnt_Click"/>
                    <asp:BulletedList ID="validBookErrors" runat="server">
                    </asp:BulletedList>
                </div>
            </asp:View>
            <asp:View ID="borBookView" runat="server">
                <div id="borrowBook"  class="info" >
                    <h2>Mượn sách</h2>
                    <asp:ListBox ID="listBorUser" runat="server">
                    </asp:ListBox>
                    <asp:ListBox ID="listBorBook" runat="server" AutoPostBack="True" OnSelectedIndexChanged="listBorBook_SelectedIndexChanged">
                    </asp:ListBox>
                    <label>Thời hạn</label>

                    <asp:DropDownList ID="returnDate" runat="server" >
                        <asp:ListItem Value="1">1 Tuần</asp:ListItem>  
                        <asp:ListItem Value="2">2 Tuần</asp:ListItem>  
                        <asp:ListItem Value="3">3 Tuần</asp:ListItem>  
                        <asp:ListItem Value="4">4 Tuần</asp:ListItem>  
                    </asp:DropDownList>

                    <asp:Button ID="borrowBtn" runat="server" Text="Borrow" OnClick="borrowBtn_Click" />

                    <asp:BulletedList ID="borrowMessages" runat="server"></asp:BulletedList>
                </div>
            </asp:View>
            
        </asp:MultiView>

        <asp:MultiView ID="preMView" runat="server" ActiveViewIndex="0">
            <asp:View ID="View5" runat="server">
                <div id="previewUser"> 
                    <div id="preUserInfo">
                        <table>
                            <tr>
                                <td>
                                    <label>Username :</label>
                                </td>
                                <td>
                                    <asp:Label ID="accName" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Password :</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="passWord" runat="server" Enabled="False" ViewStateMode="Enabled"  ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Họ tên :</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="realName" runat="server" Enabled="False" ViewStateMode="Enabled"  ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>CMND :</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="CMND" runat="server" Enabled="False" ViewStateMode="Enabled"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Địa chỉ :</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="dchi" runat="server" Enabled="False" ViewStateMode="Enabled"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Số lượng sách đang mượn :</label>
                                </td>
                                <td>
                                    <asp:Label ID="borBookCount" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr id="last-row">
                                <td>
                                    <asp:Button ID="editUser" runat="server" Text="Edit" OnClick="editUser_Click" />
                                    <asp:Button ID="saveUser" runat="server" Text="Save" OnClick="saveUser_Click" />

                                </td>
                                <td>
                                    <asp:BulletedList ID="validationUserError" runat="server"></asp:BulletedList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="previewUserBook">
                        <asp:ListBox ID="userListBorBook" runat="server" AutoPostBack="True" OnSelectedIndexChanged="userListBorBook_SelectedIndexChanged" >
                        </asp:ListBox>
                        <asp:Button ID="returnBookBtn" runat="server" Text="Trả sách" OnClick="returnBookBtn_Click" />
                        <asp:Image ID="previewUserBookPic" runat="server" />
                    </div>
                    
                </div>

            </asp:View>
            <asp:View ID="previewBook" runat="server">
                <div id="previewBook" class="preview">
                    <h2>Preview Book</h2>
                    <div id="inforPreviewBook">
                        <label>Tên Sách : </label> 
                            <asp:Label ID="previewNameBook" runat="server" Text=""></asp:Label>
                        <br />
                        <label>Loại sách :</label>
                            <asp:Label ID="previewCategoryBook" runat="server" Text=""></asp:Label>
                        <br />
                        <label>Giới thiệu :</label>
                        <asp:Label ID="previewDecriptionBook" runat="server" Text=""></asp:Label>
                        <br />
                        <label>Giá bán :</label> 
                        <asp:Label ID="previewPriceBook" runat="server" Text=""></asp:Label>
                    </div>

                    <asp:Image ID="previewPicBook" runat="server" class="bookPic"/>

                    <asp:BulletedList ID="borBookMessage" runat="server" >
                    </asp:BulletedList>
                </div>
            </asp:View>
        </asp:MultiView>
        </div>


    </form>
    <footer>1a412</footer>
</body>
 
</html>
