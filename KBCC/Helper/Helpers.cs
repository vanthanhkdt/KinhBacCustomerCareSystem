using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.Mvc;
using KBCC.Models;

namespace KBCC.Helper
{
    public static class Helpers
    {
        /// <summary>
        /// Convert string to date. Result Datetime.MinValue of exception happen
        /// </summary>
        /// <param name="src">string of date</param>
        /// <param name="format">Date format</param>
        /// <returns></returns>
        public static DateTime StringToDate(string src, string format)
        {
            DateTime result = DateTime.MinValue;
            DateTime.TryParseExact(src, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out result);
            return result;
        }

        public static int GetWeekOfYear(DateTime time, CalendarWeekRule weekRule = CalendarWeekRule.FirstDay, DayOfWeek firstDayInWeek = DayOfWeek.Monday)
        {
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;

            int week = myCal.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            return week;
        }

        public static string GetImageSource(byte[] source)
        {
            string mintype = "image/*";
            string base64 = Convert.ToBase64String(source);
            return string.Format("data:{0};base64,{1}", mintype, base64);
        }

        public static bool IsInRole(this ApplicationUser user, string role)
        {
            using (var db = new ApplicationDbContext())
            {
                return true;
            }
        }

        public static string GetRealReturnType(string action)
        {
            try
            {
                if (action.Contains("[["))
                {
                    int index = action.IndexOf("[[");
                    action = action.Remove(0, index + 2);
                    return action.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0].Replace("System.Web.Mvc.", "");
                }
                return action;
            }
            catch (Exception)
            {
                return action;
            }
        }
    }

    public class AllowHtmlBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var name = bindingContext.ModelName;
            return request.Unvalidated[name]; //magic happens here
        }
    }

}