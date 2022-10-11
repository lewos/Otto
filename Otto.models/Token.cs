using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Otto.models
{
    public class Token: IToken
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("SalesChannel")]
        public string? SalesChannel { get; set; }
        [JsonPropertyName("MUserId")]
        public long? MUserId { get; set; }
        [JsonPropertyName("TUserId")]
        public long? TUserId { get; set; }
        [JsonPropertyName("AccessToken")]
        public string AccessToken { get; set; } = null!;
        [JsonPropertyName("RefreshToken")]
        public string RefreshToken { get; set; } = null!;
        [JsonPropertyName("Type")]
        public string? Type { get; set; }
        [JsonPropertyName("Created")]
        public DateTime? Created { get; set; }
        [JsonPropertyName("Modified")]
        public DateTime? Modified { get; set; }
        [JsonPropertyName("Active")]
        public bool? Active { get; set; }
        [JsonPropertyName("ExpiresAt")]
        public DateTime? ExpiresAt { get; set; }
        [JsonPropertyName("ExpiresIn")]
        public long? ExpiresIn { get; set; }
        [JsonPropertyName("UserId")]
        public int? UserId { get; set; }
    }
}
