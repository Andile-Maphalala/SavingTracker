

namespace SavingTracker.UI.Models.Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CustomColumnAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public int Order { get; set; } = 0;
        public bool Visible { get; set; } = true;
        public string? FormatString { get; set; }
        public bool Searchable { get; set; } = true;
        public CustomColumnAttribute()
        {
        }
    }
}
