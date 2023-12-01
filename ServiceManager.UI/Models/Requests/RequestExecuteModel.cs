using ServiceManager.Core.Enums;

namespace ServiceManager.UI.Models.Requests
{
    public class RequestExecuteModel
    {
        public Guid RequestId { get; set; }
        public RequestAction Action { get; set; }
        public Guid UserId { get; set; }
        public string? Note { get; set; }
    }
}
