using Microsoft.AspNetCore.Identity;
using ServiceManager.Core.Exceptions;

namespace ServiceManager.Core.Extensions
{
    public static class IdentityResultExtensions
    {
        public static void ExceptionIfFailed(this IdentityResult result)
        {
            if (!result.Succeeded)
                throw new IdentityException(result);
        }
    }
}
