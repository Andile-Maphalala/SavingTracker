

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SavingTracker.Data.Models;

namespace SavingTracker.Data.Context
{
    public class ContributionConfiguration : IEntityTypeConfiguration<Contribution>
    {
        public void Configure(EntityTypeBuilder<Contribution> builder)
        {
            builder.ToTable(nameof(Contribution));

            //Key
            builder.HasKey(c => c.Id);

            //Properties
            builder.Property(c => c.MemberId)
                .IsRequired();

            builder.Property(c => c.Date)
                .IsRequired();

            builder.Property(c => c.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

        }
    }
}
