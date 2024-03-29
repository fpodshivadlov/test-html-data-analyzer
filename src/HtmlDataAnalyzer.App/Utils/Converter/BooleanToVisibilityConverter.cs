﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HtmlDataAnalyzer.App.Utils.Converter
{
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public Visibility True { get; set; }
        public Visibility False { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is true ? True : False;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility valueVal && valueVal == True;
        }
    }
}
