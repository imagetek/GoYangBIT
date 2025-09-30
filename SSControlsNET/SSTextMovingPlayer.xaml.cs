using SSCommonNET;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SSControlsNET
{
	/// <summary>
	/// Interaction logic for SSTextMovingPlayer.xaml
	/// </summary>
	public partial class SSTextMovingPlayer : UserControl, IComponentConnector
	{
		private Window _p;
		private bool _isLoaded;
		private DispatcherTimer _dt이동;
		private DispatcherTimer _dt대기;
		private double fMovingPoint;
		private TextAlignment mTextAlign;
		private double TotalWidth;
		private double TextWidth;
		private int OverWidth = 2;
		private double mvPoint;
		public SSTextMovingPlayer()
		{
			InitializeComponent();
		}

		public void SetParentWindow(Window p) => this._p = p;

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this._isLoaded)
					return;
				this.InitProc();
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
			finally
			{
				this._isLoaded = true;
			}
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			try
			{
				Console.WriteLine("### Unloaded : SSTextMovePlayer ###");
				this.DoFinal();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void DoFinal()
		{
			try
			{
				if (this._dt이동 != null)
				{
					this._dt이동.Stop();
					this._dt이동.Tick -= new EventHandler(this._dt이동_Tick);
					this._dt이동 = (DispatcherTimer)null;
				}
				if (this._dt대기 != null)
				{
					this._dt대기.Stop();
					this._dt대기.Tick -= new EventHandler(this._dt대기_Tick);
					this._dt대기 = (DispatcherTimer)null;
				}
				this.txtplayer.Text = "";
				GC.Collect();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void InitProc()
		{
			try
			{
				if (this._dt이동 == null)
				{
					this._dt이동 = new DispatcherTimer();
					this._dt이동.Tick += new EventHandler(this._dt이동_Tick);
				}
				this._dt이동.Interval = TimeSpan.FromMilliseconds(25.0);
				this._dt이동.Tag = (object)0;
				if (this._dt대기 == null)
				{
					this._dt대기 = new DispatcherTimer();
					this._dt대기.Tick += new EventHandler(this._dt대기_Tick);
				}
				this._dt대기.Interval = TimeSpan.FromMilliseconds(2500.0);
				this._dt대기.Tag = (object)0;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void _dt이동_Tick(object sender, EventArgs e)
		{
			try
			{
				if (Convert.ToInt32(this._dt이동.Tag) == 1)
					return;
				this._dt이동.Tag = (object)1;
				this.fMovingPoint = (this.TextWidth - this.TotalWidth) * -1.0;
				if (this.fMovingPoint > 0.0)
					this.fMovingPoint *= -1.0;
				this.Display글자이동();
				this._dt이동.Tag = (object)0;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				this._dt이동.Tag = (object)0;
			}
		}

		private void _dt대기_Tick(object sender, EventArgs e)
		{
			try
			{
				if (Convert.ToInt32(this._dt대기.Tag) == 1)
					return;
				this._dt대기.Tag = (object)1;
				if (this._dt이동 != null)
					this._dt이동.Start();
				this._dt대기.Tag = (object)0;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				this._dt대기.Tag = (object)0;
			}
		}

		private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			try
			{
				Size newSize1 = e.NewSize;
				// ISSUE: variable of a boxed type
				double width1 = newSize1.Width;
				newSize1 = e.NewSize;
				// ISSUE: variable of a boxed type
				double height = newSize1.Height;
				Console.WriteLine("SSTextMovePlayer SizeChange {0}x{1}", (object)width1, (object)height);
				if (e.WidthChanged)
				{
					ScrollViewer svMain = this.svMain;
					Size newSize2 = e.NewSize;
					double width2 = newSize2.Width;
					svMain.Width = width2;
					newSize2 = e.NewSize;
					this.TotalWidth = newSize2.Width;
				}
				if (!e.HeightChanged)
					return;
				this.svMain.Height = e.NewSize.Height;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public double MeasureTextWidth(TextBlock tb, string text)
		{
			try
			{
				return new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(tb.FontFamily, tb.FontStyle, tb.FontWeight, tb.FontStretch), tb.FontSize, (Brush)Brushes.White, new NumberSubstitution(), 200.0).Width;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return 0.0;
			}
		}

		public void SetFont설정(
		  string FontName,
		  double FontSize,
		  string FontColor,
		  int FontStyleGBN = 0,
		  int FontWeightGBN = 400,
		  int OverLength = 5)
		{
			try
			{
				this.txtplayer.FontFamily = new FontFamily(FontName);
				this.txtplayer.FontSize = FontSize;
				switch (FontStyleGBN)
				{
					case 0:
						this.txtplayer.FontStyle = FontStyles.Normal;
						break;
					case 1:
						this.txtplayer.FontStyle = FontStyles.Oblique;
						break;
					case 2:
						this.txtplayer.FontStyle = FontStyles.Italic;
						break;
				}
				switch (FontWeightGBN)
				{
					case 100:
						this.txtplayer.FontWeight = FontWeights.Thin;
						break;
					case 200:
						this.txtplayer.FontWeight = FontWeights.ExtraLight;
						break;
					case 300:
						this.txtplayer.FontWeight = FontWeights.Light;
						break;
					case 400:
						this.txtplayer.FontWeight = FontWeights.Normal;
						break;
					case 500:
						this.txtplayer.FontWeight = FontWeights.Medium;
						break;
					case 600:
						this.txtplayer.FontWeight = FontWeights.DemiBold;
						break;
					case 700:
						this.txtplayer.FontWeight = FontWeights.Bold;
						break;
					case 800:
						this.txtplayer.FontWeight = FontWeights.ExtraBold;
						break;
					case 900:
						this.txtplayer.FontWeight = FontWeights.Heavy;
						break;
					case 950:
						this.txtplayer.FontWeight = FontWeights.ExtraBlack;
						break;
				}
				this.txtplayer.Foreground = (Brush)ConvertUtils.ColorByRGBCode(FontColor);
				this.OverWidth = OverLength;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void SetMargin(double left, double top)
		{
			try
			{
				this.txtplayer.Margin = new Thickness(left, top, 0.0, 0.0);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void SetTextAlign(int nTextAlign)
		{
			try
			{
				this.mTextAlign = (TextAlignment)nTextAlign;
				this.txtplayer.TextAlignment = this.mTextAlign;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void DisplayText(string textMsg)
		{
			try
			{
				this.ClearPlayer();
				this.txtplayer.Text = textMsg;
				this.TextWidth = this.MeasureTextWidth(this.txtplayer, textMsg);
				if (this.TextWidth >= this.TotalWidth)
				{
					this.txtplayer.Width = this.TextWidth + (double)this.OverWidth;
					this.txtplayer.TextAlignment = this.mTextAlign;
					if (this._dt대기 == null)
						this.InitProc();
					if (this._dt대기 == null)
						return;
					this._dt대기.Start();
				}
				else
				{
					this.txtplayer.Width = this.TotalWidth;
					this.txtplayer.TextAlignment = this.mTextAlign;
				}
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void DisplayVarColorText(List<ColorTextData> datas, int nTextAlign = 0)
		{
			try
			{
				this.ClearPlayer();
				this.txtplayer.Inlines.Clear();
				this.mvPoint = 0.0;
				this.txtplayer.SetValue(Canvas.LeftProperty, (object)this.mvPoint);
				string text = "";
				foreach (ColorTextData data in datas)
				{
					Run run = new Run();
					run.Text = data.DispText;
					run.Foreground = (Brush)ConvertUtils.ColorByRGBCode(data.RGBCode);
					text += run.Text;
					this.txtplayer.Inlines.Add((Inline)run);
				}
				this.TextWidth = this.MeasureTextWidth(this.txtplayer, text);
				if (this.TextWidth >= this.TotalWidth)
				{
					this.txtplayer.Width = this.TextWidth + (double)this.OverWidth;
					this.txtplayer.TextAlignment = TextAlignment.Center;
					if (this._dt대기 == null)
						this.InitProc();
					if (this._dt대기 == null)
						return;
					this._dt대기.Start();
				}
				else
					this.txtplayer.TextAlignment = (TextAlignment)nTextAlign;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void ClearPlayer()
		{
			try
			{
				this.txtplayer.Width = this.TotalWidth;
				this.txtplayer.Text = "";
				this.mvPoint = 0.0;
				this.txtplayer.SetValue(Canvas.LeftProperty, (object)this.mvPoint);
				if (this._dt이동 != null)
					this._dt이동.Stop();
				if (this._dt대기 != null)
					this._dt대기.Stop();
				GC.Collect();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void Display글자이동()
		{
			try
			{
				--this.mvPoint;
				if (this.mvPoint < this.fMovingPoint - (double)this.OverWidth)
				{
					if (this._dt이동 != null)
						this._dt이동.Stop();
					CommonUtils.WaitTime(500);
					this.mvPoint = 0.0;
					this.txtplayer.SetValue(Canvas.LeftProperty, (object)this.mvPoint);
					if (this._dt대기 == null)
						return;
					this._dt대기.Start();
				}
				else
					this.txtplayer.SetValue(Canvas.LeftProperty, (object)this.mvPoint);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}
	}
}
