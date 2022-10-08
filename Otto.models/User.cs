using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Otto.models
{
    [Index(nameof(Id), nameof(Mail), IsUnique = true)]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public string Mail { get; set; }
        public string Rol { get; set; }
        public string? TUserId { get; set; }
        public string? MUserId { get; set; }        
        public int? CompanyId { get; set; }
    }
}
