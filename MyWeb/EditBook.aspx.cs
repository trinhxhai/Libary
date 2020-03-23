using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWeb.Models;
namespace MyWeb
{
    public partial class EditBook : System.Web.UI.Page
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

            // điền thông tin book vào các trường
            if(!IsPostBack) Fill(curBook);
            // Không hiểu phần này
            // Nhưng nếu bỏ đi, các trường thông tin cũ sẽ hiển thị ở các phần điền sắn trong form
            // thay vì  bị thay thế, mặc dù đã gọi hàm Fill ở click_btn event!
            // 1 Controll sẽ được cập nhật hiển thị sau lần đầu tiên bị tác động ?
            Response.Write("page load event!");
            //click bnt => page_load event (=>show control's content)=> click bnt event 

        }
        protected void Fill(Book data)
        {
            BookName.Text = data.bookName;
            BookCategory.Text = data.category;
            BookDescription.Text = data.description;
            var path = "~/Images/";
            bookPic.ImageUrl = path + data.imagePath;
            BookPrice.Text = data.price;
        }

        protected void editBookBnt_Click(object sender, EventArgs e)
        {
            var messages = new List<String>();
            LibraryContext db = new LibraryContext();
            Book book = db.Books.FirstOrDefault(b => b.bookId == BookId);
            // Nếu không có ảnh tải lên dùng ảnh cũ
            if (ImageUpload.HasFile) book.imagePath = ImageUpload.FileName;

            book.bookId = BookId;
            book.bookName = BookName.Text;
            book.category = BookCategory.Text;
            book.description = BookDescription.Text;
            book.price = BookPrice.Text;

            if (BookLogic.isValid(book, ref messages))
            {
                try
                {
                    String path = Server.MapPath("~/Images/");
                    // XÓA ĐƯỜNG ẢNH CŨ<CHƯA LÀM>
                    // LƯU ẢNH MỚI 
                    ImageUpload.PostedFile.SaveAs(path + ImageUpload.FileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                db.SaveChanges();
                messages.Add("Sửa thành công!");
                // điển lại thông tin vào các trường
                Fill(book);
            }/// else các lỗi đã dc add vào messege
            validBookErrors.DataSource = messages;
            validBookErrors.DataBind();
        }

    }
}