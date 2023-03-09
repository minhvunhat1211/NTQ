namespace UI_NTQ.Models.ProductModel
{
    public class ProductResponse<T> : BaseModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? ProductDetail { get; set; }
        public decimal? Price { get; set; }
        public bool Trending { get; set; }
        public List<T>? ProductImgs { get; set; }
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
        public string TrendName
        {
            get
            {
                if (Trending == true)
                {
                    return "On trend";
                }
                return "Normal";
            }
        }
    }
}
