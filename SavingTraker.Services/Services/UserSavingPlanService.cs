

using Microsoft.EntityFrameworkCore;
using SavingTracker.Data.Context;
using SavingTracker.Data.Models;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Interfaces;

namespace SavingTraker.App.Services
{
    public class UserSavingPlanService(IUserInfo userInfo, AppDbContext db) : IUserSavingPlanService
    {
        public async Task<List<SavingsPlanDto>> GetAllUserSavingsPlansAsync(string userId, CancellationToken cancellationToken)
        {
            if (!userInfo.IsAdmin())
            {
                throw new Exception("Access denied. Tried to perform unauthorized action.");
            }

            var result =  await db.UserSavingsPlans
                .Where(usp => usp.UserId == userId)
                .Include(usp => usp.SavingsPlan)
                .Select(usp => new SavingsPlanDto
                {
                    Id = usp.SavingsPlan.Id,
                    Name = usp.SavingsPlan.Name,
                    TargetAmount = usp.SavingsPlan.TargetAmount,
                    StartDate = usp.SavingsPlan.StartDate,
                    EndDate = usp.SavingsPlan.EndDate
                })
                .ToListAsync(cancellationToken);

            return result;
        }

        public async Task SaveUserSavingPlanAsync(UserSavingsPlanDto userSavingsPlan, CancellationToken cancellationToken)
        {
            if (!userInfo.IsAdmin())
            {
                throw new Exception("Access denied. Tried to perform unauthorized action.");
            }

            if(userSavingsPlan.SavingsPlanIds != null && userSavingsPlan.SavingsPlanIds.Any())
            {
                var dbSavingPlans =  await db.UserSavingsPlans.Where(usp => usp.UserId == userSavingsPlan.UserId).ToListAsync();
                foreach (var dbSavingPlan in dbSavingPlans)
                {
                    if (!userSavingsPlan.SavingsPlanIds.Contains(dbSavingPlan.SavingsPlanId))
                    {
                        db.UserSavingsPlans.Remove(dbSavingPlan);
                    }
                }

                foreach (var savingsPlanId in userSavingsPlan.SavingsPlanIds)
                {
                    if (!dbSavingPlans.Any(dbsp => dbsp.SavingsPlanId == savingsPlanId))
                    {
                        db.UserSavingsPlans.Add(new UserSavingsPlan
                        {
                            UserId = userSavingsPlan.UserId,
                            SavingsPlanId = savingsPlanId
                        });
                    }
                }

                await db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
