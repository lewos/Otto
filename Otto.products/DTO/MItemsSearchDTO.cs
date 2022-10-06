using System.Text.Json.Serialization;

namespace Otto.products.DTO
{
    public class MItemsSearchDTO
    {
        [JsonPropertyName("seller_id")]
        public string SellerId { get; set; }

        [JsonPropertyName("query")]
        public object Query { get; set; }

        [JsonPropertyName("paging")]
        public Paging Paging { get; set; }

        [JsonPropertyName("results")]
        public List<string> Results { get; set; }

        [JsonPropertyName("orders")]
        public List<Order> Orders { get; set; }

        [JsonPropertyName("available_orders")]
        public List<AvailableOrder> AvailableOrders { get; set; }
    }
    public class AvailableOrder
    {
        [JsonPropertyName("id")]
        public object Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Order
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Paging
    {
        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
