using Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Extension
{
    /// <summary></summary>
    public static class EnumExtensions
    {
        /// <summary></summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string GetEnumKey(this Enum o)
        {
            string resultStr = "";
            if (o == null) return resultStr;
            Type obj = o.GetType();
            EnumerationAttribute[] desc = (EnumerationAttribute[])obj.GetField(o.ToString()).GetCustomAttributes(typeof(EnumerationAttribute), false);

            if (desc != null && desc.Length == 1) { resultStr = desc[0].Key; }
            return resultStr;
        }

        /// <summary></summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum o)
        {
            string resultStr = "";
            if (o == null) return resultStr;
            Type obj = o.GetType();
            EnumerationAttribute[] desc = (EnumerationAttribute[])obj.GetField(o.ToString()).GetCustomAttributes(typeof(EnumerationAttribute), false);

            if (desc != null && desc.Length == 1) { resultStr = desc[0].Description; }
            return resultStr;
        }

        /// <summary></summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string GetEnumAction(this Enum o)
        {
            string resultStr = "";
            if (o == null) return resultStr;
            Type obj = o.GetType();
            EnumerationAttribute[] desc = (EnumerationAttribute[])obj.GetField(o.ToString()).GetCustomAttributes(typeof(EnumerationAttribute), false);

            if (desc != null && desc.Length == 1) { resultStr = desc[0].Action; }
            return resultStr;
        }
    }
}
