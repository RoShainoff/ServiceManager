using ServiceManager.Core.Entities.Requests;

namespace ServiceManager.Core.Entities.Services
{
    public class Service : BaseEntity
    {
        public string Name { get; set; } = null!;
        public decimal Cost { get; set; }
        public int Hours { get; set; }

        public Guid? ServiceTypeId { get; set; }
        public ServiceType? ServiceType { get; set; }

        public List<Request> Requests { get; set; } = new();
    }
}