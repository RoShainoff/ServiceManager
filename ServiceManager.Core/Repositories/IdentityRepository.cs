using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceManager.Core.Entities.Identity;
using ServiceManager.Core.Models.Identity;
using System.Security.Claims;

namespace ServiceManager.Core.Repositories
{
    public class IdentityRepository : BaseRepository<User>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private IHttpContextAccessor _httpContextAccessor;

        public IdentityRepository(ServicesContext ctx,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor
            ) : base( ctx ) 
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUserId()
        {
            return Guid.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        public async Task<string> GetFullName()
        {
            var username = _httpContextAccessor.HttpContext?.User.Identity?.Name;
            var employee = await FindByAsync(x => x.UserName == username);

            return employee.LastName
                + (string.IsNullOrEmpty(employee.FirstName) ? "" : $" {employee.FirstName}")
                + (string.IsNullOrEmpty(employee.Patronymic) ? "" : $" {employee.Patronymic}");
        }

        public async Task<string> GetShortName()
        {
            var username = _httpContextAccessor.HttpContext?.User.Identity?.Name;
            var employee = await FindByAsync(x => x.UserName == username);

            return employee.LastName
                + (string.IsNullOrEmpty(employee.FirstName) ? "" : $" {employee.FirstName[0]}.")
                + (string.IsNullOrEmpty(employee.Patronymic) ? "" : $" {employee.Patronymic[0]}.");
        }


        public async Task<Guid> GetId(string userName)
        {
            return await _ctx.Users
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .FirstAsync();
        }

        public async Task<ClientModel> GetClient(string userName)
        {
            var id = await GetId(userName);

            return await GetClient(id);
        }
        public async Task<ClientModel> GetClient(Guid id)
        {
            return await _ctx.Client
                .Include(x => x.User)
                .Select(x => new ClientModel
                {
                    Client = new ClientDataModel
                    {
                        Id = x.User.Id,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        Patronymic = x.User.Patronymic,
                        Email = x.User.Email,
                        PhoneNumber = x.User.PhoneNumber,
                        RoomName = x.RoomName,
                    },
                    User = new UserModel
                    {
                        Id = x.User.Id,
                        Login = x.User.UserName!,
                    }
                })
                .FirstAsync(x => x.User.Id == id);
        }

        public async Task<EmployeeModel> GetEmployee(string userName)
        {
            var id = await GetId(userName);

            return await GetEmployee(id);
        }
        public async Task<EmployeeModel> GetEmployee(Guid id)
        {
            return await _ctx.Users
                .Select(x => new EmployeeModel
                {
                    Employee = new EmployeeDataModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Patronymic = x.Patronymic,
                    },
                    User = new UserModel
                    {
                        Id = x.Id,
                        Login = x.UserName!,
                    }
                })
                .FirstAsync(x => x.User.Id == id);
        }

        public async Task<IdentityResult> CreateUser(UserModel model, PersonModel person, string? role = null)
        {
            var user = new User
            {
                UserName = model.Login,
                NormalizedUserName = _userManager.NormalizeName(model.Login),
                FirstName = person.FirstName,
                LastName = person.LastName,
                Patronymic = person.Patronymic,
            };
            var result = await _userManager.CreateAsync(user, model.Password!);
            model.Id = user.Id;
            if (!result.Succeeded || role == null)
                return result;

            result = await _userManager.AddToRoleAsync(user, role);
            return result;
        }

        public async Task<IdentityResult> SaveUserData(UserModel model, bool login = false)
        {
            var entity = (await _ctx.Users
                .FirstAsync(x => x.Id == model.Id));

            entity.UserName = model.Login;
            entity.NormalizedUserName = model.Login.ToUpper();

            if (!string.IsNullOrEmpty(model.Password))
                entity.PasswordHash = new PasswordHasher<User>().HashPassword(entity, model.Password);

            var result = await _userManager.UpdateAsync(entity);
            if (result.Succeeded && login)
                await _signInManager.RefreshSignInAsync(entity);
            return result;
        }

        public async Task SaveEmployeeData(EmployeeDataModel model)
        {
            var entity = await _ctx.Users
                .FirstAsync(x => x.Id == model.Id);

            entity.LastName = model.LastName;
            entity.FirstName = model.FirstName;
            entity.Patronymic = model.Patronymic;

            await _ctx.SaveChangesAsync();
        }

        public async Task SaveClientData(ClientDataModel model)
        {
            var entity = await _ctx.Users
                .Include(x => x.Client)
                .FirstAsync(x => x.Id == model.Id);
            entity.Client ??= new Client();

            entity.LastName = model.LastName;
            entity.FirstName = model.FirstName;
            entity.Patronymic = model.Patronymic;
            entity.Email = model.Email;
            entity.NormalizedEmail = _userManager.NormalizeEmail(model.Email);
            entity.PhoneNumber = model.PhoneNumber;
            entity.Client.RoomName = model.RoomName;

            await _ctx.SaveChangesAsync();
        }
    }
}
