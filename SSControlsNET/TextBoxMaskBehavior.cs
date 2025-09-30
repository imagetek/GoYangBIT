using System.Globalization;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace SSControlsNET
{
	public class TextBoxMaskBehavior
	{
		public static readonly DependencyProperty MinimumValueProperty = DependencyProperty.RegisterAttached("MinimumValue", typeof(double), typeof(TextBoxMaskBehavior), (PropertyMetadata)new FrameworkPropertyMetadata((object)double.NaN, new PropertyChangedCallback(TextBoxMaskBehavior.MinimumValueChangedCallback)));
		public static readonly DependencyProperty MaximumValueProperty = DependencyProperty.RegisterAttached("MaximumValue", typeof(double), typeof(TextBoxMaskBehavior), (PropertyMetadata)new FrameworkPropertyMetadata((object)double.NaN, new PropertyChangedCallback(TextBoxMaskBehavior.MaximumValueChangedCallback)));
		public static readonly DependencyProperty MaskProperty = DependencyProperty.RegisterAttached("Mask", typeof(MaskType), typeof(TextBoxMaskBehavior), (PropertyMetadata)new FrameworkPropertyMetadata(new PropertyChangedCallback(TextBoxMaskBehavior.MaskChangedCallback)));

		public static double GetMinimumValue(DependencyObject obj)
		{
			return (double)obj.GetValue(TextBoxMaskBehavior.MinimumValueProperty);
		}

		public static void SetMinimumValue(DependencyObject obj, double value)
		{
			obj.SetValue(TextBoxMaskBehavior.MinimumValueProperty, (object)value);
		}

		private static void MinimumValueChangedCallback(
		  DependencyObject d,
		  DependencyPropertyChangedEventArgs e)
		{
			TextBoxMaskBehavior.ValidateTextBox(d as TextBox);
		}

		public static double GetMaximumValue(DependencyObject obj)
		{
			return (double)obj.GetValue(TextBoxMaskBehavior.MaximumValueProperty);
		}

		public static void SetMaximumValue(DependencyObject obj, double value)
		{
			obj.SetValue(TextBoxMaskBehavior.MaximumValueProperty, (object)value);
		}

		private static void MaximumValueChangedCallback(
		  DependencyObject d,
		  DependencyPropertyChangedEventArgs e)
		{
			TextBoxMaskBehavior.ValidateTextBox(d as TextBox);
		}

		public static MaskType GetMask(DependencyObject obj)
		{
			return (MaskType)obj.GetValue(TextBoxMaskBehavior.MaskProperty);
		}

		public static void SetMask(DependencyObject obj, MaskType value)
		{
			obj.SetValue(TextBoxMaskBehavior.MaskProperty, (object)value);
		}

		private static void MaskChangedCallback(
		  DependencyObject d,
		  DependencyPropertyChangedEventArgs e)
		{
			if (e.OldValue is TextBox)
			{
				TextBox oldValue = e.OldValue as TextBox;
				oldValue.PreviewTextInput -= new TextCompositionEventHandler(TextBoxMaskBehavior.TextBox_PreviewTextInput);
				oldValue.LostFocus -= new RoutedEventHandler(TextBoxMaskBehavior._this_LostFocus);
				oldValue.PreviewKeyDown -= new KeyEventHandler(TextBoxMaskBehavior._this_KeyDown);
				DataObject.RemovePastingHandler((DependencyObject)(e.OldValue as TextBox), new DataObjectPastingEventHandler(TextBoxMaskBehavior.TextBoxPastingEventHandler));
			}
			if (!(d is TextBox textBox))
				return;
			if ((MaskType)e.NewValue != MaskType.Any)
			{
				textBox.PreviewTextInput += new TextCompositionEventHandler(TextBoxMaskBehavior.TextBox_PreviewTextInput);
				textBox.LostFocus += new RoutedEventHandler(TextBoxMaskBehavior._this_LostFocus);
				textBox.PreviewKeyDown += new KeyEventHandler(TextBoxMaskBehavior._this_KeyDown);
				DataObject.AddPastingHandler((DependencyObject)textBox, new DataObjectPastingEventHandler(TextBoxMaskBehavior.TextBoxPastingEventHandler));
			}
			TextBoxMaskBehavior.ValidateTextBox(textBox);
		}

		private static void _this_LostFocus(object sender, RoutedEventArgs e)
		{
			if (!(sender is TextBox textBox))
				return;
			if (textBox.Text.Trim().Equals(""))
			{
				textBox.Text = TextBoxMaskBehavior.GetMinimumValue((DependencyObject)textBox).ToString();
			}
			else
			{
				try
				{
					double num1 = Convert.ToDouble(textBox.Text);
					double num2 = TextBoxMaskBehavior.ValidateLimits(TextBoxMaskBehavior.GetMinimumValue((DependencyObject)textBox), TextBoxMaskBehavior.GetMaximumValue((DependencyObject)textBox), num1);
					if (num1 != num2)
					{
						textBox.Text = num2.ToString();
					}
					else
					{
						if (num1 != 0.0 || textBox.Text.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
							return;
						textBox.Text = "0";
					}
				}
				catch
				{
					textBox.Text = "0";
				}
			}
		}

		private static void _this_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key != Key.Return)
				return;
			TextBoxMaskBehavior._this_LostFocus(sender, (RoutedEventArgs)null);
		}

		private static void ValidateTextBox(TextBox _this)
		{
			if (TextBoxMaskBehavior.GetMask((DependencyObject)_this) == MaskType.Any)
				return;
			_this.Text = TextBoxMaskBehavior.ValidateValue(TextBoxMaskBehavior.GetMask((DependencyObject)_this), _this.Text, TextBoxMaskBehavior.GetMinimumValue((DependencyObject)_this), TextBoxMaskBehavior.GetMaximumValue((DependencyObject)_this));
		}

		private static void TextBoxPastingEventHandler(object sender, DataObjectPastingEventArgs e)
		{
			TextBox textBox = sender as TextBox;
			string data = e.DataObject.GetData(typeof(string)) as string;
			string str = TextBoxMaskBehavior.ValidateValue(TextBoxMaskBehavior.GetMask((DependencyObject)textBox), data, TextBoxMaskBehavior.GetMinimumValue((DependencyObject)textBox), TextBoxMaskBehavior.GetMaximumValue((DependencyObject)textBox));
			if (!string.IsNullOrEmpty(str))
				textBox.Text = str;
			e.CancelCommand();
			e.Handled = true;
		}

		private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			TextBox textBox = sender as TextBox;
			bool flag1 = TextBoxMaskBehavior.IsSymbolValid(TextBoxMaskBehavior.GetMask((DependencyObject)textBox), e.Text);
			e.Handled = !flag1;
			if (!flag1)
				return;
			int num1 = textBox.CaretIndex;
			string str = textBox.Text;
			bool flag2 = false;
			int num2 = 0;
			if (textBox.SelectionLength > 0)
			{
				str = str.Substring(0, textBox.SelectionStart) + str.Substring(textBox.SelectionStart + textBox.SelectionLength);
				num1 = textBox.SelectionStart;
			}
			if (e.Text == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
			{
				while (true)
				{
					int length;
					do
					{
						length = str.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
						if (length != -1)
							str = str.Substring(0, length) + str.Substring(length + 1);
						else
							goto label_7;
					}
					while (num1 <= length);
					--num1;
				}
				label_7:
				switch (num1)
				{
					case 0:
						str = "0" + str;
						++num1;
						break;
					case 1:
						if (string.Empty + str[0].ToString() == NumberFormatInfo.CurrentInfo.NegativeSign)
						{
							str = NumberFormatInfo.CurrentInfo.NegativeSign + "0" + str.Substring(1);
							++num1;
							break;
						}
						break;
				}
				if (num1 == str.Length)
				{
					num2 = 1;
					flag2 = true;
					str = str + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "0";
					++num1;
				}
			}
			else if (e.Text == NumberFormatInfo.CurrentInfo.NegativeSign)
			{
				flag2 = true;
				if (textBox.Text.Contains(NumberFormatInfo.CurrentInfo.NegativeSign))
				{
					str = str.Replace(NumberFormatInfo.CurrentInfo.NegativeSign, string.Empty);
					if (num1 != 0)
						--num1;
				}
				else
				{
					str = NumberFormatInfo.CurrentInfo.NegativeSign + textBox.Text;
					++num1;
				}
			}
			if (!flag2)
			{
				str = str.Substring(0, num1) + e.Text + (num1 < textBox.Text.Length ? str.Substring(num1) : string.Empty);
				++num1;
			}
			while (str.Length > 1 && str[0] == '0' && string.Empty + str[1].ToString() != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
			{
				str = str.Substring(1);
				if (num1 > 0)
					--num1;
			}
			while (str.Length > 2 && string.Empty + str[0].ToString() == NumberFormatInfo.CurrentInfo.NegativeSign && str[1] == '0' && string.Empty + str[2].ToString() != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
			{
				str = NumberFormatInfo.CurrentInfo.NegativeSign + str.Substring(2);
				if (num1 > 1)
					--num1;
			}
			if (num1 > str.Length)
				num1 = str.Length;
			textBox.Text = str;
			textBox.CaretIndex = num1;
			textBox.SelectionStart = num1;
			textBox.SelectionLength = num2;
			e.Handled = true;
		}

		private static string ValidateValue(MaskType mask, string value, double min, double max)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;
			value = value.Trim();
			if (mask != MaskType.Integer)
			{
				if (mask != MaskType.Decimal)
					return value;
				try
				{
					Convert.ToDouble(value);
					return value;
				}
				catch
				{
				}
				return string.Empty;
			}
			try
			{
				Convert.ToInt64(value);
				return value;
			}
			catch
			{
			}
			return string.Empty;
		}

		private static double ValidateLimits(double min, double max, double value)
		{
			if (!min.Equals(double.NaN) && value < min)
				return min;
			return !max.Equals(double.NaN) && value > max ? max : value;
		}

		private static bool IsSymbolValid(MaskType mask, string str)
		{
			switch (mask)
			{
				case MaskType.Any:
					return true;
				case MaskType.Integer:
					if (str == NumberFormatInfo.CurrentInfo.NegativeSign)
						return true;
					break;
				case MaskType.Decimal:
					if (str == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator || str == NumberFormatInfo.CurrentInfo.NegativeSign)
						return true;
					break;
			}
			if (!mask.Equals((object)MaskType.Integer) && !mask.Equals((object)MaskType.Decimal))
				return false;
			foreach (char c in str)
			{
				if (!char.IsDigit(c))
					return false;
			}
			return true;
		}
	}

	public enum MaskType
	{
		Any,
		Integer,
		Decimal,
	}
}
