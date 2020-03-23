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
        public List<User> listUser = new List<User>();
        public List<Book> listBook = new List<Book>();
        public string userName = "";
        private List<string> listUserName;
        protected void Page_Load(object sender, EventArgs e)
        {

            // Script Client side
            

            // check permission
            if ( Session["userName"]==null || !UserLogic.isAdmin(Session["userName"].ToString()) ) Response.Redirect("NoPermisson.html");
            // greating
            if (Session["userName"]!=null) userName ="Hello "+ Session["userName"].ToString();
            // take List User
            
            listUser = db.Users.ToList();
            // take List Book

            // Load lại dùng lại ở view borrowBook và view listUser
            listUserName = db.Users.Select(user => user.userName).ToList();


            if (!IsPostBack)
            {
                // FIRST LOAD
                listBoxUser.DataSource = listUserName;
                listBoxUser.DataBind();

                string tmpUserName = listUserName[0];
                // Danh sach sach dang muon cua nguoi dung
                if (db.Users.FirstOrDefault(u => u.userName== tmpUserName).borBooks != null)
                {
                    returnDropListBook.DataSource = db.Users
                        .FirstOrDefault(u => u.userName == tmpUserName)
                        .borBooks.Select(
                            // từ  danh sách "những sách đang mượn của người dùng" thông qua db.Books để lấy tên
                            bb => db.Books.FirstOrDefault(b => b.bookId == bb.BookId).bookName
                        ).ToList();
                    returnDropListBook.DataBind();
                }

            }
            else
            {
            }
            // Sinh dữ liệu mẫu để test
            // sinh 100  quyển chỉ cần khác tên, các cái còn lại có thể giống nhau
            //genRandomBook(80);

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
                    
                    listUser = db.Users.ToList();
                }
                else
                {
                    messages.Add("Username đã tồn tại!");
                }
                
            }
            validUserErrors.DataSource = messages;
            validUserErrors.DataBind();


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
        protected void dropListBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*int bookId = Int16.Parse(dropListBook.SelectedValue);
            Response.Write("LOL");
            LibraryContext db = new LibraryContext();
            Book curBook = db.Books.FirstOrDefault(book => book.bookId == bookId);
            FillPreview(curBook);*/
        }

        //Người dùng chỉ được mượn những sách chưa mượn

        protected void borrowBtn_Click(object sender, EventArgs e)
        {
           /* LibraryContext db = new LibraryContext();
            List<String> message = new List<string>();

            string userName = dropListUser.SelectedValue;
            int bookId = Int16.Parse(dropListBook.SelectedValue);

            User user = db.Users.FirstOrDefault(u => u.userName == userName);
            Book curBook = db.Books.FirstOrDefault(book => book.bookId == bookId);

            // Kiểm tra đã mượn sách này hay chưa
            if (user.borBooks.Any(bb => bb.BookId == curBook.bookId)) {
                message.Add("Bạn đã mượn sách này rồi!");
            }
            else
            {
                BorBook availableBook = curBook.BorBooks.FirstOrDefault(bb => bb.state == false);
                if (availableBook == null)
                {
                    message.Add("Sách này đã hết số lượng!");
                }
                else
                {
                    availableBook.state = true;
                    availableBook.returnDate = DateTime.Now.AddDays(Int16.Parse(returnDate.SelectedValue) * 7);
                    user.borBooks.Add(availableBook);
                    db.SaveChanges();
                    message.Add("Mượn sách thành công!");
                }

            }

            borrowMessages.DataSource = message;
            borrowMessages.DataBind();
            // UPDATE RETURN BOOK
            updateReturnDropListBook();*/

        }
        private void updateReturnDropListBook()
        {
            LibraryContext db = new LibraryContext();
            if (db.Users.FirstOrDefault(u => u.userName == returnDropListUser.SelectedValue).borBooks != null)
            {
                returnDropListBook.DataSource =
                    db.Users.FirstOrDefault(u => u.userName == returnDropListUser.SelectedValue)
                    .borBooks.Select(bb => db.Books.FirstOrDefault(b => b.bookId == bb.BookId).bookName).ToList();
                returnDropListBook.DataBind();
            }
        }
        protected void returnDropListUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            LibraryContext db = new LibraryContext();
            if (db.Users.FirstOrDefault(u => u.userName == returnDropListUser.SelectedValue).borBooks != null)
            {
                returnDropListBook.DataSource = db.Users.FirstOrDefault(u => u.userName == returnDropListUser.SelectedValue).
                    borBooks.Select(bb => db.Books.FirstOrDefault(b => b.bookId == bb.BookId).bookName).ToList();
                returnDropListBook.DataBind();
            }
        }

        protected void returnBookBtn_Click(object sender, EventArgs e)
        {
            LibraryContext db = new LibraryContext();
            var UserBorBooks = db.Users.FirstOrDefault(u => u.userName == returnDropListUser.SelectedValue).borBooks;
            var removeTarget = UserBorBooks.FirstOrDefault(
                                         bb => db.Books.FirstOrDefault(b => b.bookId == bb.BookId).bookName == returnDropListBook.SelectedValue
                                );
            removeTarget.state = false;
            UserBorBooks.Remove(removeTarget);
            db.SaveChanges();
            var messages = new List<string>();
            messages.Add("Trả sách thành công");
            returnBookMessages.DataSource = messages;
            returnBookMessages.DataBind();
            updateReturnDropListBook();
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
            preMView.ActiveViewIndex = 0;
        }

        protected void viewNewBook_Click(object sender, EventArgs e)
        {
            
            inforMView.ActiveViewIndex = 2;
            preMView.ActiveViewIndex = 1;
        }

        protected void viewBorBook_Click(object sender, EventArgs e)
        {
            listBorUser.DataSource = listUserName.ToList();
            listBorUser.DataBind();
            listBorBook.DataSource = db.Books.Select(book=>book.bookName).ToList();
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
            Book book = db.Books.FirstOrDefault(b => b.bookName == listBorBook.SelectedValue);
            FillPreview(book);
            // Kiểm tra xem người dùng đã mượn sách này chưa
        }
    }
}