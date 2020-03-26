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
        private List<BorBook> listBorBook = new List<BorBook>();
        private LibraryContext db = new LibraryContext();
        private Book curBook;

        protected void Page_Load(object sender, EventArgs e)
        {   
            // Kiểm tra quyền  Admin, hiện bảng BorBook
            if (Session["username"] != null && UserLogic.isAdmin(Session["username"].ToString())) {
                editBtn.Visible = true;
                removeBtn.Visible = true;
            }
            else
            {
                borBooks.Style.Add("display", "none");
            }
            int BookId;
            // lấy thông tin sách
            // nếu k chỉ định id sách, sẽ điều hướng sang trang NotFound
            if (Request.QueryString["BookId"] == null) Response.Redirect("NotFound.html");
            BookId = Int16.Parse(Request.QueryString["BookId"]);
            // tìm sách theo id
            curBook = db.Books.FirstOrDefault(b => b.bookId == BookId);
            // k tìm thấy sách thì tiếp tục điều hướng sang NotFound.html
            if (curBook == null) Response.Redirect("NotFound.html");

            // load dữ liệu cho control
            BookName.Text = curBook.bookName;
            BookCategory.Text = curBook.category;
            BookDescription.InnerHtml = curBook.description;
            var path = "~/Images/";
            bookPic.ImageUrl = path + curBook.imagePath;
            BookPrice.Text = curBook.price;



            //LOAD BẢNG BORBOOK cho admin
            //listBorBook = db.BorBooks.Where(bb => bb.BookId == curBook.bookId);
            listBorBook = curBook.BorBooks.OrderBy(bb => bb.state).ToList();
            for (int i = 0; i < listBorBook.Count; i++)
            {
                var row = new TableRow();
                var cellID = new TableCell();
                cellID.Text = listBorBook[i].id.ToString();
                row.Cells.Add(cellID);
                var cellState = new TableCell();
                cellState.Text = (listBorBook[i].state>0)? (listBorBook[i].state ==1 )?"Được đặt":"Đã mượn" : "Có sẵn";
                row.Cells.Add(cellState);
                var cellUser = new TableCell();
                var cellDate = new TableCell();
                if ((listBorBook[i].User == null))
                {
                    cellUser.Text = "";
                    cellDate.Text = "";
                }
                else
                {
                    cellUser.Text = listBorBook[i].User.userName;
                    cellDate.Text = listBorBook[i].returnDate.ToString();
                }
                row.Cells.Add(cellUser);
                row.Cells.Add(cellDate);
                borBooksTable.Rows.Add(row);
            }
            borBooksTable.DataBind();
        }

        protected void editBtn_Click(object sender, EventArgs e)
        {
            BookName.Enabled = true;
            BookCategory.Enabled = true;
            BookDescription.Disabled = false;
            BookPrice.Enabled = true;
            saveBtn.Visible = true;
            BookName.Focus();
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            // khi nhấn được nút btn => chắc chắn đã able text
            string bNam = BookName.Text;
            string bCate = BookCategory.Text;
            string bPrice = BookPrice.Text;
            string bDescrip = Request.Form["BookDescription"];

            
            // nếu lưu thành công => 
            saveBtn.Visible = false; 
            // disable edit các trường
            BookName.Enabled = false;
            BookCategory.Enabled = false;
            BookPrice.Enabled = false;
            // texrea
            BookDescription.Disabled = true;
            saveBtn.Visible = false;
        }

 
        protected void borrowBtn_Click(object sender, EventArgs e)
        {
            List<String> message = new List<string>();

            // Kiểm tra đăng nhập
            if (Session["userName"] == null)
            {
                message.Add("Bạn cần đăng nhập trước khi đăng kí mượn sách !");
                errorBorrow.DataSource = message;
                errorBorrow.DataBind();
                return;
            }
            string tmpUserName = Session["username"].ToString();
            User user = db.Users.FirstOrDefault(u => u.userName == tmpUserName);
            // Kiểm tra người dùng đã mượn sách này hay chưa, hoặc đã đặt trước sách này hay chưa
            if (user.borBooks.Any(bb => bb.BookId == curBook.bookId))
            {
                message.Add("Bạn đã đặt mượn trước sách này rồi !");
                errorBorrow.DataSource = message;
                errorBorrow.DataBind();
                return;
            }

            // Kiểm tra số lượng sách còn lại
            BorBook tmp = db.BorBooks.FirstOrDefault(bb => bb.BookId == curBook.bookId &&  bb.state == 0);

            if (tmp!=null)
            {
                // nếu có sách cho người dùng mượn và chuyển sách 
                tmp.User = user;
                tmp.state = 1;
                user.borBooks.Add(tmp);
                message.Add("Đặt mượn trước thành công!");
                db.SaveChanges();
            }
            else
            {
                message.Add("Xin lỗi, sách này đã được mượn hết!");
            }
            errorBorrow.DataSource = message;
            errorBorrow.DataBind();
        }
    }
    class BookInstance
    {
        public int id { get; set; } 
        public string BookName { get; set; }
        public string userName { get; set; }
        public string returnDate { get; set; }
    }
}