using System.Text.Json.Serialization;

namespace Otto.orders.DTOs
{
    public class MissedFeedsDTO
    {
        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; }
    }
    public class Message : MOrderNotificationBase
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("request")]
        public Request Request { get; set; }

        [JsonPropertyName("response")]
        public MessageResponse Response { get; set; }
    }
    public class Headers
    {
        [JsonPropertyName("Accept")]
        public List<string> Accept { get; set; }

        [JsonPropertyName("Connection")]
        public List<string> Connection { get; set; }

        [JsonPropertyName("Content-Type")]
        public List<string> ContentType { get; set; }

        [JsonPropertyName("User-Agent")]
        public List<string> UserAgent { get; set; }

        [JsonPropertyName("X-Rest-Pool-Name")]
        public List<string> XRestPoolName { get; set; }

        [JsonPropertyName("X-Socket-Timeout")]
        public List<string> XSocketTimeout { get; set; }

        [JsonPropertyName("Content-Length")]
        public List<string> ContentLength { get; set; }

        [JsonPropertyName("Date")]
        public List<string> Date { get; set; }

        [JsonPropertyName("Ngrok-Error-Code")]
        public List<string> NgrokErrorCode { get; set; }

        [JsonPropertyName("Ngrok-Trace-Id")]
        public List<string> NgrokTraceId { get; set; }
    }
    public class Request
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("headers")]
        public Headers Headers { get; set; }
    }
    public class MessageResponse
    {
        [JsonPropertyName("req_time")]
        public int ReqTime { get; set; }

        [JsonPropertyName("http_code")]
        public int HttpCode { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("headers")]
        public Headers Headers { get; set; }
    }
}
