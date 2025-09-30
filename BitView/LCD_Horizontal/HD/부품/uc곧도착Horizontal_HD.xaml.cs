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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using SSCommonNET;
using SSControlsNET;
using SSData;

namespace BitView
{
	public partial class uc곧도착Horizontal_HD : UserControl
	{
		public uc곧도착Horizontal_HD()
		{
			InitializeComponent();

			this.FontFamily = (FontFamily)FindResource(DataManager.ConfigInfo.FONTS_NM);
			this.FontSize = DataManager.ConfigInfo.FONTS_SZ;
		}

		Window _p = null;
		public void SetParentWindow(Window p)
		{
			_p = p;
		}

		bool _isLoaded = false;
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				if (_isLoaded == false)
				{
					InitProc();
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			finally
			{
				_isLoaded = true;
			}
		}

		private void InitProc()
		{
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			try
			{
				DoFinal();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public void DoFinal()
		{
			try
			{
				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public void Clear정보()
		{
			try
			{
				txt곧도착.Text = "";

				//if (txt곧도착.Text.Equals(str곧도착old) == false)
				//{
				//	str곧도착old = txt곧도착.Text;					
				//	Txt_Changed();					
				//}

				if (oldArrivingSoonRoutes.Count > 0)
				{
					oldArrivingSoonRoutes.Clear();
					Txt_Changed();
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		//string str곧도착old = "";
		List<string> oldArrivingSoonRoutes = [];
		public void Refresh정보(List<BisArrivalInformation> items)
		{
			try
			{
				if (items != null && items.Count > 0)
				{
					List<string> routesToDisplay = items.Select(data => data.RouteNumber).ToList();
					
					string disp = string.Join(" , ", routesToDisplay);
					txt곧도착.Text = disp;
					txt곧도착.FontSize = CommonUtils.ReduceFontSizeIfNeeded(disp, txt곧도착);					

					if (oldArrivingSoonRoutes.Count != routesToDisplay.Count || oldArrivingSoonRoutes.Except(routesToDisplay).Any())
					{
						oldArrivingSoonRoutes.Clear();
						oldArrivingSoonRoutes.AddRange(routesToDisplay);
						Txt_Changed();
						Task.Run(() => { EventManager.PlayBusNoTTS(routesToDisplay); });						
					}

				}
				else
				{
					Clear정보();
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Txt_Changed()
		{
			//edgar dispatch removed
			//txt곧도착.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => DoArrivalSoon()));
			Task.Run(() => 
			{
				DoArrivalSoon();
			});
		}

		private static void DoArrivalSoon()
		{
			CommonUtils.WaitTime(50, true); //pjh 50 -> 500
			if (MainWindow.startUpFine)
			{
				//this is just for arriving soon box, but due to issue during cold weather we update all box
				//MainWindow.CaptureScreenVertical(new Rect(20, 160, 680, 70));   // 글자부분만 //edgar changed arriving soon capture
				MainWindow.CaptureScreenHorizontal(new Rect(596, 0, 72, 1280)); //take full box image
			}
				
		}
	}
}