using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MyWeb.Models
{
    public class BorBook
    {
        [Required(ErrorMessage = "Nhập trường Id Borrowable book !")]
        [Key]
        public int id { get; set; }
        public DateTime returnDate { get; set; }
        public bool state { get; set; }
        // một BorBook nhất định phải thuộc một sách nào đó, thêm trường BookId để có một trường FK not null (BookId) thể hiện quan hệ trên
        // User thì có thể có người mượn hoặc không, nên không cần một UserId
        public int BookId { get; set; }
        public Book Book { get; set; }
        public User user { get; set; }

    }
}