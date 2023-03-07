namespace UI_NTQ.Models.ProductModel
{
    public class ProductImg
    {
        public int? Id { get; set; }

        public int? ProductId { get; set; }

        public string? ImagePath { get; set; }

        public string? Caption { get; set; }

        public bool? IsDefault { get; set; }

        public long? FileSize { get; set; }
    }
}
