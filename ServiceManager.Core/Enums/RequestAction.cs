using System.ComponentModel.DataAnnotations;

namespace ServiceManager.Core.Enums
{
    public enum RequestAction
    {        
        [Display(Name = "Создана")]
        Create,

        [Display(Name = "Закрыта с утратой актуальности")]
        Close,

        [Display(Name = "Принята")]
        Accept,

        [Display(Name = "Просрочена")]
        Expired,

        [Display(Name = "Закрыта Успешно")]
        GoodComplete,

        [Display(Name = "Закрыта Не успешно")]
        BadComplete,

        [Display(Name = "Изменён")]
        Edit,
    }
}