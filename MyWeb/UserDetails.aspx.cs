using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWeb.Models;
namespace MyWeb
{
    public partial class UserDetails : System.Web.UI.Page
    {
        public User user = new User();
        public List<BorBook> borBooks = new List<BorBook>();
        protected void Page_Load(object sender, EventArgs e)
        {
            string idq = Request.QueryString["userName"];
            if (idq == null) Response.Redirect("NotFound.html");
            string userName = idq.ToString();
            LibraryContext db = new LibraryContext();
            user = db.Users.SingleOrDefault(c => c.userName == userName);
            if(user==null) Response.Redirect("NotFound.html");
            borBooks = user.borBooks.ToList();
            
            for(int i = 0; i < borBooks.Count; i++)
            {
                var row = new TableRow();
                var cellId = new TableCell();
                cellId.Text = borBooks[i].BookId.ToString();
                row.Cells.Add(cellId);
                var cellBookName = new TableCell();
                int bookid = borBooks[i].BookId;
                cellBookName.Text = db.Books.FirstOrDefault(b => b.bookId == bookid).bookName;
                row.Cells.Add(cellBookName);
                var cellDate = new TableCell();
                cellDate.Text = borBooks[i].returnDate.ToString();
                row.Cells.Add(cellDate);
                borBookTable.Rows.Add(row);
            }
            borBookTable.DataBind();




        }
    }
}