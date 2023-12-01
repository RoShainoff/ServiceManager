using ServiceManager.Core.Entities.Requests;

namespace ServiceManager.Core.Entities.Services
{
    public class Material : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int Count { get; set; }

        public List<RequestMaterial> RequestMaterials { get; set; } = new();
        public List<Request> Requests { get; set; } = new();
    }
}
