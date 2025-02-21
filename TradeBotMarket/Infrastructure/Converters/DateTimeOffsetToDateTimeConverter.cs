using System.Globalization;
using System.Windows.Data;

namespace TradeBotMarket.Infrastructure.Converters
{
    public class DateTimeOffsetToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dateTimeOffset)
                return dateTimeOffset.DateTime; 
            return DateTime.MinValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
                return new DateTimeOffset(dateTime); 
            return DateTimeOffset.MinValue;
        }
    }
}
