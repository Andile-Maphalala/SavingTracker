

using Microsoft.EntityFrameworkCore;
using SavingTracker.Data.Context;
using SavingTracker.Data.Models;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Exceptions;
using SavingTraker.App.Interfaces;

namespace SavingTraker.App.Services
{
    public class SavingsPlanService : ISavingsPlanService
    {
        private readonly AppDbContext _db;

        public SavingsPlanService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<SavingsPlanDto>> GetAll(CancellationToken cancellationToken)
        {
            return await _db.SavingsPlans
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

        public async Task<SavingsPlanDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var entity = await _db.SavingsPlans.FindAsync(Id, cancellationToken);

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
            var entity = await _db.SavingsPlans.FindAsync(dto.Id, cancellationToken);
            if (entity == null)
            {
                entity = new SavingsPlan
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    TargetAmount = dto.TargetAmount,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate
                };
                _db.SavingsPlans.Add(entity);
            }
            else
            {
                entity.Name = dto.Name;
                entity.Description = dto.Description;
                entity.TargetAmount = dto.TargetAmount;
                entity.StartDate = dto.StartDate;
                entity.EndDate = dto.EndDate;
            }
            await _db.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task<int> Delete(int Id, CancellationToken cancellationToken)
        {
            var entity = await _db.SavingsPlans.FindAsync(Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("Savings Plan", Id);
            }
            _db.SavingsPlans.Remove(entity);
            return await _db.SaveChangesAsync(cancellationToken);

        }
    }
}
