<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListBook.aspx.cs" Inherits="MyWeb.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" ClientIDMode="Static">
    <link href="Style/Layout.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
    <link href="Style/ListBook.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
    <script src="Script/BookFilter.js"></script>
    <script>
        function showHideCategoryList() {
            let categoryArea = document.getElementById("categoryArea");
            console.log(categoryArea.style.transform);
            if (categoryArea.style.transform == "" || categoryArea.style.transform == "translateX(-100%)") 
                categoryArea.style.transform = "translateX(0)";
            else 
                categoryArea.style.transform = "translateX(-100%)";
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" ClientIDMode="Static">

            <div id="bodyDiv">
            <div id="searchBar">
                <asp:TextBox ID="searchTextBox" runat="server" ></asp:TextBox>
                <asp:Button ID="searchBtn" runat="server" Text="Tìm kiếm" OnClick="searchBtn_Click" />
            </div>
            <div id="categoryArea">
                
                <label id="titleCategory">
                    Thể Loại
                </label>
                <asp:Button ID="removeCheck" runat="server" Text="Bỏ chọn" OnClick="removeCheck_Click" />
                <asp:CheckBoxList ID="categoryCheckList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="categoryCheckList_SelectedIndexChanged"></asp:CheckBoxList>
                <button id="showHideBtn" onclick="showHideCategoryList(); return false;">
                    Show / Hide
                </button>
            </div>
            <div id="bookContainer">
                <%for( int i=0 ; i < curListBook.Count ; i++ ) %>
                <%{ %>
                <div class="book">
                    <div class="bookPic">
                        <a href="BookDetails.aspx?bookId=<%:curListBook[i].bookId.ToString()%>">
                            <image src="Images/<%:curListBook[i].imagePath%>"></image>
                        </a>
                    </div>
                    <div class="bookInfo">
                        <h3 class="bookTitle">
                            <a href="BookDetails.aspx?bookId=<%:curListBook[i].bookId.ToString()%>"> <%:curListBook[i].bookName %></a>
                        </h3>
                    </div>
                    <%if (curListBook[i].availableLocation == "Đã hết") %>
                    <% {%>
                        <span class="availableLocation bad">
                    <% }else%>
                     <% {%>
                        <span class="availableLocation good" >
                    <% }%>

                        <%:curListBook[i].availableLocation%>
                    </span>
                    
                </div>
            
                <%} %>
            </div>
            <div id="pageNav">
                <asp:Button ID="prevPageBtn" runat="server" Text="Trang trước" OnClick="prevPageBtn_Click" />
                <asp:TextBox ID="inpPage" runat="server" Enabled="False"></asp:TextBox>
                <asp:Button ID="nextPageBtn" runat="server" Text="Trang tiếp" OnClick="nextPageBtn_Click" />

            </div>
        </div>

</asp:Content>
