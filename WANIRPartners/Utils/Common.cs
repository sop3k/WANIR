using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WANIRPartners.Utils
{
    public class Common
    {
        public static object Missing = System.Type.Missing;
    };

    public class EnumeratorJoin<T>
    {
        static public IEnumerable<T> Join(params IEnumerable<T>[] enums)
        {
            foreach (IEnumerable<T> en in enums)
                foreach (T t in en)
                    yield return t;
        }
    }
    public class ProjectMailingoColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return new SolidColorBrush(Colors.Transparent);
            }

            return System.Convert.ToBoolean(value) ?
                new SolidColorBrush(Colors.Red)
              : new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
