using System;
using System.Data.Common;

namespace Framework.Core.Extensions
{
    public static class DbDataReaderExtension
    {
        public static string SafeGetString(this DbDataReader reader, string name)
        {
            var ordinal = reader.GetOrdinal(name);
            if (!reader.IsDBNull(ordinal))
                return reader.GetString(ordinal);
            return string.Empty;
        }

        public static DateTime SafeGetDateTime(this DbDataReader reader, string name)
        {
            var ordinal = reader.GetOrdinal(name);
            if (!reader.IsDBNull(ordinal))
                return reader.GetDateTime(ordinal);
            return DateTime.MinValue;
        }
    }
}
