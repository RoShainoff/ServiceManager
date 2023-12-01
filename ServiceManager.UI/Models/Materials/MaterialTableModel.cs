using ServiceManager.UI.Models.Base;

namespace ServiceManager.UI.Models.Materials
{
    public class MaterialTableModel : BaseTableModel
    {
        public string Name { get; set; } = null!;
        public int Count { get; set; }
        public int RequestsCount { get; set; }

        public override bool CanDelete => RequestsCount == 0;
    }
}
