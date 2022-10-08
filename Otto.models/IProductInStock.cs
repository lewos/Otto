namespace Otto.models
{
    public interface IProductInStock
    {
        string? Category { get; set; }
        string? Code { get; set; }
        string? Description { get; set; }
        int Id { get; set; }
        string? MItemId { get; set; }
        string? MSellerId { get; set; }
        string? Name { get; set; }
        string Origin { get; set; }
        int Quantity { get; set; }
        string SellerId { get; set; }
        string SellerIdMail { get; set; }
        string? SKU { get; set; }
        State State { get; set; }
        string? StateDescription { get; set; }
        string? TItemId { get; set; }
        string? TSellerId { get; set; }
        string? Size { get; set; }
        string? Batch { get; set; }  
        public int UserId { get; set; }
        public int CompanyId { get; set; }
    }
}