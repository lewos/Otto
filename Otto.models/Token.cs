using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Otto.models
{
    public class Token: IToken
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }        
        public string? SalesChannel { get; set; }
        public long? MUserId { get; set; }
        public long? TUserId { get; set; }
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string? Type { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public bool? Active { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public long? ExpiresIn { get; set; }
        public int? UserId { get; set; }
    }
}
