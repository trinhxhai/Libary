using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyWeb
{
    public partial class Location : System.Web.UI.Page
    {
        public string username;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userName"] != null)
            {
                headerLoginBox.Style.Add("display", "none");
                username = Session["userName"].ToString();
            }
            else
            {
                userNav.Style.Add("display", "none");
            }
        }

        protected void logoutBtn_Click(object sender, EventArgs e)
        {
            Session["userName"] = null;
            Response.Redirect("ListBook.aspx");
        }
    }
}