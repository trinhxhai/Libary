using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyWeb
{
    public partial class JustForTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Write(ListBox1.SelectedValue);
            TextBox1.Text= ListBox1.SelectedValue;
        }

        protected void ListBox1_TextChanged(object sender, EventArgs e)
        {
            Response.Write(ListBox1.SelectedValue);
            TextBox1.Text = ListBox1.SelectedValue;
        }
    }
}