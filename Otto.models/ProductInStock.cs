namespace Otto.models
{
    public class ProductInStock : IProductInStock
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string Origin { get; set; }
        public int Quantity { get; set; }
        public string? MSellerId { get; set; }
        public string? TSellerId { get; set; }

        public string? MItemId { get; set; }
        public string? TItemId { get; set; }

        public string? SKU { get; set; }
        public string? Code { get; set; }
        public string? Category { get; set; }
        public State State { get; set; }
        public string? StateDescription { get; set; }

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserLastName { get; set; }
        public string? UserIdMail { get; set; }
        public int CompanyId { get; set; }
        public string? Size { get; set; }
        public string? Batch { get; set; }
        public string? Location { get; set; }
        public string? EAN { get; set; }
    }
}
