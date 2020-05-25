using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Groupify.Mobile.Resources.Converter
{
    public class IntegerIsEqualsConverter : IMarkupExtension, IValueConverter
    {
        public int OtherInteger { get; set; }

        public bool Inverted { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            if (!(value is int integerValue))
                return false;

            return Inverted ? integerValue != OtherInteger : (object)(integerValue == OtherInteger);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
        public object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
