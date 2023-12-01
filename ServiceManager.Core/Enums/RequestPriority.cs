using System.ComponentModel.DataAnnotations;

namespace ServiceManager.Core.Enums
{
    public enum RequestPriority
    {
        [Display(Name = "Низкий")]
        Low     = -1,

        [Display(Name = "Средний")]
        Medium  = 0,

        [Display(Name = "Высокий")]
        High    = 1
    }
}