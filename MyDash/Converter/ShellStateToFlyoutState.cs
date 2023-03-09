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

            return (ShellState)value switch
            {
                ShellState.Loading => route switch
                {
                    "Loading" => true,
                    _ => false
                },
                ShellState.Login => route switch
                {
                    "Login" => true,
                    _ => false
                },
                ShellState.Dashboard => true,
                _ => throw new InvalidOperationException()
            };
        }
    }

    internal sealed class ShellStateToFlyoutEnabled : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string route = (string)parameter;

            return (ShellState)value switch
            {
                ShellState.Loading => true,
                ShellState.Login => true,
                ShellState.Dashboard => route switch
                {
                    "Dashboard" => true,
                    _ => false
                },
                _ => throw new InvalidOperationException()
            };
        }
    }
}
