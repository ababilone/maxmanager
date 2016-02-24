using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MaxManager.ValueConverters
{
	class BooleanToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value == null)
				return parameter == null ? Visibility.Collapsed : Visibility.Visible;

			if (System.Convert.ToBoolean(value))
				return parameter == null ? Visibility.Visible : Visibility.Collapsed;

			return parameter == null ? Visibility.Collapsed : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if (value is Visibility && (Visibility)value == Visibility.Visible)
				return true;

			return false;
		}
	}
}