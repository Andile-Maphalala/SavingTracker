using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SavingTracker.UI.Models.Common
{
    public class ColumnDefinition
    {
        public PropertyInfo PropertyInfo { get; }
        public string DisplayName { get; }
        public int Order { get; }
        public bool Visible { get; }
        public string? FormatString { get; }

        public ColumnDefinition(PropertyInfo property)
        {
            PropertyInfo = property;

            var displayAttr = property.GetCustomAttribute<DisplayAttribute>();
            var scaffoldAttr = property.GetCustomAttribute<ScaffoldColumnAttribute>();
            var displayFormat = property.GetCustomAttribute<DisplayFormatAttribute>();

            DisplayName = displayAttr?.Name ?? property.Name;
            Order = displayAttr?.GetOrder() ?? 0;
            Visible = scaffoldAttr?.Scaffold ?? true;
            FormatString = displayFormat?.DataFormatString;
        }
    }
}
