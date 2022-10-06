using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Otto.Models
{
    public class Order
    {
        //id, vendedor, id_item, item_description, quantity ,id_carrito, sku, , shipping_status
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //vendedor
        public string? UserId { get; set; }
        public long? MUserId { get; set; }
        public long? MOrderId { get; set; }
        public long? MShippingId { get; set; }
        //public long? BusinessId { get; set; }
        //public Company Business { get; set; }
        public string ItemId { get; set; } = null!;
        public string ItemDescription { get; set; } = null!;
        public int Quantity { get; set; }
        public string? PackId { get; set; }
        public string? SKU { get; set; }
        public State ShippingStatus { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public State State { get; set; }
        public string? StateDescription { get; set; }
        public bool? InProgress { get; set; }
        public string? UserIdInProgress { get; set; }
        public DateTime? InProgressDateTimeTaken { get; set; }
        public DateTime? InProgressDateTimeModified { get; set; }
        public int? ProductInStockId { get; set; }
    }    
}
