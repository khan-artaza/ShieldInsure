using Microsoft.EntityFrameworkCore;

namespace Final_Insure.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<ClaimDocument> ClaimDocuments { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<FraudCheck> FraudChecks { get; set; }
        public DbSet<SettlementLog> SettlementLogs { get; set; }
        public DbSet<InsurancePlan> InsurancePlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Column Type Specifications for Decimals
            modelBuilder.Entity<Policy>()
                .Property(p => p.CoverageAmount)
                .HasColumnType("decimal(15,2)");

            modelBuilder.Entity<Policy>()
                .Property(p => p.PremiumAmount)
                .HasColumnType("decimal(15,2)");

            modelBuilder.Entity<Claim>()
                .Property(c => c.ClaimAmount)
                .HasColumnType("decimal(15,2)");

            modelBuilder.Entity<Assessment>()
                .Property(a => a.AssessedAmount)
                .HasColumnType("decimal(15,2)");

            modelBuilder.Entity<SettlementLog>()
                .Property(s => s.SettlementAmount)
                .HasColumnType("decimal(15,2)");

            // New decimal specifications for InsurancePlan
            modelBuilder.Entity<InsurancePlan>()
                .Property(p => p.BasePremium)
                .HasColumnType("decimal(15,2)");

            modelBuilder.Entity<InsurancePlan>()
                .Property(p => p.CoverageAmount)
                .HasColumnType("decimal(15,2)");


            // 2. Relationship and Delete Behaviors
            modelBuilder.Entity<Policy>()
                .HasOne(p => p.Customer)
                .WithMany(u => u.Policies)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Customer)
                .WithMany(u => u.Claims)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Policy)
                .WithMany(p => p.Claims)
                .HasForeignKey(c => c.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link the Assigned Claim Officer
            modelBuilder.Entity<Claim>()
                .HasOne(c => c.AssignedOfficer)
                .WithMany()
                .HasForeignKey(c => c.AssignedOfficerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClaimDocument>()
                .HasOne(d => d.Claim)
                .WithMany(c => c.ClaimDocuments)
                .HasForeignKey(d => d.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Assessment>()
                .HasOne(a => a.Claim)
                .WithMany(c => c.Assessments)
                .HasForeignKey(a => a.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Assessment>()
                .HasOne(a => a.Surveyor)
                .WithMany(u => u.Assessments)
                .HasForeignKey(a => a.SurveyorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FraudCheck>()
                .HasOne(f => f.Claim)
                .WithMany(c => c.FraudChecks)
                .HasForeignKey(f => f.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);

            // Link the Compliance Officer to the Fraud Check
            modelBuilder.Entity<FraudCheck>()
                .HasOne(f => f.ComplianceOfficer)
                .WithMany()
                .HasForeignKey(f => f.ComplianceOfficerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SettlementLog>()
                .HasOne(s => s.Claim)
                .WithMany(c => c.SettlementLogs)
                .HasForeignKey(s => s.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);


            // 3. Seed the default Insurance Plans
            modelBuilder.Entity<InsurancePlan>().HasData(
                new InsurancePlan
                {
                    PlanId = 1,
                    PlanName = "Third-Party Mandatory",
                    PlanType = "Vehicle",
                    Description = "Covers damage to others (Third-Party liability only). Required by law.",
                    BasePremium = 7200m,
                    CoverageAmount = 800000m,
                    IsActive = true
                },
                new InsurancePlan
                {
                    PlanId = 2,
                    PlanName = "Comprehensive Car Insurance",
                    PlanType = "Vehicle",
                    Description = "Covers damage to your car + others (Natural disasters, theft, accidents).",
                    BasePremium = 18500m,
                    CoverageAmount = 2500000m,
                    IsActive = true
                }
            );
        }
    }
}