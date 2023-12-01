using System.ComponentModel.DataAnnotations;

namespace ServiceManager.Core.Models.Identity
{
    public class PersonModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = null!;

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Отчество")]
        public string? Patronymic { get; set; }
    }
}
