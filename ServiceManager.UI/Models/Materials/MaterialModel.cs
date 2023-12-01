using ServiceManager.UI.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ServiceManager.UI.Models.Materials
{
    public class MaterialModel : BaseModel
    {
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; } = null!;

        [Display(Name = "Количество")]
        [Range(1, int.MaxValue, ErrorMessage = "Кол-во должно быть больше нуля")]
        public int Count { get; set; }
    }
}
