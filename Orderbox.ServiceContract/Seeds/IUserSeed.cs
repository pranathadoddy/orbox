using System.Threading.Tasks;

namespace Orderbox.ServiceContract.Seeds
{
    public interface IUserSeed
    {
        Task<string> SeedAdministratorUserAsync();
        Task<string> SeedAdministrator2UserAsync();
        Task<string> SeedAdministrator3UserAsync();
        Task<string> SeedAdministrator4UserAsync();
    }
}
