using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Optimus.Wpf;

public sealed class HealthScoreToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int score)
        {
            if (score >= 80) return new SolidColorBrush(Color.FromRgb(46, 204, 113)); // Verde
            if (score >= 50) return new SolidColorBrush(Color.FromRgb(241, 196, 15));  // Galben
            return new SolidColorBrush(Color.FromRgb(231, 76, 60));                  // Roșu
        }
        return new SolidColorBrush(Colors.Gray);
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
}