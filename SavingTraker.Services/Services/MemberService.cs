

using Microsoft.EntityFrameworkCore;
using SavingTracker.Data.Context;
using SavingTracker.Data.Models;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Exceptions;
using SavingTraker.App.Interfaces;

namespace SavingTraker.App.Services
{

    public class MemberService : IMemberService
    {
        private readonly AppDbContext _db;

        public MemberService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<MemberDto>> GetAll(CancellationToken cancellationToken)
        {
            return await _db.Members
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
                }).ToListAsync(cancellationToken);
        }

        public async Task<MemberDto?> GetById(int Id, CancellationToken cancellationToken)
        {
            var entity = await _db.Members.FindAsync(Id, cancellationToken);
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
            };
        }

        public async Task<int> UpSert(MemberDto dto, CancellationToken cancellationToken)
        {
            var entity = await _db.Members.FindAsync(dto.Id, cancellationToken);
            if (entity == null)
            {
                entity = new Member
                {
                    FullName = dto.FullName,
                    SavingsPlanId = dto.SavingsPlanId,
                    ContributionTypeId = dto.ContributionTypeId
                };
                _db.Members.Add(entity);
            }
            else
            {
                entity.FullName = dto.FullName;
                entity.SavingsPlanId = dto.SavingsPlanId;
                entity.ContributionTypeId = dto.ContributionTypeId;
            }
            await _db.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        public async Task<int> Delete(int Id, CancellationToken cancellationToken)
        {
            var entity = await _db.Members.FindAsync(Id, cancellationToken);
            
            if (entity == null)
            {
                throw new NotFoundException("Member", Id);
            }
            _db.Members.Remove(entity);
            return await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<int>> BulkUpsert(List<MemberDto> dtos, CancellationToken cancellationToken)
        {
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
