using Framework.ServiceContract.Request;
using Orderbox.Dto.Authentication;
using Orderbox.ServiceContract.Seeds;
using Orderbox.ServiceContract.Authentication;
using Orderbox.ServiceContract.Authentication.Request;
using System.Threading.Tasks;

namespace Orderbox.Service.Seeds
{
    public class UserSeed : IUserSeed
    {
        private readonly IUserService _userService;

        public UserSeed(IUserService userService)
        {
            this._userService = userService;
        }

        public async Task<string> SeedAdministratorUserAsync()
        {
            var userRequest = new UserRequest
            {
                User = new ApplicationUserDto
                {
                    UserName = "wiratanaya@hotmail.com",
                    Email = "wiratanaya@hotmail.com",
                    FirstName = "Ketut",
                    LastName = "Wiratanaya",
                    IsActive = true,
                    TimeZone = "Singapore Standard Time"
                },
                Role = "Administrator",
                Password = "Temp123!"
            };

            return await this.Seed(userRequest);
        }

        public async Task<string> SeedAdministrator2UserAsync()
        {
            var userRequest = new UserRequest
            {
                User = new ApplicationUserDto
                {
                    UserName = "dwiananta1988@gmail.com",
                    Email = "dwiananta1988@gmail.com",
                    FirstName = "Dwi",
                    LastName = "Ananta",
                    IsActive = true,
                    TimeZone = "Singapore Standard Time"
                },
                Role = "Administrator",
                Password = "Temp123!"
            };

            return await this.Seed(userRequest);
        }

        public async Task<string> SeedAdministrator3UserAsync()
        {
            var userRequest = new UserRequest
            {
                User = new ApplicationUserDto
                {
                    UserName = "pranathadoddy@gmail.com",
                    Email = "pranathadoddy@gmail.com",
                    FirstName = "Doddy",
                    LastName = "Pranatha",
                    IsActive = true,
                    TimeZone = "Singapore Standard Time"
                },
                Role = "Administrator",
                Password = "Temp123!"
            };

            return await this.Seed(userRequest);
        }

        public async Task<string> SeedAdministrator4UserAsync()
        {
            var userRequest = new UserRequest
            {
                User = new ApplicationUserDto
                {
                    UserName = "iwayan.hendra.mp@gmail.com",
                    Email = "iwayan.hendra.mp@gmail.com",
                    FirstName = "Wayan",
                    LastName = "Hendra",
                    IsActive = true,
                    TimeZone = "Singapore Standard Time"
                },
                Role = "Administrator",
                Password = "Temp123!"
            };

            return await this.Seed(userRequest);
        }

        private async Task<string> Seed(UserRequest user)
        {
            var readResponse = await this._userService.ReadByUserNameAsync(new GenericRequest<string>
            {
                Data = user.User.UserName
            });

            if (readResponse.Data != null)
            {
                return $"User with username {user.User.UserName} is already exist.";
            }

            var insertResponse = await this._userService.InsertAsync(user);

            if (insertResponse.IsError())
            {
                return $"Failed to insert user with username {user.User.UserName}.";
            }

            return $"User with username {user.User.UserName} has been successfully inserted.";
        }
    }
}
