using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWeb.Models;
namespace MyWeb
{
    public partial class Site : System.Web.UI.MasterPage
    {
        public string username;
        public int onlineAcc=0;
        protected void Page_Load(object sender, EventArgs e)
        {
            User u = (User)Session["user"];
            if (u != null) {
                username = u.userName;
                headerLoginBox.Style.Add("display", "none");
            }else
            userNav.Style.Add("display", "none");
            onlineAcc = int.Parse(Application["onlineAcc"].ToString());
        }
        protected void logoutBtn_Click(object sender, EventArgs e)
        {
            Session["user"] = null;
            Response.Redirect("ListBook.aspx");
        }
    }
}