using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace SSControlsNET
{
	public class SSDrawingText : FrameworkElement
	{
		private bool AlreadyDisposed;
		protected string m_Text;
		private uint m_iRenderOption;
		private bool m_WordWrap;
		private HorizontalAlignment m_HorzAlign;
		private int m_iOverSizeX;
		private int m_iOverSizeY;
		private double m_Width;
		private double _strokeThickness;
		private double m_Height;
		private Brush _fill;
		private Brush _stroke;
		public Geometry _textGeometry;
		private FormattedText m_FormattedText;
		public static readonly DependencyProperty CaptionPropery = DependencyProperty.Register(nameof(Caption), typeof(string), typeof(SSDrawingText), (PropertyMetadata)new FrameworkPropertyMetadata((object)null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(SSDrawingText.OnCaptionProperyChanged)));
		public static readonly DependencyProperty BackColorProperty = DependencyProperty.Register(nameof(BackColor), typeof(Brush), typeof(SSDrawingText), (PropertyMetadata)new UIPropertyMetadata((object)new SolidColorBrush(Colors.White)));
		public static readonly DependencyProperty FFontFamilyProperty = DependencyProperty.Register(nameof(FFontFamily), typeof(FontFamily), typeof(SSDrawingText), (PropertyMetadata)new UIPropertyMetadata((object)new FontFamily("KoPubDotumMedium")));
		public static readonly DependencyProperty FFontSizeProperty = DependencyProperty.Register(nameof(FFontSize), typeof(double), typeof(SSDrawingText), (PropertyMetadata)new UIPropertyMetadata((object)50.0));
		public static readonly DependencyProperty FFontStyleProperty = DependencyProperty.Register(nameof(FFontStyle), typeof(FontStyle), typeof(SSDrawingText), (PropertyMetadata)new UIPropertyMetadata((object)FontStyles.Normal));
		public static readonly DependencyProperty FFontWeightProperty = DependencyProperty.Register(nameof(FFontWeight), typeof(FontWeight), typeof(SSDrawingText), (PropertyMetadata)new UIPropertyMetadata((object)FontWeights.Normal));
		public static readonly DependencyProperty FFontStretchProperty = DependencyProperty.Register(nameof(FFontStretch), typeof(FontStretch), typeof(SSDrawingText), (PropertyMetadata)new UIPropertyMetadata((object)FontStretches.Normal));
		public static readonly DependencyProperty ForeColorProperty = DependencyProperty.Register(nameof(ForeColor), typeof(Brush), typeof(SSDrawingText), (PropertyMetadata)new UIPropertyMetadata((object)new SolidColorBrush(Colors.Black)));

		public SSDrawingText()
		{
			int num1 = 200;
			int num2 = 100;
			int num3 = 5;
			int num4 = 3;
			this.m_Width = (double)(num1 - num3 * 2);
			this.m_Height = (double)(num2 - num4 * 2);
			this.m_Text = this.ToString();
			this._strokeThickness = 1.0;
			this.m_FormattedText = (FormattedText)null;
			this.m_iOverSizeX = num3;
			this.m_iOverSizeY = num4;
			this.m_iRenderOption = 0U;
			this.ClipToBounds = true;
			this.m_HorzAlign = HorizontalAlignment.Left;
			this.Changed();
		}

		private void Changed() => this.InvalidateVisual();

		private void CleanUp()
		{
		}

		protected void CreateText(
		  string pText,
		  int iLeft,
		  int iTop,
		  HorizontalAlignment AHorzAlign,
		  double PixelDPI = 200.0)
		{
			try
			{
				if (pText == null)
					pText = "";
				this.m_Text = pText;
				this.m_HorzAlign = AHorzAlign;
				this.m_FormattedText = (FormattedText)null;
				this.m_FormattedText = new FormattedText(this.m_Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(this.FFontFamily, this.FFontStyle, this.FFontWeight, FontStretches.Condensed), this.FFontSize, this.ForeColor, PixelDPI);
				this.m_FormattedText.TextAlignment = TextAlignment.Left;
				if (this.m_WordWrap)
				{
					this.m_FormattedText.MaxTextWidth = this.m_Width;
					this.m_FormattedText.MaxTextHeight = this.m_Height;
				}
				double trailingWhitespace = this.m_FormattedText.WidthIncludingTrailingWhitespace;
				this._textGeometry = (Geometry)null;
				if (this.m_Width - (double)(this.m_iOverSizeX * 2) < trailingWhitespace)
				{
					this._textGeometry = this.m_FormattedText.BuildGeometry(new Point((double)iLeft, (this.m_Height - this.m_FormattedText.Height) / 2.0));
					this._textGeometry.Transform = (Transform)new ScaleTransform((this.m_Width - (double)(this.m_iOverSizeX * 2)) / trailingWhitespace, 1.0);
				}
				else
				{
					double height = this.m_FormattedText.Height;
					switch (this.m_HorzAlign)
					{
						case HorizontalAlignment.Left:
							this._textGeometry = this.m_FormattedText.BuildGeometry(new Point((double)iLeft, (this.m_Height - this.m_FormattedText.Height) / 2.0));
							break;
						case HorizontalAlignment.Center:
							this._textGeometry = this.m_FormattedText.BuildGeometry(new Point((this.m_Width - trailingWhitespace) / 2.0, (this.m_Height - this.m_FormattedText.Height) / 2.0));
							break;
						case HorizontalAlignment.Right:
							this._textGeometry = this.m_FormattedText.BuildGeometry(new Point(this.m_Width - trailingWhitespace - (double)this.m_iOverSizeX, (this.m_Height - this.m_FormattedText.Height) / 2.0));
							break;
						case HorizontalAlignment.Stretch:
							this._textGeometry = this.m_FormattedText.BuildGeometry(new Point((this.m_Width - trailingWhitespace) / 2.0, (this.m_Height - this.m_FormattedText.Height) / 2.0));
							break;
						default:
							this._textGeometry = this.m_FormattedText.BuildGeometry(new Point((double)iLeft, (this.m_Height - this.m_FormattedText.Height) / 2.0));
							break;
					}
				}
				this._textGeometry.Freeze();
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("## {0}\r\n{1} ##", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void Dispose()
		{
			this.CleanUp();
			if (this.AlreadyDisposed)
				return;
			this.AlreadyDisposed = true;
			GC.SuppressFinalize((object)this);
		}

		~SSDrawingText() => this.Dispose();

		public DrawingVisual getDrawingVisual()
		{
			this.m_Width = this.Width;
			this.m_Height = this.Height;
			this.SetFigure(this.m_iRenderOption);
			this.CreateText(this.m_Text, this.m_iOverSizeX, this.m_iOverSizeY, this.m_HorzAlign);
			Pen pen = new Pen(this._stroke, this._strokeThickness);
			pen.StartLineCap = PenLineCap.Round;
			pen.EndLineCap = PenLineCap.Round;
			pen.LineJoin = PenLineJoin.Round;
			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext drawingContext = drawingVisual.RenderOpen();
			drawingContext.DrawGeometry(this._fill, pen, this._textGeometry);
			drawingContext.DrawGeometry(this._fill, new Pen(), this._textGeometry);
			drawingContext.Close();
			return drawingVisual;
		}

		public static double GetDrawingWidth(
		  string AText,
		  string AFontName,
		  double AFontSize,
		  double PixelDPI = 200.0)
		{
			return new FormattedText(AText, CultureInfo.GetCultureInfo(""), FlowDirection.LeftToRight, new Typeface(new FontFamily(AFontName), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), AFontSize, (Brush)Brushes.White, PixelDPI)
			{
				TextAlignment = TextAlignment.Left
			}.WidthIncludingTrailingWhitespace * 1.1;
		}

		public static int GetStringLines(string AText)
		{
			int length = AText.Length;
			char[] destination = new char[length];
			int stringLines = 0;
			AText.CopyTo(0, destination, 0, length);
			for (int index = 0; index < length; ++index)
			{
				if (destination[index] == '\n')
					++stringLines;
			}
			return stringLines;
		}

		public static int GetStringlist(
		  List<string> list,
		  string AText,
		  double cutWidth,
		  string AFontName,
		  double AFontSize,
		  ref int iCount)
		{
			int count1 = 0;
			if (list != null)
			{
				list.Clear();
				int num = (int)(SSDrawingText.GetDrawingWidth(AText, AFontName, AFontSize) / cutWidth);
				if (num <= 0)
				{
					list.Add(AText);
					iCount = 1;
					return AText.Length;
				}
				count1 = AText.Length / num;
				int count2 = AText.Length % num;
				iCount = num;
				char[] destination = new char[count1];
				for (int index = 0; index < num; ++index)
				{
					AText.CopyTo(index * count1, destination, 0, count1);
					string str = new string(destination);
					list.Add(str);
				}
				if (count2 <= 0)
					return count1;
				for (int index = 0; index < count1; ++index)
					destination[index] = ' ';
				AText.CopyTo(num * count1, destination, 0, count2);
				string str1 = new string(destination);
				list.Add(str1);
				++iCount;
			}
			return count1;
		}

		private static void OnCaptionProperyChanged(
		  DependencyObject o,
		  DependencyPropertyChangedEventArgs e)
		{
			SSDrawingText ssDrawingText = o as SSDrawingText;
			ssDrawingText.m_Text = ssDrawingText.Caption;
			ssDrawingText.Changed();
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			try
			{
				this.m_Width = this.Width;
				this.m_Height = this.Height;
				this.SetFigure(this.m_iRenderOption);
				this.CreateText(this.m_Text, this.m_iOverSizeX, this.m_iOverSizeY, this.m_HorzAlign);
				drawingContext.DrawGeometry(this._fill, new Pen(this._stroke, this._strokeThickness)
				{
					StartLineCap = PenLineCap.Round,
					EndLineCap = PenLineCap.Round,
					LineJoin = PenLineJoin.Round
				}, this._textGeometry);
				drawingContext.DrawGeometry(this._fill, new Pen(), this._textGeometry);
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("## {0}\r\n{1} ##", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		protected void SetFigure(uint iFigure) => this._fill = this.ForeColor;

		public Brush BackColor
		{
			get => (Brush)this.GetValue(SSDrawingText.BackColorProperty);
			set => this.SetValue(SSDrawingText.BackColorProperty, (object)value);
		}

		public string Caption
		{
			get => (string)this.GetValue(SSDrawingText.CaptionPropery);
			set => this.SetValue(SSDrawingText.CaptionPropery, (object)value);
		}

		public FontFamily FFontFamily
		{
			get => (FontFamily)this.GetValue(SSDrawingText.FFontFamilyProperty);
			set => this.SetValue(SSDrawingText.FFontFamilyProperty, (object)value);
		}

		public double FFontSize
		{
			get => (double)this.GetValue(SSDrawingText.FFontSizeProperty);
			set
			{
				this.SetValue(SSDrawingText.FFontSizeProperty, (object)value);
				this.Changed();
			}
		}

		public FontStyle FFontStyle
		{
			get => (FontStyle)this.GetValue(SSDrawingText.FFontStyleProperty);
			set => this.SetValue(SSDrawingText.FFontStyleProperty, (object)value);
		}

		public FontWeight FFontWeight
		{
			get => (FontWeight)this.GetValue(SSDrawingText.FFontWeightProperty);
			set => this.SetValue(SSDrawingText.FFontWeightProperty, (object)value);
		}

		public FontStretch FFontStretch
		{
			get => (FontStretch)this.GetValue(SSDrawingText.FFontStretchProperty);
			set => this.SetValue(SSDrawingText.FFontStretchProperty, (object)value);
		}

		public Brush ForeColor
		{
			get => (Brush)this.GetValue(SSDrawingText.ForeColorProperty);
			set => this.SetValue(SSDrawingText.ForeColorProperty, (object)value);
		}

		public HorizontalAlignment HorzAlign
		{
			get => this.m_HorzAlign;
			set
			{
				this.m_HorzAlign = value;
				this.Changed();
			}
		}

		public bool WordWrap
		{
			get => this.m_WordWrap;
			set
			{
				this.m_WordWrap = value;
				this.Changed();
			}
		}

		public uint RenderOption
		{
			get => this.m_iRenderOption;
			set
			{
				this.m_iRenderOption = value;
				this.Changed();
			}
		}

		public double StrokeThickness
		{
			get => this._strokeThickness;
			set
			{
				this._strokeThickness = value;
				this.Changed();
			}
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			if (e.Property.Equals((object)SSDrawingText.FFontSizeProperty) || e.Property.Equals((object)SSDrawingText.BackColorProperty) || e.Property.Equals((object)SSDrawingText.ForeColorProperty) || e.Property.Equals((object)SSDrawingText.FFontStyleProperty) || e.Property.Equals((object)SSDrawingText.FFontWeightProperty) || e.Property.Equals((object)SSDrawingText.FFontFamilyProperty) || e.Property.Equals((object)SSDrawingText.FFontStretchProperty))
				this.Changed();
			base.OnPropertyChanged(e);
		}
	}
}
