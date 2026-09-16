
namespace SavingTracker.UI.Services.Helpers
{
    public static class DateHelper
    {
        public static string GetDateFormat<T>(this T? model, string? propertyName)
        {
            if(model == null || string.IsNullOrEmpty(propertyName))
            {
                return "dd/MM/yyyy";
            }
            return model.GetFormatString(propertyName);
        }
    }
}
