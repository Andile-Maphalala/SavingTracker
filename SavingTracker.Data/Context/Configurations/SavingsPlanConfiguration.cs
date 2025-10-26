using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SavingTracker.Data.Models;

namespace SavingTracker.Data.Context
{
    public class SavingsPlanConfiguration : IEntityTypeConfiguration<SavingsPlan>
    {
        public void Configure(EntityTypeBuilder<SavingsPlan> builder)
        {
            builder.ToTable(nameof(SavingsPlan));

            //key
            builder.HasKey(sp => sp.Id);

            //properties
            builder.Property(sp => sp.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(sp => sp.Description)
                 .IsRequired(false)
                 .HasMaxLength(1500);

            builder.Property(sp => sp.TargetAmount)
                .HasDefaultValue(0)
                .HasColumnType("decimal(18,2)");

            builder.Property(sp => sp.StartDate)
                .IsRequired();

            builder.Property(sp => sp.EndDate);

            //relationships
            builder.HasMany(s => s.Members)
                .WithOne(m => m.SavingsPlan)
                .HasForeignKey(m => m.SavingsPlanId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
