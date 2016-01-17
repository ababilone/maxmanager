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
				return Visibility.Collapsed;

			return System.Convert.ToBoolean(value) ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if (value is Visibility && (Visibility) value == Visibility.Visible)
				return true;

			return false;
		}
	}
}
