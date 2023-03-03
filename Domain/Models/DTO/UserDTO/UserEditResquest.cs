using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DTO.UserDTO
{
    public class UserEditResquest
    {
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
