
using Microsoft.EntityFrameworkCore;
using SavingTracker.Data.Context;
using SavingTracker.Data.Models;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Exceptions;
using SavingTraker.App.Interfaces;

namespace SavingTraker.App.Services
{
    public class ContributionTypeService : IContributionTypeService
    {
        private readonly AppDbContext _db;

        public ContributionTypeService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ContributionTypeDto>> GetAll(CancellationToken cancellationToken)
        {
            return await _db.ContributionTypes
                .Select(ct => new ContributionTypeDto
                {
                    Id = ct.Id,
                    Name = ct.Name,
                    Amount = ct.Amount,
                    Frequency = ct.Frequency,
                }).ToListAsync(cancellationToken);
        }

        public async Task<ContributionTypeDto?> GetById(int Id, CancellationToken cancellationToken)
        {
            var entity = await _db.ContributionTypes.FindAsync(Id, cancellationToken);
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
            };
        }

        public async Task<int> UpSert(ContributionTypeDto dto, CancellationToken cancellationToken)
        {
            var entity = await _db.ContributionTypes.FindAsync(dto.Id, cancellationToken);
            if (entity == null)
            {
                entity = new ContributionType
                {
                    Name = dto.Name,
                    Amount = dto.Amount,
                    Frequency = dto.Frequency,
                };
                _db.ContributionTypes.Add(entity);
            }
            else
            {
                entity.Name = dto.Name;
                entity.Amount = dto.Amount;
                entity.Frequency = dto.Frequency;
            }
            await _db.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        public async Task<int> Delete(int Id, CancellationToken cancellationToken)
        {
            var entity = await _db.ContributionTypes.FindAsync(Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("ContributionType", Id);
            }

            _db.ContributionTypes.Remove(entity);
            return await _db.SaveChangesAsync(cancellationToken);

        }

    }
}
