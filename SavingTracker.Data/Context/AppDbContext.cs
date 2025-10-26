

using Microsoft.EntityFrameworkCore;
using SavingTracker.Data.Models;

namespace SavingTracker.Data.Context
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Contribution> Contributions { get; set; }
        public virtual DbSet<ContributionType> ContributionTypes { get; set; }
        public virtual DbSet<SavingsPlan> SavingsPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.ApplyConfiguration(new MemberConfiguration());
            modelBuilder.ApplyConfiguration(new ContributionConfiguration());
            modelBuilder.ApplyConfiguration(new ContributionTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SavingsPlanConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
