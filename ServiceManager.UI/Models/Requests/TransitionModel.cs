using ServiceManager.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceManager.UI.Models.Requests
{
    public class TransitionModel
    {
        [Display(Name = "Номер заявки")]
        public int Number { get; set; }

        [Display(Name = "Текст заявки")]
        public string Text { get; set; } = null!;

        public Guid RequestId { get; set; }
        public RequestAction Action { get; set; }

        [Display(Name = "Комментарий")]
        public string? Note { get; set; }

        [Display(Name = "Очередь")]
        public Guid? ServiceTypeId { get; set; }
        [Display(Name = "Сервис")]
        public Guid? ServiceId { get; set; }

        [Display(Name = "Материалы (необязательно)")]
        public List<RequestMaterialModel> Materials { get; set; } = new();
    }
}
