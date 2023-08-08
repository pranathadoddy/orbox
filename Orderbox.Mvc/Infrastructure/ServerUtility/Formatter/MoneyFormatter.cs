using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Infrastructure.ServerUtility.Formatter
{
    public static class MoneyFormatter
    {
        public static string GetBigNumber(Decimal money)
        {
            if (money >= 1000000000)
            {
                return ((int)Math.Floor(money / 1000000000)).ToString();
            }
            else if (money >= 1000000)
            {
                return ((int)Math.Floor(money / 1000000)).ToString();
            }
            else if (money >= 1000)
            {
                return ((int)Math.Floor(money / 1000)).ToString();
            }
            else
            {
                return ((int)money).ToString();
            }
        }

        public static string GetSmallNumber(Decimal money)
        {
            if (money >= 1000000000)
            {
                return (money % 1000000000).ToString("000,000,000.00");
            }
            else if (money >= 1000000)
            {
                return (money % 1000000).ToString("000,000.00");
            }
            else if (money >= 1000)
            {
                return (money % 1000).ToString("000.00");
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
