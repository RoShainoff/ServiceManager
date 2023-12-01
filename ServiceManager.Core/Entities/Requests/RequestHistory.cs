using ServiceManager.Core.Entities.Identity;
using ServiceManager.Core.Enums;

namespace ServiceManager.Core.Entities.Requests
{
    public class RequestHistory : BaseEntity
    {
        public string? Note { get; set; }
        public RequestAction Action { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public Guid RequestId { get; set; }
        public Request Request { get; set; } = null!;

        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }
}