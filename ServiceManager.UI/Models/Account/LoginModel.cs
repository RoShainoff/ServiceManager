using System.ComponentModel.DataAnnotations;

namespace ServiceManager.UI.Models.Account
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = null!;

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
