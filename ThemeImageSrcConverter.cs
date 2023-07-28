using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docutain_SDK_Example_.NET_MAUI
{
    public class ThemeImageSrcConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ImgSrc = (string)value;

            AppTheme oSAppTheme = Application.Current.RequestedTheme;
            if (oSAppTheme == AppTheme.Light)
            {
                return ImgSrc + "black.png";
            }
            else
            {
                return ImgSrc + "white.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return AppTheme.Dark;
        }
    }
}
