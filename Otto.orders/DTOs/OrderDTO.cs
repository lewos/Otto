namespace Otto.orders.DTOs
{
    //Deberia poner una base clase
    public class OrderDTO
    {

        //id, vendedor, id_item, item_description, quantity ,id_carrito, sku, , shipping_status
        public int Id { get; set; }
        //vendedor
        public int? UserId { get; set; }

        public string? UserName { get; set; }
        public string? UserLastName { get; set; }

        public long? MUserId { get; set; }
        public long? MOrderId { get; set; }
        public long? MShippingId { get; set; }

        public long? TUserId { get; set; }
        public long? TOrderId { get; set; }
        public long? TShippingId { get; set; }

        public int? CompanyId { get; set; }
        public string ItemId { get; set; } = null!;
        public string ItemDescription { get; set; } = null!;
        public int Quantity { get; set; }
        public string? PackId { get; set; }
        public string? SKU { get; set; }
        public string ShippingStatus { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public string? State { get; set; }
        public bool? InProgress { get; set; }
        public int? UserIdInProgress { get; set; }

        public DateTime? InProgressDateTimeTaken { get; set; }
        public DateTime? InProgressDateTimeModified { get; set; }
    }
}
