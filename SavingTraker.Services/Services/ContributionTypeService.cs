
using Microsoft.EntityFrameworkCore;
using SavingTracker.Data.Context;
using SavingTracker.Data.Enums;
using SavingTracker.Data.Models;
using SavingTraker.App.Common.Helpers;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Dtos.Lookup;
using SavingTraker.App.Exceptions;
using SavingTraker.App.Interfaces;

namespace SavingTraker.App.Services
{
    public class ContributionTypeService(IUserInfo userInfo, AppDbContext db) : IContributionTypeService
    {

        public async Task<List<ContributionTypeDto>> GetAll(CancellationToken cancellationToken)
        {
            return await db.ContributionTypes
                .Select(ct => new ContributionTypeDto
                {
                    Id = ct.Id,
                    Name = ct.Name,
                    Amount = ct.Amount,
                    Frequency = ct.Frequency,
                    FrequencyName = ct.Frequency.GetFrequencyName()
                }).ToListAsync(cancellationToken);
        }

        public async Task<ContributionTypeDto?> GetById(int Id, CancellationToken cancellationToken)
        {
            var entity = await db.ContributionTypes.FindAsync(Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("ContributionType", Id);
            }

            return new ContributionTypeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Amount = entity.Amount,
                Frequency = entity.Frequency,
                FrequencyName = entity.Frequency.GetFrequencyName()
            };
        }

        public async Task<int> UpSert(ContributionTypeDto dto, CancellationToken cancellationToken)
        {
            if (!userInfo.IsAdmin())
            {
                throw new Exception("Access denied. Tried to perform unauthorized action.");
            }
            var entity = await db.ContributionTypes.FindAsync(dto.Id, cancellationToken);
            if (entity == null)
            {
                entity = new ContributionType
                {
                    Name = dto.Name,
                    Amount = dto.Amount,
                    Frequency = dto.Frequency,
                };
                db.ContributionTypes.Add(entity);
            }
            else
            {
                entity.Name = dto.Name;
                entity.Amount = dto.Amount;
                entity.Frequency = dto.Frequency;
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
            var entity = await db.ContributionTypes.FindAsync(Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("ContributionType", Id);
            }

            db.ContributionTypes.Remove(entity);
            return await db.SaveChangesAsync(cancellationToken);

        }

        public List<LookUpDto> GetContributionFrequenyList()
        {
            return Enum.GetValues<ContributionFrequency>()
                .Select(cf => new LookUpDto
                {
                    Id = (int)cf,
                    Name = cf.ToString()
                }).ToList();
        }
    }
}
