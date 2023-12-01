using ServiceManager.Core.Enums;
using ServiceManager.UI.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ServiceManager.UI.Models.Requests
{
    public class RequestModel : BaseModel
    {
        [Display(Name = "Номер")]
        public int? Number { get; set; }

        [Display(Name = "Описание")]
        public string Text { get; set; } = null!;

        [Display(Name = "Приоритет")]
        public RequestPriority Priority { get; set; } = RequestPriority.Medium;


        [Display(Name = "Клиент")]
        public Guid ClientId { get; set; }

        [Display(Name = "Агент")]
        public Guid? ExecutorId { get; set; }

        [Display(Name = "Очередь")]
        public Guid? ServiceTypeId { get; set; }

        [Display(Name = "Сервис")]
        public Guid? ServiceId { get; set; }

        [Display(Name = "Материалы")]
        public List<MaterialCountModel> Materials { get; set; } = new();

        public RequestAction Status { get; set; }
    }
}
