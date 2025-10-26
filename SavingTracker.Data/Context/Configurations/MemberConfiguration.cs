
using SavingTracker.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SavingTracker.Data.Context
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable(nameof(Member));

            //Key
            builder.HasKey(m => m.Id);

            //properties
            builder.Property(m => m.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.ContributionTypeId)
                .IsRequired();

            builder.Property(m => m.SavingsPlanId)
                .IsRequired();

            //Relationships
            builder.HasMany(m => m.Contributions)
                .WithOne(c => c.Member)
                .HasForeignKey(c => c.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
