using ServiceManager.Core.Enums;
using ServiceManager.UI.Models.Base;

namespace ServiceManager.UI.Models.Requests
{
    public class NotifyModel
    {
        public Guid Id { get; set; }
        public bool IsRead { get; set; }
        public string? Text { get; set; }
        public DateTime Date { get; set; }
        public NamedModel Request { get; set; } = null!;
        public string RequestText { get; set; } = null!;
        public NamedModel? Executer { get; set; }
        public RequestAction Action { get; set; }
    }
}
