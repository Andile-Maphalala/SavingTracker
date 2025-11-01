using SavingTracker.UI.Models.Common;

namespace SavingTracker.UI.Models.CRUDs
{
    public class MemberModel
    {
        [CustomColumn(DisplayName = "Id", Order = 0, Visible = false)]
        public int Id { get; set; }

        [CustomColumn(DisplayName = "Name", Order = 1)]
        public string FullName { get; set; }

        [CustomColumn(DisplayName = "Name", Order = 2, Visible = false)]
        public int SavingsPlanId { get; set; }

        [CustomColumn(DisplayName = "Savings Plan", Order = 3)]
        public string SavingsPlanName { get; set; }

        [CustomColumn(DisplayName = "Contribution Type Id", Order = 4, Visible = false)]
        public int ContributionTypeId { get; set; }

        [CustomColumn(DisplayName = "Contribution Type", Order = 5)]
        public string ContributionTypeName { get; set; }

        [CustomColumn(DisplayName = "Amount", Order = 6, FormatString = "N2")]
        public decimal Amount { get; set; }

        [CustomColumn(DisplayName = "Frequency Id", Order = 7, Visible = true)]
        public int Frequency { get; set; }

        [CustomColumn(DisplayName = "FrequencyName", Order = 8, Visible = true)]
        public string FrequencyName { get; set; }

    }
}
