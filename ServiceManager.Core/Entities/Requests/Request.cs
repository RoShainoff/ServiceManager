using ServiceManager.Core.Entities.Identity;
using ServiceManager.Core.Entities.Services;
using ServiceManager.Core.Enums;

namespace ServiceManager.Core.Entities.Requests
{
    public class Request : BaseEntity
    {
        public int Number { get; set; } = default!;
        public string Text { get; set; } = null!;
        public RequestPriority Priority { get; set; }

        public Guid ClientId { get; set; }
        public User Client { get; set; } = null!;

        public Guid? ExecutorId { get; set; }
        public User? Executor { get; set; }

        public Guid? ServiceTypeId { get; set; }
        public ServiceType? ServiceType { get; set; }

        public Guid? ServiceId { get; set; }
        public Service? Service { get; set; }

        public List<RequestHistory> Histories { get; set; } = new();
        public List<RequestMaterial> RequestMaterials { get; set; } = new();
        public List<Material> Materials { get; set; } = new();
    }
}