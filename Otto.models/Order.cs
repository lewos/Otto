using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Otto.models
{
    public class Order
    {
        //id, vendedor, id_item, item_description, quantity ,id_carrito, sku, , shipping_status
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //vendedor
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserLastName { get; set; }
        public int? ProductInStockId { get; set; }
        public int? CompanyId { get; set; }
        public string ItemId { get; set; } = null!;
        public string? PackId { get; set; }
        public int Quantity { get; set; }

        public long? MUserId { get; set; }
        public long? MOrderId { get; set; }
        public long? MShippingId { get; set; }

        public long? TUserId { get; set; }
        public long? TOrderId { get; set; }
        public long? TShippingId { get; set; }

        public string ItemDescription { get; set; } = null!;
        public string? SKU { get; set; }
        public string? EAN { get; set; }
        public State ShippingStatus { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public OrderState State { get; set; }
        public string? StateDescription { get; set; }
        public bool? InProgress { get; set; }
        public int? UserIdInProgress { get; set; }
        public DateTime? InProgressDateTimeTaken { get; set; }
        public DateTime? InProgressDateTimeModified { get; set; }
        public string? Location { get; set; }
    }
}
