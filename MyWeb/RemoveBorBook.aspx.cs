using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWeb.Models;

namespace MyWeb
{
    public partial class RemoveBorBook : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LibraryContext db = new LibraryContext();


            User curAdmin = (User)Session["user"];
            if (curAdmin ==null | curAdmin.role!="admin") Response.Redirect("NoPermisson.html");
            var username = curAdmin.userName;

            // nếu là admin / tồn tại user thì ẩn hộp login - vì đã đăng nhập r
            //headerLoginBox.Style.Add("display", "none");

            // check quyền Admin

            var sidBorBook = Request.QueryString.Get("idBorBook");
            int idBorBook;
            // trường hợp id không hợp lệ
            if (!int.TryParse(sidBorBook,out idBorBook))
                Response.Redirect("NotFound.html");

            var borBook = db.BorBooks.FirstOrDefault(bb=>bb.id==idBorBook);
            // check xem Admin có thuộc location của sách k
            if (curAdmin.LocationId!= borBook.LocationId)
                Response.Redirect("NoPermisson.html");
            // để chắc chắn rằng sách ở  trạng thái 2 không bị xóa
            if (borBook.state==2) Response.Redirect("NoPermisson.html");

            db.BorBooks.Remove(borBook);
            db.SaveChanges();
            Session["RemoveBorBookBacking"] = "true";
            Response.Redirect("AdminPage.aspx");

        }
    }
}