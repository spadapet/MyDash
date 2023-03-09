using MyDash.Controls.Converter;
using MyDash.Data.Model;
using System;
using System.Globalization;

namespace MyDash.Converter
{
    internal sealed class ShellStateToFlyoutVisible : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string route = (string)parameter;

            return (AppState)value switch
            {
                AppState.Loading => route switch
                {
                    "Loading" => true,
                    _ => false
                },
                AppState.Login => route switch
                {
                    "Login" => true,
                    _ => false
                },
                AppState.Dashboard => true,
                _ => throw new InvalidOperationException()
            };
        }
    }

    internal sealed class ShellStateToFlyoutEnabled : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string route = (string)parameter;

            return (AppState)value switch
            {
                AppState.Loading => true,
                AppState.Login => true,
                AppState.Dashboard => route switch
                {
                    "Dashboard" => true,
                    _ => false
                },
                _ => throw new InvalidOperationException()
            };
        }
    }
}
