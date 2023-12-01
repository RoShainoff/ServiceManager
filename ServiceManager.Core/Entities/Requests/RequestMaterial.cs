using ServiceManager.Core.Entities.Services;

namespace ServiceManager.Core.Entities.Requests
{
    public class RequestMaterial
    {
        public int Count { get; set; }

        public Guid RequestId { get; set; }
        public Request Request { get; set; } = null!;

        public Guid MaterialId { get; set; }
        public Material Material { get; set; } = null!;
    }
}
