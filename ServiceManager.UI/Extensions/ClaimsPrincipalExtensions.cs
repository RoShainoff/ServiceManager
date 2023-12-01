using System.Security.Claims;

namespace ServiceManager.UI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsInRoles(this ClaimsPrincipal User, string roles)
        {
            return roles.Split(",").Any(x => User.IsInRole(x.Trim()));
        }

        public static bool IsAdmin(this ClaimsPrincipal User)
        {
            return User.IsInRole("Admin");
        }

        public static bool IsEmployee(this ClaimsPrincipal User)
        {
            return User.IsInRole("Employee");
        }

        public static bool IsEmployeeOrAdmin(this ClaimsPrincipal User)
        {
            return User.IsInRoles("Admin, Employee");
        }

        public static bool IsClient(this ClaimsPrincipal User)
        {
            return !User.IsEmployeeOrAdmin();
        }
    }
}
