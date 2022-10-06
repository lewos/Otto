namespace Otto.Models
{
    public interface IToken
    {
        int Id { get; set; }
        string? SalesChannel { get; set; }
        long? MUserId { get; set; }
        long? TUserId { get; set; }
        string AccessToken { get; set; }
        string RefreshToken { get; set; }
        string? Type { get; set; }
        DateTime? Created { get; set; }
        DateTime? Modified { get; set; }
        bool? Active { get; set; }
        DateTime? ExpiresAt { get; set; }
        long? ExpiresIn { get; set; }
        int? UserId { get; set; }
    }
}
