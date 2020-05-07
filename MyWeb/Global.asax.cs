using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Data.Entity;
using MyWeb.Models;
namespace MyWeb
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Database.SetInitializer(new LibraryDbInitzer());
            Application["onlineAcc"] = 0;
                
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Application["onlineAcc"] = (int)Application["onlineAcc"] + 1;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            Application["onlineAcc"] = (int)Application["onlineAcc"] - 1;
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}