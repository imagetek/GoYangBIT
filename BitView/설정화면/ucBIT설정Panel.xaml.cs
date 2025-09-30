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
	/// <summary>
	/// ucENV2Panel.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ucBIT설정Panel : UserControl
	{
		public ucBIT설정Panel()
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
					//InitProc();

					//Load기본값();

					//Load환경설정();
				}

				//if (_dtRequest != null) _dtRequest.Start();
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			finally
			{
				_isLoaded = true;
			}
		}

		System.Windows.Threading.DispatcherTimer _dt기능 = null;
		private void InitProc()
		{
			try
			{			
				if (_dt기능 == null)
				{
					_dt기능 = new System.Windows.Threading.DispatcherTimer();
					_dt기능.Tick += _dt기능_Tick;
				}
				_dt기능.Interval = TimeSpan.FromSeconds(60);
				_dt기능.Tag = 0;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			finally
			{
				this.IsEnabled = true;
			}
		}

		public void Initialize웹캠()
		{
			try
			{
				InitProc();

				Load환경설정();

				Load자동실행();

				if (_dt기능!= null) _dt기능.Start();
			}
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
		}

		private void Load환경설정()
		{
			try
			{
				
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Load자동실행()
		{
			try
			{
				
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
		
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			try
			{
				//DoFinal();
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
				if (_dt기능 != null)
				{
					_dt기능.Stop();
					_dt기능.Tick -= _dt기능_Tick;
					_dt기능 = null;
				}
				
				GC.Collect();
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void _dt기능_Tick(object sender, EventArgs e)
		{
			try
			{
				if (Convert.ToInt32(_dt기능.Tag) == 1) return;
				_dt기능.Tag = 1;
							

				_dt기능.Tag = 0;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				_dt기능.Tag = 0;
			}
		}

		private void tabMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				//TabControl tab = sender as TabControl;
				//if (tab == null) return;
				//TabItem item = tab.SelectedItem as TabItem;
				//if (item == null) return;
				//if (item.Tag == null) return;

				//int tagNo = Convert.ToInt32(item.Tag);
				//switch (tagNo)
				//{
				//	case 1://메인화면
				//	case 3://프로그램
				//	case 4: //BIT설정
				//	case 5://LED설정
				//		break;
				//	case 2: //화면
				//		_u화면.Refresh화면();
				//		break;
				//}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

	}
}
