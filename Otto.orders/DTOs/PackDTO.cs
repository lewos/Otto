using Otto.models;

namespace Otto.orders.DTOs
{
    public class PackDTO
    {
        public string PackId { get; set; }
        public List<Order> Items { get; set; }

        public PackDTO(string packId, List<Order> items)
        {
            PackId = packId;
            Items = items;
        }
        public PackDTO()
        {

        }
    }
}
