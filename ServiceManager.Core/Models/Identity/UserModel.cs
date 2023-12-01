using System.ComponentModel.DataAnnotations;

namespace ServiceManager.Core.Models.Identity
{
    public class UserModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; } = null!;

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string? PasswordConfirm { get; set; }
    }
}
