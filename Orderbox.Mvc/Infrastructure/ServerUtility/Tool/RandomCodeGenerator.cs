using System;

namespace Orderbox.Mvc.Infrastructure.ServerUtility.Tool
{
    public class RandomCodeGenerator
    {
        public static string Generate()
        {
            var generator = new Random();
            return generator.Next(0, 999999).ToString("D6");
        }
    }
}
