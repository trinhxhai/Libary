<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookDetails.aspx.cs" Inherits="MyWeb.WebForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" ClientIDMode="Static">

    <link href="/Style/BookDetails.css" rel="stylesheet" type="text/css" media="screen" runat="server" />
 

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" ClientIDMode="Static">
               <div id="bookContainer">
           <div id="inforBook">
               <div id="borrowArea">
                   <asp:Button ID="borrowBtn" runat="server" Text="Đặt mượn sách này " OnClick="borrowBtn_Click" />
                   <asp:BulletedList ID="errorBorrow" runat="server" ViewStateMode="Disabled"></asp:BulletedList>

                   <asp:Label ID="locationLabel" runat="server" Text="Địa điểm : "></asp:Label>
                   <asp:DropDownList ID="listLocation" runat="server"></asp:DropDownList>
               </div>
               

               <asp:Table ID="Table1" runat="server">
                   <asp:TableRow>
                       <asp:TableCell>
                           <label>Tên sách :</label> 
                       </asp:TableCell>
                       <asp:TableCell>
                           <textarea id="BookName" runat="server" disabled> </textarea>
                       </asp:TableCell>
                   </asp:TableRow>
                   <asp:TableRow>
                       <asp:TableCell><label>Loại sách :</label></asp:TableCell>
                       <asp:TableCell><asp:TextBox ID="BookCategory" runat="server"  Enabled="False"></asp:TextBox></asp:TableCell>
                   </asp:TableRow>
                   <asp:TableRow>
                       <asp:TableCell><label>Giá bán :</label></asp:TableCell>
                       <asp:TableCell><asp:TextBox ID="BookPrice" runat="server"  Enabled="False"></asp:TextBox></asp:TableCell>
                   </asp:TableRow>
                   <asp:TableRow>
                       <asp:TableCell><label>Giới thiệu :</label> </asp:TableCell>
                       <asp:TableCell><textarea id="BookDescription" runat="server" disabled ></textarea></asp:TableCell>
                   </asp:TableRow>
                   <asp:TableRow>
                       <asp:TableCell><asp:Label ID="BookCountLbl" runat="server" Text="Số lượng :" Visible="False"></asp:Label></asp:TableCell>
                       <asp:TableCell><asp:TextBox ID="BookCount" runat="server"  Enabled="False" Visible="False"></asp:TextBox></asp:TableCell>
                   </asp:TableRow>
               </asp:Table>
                <asp:Button ID="editBtn" runat="server" Text="Edit" Visible="False" OnClick="editBtn_Click" />
                <asp:Button ID="saveBtn" runat="server" Text="Save" Visible="False" OnClick="saveBtn_Click" />
                <asp:Button ID="removeBtn" runat="server" Text="Remove" Visible="False"  OnClick="removeBtn_Click" />
                <asp:BulletedList ID="errorEditBook" runat="server"></asp:BulletedList>

               

           </div>
            
           <div id ="bookPicContainer">
               <asp:Image ID="bookPic" runat="server" />
           </div>
           <div id="borBooks" runat="server">
               <asp:Table ID="borBooksTable" runat="server" ViewStateMode="Disabled">
               </asp:Table>
           </div>
        </div>
    
</asp:Content>
