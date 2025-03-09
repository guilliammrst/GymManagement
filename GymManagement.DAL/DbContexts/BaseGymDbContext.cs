using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.DbContexts
{
    public class BaseGymDbContext : DbContext
    {
        private readonly string _connectionString = "connectionString";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_connectionString);
        }

        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
