using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extension
{
    /// <summary>对象函数扩展</summary>
    public static class ObjectExtension
    {
        /// <summary>获取当前字符串MD5Hash后的字符值（加密编码格式默认UTF-8）</summary>
        /// <param name="str">当前字符对象</param>
        /// <param name="encoding">编码格式,default utf-8</param>
        /// <returns></returns>
        public static string MD5Hash(this string str, string encoding = "UTF-8")
        {
            return Utils.MD5Hash(str, encoding);
        }

        /// <summary>获取当前字符串SHA1Hash后的字符值（加密编码格式默认UTF-8）</summary>
        /// <param name="str">当前字符对象</param>
        /// <param name="encoding">编码格式,default utf-8</param>
        /// <returns></returns>
        public static string SHA1Hash(this string str, string encoding = "UTF-8")
        {
            return Utils.SHA1Hash(str, encoding);
        }

        /// <summary>将指定字符串中的格式项替换为指定数组中相应对象的字符串表示形式。</summary>
        /// <param name="value">复合格式字符串。</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        /// <returns>format 的副本，其中的格式项已替换为 args 中相应对象的字符串表示形式。</returns>
        public static string StrFormat(this string value, params object[] args)
        {
            return String.Format(value, args);
        }

        /// <summary>确定此字符串实例中是否包含指定的字符串匹配。</summary>
        /// <param name="value">要测试的字符串。</param>
        /// <param name="with">要比较的字符串。</param>
        /// <returns>如果 with 参数在要测试的字符串出现，则为true。</returns>
        public static bool InWith(this string value, string with)
        {
            return value.IndexOf(with, StringComparison.CurrentCultureIgnoreCase) > -1;
        }

        /// <summary>指示指定的字符串是 null、空还是仅由空白字符组成。</summary>
        /// <param name="value">要测试的字符串。</param>
        /// <returns>如果 value 参数为 null 或 System.String.Empty，或者如果 value 仅由空白字符组成，则为 true。</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return String.IsNullOrWhiteSpace(value);
        }

        /// <summary>获取当前string类型转换成long类型的值</summary>
        /// <param name="value">当前对象值</param>
        /// <returns></returns>
        public static long ToLong(this string value)
        {
            if (string.IsNullOrEmpty(value)) { return 0; }
            return Convert.ToInt64(value);
        }

        /// <summary>获取当前string类型转换成int类型的值</summary>
        /// <param name="value">当前对象值</param>
        /// <returns></returns>
        public static int ToInt(this string value)
        {
            if (string.IsNullOrEmpty(value)) { return 0; }
            return Convert.ToInt32(value);
        }

        /// <summary>获取当前对象值转换成double类型的值</summary>
        /// <param name="value">当前对象值</param>
        /// <returns></returns>
        public static double ToDouble(this object value)
        {
            return ((value == null || value == DBNull.Value) ? 0 : Convert.ToDouble(value.ToString()));
        }

        /// <summary>获取当前对象值转换成decimal类型的值</summary>
        /// <param name="value">当前对象值</param>
        /// <returns></returns>
        public static decimal ToDecimal(this object value)
        {
            return Convert.ToDecimal(ToDouble(value));
        }

        /// <summary>转换成时间类型</summary>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value)
        {
            if (string.IsNullOrEmpty(value)) { return DateTime.Now; }
            return Convert.ToDateTime(value);
        }

        /// <summary>获取当前时间戳</summary>
        /// <returns></returns>
        public static long ToTimeStamp(this DateTime date)
        {
            return Convert.ToInt64((date - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalMilliseconds);
        }

        /// <summary>转换时间戳为日期</summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long timestamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).Add(new TimeSpan(timestamp * 10000));
        }

        /// <summary>对象是否为null</summary>
        /// <param name="obj">当前对象</param>
        /// <returns>对象为空true，否则false</returns>
        public static bool IsNull(this object obj)
        {
            return null == obj;
        }
    }
}
