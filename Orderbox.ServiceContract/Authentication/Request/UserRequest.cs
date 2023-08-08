using Orderbox.Dto.Authentication;

namespace Orderbox.ServiceContract.Authentication.Request
{
    public class UserRequest
    {
        public ApplicationUserDto User { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
