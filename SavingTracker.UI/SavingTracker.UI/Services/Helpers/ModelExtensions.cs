using SavingTracker.UI.Models.Common;
using System.Reflection;

namespace SavingTracker.UI.Services.Helpers
{
    public static class ModelExtensions
    {
        public static string GetFormatString<T>(this T model, string propertyName)
        {
            var prop = typeof(T).GetProperty(propertyName);
            var attr = prop?.GetCustomAttribute<CustomColumnAttribute>();
            return attr?.FormatString;
        }
    }
}
