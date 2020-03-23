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
    public partial class EditUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // SSession["userName"] là username của người dùng đăng đăng nhập, đang sử dụng ứng dụng
            // Kiểm tra đã đăng nhập hay chưa, hoặc có phải là Admin không ?
            if (Session["userName"] == null || !UserLogic.isAdmin(Session["userName"].ToString()))
            {
                Response.Redirect("NoPermisson.html");
                return;
            }
            // user name truyền theo đường dẫn, username của User cần edit
            string userName = Request.QueryString["userName"];
            if (userName == null) Response.Redirect("AdminPage.aspx");

            LibraryContext db = new LibraryContext();
            IQueryable<User> qr = db.Users;
            User user = qr.SingleOrDefault(c => c.userName == userName);
            inpUserName.Enabled = false;
            if (user != null)
            {
                inpUserName.Text = user.userName;
            }
            else
            {
                Response.Redirect("NotFound.hmtl");
            }

        }

        protected void editUserBnt_Click(object sender, EventArgs e)
        {
            //using System.ComponentModel.DataAnnotations;
            //Response.Write("IT'S GOOD\n");
            User tmp = new User()
            {
                userName = inpUserName.Text,
                passWord = inpPassWord.Text,
                role = inpRole.SelectedValue
            };

            ValidationContext ctx = new ValidationContext(tmp, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(tmp, ctx, results, true);
            var messages = results.Select(res => res.ErrorMessage.ToString()).ToList();
            if (isValid)
            {
                LibraryContext db = new LibraryContext();
                var user = db.Users.SingleOrDefault(u => u.userName == tmp.userName);
                if (user != null)
                {
                    user.passWord = tmp.passWord;
                    user.role = tmp.role;
                    db.SaveChanges();
                    Response.Redirect("AdminPage.aspx");
                }
                
            }
            validationError.DataSource = messages;
            validationError.DataBind();
        }
    }
}