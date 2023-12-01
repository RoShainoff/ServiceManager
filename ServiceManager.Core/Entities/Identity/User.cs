using Microsoft.AspNetCore.Identity;
using ServiceManager.Core.Entities.Requests;

namespace ServiceManager.Core.Entities.Identity
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Patronymic { get; set; }

        public string FullName => LastName
            + (string.IsNullOrEmpty(FirstName) ? "" : $" {FirstName}")
            + (string.IsNullOrEmpty(Patronymic) ? "" : $" {Patronymic}");

        public string ShortName => LastName
            + (string.IsNullOrEmpty(FirstName) ? "" : $" {FirstName[0]}.")
            + (string.IsNullOrEmpty(Patronymic) ? "" : $" {Patronymic[0]}.");

        public Client? Client { get; set; }
        public List<RequestNotify> RequestNotify { get; set; } = new();
        public List<Request> ExecutedRequests { get; set; } = new();
        public List<Request> ClientRequests { get; set; } = new();
    }
}