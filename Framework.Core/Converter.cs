using Newtonsoft.Json;

namespace Framework.Core
{
    public class Converter
    {
        public static T ToType<T>(dynamic obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var result = JsonConvert.DeserializeObject<T>(json);

            return result;
        }
    }
}
