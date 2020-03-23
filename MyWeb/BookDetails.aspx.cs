using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWeb.Models;
namespace MyWeb
{
    public partial class BookDetails : System.Web.UI.Page
    {
        private int BookId;
        public List<BorBook> listBorBook = new List<BorBook>();
        public string adminMode = "none";

        protected void Page_Load(object sender, EventArgs e)
        {   
            // Kiểm tra quyền  Admin, hiện bảng BorBook
            LibraryContext db = new LibraryContext();
            if (Session["username"] != null && UserLogic.isAdmin(Session["username"].ToString())) adminMode = "inline-block";
            // lấy thông tin sách
            if (Request.QueryString["BookId"] == null) Response.Redirect("NotFound.html");
            BookId = Int16.Parse(Request.QueryString["BookId"]);

            Book curBook = db.Books.FirstOrDefault(b => b.bookId == BookId);
            if (curBook == null) Response.Redirect("NotFound.html");
            BookName.Text = curBook.bookName;
            BookCategory.Text = curBook.category;
            BookDescription.Text = curBook.description;
            var path = "~/Images/";
            bookPic.ImageUrl = path + curBook.imagePath;
            BookPrice.Text = curBook.price;

            listBorBook = db.BorBooks.Where(bb => bb.BookId == curBook.bookId).OrderBy(bb => bb.state).ToList();

            for (int i = 0; i < listBorBook.Count; i++) {
                Response.Write(listBorBook[i].id.ToString()+'\n');
                var row = new TableRow();
                var cellID = new TableCell();
                cellID.Text = listBorBook[i].id.ToString();
                row.Cells.Add(cellID);
                var cellState = new TableCell();
                cellState.Text = listBorBook[i].state.ToString();
                row.Cells.Add(cellState);
                var cellUser = new TableCell();
                var cellDate = new TableCell();
                if ((listBorBook[i].user == null))
                {
                    cellUser.Text = "";
                    cellDate.Text = "";
                }
                else
                {
                    cellUser.Text = listBorBook[i].user.userName;
                    cellDate.Text = listBorBook[i].returnDate.ToString();
                }
                row.Cells.Add(cellUser);
                row.Cells.Add(cellDate);
                borBooks.Rows.Add(row);
            }
            borBooks.DataBind();
            
        }

    }
}