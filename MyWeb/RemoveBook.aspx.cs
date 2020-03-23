using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWeb.Models;
namespace MyWeb
{
    public partial class RemoveBook : System.Web.UI.Page
    {
        private int BookId;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            // kiểm tra quyền admin
            if (Session["username"] == null || !UserLogic.isAdmin(Session["username"].ToString()))
                Response.Redirect("NoPermisson.html");
            // lấy thông tin sách
            if (Request.QueryString["BookId"] == null) Response.Redirect("NotFound.html");
            BookId = Int16.Parse(Request.QueryString["BookId"]);

            LibraryContext db = new LibraryContext();
            Book curBook = db.Books.FirstOrDefault(b => b.bookId == BookId);
            if (curBook == null) Response.Redirect("NotFound.html");


        }

        protected void RemoveBtn_Click(object sender, EventArgs e)
        {
            LibraryContext db = new LibraryContext();
            Book curBook = db.Books.FirstOrDefault(b => b.bookId == BookId);
            db.Books.Remove(curBook);
            db.SaveChanges();
            Response.Redirect("ListBook.aspx");
        }

        protected void CancelBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListBook.aspx");
        }
    }
}