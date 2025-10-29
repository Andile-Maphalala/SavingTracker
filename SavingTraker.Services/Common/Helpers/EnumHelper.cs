using SavingTracker.Data.Enums;

namespace SavingTraker.App.Common.Helpers
{
    public static class EnumHelper
    {
        public static string GetFrequencyName(this int frequency)
        {
            var enumValue = (ContributionFrequency)frequency;
            return enumValue.ToString();
        }
    }
}
