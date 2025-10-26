using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SavingTracker.Data.Models;

namespace SavingTracker.Data.Context
{
    public class ContributionTypeConfiguration : IEntityTypeConfiguration<ContributionType>
    {
        public void Configure(EntityTypeBuilder<ContributionType> builder)
        {
            builder.ToTable(nameof(ContributionType));

            //Key
            builder.HasKey(ct => ct.Id);

            //Properties
            builder.Property(ct => ct.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ct => ct.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(ct => ct.Frequency)
                .IsRequired();

            //Relationships
            builder.HasMany(ct => ct.Members)
                .WithOne(m => m.ContributionType)
                .HasForeignKey(m => m.ContributionTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
