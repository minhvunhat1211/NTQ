using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DTO.ReviewDTO
{
    public class ReviewCreateRequest : Base
    {
        public string Title { get; set; }
        public string Comment { get; set; }
    }
}
