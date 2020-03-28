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
        public string username;
        protected void Page_Load(object sender, EventArgs e)
        {
            username = Request.QueryString["userName"];
            if (username == null) Response.Redirect("NotFound.html");
            LibraryContext db = new LibraryContext();
            user = db.Users.SingleOrDefault(c => c.userName == username);
            if(user==null) Response.Redirect("NotFound.html");

            


            listBorBooks = user.borBooks.ToList();

            tbUserName.Text = username;
            tbRealName.Text = user.realName;
            tbDiaChi.Text = user.dchi;
            borBookCount.Text = user.borBooks.Count.ToString()+"/20";
            tbRole.Text = user.role;
        }

        protected void logoutBtn_Click(object sender, EventArgs e)
        {
            Session["userName"] = null;
            Response.Redirect("ListBook.aspx");
        }
    }
}