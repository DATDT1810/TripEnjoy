using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TripEnjoy.Presentation.WPF.Converters
{
    public class NotEmptyValidationRule : ValidationRule
    {
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
			{
				return new ValidationResult(false, "Trường không được để trống.");
			}
			return ValidationResult.ValidResult;
		}
	}
}
