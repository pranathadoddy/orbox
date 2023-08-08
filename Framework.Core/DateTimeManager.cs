using JWT;
using System;

namespace Framework.Core
{
    public static class DateTimeManager
    {
        #region (public) Methods

        public static DateTime GetCurrentLocalDate(string timeZoneId)
        {
            return GetCurrentLocalDateTime(timeZoneId).Date;
        }

        public static DateTime GetCurrentLocalDateTime(string timeZoneId)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            var currentDateTimeUtc = DateTime.UtcNow;
            var currentLocalDate = currentDateTimeUtc.Add(timeZoneInfo.BaseUtcOffset);

            return currentLocalDate;
        }

        public static DateTime UtcToLocalDateTime(string timeZoneId, DateTime utcDateTime)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            var currentLocalDate = utcDateTime.Add(timeZoneInfo.BaseUtcOffset);
            return currentLocalDate;
        }

        public static double GetSecondSinceEpoch(int daysFromNow = 0)
        {
            IDateTimeProvider provider = new UtcDateTimeProvider();
            var expiration = provider.GetNow().AddDays(daysFromNow);

            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // or use JwtValidator.UnixEpoch
            var secondsSinceEpoch = Math.Round((expiration - unixEpoch).TotalSeconds);

            return secondsSinceEpoch;
        }

        public static DateTime SecondSinceEpochToDateTime(double secondSinceEpoch)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return unixEpoch.AddSeconds(secondSinceEpoch);
        }

        public static DateTime LocalToUtcDateTime(string timeZoneId, DateTime dateTime)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            var utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZoneInfo);

            return utcDateTime;
        }

        #endregion
    }
}
