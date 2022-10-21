using System.Text.Json.Serialization;

namespace Otto.orders.DTOs
{
    public class TOrderNotificationDTO
    {
        //{"store_id":2332874,
        //"event":"order/paid",
        //"id":807493102}

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("store_id")]
        public long StoreId { get; set; }

        [JsonPropertyName("event")]
        public string Event { get; set; }
    }
}
