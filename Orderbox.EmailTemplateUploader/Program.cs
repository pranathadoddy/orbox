using Framework.ServiceContract;
using Framework.ServiceContract.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orderbox.Service.Email;
using Orderbox.ServiceContract.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Orderbox.EmailTemplateUploader
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IAgentInvitationEmailManager, AgentInvitationEmailManager>()
                .AddSingleton<IResetPasswordEmailManager, ResetPasswordEmailManager>()
                .AddSingleton<IVerificationEmailManager, VerificationEmailManager>()
                .AddSingleton(configuration)
                .BuildServiceProvider();

            Console.WriteLine("Pilihan :");
            Console.WriteLine("1. Upload template.");
            Console.WriteLine("2. Delete template.");
            var input = Console.ReadLine();
            var integerInput = int.Parse(input);

            switch (integerInput)
            {
                case 1:
                    UploadTemplate(serviceProvider, configuration);
                    break;
                case 2:
                    Console.WriteLine("Masukkan nama template : ");
                    var templateName = Console.ReadLine();
                    DeleteTemplate(serviceProvider, templateName);
                    break;
            }
        }

        static void UploadTemplate(
            IServiceProvider serviceProvider,
            IConfiguration configuration
        )
        {
            IAgentInvitationEmailManager agentInvitationEmailManager = serviceProvider.GetService<IAgentInvitationEmailManager>();
            IResetPasswordEmailManager resetPasswordEmailManager = serviceProvider.GetService<IResetPasswordEmailManager>();
            IVerificationEmailManager verificationEmailManager = serviceProvider.GetService<IVerificationEmailManager>();

            string GetEmailTemplateFilePath()
            {
                var rootDirectory = Directory.GetCurrentDirectory();
                var emailTemplateDirectory = configuration.GetValue<string>("EmailConfiguration:EmailTemplatesFolder");
                var fullemailTemplateDirectory = Path.Combine(rootDirectory, emailTemplateDirectory);
                return fullemailTemplateDirectory;
            }

            var emailTemplateFilePath = GetEmailTemplateFilePath();
            BasicResponse response;

            agentInvitationEmailManager.TemplateFilePath = emailTemplateFilePath;
            response = agentInvitationEmailManager.UploadTemplateAsync().GetAwaiter().GetResult();
            DisplayMessage(response);

            resetPasswordEmailManager.TemplateFilePath = emailTemplateFilePath;
            response = resetPasswordEmailManager.UploadTemplateAsync().GetAwaiter().GetResult();
            DisplayMessage(response);

            verificationEmailManager.TemplateFilePath = emailTemplateFilePath;
            response = verificationEmailManager.UploadTemplateAsync().GetAwaiter().GetResult();
            DisplayMessage(response);

            Console.ReadKey();
        }

        static void DeleteTemplate(
            IServiceProvider serviceProvider,
            string templateName
        )
        {
            IResetPasswordEmailManager resetPasswordEmailManager = serviceProvider.GetService<IResetPasswordEmailManager>();
            IVerificationEmailManager verificationEmailManager = serviceProvider.GetService<IVerificationEmailManager>();

            var templates = new Dictionary<string, IAwsSesEmailManagerBase>
            {
                { "Orderbox_ResetPassword", resetPasswordEmailManager },
                { "Orderbox_VerificationEmail", verificationEmailManager }
            };

            try
            {
                templates[templateName].DeleteTemplateAsync().GetAwaiter().GetResult();
                Console.WriteLine($"Success delete {templateName}");
            }
            catch
            {
                Console.Error.WriteLine($"{templateName} not found");
            }
        }

        static void DisplayMessage(BasicResponse response)
        {
            if (response.IsError())
            {
                Console.Error.WriteLine(response.GetErrorMessage());
            }
            else
            {
                Console.WriteLine(response.GetMessageInfoTextArray().First());
            }
        }
    }
}
