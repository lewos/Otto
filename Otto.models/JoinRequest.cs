namespace Otto.Models
{
    public class JoinRequest
    {
        public int Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public State State { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }        
    }    
}
