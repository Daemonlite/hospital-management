using Microsoft.EntityFrameworkCore;
using Health.Entities;

namespace Health.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Patients> Patients { get; set; }
        public DbSet<Department> Departments { get; set; }
        
        public DbSet<PatientsFiles> PatientsFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User-Department relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict); // Or Cascade as needed

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>() // Stores enum as string in DB
                .HasMaxLength(20);

            base.OnModelCreating(modelBuilder);
        }

    }
}