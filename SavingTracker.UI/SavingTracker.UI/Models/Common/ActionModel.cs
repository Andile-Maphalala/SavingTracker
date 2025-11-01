using SavingTracker.UI.Models.Enums;

namespace SavingTracker.UI.Models.Common
{
    public class ActionModel<TItem>
    {
        public TItem Item { get; set; } = default!;
        public FormModeEnum Mode { get; set; }
    }
}
