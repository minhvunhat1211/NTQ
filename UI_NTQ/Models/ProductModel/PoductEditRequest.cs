namespace UI_NTQ.Models.ProductModel
{
    public class PoductEditRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? ProductDetail { get; set; }
        public decimal? Price { get; set; }
        public bool Trending { get; set; }
        public ICollection<IFormFile>? ProductImgs { get; set; }
    }
}
