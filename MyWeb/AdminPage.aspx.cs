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
    public partial class ListUser : System.Web.UI.Page
    {
        private LibraryContext db = new LibraryContext();
        private List<User> listUser = new List<User>();
        private List<Book> listBook = new List<Book>();
        public string userName = "";
        private List<string> listUserName;
        protected void Page_Load(object sender, EventArgs e)
        {

            // check permission
            if ( Session["userName"]==null || !UserLogic.isAdmin(Session["userName"].ToString()) ) Response.Redirect("NoPermisson.html");
            // greating
            if (Session["userName"]!=null) userName ="Hello "+ Session["userName"].ToString();

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

                // LoadList User vì ViewUser là view mặc định
                listBoxUser.DataSource = listUserName;
                listBoxUser.DataBind();

                //setup type cho trường chọn sách mượn
                listBorBook.ItemType = "BookItem";
                listBorBook.DataTextField = "name";
                listBorBook.DataValueField = "id";
                
                // Danh sách sách dang mượn của người dùng đầu tiên hiển thị đầu tiên trên màn hình
                string tmpUserName = listUserName[0];
                    if (db.Users.FirstOrDefault(u => u.userName == tmpUserName).borBooks != null)
                    {
                        userListBorBook.DataSource = db.Users
                            .FirstOrDefault(u => u.userName == tmpUserName)
                            .borBooks.Select(
                                bb => new BorBookItem
                                {
                                    id = bb.id,
                                    name = db.Books.FirstOrDefault(b => b.bookId == bb.BookId).bookName,
                                }
                            ).ToList();
                        userListBorBook.DataBind();
                    }
            }
            else
            {
            }

            // Sinh dữ liệu mẫu để test
            // sinh 100  quyển chỉ cần khác tên, các cái còn lại có thể giống nhau
            //genRandomBook(80);
            previewUserBookPic.ImageUrl = "";


        }


        //sinh book mẫu
        public void genRandomBook(int num)
        {
            LibraryContext db = new LibraryContext();
            int fId = db.Books.Max(b => b.bookId)+1;
            Random rand = new Random();
            for (int i = 0; i < num; i++)
            {
                Book tmp = new Book
                {
                    bookId = fId++, // Id của sách sẽ +1 so với id  lớn nhất của các sách hiện tại
                    bookName = genRandomString(ref rand,8),
                    amount = "10",
                    imagePath = "LaoSomeThing.png",
                    price = "150000",
                };
                db.Books.Add(tmp);
                db.SaveChanges();
                // Sinh các borrowable cho book
                BookLogic.genBorBook(tmp);
                db.SaveChanges();
            }
        }
        // sinh tên ngẫu nhiên
        public string genRandomString(ref Random rand,int len)
        {
            string res = "";
            for (int i = 0; i < len; i++) res += (char)(rand.Next(97, 122));
            return res;
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
                role = inpRole.SelectedValue
            };

            ValidationContext ctx = new ValidationContext(tmp,serviceProvider:null,items:null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(tmp, ctx, results, true);

            var messages = results.Select(res => res.ErrorMessage.ToString()).ToList();
            if (isValid)
            {
                LibraryContext db = new LibraryContext();
                var exist = db.Users.Any(u => u.userName==inpUserName.Text);
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
                    bookId = db.Books.Max(b => b.bookId)+1, // Id của sách sẽ +1 so với id  lớn nhất của các sách hiện tại
                    bookName = BookName.Text,
                    category = BookCategory.Text,
                    description = BookDescription.Text,
                    amount= Amount.Text,
                    imagePath = ImageUpload.FileName,
                    price = BookPrice.Text,

                };
                if (BookLogic.isValid(tmp,ref messages))
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
                    BookLogic.genBorBook(tmp);
                        
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

            previewPicBook.ImageUrl = "/Images/"+book.imagePath;
            previewPicBook.DataBind();
        }
 
        protected void borrowBtn_Click(object sender, EventArgs e)
        {
            LibraryContext db = new LibraryContext();
            List<String> message = new List<string>();
            if (listBorUser.SelectedValue == "" || listBorBook.SelectedValue == "") return;
            string userName = listBorUser.SelectedValue;
            int bookId = int.Parse(listBorBook.SelectedValue);

            User user = db.Users.FirstOrDefault(u => u.userName == userName);
            Book curBook = db.Books.FirstOrDefault(book => book.bookId == bookId);

            // Kiểm tra đã mượn sách này hay chưa !
            if (user.borBooks.Any(bb => bb.BookId == curBook.bookId))
                message.Add("Bạn đã mượn sách này rồi !");

            // kiểm  tra số lượng sách người dùng đang mượn !
            if (user.borBooks.Count > 20)
                message.Add("Bạn đã mượn 20 quyển sách, hãy trả sách để có thể mượn thêm sách !");

            //danh sách sách còn lại của sách này
            List<BorBook> avableBorBook = db.BorBooks.Where(bb => bb.state == 0).ToList();
            

            // Những sách thuộc loại sách đã chọn
            avableBorBook = avableBorBook.Where(bb => bb.BookId == curBook.bookId).ToList();

            // nếu nó rỗng tức không còn loại sách này
            if (avableBorBook.Count == 0)
                message.Add("Xin lỗi sách này đã hết !");
            // trường hợp người dùng đã đặt trước ?? th đặc biệt
            BorBook tmp = user.borBooks.FirstOrDefault(bb => bb.BookId == bookId&&bb.state==1);
            if (tmp!=null)
            {
                // không cần xét dk số lượng sách đã mượn vì, k tăng thêm chỉ chỉnh sách's state lên 2
                message.Clear();
                tmp.state = 2;
                int day = int.Parse(returnDate.SelectedValue);
                tmp.returnDate = DateTime.Now.AddDays(day * 7);
                db.SaveChanges();

                message.Add("Người dùng này đã mượn trước, trạng thái sách chuyển thành 'đang được mượn' !");
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
                BB.returnDate = DateTime.Now.AddDays(day * 7);
                // set state =3 
                BB.state = 2;
                BB.Book = curBook;
                user.borBooks.Add(BB);
                message.Add("Sách được mượn bởi tài khoản admin, trạng thái sách chuyển thành 'đang được mượn' !");
                db.SaveChanges();
            }
            // Gán và hiển thị lỗi - tin nhắn cho người dùng
            borrowMessages.DataSource = message;
            borrowMessages.DataBind();
        }



        protected void viewListUser_Click(object sender, EventArgs e)
        {
            // LOAD LAI danh sach user
            listBoxUser.DataSource = listUserName;
            listBoxUser.DataBind();
            // Chuyển sang trang ListUser
            inforMView.ActiveViewIndex = 0;
            preMView.ActiveViewIndex = 0;
        }

        protected void viewAddUser_Click(object sender, EventArgs e)
        {
            inforMView.ActiveViewIndex = 1;
            preMView.ActiveViewIndex = -1;
        }

        protected void viewNewBook_Click(object sender, EventArgs e)
        {
            inforMView.ActiveViewIndex = 2;
            preMView.ActiveViewIndex = -1;
        }

        protected void viewBorBook_Click(object sender, EventArgs e)
        {
            listBorUser.DataSource = listUserName.ToList();
            listBorUser.DataBind();
            listBorBook.DataSource = db.Books.Select(
                                        book=> new BookItem
                                        {
                                            id = book.bookId,
                                            name = book.bookName
                                       }).ToList();
            listBorBook.DataBind();
            //Load lại 
            inforMView.ActiveViewIndex = 3;
            preMView.ActiveViewIndex = 1;
        }
        
        protected void viewReturnBook_Click(object sender, EventArgs e)
        {
            inforMView.ActiveViewIndex = 4;
            preMView.ActiveViewIndex = 1;
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
            var username = listBoxUser.SelectedValue;
            User user = db.Users.FirstOrDefault(u=>u.userName==username);
            if (user == null) return;
            accName.Text = user.userName;
            borBookCount.Text = user.borBooks.Count.ToString();
            // load danh sachs sachs muon
            if (user.borBooks != null && user.borBooks.Count > 0)
            {
                userListBorBook.DataSource = user.borBooks
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
                userListBorBook.DataSource = new List<BorBookItem>() ;
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
                BorBook BB = db.BorBooks.FirstOrDefault(b => b.id == BBid );
                if (BB == null) return;
                user.borBooks.Remove(BB);
                BB.state = 0;
                BB.User = null;
                db.SaveChanges();
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
        }
        private void reloadUserInfo()
        {
            string username = listBoxUser.SelectedValue;
            var user = db.Users.FirstOrDefault(u => u.userName == username);
            if (user == null) {
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
            }
            
        }
        protected void removeUserBtn_Click(object sender, EventArgs e)
        {
            string username = listBoxUser.SelectedValue;
            var u = db.Users.FirstOrDefault(user => user.userName == username);
            if (u == null) return;
            u.borBooks.Clear();
            db.Users.Remove(u);
            db.SaveChanges();

            // load lai
            listUserName.Remove(username);
            listBoxUser.DataSource = listUserName;
            listBoxUser.DataBind();
            reloadUserInfo();
        }

        // PreView Book trong ***LISTUSER ***
        protected void userListBorBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userListBorBook.SelectedValue == "") return;

            int idBorBook = int.Parse(userListBorBook.SelectedValue);
            // chắc chắn có
            BorBook borBook = db.BorBooks.FirstOrDefault(bb=>bb.id==idBorBook);
            Book book = db.Books.FirstOrDefault(b => b.bookId == borBook.BookId);
            previewUserBookPic.ImageUrl = "Images/" + book.imagePath;
            reloadUserInfo();
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

            User tmp = new User()
            {
                userName = curUser.userName,
                realName = realName.Text,
                CMND = CMND.Text,
                dchi = dchi.Text,
                passWord = passWord.Text,
                role=curUser.role,
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
            if (listBoxUser.SelectedValue == ""|| listBoxUser.SelectedValue==null)
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
    }
    class BookItem
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    class BorBookItem
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}