using Microsoft.EntityFrameworkCore;
using SavingTracker.Data.Context;
using SavingTracker.Data.Models;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Exceptions;
using SavingTraker.App.Interfaces;


namespace SavingTraker.App.Services
{
    public class ContributionService(IUserInfo userInfo, AppDbContext db) : IContributionService
    {

        public async Task<List<ContributionDto>> GetAll(CancellationToken cancellationToken)
        {
            return await db.Contributions
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
            return await db.Contributions
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
            var entity = await db.Contributions.FindAsync(Id, cancellationToken);
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
            if (!userInfo.IsAdmin())
            {
                throw new Exception("Access denied. Tried to perform unauthorized action.");
            }
            var entity = await db.Contributions.FindAsync(dto.Id, cancellationToken);
            if (entity == null)
            {
                entity = new Contribution
                {
                    MemberId = dto.MemberId,
                    Date = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc),
                    Amount = dto.Amount
                };
                db.Contributions.Add(entity);
            }
            else
            {
                entity.Date = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc);
                entity.Amount = dto.Amount;
                db.Contributions.Update(entity);
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
            var entity = await db.Contributions.FindAsync(Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("Contribution", Id);
            }
            db.Contributions.Remove(entity);
            return await db.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<int>> BulkUpsert(List<ContributionDto> dto, CancellationToken cancellationToken)
        {

            if (!userInfo.IsAdmin())
            {
                throw new Exception("Access denied. Tried to perform unauthorized action.");
            }
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
