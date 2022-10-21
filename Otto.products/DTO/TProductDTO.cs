namespace Otto.products.DTO
{
    public class TProductDTO: TItemSearchDTO
    {
        public TProductDTO(TItemSearchDTO item, bool inStock, int quantityInStock)
        {
            this.QuantityInStock = quantityInStock;
            this.InStock = inStock;

            this.Id = item.Id;
            this.Name = item.Name;
            this.Description = item.Description;
            this.Handle = item.Handle;
            this.Attributes = item.Attributes;
            this.Published = item.Published;
            this.FreeShipping = item.FreeShipping;
            this.RequiresShipping = item.RequiresShipping;
            this.CanonicalUrl = item.CanonicalUrl;
            this.VideoUrl = item.VideoUrl;
            this.SeoTitle = item.SeoTitle;
            this.SeoDescription = item.SeoDescription;
            this.Brand = item.Brand;
            this.CreatedAt = item.CreatedAt;
            this.UpdatedAt = item.UpdatedAt;
            this.Variants = item.Variants;
            this.Tags = item.Tags;
            this.Images = item.Images;
            this.Categories = item.Categories;
        }

        public bool InStock { get; set; }
        public int QuantityInStock { get; set; }

    }
}
