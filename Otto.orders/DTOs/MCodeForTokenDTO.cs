using System.Text.Json.Serialization;

namespace Otto.orders.DTOs
{
    public class MCodeForTokenDTO : BaseCodeForTokenDTO
    {
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
