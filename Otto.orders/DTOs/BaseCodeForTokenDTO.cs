using System.Text.Json.Serialization;

namespace Otto.orders.DTOs
{
    public abstract class BaseCodeForTokenDTO
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("token_type")]
        public string Type { get; set; }
        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }
    }
}
