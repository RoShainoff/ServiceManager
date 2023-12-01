using ServiceManager.UI.Models.Base;

namespace ServiceManager.UI.Models.Services
{
    public class ServiceTypeTableModel : BaseTableModel
    {
        public string Name { get; set; } = null!;
        public int ServiceCount { get; set; }

        public override bool CanDelete => ServiceCount == 0;
    }
}
