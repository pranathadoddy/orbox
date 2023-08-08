using Orderbox.Dto.Authentication;

namespace Orderbox.ServiceContract.Authentication.Request
{
    public class ChangePasswordRequest
    {
        public ApplicationUserDto User { get; set; }

        public string NewPassword { get; set; }

        public string OldPassword { get; set; }

    }
}
