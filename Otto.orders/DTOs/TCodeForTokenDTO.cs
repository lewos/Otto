using System.Text.Json.Serialization;

namespace Otto.orders.DTOs
{
    public class TCodeForTokenDTO : BaseCodeForTokenDTO
    {

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }
    }
}
