

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SavingTracker.Data.Models;

namespace SavingTracker.Data.Context
{
    public class UserSavingsPlanConfiguration : IEntityTypeConfiguration<UserSavingsPlan>
    {
        public void Configure(EntityTypeBuilder<UserSavingsPlan> builder)
        {
            //Key
            builder.HasKey(ct => ct.Id);

            //Relationships
            builder.HasOne(ct => ct.User)
                .WithMany(u => u.UserSavingsPlans)
                .HasForeignKey(ct => ct.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ct => ct.SavingsPlan)
                .WithMany(sp => sp.UserSavingsPlans)
                .HasForeignKey(ct => ct.SavingsPlanId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
