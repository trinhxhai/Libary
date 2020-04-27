using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MyWeb.Models
{
    public partial class Book
    {
      
/*        public Book(Book tmp)
        {
            this.bookId = tmp.bookId;
            this.bookName = tmp.bookName;
            this.category = tmp.category;
            this.description = tmp.description;
            this.imagePath = tmp.imagePath;
            this.price = tmp.price;
            this.amount = tmp.amount;
        }*/
        public Book() { }

        [Key]
        [Required(ErrorMessage ="Nhập trường ID sách")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Book Id là một số !")]
        public int bookId { get; set; }


        [Required(ErrorMessage = "Nhập trường Tên sách")]
        public string bookName { get; set; }


        public string category { get; set; }


        public string description { get; set; }


        [Required(ErrorMessage = "Yêu cầu đính kèm Ảnh sách")]
        public string imagePath { get; set; }


        [Required(ErrorMessage = "Nhập trường Giá sách")]
        [RegularExpression(@"\d+", ErrorMessage = "Giá bán là một số !")] // ít nhất là 1 kí tự số <=> [0-9]+
        public string price { get; set; }


        [Required(ErrorMessage = "Nhập số lượng sách")]
        [RegularExpression(@"\d+", ErrorMessage = "Số lượng sach là một số !")]
        public string amount { get; set; }


        public virtual ICollection<BorBook> BorBooks { get; set; }
    }
}