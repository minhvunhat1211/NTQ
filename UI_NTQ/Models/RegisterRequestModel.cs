using System.ComponentModel.DataAnnotations;

namespace UI_NTQ.Models
{
    public class RegisterRequestModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email khong duoc de trong")]
        [EmailAddress(ErrorMessage = "Sai dinh dang email")]
        [MaxLength(30, ErrorMessage = "Do dai email trong khoang 10 den 30 ki tu")]
        [MinLength(10, ErrorMessage = "Do dai email trong khoang 10 den 30 ki tu")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mat khau khong duoc de trong")]
        [RegularExpression(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$", ErrorMessage = "Password phải từ 8-20 kí tự bao gồm ít nhất 1 kí tự số, 1 kí tự viết hoa và 1 kí tự đặc biệt")]
        public string PassWord { get; set; }
    }
}
