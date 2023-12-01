namespace ServiceManager.UI.Models.Base
{
    public abstract class BaseTableModel : BaseModel
    {
        public abstract bool CanDelete { get; }
    }
}
