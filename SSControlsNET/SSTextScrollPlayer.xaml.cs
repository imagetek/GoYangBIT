using SSCommonNET;
using System;
using System.Collections.Generic;
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
	/// Interaction logic for SSTextScrollPlayer.xaml
	/// </summary>
	public partial class SSTextScrollPlayer : UserControl, IComponentConnector
	{
		private Window _p;
		private bool _isLoaded;
		private DispatcherTimer _dt이동;
		private double TotalWidth;
		private double mvWidth;
		private int moveDistance = 1;
		public SSTextScrollPlayer()
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
				Console.WriteLine("### Unloaded : SSMediaPlayer ###");
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
				this._dt이동.Interval = TimeSpan.FromMilliseconds(100.0);
				this._dt이동.Tag = (object)0;
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
				this.Move이동Event();
				this._dt이동.Tag = (object)0;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				this._dt이동.Tag = (object)0;
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
				double height1 = newSize1.Height;
				Console.WriteLine("SSTextPlayer SizeChange {0}x{1}", (object)width1, (object)height1);
				Size newSize2;
				if (e.WidthChanged)
				{
					Canvas cvMain = this.cvMain;
					newSize2 = e.NewSize;
					double width2 = newSize2.Width;
					cvMain.Width = width2;
					SSDrawingText tplayer = this.tplayer;
					newSize2 = e.NewSize;
					double width3 = newSize2.Width;
					tplayer.Width = width3;
					this.TotalWidth = this.cvMain.Width;
				}
				if (!e.HeightChanged)
					return;
				Canvas cvMain1 = this.cvMain;
				newSize2 = e.NewSize;
				double height2 = newSize2.Height;
				cvMain1.Height = height2;
				SSDrawingText tplayer1 = this.tplayer;
				newSize2 = e.NewSize;
				double height3 = newSize2.Height;
				tplayer1.Height = height3;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void StartPlayer(string TXT, string F_NM, int F_SZ, string F_CMYK, bool MoveYN = false)
		{
			try
			{
				this.tplayer.FFontFamily = (FontFamily)this.FindResource((object)F_NM);
				this.tplayer.FFontSize = (double)F_SZ;
				this.tplayer.ForeColor = (Brush)new BrushConverter().ConvertFrom((object)F_CMYK);
				this.tplayer.Caption = TXT;
				if (!MoveYN || this._dt이동 == null)
					return;
				this._dt이동.Start();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void StopPlayer()
		{
			try
			{
				if (this._dt이동 != null)
					this._dt이동.Stop();
				this.tplayer.Caption = "";
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void Move이동Event()
		{
			try
			{
				this.mvWidth -= (double)this.moveDistance;
				if (this.mvWidth <= 0.0)
					this.mvWidth = this.TotalWidth;
				this.tplayer.SetValue(Canvas.LeftProperty, (object)this.mvWidth);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}
	}
}
