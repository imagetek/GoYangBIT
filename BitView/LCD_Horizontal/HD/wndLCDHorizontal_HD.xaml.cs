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
using System.Windows.Shapes;

using SSCommonNET;
using SSControlsNET;
using SSData;
using SSData.MQTT;

namespace BitView
{
	/// <summary>
	/// wndLCDVertical_HD.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class wndLCDHorizontal_HD : Window
	{
		public wndLCDHorizontal_HD()
		{
			InitializeComponent();
		}

		Window _p = null;
		public void SetParentWindow(Window p)
		{
			_p = p;
		}

		bool _isLoaded = false;
		bool isMovable = false;
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				this.ResizeMode = ResizeMode.NoResize;
				this.WindowStyle = WindowStyle.None;
				if (_isLoaded == false)
				{
					InitProc();
					Load기본값();
					Load설정값();
				}
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

		#region 윈도우창 기본 코드 

		//public override void OnApplyTemplate()
		//{
		//    try
		//    {
		//        Grid gridMain = GetTemplateChild("PART_TITLEBAR") as Grid;
		//        if (gridMain != null)
		//        {
		//            gridMain.MouseDown += Grid_MouseLeftButtonDown;
		//            gridMain.MouseMove += Grid_MouseMove;
		//            gridMain.MouseUp += Grid_MouseLeftButtonUp;
		//        }

		//        Button btnClose = GetTemplateChild("btnClose") as Button;
		//        if (btnClose != null)
		//        {
		//            btnClose.Click += btnMainClose_Click;
		//        }

		//        Button btnMainClose = GetTemplateChild("btnMainClose") as Button;
		//        if (btnMainClose != null)
		//        {
		//            btnMainClose.Click += btnMainClose_Click;
		//        }
		//        //  
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
		//    }
		//    base.OnApplyTemplate();
		//}

		//private void btnMainClose_Click(object sender, RoutedEventArgs e)
		//{
		//    this.Close();
		//}


		#region 단축키 이벤트

		protected override void OnKeyDown(KeyEventArgs e)
		{
			try
			{
				//Console.WriteLine("## override OnKeyDown : 환경설정 ##");
				if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt || e.SystemKey == Key.F4)
				{
					e.Handled = true;
					return;
				}
				//switch (e.Key)
				//{
				//    case Key.F5:

				//        //btn닫기_Click(btn닫기, null);
				//        break;
				//}
				if (e.SystemKey == Key.LeftCtrl && e.Key == Key.F5)
				{
					this.WindowState = WindowState.Minimized;
				}
				base.OnKeyDown(e);
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		#endregion


		//#region 마우스창 이동 기능 

		////마우스로 창 이동 ==========================
		//private Point mousePoint;
		//private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		//{
		//    try
		//    {
		//        //Console.WriteLine("## Grid_MouseLeftButtonDown : 환경설정 ##");
		//        Point pnt = e.GetPosition(_u상단);
		//        mousePoint = new Point(pnt.X, pnt.Y);

		//        ((Grid)sender).CaptureMouse();
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//    }
		//}

		//System.Drawing.Rectangle? _prevExtRect = null;
		//private void Grid_MouseMove(object sender, MouseEventArgs e)
		//{
		//    try
		//    {
		//        if (_prevExtRect != null) return;
		//        //Console.WriteLine("## Grid_MouseMove : 환경설정 ##");
		//        if (((UserControl)sender).IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
		//        {
		//            Point pnt = e.GetPosition(this);
		//            this.Left = this.Left - (mousePoint.X - pnt.X);
		//            this.Top = this.Top - (mousePoint.Y - pnt.Y);
		//        }
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//    }
		//}

		//private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		//{
		//    try
		//    {
		//        //Console.WriteLine("## Grid_MouseLeftButtonUp : 환경설정 ##");
		//        ((UserControl)sender).ReleaseMouseCapture();
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//    }
		//}

		//#endregion

		//#region 윈도우 사이즈 변경

		//Point _curGridSP;
		//Size _curSize;
		//private void gridGrip_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		//{
		//	try
		//	{
		//		if (this.WindowState != System.Windows.WindowState.Normal) return;

		//		if (e.ButtonState == MouseButtonState.Pressed)
		//		{
		//			_curGridSP = e.GetPosition(this);
		//			_curSize = new Size(this.Width, this.Height);

		//			gridGrip.CaptureMouse();
		//		}
		//	}
		//	catch (Exception ee)
		//	{
		//		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//	}
		//}

		//private void gridGrip_MouseMove(object sender, MouseEventArgs e)
		//{
		//	try
		//	{
		//		if (this.WindowState != System.Windows.WindowState.Normal || _prevExtRect != null) return;

		//		if (e.LeftButton == MouseButtonState.Pressed)
		//		{

		//			Vector gap = e.GetPosition(this) - _curGridSP;

		//			double w = _curSize.Width + gap.X;
		//			if (w < this.MinWidth) w = this.MinWidth;
		//			double h = _curSize.Height + gap.Y;
		//			if (h < this.MinHeight) h = this.MinHeight;

		//			this.Width = w;
		//			this.Height = h;
		//		}
		//	}
		//	catch (Exception ee)
		//	{
		//		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//	}
		//}

		//private void gridGrip_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		//{
		//	try
		//	{
		//		if (this.WindowState != System.Windows.WindowState.Normal) return;

		//		gridGrip.ReleaseMouseCapture();

		//		//Console.WriteLine("{0}X{1}", this.ActualWidth, this.ActualHeight);
		//		//DxMapper.DxMapperManager.SetSizeChangedEvent();
		//	}
		//	catch (Exception ee)
		//	{
		//		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//	}
		//}

		//#endregion

		#endregion

		private void Window_Closed(object sender, EventArgs e)
		{
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
				if (_w숫자 != null) _w숫자.Close();

				if (_dt시간 != null)
				{
					_dt시간.Stop();
					_dt시간.Tick -= _dt시간_Tick;
					_dt시간 = null;
				}

				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		System.Windows.Threading.DispatcherTimer _dt시간 = null;

		private void InitProc()
		{
			try
			{
				if (_dt시간 == null)
				{
					_dt시간 = new System.Windows.Threading.DispatcherTimer();
					_dt시간.Interval = TimeSpan.FromSeconds(1); //pjh 큐잉하는 시간
					_dt시간.Tick += _dt시간_Tick;
					_dt시간.Tag = 0;
				}
				_dt시간.Start();
				_dt시간_Tick(null, null);
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
		private void Load기본값()
		{
		}
		private void Load설정값()
		{
		}

		#region 하단에 비밀숫자입력부분
		wnd숫자입력창 _w숫자 = null;
		private void _u하단_On환경설정Event()
		{
			try
			{
				if (this.WindowState != WindowState.Minimized) this.WindowState = WindowState.Minimized;
				//if (_w숫자 == null)
				//{
				//    _w숫자 = new wnd숫자입력창();
				//    _w숫자.Closed += _w숫자_Closed;
				//    _w숫자.On번호일치Event += _w숫자_On번호일치Event;
				//}
				////_w숫자.SetParentWindow(_p);
				//_w숫자.Owner = this.Owner;
				//_w숫자.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				//_w숫자.Topmost = true;
				//_w숫자.Show();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
		private void _w숫자_On번호일치Event(bool matchYN)
		{
			try
			{
				if (_w숫자 != null) _w숫자.Close();

				if (matchYN == true)
				{
					this.WindowState = WindowState.Minimized;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
		private void _w숫자_Closed(object sender, EventArgs e)
		{
			try
			{
				if (_w숫자 != null)
				{
					_w숫자.Closed -= _w숫자_Closed;
					_w숫자.On번호일치Event -= _w숫자_On번호일치Event;
					_w숫자 = null;
				}
				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
		#endregion


		private void _dt시간_Tick(object sender, EventArgs e)
		{
			EPD_DisplayStartup();
		}

		static int 전송순서 = 0, startDelay = 0;

		private void EPD_DisplayStartup()
		{
			switch (전송순서)
			{
				case 0:
					if (++startDelay > 2)   //0 -> 20sec, for rPi too slow appl startup
						전송순서++;
					break;
				case 1:
					MainWindow.CaptureScreenHorizontal(new Rect(0, 776, 596, 504)); // 하단 //edgar changed promotion and air capture for horizontal //rotate 90 => 0, 776, 595, 504 //original => 776, 125, 504, 595
					전송순서++;
					break;
				case 2:
					MainWindow.CaptureScreenHorizontal(new Rect(596, 0, 72, 1280));  // 곧도착 전체 //edgar changed arriving soon capture for horizontal //rotate 90 => 595, 0, 70, 1280 //original => 0, 55, 1280, 70
					전송순서++;
					break;
				case 3:
					MainWindow.CaptureScreenHorizontal(new Rect(0, 0, 596, 776));  // 버스정보 전체 //edgar changed bus info capture for horizontal //rotate 90 => 0, 0, 595, 776 //original => 0, 125, 776, 595
					전송순서++;
					break;
				case 4: 
					MainWindow.CaptureScreenHorizontal(new Rect(664, 0, 56, 1280));    // 상단화면 //edgar changed header capture for horizontal //rotate 90 => 665, 0, 55, 1280 //original => 0, 0, 1280, 55
					MainWindow.startUpFine = true;
					전송순서++;
					break;
				default:
					if (++startDelay > 60 * 60) //pjh 60분 주기로 다시한번 전체 그림
					{
						startDelay = 0;
						전송순서 = 0;
					}
					break;
			}
		}

		public void Refresh도착정보(List<BisArrivalInformation> datas)
		{
			try
			{
				if (datas != null && datas.Count > 0)
				{
					List<BisArrivalInformation> items곧도착 = BITDataManager.Get곧도착예정정보(datas);
					_u도착.Refresh정보(items곧도착);

					List<BisArrivalInformation> items = BITDataManager.Get도착정보목록(datas);
					_u정보.Refresh도착정보(items);
				}
				else
				{
					_u도착.Refresh정보(new List<BisArrivalInformation>());
					_u정보.Refresh도착정보(new List<BisArrivalInformation>());
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public void Refresh날씨정보(AirQualityData item)
		{
			try
			{
				_u하단.Refresh날씨정보(item);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public void Refresh공지사항(List<NoticeData> items)
		{
			try
			{
			//	_u하단.Refresh공지사항(items);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (!isMovable)
			{
				this.WindowStyle = WindowStyle.SingleBorderWindow;
				this.ResizeMode = ResizeMode.CanMinimize;
				isMovable = true;
			} 
			else
			{
				this.WindowStyle = WindowStyle.None;
				this.ResizeMode = ResizeMode.NoResize;
				isMovable = false;
			}
			
		}

		public void Refresh네트워크연결해제()
		{
			try
			{
				_u도착.Clear정보();
				_u정보.Refresh버스정보창(LCD화면Type.네트워크오류);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}				
	}
}
