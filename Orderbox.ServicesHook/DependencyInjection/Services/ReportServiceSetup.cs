using Microsoft.Extensions.DependencyInjection;
using Orderbox.Service.Report;
using Orderbox.ServiceContract.Report;

namespace Orderbox.ServicesHook.DependencyInjection.Services
{
    public class ReportServiceSetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddScoped<IReportService, ReportService>();
        }
    }
}
