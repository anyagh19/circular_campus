using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campus.Services.AuthenticationAPI.Model.Entities
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [Required]
        public string StudentPRN { get; set; } = string.Empty;
        [Required]
        public string StudentName { get; set; } = string.Empty;
        [Required]
        public string StudentPhone { get; set; } = string.Empty;
        [Required]
        public string StudentAddress { get; set; } = string.Empty;
        [Required]
        public string CampusCode { get; set; } = string.Empty;
        public string StudentDepartment { get; set; } = string.Empty;
        public string StudentYear { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string StudentEmail { get; set; } = string.Empty;
        [Required]
        public string StudentPassword { get; set; } = string.Empty;
        public string Role { get; set; } = "Student";
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; } = DateTime.UtcNow;
        public int CampusId { get; set; }

        [ForeignKey("CampusId")]
        public CampusBranch? CampusBranch { get; set; }
    }
}
