using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Common;
using System.Net;
using System.IO;
using System.Diagnostics;
using Excel;

namespace WANIRPartners.Utils
{
    class TypeUtils
    {
        public static T GetValueFromAnonymousType<T>(object dataitem, string itemkey)
        {
            System.Type type = dataitem.GetType();
            PropertyInfo info = type.GetProperty(itemkey);
            if (info != null)
            {
                T itemvalue = (T)info.GetValue(dataitem, null);
                return itemvalue;
            }
            return default(T);
        }

        public static int GetIntFromReader(IExcelDataReader reader, int index)
        {
            string tmp = reader.GetString(index);
            int vout = 0;
            int.TryParse(tmp, out vout);
            return vout;
        }

        public static string GetStringFromReader(IExcelDataReader reader, int index)
        {
            string tmp = reader.GetString(index);
            if (string.IsNullOrEmpty(tmp))
                return "";
            return tmp.Trim();
        }
    }
}
