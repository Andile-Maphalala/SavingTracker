using SavingTracker.UI.Models.Common;

namespace SavingTracker.UI.Models.CRUDs
{
    public class ContributionModel
    {
        [CustomColumn(DisplayName = "Id", Order = 0, Visible = false)]
        public int Id { get; set; }

        [CustomColumn(DisplayName = "Member Id", Order = 1, Visible = false)]
        public int MemberId { get; set; }

        [CustomColumn(DisplayName = "Member Name", Order = 2)]
        public string MemberName { get; set; }

        [CustomColumn(DisplayName = "Date", Order = 3, FormatString = "yyyy/MM/dd")]
        public DateTime? Date { get; set; }

        [CustomColumn(DisplayName = "Amount", Order = 4, FormatString = "N2")]
        public decimal Amount { get; set; }
    }
}
