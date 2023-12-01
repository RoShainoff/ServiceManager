using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceManager.Core.Entities.Identity;
using ServiceManager.Core.Models.Identity;
using ServiceManager.Core.Repositories;
using ServiceManager.UI.Extensions;
using ServiceManager.UI.Models.Account;

namespace ServiceManager.UI.Controllers.Identity
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;

        private readonly IdentityRepository _identityRepository;

        public AccountController(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            IdentityRepository userRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _identityRepository = userRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (User?.Identity?.Name == null) throw new InvalidOperationException();

            if (User.IsEmployeeOrAdmin())
            {
                var user = await _identityRepository.GetEmployee(User.Identity.Name);
                return View("EmployeeProfile", user);
            }
            else
            {
                var user = await _identityRepository.GetClient(User.Identity.Name);
                return View("ClientProfile", user);
            }
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Неправильный логин или пароль");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveUserData([Bind(Prefix = "User")] UserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Thread.Sleep(1000);
            var result = await _identityRepository.SaveUserData(model, login: true);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                    ModelState.AddModelError(string.Empty, item.Description);
                return BadRequest(ModelState);
            }

            return Ok();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEmployeeData([Bind(Prefix = "Employee")] EmployeeDataModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _identityRepository.SaveEmployeeData(model);
            Thread.Sleep(500);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveClientData([Bind(Prefix = "Client")] ClientDataModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _identityRepository.SaveClientData(model);
            Thread.Sleep(500);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GetFullName()
        {
            return Json(await _identityRepository.GetFullName());
        }
    }
}
