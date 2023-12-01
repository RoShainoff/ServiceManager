using Microsoft.AspNetCore.Identity;

namespace ServiceManager.Core.Exceptions
{
    public class IdentityException : Exception
    {
        public IEnumerable<IdentityError> Errors { get; private set; }
        public IdentityException(IdentityResult result)
        {
            Errors = result.Errors;
        }
    }
}
