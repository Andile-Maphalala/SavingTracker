

using Microsoft.EntityFrameworkCore;
using SavingTracker.Data.Context;
using SavingTracker.Data.Models;
using SavingTraker.App.Common.Helpers;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Exceptions;
using SavingTraker.App.Interfaces;

namespace SavingTraker.App.Services
{

    public class MemberService(IUserInfo userInfo, AppDbContext db) : IMemberService
    {

        public async Task<List<MemberDto>> GetAll(CancellationToken cancellationToken)
        {
            return await db.Members
                .Select(m => new MemberDto
                {
                    Id = m.Id,
                    FullName = m.FullName,
                    SavingsPlanId = m.SavingsPlan.Id,
                    SavingsPlanName = m.SavingsPlan.Name,
                    ContributionTypeId = m.ContributionType.Id,
                    ContributionTypeName = m.ContributionType.Name,
                    Amount = m.ContributionType.Amount,
                    Frequency = m.ContributionType.Frequency,
                    FrequencyName = m.ContributionType.Frequency.GetFrequencyName()
                }).ToListAsync(cancellationToken);
        }

        public async Task<MemberDto?> GetById(int Id, CancellationToken cancellationToken)
        {
            var entity = await db.Members.FindAsync(Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("Member", Id);
            }
            return new MemberDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                SavingsPlanId = entity.SavingsPlan.Id,
                SavingsPlanName = entity.SavingsPlan.Name,
                ContributionTypeId = entity.ContributionType.Id,
                ContributionTypeName = entity.ContributionType.Name,
                Amount = entity.ContributionType.Amount,
                Frequency = entity.ContributionType.Frequency,
                FrequencyName = entity.ContributionType.Frequency.GetFrequencyName()
            };
        }

        public async Task<int> UpSert(MemberDto dto, CancellationToken cancellationToken)
        {
            if (!userInfo.IsAdmin())
            {
                throw new Exception("Access denied. Tried to perform unauthorized action.");
            }
            var entity = await db.Members.FindAsync(dto.Id, cancellationToken);
            if (entity == null)
            {
                entity = new Member
                {
                    FullName = dto.FullName,
                    SavingsPlanId = dto.SavingsPlanId,
                    ContributionTypeId = dto.ContributionTypeId
                };
                db.Members.Add(entity);
            }
            else
            {
                entity.FullName = dto.FullName;
                entity.SavingsPlanId = dto.SavingsPlanId;
                entity.ContributionTypeId = dto.ContributionTypeId;
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
            var entity = await db.Members.FindAsync(Id, cancellationToken);
            
            if (entity == null)
            {
                throw new NotFoundException("Member", Id);
            }
            db.Members.Remove(entity);
            return await db.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<int>> BulkUpsert(List<MemberDto> dtos, CancellationToken cancellationToken)
        {
            if (!userInfo.IsAdmin())
            {
                throw new Exception("Access denied. Tried to perform unauthorized action.");
            }
            List<int> ids = new List<int>();
            foreach (var dto in dtos)
            {
                var id = await UpSert(dto, cancellationToken);
                ids.Add(id);
            }

            return ids;
        }
    }
}
