using ServiceManager.Core.Entities.Identity;

namespace ServiceManager.Core.Entities.Requests
{
    public class RequestNotify : BaseEntity
    {
        public string Text { get; set; } = null!;
        public bool IsRead { get; set; }

        public Guid RequestHistoryId { get; set; }
        public RequestHistory RequestHistory { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
