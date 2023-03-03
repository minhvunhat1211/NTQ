using System.ComponentModel.DataAnnotations;

namespace UI_NTQ.Models
{
    public class UserEditRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Firstname khong duoc de trong")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Lastname khong duoc de trong")]
        public string Lastname { get; set; }
    }
}
