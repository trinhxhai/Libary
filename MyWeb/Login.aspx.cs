using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWeb.Models;
namespace MyWeb
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string userName = Request.Form.Get("username");
                string passWord = Request.Form.Get("password");
                string resq;
                if (UserLogic.userLogin(userName, passWord, out resq))
                {
                    Session["userName"] = userName;
                    Response.Redirect("ListBook.aspx");
                }
                Response.Write(resq);
            }
        }
    }
}