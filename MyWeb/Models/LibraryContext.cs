using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
    // View -> Other windows -> Package Manager Console
    // install-package entityframework -version 5.0.0.0
namespace MyWeb.Models
{
    public class LibraryContext: DbContext
    {
        public LibraryContext() : base("MyWeb")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BorBook> BorBooks { get; set; }
 
    }
}
