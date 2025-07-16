using GymManagement.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Persistence.DbContexts
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
        public DbSet<AddressModel> Addresses { get; set; }
        public DbSet<ClubModel> Clubs { get; set; }
        public DbSet<MembershipModel> Memberships { get; set; }
        public DbSet<MembershipPlanModel> MembershipPlans { get; set; }
        public DbSet<PaymentDetailModel> PaymentDetails { get; set; }
        public DbSet<AttendanceModel> Attendances { get; set; }
        public DbSet<CoachingModel> Coachings { get; set; }
        public DbSet<CoachingPlanModel> CoachingPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<UserModel>()
                .HasOne(u => u.Address)
                .WithOne()
                .HasForeignKey<UserModel>(u => u.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClubModel>()
                .HasOne(c => c.Address)
                .WithOne()
                .HasForeignKey<ClubModel>(c => c.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClubModel>()
                .HasOne(c => c.Manager)
                .WithMany()
                .HasForeignKey(c => c.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<MembershipModel>()
                .HasOne(m => m.Member)
                .WithMany(u => u.Memberships)
                .HasForeignKey(m => m.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MembershipModel>()
                .HasOne(m => m.HomeClub)
                .WithMany(hc => hc.Memberships)
                .HasForeignKey(m => m.HomeClubId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<MembershipModel>()
                .HasOne(m => m.MembershipPlan)
                .WithMany(mp => mp.Memberships)
                .HasForeignKey(m => m.MembershipPlanId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<MembershipModel>()
                .HasOne(m => m.PaymentDetail)
                .WithOne(pd => pd.Membership)
                .HasForeignKey<MembershipModel>(m => m.PaymentDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AttendanceModel>()
                .HasOne(a => a.Member)
                .WithMany(u => u.Attendances)
                .HasForeignKey(a => a.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AttendanceModel>()
                .HasOne(a => a.Club)
                .WithMany(c => c.Attendances)
                .HasForeignKey(a => a.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CoachingPlanModel>()
                .HasOne(c => c.Coach)
                .WithMany()
                .HasForeignKey(c => c.CoachId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CoachingPlanModel>()
                .HasOne(c => c.Club)
                .WithMany(mp => mp.CoachingPlans)
                .HasForeignKey(c => c.ClubId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CoachingPlanModel>()
                .HasMany(mp => mp.Coachings)
                .WithOne(c => c.CoachingPlan)
                .HasForeignKey(c => c.CoachingPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CoachingModel>()
                .HasOne(c => c.Member)
                .WithMany()
                .HasForeignKey(c => c.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CoachingModel>()
                .HasOne(c => c.PaymentDetail)
                .WithOne(pd => pd.Coaching)
                .HasForeignKey<CoachingModel>(c => c.PaymentDetailId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        // Override SaveChangesAsync to set the UpdatedAt property for modified entities.
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified)
                .ToList();

            foreach (var entityEntry in entities)
            {
                if (entityEntry.Entity is BaseModel baseModel)
                {
                    baseModel.UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        // Override SaveChanges to set the UpdatedAt property for modified entities.
        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified)
                .ToList();

            foreach (var entityEntry in entities)
            {
                if (entityEntry.Entity is BaseModel baseModel)
                {
                    baseModel.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }
    }
}
