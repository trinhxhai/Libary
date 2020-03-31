using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWeb.Models;
using System.ComponentModel.DataAnnotations;
namespace MyWeb
{
    public partial class BookDetails : System.Web.UI.Page
    {
        private List<BorBook> listBorBook = new List<BorBook>();
        private LibraryContext db = new LibraryContext();
        private Book curBook;
        public string username ;
        protected void Page_Load(object sender, EventArgs e)
        {

            //thêm sự kiện xác nhận
            removeBtn.Attributes.Add("onclick", "return ConfirmOnDelete()");
            // lấy thông tin sách
            int BookId;
            if (Request.QueryString["BookId"] == null) Response.Redirect("NotFound.html");
            BookId = Int16.Parse(Request.QueryString["BookId"]);
            curBook = db.Books.FirstOrDefault(b => b.bookId == BookId);
            // nếu không tồn tại sách 
            if (curBook == null) Response.Redirect("NotFound.html");

            if (Session["userName"] != null)
            {
                headerLoginBox.Style.Add("display", "none");
                username = Session["userName"].ToString();
            }
            else
            {
                userNav.Style.Add("display", "none");
            }

            // Kiểm tra quyền  Admin, hiện bảng BorBook
            if (Session["username"] != null && UserLogic.isAdmin(Session["username"].ToString()))
            {
                editBtn.Visible = true;
                removeBtn.Visible = true;
                // không thể dùng viewstate để lưu table động, nạp lại mỗi lần :<
                updateBorBook();
            }
            else
            {
                // nếu không phải admin không hiện danh sách sách
                borBooks.Style.Add("display", "none");
            }
            

            if (IsPostBack)
            {
                
            }
            else
            {
                // chỉ lần đầu tiên
                // load dữ liệu cho control
                BookName.Text = curBook.bookName;
                BookCategory.Text = curBook.category;
                BookDescription.InnerHtml = curBook.description;
                var path = "~/Images/";
                bookPic.ImageUrl = path + curBook.imagePath;
                BookPrice.Text = curBook.price;
                BookCount.Text = curBook.amount;
            }
        }

        private void updateBorBook()
        {
            var z = borBooksTable;
            borBooksTable.Controls.Clear();
            listBorBook = curBook.BorBooks.OrderBy(bb => -bb.state).ToList();
            var headerRow = new TableHeaderRow();
            var headerID = new TableHeaderCell();
            headerID.Text = "ID";
            headerRow.Controls.Add(headerID);
            var headerState = new TableHeaderCell();
            headerState.Text = "Trạng thái";
            headerRow.Controls.Add(headerState);
            var headerUser = new TableHeaderCell();
            headerUser.Text = "Người mượn/đặt";
            headerRow.Controls.Add(headerUser);
            var headerBorrowDate = new TableHeaderCell();
            headerBorrowDate.Text = "Ngày mượn";
            headerRow.Controls.Add(headerBorrowDate);
            var headerReturnDate = new TableHeaderCell();
            headerReturnDate.Text = "Hạn trả";
            headerRow.Controls.Add(headerReturnDate);
            borBooksTable.Rows.Add(headerRow);
            for (int i = 0; i < listBorBook.Count; i++)
            {
                var row = new TableRow();
                var cellID = new TableCell();
                cellID.Text = listBorBook[i].id.ToString();
                row.Cells.Add(cellID);
                var cellState = new TableCell();
                cellState.Text = (listBorBook[i].state > 0) ? (listBorBook[i].state == 1) ? "Đặt trước" : "Đã mượn" : "Có sẵn";
                row.Cells.Add(cellState);
                var cellUser = new TableCell();
                var cellborrowDate = new TableCell();
                var cellreturnDate = new TableCell();
                if ((listBorBook[i].User == null))
                {
                    cellUser.Text = "";
                    cellborrowDate.Text = "";
                    cellreturnDate.Text = "";
                }
                else
                {
                    cellUser.Text = listBorBook[i].User.userName;
                    cellborrowDate.Text = listBorBook[i].borrowDate.ToString();
                    cellreturnDate.Text = listBorBook[i].returnDate.ToString();
                }
                row.Cells.Add(cellUser);
                row.Cells.Add(cellborrowDate);
                row.Cells.Add(cellreturnDate);
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
            BookCountLbl.Visible = true;
            BookCount.Visible = true;
            BookCount.Enabled = true;
            BookName.Focus();
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            List<string> messages= new List<string>();
            // khi nhấn được nút btn => chắc chắn đã able text
            string bName = BookName.Text;
            string bCate = BookCategory.Text;
            string bPrice = BookPrice.Text;
            string bDescrip = Request.Form["BookDescription"];
            // validate thông tin sách
            int bookCount;
            if (!int.TryParse(BookCount.Text, out bookCount))
            {
                messages.Add("Số lượng không hợp lệ");
                errorEditBook.DataSource = messages;
                errorEditBook.DataBind();
                return;
            }
            int borBookState1 = curBook.BorBooks.Count(bb => bb.state >= 1);

            if (bookCount < borBookState1)
            {
                messages.Add("Số lượng sách không được ít hơn số lượng sách đã được mượn");
                errorEditBook.DataSource = messages;
                errorEditBook.DataBind();
                return;
            }

            Book tmp = new Book
            {
                bookId = curBook.bookId,
                bookName = bName,
                category = bCate,
                amount = BookCount.Text,
                imagePath = curBook.imagePath,
                price = bPrice,
                description = bDescrip
            };
            ValidationContext validBook = new ValidationContext(tmp, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(tmp, validBook, results, true);
            messages = results.Select(res => res.ErrorMessage.ToString()).ToList();
            // nếu lưu thành công => 
            // validate trường số lượng sách

            if (isValid)
            {
                // sinh/hủy số sách chênh lệch !
                int delta = bookCount - int.Parse(curBook.amount);
                //
                if (delta < 0)
                {
                    //tức là số sách nhập vào ít hơn số sách cũ, admin muốn loại bỏ bớt số sách đang "có sẵn - không có ai mượn";
                    BookLogic.removeBorBook(ref curBook, -delta);
                }
                else
                {
                    // tăng số lượng có sẵn của sách
                    BookLogic.genBorBook(ref curBook, delta);
                }

                // Nếu sửa thành công cần reload lại bảng borBook
                curBook.bookName = tmp.bookName;
                curBook.category = tmp.category;
                curBook.price = tmp.price;
                curBook.amount = bookCount.ToString();

                curBook.description = tmp.description;
                messages.Add("Lưu thành công!");
                db.SaveChanges();

                BookCount.Text = bookCount.ToString();

                // disable edit các trường

                BookName.Enabled = false;
                BookCategory.Enabled = false;
                BookPrice.Enabled = false;
                // texrea
                BookDescription.Disabled = true;

                saveBtn.Visible = false;

                BookCountLbl.Visible = false;
                BookCount.Visible = false;
                BookCount.Enabled = false;
                // it good ideaaa
                // for reload the borbook list too
                // cực kì cần thận với lệnh redirect này, nó sẽ chạy các lệnh trong khối !IsPostBack
                // cần set lại các giá trị thật đúng đắn
                Response.Redirect("BookDetails.aspx?bookId=" + curBook.bookId);
            }
            else
            {
                errorEditBook.DataSource = results.Select(res => res.ErrorMessage.ToString()).ToList();
                errorEditBook.DataBind();
            }
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
                // ĐẶT MƯỢN THÀNH CÔNG => Cập nhật danh sách BorBook
                updateBorBook();

            }
            else
            {
                message.Add("Xin lỗi, sách này đã được mượn hết!");
            }
            errorBorrow.DataSource = message;
            errorBorrow.DataBind();
        }

        protected void removeBtn_Click(object sender, EventArgs e)
        {
            db.Books.Remove(curBook);
            db.SaveChanges();
            Response.Redirect("ListBook.aspx");
        }

        protected void logoutBtn_Click(object sender, EventArgs e)
        {
            Session["userName"] = null;
            Response.Redirect("ListBook.aspx");
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