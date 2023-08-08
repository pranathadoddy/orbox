using Microsoft.AspNetCore.Identity;
using Orderbox.Core.Resources.Account;

namespace Orderbox.Core.Extensions
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = string.Format(IdentityErrorMessageResource.PasswordTooShort, length)
            };
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = string.Format(IdentityErrorMessageResource.PasswordRequiresUniqueChars, uniqueChars)
            };
        }
    }
}
