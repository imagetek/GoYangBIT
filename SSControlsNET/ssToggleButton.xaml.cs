using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SSControlsNET
{
	/// <summary>
	/// Interaction logic for ssToggleButton.xaml
	/// </summary>
	public partial class ssToggleButton : ToggleButton, IComponentConnector
	{
		public static readonly DependencyProperty EnabledUncheckedProperty = DependencyProperty.Register(nameof(EnabledUnchecked), typeof(ImageSource), typeof(ssToggleButton));
		public static readonly DependencyProperty EnabledCheckedProperty = DependencyProperty.Register(nameof(EnabledChecked), typeof(ImageSource), typeof(ssToggleButton));
		public static readonly DependencyProperty DisabledUncheckedProperty = DependencyProperty.Register(nameof(DisabledUnchecked), typeof(ImageSource), typeof(ssToggleButton));
		public static readonly DependencyProperty DisabledCheckedProperty = DependencyProperty.Register(nameof(DisabledChecked), typeof(ImageSource), typeof(ssToggleButton));
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(ssToggleButton), (PropertyMetadata)new UIPropertyMetadata((object)""));
		public static readonly DependencyProperty TextUncheckedColorProperty = DependencyProperty.Register(nameof(TextUncheckedColor), typeof(Brush), typeof(ssToggleButton), (PropertyMetadata)new UIPropertyMetadata((object)new SolidColorBrush(Colors.White)));
		public static readonly DependencyProperty TextCheckedColorProperty = DependencyProperty.Register(nameof(TextCheckedColor), typeof(Brush), typeof(ssToggleButton), (PropertyMetadata)new UIPropertyMetadata((object)new SolidColorBrush(Colors.Black)));
		public static readonly DependencyProperty TextMarginProperty = DependencyProperty.Register(nameof(TextMargin), typeof(Thickness), typeof(ssToggleButton), (PropertyMetadata)new UIPropertyMetadata((object)new Thickness(10.0)));
		public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(nameof(TextAlignment), typeof(HorizontalAlignment), typeof(ssToggleButton), new PropertyMetadata((object)HorizontalAlignment.Center));
		public static readonly DependencyProperty ImageStretchProperty = DependencyProperty.Register(nameof(ImageStretch), typeof(Stretch), typeof(ssToggleButton), new PropertyMetadata((object)Stretch.Uniform));
		private bool hoverdState;
		public ssToggleButton()
		{
			InitializeComponent();
		}

		public ImageSource EnabledUnchecked
		{
			get => (ImageSource)this.GetValue(ssToggleButton.EnabledUncheckedProperty);
			set => this.SetValue(ssToggleButton.EnabledUncheckedProperty, (object)value);
		}

		public ImageSource EnabledChecked
		{
			get => (ImageSource)this.GetValue(ssToggleButton.EnabledCheckedProperty);
			set => this.SetValue(ssToggleButton.EnabledCheckedProperty, (object)value);
		}

		public ImageSource DisabledUnchecked
		{
			get => (ImageSource)this.GetValue(ssToggleButton.DisabledUncheckedProperty);
			set => this.SetValue(ssToggleButton.DisabledUncheckedProperty, (object)value);
		}

		public ImageSource DisabledChecked
		{
			get => (ImageSource)this.GetValue(ssToggleButton.DisabledCheckedProperty);
			set => this.SetValue(ssToggleButton.DisabledCheckedProperty, (object)value);
		}

		public string Text
		{
			get => (string)this.GetValue(ssToggleButton.TextProperty);
			set => this.SetValue(ssToggleButton.TextProperty, (object)value);
		}

		public Brush TextUncheckedColor
		{
			get => (Brush)this.GetValue(ssToggleButton.TextUncheckedColorProperty);
			set => this.SetValue(ssToggleButton.TextUncheckedColorProperty, (object)value);
		}

		public Brush TextCheckedColor
		{
			get => (Brush)this.GetValue(ssToggleButton.TextCheckedColorProperty);
			set => this.SetValue(ssToggleButton.TextCheckedColorProperty, (object)value);
		}

		public Thickness TextMargin
		{
			get => (Thickness)this.GetValue(ssToggleButton.TextMarginProperty);
			set => this.SetValue(ssToggleButton.TextMarginProperty, (object)value);
		}

		public HorizontalAlignment TextAlignment
		{
			get => (HorizontalAlignment)this.GetValue(ssToggleButton.TextAlignmentProperty);
			set => this.SetValue(ssToggleButton.TextAlignmentProperty, (object)value);
		}

		public Stretch ImageStretch
		{
			get => (Stretch)this.GetValue(ssToggleButton.ImageStretchProperty);
			set => this.SetValue(ssToggleButton.ImageStretchProperty, (object)value);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			if (e.Property.Equals((object)ssToggleButton.EnabledUncheckedProperty) || e.Property.Equals((object)ssToggleButton.TextProperty) || e.Property.Equals((object)ssToggleButton.TextMarginProperty) || e.Property.Equals((object)ssToggleButton.TextUncheckedColorProperty) || e.Property.Equals((object)ssToggleButton.TextCheckedColorProperty) || e.Property.Equals((object)ssToggleButton.TextAlignmentProperty) || e.Property.Equals((object)ssToggleButton.ImageStretchProperty))
				this.ChangeImage();
			base.OnPropertyChanged(e);
		}

		private void ToggleButton_Loaded(object sender, RoutedEventArgs e) => this.ChangeImage();

		private void ToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			try
			{
				this.ChangeImage();
			}
			catch (Exception ex)
			{
			}
		}

		private void ToggleButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.ChangeImage();
		}

		private void ChangeImage()
		{
			try
			{
				this.ToggleButtonImage.Stretch = this.ImageStretch;
				this.ButtonText.Width = this.ActualWidth;
				this.ButtonText.Height = this.ActualHeight;
				if (!this.Text.Trim().Equals(""))
				{
					this.ButtonText.Visibility = Visibility.Visible;
					this.ButtonText.Caption = this.Text;
					this.ButtonText.HorzAlign = this.TextAlignment;
				}
				else
					this.ButtonText.Visibility = Visibility.Hidden;
				if (this.IsEnabled)
				{
					if (this.IsChecked.Value || this.hoverdState)
					{
						if (this.EnabledChecked != null)
						{
							this.ToggleButtonImage.Source = (ImageSource)null;
							this.ToggleButtonImage.Source = this.EnabledChecked;
						}
						this.ButtonText.ForeColor = this.TextCheckedColor;
					}
					else
					{
						this.ToggleButtonImage.Source = (ImageSource)null;
						if (this.EnabledUnchecked != null)
						{
							this.ToggleButtonImage.Source = (ImageSource)null;
							this.ToggleButtonImage.Source = this.EnabledUnchecked;
						}
						this.ButtonText.ForeColor = this.TextUncheckedColor;
					}
				}
				else if (this.IsChecked.Value)
				{
					if (this.DisabledChecked != null)
					{
						this.ToggleButtonImage.Source = (ImageSource)null;
						this.ToggleButtonImage.Source = this.DisabledChecked;
					}
					else
					{
						this.ToggleButtonImage.Source = (ImageSource)null;
						this.ToggleButtonImage.Source = this.EnabledChecked;
					}
					this.ButtonText.ForeColor = this.TextCheckedColor;
				}
				else
				{
					if (this.DisabledUnchecked != null)
					{
						this.ToggleButtonImage.Source = (ImageSource)null;
						this.ToggleButtonImage.Source = this.DisabledUnchecked;
					}
					else
					{
						this.ToggleButtonImage.Source = (ImageSource)null;
						this.ToggleButtonImage.Source = this.EnabledUnchecked;
					}
					this.ButtonText.ForeColor = this.TextUncheckedColor;
				}
			}
			catch (Exception ex)
			{
			}
		}

		private void ToggleButton_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.ChangeImage();
		}

		private void ToggleButton_MouseEnter(object sender, MouseEventArgs e)
		{
		}

		private void ToggleButton_MouseLeave(object sender, MouseEventArgs e)
		{
		}

		private void RadioButton_Unloaded(object sender, RoutedEventArgs e)
		{
			try
			{
				this.ToggleButtonImage.Source = (ImageSource)null;
			}
			catch (Exception ex)
			{
			}
		}

		private void RadioButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
		}
	}
}
