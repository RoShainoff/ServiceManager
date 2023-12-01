using ServiceManager.UI.Models.Base;

namespace ServiceManager.UI.Models.Requests
{
    public class RequestMaterialModel : BaseModel
    {
        public Guid MaterialId { get; set; }
        public int Count { get; set; } = 1;
    }
}
