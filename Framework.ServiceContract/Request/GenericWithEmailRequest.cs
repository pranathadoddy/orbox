namespace Framework.ServiceContract.Request
{
    public class GenericWithEmailRequest<T> : GenericRequest<T>
    {
        public string EmailTemplateFilePath { get; set; }

        public string Url { get; set; }
    }
}
