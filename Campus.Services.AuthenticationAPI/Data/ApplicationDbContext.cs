using Campus.Services.AuthenticationAPI.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Campus.Services.AuthenticationAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<CampusBranch> Campuses { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
