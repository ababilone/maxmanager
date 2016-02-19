using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace MaxManager.ValueConverters
{
	class BooleanToBrushConverter : IValueConverter
	{
		public Brush TrueBrush { get; set; }
		public Brush FalseBrush { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value == null)
				return FalseBrush;

			return System.Convert.ToBoolean(value) ? TrueBrush : FalseBrush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}