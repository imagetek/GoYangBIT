using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SSControlsNET
{
	/// <summary>
	/// Interaction logic for ssButton.xaml
	/// </summary>
	public partial class ssButton : Button, IComponentConnector
	{
		public static readonly DependencyProperty EnabledReleasedProperty = DependencyProperty.Register(nameof(EnabledReleased), typeof(ImageSource), typeof(ssButton));
		public static readonly DependencyProperty EnabledClickedProperty = DependencyProperty.Register(nameof(EnabledClicked), typeof(ImageSource), typeof(ssButton));
		public static readonly DependencyProperty DisabledReleasedProperty = DependencyProperty.Register(nameof(DisabledReleased), typeof(ImageSource), typeof(ssButton));
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(ssButton), (PropertyMetadata)new UIPropertyMetadata((object)""));
		public static readonly DependencyProperty HoverProperty = DependencyProperty.Register(nameof(IsHover), typeof(bool), typeof(ssButton), (PropertyMetadata)new UIPropertyMetadata((object)false));
		public static readonly DependencyProperty TextRelasedColorProperty = DependencyProperty.Register(nameof(TextReleasedColor), typeof(Brush), typeof(ssButton), (PropertyMetadata)new UIPropertyMetadata((object)new SolidColorBrush(Colors.White)));
		public static readonly DependencyProperty TextClickedColorProperty = DependencyProperty.Register(nameof(TextClickedColor), typeof(Brush), typeof(ssButton), (PropertyMetadata)new UIPropertyMetadata((object)new SolidColorBrush(Colors.Black)));
		public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(nameof(TextAlignment), typeof(HorizontalAlignment), typeof(ssButton), new PropertyMetadata((object)HorizontalAlignment.Center));
		public static readonly DependencyProperty ImageStretchProperty = DependencyProperty.Register(nameof(ImageStretch), typeof(Stretch), typeof(ssButton), new PropertyMetadata((object)Stretch.Uniform));
		private bool clickedState;
		private bool hoverdState;
		//internal ssButton btnMain;
		//internal Image ButtonImage;
		//internal SSDrawingText ButtonText;
		//private bool _contentLoaded;

		public ssButton() => this.InitializeComponent();

		public ImageSource EnabledReleased
		{
			get => (ImageSource)this.GetValue(ssButton.EnabledReleasedProperty);
			set => this.SetValue(ssButton.EnabledReleasedProperty, (object)value);
		}

		public ImageSource EnabledClicked
		{
			get => (ImageSource)this.GetValue(ssButton.EnabledClickedProperty);
			set => this.SetValue(ssButton.EnabledClickedProperty, (object)value);
		}

		public ImageSource DisabledReleased
		{
			get => (ImageSource)this.GetValue(ssButton.DisabledReleasedProperty);
			set => this.SetValue(ssButton.DisabledReleasedProperty, (object)value);
		}

		public string Text
		{
			get => (string)this.GetValue(ssButton.TextProperty);
			set => this.SetValue(ssButton.TextProperty, (object)value);
		}

		public bool IsHover
		{
			get => (bool)this.GetValue(ssButton.HoverProperty);
			set => this.SetValue(ssButton.HoverProperty, (object)value);
		}

		public Brush TextReleasedColor
		{
			get => (Brush)this.GetValue(ssButton.TextRelasedColorProperty);
			set => this.SetValue(ssButton.TextRelasedColorProperty, (object)value);
		}

		public Brush TextClickedColor
		{
			get => (Brush)this.GetValue(ssButton.TextClickedColorProperty);
			set => this.SetValue(ssButton.TextClickedColorProperty, (object)value);
		}

		public HorizontalAlignment TextAlignment
		{
			get => (HorizontalAlignment)this.GetValue(ssButton.TextAlignmentProperty);
			set => this.SetValue(ssButton.TextAlignmentProperty, (object)value);
		}

		public Stretch ImageStretch
		{
			get => (Stretch)this.GetValue(ssButton.ImageStretchProperty);
			set => this.SetValue(ssButton.ImageStretchProperty, (object)value);
		}
		//public ssButton()
		//{
		//	InitializeComponent();
		//}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			if (e.Property.Equals((object)ssButton.EnabledReleasedProperty) || e.Property.Equals((object)ssButton.EnabledClickedProperty) || e.Property.Equals((object)ssButton.DisabledReleasedProperty) || e.Property.Equals((object)ssButton.TextProperty) || e.Property.Equals((object)ssButton.TextRelasedColorProperty) || e.Property.Equals((object)ssButton.TextClickedColorProperty) || e.Property.Equals((object)ssButton.TextAlignmentProperty) || e.Property.Equals((object)ssButton.ImageStretchProperty))
				this.ChangeImage();
			base.OnPropertyChanged(e);
		}

		private void Button_Loaded(object sender, RoutedEventArgs e) => this.ChangeImage();

		private void Button_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.ChangeImage();
		}

		private void ChangeImage()
		{
			try
			{
				this.ButtonImage.Stretch = this.ImageStretch;
				this.ButtonText.FFontSize = this.FontSize;
				this.ButtonText.Width = this.ActualWidth;
				this.ButtonText.Height = this.ActualHeight;
				if (!this.Text.Trim().Equals(""))
				{
					this.ButtonText.Caption = this.Text;
					this.ButtonText.HorzAlign = this.TextAlignment;
					this.ButtonText.Visibility = Visibility.Visible;
				}
				else
					this.ButtonText.Visibility = Visibility.Hidden;
				if (this.IsEnabled)
				{
					if (this.clickedState || this.hoverdState)
					{
						if (this.EnabledClicked != null)
						{
							this.ButtonImage.Source = (ImageSource)null;
							this.ButtonImage.Source = this.EnabledClicked;
						}
						this.ButtonText.ForeColor = this.TextClickedColor;
						this.ButtonText.BackColor = this.TextClickedColor;
					}
					else
					{
						if (this.EnabledReleased != null)
						{
							this.ButtonImage.Source = (ImageSource)null;
							this.ButtonImage.Source = this.EnabledReleased;
						}
						this.ButtonText.ForeColor = TextReleasedColor;
						this.ButtonText.BackColor = this.TextReleasedColor;
					}
				}
				else
				{
					if (this.DisabledReleased != null)
					{
						this.ButtonImage.Source = (ImageSource)null;
						this.ButtonImage.Source = this.DisabledReleased;
					}
					else
					{
						this.ButtonImage.Source = (ImageSource)null;
						this.ButtonImage.Source = this.EnabledReleased;
					}
					this.ButtonText.ForeColor = this.TextReleasedColor;
					this.ButtonText.BackColor = this.TextReleasedColor;
				}
			}
			catch (Exception ex)
			{
			}
		}

		private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				this.clickedState = true;
				this.ChangeImage();
			}
			catch (Exception ex)
			{
			}
		}

		private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			try
			{
				this.clickedState = false;
				this.ChangeImage();
			}
			catch (Exception ex)
			{
			}
		}

		private void Button_MouseEnter(object sender, MouseEventArgs e)
		{
			if (this.IsHover)
				this.hoverdState = true;
			this.ChangeImage();
		}

		private void Button_MouseLeave(object sender, MouseEventArgs e)
		{
			this.hoverdState = false;
			this.ChangeImage();
		}

		private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			try
			{
				this.ChangeImage();
			}
			catch (Exception ex)
			{
			}
		}

		private void Button_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.ChangeImage();
		}

		private void Button_Unloaded(object sender, RoutedEventArgs e)
		{
			try
			{
				this.ButtonImage.Source = (ImageSource)null;
			}
			catch (Exception ex)
			{
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
		}

		//[DebuggerNonUserCode]
		//[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		//public void InitializeComponent()
		//{
		//	if (this._contentLoaded)
		//		return;
		//	this._contentLoaded = true;
		//	Application.LoadComponent((object)this, new Uri("/SSControlsNET;component/imagebutton/ssbutton.xaml", UriKind.Relative));
		//}

		//[DebuggerNonUserCode]
		//[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		//internal Delegate _CreateDelegate(Type delegateType, string handler)
		//{
		//	return Delegate.CreateDelegate(delegateType, (object)this, handler);
		//}

		//[DebuggerNonUserCode]
		//[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		//[EditorBrowsable(EditorBrowsableState.Never)]
		//void IComponentConnector.Connect(int connectionId, object target)
		//{
		//	switch (connectionId)
		//	{
		//		case 1:
		//			this.btnMain = (ssButton)target;
		//			this.btnMain.Loaded += new RoutedEventHandler(this.Button_Loaded);
		//			this.btnMain.Unloaded += new RoutedEventHandler(this.Button_Unloaded);
		//			this.btnMain.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.Button_IsVisibleChanged);
		//			this.btnMain.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.Button_IsEnabledChanged);
		//			this.btnMain.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.Button_MouseLeftButtonDown);
		//			this.btnMain.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.Button_MouseLeftButtonUp);
		//			this.btnMain.SizeChanged += new SizeChangedEventHandler(this.Button_SizeChanged);
		//			this.btnMain.MouseEnter += new MouseEventHandler(this.Button_MouseEnter);
		//			this.btnMain.MouseLeave += new MouseEventHandler(this.Button_MouseLeave);
		//			this.btnMain.Click += new RoutedEventHandler(this.Button_Click);
		//			break;
		//		case 2:
		//			this.ButtonImage = (Image)target;
		//			break;
		//		case 3:
		//			this.ButtonText = (SSDrawingText)target;
		//			break;
		//		default:
		//			this._contentLoaded = true;
		//			break;
		//	}
		//}
	}
}
