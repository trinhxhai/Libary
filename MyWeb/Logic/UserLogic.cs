using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class UserLogic
    {
        public static bool isAdmin(string userName)
        {

            LibraryContext db = new LibraryContext();
            IQueryable<User> listUser = db.Users;
            User res = listUser.FirstOrDefault(q => q.userName == userName);
            // do not throws an InvalidOperationException 
            // First will
            return (res != null && res.role == "admin");
        }
    }
}