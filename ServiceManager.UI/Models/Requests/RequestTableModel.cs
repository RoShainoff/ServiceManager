using ServiceManager.Core.Enums;
using ServiceManager.UI.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ServiceManager.UI.Models.Requests
{
    public class RequestTableModel : BaseTableModel
    {
        [Display(Name = "#")]
        public int Number { get; set; }

        [Display(Name = "Описание")]
        public string Text { get; set; } = null!;

        [Display(Name = "Приоритет")]
        public RequestPriority Priority { get; set; }

        [Display(Name = "Статус")]
        public RequestAction Status { get; set; }


        [Display(Name = "Клиент")]
        public string Client { get; set; } = null!;
        public Guid ClientId{ get; set; }

        [Display(Name = "Агент")]
        public string? Executer { get; set; }
        public Guid? ExecuterId { get; set; }


        [Display(Name = "Очередь")]
        public string? ServiceType { get; set; }

        [Display(Name = "Сервис")]
        public string? Service { get; set; }


        [Display(Name = "Создан")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Изменён")]
        public DateTime LastEditDate { get; set; }

        public override bool CanDelete => false;

        public bool IsClosed =>
            Status == RequestAction.Close ||
            Status == RequestAction.GoodComplete ||
            Status == RequestAction.BadComplete;
    }
}
