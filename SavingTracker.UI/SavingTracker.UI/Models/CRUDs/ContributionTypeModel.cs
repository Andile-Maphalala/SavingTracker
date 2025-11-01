using SavingTracker.UI.Models.Common;

namespace SavingTracker.UI.Models.CRUDs
{
    public class ContributionTypeModel
    {
        [CustomColumn(DisplayName = "Id", Order = 0, Visible = false)]
        public int Id { get; set; }

        [CustomColumn(DisplayName = "Amount", Order = 1)]
        public string Name { get; set; }

        [CustomColumn(DisplayName = "Amount", Order = 2, FormatString = "N2")]
        public decimal Amount { get; set; }

        [CustomColumn(DisplayName = "Frequency Id", Order = 3)]
        public int Frequency { get; set; }

        [CustomColumn(DisplayName = "FrequencyName", Order = 4, Visible = false)]
        public string FrequencyName { get; set; }

    }
}
