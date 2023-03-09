﻿using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace MyDash.Controls.Converter;

/// <summary>
/// Base class helper for value converters
/// </summary>
public abstract class ValueConverter : IValueConverter
{
    public virtual object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException();
    }

    public virtual object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException();
    }

    object IValueConverter.Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        return Convert(value, targetType, parameter, culture);
    }

    object IValueConverter.ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        return ConvertBack(value, targetType, parameter, culture);
    }
}
