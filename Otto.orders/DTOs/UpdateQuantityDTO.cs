namespace Otto.orders.DTOs
{
    public class UpdateQuantityDTO
    {
        public int Quantity { get; set; }
        public string? MItemId { get; set; }
        public UpdateQuantityDTO(int quantity, string? mItemId)
        {
            Quantity = quantity;
            MItemId = mItemId;
        }
    }
}
