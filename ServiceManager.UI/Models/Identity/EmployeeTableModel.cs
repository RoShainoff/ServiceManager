using ServiceManager.UI.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ServiceManager.UI.Models.Identity
{
    public class EmployeeTableModel : BaseTableModel
    {
        [Display(Name = "ФИО")]
        public string FullName { get; set; } = null!;

        [Display(Name = "Кол-во активных заявок")]
        public int RequestsWorkCount { get; set; }

        [Display(Name = "Кол-во завершённых заявок")]
        public int RequestsComplitedCount { get; set; }

        public override bool CanDelete =>
            RequestsWorkCount == 0 &&
            RequestsComplitedCount == 0;
    }
}
