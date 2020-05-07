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
    public partial class WebForm1 : System.Web.UI.Page
    {
        private LibraryContext db = new LibraryContext();
        private List<User> listUser = new List<User>();
        private List<Book> listBook = new List<Book>();
        public string username = ""; // để hiển bên client
        private List<string> listUserName;
        private User curAdmin;
        protected void Page_Load(object sender, EventArgs e)
        {

            // regiter for js in clientside
            Page.ClientScript.RegisterClientScriptInclude("AdminPage", "Script/AdminPage.js");

            // kiểm tra quyền ad min
            curAdmin = (User)Session["user"];
            if (curAdmin == null || curAdmin.role != "admin") Response.Redirect("NoPermisson.html");


            // load view đầu tiên - mặc định

            locationInfo.Text = "Địa điểm :" + curAdmin.Location.dchi;

            borrowBook_locationInfo.Text = "Địa điểm :" + curAdmin.Location.dchi;
            // take List User
            listUser = db.Users.ToList();

            // Load lại dùng lại ở view borrowBook và view listUser
            listUserName = db.Users.Select(user => user.userName).ToList();

            // Danh sách sách dang mượn của người dùng
            //Setup cho listbook
            userListBorBook.ItemType = "BorBookItem";
            userListBorBook.DataTextField = "name";
            userListBorBook.DataValueField = "id";


            if (!IsPostBack)
            {

                // FIRST LOAD

                // Kiểm  tra  có phải là quay lại từ việc xóa hay k ?
                if (Session["RemoveBorBookBacking"] == "true")
                {
                    // chuyển view
                    inforMView.ActiveViewIndex = 2;
                    preMView.ActiveViewIndex = 2;
                    // load lại sách
                    addBook_locationInfo.Text = "Địa điểm :" + curAdmin.Location.dchi;
                    addBorBookList.ItemType = "BookItem";
                    addBorBookList.DataTextField = "name";
                    addBorBookList.DataValueField = "id";

                    var tmp = db.Books.Select(
                            b => new BookItem
                            {
                                id = b.bookId,
                                name = b.bookName,
                            }
                            ).ToList();
                    addBorBookList.DataSource = tmp;
                    //addBorBookList.SelectedValue = Session["BookId"];

                    int idx = -1;
                    for (int i = 0; i < tmp.Count; i++)
                    {
                        if (tmp[i].id == int.Parse(Session["RemoveBorBookBacking_BookId"].ToString()))
                        {
                            idx = i;
                            break;
                        }
                    }
                    addBorBookList.DataBind();
                    addBorBookList.SelectedIndex = idx;
                    // init Table BorBook
                    viewAddBook_reloadBorBookTable();
                    Session["RemoveBorBookBacking"] = "false";
                }


                // LoadList User vì ViewUser là view mặc định
                listBoxUser.DataSource = listUserName;
                listBoxUser.DataBind();

                //setup type cho trường chọn sách mượn
                listBorBook.ItemType = "BookItem";
                listBorBook.DataTextField = "name";
                listBorBook.DataValueField = "id";


            }
            else
            {
            }
            previewUserBookPic.ImageUrl = "";
        }


        // XỬ LÝ CỬA SỔ
        protected void viewListUser_Click(object sender, EventArgs e)
        {

            // LOAD LAI danh sach user
            listBoxUser.DataSource = listUserName;
            listBoxUser.DataBind();
            // Chuyển sang trang ListUser
            inforMView.ActiveViewIndex = 0;
            preMView.ActiveViewIndex = 0;

            // Xóa dữ liệu tại danh sách "sách đang mượn" do ViewState để lại
            // Tránh việc qua lại các trang dữ  liệu vẫn còn
            userListBorBook.DataSource = new List<BorBook>(); // để null hình như vẫn được xét là ViewState
            userListBorBook.DataBind();

        }

        protected void viewAddUser_Click(object sender, EventArgs e)
        {
            inforMView.ActiveViewIndex = 1;
            preMView.ActiveViewIndex = -1;
        }

        protected void viewNewBook_Click(object sender, EventArgs e)
        {
            inforMView.ActiveViewIndex = 2;
            preMView.ActiveViewIndex = 2;

            // Khởi tạo preMView - bên phải
            // danh sách Book - nên đổi lại tên
            addBook_locationInfo.Text = "Địa điểm :" + curAdmin.Location.dchi;
            addBorBookList.ItemType = "BookItem";
            addBorBookList.DataTextField = "name";
            addBorBookList.DataValueField = "id";
            addBorBookList.DataSource = db.Books.Select(
                    b => new BookItem
                    {
                        id = b.bookId,
                        name = b.bookName,
                    }
                    ).ToList();
            addBorBookList.DataBind();
            // khởi tạo bảng bên phải
            //removeBorBookTable.Rows.Add();
            //initremoveBorBookTable(ref removeBorBookTable);

        }
        // thêm header cơ bản cho remove Bor Book talbe
        private void initremoveBorBookTable(ref Table table)
        {
            TableHeaderCell[] cellsHeader = new TableHeaderCell[6];
            for (int i = 0; i < 6; i++) cellsHeader[i] = new TableHeaderCell();

            cellsHeader[0].Text = "Id";
            cellsHeader[1].Text = "State";
            cellsHeader[2].Text = "Người mượn/đặt";
            cellsHeader[3].Text = "Ngày mượn";
            cellsHeader[4].Text = "Hạn trả";

            var headerRow = new TableHeaderRow();
            for (int i = 0; i < 6; i++) headerRow.Cells.Add(cellsHeader[i]);

            table.Rows.Add(headerRow);

            table.DataBind();
        }

        protected void viewBorBook_Click(object sender, EventArgs e)
        {
            listBorUser.DataSource = listUserName.ToList();
            listBorUser.DataBind();
            //Thêm đuôi đã hết cho những sách đã hết số lượng(borbook) ở location hiện tại
            listBorBook.DataSource = addTailsBorrowableBooks();
            listBorBook.DataBind();
            //Load lại 
            inforMView.ActiveViewIndex = 3;
            preMView.ActiveViewIndex = 1;
        }

        // thêm đuôi cho những sách ở địa điểm (location) hiện tại
        private List<BookItem> addTailsBorrowableBooks()
        {
            var listBorrowableBook = new List<BookItem>();

            // sách có ở thư viện(Locaiton) hiện tại
            // Cập nhật  lại admin vì borbooks đã thay đổi lí do curAdmin không thay đổi theo
            // vì nó được tạo ra ở context khác
            curAdmin = db.Users.FirstOrDefault(user => user.userName == curAdmin.userName);
            var tmp = curAdmin.Location.BorBooks;
            // Tất cả các sách
            var listBook = db.Books.ToList();

            for (int i = 0; i < listBook.Count; i++)
            {
                string tails = "";
                // nếu thử viện (location) hiện tại không còn loại sách này
                int c = tmp.Count(bb => bb.BookId == listBook[i].bookId && bb.state == 0);
                if (c == 0)
                {
                    tails = " [Đã hết]";
                }
                else
                {
                    tails = "[ " + c + "/" + tmp.Count(bb => bb.BookId == listBook[i].bookId) + "] ";
                }
                listBorrowableBook.Add(
                        new BookItem
                        {
                            id = listBook[i].bookId,
                            name = listBook[i].bookName + tails,
                            order = false

                        }
                    );
            }

            return listBorrowableBook;
        }
        protected void viewReturnBook_Click(object sender, EventArgs e)
        {
            inforMView.ActiveViewIndex = 4;
            preMView.ActiveViewIndex = 1;
        }

        protected void addUserBnt_Click(object sender, EventArgs e)
        {
            //using System.ComponentModel.DataAnnotations;
            //Response.Write("IT'S GOOD\n");
            User tmp = new User()
            {
                userName = inpUserName.Text,
                passWord = inpPassWord.Text,
                realName = inpRealName.Text,
                CMND = inpCMND.Text,
                dchi = inpDchi.Text,
                role = inpRole.SelectedValue,
                LocationId = curAdmin.LocationId

            };

            ValidationContext ctx = new ValidationContext(tmp, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(tmp, ctx, results, true);

            var messages = results.Select(res => res.ErrorMessage.ToString()).ToList();
            if (isValid)
            {
                LibraryContext db = new LibraryContext();
                var exist = db.Users.Any(u => u.userName == inpUserName.Text);
                if (!exist)
                {
                    db.Users.Add(tmp);
                    db.SaveChanges();
                    messages.Add("Thêm User thành công!");
                    listUser = db.Users.ToList();
                }
                else
                {
                    messages.Add("Username đã tồn tại!");
                }

            }
            validUserErrors.DataSource = messages;
            validUserErrors.DataBind();
            // Không hiển thị preview
            preMView.ActiveViewIndex = -1;
        }

        protected void addBookBnt_Click(object sender, EventArgs e)
        {
            var messages = new List<String>();
            if (ImageUpload.HasFile)
            {
                LibraryContext db = new LibraryContext();
                Book tmp = new Book
                {
                    bookId = db.Books.Max(b => b.bookId) + 1, // Id của sách sẽ +1 so với id  lớn nhất của các sách hiện tại
                    bookName = BookName.Text,
                    category = BookCategory.Text,
                    description = BookDescription.Text,
                    amount = Amount.Text,
                    imagePath = ImageUpload.FileName,
                    price = BookPrice.Text,

                };
                if (Book.isValid(tmp, ref messages))
                {
                    try
                    {
                        String path = Server.MapPath("~/Images/");
                        ImageUpload.PostedFile.SaveAs(path + ImageUpload.FileName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    db.Books.Add(tmp);
                    db.SaveChanges();
                    // Sinh các borrowable cho book
                    Book.genBorBook(tmp);

                    db.SaveChanges();
                    messages.Add("Thêm thành công!");
                }
            }
            else
            {
                messages.Add("Thêm ảnh");

            }
            validBookErrors.DataSource = messages;
            validBookErrors.DataBind();
        }
        private void FillPreview(Book book)
        {
            previewNameBook.Text = book.bookName;
            previewNameBook.DataBind();

            previewCategoryBook.Text = book.category;
            previewCategoryBook.DataBind();

            previewDecriptionBook.Text = book.description;
            previewDecriptionBook.DataBind();

            previewPriceBook.Text = book.price;
            previewPriceBook.DataBind();

            previewPicBook.ImageUrl = "/Images/" + book.imagePath;
            previewPicBook.DataBind();
        }

        protected void borrowBtn_Click(object sender, EventArgs e)
        {
            List<String> message = new List<string>();

            if (listBorUser.SelectedValue == "")
            {
                message.Add("Hãy chọn người mượn!");
                borrowMessages.DataSource = message;
                borrowMessages.DataBind();
                return;
            }
            if (listBorBook.SelectedValue == "")
            {
                message.Add("Hãy chọn sách mượn!");
                borrowMessages.DataSource = message;
                borrowMessages.DataBind();
                return;
            }

            string userName = listBorUser.SelectedValue;

            int bookId = int.Parse(listBorBook.SelectedValue);

            User user = db.Users.FirstOrDefault(u => u.userName == userName);
            Book curBook = db.Books.FirstOrDefault(book => book.bookId == bookId);

            // Kiểm tra đã mượn sách này hay chưa !
            if (user.borBooks.Any(bb => bb.BookId == curBook.bookId))
                message.Add("Bạn đã mượn sách này rồi !");

            // kiểm  tra số lượng sách người dùng đang mượn !
            int borrowedBook = user.borBooks.Count(bb => bb.state == 2);
            if (borrowedBook == Book.limitBorBook)
                message.Add("Bạn đã mượn " + borrowedBook + "quyển sách, hãy trả sách để có thể mượn thêm sách !");

            //danh sách sách còn lại của sách này
            // chỉ được tính sách ở location của Admin đang lưu trữ

            List<BorBook> avableBorBook = db.BorBooks
                        .Where(bb => bb.state == 0
                                            &&
                                     bb.BookId == curBook.bookId
                                            &&
                                    bb.Location.id == curAdmin.Location.id
                                    ).ToList();

            // nếu nó rỗng tức không còn loại sách này
            if (avableBorBook.Count == 0)
                message.Add("Xin lỗi sách này đã hết !");

            // trường hợp người dùng đã đặt trước ?? th đặc biệt
            BorBook tmp = user.borBooks.FirstOrDefault(bb => bb.BookId == bookId && bb.state == 1);
            if (tmp != null)
            {
                // không cần xét dk số lượng sách đã mượn vì, k tăng thêm chỉ chỉnh sách's state lên 2
                message.Clear();
                tmp.state = 2;
                int day = int.Parse(returnDate.SelectedValue);
                tmp.borrowDate = DateTime.Now;
                tmp.returnDate = tmp.borrowDate.AddDays(day * 7);

                db.SaveChanges();

                message.Add("[Người dùng đã mượn trước], trạng thái sách chuyển thành 'đang được mượn' !");

                // cập nhật lại danh sách book
                reloadBorBook(ref user);

                borrowMessages.DataSource = message;
                borrowMessages.DataBind();
                return;
            }

            // Nếu không gặp 2 lỗi trên cho phép mượn
            if (message.Count == 0)
            {
                BorBook BB = avableBorBook[0];

                //BB = db.BorBooks.FirstOrDefault(bb => bb.id == BB.id);

                int day = int.Parse(returnDate.SelectedValue);
                BB.borrowDate = DateTime.Now;
                BB.returnDate = BB.borrowDate.AddDays(day * 7);
                // set state =3 
                BB.state = 2;
                BB.Book = curBook;
                user.borBooks.Add(BB);
                message.Add("Sách được mượn bởi tài khoản admin, trạng thái sách chuyển thành 'đang được mượn' !");
                db.SaveChanges();
                reloadBorBook(ref user);
            }
            // Gán và hiển thị lỗi - tin nhắn cho người dùng
            borrowMessages.DataSource = message;
            borrowMessages.DataBind();
        }


        protected void listBorBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Load thông tin qua preview Book
            int id = int.Parse(listBorBook.SelectedValue);
            Book book = db.Books.FirstOrDefault(b => b.bookId == id); ;
            FillPreview(book);
        }

        protected void listBoxUser_SelectedIndexChanged(object sender, EventArgs e)
        {

            // disable editing các trường dữ liệu! tránh trường hợp đang edit dở ở một trường user này, switch qua user khác các trường vấn 
            realName.Enabled = false;
            CMND.Enabled = false;
            dchi.Enabled = false;
            passWord.Enabled = false;

            listUserMessages.DataSource = new List<string>();
            listUserMessages.DataBind();


            var username = listBoxUser.SelectedValue;
            User user = db.Users.FirstOrDefault(u => u.userName == username);
            if (user == null) return;
            accName.Text = user.userName;
            borBookCount.Text = user.borBooks.Count.ToString();
            // load danh sachs sachs muon
            if (user.borBooks != null && user.borBooks.Count > 0)
            {
                userListBorBook.DataSource = user.borBooks
                    .Where(bb => bb.state == 2)
                    .Select(bb =>
                            new BorBookItem
                            {
                                id = bb.id,
                                name = bb.Book.bookName
                                //name = db.Books.FirstOrDefault(b => b.bookId == bb.BookId).bookName
                            }

                    ).ToList();
            }
            else
            {
                userListBorBook.DataSource = new List<BorBookItem>();
            }
            userListBorBook.DataBind();
            // load lại các trường thông tin
            reloadUserInfo();

        }


        protected void returnBookBtn_Click(object sender, EventArgs e)
        {
            if (userListBorBook.SelectedValue != null)
            {
                string username = listBoxUser.SelectedValue;
                var user = db.Users.FirstOrDefault(u => u.userName == username);
                if (user == null) return;
                int BBid = int.Parse(userListBorBook.SelectedValue);
                BorBook BB = db.BorBooks.FirstOrDefault(b => b.id == BBid);
                if (BB == null) return;
                user.borBooks.Remove(BB);

                BB.state = 0;
                BB.User = null;

                // Kiểm tra borBook ở location nào
                if (BB.LocationId != curAdmin.LocationId)
                {
                    // Người dùng trả sách ở nơi khác
                    db.Locations.FirstOrDefault(lc => lc.id == BB.LocationId).BorBooks.Remove(BB);
                    db.Locations.FirstOrDefault(lc => lc.id == curAdmin.LocationId).BorBooks.Add(BB);
                }

                db.SaveChanges();
                returnBookBtn.Enabled = false;
                //load lai danh sách sách người dùng mượn
                if (user.borBooks.Count > 0 && user.borBooks != null)
                {
                    userListBorBook.DataSource = user.borBooks.Select(
                            bb => new BorBookItem
                            {
                                id = bb.id,
                                name = db.Books.FirstOrDefault(b => b.bookId == bb.BookId).bookName,
                            }
                        ).ToList();
                }
                else
                {
                    userListBorBook.DataSource = new List<BorBookItem>();
                }

                userListBorBook.DataBind();
            }
            reloadUserInfo();
        }

        // Fill lại các trường in4 của người dùng
        private void reloadUserInfo()
        {
            string username = listBoxUser.SelectedValue;
            var user = db.Users.FirstOrDefault(u => u.userName == username);
            if (user == null)
            {
                accName.Text = "";
                realName.Text = "";
                CMND.Text = "";
                dchi.Text = "";
                passWord.Text = "";
                return;
            }
            else
            {

                accName.Text = user.userName;
                realName.Text = user.realName;
                CMND.Text = user.CMND;
                dchi.Text = user.dchi;
                passWord.Text = "";
                borBookCount.Text = user.borBooks.Count(bb => bb.state == 2).ToString();
            }
        }
        protected void removeUserBtn_Click(object sender, EventArgs e)
        {
            string username = listBoxUser.SelectedValue;
            List<string> messages = new List<string>();
            var u = db.Users.FirstOrDefault(user => user.userName == username);
            if (u == null)
            {
                messages.Add("Hãy chọn User");
                listUserMessages.DataSource = messages;
                listUserMessages.DataBind();
                return;

            }

            if (u.borBooks.Count(bb => bb.state == 2) > 0)
            {
                messages.Add("User còn sách chưa trả,  hãy trả hết sách user mượn trước khi xóa user");
                listUserMessages.DataSource = messages;
                listUserMessages.DataBind();
                return;
            }


            db.Users.Remove(u);
            db.SaveChanges();

            // load lai
            u.borBooks.Clear(); // remove các sách state  = 1; hay đặt mượn
            listUserName.Remove(username);
            listBoxUser.DataSource = listUserName;
            listBoxUser.DataBind();
            reloadUserInfo();
        }

        // PreView Book trong ***LISTUSER ***
        protected void userListBorBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnBookBtn.Enabled = true;
            if (userListBorBook.SelectedValue == "") return;
            int idBorBook = int.Parse(userListBorBook.SelectedValue);
            // chắc chắn có
            BorBook borBook = db.BorBooks.FirstOrDefault(bb => bb.id == idBorBook);
            Book book = db.Books.FirstOrDefault(b => b.bookId == borBook.BookId);
            previewUserBookPic.ImageUrl = "Images/" + book.imagePath;
            reloadUserInfo();
            // Load lại danh sách "sách đang mượn" của người dùng
        }

        protected void saveUser_Click(object sender, EventArgs e)
        {
            if (realName.Enabled == false) return;
            List<string> messages = new List<string>();
            if (listBoxUser.SelectedValue == "" || listBoxUser.SelectedValue == null)
            {
                messages.Add("Hãy chọn user");
                validationUserError.DataSource = messages;
                validationUserError.DataBind();
                return;
            }
            var curUser = db.Users.FirstOrDefault(u => u.userName == listBoxUser.SelectedValue);
            if (curUser == null) return;

            string tmpPass = passWord.Text;

            if (passWord.Text == "") tmpPass = curUser.passWord;

            User tmp = new User()
            {
                userName = curUser.userName,
                realName = realName.Text,
                CMND = CMND.Text,
                dchi = dchi.Text,
                passWord = passWord.Text,
                role = curUser.role,
            };

            ValidationContext ctx = new ValidationContext(tmp, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(tmp, ctx, results, true);
            messages = results.Select(res => res.ErrorMessage.ToString()).ToList();
            if (isValid)
            {
                LibraryContext db = new LibraryContext();
                var user = db.Users.SingleOrDefault(u => u.userName == tmp.userName);
                if (user != null)
                {
                    user.realName = tmp.realName;
                    user.CMND = tmp.CMND;
                    user.dchi = tmp.dchi;
                    user.passWord = tmp.passWord;
                    db.SaveChanges();
                    Response.Redirect("AdminPage.aspx");
                }

            }
            validationUserError.DataSource = messages; // vì không gian hiển thị ít
            validationUserError.DataBind();
            reloadUserInfo();
            realName.DataBind();

        }

        protected void editUser_Click(object sender, EventArgs e)
        {
            List<string> messages = new List<string>();
            if (listBoxUser.SelectedValue == "" || listBoxUser.SelectedValue == null)
            {
                messages.Add("Hãy chọn user");
                validationUserError.DataSource = messages;
                validationUserError.DataBind();
                return;
            }

            realName.Enabled = true;
            CMND.Enabled = true;
            dchi.Enabled = true;
            passWord.Enabled = true;
        }

        protected void listBorUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cập nhật danh sách book, thêm đuôi( đã đặt trước ) khi người dùng đã có đặt trước;

            LibraryContext db = new LibraryContext();

            List<String> message = new List<string>();
            if (listBorUser.SelectedValue == "") return;

            string userName = listBorUser.SelectedValue;

            User user = db.Users.FirstOrDefault(u => u.userName == userName);
            // thêm đuôi [Đã đặt trước] hoặc [Đã mượn[ơ
            reloadBorBook(ref user);
        }

        private void reloadBorBook(ref User user)
        {
            // thêm đuôi [Đã hết] cho những sách đã hết ở thư viện
            List<BookItem> tmp = addTailsBorrowableBooks();

            // thêm đuôi [ Đã đặt trước ]  cho những sách đã mà người dùng đã đặt trước
            for (int i = 0; i < tmp.Count; i++)
            {
                if (user.borBooks.Any(bb => bb.BookId == tmp[i].id && bb.state == 2))
                    tmp[i].name += " [ Đã mượn ]";
                if (user.borBooks.Any(bb => bb.BookId == tmp[i].id && bb.state == 1))
                {
                    tmp[i].name += " [ Đã đặt trước ] ";
                    tmp[i].order = true;
                }
            }
            listBorBook.DataSource = tmp.OrderBy(bi => !bi.order);
            listBorBook.DataBind();

        }

        protected void addBorBookList_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewAddBook_reloadBorBookTable();
            // for backing when remove borBook
            Session["RemoveBorBookBacking_BookId"] = addBorBookList.SelectedValue;
        }

        // load remove bor book table theo  sách được chọn ở addBorBookList
        private void viewAddBook_reloadBorBookTable()
        {
            initremoveBorBookTable(ref removeBorBookTable);
            int bookId = int.Parse(addBorBookList.SelectedValue);
            var locationBorBooks = db.BorBooks.Where(bb => bb.Book.bookId == bookId && bb.LocationId == curAdmin.LocationId).ToList();
            for (int i = 0; i < locationBorBooks.Count; i++)
            {
                TableCell[] cells = new TableCell[6];
                for (int j = 0; j < 6; j++) cells[j] = new TableCell();

                cells[0].Text = locationBorBooks[i].id.ToString();
                cells[1].Text = (locationBorBooks[i].state == 0) ? "Có sẵn" : (locationBorBooks[i].state == 1) ? "Có người đặt" : "Đã mượn";
                if (locationBorBooks[i].state >= 1)
                    cells[2].Text = locationBorBooks[i].User.userName;
                if (locationBorBooks[i].state != 0)
                    cells[3].Text = locationBorBooks[i].borrowDate.ToString("dd'/'MM'/'yyyy");
                if (locationBorBooks[i].state == 2)
                    cells[4].Text = locationBorBooks[i].returnDate.ToString("dd'/'MM'/'yyyy");
                if (locationBorBooks[i].state != 2)
                    cells[5].Text = @"<a href='RemoveBorBook.aspx?idBorBook=" + locationBorBooks[i].id.ToString() + "'>Xóa</a>";
                var row = new TableRow();
                for (int j = 0; j < 6; j++) row.Cells.Add(cells[j]);
                removeBorBookTable.Rows.Add(row);
            }
            removeBorBookTable.DataBind();
        }

        protected void addMoreBorBook_Click(object sender, EventArgs e)
        {
            preMView.ActiveViewIndex = 2;
            var messenger = new List<string>();
            if (addBorBookList.SelectedIndex != -1)
            {
                int bookId = int.Parse(addBorBookList.SelectedValue);
                Book book = db.Books.FirstOrDefault(b => b.bookId == bookId);


                int amout;
                if (int.TryParse(addingAmount.Text, out amout))
                {
                    if (amout > 0)
                    {
                        var tmp = curAdmin.Location;
                        Book.genBorBook(ref book, amout, ref tmp);
                        messenger.Add("Thêm thành công");
                        db.SaveChanges();
                        // load lại table 
                        viewAddBook_reloadBorBookTable();
                    }
                    else
                    {
                        messenger.Add("Số lượng >= 0");
                    }

                }
                else
                {
                    messenger.Add("Số lượng phải là số !");
                }
            }
            else
            {
                messenger.Add("Chọn sách");
                // thông báo chọn sách
            }
            addBorBookMessage.DataSource = messenger;
            addBorBookMessage.DataBind();
        }
    
    class BookItem
    {
        public int id { get; set; }
        public string name { get; set; }

        // để sắp xếp các sách đã đặt lên trên
        public bool order { get; set; }
    }
    class BorBookItem
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}
}