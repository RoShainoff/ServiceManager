using System.ComponentModel.DataAnnotations;

namespace ServiceManager.UI.Models.Base
{
    public class NamedModel : BaseModel
    {
        [Display(Name = "Название")]
        public string Name { get; set; } = null!;
    }
}
