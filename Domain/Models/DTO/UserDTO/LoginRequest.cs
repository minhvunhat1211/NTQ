using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace Domain.Models.DTO.UserDTO
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string PassWord { get; set; }
    }
}
