using Microsoft.EntityFrameworkCore;
using SavingTracker.Data.Context;
using SavingTracker.Data.Enums;
using SavingTracker.Data.Models;
using SavingTraker.App.Dtos.Summary;
using SavingTraker.App.Exceptions;
using SavingTraker.App.Interfaces;
using System.Threading;

namespace SavingTraker.App.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _db;

        public DashboardService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<DashboardSummaryDto> GetDashboardSummary(int savingPlanId, int FrequncyType, CancellationToken cancellationToken)
        {

            DashboardSummaryDto summary = new DashboardSummaryDto();
            var savingsPlan = await _db.SavingsPlans.FindAsync(savingPlanId, cancellationToken);
            if (savingsPlan == null)
            {
                throw new NotFoundException("Savings Plan", savingPlanId);
            }

            var members = _db.Members
                .Include(x => x.ContributionType)
                .Include(x => x.Contributions)
                .Where(m => m.SavingsPlan.Id == savingPlanId);
                
            var frequency = (ContributionFrequency)FrequncyType;
            foreach (var member in members)
            {
                MemberDashboardSummaryDto dto = new MemberDashboardSummaryDto();
                dto.MemberId = member.Id;
                dto.MemberName = member.FullName;
                dto.PeriodFee = ConvertAmount(member.ContributionType.Amount, (ContributionFrequency)member.ContributionType.Frequency, frequency);
                dto.TotalContributedAmount = member.Contributions.Sum(c => c.Amount);
                dto.PeriodsPaid = CalulatePeriodsPaid(dto.TotalContributedAmount, dto.PeriodFee);
                dto.OustandingAmount = CalculateOutandingAmount(dto.PeriodFee,frequency,dto.TotalContributedAmount,savingsPlan.StartDate);
                dto.AllContributions = member.Contributions
                    .Select(c => new ContributionSummaryDto
                    {
                        Id = c.Id,
                        Date = c.Date,
                        Amount = c.Amount
                    }).ToList();
                summary.MemberSummaries.Add(dto);
            }
            summary.TotalMembers = summary.MemberSummaries.Count;
            summary.TotalContribution = summary.MemberSummaries.Sum(m => m.TotalContributedAmount);
            summary.TotalOutstandingAmount = summary.MemberSummaries.Sum(m => m.OustandingAmount);
            summary.PeriodIncome = summary.MemberSummaries.Sum(m => m.PeriodFee);
            summary.TotalPeriods = GetTotalPeriods(savingsPlan.StartDate, frequency);

            return summary;
        }


        public async Task<SavingsPlanProjectionDto> GetSavingProjection(int savingPlanId, decimal TargetAmount, int FrequncyType, int projectedMonths, decimal interestRate, CancellationToken cancellationToken)
        {
            SavingsPlanProjectionDto result = new SavingsPlanProjectionDto();
            var projection = new List<SavingsProjectionPointDto>();
            var savingsPlan = await _db.SavingsPlans.FindAsync(savingPlanId, cancellationToken);
            if (savingsPlan == null)
            {
                throw new NotFoundException("Savings Plan", savingPlanId);
            }

            var frequency = (ContributionFrequency)FrequncyType;
            var totalPeriods = GetTotalPeriods(savingsPlan.StartDate, frequency);
            var members = _db.Members.Include(x => x.Contributions).Where(m => m.SavingsPlan.Id == savingPlanId);
            decimal total = members.Sum(x => x.Contributions.Sum(c => c.Amount));
            var contributionAmount = members.Sum(x => ConvertAmount(x.ContributionType.Amount, (ContributionFrequency)x.ContributionType.Frequency,frequency));
            List<decimal> convertedInterests = new List<decimal>();
            foreach (var member in members)
            {
                var convertedInterest = ConvertAmount(interestRate, (ContributionFrequency)member.ContributionType.Frequency, frequency);
                convertedInterests.Add(convertedInterest);
            }


            return result;
        }

        public decimal ConvertAmount(decimal Amount, ContributionFrequency contributionFrequency, ContributionFrequency targetFrequency)
        {
            // Get how many times each frequency occurs in a year
            decimal sourcePerYear = GetFrequencyPerYear(contributionFrequency);
            decimal targetPerYear = GetFrequencyPerYear(targetFrequency);

            // Convert source amount to annual total, then divide by target frequency count
            decimal annualAmount = Amount * sourcePerYear;
            decimal converted = annualAmount / targetPerYear;

            return Math.Round(converted, 2);
        }

        private static decimal GetFrequencyPerYear(ContributionFrequency frequency)
        {
            switch (frequency)
            {
                case ContributionFrequency.Daily:
                    return 365m;
                case ContributionFrequency.Weekly:
                    return 52m;
                case ContributionFrequency.BiWeekly:
                    return 26m;
                case ContributionFrequency.Monthly:
                    return 12m;
                case ContributionFrequency.Quarterly:
                    return 4m;
                case ContributionFrequency.SemiAnnual:
                    return 2m;
                case ContributionFrequency.Yearly:
                    return 1m;
                default:
                    throw new ArgumentOutOfRangeException(nameof(frequency), frequency, "Unsupported contribution frequency");
            }
        }

        private int CalulatePeriodsPaid(decimal totalContributed, decimal periodFee)
        {
            if (periodFee == 0) return 0;
            return (int)(totalContributed / periodFee);
        }

        private decimal CalculateOutandingAmount(decimal periodFee, ContributionFrequency frequency, decimal totalContributed, DateTime startDate)
        {
            switch(frequency)
            {       case ContributionFrequency.Daily:
                    int days = (DateTime.Now - startDate).Days;
                    decimal expectedDailyTotal = days * periodFee;
                    return Math.Max(0, expectedDailyTotal - totalContributed);
                case ContributionFrequency.Weekly:
                    int weeks = (DateTime.Now - startDate).Days / 7;
                    decimal expectedWeeklyTotal = weeks * periodFee;
                    return Math.Max(0, expectedWeeklyTotal - totalContributed);
                case ContributionFrequency.BiWeekly:
                    int biWeeks = (DateTime.Now - startDate).Days / 14;
                    decimal expectedBiWeeklyTotal = biWeeks * periodFee;
                    return Math.Max(0, expectedBiWeeklyTotal - totalContributed);
                case ContributionFrequency.Monthly:
                    int months = ((DateTime.Now.Year - startDate.Year) * 12) + DateTime.Now.Month - startDate.Month;
                    decimal expectedMonthlyTotal = months * periodFee;
                    return Math.Max(0, expectedMonthlyTotal - totalContributed);
                case ContributionFrequency.Quarterly:
                    int quarters = (((DateTime.Now.Year - startDate.Year) * 12) + DateTime.Now.Month - startDate.Month) / 3;
                    decimal expectedQuarterlyTotal = quarters * periodFee;
                    return Math.Max(0, expectedQuarterlyTotal - totalContributed);
                case ContributionFrequency.SemiAnnual:
                    int semiAnnuals = (((DateTime.Now.Year - startDate.Year) * 12) + DateTime.Now.Month - startDate.Month) / 6;
                    decimal expectedSemiAnnualTotal = semiAnnuals * periodFee;
                    return Math.Max(0, expectedSemiAnnualTotal - totalContributed);
                case ContributionFrequency.Yearly:
                    int years = DateTime.Now.Year - startDate.Year;
                    decimal expectedYearlyTotal = years * periodFee;
                    return Math.Max(0, expectedYearlyTotal - totalContributed);
                default:
                    throw new ArgumentOutOfRangeException(nameof(frequency), frequency, "Unsupported contribution frequency");
            }
        }

        private int GetTotalPeriods(DateTime startDate, ContributionFrequency frequency)
        {
            var currentDate = DateTime.Now;
            if(currentDate < startDate)
            {
                return 0;
            }

            switch (frequency)
            {
                case ContributionFrequency.Daily:
                    return (currentDate - startDate).Days;
                case ContributionFrequency.Weekly:
                    return (currentDate - startDate).Days / 7;
                case ContributionFrequency.BiWeekly:
                    return (currentDate - startDate).Days / 14;
                case ContributionFrequency.Monthly:
                    return ((currentDate.Year - startDate.Year) * 12) + currentDate.Month - startDate.Month;
                case ContributionFrequency.Quarterly:
                    return (((currentDate.Year - startDate.Year) * 12) + currentDate.Month - startDate.Month) / 3;
                case ContributionFrequency.SemiAnnual:
                    return(((currentDate.Year - startDate.Year) * 12) + currentDate.Month - startDate.Month) / 6;
                case ContributionFrequency.Yearly:
                    return currentDate.Year - startDate.Year;
                default:
                    throw new ArgumentOutOfRangeException(nameof(frequency), frequency, "Unsupported contribution frequency");
            }
        }

    }
}
