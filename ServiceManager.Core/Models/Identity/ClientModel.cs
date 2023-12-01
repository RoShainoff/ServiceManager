using System.ComponentModel.DataAnnotations;

namespace ServiceManager.Core.Models.Identity
{
    public class ClientModel
    {
        public ClientDataModel Client { get; set; } = new ClientDataModel();
        public UserModel User { get; set; } = new UserModel();
    }

    public class ClientDataModel : PersonModel
    {
        //[DataType(DataType.EmailAddress)]
        [Display(Name="Электронная почта")]
        public string? Email { get; set; }

        //[DataType(DataType.PhoneNumber)]
        [Display(Name = "Номер телефона")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Номер кабинета")]
        public string? RoomName { get; set; }
    }
}
