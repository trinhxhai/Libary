<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="MyWeb.WebForm4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Style/UserDetails.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div id="userContainer">
            <label id="containerTitle">
                User Details
            </label>
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
            <label id="bbTitle">Sách mượn:</label>
            <%for (int i = 0; i < listBorBooks.Count; i++) %>
            <% {%>

                <%if (listBorBooks[i].state == 1)  %>
                <% {%>
                <div class="book" style="opacity:0.5">
                <%}
                    else %>
                <% {%>
                <div class="book">
                    
                <% }%>


                    <img src="Images/<%:listBorBooks[i].Book.imagePath %>" alt="" />

                    <div class="info">
                        <label>Tên sách:</label>
                        <%: listBorBooks[i].Book.bookName %><br />
                        <label>Ngày mượn:</label>
                        <%: listBorBooks[i].borrowDate.ToString("dd'/'MM'/'yyyy") %><br />
                        <%if (listBorBooks[i].state == 2)  %>
                        <% {%>
                            <label>Hạn trả:</label>
                            <%: listBorBooks[i].returnDate.ToString("dd'/'MM'/'yyyy") %>
                        <% }%>
                        
                    </div>
                </div>
            <% }%>

            </div>


        </div>
</asp:Content>
