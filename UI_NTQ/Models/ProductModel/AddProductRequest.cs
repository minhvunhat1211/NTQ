using Microsoft.Build.Framework;

namespace UI_NTQ.Models.ProductModel
{
    public class AddProductRequest
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Slug { get; set; }
        [Required]
        public string? ProductDetail { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public bool Trending { get; set; }
        public List<IFormFile>? ProductImgs { get; set; }
    }
}
