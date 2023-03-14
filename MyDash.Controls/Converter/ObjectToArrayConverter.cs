using System;
using System.Globalization;

namespace MyDash.Controls.Converter;

public class ObjectToArrayConverter : ValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value != null) ? new object[] { value } : null;
    }
}