using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transport.Areas.Manage.Infrastructure
{
    public static class CodingDayOfWeek
    {
        public static string DecodeDayOfWeek(DayOfWeek WeekDay)
        {
            string ConvertedString;
            switch (WeekDay)
            {
                case DayOfWeek.Saturday:
                    ConvertedString = "شنبه";
                    break;
                case DayOfWeek.Sunday:
                    ConvertedString = "یکشنبه";
                    break;
                case DayOfWeek.Monday:
                    ConvertedString = "دوشنبه";
                    break;
                case DayOfWeek.Tuesday:
                    ConvertedString = "سه شنبه";
                    break;
                case DayOfWeek.Wednesday:
                    ConvertedString = "چهارشنبه";
                    break;
                case DayOfWeek.Thursday:
                    ConvertedString = "پنج شنبه";
                    break;
                case DayOfWeek.Friday:
                    ConvertedString = "جمعه";
                    break;
                default:
                    ConvertedString = "";
                    break;
            }
            return ConvertedString;
        }

        public static DayOfWeek EncodeDaysOfWeek(string Day)
        {
            DayOfWeek EncodedDay;
            switch (Day)
            {
                case "شنبه":
                    EncodedDay = DayOfWeek.Saturday;
                    break;
                case "یکشنبه":
                    EncodedDay = DayOfWeek.Sunday;
                    break;
                case "دوشنبه":
                    EncodedDay = DayOfWeek.Monday;
                    break;
                case "سه شنبه":
                    EncodedDay = DayOfWeek.Tuesday;
                    break;
                case "چهارشنبه":
                    EncodedDay = DayOfWeek.Wednesday;
                    break;
                case "پنج شنبه":
                    EncodedDay = DayOfWeek.Thursday;
                    break;
                case "جمعه":
                    EncodedDay = DayOfWeek.Friday;
                    break;
                default:
                    EncodedDay = DayOfWeek.Friday;
                    break;
            }
            return EncodedDay;
        }
    }
}