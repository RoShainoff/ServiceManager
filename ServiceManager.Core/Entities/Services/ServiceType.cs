namespace ServiceManager.Core.Entities.Services
{
    public class ServiceType : BaseEntity
    {
        public string Name { get; set; } = null!;

        public List<Service> Services { get; set; } = new();
    }
}