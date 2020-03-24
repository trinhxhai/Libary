using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace MyWeb.Models
{
    public class User
    {
        public User()
        {
        }

        [Required(ErrorMessage = "Nhập trường username !")]
        [Key]
        public string userName { get; set; }


        [Required(ErrorMessage = "Nhập trường Họ và tên !")]
        public string realName { get; set; }
        public string dchi { get; set; }

        [Required(ErrorMessage = "Nhập trường CMND !")]
        [RegularExpression(@"\d{12}", ErrorMessage = "CMND phải gồm 12 kí tự số !")]
        public string CMND { get; set; }

        [Required(ErrorMessage = "Nhập trường password !")]
        public string passWord { get; set; }



        [Required(ErrorMessage = "Yêu cầu chọn Role !")]
        public string role { get; set; }
        public virtual ICollection<BorBook> borBooks { get; set; } 
    }
}