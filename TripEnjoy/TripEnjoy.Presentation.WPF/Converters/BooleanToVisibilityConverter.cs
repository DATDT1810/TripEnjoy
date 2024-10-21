using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TripEnjoy.Presentation.WPF.Converters
{
	public class BooleanToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// value: giá trị đầu vào
			// thuộc tính đích
			// parameter: tham số 
			// culture: chưa biết cách sài
			if (value is bool booleanValue)
			{
				return booleanValue ? Visibility.Visible : Visibility.Collapsed;
			}
			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Visibility visibility)
			{
				return visibility == Visibility.Visible;
			}
			return false;
		}
	}
}
