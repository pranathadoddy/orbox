using System.Threading.Tasks;

namespace Orderbox.ServiceContract.Seeds
{
    public interface IRoleSeed
    {
        Task<string> SeedAdministratorRoleAsync();

        Task<string> SeedUserRoleAsync();

        Task<string> SeedAgentRoleAsync();
    }
}
