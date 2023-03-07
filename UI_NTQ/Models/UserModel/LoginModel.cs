using System.ComponentModel.DataAnnotations;

namespace UI_NTQ.Models.UserModel
{
#nullable disable
    public class LoginModel
    {
        [Required(ErrorMessage = "Email khong duoc de trong")]
        [EmailAddress(ErrorMessage = "Sai dinh dang email")]
        [MaxLength(30, ErrorMessage = "Do dai email trong khoang 10 den 30 ki tu")]
        [MinLength(10, ErrorMessage = "Do dai email trong khoang 10 den 30 ki tu")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mat khau khong duoc de trong")]
        [MaxLength(20, ErrorMessage = "Do dai mat khau trong khoang 8 den 20 ki tu")]
        [MinLength(6, ErrorMessage = "Do dai mat khau trong khoang 8 den 20 ki tu")]
        public string PassWord { get; set; }
    }
}
