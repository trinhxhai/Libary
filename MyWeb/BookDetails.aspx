<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookDetails.aspx.cs" Inherits="MyWeb.BookDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Style/Layout.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
    <style>
        #bookContainer{
            display:grid;
            border: 1.75vw solid lightgreen;
            border-radius: 1.75vw;
            width:72%;
            margin-top:5vw;
            margin-left:auto;
            margin-right:auto;
            grid-template-columns: 50% 50%;
            grid-template-rows: 20vw 20vw 20vw;
            grid-gap:1%;
            padding:1vw;
            padding-left:2vw;
        }
            #bookContainer #inforBook {
                grid-column: 1/span 1;
                grid-row: 1 / span 2;
                font-size: 2vw;
                padding: 0.2vw;
            }
                #bookContainer #inforBook td{
                    vertical-align:text-top;
                }
                #bookContainer #inforBook #borBooks {
                    display: block;
                    width: 100%;
                    height: 30vw;
                    margin: auto;
                    overflow: scroll;
                }

                #bookContainer #inforBook input[type="text"] {
                    border:none;
                    border-bottom:0.15vw solid #b3e3db;
                    font-size: 1.5vw;
                    background-color: white;
                }
                #bookContainer #inforBook #BookDescription {
                    border: 0.15vw solid #b3e3db;
                    background-color: white;
                    resize: none;
                    display: block;
                    min-height: 18vw;
                    width:100%;
                }
                 #bookContainer #inforBook table{
                     width:95%;
                 }
            #bookContainer #inforBook #editBtn,#removeBtn{
                margin-left:auto;
                margin-left:auto;
            }

            #bookContainer #bookPicContainer {
                grid-column: 2/span 1;
                grid-row: 1 / span 2;
                padding: 0.25vw;
            }
            
            #bookContainer#bookPic {
                max-height: 80%;
                max-width: 90%;
                margin-left:auto;
                margin-right:auto;
            }
            #bookContainer  #bookManipulate{
                grid-column: 2/ span 1;
                grid-row: 3 / span 1;
            }
            #bookContainer #bookManipulate #borrowBtn{
                display: block;
                height: 5vw;
                width: 70%;
                margin-left: auto;
                margin-right: auto;
            }

            #bookContainer #bookPicContainer #borrowBtn:hover {
                /* transform:translateX(30%);
                    tròi đ* xấu ** - nghĩ cái khác để đấy cho zui, cười ** */
            }

            #bookContainer #bookPicContainer #bookPic {
                margin-top: 1vw;
                display: block;
                max-height: 80%;
                max-width: 90%;
                margin-left: auto;
                margin-right: auto;
            }


            #bookContainer #borBooks {
                grid-column: 1/span 1;
                grid-row: 3 / span 1;
                overflow: scroll;
                padding: 0.25vw;
            }

                #bookContainer #borBooks th {
                    font-size: 1.25vw;
                    background-color: aquamarine;
                }
                #bookContainer #borBooks  table {
                    border-collapse: collapse;
                    border:2px solid #759c91;
                }
                #bookContainer #borBooks td, #bookContainer #borBooks th{
                    border:2px solid #759c91;
                    padding:0.25vw;
                }
            
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
        <!-- AAAAAAAAAAAA-->
       <div id="bookContainer">
           <div id="inforBook">
               <table>
                   <tr>
                       <td><label>Tên sách :</label> </td>
                       <td><asp:TextBox ID="BookName" runat="server"  Enabled="False"></asp:TextBox></td>
                   </tr>
                   <tr>
                       <td><label>Loại sách :</label> </td>
                       <td><asp:TextBox ID="BookCategory" runat="server"  Enabled="False"></asp:TextBox></td>
                   </tr>
                   <tr>
                       <td><label>Giá bán :</label> </td>
                       <td><asp:TextBox ID="BookPrice" runat="server"  Enabled="False"></asp:TextBox></td>
                   </tr>
                   <tr>
                       <td><label>Giới thiệu :</label> </td>
                       <td><textarea id="BookDescription" runat="server" disabled ></textarea></td>
                       
                   </tr>
                   <tr>
                       <td> <asp:Button ID="Button1" runat="server" Text="Chọn Tệp" Visible="False" OnClick="editBtn_Click" /> </td>
                       <td><asp:Button ID="Button2" runat="server" Text="Mượn sách" Visible="False" /></td>
                   </tr>
                   <tr>
                       <td> <asp:Button ID="editBtn" runat="server" Text="Edit" Visible="False" OnClick="editBtn_Click" /></td>
                       <td><asp:Button ID="removeBtn" runat="server" Text="Remove" Visible="False" Width="99px" /></td>
                   </tr>
                   <tr>
                       <td><asp:Button ID="saveBtn" runat="server" Text="Save" Visible="False" OnClick="saveBtn_Click" /></td>
                   </tr>
                   <tr>
                       <asp:BulletedList ID="errorEditBook" runat="server"></asp:BulletedList>
                   </tr>
               </table>
           </div>
            
           <div id ="bookPicContainer">
               
               <asp:Image ID="bookPic" runat="server" />
           </div>
           <div id="bookManipulate">
                <asp:BulletedList ID="errorBorrow" runat="server"></asp:BulletedList>
                <asp:Button ID="borrowBtn" runat="server" Text="Mượn sách này " OnClick="borrowBtn_Click" />
           </div>
            
           <div id="borBooks" runat="server">
               <asp:Table ID="borBooksTable" runat="server">
                  <asp:TableHeaderRow>
                      <asp:TableHeaderCell>Id</asp:TableHeaderCell>
                      <asp:TableHeaderCell>Trạng thái</asp:TableHeaderCell>
                      <asp:TableHeaderCell>Người mượn/đặt</asp:TableHeaderCell>
                      <asp:TableHeaderCell>Hạn Trả</asp:TableHeaderCell>
                  </asp:TableHeaderRow>
            
               </asp:Table>
           </div>
           
        
        </div>
    </form>
    <footer>1a412</footer>
</body>
</html>
