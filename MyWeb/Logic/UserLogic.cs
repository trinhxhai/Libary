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
        public static bool userLogin(string username,string password,out string resp)
        {
            LibraryContext db = new LibraryContext();
            IQueryable<User> q = db.Users;
            User res = q.FirstOrDefault(user => user.userName == username);
            if (res == null)
            {
                resp = "Khong ton tai nguoi dung!";
                return false;
            }
            if (res.passWord != password)
            {
                resp = "Sai mat khau!";
                return false;
            }
            resp = "Dang nhap thanh cong!";
            return true;
        }
    }
}