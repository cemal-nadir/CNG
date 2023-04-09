#nullable enable
using System.Runtime.CompilerServices;

namespace CNG.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime(this string source, bool throwExceptionIfFailed = false)
        {
            if (DateTime.TryParse(source, out var result) || !throwExceptionIfFailed)
                return result;
            throw new Exception("Cannot be converted dateTime");
        }

        public static DateTime ToDateTime(this int source) => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddMilliseconds(source).ToLocalTime();

        public static DateTime ToDateTimeUtc(this int source) => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(source).ToLocalTime();

        public static DateTime ToDateTime(this long source) => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddMilliseconds(source).ToLocalTime();

        public static DateTime ToDateTimeUtc(this long source) => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(source).ToLocalTime();

        public static TimeSpan ToTimeSpan(this string source)
        {
            return !TimeSpan.TryParse(source.Trim(), out var result) ? TimeSpan.Zero : result;
        }

        public static bool Between(this DateTime date, DateTime first, DateTime last) => date.Ticks >= first.Ticks && date.Ticks <= last.Ticks;

        public static int CalculateAge(this DateTime dateTime)
        {
            var age = DateTime.Now.Year - dateTime.Year;
            if (DateTime.Now < dateTime.AddYears(age))
                --age;
            return age;
        }

        public static string ToReadable(this TimeSpan source)
        {
            var readable = "";
            DefaultInterpolatedStringHandler interpolatedStringHandler;
            if (source.Days > 0)
            {
                var str = readable;
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 1);
                interpolatedStringHandler.AppendFormatted(source.Days);
                interpolatedStringHandler.AppendLiteral(" gün ");
                var stringAndClear = interpolatedStringHandler.ToStringAndClear();
                readable = str + stringAndClear;
            }
            if (source.Hours > 0)
            {
                var str = readable;
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
                interpolatedStringHandler.AppendFormatted(source.Hours);
                interpolatedStringHandler.AppendLiteral(" saat ");
                var stringAndClear = interpolatedStringHandler.ToStringAndClear();
                readable = str + stringAndClear;
            }
            if (source.Minutes > 0)
            {
                var str = readable;
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
                interpolatedStringHandler.AppendFormatted(source.Minutes);
                interpolatedStringHandler.AppendLiteral(" dakika ");
                var stringAndClear = interpolatedStringHandler.ToStringAndClear();
                readable = str + stringAndClear;
            }
            if (source.Seconds > 0)
            {
                var str = readable;
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
                interpolatedStringHandler.AppendFormatted(source.Seconds);
                interpolatedStringHandler.AppendLiteral(" saniye ");
                var stringAndClear = interpolatedStringHandler.ToStringAndClear();
                readable = str + stringAndClear;
            }

            if (source.Milliseconds <= 0) return readable;
            {
                var str = readable;
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
                interpolatedStringHandler.AppendFormatted(source.Milliseconds);
                interpolatedStringHandler.AppendLiteral(" Milisaniye ");
                var stringAndClear = interpolatedStringHandler.ToStringAndClear();
                readable = str + stringAndClear;
            }
            return readable;
        }

        public static string ToReadable(this DateTime startedDate)
        {
            var timeSpan = DateTime.Now - startedDate;
            var readable = "";
            DefaultInterpolatedStringHandler interpolatedStringHandler;
            if (timeSpan.Days > 0)
            {
                var str = readable;
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 1);
                interpolatedStringHandler.AppendFormatted(timeSpan.Days);
                interpolatedStringHandler.AppendLiteral(" gün ");
                var stringAndClear = interpolatedStringHandler.ToStringAndClear();
                readable = str + stringAndClear;
            }
            if (timeSpan.Hours > 0)
            {
                var str = readable;
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
                interpolatedStringHandler.AppendFormatted(timeSpan.Hours);
                interpolatedStringHandler.AppendLiteral(" saat ");
                var stringAndClear = interpolatedStringHandler.ToStringAndClear();
                readable = str + stringAndClear;
            }
            if (timeSpan.Minutes > 0)
            {
                var str = readable;
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
                interpolatedStringHandler.AppendFormatted(timeSpan.Minutes);
                interpolatedStringHandler.AppendLiteral(" dakika ");
                var stringAndClear = interpolatedStringHandler.ToStringAndClear();
                readable = str + stringAndClear;
            }
            if (timeSpan.Seconds > 0)
            {
                var str = readable;
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
                interpolatedStringHandler.AppendFormatted(timeSpan.Seconds);
                interpolatedStringHandler.AppendLiteral(" saniye ");
                var stringAndClear = interpolatedStringHandler.ToStringAndClear();
                readable = str + stringAndClear;
            }
            if (timeSpan.Milliseconds > 0)
            {
                var str = readable;
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
                interpolatedStringHandler.AppendFormatted(timeSpan.Milliseconds);
                interpolatedStringHandler.AppendLiteral(" ms ");
                var stringAndClear = interpolatedStringHandler.ToStringAndClear();
                readable = str + stringAndClear;
            }
            return readable;
        }

        public static string ToReadableTime(this DateTime source, DateTime? endDate = null)
        {
            var dateTime = endDate ?? DateTime.Now;
            var timeSpan = new TimeSpan(dateTime.Ticks - source.Ticks);
            var totalSeconds = timeSpan.TotalSeconds;
            var str = source.Ticks < dateTime.Ticks ? "önce" : "sonra";
            if (totalSeconds < 0.0)
            {
                timeSpan = new TimeSpan(source.Ticks - dateTime.Ticks);
                totalSeconds = timeSpan.TotalSeconds;
            }
            var num = totalSeconds;
            if (num >= 60.0)
            {
                if (num < 120.0)
                    return "bir dakika " + str;
                if (num >= 2700.0)
                {
                    if (num < 5400.0)
                        return "bir saat " + str;
                    if (num >= 86400.0)
                    {
                        if (num < 172800.0)
                            return "dün";
                        if (num >= 2592000.0)
                        {
                            if (num < 31104000.0)
                            {
                                var int32 = Convert.ToInt32(Math.Floor(timeSpan.Days / 30.0));
                                string readableTime;
                                if (int32 > 1)
                                {
                                    var interpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 2);
                                    interpolatedStringHandler.AppendFormatted(int32);
                                    interpolatedStringHandler.AppendLiteral(" hafta ");
                                    interpolatedStringHandler.AppendFormatted(str);
                                    readableTime = interpolatedStringHandler.ToStringAndClear();
                                }
                                else
                                    readableTime = "bir hafta " + str;
                                return readableTime;
                            }
                            var int321 = Convert.ToInt32(Math.Floor(timeSpan.Days / 365.0));
                            string readableTime1;
                            if (int321 > 1)
                            {
                                var interpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 2);
                                interpolatedStringHandler.AppendFormatted(int321);
                                interpolatedStringHandler.AppendLiteral(" yıl ");
                                interpolatedStringHandler.AppendFormatted(str);
                                readableTime1 = interpolatedStringHandler.ToStringAndClear();
                            }
                            else
                                readableTime1 = "bir yıl " + str;
                            return readableTime1;
                        }
                        var interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(5, 2);
                        interpolatedStringHandler1.AppendFormatted(timeSpan.Days);
                        interpolatedStringHandler1.AppendLiteral(" gün ");
                        interpolatedStringHandler1.AppendFormatted(str);
                        return interpolatedStringHandler1.ToStringAndClear();
                    }
                    var interpolatedStringHandler2 = new DefaultInterpolatedStringHandler(6, 2);
                    interpolatedStringHandler2.AppendFormatted(timeSpan.Hours);
                    interpolatedStringHandler2.AppendLiteral(" saat ");
                    interpolatedStringHandler2.AppendFormatted(str);
                    return interpolatedStringHandler2.ToStringAndClear();
                }
                var interpolatedStringHandler3 = new DefaultInterpolatedStringHandler(8, 2);
                interpolatedStringHandler3.AppendFormatted(timeSpan.Minutes);
                interpolatedStringHandler3.AppendLiteral(" dakika ");
                interpolatedStringHandler3.AppendFormatted(str);
                return interpolatedStringHandler3.ToStringAndClear();
            }
            string readableTime2;
            if (timeSpan.Seconds != 1)
            {
                var interpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 2);
                interpolatedStringHandler.AppendFormatted(timeSpan.Seconds);
                interpolatedStringHandler.AppendLiteral(" saniye ");
                interpolatedStringHandler.AppendFormatted(str);
                readableTime2 = interpolatedStringHandler.ToStringAndClear();
            }
            else
                readableTime2 = "bir saniye " + str;
            return readableTime2;
        }

        public static string ToReadableTime(this DateTime? value) => !value.HasValue ? "" : value.Value.ToReadableTime();

        public static int ToDay(this DateTime value) => (int)new TimeSpan(DateTime.Now.Ticks - value.Ticks).TotalSeconds;

        public static bool WorkingDay(this DateTime date) => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != 0;

        public static bool IsWeekend(this DateTime date) => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

        public static DateTime NextWorkday(this DateTime date)
        {
            var date1 = date.AddDays(1.0);
            while (!date1.WorkingDay())
                date1 = date1.AddDays(1.0);
            return date1;
        }

        public static int ToTimeStamp(this DateTime date)
        {
            var dateTime = new DateTime(1970, 1, 1);
            return (int)date.Subtract(dateTime).TotalSeconds;
        }
    }
}
