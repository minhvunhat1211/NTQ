using System.Drawing.Printing;

namespace UI_NTQ.Models
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PassWord { get; set; }
        public int Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? DeleteAt { get; set; }
        public string? Role { get; set; }
        public string StatusName
        {
            get
            {
                if (Status == 1)
                {
                    return "Active";
                }
                return "Delete";
            }
        }
        public string FullName
        {
            get
            {
                return Firstname + " " + LastName;
            }
        }
        
    }

}
