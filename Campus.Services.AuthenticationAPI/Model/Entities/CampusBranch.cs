using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campus.Services.AuthenticationAPI.Model.Entities
{
    public class CampusBranch
    {
        [Key]
        public int CampusId { get; set; }
        [Required]
        public string CampusName { get; set; } = string.Empty;
        [Required]
        public string CampusLocation { get; set; } = string.Empty;
        [Required]
        public string CampusCode { get; set; } = string.Empty;
        public string CampusPhone { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string CampusEmail { get; set; } = string.Empty;
        [Required]
        public string CampusPassword { get; set; } = string.Empty;
        public string Role { get; set; } = "Campus";
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; } = DateTime.UtcNow;
        public ICollection<Student> Students { get; set; } = new List<Student>();

    }
}
