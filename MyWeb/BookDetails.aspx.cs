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
    public partial class WebForm : System.Web.UI.Page
    {
        private List<BorBook> listBorBook = new List<BorBook>();
        private LibraryContext db = new LibraryContext();
        private Book curBook;
        public string username;
        private User curUser;
        Dictionary<int, ValueTuple<string, int>> locationDict = new Dictionary<int, ValueTuple<string, int>>();
        protected void Page_Load(object sender, EventArgs e)
        {
            //thêm sự kiện xác nhận xóa borBook
            removeBtn.Attributes.Add("onclick", "return ConfirmOnDelete()");

            // lấy thông tin sách
            int BookId;
            if (Request.QueryString["BookId"] == null) Response.Redirect("NotFound.html");
            BookId = Int16.Parse(Request.QueryString["BookId"]);
            curBook = db.Books.FirstOrDefault(b => b.bookId == BookId);
            // nếu không tồn tại sách 
            if (curBook == null) Response.Redirect("NotFound.html");


            // check xem còn sách này k
            if (!curBook.BorBooks.Any(bb => bb.state == 0))
            {
                borrowBtn.Text = "Đã hết";
                borrowBtn.Style.Add("background-color", "gray !important");
                borrowBtn.Enabled = false;
                locationLabel.Visible = false;
                listLocation.Visible = false;
                borrowBtn.DataBind();
            };
            // trường hợp sách bị người dùng mượn sẽ được ghi đè ở dưới

            curUser = (User)Session["user"];

            if (curUser != null)
            {
                // Kiểm tra quyền  Admin, hiện bảng BorBook
                if (curUser.role == "admin")
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


                //cập nhật user trước khi check 
                curUser = db.Users.FirstOrDefault(user => user.userName == curUser.userName);

                // trong danh sách borBooks của User nếu có chỉ có thể có 1 borBook của loại sách này, dễ dàng xét được nếu có
                var borBook = curUser.borBooks.FirstOrDefault(bb => bb.BookId == curBook.bookId);
                if (borBook != null)
                {
                    if (borBook.state == 1)
                    {
                        borrowBtn.Text = "Hủy đặt trước";
                        borrowBtn.Style.Add("background-color", "red !important");
                        borrowBtn.Enabled = true;
                        locationLabel.Visible = false;
                        listLocation.Visible = false;
                        borrowBtn.DataBind();
                    }
                    if (borBook.state == 2)
                    {
                        borrowBtn.Text = "Đã mượn";
                        borrowBtn.Style.Add("background-color", "gray !important");
                        borrowBtn.Enabled = false;
                        locationLabel.Visible = false;
                        listLocation.Visible = false;
                        borrowBtn.DataBind();
                    }
                }
            }

            if (IsPostBack)
            {
            }
            else
            {
                // chỉ lần đầu tiên
                // load dữ liệu cho control
                //BookName.Text = curBook.bookName;
                BookName.InnerText = curBook.bookName;
                BookCategory.Text = curBook.category;
                BookDescription.InnerHtml = curBook.description;
                var path = "~/Images/";
                bookPic.ImageUrl = path + curBook.imagePath;
                BookPrice.Text = curBook.price;
                BookCount.Text = curBook.amount;

                listLocation.ItemType = "LocationInstance";
                listLocation.DataTextField = "locationInfo";
                listLocation.DataValueField = "id";
            }
            parseLocation();
        }

        public void parseLocation()
        {
            if (curBook == null) return;
            locationDict.Clear();
            // update CurBook
            //curBook = db.Books.FirstOrDefault(book => book.bookId == curBook.bookId);
            List<BorBook> availableBorBooks = curBook.BorBooks.Where(bb => bb.state == 0).ToList();
            Dictionary<int, string> locationName = new Dictionary<int, string>();
            for (int i = 0; i < availableBorBooks.Count; i++)
            {

                ValueTuple<string, int> val;
                int lcId = availableBorBooks[i].LocationId;
                string lcName = availableBorBooks[i].Location.dchi;
                if (locationDict.TryGetValue(lcId, out val))
                {
                    locationDict[lcId] = (val.Item1, val.Item2 + 1);
                }
                else
                {
                    locationDict.Add(lcId, (lcName, 1));

                }

            }

            listLocation.DataSource = locationDict.Select(lc =>
                    new LocationInstance
                    {
                        id = lc.Key,
                        locationInfo = lc.Value.Item1 + "(" + lc.Value.Item2 + ")"
                    }
                );
            listLocation.DataBind();

        }
        private void updateBorBook()
        {
            var z = borBooksTable;
            borBooksTable.Controls.Clear();
            //curBook = db.Books.FirstOrDefault(book => book.bookId == curBook.bookId);
            listBorBook = curBook.BorBooks.OrderBy(bb => bb.LocationId).ToList();

            var headerRow = new TableHeaderRow();

            var headerID = new TableHeaderCell();
            headerID.Text = "ID";
            headerRow.Controls.Add(headerID);
            var headerState = new TableHeaderCell();
            headerState.Text = "Trạng thái";
            headerRow.Controls.Add(headerState);
            var headerLocation = new TableHeaderCell();
            headerLocation.Text = "Địa điểm";
            headerRow.Controls.Add(headerLocation);
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

                var cellLocation = new TableCell();
                cellLocation.Text = listBorBook[i].Location.dchi;
                row.Cells.Add(cellLocation);

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
                    cellborrowDate.Text = listBorBook[i].borrowDate.ToString("dd'/'MM'/'yyyy");
                    if (listBorBook[i].state == 2)
                        cellreturnDate.Text = listBorBook[i].returnDate.ToString("dd'/'MM'/'yyyy");
                    else
                        cellreturnDate.Text = "";
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
            //BookName.Enabled = true;
            BookName.Disabled = false;
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
            List<string> messages = new List<string>();
            // khi nhấn được nút btn => chắc chắn đã able text
            //string bName = BookName.Text;
            string bName = BookName.InnerText;
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
                    Book.removeBorBook(ref curBook, -delta);
                }
                else
                {
                    // tăng số lượng có sẵn của sách
                    //*****************************************
                    //*****************************************
                    //*****************************************
                    //*****************************************
                    //*****************************************
                    //BookLogic.genBorBook(ref curBook, delta);
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

                //BookName.Enabled = false;
                BookName.Disabled = true;
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
            curUser = (User)Session["user"];

            if (curUser == null)
            {
                message.Add("Bạn cần đăng nhập trước khi đặt mượn sách !");
                errorBorrow.DataSource = message;
                errorBorrow.DataBind();
                return;
            }

            // không thể thao thác thêm borBook vào User được tạo ra từ 2 context khác nhau
            // bắt buộc phải dùng User được lấy ra từ db ở hiện tại
            // cũng k thể dùng Session lưu context , như vậy context sẽ k được cập nhật
            // Tốt hơn nên tạo Context mới ở những thao tác ntn
            db = new LibraryContext();
            curUser = db.Users.FirstOrDefault(user => user.userName == curUser.userName);

            // Kiểm tra xem người dùng đã đặt sách này chưa
            BorBook borBook = curUser.borBooks.FirstOrDefault(bb => bb.BookId == curBook.bookId);
            // tức người dùng có đặt/mượn sách rồi
            if (borBook != null)
            {
                if (borBook.state == 1)
                {
                    // HỦY ĐẶT
                    // nếu đã đặt thì hủy đặt
                    curUser.borBooks.Remove(borBook);
                    borBook.state = 0;
                    db.SaveChanges();
                    borrowBtn.Text = "Đặt mượn sách này";
                    borrowBtn.Style.Add("background-color", "");
                    locationLabel.Visible = true;
                    listLocation.Visible = true;
                    //red!important;
                    borrowBtn.DataBind();

                    // ***  cập nhật CurBook vì borBook ở trên vừa thay đổi - borBook là BorBook của curBook
                    curBook = db.Books.FirstOrDefault(book => book.bookId == curBook.bookId);

                    parseLocation();
                    updateBorBook();

                    message.Add("Hủy đặt sách thành công !");
                    errorBorrow.DataSource = message;
                    errorBorrow.DataBind();
                    locationLabel.Visible = true;
                    listLocation.Visible = true;
                    return;
                };

                // Kiểm tra người dùng đã mượn sách này hay chưa, hoặc đã đặt trước sách này hay chưa
                // never happen // pageload got it
                if (borBook.state == 2)
                {
                    message.Add("Bạn đã mượn trước sách này rồi !");
                }
            }



            if (listLocation.SelectedIndex == -1)
            {
                message.Add("Hãy chọn địa điểm!");
                errorBorrow.DataSource = message;
                errorBorrow.DataBind();
                return;
            }


            int lcId = int.Parse(listLocation.SelectedValue);

            // Kiểm tra số lượng sách người dùng đã đặt & mượn
            if (curUser.borBooks.Count == Book.limitBorBook)
            {
                message.Add("Bạn đã đặt/mượn giới hạn " + Book.limitBorBook + " sách !");
                errorBorrow.DataSource = message;
                errorBorrow.DataBind();
            }
            else
            {
                // Kiểm tra số lượng sách còn lại
                BorBook tmp = db.BorBooks.FirstOrDefault(bb => bb.BookId == curBook.bookId && bb.state == 0 && bb.LocationId == lcId);

                if (tmp != null)
                {
                    // nếu có sách cho người dùng mượn và chuyển sách 
                    tmp.User = curUser;
                    tmp.state = 1;
                    curUser.borBooks.Add(tmp);
                    message.Add("Đặt mượn trước thành công!");
                    db.SaveChanges();
                    locationLabel.Visible = false;
                    listLocation.Visible = false;

                    // ***  cập nhật CurBook vì borBook ở trên vừa thay đổi - borBook là BorBook của curBook
                    curBook = db.Books.FirstOrDefault(book => book.bookId == curBook.bookId);

                    // ĐẶT MƯỢN THÀNH CÔNG => Cập nhật danh sách BorBook

                    updateBorBook();
                    parseLocation();

                    borrowBtn.Text = "Hủy đặt trước";
                    borrowBtn.Style.Add("background-color", "red !important");
                    //red!important;
                    borrowBtn.DataBind();

                }
                else
                {
                    message.Add("Xin lỗi, sách này đã được mượn hết!");
                }
            }

            errorBorrow.DataSource = message;
            errorBorrow.DataBind();

        }

        protected void removeBtn_Click(object sender, EventArgs e)
        {
            User curAdmin = (User)Session["user"];
            // cập nhật curBook
            curBook = db.Books.FirstOrDefault(b => b.bookId == curBook.bookId);
            List<BorBook> removebb = curBook.BorBooks.Where(b => b.LocationId == curAdmin.LocationId).ToList();
            for (int i = 0; i < removebb.Count; i++) db.BorBooks.Remove(removebb[i]);
            if (curBook.BorBooks.Count == 0) db.Books.Remove(curBook);
            db.SaveChanges();
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
    class LocationInstance
    {
        public int id { get; set; }
        public string locationInfo { get; set; }
    }
}