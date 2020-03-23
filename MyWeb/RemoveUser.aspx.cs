using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWeb.Models;
namespace MyWeb
{
    public partial class RemoveUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // SSession["userName"] là username của người dùng đăng đăng nhập, đang sử dụng ứng dụng
            // Kiểm tra đã đăng nhập hay chưa, hoặc có phải là Admin không ?
            if (Session["userName"] == null || !UserLogic.isAdmin(Session["userName"].ToString())) {
                Response.Redirect("NoPermisson.html");
                return ; 
            }
            // user name truyền theo đường dẫn, username của User cần xóa
            string userName = Request.QueryString["userName"];

            LibraryContext db = new LibraryContext();
            IQueryable<User> qr = db.Users;
            User user = qr.SingleOrDefault(c => c.userName == userName);
            db.Users.Remove(user);
            db.SaveChanges();
                
            Response.Redirect("AdminPage.aspx");
        }
    }
}