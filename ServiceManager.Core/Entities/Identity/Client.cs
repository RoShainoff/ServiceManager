namespace ServiceManager.Core.Entities.Identity
{
    public class Client : BaseEntity
    {
        public string? RoomName { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
