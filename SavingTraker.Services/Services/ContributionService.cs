using Microsoft.EntityFrameworkCore;
using SavingTracker.Data.Context;
using SavingTracker.Data.Models;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Exceptions;
using SavingTraker.App.Interfaces;


namespace SavingTraker.App.Services
{
    public class ContributionService : IContributionService
    {
        private readonly AppDbContext _db;

        public ContributionService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ContributionDto>> GetAll(CancellationToken cancellationToken)
        {
            return await _db.Contributions
                .Select(c => new ContributionDto
                {
                    Id = c.Id,
                    MemberId = c.Member.Id,
                    MemberName = c.Member.FullName,
                    Date = c.Date,
                    Amount = c.Amount
                }).ToListAsync(cancellationToken);
        }

        public async Task<List<ContributionDto>> GetAllByMemberId(int memberId, CancellationToken cancellationToken)
        {
            return await _db.Contributions
                .Where(c => c.Member.Id == memberId)
                .Select(c => new ContributionDto
                {
                    Id = c.Id,
                    MemberId = c.Member.Id,
                    MemberName = c.Member.FullName,
                    Date = c.Date,
                    Amount = c.Amount
                }).ToListAsync(cancellationToken);
        }

        public async Task<ContributionDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var entity = await _db.Contributions.FindAsync(Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("Contribution", Id);
            }

            return new ContributionDto
            {
                Id = entity.Id,
                MemberId = entity.Member.Id,
                MemberName = entity.Member.FullName,
                Date = entity.Date,
                Amount = entity.Amount
            };
        }

        public async Task<int> UpSert(ContributionDto dto, CancellationToken cancellationToken)
        {
            var entity = await _db.Contributions.FindAsync(dto.Id, cancellationToken);
            if (entity == null)
            {
                entity = new Contribution
                {
                    MemberId = dto.MemberId,
                    Date = dto.Date,
                    Amount = dto.Amount
                };
                _db.Contributions.Add(entity);
            }
            else
            {
                entity.Date = dto.Date;
                entity.Amount = dto.Amount;
                _db.Contributions.Update(entity);
            }
            await _db.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        public async Task<int> Delete(int Id, CancellationToken cancellationToken)
        {
            var entity = await _db.Contributions.FindAsync(Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("Contribution", Id);
            }
            _db.Contributions.Remove(entity);
            return await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<int>> BulkUpsert(List<ContributionDto> dto, CancellationToken cancellationToken)
        {
            List<int> ids = new List<int>();
            foreach (var item in dto)
            {
                await UpSert(item, cancellationToken);
                ids.Add(item.Id);
            }

            return ids;
        }

    }
}
