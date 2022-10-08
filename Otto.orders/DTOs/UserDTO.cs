using System.Text.Json.Serialization;

namespace Otto.orders.DTOs
{
    public class UserDTO
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("userName")]
        public string Name { get; set; }

        [JsonPropertyName("userPass")]
        public string Pass { get; set; }

        [JsonPropertyName("userMail")]
        public string Mail { get; set; }

        [JsonPropertyName("userRol")]
        public string Rol { get; set; }
        [JsonPropertyName("tUserId")]
        public string TUserId { get; set; }
        [JsonPropertyName("mUserId")]
        public string MUserId { get; set; }

        [JsonPropertyName("permisos")]
        public List<string>? Permisos { get; set; }
        
    }
}
