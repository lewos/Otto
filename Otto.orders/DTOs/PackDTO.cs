namespace Otto.orders.DTOs
{
    public class PackDTO
    {
        public string MOrderId { get; set; }
        public string PackId { get; set; }
        public List<OrderDTO> Items { get; set; }

        public PackDTO(string id, string packId, List<OrderDTO> items)
        {
            MOrderId = id;
            PackId = packId;
            Items = items;
        }
        public PackDTO()
        {

        }
    }
}
