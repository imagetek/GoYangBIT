using SSCommonNET;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SSControlsNET
{
	/// <summary>
	/// Interaction logic for SSLEDTextPlayer.xaml
	/// </summary>
	public partial class SSLEDTextPlayer : UserControl
	{
		private Window _p;
		private bool _isLoaded;

		public SSLEDTextPlayer() => this.InitializeComponent();

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

		public void StartPlayer(string TXT, string F_NM, int F_SZ, string F_CMYK)
		{
			try
			{
				this.tplayer.FFontFamily = (FontFamily)this.FindResource((object)F_NM);
				this.tplayer.FFontSize = (double)F_SZ;
				this.tplayer.ForeColor = (Brush)new BrushConverter().ConvertFrom((object)F_CMYK);
				this.tplayer.Caption = TXT;
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
				this.tplayer.Caption = "";
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}
	}
}
