using System.Text.Json.Serialization;

namespace Otto.orders.DTOs
{
    public class MOrderNotificationDTO: MOrderNotificationBase
    {
        /*
         * {
             "_id":"f9f08571-1f65-4c46-9e0a-c0f43faas1557e",  
             "resource":"/orders/2195160686",
              "user_id": 468424240,
              "topic":"orders_v2",
              "application_id": 5503910054141466,
              "attempts":1,
              "sent":"2019-10-30T16:19:20.129Z",
              "received":"2019-10-30T16:19:20.106Z"
            }
         */
        [JsonPropertyName("user_id")]
        public long? MUserId { get; set; }

    }

    //Deberia estar en otra archivo
    public class MOrderNotificationBase
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        [JsonPropertyName("resource")]
        public string? Resource { get; set; }
        [JsonPropertyName("topic")]
        public string Topic { get; set; }
        [JsonPropertyName("application_id")]
        public long? ApplicationId { get; set; }
        [JsonPropertyName("attempts")]
        public int Attempts { get; set; }
        [JsonPropertyName("sent")]
        public DateTime? Sent { get; set; }
        [JsonPropertyName("received")]
        public DateTime? Received { get; set; }
    }

}
