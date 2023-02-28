using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class Product : Base
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? ProductDetail { get; set; }
        public decimal? Price { get; set; }
        public bool? Trending { get; set; }
        public ICollection<ProductImg>? ProductImgs { get; set; }

    }
}
