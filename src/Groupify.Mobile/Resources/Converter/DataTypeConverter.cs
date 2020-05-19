using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Groupify.Mobile.Resources.Converter
{
    public class DataTypeConverter : IValueConverter, IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter,
          CultureInfo culture)
        {
            return value?.GetType() ?? default(object);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
          CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
