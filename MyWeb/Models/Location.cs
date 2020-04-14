using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyWeb.Models
{
    public class Location
    {

        public Location() { }

        [Key]
        [Required(ErrorMessage = "Nhập trường Location id")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Book Id là một số !")]
        public int id { get; set; }

        [Required(ErrorMessage = "Nhập trường địa chỉ")]
        public string dchi { get; set; }
        public virtual ICollection<BorBook> BorBooks { get; set; }
        public virtual ICollection<User> Users { get; set; }

    }
}