using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.ServiceContract
{
    public interface IEmailManagerBase
    {
        string TemplateFilePath { get; set; }

        ICollection<string> Recipients { get; }

        ICollection<Dictionary<string, string>> Subtitutes { get; }

        Task SendAsync();

        void Clear();
    }
}
