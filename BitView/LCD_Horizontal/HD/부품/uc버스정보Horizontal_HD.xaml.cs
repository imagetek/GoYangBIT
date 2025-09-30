using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Reflection;
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
	public partial class uc버스정보Horizontal_HD : UserControl
	{
		public uc버스정보Horizontal_HD()
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

		System.Windows.Threading.DispatcherTimer _dt이동 = null;
		int busArrivalRefreshInterval = BITDataManager.BitENVConfig.ITEM.BusArrivalInfoRefreshInterval; //original value 8
		private void InitProc()
		{
			try
			{
				if (_dt이동 == null)
				{
					_dt이동 = new System.Windows.Threading.DispatcherTimer();
					_dt이동.Tick += _dt이동_Tick;
				}
				_dt이동.Interval = TimeSpan.FromSeconds(busArrivalRefreshInterval);	//20=ok,15=overflow발생 //5); //pjh 버스정보페이지 전환하는 시간간격. 최소한 곧도착(3초)과 버스정보(8초) 표시 시간합(11초)보다 커야 한다.
				_dt이동.Tag = 0;								//

				Clear도착정보();
				Refresh버스정보창(LCD화면Type.BIS정보미존재);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void _dt이동_Tick(object sender, EventArgs e)
		{
			try
			{
				if (Convert.ToInt32(_dt이동.Tag) == 1) return;
				_dt이동.Tag = 1;

				if (TotalPage수 > 0)
				{
					if (current화면.Equals(LCD화면Type.BIS정보) == true)  // 중복방지
						다음페이지Proc();
				}

				_dt이동.Tag = 0;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				_dt이동.Tag = 0;
			}
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

		int idxNo = 0;
		int items수 = 5;
		int TotalPage수 = 0;
		List<BisArrivalInformation> items정보 = new List<BisArrivalInformation>();

		public void Refresh도착정보(List<BisArrivalInformation> items)
		{
			try
			{
				items정보 ??= new List<BisArrivalInformation>();
				items정보.Clear();

				//idxNo = 0; //edgar removed this to keep showing  correct page when new data arrives, otherwise when new data arrives page 0 will be shown
				items정보.AddRange(items); //edgar

				if (items정보 != null && items정보.Count > 0)
				{
					Refresh버스정보창(LCD화면Type.BIS정보);

					bool 나머지YN = items정보.Count % items수 > 0;
					TotalPage수 = items정보.Count / items수 + (나머지YN == true ? 1 : 0);

					//Display도착정보(); // edgar, removed this to keep showing pages from dispatcher timer. otherwise when new data arrives current displayed page will be refreshed immediatelly, and not after timer

					//idxNo = 0; //edgar added for test

					if (TotalPage수 > 1)
					{
						_dt이동?.Start();
					}
					else
					{
						_dt이동?.Stop();
						idxNo = 0;
						Display도착정보();
					}
				}
				else
				{
					_dt이동?.Stop();
					idxNo = 0;
					TotalPage수 = 0;

					Clear도착정보();
					Refresh버스정보창(LCD화면Type.BIS정보미존재);

					item정보old = null;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		LCD화면Type current화면 = LCD화면Type.NONE;

		public void Refresh버스정보창(LCD화면Type item)
		{
			try
			{
				if (current화면.Equals(item) == true) return;	// 중복방지
				current화면 = item;

				string URL = "";
				string MSG = "";
				switch (item)
				{
					case LCD화면Type.BIS정보:
						if (sp정보.Visibility != Visibility.Visible)
						{
							txt메세지.Visibility = Visibility.Hidden;
							sp정보.Visibility = Visibility.Visible;
							//URL = string.Format(@"pack://application:,,,/SSResource;component/images/EPD/버스정보배경H.png");
						}
						break;

					case LCD화면Type.BIS정보미존재:
						if (sp정보.Visibility == Visibility.Visible) sp정보.Visibility = Visibility.Hidden;
						//URL = string.Format(@"pack://application:,,,/SSResource;component/images/EPD/버스정보배경H.png");
						MSG = "버스운행정보가 없습니다.";
						break;

					case LCD화면Type.긴급메세지:
						if (sp정보.Visibility == Visibility.Visible) sp정보.Visibility = Visibility.Hidden;
						//URL = string.Format(@"pack://application:,,,/SSResource;component/images/EPD/버스정보배경H.png");
						MSG = "긴급메세지입니다.";
						break;

					case LCD화면Type.네트워크오류:
						if (sp정보.Visibility == Visibility.Visible) sp정보.Visibility = Visibility.Hidden;
						//URL = string.Format(@"pack://application:,,,/SSResource;component/images/EPD/버스정보배경H.png");
						MSG = "시스템 점검중입니다.";
						break;

					case LCD화면Type.시스템에러:
						if (sp정보.Visibility == Visibility.Visible) sp정보.Visibility = Visibility.Hidden;
						//URL = string.Format(@"pack://application:,,,/SSResource;component/images/EPD/버스정보배경H.png");
						MSG = "시스템 에러입니다.";
						break;
				}

				if (URL.Equals("") == false)
				{
					Display버스정보배경(URL);
				}

				txt메세지.Text = MSG;
				txt메세지.Foreground = new SolidColorBrush(Colors.Black);
				if (MSG.Equals("") == false)
				{
					txt메세지.Visibility = Visibility.Visible;

					//edgar refresh bus arrival
					idxNo = 0;
					if ((bool)_dt이동?.IsEnabled)
					{
						_dt이동?.Stop();
					}

					item정보old = null;
					Txt_Changed(txt메세지);
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Display버스정보배경(string URL)
		{
			try
			{
				BitmapSource _bs배경 = new BitmapImage(new Uri(URL, UriKind.RelativeOrAbsolute));
				ImageBrush imgBrush = new ImageBrush(_bs배경);
				//mainGrid.Background = imgBrush.Clone();
				this.Background = imgBrush.Clone();

				CommonUtils.WaitTime(50, true);

				_bs배경.Freeze();
				_bs배경 = null;
				imgBrush.Freeze();
				imgBrush = null;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Clear도착정보()
		{
			try
			{
				foreach (UIElement ctrl in sp정보.Children)
				{
					uc도착정보HorizontalItem_HD item = ctrl as uc도착정보HorizontalItem_HD;
					if (item == null) continue;
					item.Clear도착정보();
					item.Visibility = Visibility.Hidden;
				}

				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		BisArrivalInformation item정보old = null;	// 비교용, 첫줄
		uc도착정보HorizontalItem_HD ctrl_old = null;     // 마지막 유효 obj
		readonly List<BisArrivalInformation> oldArrivalList = [];
		private void Display도착정보()
		{
			try
			{
				Clear도착정보();

				List<bool> changed = [];

				int S_NO = idxNo * items수;
				int E_NO = (idxNo + 1) * items수;

				for (int i = 1; i <= items수; i++)
				{
					uc도착정보HorizontalItem_HD ctrl = sp정보.FindName(string.Format("_u정보{0}", i)) as uc도착정보HorizontalItem_HD;
					if (ctrl == null) continue;
					int SEQ_NO = S_NO + i - 1;
					if (SEQ_NO >= items정보.Count) continue;
					BisArrivalInformation item정보 = items정보[SEQ_NO];
					if (item정보 != null)
					{
						ctrl.Refresh도착정보(item정보);
						ctrl.Visibility = Visibility.Visible;
						//ctrl_old = ctrl;	//pjh last save

						////if (i == 1)
						////{
						//	if ((item정보old == null)
						//		|| (item정보.RouteNumber.Equals(item정보old.RouteNumber) == false)								
						//		|| ArrivalMinutesAreNotSame(item정보.EstimatedTimeOfArrival, item정보old.EstimatedTimeOfArrival))
						//	{
						//		item정보old = item정보;  // 해당페이지의 첫줄 저장
						//	changed.Add(true);
						//	}
						//	//else
						//	//changed.Add(false);
						////}
					}
				}
                //if (changed.Any(c => c))
                //{
                //	Ctrl_Changed(ctrl_old);
                //}
                if (TotalPage수 == 1)
                {    
					CompareArrivalItemsAndSendToEpdIfNeeded(oldArrivalList, items정보);
					oldArrivalList.Clear();
					oldArrivalList.AddRange(items정보);
				} 
				else
				{
					DoSomething();
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void CompareArrivalItemsAndSendToEpdIfNeeded(List<BisArrivalInformation> oldArrivalList, List<BisArrivalInformation> items정보)
		{			
            for (int i = 0; i < oldArrivalList.Count; i++)
            {
				var oldItem = oldArrivalList[i];
				var newItem = items정보[i];
                if (ArrivalMinutesAreNotSame(newItem.EstimatedTimeOfArrival, oldItem.EstimatedTimeOfArrival) || oldItem.StopId != newItem.StopId || !newItem.StopName.Equals(oldItem.StopName))
                {
					DoSomething();
					break;					
				}
            }
        }

		private bool ArrivalMinutesAreNotSame(int newTime, int oldTime)
		{
			return Math.Ceiling((decimal)newTime / 60) != Math.Ceiling((decimal)oldTime / 60);
		}

		private void 다음페이지Proc()
		{
			
			if (idxNo + 1 > TotalPage수)
			{
				idxNo = 0;
			}
			Display도착정보();
			idxNo++;
		}

		private void Txt_Changed(TextBlock txt메세지)
		{
			//edgar dispatch removed
			//txt메세지.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => DoSomething()));
			Task.Run(() => { DoSomething(); });
		}
		private void Ctrl_Changed(uc도착정보HorizontalItem_HD ctrl)  //ctrl_old
		{
			ctrl.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => DoSomething()));
		}
		private void DoSomething()
		{
			//This will get called after the UI is complete rendering
			CommonUtils.WaitTime(50, true);

			if (MainWindow.startUpFine)
			{
				//MainWindow.CaptureScreenVertical(new Rect(5, 315, 705, 580));   // 글자부분만 //edgar changed bus info capture
				
				MainWindow.CaptureScreenHorizontal(new Rect(0, 0, 596, 776)); //take all box image
			}
				
		}
	}
}
