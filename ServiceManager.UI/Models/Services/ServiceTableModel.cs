using ServiceManager.UI.Models.Base;

namespace ServiceManager.UI.Models.Services
{
    public class ServiceTableModel : BaseTableModel
    {
        public string Name { get; set; } = null!;
        public decimal Cost { get; set; }
        public int Hours { get; set; }
        public int RequestCount { get; set; }

        public NamedModel? ServiceType { get; set; }

        public override bool CanDelete => RequestCount == 0;
    }
}
