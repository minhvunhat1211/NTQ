using System.ComponentModel.DataAnnotations;

namespace UI_NTQ.Models.ProductModel
{
    public class PoductEditRequest
    {
        [Required]
        public int Id { get; set; }
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
        public ICollection<IFormFile>? ProductImgs { get; set; }
    }
}
