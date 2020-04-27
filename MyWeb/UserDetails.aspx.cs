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
        private User user = new User();
        public List<BorBook> listBorBooks = new List<BorBook>();
        private LibraryContext db = new LibraryContext();
        public string username;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if(user==null) Response.Redirect("NotFound.html");
            user = (User)Session["user"];
            
            if (user==null) Response.Redirect("NotFound.html");

            // cập nhật user
            user = db.Users.FirstOrDefault(u => u.userName == user.userName);
            username = user.userName;

            // sort để hiển thị danh sách các sách mượn trước , sau đó mới là sách đặt (opacity : 0.5)
            listBorBooks = user.borBooks.OrderBy(bb=>-bb.state).ToList();

            tbUserName.Text = username;
            tbRealName.Text = user.realName;
            tbDiaChi.Text = user.dchi;
            borBookCount.Text = user.borBooks.Count(bb=>bb.state==2)+"/" + Book.limitBorBook 
                + "(đang đặt :" + user.borBooks.Count(bb => bb.state == 1)+")" ;
            tbRole.Text = user.role;
        }

        protected void logoutBtn_Click(object sender, EventArgs e)
        {
            Session["user"] = null;
            Response.Redirect("ListBook.aspx");
        }
    }
}