namespace Otto.products.DTO
{
    public class ProductDTO: ProductBody
    {
        public ProductDTO(ProductBody productBody, bool inStock, int quantityInStock)
        {
            this.QuantityInStock = quantityInStock;
            this.InStock = inStock;
            this.Id = productBody.Id;
            this.Permalink = productBody.Permalink;
            this.SecureThumbnail = productBody.SecureThumbnail;
            this.Price = productBody.Price;
            this.Title = productBody.Title;
            this.Attributes = productBody.Attributes;
            this.AvailableQuantity = productBody.AvailableQuantity;
            this.Thumbnail = productBody.Thumbnail;
            this.Shipping = productBody.Shipping;
        }

        public bool InStock { get; set; }
        public int QuantityInStock { get; set; }
    }
}
