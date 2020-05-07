using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWeb.Models;
namespace MyWeb
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string userName = Request.Form.Get("username");
                string passWord = Request.Form.Get("password");
                string resq = "";

                LibraryContext db = new LibraryContext();

                var user = db.Users.FirstOrDefault(u => u.userName == userName && u.passWord == passWord);

                if (user == null)
                {
                    resq = "Đăng nhập không thành công !";
                }
                else
                {
                    Session["user"] = user;
                    //resq = "Đăng nhập thành công !";
                    if (user.role == "admin")
                        Response.Redirect("AdminPage.aspx");
                    else Response.Redirect("ListBook.aspx");
                }

                Response.Write(resq);// lỗi nếu có
            }
        }
    }
}