using ServiceManager.UI.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ServiceManager.UI.Models.Services
{
    public class ServiceModel : BaseModel
    {
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; } = null!;

        [Range(0.01, int.MaxValue, ErrorMessage = "Цена быть больше нуля")]
        [Display(Name = "Цена")]
        public decimal Cost { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Время на выполнения должно быть больше нуля")]
        [Display(Name = "Время на выполнение")]
        public int Hours { get; set; }

        [Display(Name = "Очередь")]
        public Guid? ServiceTypeId { get; set; }
    }
}
