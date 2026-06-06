

using Microsoft.EntityFrameworkCore;
using SavingTracker.Data.Context;
using SavingTracker.Data.Models;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Exceptions;
using SavingTraker.App.Interfaces;

namespace SavingTraker.App.Services
{
    public class SavingsPlanService(AppDbContext db, IUserInfo userInfo) : ISavingsPlanService
    {

        public async Task<List<SavingsPlanDto>> GetAll(CancellationToken cancellationToken)
        {
            if (userInfo.IsAdmin())
            {
                return await db.SavingsPlans
                    .Select(sp => new SavingsPlanDto
                    {
                        Id = sp.Id,
                        Name = sp.Name,
                        Description = sp.Description,
                        TargetAmount = sp.TargetAmount,
                        StartDate = sp.StartDate,
                        EndDate = sp.EndDate
                    }).ToListAsync(cancellationToken);
            }
            else
            {
                var userId = userInfo.GetUserId();
                return await db.UserSavingsPlans
                    .Include(sp => sp.SavingsPlan)
                    .Where(sp => sp.UserId == userId)
                    .Select(sp => new SavingsPlanDto
                    {
                        Id = sp.SavingsPlan.Id,
                        Name = sp.SavingsPlan.Name,
                        Description = sp.SavingsPlan.Description,
                        TargetAmount = sp.SavingsPlan.TargetAmount,
                        StartDate = sp.SavingsPlan.StartDate,
                        EndDate = sp.SavingsPlan.EndDate
                    }).ToListAsync(cancellationToken);
            }
        }

        public async Task<SavingsPlanDto> GetById(int Id, CancellationToken cancellationToken)
        {
            if (!userInfo.IsAdmin())
            {
                throw new Exception("Access denied. Tried to perform unauthorized action.");
            }
            var entity = await db.SavingsPlans.FindAsync(Id, cancellationToken);

            if(entity == null)
            {
                throw new NotFoundException("Savings Plan", Id);
            }
            return new SavingsPlanDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                TargetAmount = entity.TargetAmount,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate
            };
        }

        public async Task<int> UpSert(SavingsPlanDto dto, CancellationToken cancellationToken)
        {
            if (!userInfo.IsAdmin())
            {
                throw new Exception("Access denied. Tried to perform unauthorized action.");
            }
            var entity = await db.SavingsPlans.FindAsync(dto.Id, cancellationToken);
            if (entity == null)
            {
                entity = new SavingsPlan
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    TargetAmount = dto.TargetAmount,
                    StartDate  = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
                    EndDate = dto.EndDate
                };
                if (entity.EndDate.HasValue)
                {
                    entity.EndDate = DateTime.SpecifyKind(entity.EndDate.Value, DateTimeKind.Utc);
                }

                db.SavingsPlans.Add(entity);
            }
            else
            {
                entity.Name = dto.Name;
                entity.Description = dto.Description;
                entity.TargetAmount = dto.TargetAmount;
                entity.StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc);
                if (entity.EndDate.HasValue)
                {
                    entity.EndDate = DateTime.SpecifyKind(entity.EndDate.Value, DateTimeKind.Utc);
                }
            }
            await db.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task<int> Delete(int Id, CancellationToken cancellationToken)
        {
            if (!userInfo.IsAdmin())
            {
                throw new Exception("Access denied. Tried to perform unauthorized action.");
            }
            var entity = await db.SavingsPlans.FindAsync(Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("Savings Plan", Id);
            }
            db.SavingsPlans.Remove(entity);
            return await db.SaveChangesAsync(cancellationToken);

        }
    }
}
