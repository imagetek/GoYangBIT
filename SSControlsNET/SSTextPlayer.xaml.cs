using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


namespace SSControlsNET
{
	/// <summary>
	/// Interaction logic for SSTextPlayer.xaml
	/// </summary>
	public partial class SSTextPlayer : Canvas
	{
		private StringCollection _scTexts;
		private SceneDirection _sceneDirection;
		private DispatcherTimer _timer;
		private double _duration = 10000.0;
		private Image _Image1;
		private Image _Image2;
		private Image _PrevImage;
		private RotateTransform _RotateTransform;
		private ScaleTransform _ScaleTransform;
		private SkewTransform _SkewTransform;
		private TransformGroup _TransformGroup;
		private TranslateTransform _TranslateTransform;
		private bool _bIsOdd;
		private int _iCurrIndex;
		public static readonly DependencyProperty FFontFamilyProperty = DependencyProperty.Register(nameof(FFontFamily), typeof(FontFamily), typeof(SSTextPlayer), (PropertyMetadata)new UIPropertyMetadata((object)new FontFamily("NanumBarunGothic")));
		public static readonly DependencyProperty ForeColorProperty = DependencyProperty.Register(nameof(ForeColor), typeof(Brush), typeof(SSTextPlayer), (PropertyMetadata)new UIPropertyMetadata((object)new SolidColorBrush(Colors.White)));
		public static readonly DependencyProperty BackColorProperty = DependencyProperty.Register(nameof(BackColor), typeof(Brush), typeof(SSTextPlayer), (PropertyMetadata)new UIPropertyMetadata((object)new SolidColorBrush(Colors.White)));
		private double _fontSize = 30.0;
		public static readonly DependencyProperty FFontStyleProperty = DependencyProperty.Register(nameof(FFontStyle), typeof(FontStyle), typeof(SSTextPlayer), (PropertyMetadata)new UIPropertyMetadata((object)FontStyles.Normal));
		public static readonly DependencyProperty FFontWeightProperty = DependencyProperty.Register(nameof(FFontWeight), typeof(FontWeight), typeof(SSTextPlayer), (PropertyMetadata)new UIPropertyMetadata((object)FontWeights.Normal));
		
		public SSTextPlayer()
		{
			try
			{
				InitializeComponent();
				_scTexts = new StringCollection();
			}
			catch (Exception ex)
			{
			}
		}

		public double Duration
		{
			get => this._duration;
			set => this._duration = value;
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				this._RotateTransform = (RotateTransform)null;
				this._ScaleTransform = (ScaleTransform)null;
				this._SkewTransform = (SkewTransform)null;
				if (this._TransformGroup != null)
				{
					this._TransformGroup.Children.Clear();
					this._TransformGroup = (TransformGroup)null;
				}
				this.RenderTransform = (Transform)null;
				this.InitTransform((UIElement)this);
				this._Image1 = new Image();
				this._Image1.Width = this.ActualWidth;
				this._Image1.Height = this.ActualHeight;
				this._Image2 = new Image();
				this._Image2.Width = this.ActualWidth;
				this._Image2.Height = this.ActualHeight;
				this.Children.Clear();
				this.Children.Add((UIElement)this._Image1);
				this.Children.Add((UIElement)this._Image2);
				this.RenderTransform = (Transform)null;
				this.ClipToBounds = true;
			}
			catch (Exception ex)
			{
			}
		}

		public void AddMessages(List<string> msgs)
		{
			try
			{
				this._scTexts.Clear();
				foreach (string msg in msgs)
					this._scTexts.Add(msg);
			}
			catch (Exception ex)
			{
			}
		}

		public void InitProc()
		{
			try
			{
				this._scTexts.Clear();
				this.CleanImage();
			}
			catch (Exception ex)
			{
			}
		}

		public void Start()
		{
			try
			{
				if (this._timer == null)
				{
					this._timer = new DispatcherTimer();
					this._timer.Interval = TimeSpan.FromMilliseconds(this._duration);
					this._timer.Tag = (object)0;
					this._timer.Tick += new EventHandler(this._timer_Tick);
				}
				this._timer_Tick((object)null, (EventArgs)null);
				this._timer.Start();
			}
			catch (Exception ex)
			{
			}
		}

		public void Stop()
		{
			try
			{
				if (this._timer == null)
					return;
				this._timer.Stop();
				this._timer.Tick -= new EventHandler(this._timer_Tick);
				this._timer = (DispatcherTimer)null;
			}
			catch (Exception ex)
			{
			}
		}

		private void _timer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (Convert.ToInt32(this._timer.Tag) == 1)
					return;
				this._timer.Tag = (object)1;
				this.Dispatcher.BeginInvoke(DispatcherPriority.Render, (Delegate)new SSTextPlayer.ChangeDelegate(this.Transition));
				this._timer.Tag = (object)0;
			}
			catch (Exception ex)
			{
				this._timer.Tag = (object)0;
			}
		}

		private void CleanUpPrevImage()
		{
			try
			{
				if (this._PrevImage == null)
					return;
				this._PrevImage.Visibility = Visibility.Hidden;
				this._PrevImage.Source = (ImageSource)null;
				this._PrevImage.Effect = (Effect)null;
				this._PrevImage = (Image)null;
			}
			catch (Exception ex)
			{
			}
		}

		public SceneDirection SceneDirection
		{
			get => this._sceneDirection;
			set => this._sceneDirection = value;
		}

		private void CleanUpTransform(UIElement AElement)
		{
			try
			{
				AElement.RenderTransform = (Transform)null;
				if (this._TransformGroup != null)
					this._TransformGroup.Children.Clear();
				this._TranslateTransform = (TranslateTransform)null;
			}
			catch (Exception ex)
			{
			}
		}

		private void InitTransform(UIElement AElement)
		{
			try
			{
				this.CleanUpTransform(AElement);
				this._TranslateTransform = new TranslateTransform(0.0, 0.0);
				this._RotateTransform = new RotateTransform();
				this._RotateTransform.CenterX = this.Width / 2.0;
				this._RotateTransform.CenterY = this.Height / 2.0;
				this._RotateTransform.Angle = 0.0;
				this._ScaleTransform = new ScaleTransform(1.0, 1.0, this.Width / 2.0, this.Height / 2.0);
				this._SkewTransform = new SkewTransform();
				this._TransformGroup = new TransformGroup();
				this._TransformGroup.Children.Add((Transform)this._TranslateTransform);
				this._TransformGroup.Children.Add((Transform)this._RotateTransform);
				this._TransformGroup.Children.Add((Transform)this._ScaleTransform);
				this._TransformGroup.Children.Add((Transform)this._SkewTransform);
				AElement.RenderTransform = (Transform)this._TransformGroup;
			}
			catch (Exception ex)
			{
			}
		}

		private void ChangeScene(Image nextImage, Image prevImage)
		{
			try
			{
				this._PrevImage = prevImage;
				if (nextImage == null || prevImage == null)
					return;
				this.InitTransform((UIElement)nextImage);
				prevImage.Source = (ImageSource)null;
				prevImage.Effect = (Effect)null;
				prevImage.Visibility = Visibility.Visible;
				nextImage.Visibility = Visibility.Visible;
				nextImage.BringIntoView();
				SlideInTransitionEffect transitionEffect1 = new SlideInTransitionEffect();
				switch (this._sceneDirection)
				{
					case SceneDirection.SD_ToUp:
						transitionEffect1.SlideAmount = new Point(0.0, 1.0);
						break;
					case SceneDirection.SD_ToDown:
						transitionEffect1.SlideAmount = new Point(0.0, -1.0);
						break;
					case SceneDirection.SD_ToLeft:
						transitionEffect1.SlideAmount = new Point(1.0, 0.0);
						break;
					case SceneDirection.SD_ToRight:
						transitionEffect1.SlideAmount = new Point(-1.0, 0.0);
						break;
					case SceneDirection.SD_LeftToUp:
						transitionEffect1.SlideAmount = new Point(-1.0, 1.0);
						break;
					case SceneDirection.SD_UpToLeft:
						transitionEffect1.SlideAmount = new Point(1.0, -1.0);
						break;
					case SceneDirection.SD_RightToUp:
						transitionEffect1.SlideAmount = new Point(1.0, 1.0);
						break;
					case SceneDirection.SD_UpToRight:
						transitionEffect1.SlideAmount = new Point(-1.0, -1.0);
						break;
					default:
						transitionEffect1.SlideAmount = new Point(0.0, 1.0);
						break;
				}
				SlideInTransitionEffect transitionEffect2 = transitionEffect1;
				DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
				animation.KeyFrames.Add((DoubleKeyFrame)new SplineDoubleKeyFrame(0.0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0))));
				animation.KeyFrames.Add((DoubleKeyFrame)new SplineDoubleKeyFrame(75.0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(this._duration / 2.0))));
				animation.KeyFrames.Add((DoubleKeyFrame)new SplineDoubleKeyFrame(100.0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(this._duration))));
				RenderTargetBitmap image = new RenderTargetBitmap((int)this.Width, (int)this.Height, 96.0, 96.0, PixelFormats.Pbgra32);
				image.Render((Visual)prevImage);
				ImageBrush imageBrush = new ImageBrush((ImageSource)image);
				transitionEffect2.Texture2 = (Brush)imageBrush;
				nextImage.Effect = (Effect)transitionEffect2;
				this.CleanUpPrevImage();
				transitionEffect2.BeginAnimation(SlideInTransitionEffect.ProgressProperty, (AnimationTimeline)animation);
			}
			catch (Exception ex)
			{
			}
		}

		public FontFamily FFontFamily
		{
			get => (FontFamily)this.GetValue(SSTextPlayer.FFontFamilyProperty);
			set => this.SetValue(SSTextPlayer.FFontFamilyProperty, (object)value);
		}

		public Brush ForeColor
		{
			get => (Brush)this.GetValue(SSTextPlayer.ForeColorProperty);
			set => this.SetValue(SSTextPlayer.ForeColorProperty, (object)value);
		}

		public Brush BackColor
		{
			get => (Brush)this.GetValue(SSTextPlayer.BackColorProperty);
			set => this.SetValue(SSTextPlayer.BackColorProperty, (object)value);
		}

		public double FFontSize
		{
			get => this._fontSize;
			set => this._fontSize = value;
		}

		public FontStyle FFontStyle
		{
			get => (FontStyle)this.GetValue(SSTextPlayer.FFontStyleProperty);
			set => this.SetValue(SSTextPlayer.FFontStyleProperty, (object)value);
		}

		public FontWeight FFontWeight
		{
			get => (FontWeight)this.GetValue(SSTextPlayer.FFontWeightProperty);
			set => this.SetValue(SSTextPlayer.FFontWeightProperty, (object)value);
		}

		private void CleanUp()
		{
			try
			{
				this.CacheMode = (CacheMode)null;
				this.CleanUpTransform((UIElement)this);
				this.Children.Clear();
				this.Effect = (Effect)null;
				this.Background = (Brush)null;
				this.Dispatcher.UnhandledException -= new DispatcherUnhandledExceptionEventHandler(this.Dispatcher_UnhandledException);
			}
			catch (Exception ex)
			{
			}
		}

		private void Dispatcher_UnhandledException(
		  object sender,
		  DispatcherUnhandledExceptionEventArgs e)
		{
			Trace.WriteLine(string.Format("{0} 에서 예기치 않은 오류가 발생했습니다", (object)this.Name));
		}

		private void CleanImage()
		{
			try
			{
				if (this._Image2 != null)
				{
					this._Image2.Source = (ImageSource)null;
					this._Image2.Effect = (Effect)null;
					this._Image2 = (Image)null;
				}
				if (this._Image1 == null)
					return;
				this._Image1.Source = (ImageSource)null;
				this._Image1.Effect = (Effect)null;
				this._Image1 = (Image)null;
			}
			catch (Exception ex)
			{
			}
		}

		public event SSTextPlayer.OnCompleteDelegate OnComplete;

		private void Transition()
		{
			try
			{
				this.CleanUpPrevImage();
				this._PrevImage = (Image)null;
				if (this._scTexts.Count > 0)
				{
					if (this._bIsOdd)
					{
						if (this._Image2 != null)
						{
							this.Children.Remove((UIElement)this._Image2);
							this._Image2.Source = (ImageSource)null;
							this._Image2.Effect = (Effect)null;
							this._Image2 = (Image)null;
							SSDrawingText ssDrawingText = new SSDrawingText();
							ssDrawingText.Width = this.Width;
							ssDrawingText.Height = this.Height;
							ssDrawingText.Caption = this._scTexts[this._iCurrIndex];
							ssDrawingText.ForeColor = this.ForeColor;
							ssDrawingText.BackColor = this.BackColor;
							ssDrawingText.FFontFamily = this.FFontFamily;
							ssDrawingText.FFontSize = this._fontSize;
							ssDrawingText.FFontStyle = this.FFontStyle;
							ssDrawingText.FFontWeight = this.FFontWeight;
							ssDrawingText.RenderOption = 1U;
							this._Image2 = new Image();
							this._Image2.Width = this.Width;
							this._Image2.Height = this.Height;
							RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)this.Width, (int)this.Height, 96.0, 96.0, PixelFormats.Pbgra32);
							renderTargetBitmap.Render((Visual)ssDrawingText.getDrawingVisual());
							this._Image2.Source = (ImageSource)null;
							this._Image2.Source = (ImageSource)renderTargetBitmap;
							renderTargetBitmap.Freeze();
							this._PrevImage = this._Image1;
							this.Children.Add((UIElement)this._Image2);
						}
						this.ChangeScene(this._Image2, this._Image1);
						this._bIsOdd = false;
					}
					else
					{
						if (this._Image1 != null)
						{
							this.Children.Remove((UIElement)this._Image1);
							this._Image1.Source = (ImageSource)null;
							this._Image1.Effect = (Effect)null;
							this._Image1 = (Image)null;
							SSDrawingText ssDrawingText = new SSDrawingText();
							ssDrawingText.Width = this.Width;
							ssDrawingText.Height = this.Height;
							ssDrawingText.Caption = this._scTexts[this._iCurrIndex];
							ssDrawingText.ForeColor = this.ForeColor;
							ssDrawingText.BackColor = this.BackColor;
							ssDrawingText.FFontFamily = this.FFontFamily;
							ssDrawingText.FFontSize = this._fontSize;
							ssDrawingText.FFontStyle = this.FFontStyle;
							ssDrawingText.RenderOption = 1U;
							this._Image1 = new Image();
							this._Image1.Width = this.Width;
							this._Image1.Height = this.Height;
							RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)this.Width, (int)this.Height, 96.0, 96.0, PixelFormats.Pbgra32);
							renderTargetBitmap.Render((Visual)ssDrawingText.getDrawingVisual());
							this._Image1.Source = (ImageSource)null;
							this._Image1.Source = (ImageSource)renderTargetBitmap;
							renderTargetBitmap.Freeze();
							this._PrevImage = this._Image2;
							this.Children.Add((UIElement)this._Image1);
						}
						this.ChangeScene(this._Image1, this._Image2);
						this._bIsOdd = true;
					}
					++this._iCurrIndex;
					if (this._iCurrIndex < this._scTexts.Count)
						return;
					if (this.OnComplete != null)
						this.OnComplete();
					this._iCurrIndex = 0;
				}
				else
				{
					if (this.OnComplete != null)
						this.OnComplete();
					this._iCurrIndex = 0;
					if (this._Image1 != null)
					{
						this.Children.Remove((UIElement)this._Image1);
						this._Image1.Source = (ImageSource)null;
						this._Image1.Effect = (Effect)null;
						this._Image1 = (Image)null;
						SSDrawingText ssDrawingText = new SSDrawingText();
						ssDrawingText.Width = this.Width;
						ssDrawingText.Height = this.Height;
						ssDrawingText.Caption = "";
						ssDrawingText.ForeColor = this.ForeColor;
						ssDrawingText.BackColor = this.BackColor;
						ssDrawingText.FFontFamily = this.FFontFamily;
						ssDrawingText.FFontSize = this._fontSize;
						ssDrawingText.FFontStyle = this.FFontStyle;
						ssDrawingText.RenderOption = 1U;
						this._Image1 = new Image();
						this._Image1.Width = this.Width;
						this._Image1.Height = this.Height;
						RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)this.Width, (int)this.Height, 96.0, 96.0, PixelFormats.Pbgra32);
						renderTargetBitmap.Render((Visual)ssDrawingText.getDrawingVisual());
						this._Image1.Source = (ImageSource)null;
						this._Image1.Source = (ImageSource)renderTargetBitmap;
						renderTargetBitmap.Freeze();
						this._PrevImage = this._Image2;
						this.Children.Add((UIElement)this._Image1);
					}
					this.ChangeScene(this._Image1, this._Image2);
				}
			}
			catch (Exception ex)
			{
			}
		}

		private void Canvas_Unloaded(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this._timer != null)
				{
					this._timer.Stop();
					this._timer.Tick -= new EventHandler(this._timer_Tick);
					this._timer = (DispatcherTimer)null;
				}
				if (this._scTexts != null)
					this._scTexts.Clear();
				this.CleanUp();
			}
			catch (Exception ex)
			{
			}
		}

		public delegate void ChangeDelegate();

		public delegate void OnCompleteDelegate();
	}
}
