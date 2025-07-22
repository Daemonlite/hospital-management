using Microsoft.EntityFrameworkCore;
using Health.Entities;

namespace Health.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Patients> Patients { get; set; }

        public DbSet<Department> Departments { get; set; }
    }
}
