using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WANIRPartners.Utils
{
    static class ArrayUtils
    {
        public static T[] Empty<T>()
        {
            return new T[0]; // Won't crash in foreach
        }

        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            // Handles negative ends.
            if (end < 0)
            {
                end = source.Length + end + 1;
            }
            int len = end - start;

            if (len <= 0)
                return ArrayUtils.Empty<T>();

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }

        public static T[] Merge<T>(this T[] originalArray, T[] newArray)
        {
            int startIndexForNewArray = originalArray.Length;
            Array.Resize<T>(ref originalArray, originalArray.Length + newArray.Length);
            if (newArray.Length >= startIndexForNewArray)
                newArray.CopyTo(originalArray, startIndexForNewArray);
            return originalArray;
        }
    }
}
