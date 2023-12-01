using System.ComponentModel.DataAnnotations;

namespace ServiceManager.UI.Models.Requests
{
    public class MaterialCountModel
    {
        [Display(Name = "Название")]
        public string Name { get; set; } = null!;

        [Display(Name = "Кол-во")]
        public int Count { get; set; }
    }
}
