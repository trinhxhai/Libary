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
        public DateTime borrowDate { get; set; }
        public DateTime returnDate { get; set; }

        // 0 : tức không có ai mượn
        // 1 : tức có người đặt mượn trên mạng
        // 2 : đã có người mượn, sách đang ở chỗ người mượn
        public int state { get; set; }
        // một BorBook nhất định phải thuộc một sách nào đó, thêm trường BookId để có một trường FK not null (BookId) thể hiện quan hệ trên
        // User thì có thể có người mượn hoặc không, nên không cần một UserId
        public int BookId { get; set; }
        // Boook cũng cần có "virtual" ?? nên tìm hiểu 
        // để có thể chưa reference
        public virtual Book Book { get; set; }
        // User là bắt buộc để ... user ở Boor book có thể chứa giá trị
        // nó sẽ tự động gán giá trị user của user mà nó được gán vào
        // tức là khi add một BorBook vào một User thì mặc định user của borbook đó sẽ nhận giá trị của user
        // đồng thời cũng giải quyết luôn vấn đền Book nhận 
        public virtual User User { get; set; }

    }
}