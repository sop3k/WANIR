using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace WANIRPartners.Utils
{
   public class BoolToComboBoxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (((bool)value))
            {
                case true:
                    return Const.YES_CAPTION;
                case false:
                    return Const.NO_CAPTION;
                default:
                    return Const.NOT_SET;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (((string)value).ToLower())
            {
                case "tak":
                    return true;
                case "nie":
                    return false;
                default:
                    return null;
            }
        }
    }

    public class RowHeaderAddition : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, 
                System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, 
                System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
