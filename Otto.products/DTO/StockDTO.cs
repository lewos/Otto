namespace Otto.products.DTO
{
    public class StockDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string Origin { get; set; }
        public int Quantity { get; set; }

        public string SellerId { get; set; }
        public string SellerIdMail { get; set; }
        public string? MSellerId { get; set; }
        public string? TSellerId { get; set; }

        public string? MItemId { get; set; }
        public string? TItemId { get; set; }

        public string? SKU { get; set; }
        public string? Code { get; set; }
        public string? Category { get; set; }
        //"confirmado",
        //"pendiente"
        //"error",
        public string State { get; set; }
        public string? StateDescription { get; set; }
    }
}
