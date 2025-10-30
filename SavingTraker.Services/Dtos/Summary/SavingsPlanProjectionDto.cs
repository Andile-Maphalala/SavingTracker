

namespace SavingTraker.App.Dtos.Summary
{
    public class SavingsPlanProjectionDto
    {
        public decimal TargetAmount { get; set; }
        public List<SavingsProjectionPointDto> Projection { get; set; } = new();
    }
}
