using SavingTracker.UI.Models.Common;

namespace SavingTracker.UI.Models.CRUDs
{
    public class SavingsPlanModel
    {
        [CustomColumn(DisplayName = "Id",Order = 0)]
        public int Id { get; set; }

        [CustomColumn(DisplayName = "DisplayName", Order = 1)]
        public string Name { get; set; }

        [CustomColumn(DisplayName = "Description", Order = 2)]
        public string? Description { get; set; }

        [CustomColumn(DisplayName = "TargetAmount", Order = 3)]
        public decimal TargetAmount { get; set; }

        [CustomColumn(DisplayName = "StartDate", Order = 4, FormatString = "{0:yyyy/MM/dd}")]
        public DateTime? StartDate { get; set; }

        [CustomColumn(DisplayName = "EndDate", Order = 5,FormatString = "{0:yyyy/MM/dd}")]
        public DateTime? EndDate { get; set; }
    }
}
