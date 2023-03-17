using System.ComponentModel.DataAnnotations;

namespace UI_NTQ.Models.ProductModel
{
    public class PoductEditRequest
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name khong duoc de trong")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Slug khong duoc de trong")]
        public string? Slug { get; set; }
        [Required(ErrorMessage = "ProductDetail khong duoc de trong")]
        public string? ProductDetail { get; set; }
        [Required(ErrorMessage = "Price khong duoc de trong")]
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "Trending khong duoc de trong")]
        public bool Trending { get; set; }
        public ICollection<IFormFile>? ProductImgs { get; set; }
    }
}
