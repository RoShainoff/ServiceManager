using ServiceManager.UI.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ServiceManager.UI.Models.Identity
{
    public class ClientTableModel : BaseTableModel
    {
        [Display(Name = "ФИО")]
        public string FullName { get; set; } = null!;

        [Display(Name = "Эл. Почта")]
        public string? Email { get; set; }

        [Display(Name = "Кабинет")]
        public string? RoomName { get; set; }

        [Display(Name = "Тел. номер")]
        public string? PhoneNumber { get; set; }


        [Display(Name = "Кол-во всех заявок")]
        public int RequestsAllCount { get; set; }

        [Display(Name = "Кол-во активных заявок")]
        public int RequestsWorkCount { get; set; }

        [Display(Name = "Кол-во завершённых заявок")]
        public int RequestsComplitedCount { get; set; }

        public override bool CanDelete =>
            RequestsAllCount == 0 &&
            RequestsWorkCount == 0 &&
            RequestsComplitedCount == 0;
    }
}
