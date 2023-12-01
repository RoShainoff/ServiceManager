namespace ServiceManager.Core.Models.Identity
{
    public class EmployeeModel
    {
        public EmployeeDataModel Employee { get; set; } = new EmployeeDataModel();
        public UserModel User { get; set; } = new UserModel();
    }

    public class EmployeeDataModel : PersonModel
    {

    }
}
