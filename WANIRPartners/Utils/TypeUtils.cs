using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Common;
using System.Net;
using System.IO;
using System.Diagnostics;

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
    }
}
