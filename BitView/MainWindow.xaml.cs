using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Point = System.Windows.Point;
using Size = System.Windows.Size;
using Rectangle = System.Drawing.Rectangle;
using Color = System.Drawing.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

using SSCommonNET;
using SSControlsNET;
using SSData;
using SSMemory;
using System.Configuration;
using BitView;
using System.Diagnostics;
using SSData.Utils;
using System.Threading;
using SSData.GwachonAPI;
using SSData.DashboardAPI;

namespace BitView
{
	/// <summary>
	/// MainWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			this.IsEnabled = false;
		}

		public bool ResultDialog { get; set; }

		bool _isLoaded = false;

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			System.Windows.Threading.DispatcherTimer _dt초기화 = new();
			try
			{
				//pjh txt제목.Text = string.Format("BITViewer v.{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString());
				txt제목.Text = string.Format("BITViewer V {0}  {1}", 1.01, "25-07-22");

				if (_isLoaded == false)
				{
					InitProc();
				}

				_dt초기화.Interval = TimeSpan.FromSeconds(5);
				_dt초기화.Tick += delegate (object ds, EventArgs de)
				{
					_dt초기화.Stop();

					//CheckAgent실행();

					_dt한시간간격?.Start();
					_dt십분간격?.Start();
					_dt일초간격?.Start();

					if (DataManager.DebugYN == true)
					{
						_dt한시간간격_Tick(_dt한시간간격, null);
						_dt십분간격_Tick(_dt십분간격, null);
						_dt일초간격_Tick(_dt일초간격, null);
					}
				};
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
			}
			finally
			{
				if (wnd초기화면.SplashWindow != null)
				{
					wnd초기화면.SplashWindow.CloseSplashScreen();
					CommonUtils.WaitTime(50, true);
				}
				_dt초기화.Start();

				_isLoaded = true;
				this.IsEnabled = true;
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{				
				DoFinal();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
			}
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			try
			{
				MsgDlgManager.ShowIntervalInformationDlg("알림", "프로그램이 종료중입니다.\r\n잠시만 기다려 주세요.");
				CommonUtils.WaitTime(300, true);
				
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
			}
		}

		//Alt + F4 방지
		protected override void OnKeyDown(KeyEventArgs e)
		{
			try
			{
				if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt || e.SystemKey == Key.F4)
				{
					e.Handled = true;
					return;
				}

				Console.WriteLine("MainWindows KeyDown {0}", e.Key);
				base.OnKeyDown(e);
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
			}
		}

		#region 최대화 최소화
		private void ContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			try
			{
				btnMaximize_Click(null, null);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btnMainClose_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void btnMinimize_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				WindowState = System.Windows.WindowState.Minimized;
				//if (_w가로형 != null) _w가로형.WindowState = WindowState.Normal;
				//this.Hide();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btnMaximize_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.WindowState == System.Windows.WindowState.Normal)
				{
					this.WindowState = System.Windows.WindowState.Maximized;
					icNormal.Visibility = System.Windows.Visibility.Collapsed;
					icMaximize.Visibility = System.Windows.Visibility.Visible;
				}
				else if (this.WindowState == System.Windows.WindowState.Maximized)
				{
					this.WindowState = System.Windows.WindowState.Normal;
					icNormal.Visibility = System.Windows.Visibility.Visible;
					icMaximize.Visibility = System.Windows.Visibility.Collapsed;
				}
				CommonUtils.WaitTime(10, true);
				// DxMapper.DxMapperManager.SetSizeChangedEvent();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
			}
		}

		#endregion

		#region 마우스창 이동 기능 

		//마우스로 창 이동 ==========================
		private Point mousePoint;
		private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				Point pnt = e.GetPosition(this);
				mousePoint = new Point(pnt.X, pnt.Y);

				((Grid)sender).CaptureMouse();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Grid_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				if (_prevExtRect != null) return;

				if (((Grid)sender).IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
				{
					Point pnt = e.GetPosition(this);
					this.Left = this.Left - (mousePoint.X - pnt.X);
					this.Top = this.Top - (mousePoint.Y - pnt.Y);
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			try
			{
				((Grid)sender).ReleaseMouseCapture();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		Point _curGridSP;
		Size _curSize;

		private void gridGrip_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				if (this.WindowState != System.Windows.WindowState.Normal) return;

				if (e.ButtonState == MouseButtonState.Pressed)
				{
					_curGridSP = e.GetPosition(this);
					_curSize = new Size(this.Width, this.Height);

					gridGrip.CaptureMouse();
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
			}

		}

		private void gridGrip_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				if (this.WindowState != System.Windows.WindowState.Normal || _prevExtRect != null) return;

				if (e.LeftButton == MouseButtonState.Pressed)
				{

					Vector gap = e.GetPosition(this) - _curGridSP;

					double w = _curSize.Width + gap.X;
					if (w < this.MinWidth) w = this.MinWidth;
					double h = _curSize.Height + gap.Y;
					if (h < this.MinHeight) h = this.MinHeight;

					this.Width = w;
					this.Height = h;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
			}

		}

		private void gridGrip_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			try
			{
				if (this.WindowState != System.Windows.WindowState.Normal) return;

				gridGrip.ReleaseMouseCapture();

				//DxMapper.DxMapperManager.SetSizeChangedEvent();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
			}
		}

		System.Drawing.Rectangle? _prevExtRect = null;


		#endregion

		private void DoFinal()
		{
			try
			{
				//이벤트제거
				try
				{
					mShare.ReceiveHandler -= MShare_ReceiveHandler;

					EventManager.OnRefreshDisplayEvent -= EventManager_OnRefreshDisplayEvent;
					EventManager.OnPlayBusNoTTSEvent -= EventManager_OnPlayBusNoTTSEvent;
					EventManager.OnPlayMessageTTSEvent -= EventManager_OnPlayMessageTTSEvent;
				}
				catch (Exception eex)
				{
					Console.WriteLine(eex.Message);
				}

				if (tts != null)
				{
					tts.DoFinal();
					tts = null;
				}

				ClearPAJUBIS센터();

				_uENV2.DoFinal();

				//_uWebcam.DoFinal();

				//if (_dtCursor != null)
				//{
				//	_dtCursor.Stop();
				//	_dtCursor.Tick -= _dtCursor_Tick;
				//	_dtCursor = null;
				//}

				if (_dt일초간격 != null)
				{
					_dt일초간격.Stop();
					_dt일초간격.Tick -= _dt일초간격_Tick;
					_dt일초간격 = null;
				}

				if (_dt십분간격 != null)
				{
					_dt십분간격.Stop();
					_dt십분간격.Tick -= _dt십분간격_Tick;
					_dt십분간격 = null;
				}

				if (_dt한시간간격 != null)
				{
					_dt한시간간격.Stop();
					_dt한시간간격.Tick -= _dt한시간간격_Tick;
					_dt한시간간격 = null;
				}

				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void InitProc()
		{
			try
			{				
				btn1.Visibility = Visibility.Collapsed;
				if (DataManager.DebugYN == true)
				{
					btn1.Visibility = Visibility.Visible;
				}

				wnd초기화면.SplashWindow.SetProgress(10, "관련 프로그램을 초기화중입니다.");
				grd테스트.Visibility = DataManager.DebugYN == true ? Visibility.Visible : Visibility.Collapsed;

				wnd초기화면.SplashWindow.SetProgress(20, "프로그램을 초기화중입니다.");
				BITDataManager.Initialize();

				wnd초기화면.SplashWindow.SetProgress(30, "임시폴더를 정리중입니다.");
				CommonProc.InitDirectory정리();

				wnd초기화면.SplashWindow.SetProgress(40, "임시파일을 정리중입니다.");
				Clear파일정리();

				wnd초기화면.SplashWindow.SetProgress(50, "기초데이터를 구성중입니다.");
				CodeManager.Initialize();
				//InitCursor(); //edgar is this needed?

				wnd초기화면.SplashWindow.SetProgress(65, "현재시간을 설정중입니다.");
				Init타이머();

				wnd초기화면.SplashWindow.SetProgress(95, "초기화를 완료중입니다.");
				InitTTSProc();

				InitializeBITViewer();

				ScreenSaverManager.NotUse절전대기모드();

				mShare.ReceiveHandler += MShare_ReceiveHandler;
				//mShareSound.ReceiveHandler += MShareSound_ReceiveHandler;
				//ShareMemoryManager.OnShareMemoryEvent += ShareMemoryManager_OnShareMemoryEvent;

				EventManager.OnRefreshDisplayEvent += EventManager_OnRefreshDisplayEvent;
				EventManager.OnPlayBusNoTTSEvent += EventManager_OnPlayBusNoTTSEvent;
				EventManager.OnPlayMessageTTSEvent += EventManager_OnPlayMessageTTSEvent;


				Task.Run(async () =>
				{
					await HttpService.LoginUser();					
				});
				
				//if (DataManager.DebugYN == true)
				//{
				//    BITDataManager.BitConfig.LOGSAVE_PERCENT = 50;
				//    BITDataManager.BitConfig.LOGSAVE_DAY = 15;
				//}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		#region ShareMemory

		//   private void ShareMemoryManager_OnShareMemoryEvent(int GBN, string ShareMsg)
		//   {
		//       try
		//       {
		//           byte[] btDatas = Encoding.UTF8.GetBytes(ShareMsg);
		//           if (btDatas != null && btDatas.Length > 0)
		//           {
		//               프로그램Type item = (프로그램Type)GBN;
		//               switch (item)
		//               {
		//                   case 프로그램Type.AGENT:
		//                       mShareAgent.Write(btDatas);
		//                       TraceManager.AddInfoLog(string.Format("[IPC송신] BIT→AGENT : {0}", ShareMsg));
		//                       break;

		//	case 프로그램Type.WEBCAM:
		//		mShareWebcam.Write(btDatas);
		//		TraceManager.AddInfoLog(string.Format("[IPC송신] BIT→WEBCAM : {0}", ShareMsg));
		//		break;
		//}
		//           }
		//       }
		//       catch (Exception ee)
		//       {
		//           TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//           System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//       }
		//   }

		SharedMemory mShare = new SharedMemory("BIT", 4096);

		//SharedMemory mShareAgent = new SharedMemory("BITAGENT", 4096);
		private void MShare_ReceiveHandler(byte[] item)
		{
			try
			{
				string msg = Encoding.UTF8.GetString(item);
				msg = msg.Replace('\0', ' ');
				msg = msg.Trim();
				TraceManager.AddInfoLog(string.Format("[IPC수신] AGENT→BIT :{0}", msg));
				switch (msg)
				{
					case "EXIT":
						System.Windows.Forms.Application.ExitThread();
						System.Environment.Exit(0);
						break;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		SharedMemory mShareWebcam = new SharedMemory("BITWEBCAM", 4096);


		//SharedMemory mShareSound = new SharedMemory("BITSOUND", 4096);
		//private void MShareSound_ReceiveHandler(byte[] item)
		//{
		//    try
		//    {
		//        string msg = ASCIIEncoding.UTF8.GetString(item);
		//        msg = msg.Replace('\0', ' ');
		//        msg = msg.Trim();
		//        //TraceManager.AddInfoLog(string.Format("[IPC수신] SOUND→BIT :{0}", msg));
		//        switch (msg)
		//        {
		//            case "END":
		//                break;
		//        }
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//    }
		//}


		#endregion

		#region 커서관련

		//bool _cursorShow = true;
		//System.Windows.Threading.DispatcherTimer _dtCursor = null;
		/// <summary>
		/// 마우스 시간에 따른 표시 여부 확인
		/// </summary>
		//private void InitCursor()
		//{
		//	try
		//	{
		//		if (_dtCursor == null)
		//		{
		//			_dtCursor = new System.Windows.Threading.DispatcherTimer();
		//			_dtCursor.Tick += _dtCursor_Tick;
		//		}
		//		_dtCursor.Interval = TimeSpan.FromMilliseconds(100);
		//		_dtCursor.Tag = 0;
		//		_dtCursor.Start();
		//	}
		//	catch (Exception ee)
		//	{
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//	}
		//}

		//private void _dtCursor_Tick(object sender, EventArgs e)
		//{
		//	try
		//	{
		//		if (Convert.ToInt32(_dtCursor.Tag) == 1) return;
		//		_dtCursor.Tag = 1;

		//		if ((CommonUtils.GetIdleMilliSecond() > 2000) && _cursorShow)
		//		{
		//			Cursor = Cursors.None;
		//			_cursorShow = false;
		//		}
		//		else if ((CommonUtils.GetIdleMilliSecond() <= 2000) && !_cursorShow)
		//		{
		//			Cursor = Cursors.Arrow;
		//			_cursorShow = true;
		//		}

		//		_dtCursor.Tag = 0;
		//	}
		//	catch (Exception ee)
		//	{
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		_dtCursor.Tag = 0;
		//	}
		//}


		#endregion

		System.Windows.Threading.DispatcherTimer _dt한시간간격 = null;
		System.Windows.Threading.DispatcherTimer _dt십분간격 = null;
		System.Windows.Threading.DispatcherTimer _dt일초간격 = null;

		int n상태보고주기 = 0;
		int n영상정보주기 = 0;
		int n화면캡쳐주기 = 0;

		private void Init타이머()
		{
			try
			{
				if (_dt한시간간격 == null)
				{
					_dt한시간간격 = new System.Windows.Threading.DispatcherTimer();
					_dt한시간간격.Tick += _dt한시간간격_Tick;
				}
				_dt한시간간격.Interval = TimeSpan.FromHours(1);
				_dt한시간간격.Tag = 0;

				if (_dt십분간격 == null)
				{
					_dt십분간격 = new System.Windows.Threading.DispatcherTimer();
					_dt십분간격.Tick += _dt십분간격_Tick;
				}
				_dt십분간격.Interval = TimeSpan.FromMinutes(9);
				_dt십분간격.Tag = 0;

				if (_dt일초간격 == null)
				{
					_dt일초간격 = new System.Windows.Threading.DispatcherTimer();
					_dt일초간격.Tick += _dt일초간격_Tick;
				}
				_dt일초간격.Interval = TimeSpan.FromSeconds(1);
				_dt일초간격.Tag = 0;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private static void CheckAgent실행()
		{
			try
			{
				if (DataManager.DebugYN == false)
				{
					System.Diagnostics.Process[] processesByName = System.Diagnostics.Process.GetProcessesByName("SSAgent");
					if (processesByName == null || processesByName.Length == 0)
					{
						EventManager.DisplayLog(Log4Level.Info, "Agent 미동작중 ▷ Agent실행");

						string AEXE = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SSAgent.exe");
						if (File.Exists(AEXE) == true)
						{
							System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(AEXE);
							System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo);
							//process.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
							System.Threading.Thread.Sleep(3000);
							process.Dispose();
						}
					}
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void _dt한시간간격_Tick(object sender, EventArgs e)
		{
			try
			{
				if (Convert.ToInt32(_dt한시간간격.Tag) == 1) return;
				_dt한시간간격?.Stop();
				_dt한시간간격.Tag = 1;

				var config = BITDataManager.BitSystem;
				if (config == null) return;

				//로그정리
				Clear로그폴더();

				Clear파일정리();

				//화면밝기조정
				//Apply화면밝기();

				//볼륨조정
				Apply볼륨();

				//시간동기화 20220729
				if (DataManager.ConfigInfo.USE_SYNCTIME == true)
				{
					NTPServerUtils.SynchronizieNTPServer(DataManager.ConfigInfo.SYNC_URL);
				}

				_dt한시간간격.Tag = 0;
			}
			catch (Exception ee)
			{
				_dt한시간간격.Tag = 0;
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			finally
			{
				_dt한시간간격?.Start();
			}
		}


		private void _dt십분간격_Tick(object sender, EventArgs e)
		{
			try
			{
				if (Convert.ToInt32(_dt십분간격.Tag) == 1) return;
				_dt십분간격.Tag = 1;

				Check화면관리();

				_dt십분간격.Tag = 0;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				_dt십분간격.Tag = 0;
			}
		}

		private void _dt일초간격_Tick(object sender, EventArgs e)
		{
			try
			{
				if (Convert.ToInt32(_dt일초간격.Tag) == 1) return;
				_dt일초간격.Tag = 1;

				n화면캡쳐주기++;
				n영상정보주기++;
				n상태보고주기++;

				BIT_ENV_SETTING config = BITDataManager.BitENVConfig.ITEM;
				if (config != null)
				{		
					if (config.StateSendPeriod > 0 && n상태보고주기 >= config.StateSendPeriod)
					{
						//mBIS?.Send단말기상태정보();
						n상태보고주기 = 0;
					}					
				}

				//Clear도착예정정보();

				_dt일초간격.Tag = 0;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				_dt일초간격.Tag = 0;
			}
		}

		#region 환경설정,BIT설정

		wnd환경설정 _w환경설정 = null;
		private void Show환경설정()
		{
			try
			{
				if (_w환경설정 != null) _w환경설정.Close();

				if (_w환경설정 == null)
				{
					_w환경설정 = new wnd환경설정();
					_w환경설정.On설정변경Event += _w환경설정_On설정변경Event;
					_w환경설정.Closed += _w환경설정_Closed;
				}
				_w환경설정.SetParentWindow(this);
				_w환경설정.Owner = Application.Current.MainWindow;
				_w환경설정.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				_w환경설정.Show();
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void _w환경설정_On설정변경Event()
		{
			try
			{
				EventManager.DisplayLog(Log4Level.Info, "설정값이 변경되었습니다.");
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void _w환경설정_Closed(object sender, EventArgs e)
		{
			try
			{
				if (_w환경설정 != null)
				{
					_w환경설정.Closed -= _w환경설정_Closed;
					_w환경설정.On설정변경Event -= _w환경설정_On설정변경Event;
					_w환경설정 = null;
				}
				GC.Collect();
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		wndBIT설정 _wBIT설정 = null;
		private void ShowBIT설정()
		{
			try
			{
				if (_wBIT설정 != null) _wBIT설정.Close();

				if (_wBIT설정 == null)
				{
					_wBIT설정 = new wndBIT설정();
					_wBIT설정.On설정변경Event += _wBIT설정_On설정변경Event;
					_wBIT설정.Closed += _wBIT설정_Closed;
				}
				_wBIT설정.SetParentWindow(this);
				_wBIT설정.Owner = Application.Current.MainWindow;
				_wBIT설정.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				_wBIT설정.Show();
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void _wBIT설정_Closed(object sender, EventArgs e)
		{
			try
			{
				if (_wBIT설정 != null)
				{
					_wBIT설정.Closed -= _wBIT설정_Closed;
					_wBIT설정.On설정변경Event -= _wBIT설정_On설정변경Event;
					_wBIT설정 = null;
				}
				GC.Collect();
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void _wBIT설정_On설정변경Event()
		{
			try
			{
				EventManager.DisplayLog(Log4Level.Info, "BIT설정값이 변경되어 프로그램을 종료합니다.");

				DialogResult = true;
				System.Windows.Forms.Application.ExitThread();
				System.Environment.Exit(0);


				//Refresh설정값();

				//Refresh상태정보();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		#endregion

		//used in UI
		private void _u메뉴_On메뉴선택Event(프로그램메뉴Type mnu선택)
		{
			try
			{
				switch (mnu선택)
				{
					case 프로그램메뉴Type.환경보드:
						tabMain.SelectedItem = tab환경;
						break;

					//edgar this can be used for webcam later
					//case 프로그램메뉴Type.웹캠:
					//	tabMain.SelectedItem = tab웹캠;
					//	break;

					case 프로그램메뉴Type.HOME:
						tabMain.SelectedItem = tab메인;
						break;

					//case 프로그램메뉴Type.BIT설정:
					case 프로그램메뉴Type.BIT정보:
						tabMain.SelectedItem = tabBIT;

						break;
					case 프로그램메뉴Type.BIT설정:
						if (_wBIT설정 == null)
						{
							ShowBIT설정();
						}
						break;

					case 프로그램메뉴Type.환경설정:
						if (_w환경설정 == null)
						{
							Show환경설정();
						}
						break;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		#region BIS센터 관련

		BISManager mBIS = null;
		private void InitBIS센터()
		{
			try
			{
				if (mBIS == null)
				{
					mBIS = new BISManager();
					mBIS.OnSocket수신Event += MPAJUBIS_OnSocket수신Event;
					BISManager.OnSocket접속상태Event += MPAJUBIS_OnSocket접속상태Event;
					BISManager.On도착정보수신Event += MPAJUBIS_On도착정보수신Event;
					mBIS.On단말기제어수신Event += MPAJUBIS_On단말기제어수신Event;
					mBIS.OnAirQualityDataReceivedEvent += MPAJUBIS_OnAirQualityDataReveivedEvent;
					mBIS.OnWeatherDataReceivedEvent += MBIS_OnWeatherDataReceivedEvent;
				}
				//mBIS.Update서버정보();


				mBIS.Connet서버();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void MBIS_OnWeatherDataReceivedEvent(GwacheonWeatherResponse weatherData)
		{
			if (weatherData != null)
			{
				RefreshWeather(weatherData);
			}
		}

		private void MPAJUBIS_OnAirQualityDataReveivedEvent(AirQualityData weatherInfo)
		{
			if (weatherInfo != null)
			{
				RefreshAirQuality(weatherInfo);
			}
		}

		private void ClearPAJUBIS센터()
		{
			try
			{
				if (mBIS != null)
				{
					mBIS.On단말기제어수신Event -= MPAJUBIS_On단말기제어수신Event;
					BISManager.On도착정보수신Event -= MPAJUBIS_On도착정보수신Event;
					mBIS.OnSocket수신Event -= MPAJUBIS_OnSocket수신Event;
					BISManager.OnSocket접속상태Event -= MPAJUBIS_OnSocket접속상태Event;
					mBIS.OnAirQualityDataReceivedEvent -= MPAJUBIS_OnAirQualityDataReveivedEvent;
					mBIS.DoFinal();
					mBIS = null;
				}
				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		bool is센터접속Loaded = false;
		private void MPAJUBIS_OnSocket접속상태Event(string ip, bool connectYN)
		{
			try
			{
				EventManager.DisplayLog(Log4Level.Info, string.Format("[BIS] {0}되었습니다.", connectYN == true ? "연결" : "연결해제"));
				Refresh네트워크연결해제();
				//if (connectYN == true)
				//{
				//	if (is센터접속Loaded == false)
				//	{
				//		EventManager.DisplayLog(Log4Level.Info, string.Format("[BIS] {0}에 부팅정보를 전송합니다.", ip));

				//		//mBIS.Send부팅정보();
				//	}
				//}
				//else
				//{
				//	Refresh네트워크연결해제();
				//}

			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void MPAJUBIS_OnSocket수신Event(PAJU_BIS_OPCODE_TYPE type)
		{
			try
			{
				switch (type)
				{
					case PAJU_BIS_OPCODE_TYPE.파라메터변경:
						break;

					case PAJU_BIS_OPCODE_TYPE.도착예정정보:
						break;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		//DateTime dtRecv도착정보 = DateTime.Now.AddDays(-1);

		private void MPAJUBIS_On단말기제어수신Event(List<PajuBisStatusControl> _items)
		{
			try
			{
				EventManager.DisplayLog(Log4Level.Info, string.Format("[TCP] {0}건의 단말기상태제어값이 수신되었습니다.", _items.Count));
				this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate
				{
					BIT_ENV_SETTING item설정 = BITDataManager.BitENVConfig.ITEM;
					bool b환경보드값변경YN = false;
					foreach (PajuBisStatusControl item in _items)
					{
						PAJU_BIS_상세제어Type itemGBN = (PAJU_BIS_상세제어Type)item.ControlCode;

						switch (itemGBN)
						{
							case PAJU_BIS_상세제어Type.볼륨:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.Volume, item.ControlValue));
								item설정.Volume = item.ControlValue;
								VolumeService.SetVolume(item설정.Volume); //20220727 BHA
								break;

							case PAJU_BIS_상세제어Type.잠시후도착조건:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.ArriveSoonGBN, item.ControlValue));
								item설정.ArriveSoonGBN = item.ControlValue;
								break;

							case PAJU_BIS_상세제어Type.잠시후도착시간조건:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.ArriveSoonTimeGBN, item.ControlValue));
								item설정.ArriveSoonTimeGBN = item.ControlValue;
								break;

							case PAJU_BIS_상세제어Type.잠시후도착정류장조건:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.ArriveSoonStationGBN, item.ControlValue));
								item설정.ArriveSoonStationGBN = item.ControlValue;
								break;

							case PAJU_BIS_상세제어Type.모니터On시간:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.MonitorOnTime, item.ControlValue));
								item설정.MonitorOnTime = string.Format("{0:d4}", item.ControlValue);
								b환경보드값변경YN = true;
								break;

							case PAJU_BIS_상세제어Type.모니터Off시간:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.MonitorOffTime, item.ControlValue));
								item설정.MonitorOffTime = string.Format("{0:d4}", item.ControlValue);
								b환경보드값변경YN = true;
								break;

							case PAJU_BIS_상세제어Type.상태정보전송주기:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.StateSendPeriod, item.ControlValue));
								item설정.StateSendPeriod = item.ControlValue;
								break;

							case PAJU_BIS_상세제어Type.영상정보전송주기:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.WebcamSendPeriod, item.ControlValue));
								item설정.WebcamSendPeriod = item.ControlValue;
								break;

							case PAJU_BIS_상세제어Type.ScreeCapture전송주기:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.ScreenCaptureSendPeriod, item.ControlValue));
								item설정.ScreenCaptureSendPeriod = item.ControlValue;
								break;

							case PAJU_BIS_상세제어Type.BIT정보정렬방식:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.BITOrderGBN, item.ControlValue));
								item설정.BITOrderGBN = item.ControlValue;
								break;

							case PAJU_BIS_상세제어Type.동작감시센서사용여부:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.UseDetectSensor, item.ControlValue));
								item설정.UseDetectSensor = item.ControlValue;
								b환경보드값변경YN = true;
								break;

							case PAJU_BIS_상세제어Type.동작감시센서사용시간:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.DetectSensorServiceTime, item.ControlValue));
								item설정.DetectSensorServiceTime = item.ControlValue;
								b환경보드값변경YN = true;
								break;

							case PAJU_BIS_상세제어Type.팬동작온도조건MAX:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.FANMaxTemp, item.ControlValue));
								item설정.FANMaxTemp = item.ControlValue;
								b환경보드값변경YN = true;
								break;

							case PAJU_BIS_상세제어Type.팬동작온도조건MIN:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.FANMinTemp, item.ControlValue));
								item설정.FANMinTemp = item.ControlValue;
								b환경보드값변경YN = true;
								break;

							case PAJU_BIS_상세제어Type.히터동작온도조건MAX:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.HeaterMaxTemp, item.ControlValue));
								item설정.HeaterMaxTemp = item.ControlValue;
								b환경보드값변경YN = true;
								break;

							case PAJU_BIS_상세제어Type.히터동작온도조건MIN:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.HeaterMinTemp, item.ControlValue));
								item설정.HeaterMinTemp = item.ControlValue;
								b환경보드값변경YN = true;
								break;

							case PAJU_BIS_상세제어Type.지하철정보표시여부:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.SubwayDisplayYN, item.ControlValue));
								item설정.SubwayDisplayYN = item.ControlValue;
								break;

							case PAJU_BIS_상세제어Type.지하철호선코드:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.SubwayLineNo, item.ControlValue));
								item설정.SubwayLineNo = item.ControlValue;
								break;

							case PAJU_BIS_상세제어Type.지하철역코드:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.SubwayStationNo, item.ControlValue));
								item설정.SubwayStationNo = item.ControlValue;
								break;

							case PAJU_BIS_상세제어Type.외국어정보표시여부:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.ForeignDisplayYN, item.ControlValue));
								item설정.ForeignDisplayYN = item.ControlValue;
								break;

							case PAJU_BIS_상세제어Type.외국어정보표출시간:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.ForeignDisplayTime, item.ControlValue));
								item설정.ForeignDisplayTime = item.ControlValue;
								break;

							case PAJU_BIS_상세제어Type.충격감지감도:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.ShockDetectValue, item.ControlValue));
								item설정.ShockDetectValue = item.ControlValue;
								break;

							case PAJU_BIS_상세제어Type.LCD제어:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값 {1}이 적용되었습니다.", itemGBN.ToString(), item.ControlValue));
								Refresh화면설정(item.ControlValue);
								break;

							case PAJU_BIS_상세제어Type.Reset제어:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값 {1}이 적용되었습니다.", itemGBN.ToString(), item.ControlValue));
								if (item.ControlValue == 1) 리셋Proc();
								break;

							case PAJU_BIS_상세제어Type.시정홍보음성표출:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.PromoteSoundPlayYN, item.ControlValue));
								item설정.PromoteSoundPlayYN = item.ControlValue;
								break;

							case PAJU_BIS_상세제어Type.BIT표출글씨크기:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.BITFontSize, item.ControlValue));
								item설정.BITFontSize = item.ControlValue;
								break;

							case PAJU_BIS_상세제어Type.시험운영중표출여부:
								EventManager.DisplayLog(Log4Level.Info, string.Format("[설정변경] {0}의 설정값이 {1}→{2}변경되었습니다.", itemGBN.ToString(), item설정.TestOperationDisplayYN, item.ControlValue));
								item설정.TestOperationDisplayYN = item.ControlValue;
								break;
						}

						item설정.REGDATE = DateTime.Now;
						BITDataManager.Set파라메터변경(item설정);

						if (b환경보드값변경YN == true)
						{
							_uENV2.Set설정값변경();
						}
					}
				}));
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		DateTime dt도착정보 = DateTime.Now.AddDays(-1);
		private void MPAJUBIS_On도착정보수신Event(BisExpectedArrivalInformation item)
		{
			try
			{
				if (item == null) return;

				//string time = string.Format("{0:d8}{1:d6}", item.TransmissionDate, item.TransmissionTime);
				//DateTime dt정보 = ConvertUtils.DateTimeByString(item.TimeWhenInfoReceived).Value;
				DateTime dt정보 = DateTime.Now;

				if (dt도착정보.CompareTo(dt정보) < 0)
				{
					dt도착정보 = dt정보;

					if (item.ArrivalInformationList == null || item.ArrivalInformationList.Count == 0)
					{
						Refresh도착예정정보(null);

						string log = string.Format("[도착정보] {0} {1:d6} BIT_ID:{2} 버스운행정보가 없습니다.", item.TransmissionDate, item.TransmissionTime, item.BitId);

						EventManager.DisplayLog(Log4Level.Info, log, LogSource.BusInfo);
					}
					else
					{
						Refresh도착예정정보(item.ArrivalInformationList);
					}
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		#endregion

		private void InitializeBITViewer()
		{
			try
			{
				_uENV2.Initialize환경보드();

				InitBIS센터();

				Show화면();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		bool 충격처리YN = false;

		//edgar used in UI
		private void _uENV2_On충격감지Event(int ShockValue)
		{
			try
			{
				if (충격처리YN == true) return;

				충격처리YN = true;

				byte[] btDatas = Encoding.UTF8.GetBytes("SHOCK");
				mShareWebcam.Write(btDatas);
				TraceManager.AddInfoLog(string.Format("[IPC송신] BIT→WEBCAM : {0}", ShockValue));

				DateTime dt녹화시간 = DateTime.Now;
				//string FILE_NM = DateTime.Now.ToString("yyyyMMdd");
				System.Windows.Threading.DispatcherTimer _dt확인 = new System.Windows.Threading.DispatcherTimer();
				_dt확인.Interval = TimeSpan.FromMinutes(1);
				_dt확인.Tick += delegate (object ds, EventArgs de)
				{
					충격처리YN = false;
					_dt확인.Stop();

					string 저장DIR = Path.Combine(DataManager.Get캡쳐DIR(), "SHOCK");
					if (Directory.Exists(저장DIR) == false) return;

					List<string> items = Directory.GetFiles(저장DIR, string.Format("{0}*.mp4", DateTime.Now.ToString("yyyyMMdd")), SearchOption.TopDirectoryOnly).ToList();
					if (items != null && items.Count > 0)
					{
						foreach (string item in items)
						{
							FileInfo fi = new FileInfo(item);
							DateTime dt파일 = ConvertUtils.DateTimeByString(fi.Name.Replace(fi.Extension, "")).Value;
							int result = dt파일.CompareTo(dt녹화시간);
							if (result >= 0)
							{
								//Upload충격영상(저장DIR, fi.Name, ShockValue);
							}
							else
							{
								TraceManager.AddInfoLog(string.Format("충격저장영상이 존재하지않습니다. {0}/{1}", dt녹화시간.ToString("yyyyMMddHHmmsss"), ShockValue));
							}
						}
					}
				};
				_dt확인.Start();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
		
		#region FTP/SFTP 다운로드/업로드

		//private async void Upload충격영상(string dir, string fileName, int nShock)
		//{
		//	try
		//	{
		//		var config = BITDataManager.BitSystem;
		//		if (NetworkUtils.CheckIPPort(config.FTP_IP, config.FTP_PORT) == false) return;

		//		switch (config.FTP_GBN)
		//		{
		//			case 1:
		//				FTPManager mFTP = new FTPManager();
		//				int resultNo = await mFTP.업로드FileAsync(fileName, string.Format("/CRASH/{0}/", BITDataManager.BIT_ID), System.IO.Path.Combine(dir, fileName));
		//				if (resultNo == 1)
		//				{
		//					Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[FTP] 충격영상 업로드 완료 : {0}", fileName));
		//				}
		//				else
		//				{
		//					Log4Manager.WriteFTPLog(Log4Level.Error, string.Format("[FTP] 충격영상 업로드 오류 : {0}", fileName));
		//				}
		//				break;

		//			case 2:
		//				SFTPManager mSFTP = new SFTPManager();

		//				List<Task> taskList = new List<Task>();
		//				Task<int> taskUpdate = Task<int>.Factory.StartNew(() => mSFTP.업로드File(fileName, string.Format("/CRASH/{0}/", BITDataManager.BIT_ID), System.IO.Path.Combine(dir, fileName)));
		//				taskList.Add(taskUpdate);

		//				Task.WaitAll(taskList.ToArray(), TimeSpan.FromSeconds(3));
		//				//taskUpdate.Wait(1000);

		//				int resultSFTP = taskUpdate.Result;
		//				//int resultSFTP =  mSFTP.업로드File(fileName, "/CAPTURE/", string.Format("{0}_1.jpg", BITDataManager.BitConfig.BASIC.BIT_ID));
		//				if (resultSFTP == 1)
		//				{
		//					Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[SFTP] 충격영상 업로드 완료 : {0}", fileName));
		//					mBIS.SendBIT충격영상전송(nShock, fileName);
		//				}
		//				else
		//				{
		//					Log4Manager.WriteFTPLog(Log4Level.Error, string.Format("[SFTP] 충격영상 업로드 오류 : {0}", fileName));
		//				}
		//				break;
		//		}
		//	}
		//	catch (Exception ee)
		//	{
		//		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//	}
		//}

		#endregion

		#region 폴더 및 파일 정리
		private void Clear로그폴더()
		{
			try
			{
				EventManager.DisplayLog(Log4Level.Info, "오래된 로그파일을 정리합니다.");

				string 기본DIR = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "LOG");

				string[] directories = System.IO.Directory.GetDirectories(기본DIR);
				foreach (string dir in directories)
				{
					string[] SubDirecties = System.IO.Directory.GetDirectories(dir);
					if (SubDirecties.Length == 0)
					{
						CommonProc.ClearLowDirectory(dir, BITDataManager.BitConfig.LOGSAVE_DAY);
					}
					else
					{
						CommonProc.ClearDirectoryRemove(dir, BITDataManager.BitConfig.LOGSAVE_DAY);
					}
				}

				string LogDIR = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Trace");
				if (System.IO.Directory.Exists(LogDIR) == true)
				{
					CommonProc.ClearLowDirectory(LogDIR, BITDataManager.BitConfig.LOGSAVE_DAY);
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Clear파일정리()
		{
			try
			{
				//뉴스파일 이전데이터를 삭제합니다.
				string 뉴스DIR = System.IO.Path.Combine(DataManager.Get컨텐츠DIR(), "NEWS");
				if (System.IO.Directory.Exists(뉴스DIR) == true)
				{
					EventManager.DisplayLog(Log4Level.Info, "뉴스파일 조회후 오래된 파일을 정리합니다.");
					string[] items뉴스 = System.IO.Directory.GetFiles(뉴스DIR, "*.txt", System.IO.SearchOption.TopDirectoryOnly);
					foreach (string item in items뉴스)
					{
						string[] datas = item.Split('_');
						if (datas.Length < 2) continue;

						DateTime dt파일 = ConvertUtils.DateTimeByString(datas[1].Replace(".txt", "")).Value;
						int res = dt파일.CompareTo(DateTime.Now.AddDays(-1 * BITDataManager.BitConfig.LOGSAVE_DAY));
						if (res < 0)
						{
							try
							{
								System.IO.File.Delete(item);
							}
							catch (System.IO.IOException ex)
							{
								TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
							}
						}
					}
				}

				string 홍보DIR = System.IO.Path.Combine(DataManager.Get컨텐츠DIR(), "NOTICE", "CONFIG");
				if (System.IO.Directory.Exists(홍보DIR) == true)
				{
					EventManager.DisplayLog(Log4Level.Info, "홍보파일 조회후 오래된 파일을 정리합니다.");
					string[] items홍보 = System.IO.Directory.GetFiles(홍보DIR, "*.dat", System.IO.SearchOption.TopDirectoryOnly);
					foreach (string item in items홍보)
					{
						string[] datas = item.Split('_');
						if (datas.Length < 2) continue;

						DateTime dt파일 = ConvertUtils.DateTimeByString(datas[1].Replace(".DAT", "")).Value;
						int res = dt파일.CompareTo(DateTime.Now.AddDays(-1 * BITDataManager.BitConfig.LOGSAVE_DAY));
						if (res < 0)
						{
							try
							{
								System.IO.File.Delete(item);
							}
							catch (System.IO.IOException ex)
							{
								TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
							}
						}
					}
				}

				string 화면캡쳐DIR = System.IO.Path.Combine(DataManager.Get캡쳐DIR(), "SCREEN");
				if (System.IO.Directory.Exists(화면캡쳐DIR) == true)
				{
					EventManager.DisplayLog(Log4Level.Info, "화면캡쳐파일 조회후 오래된 파일을 정리합니다.");

					string[] items화면캡쳐 = System.IO.Directory.GetFiles(화면캡쳐DIR, "*.jpg", System.IO.SearchOption.TopDirectoryOnly);
					foreach (string item in items화면캡쳐)
					{
						//string[] datas = item.Split('_');
						System.IO.FileInfo fi = new System.IO.FileInfo(item);
						//if (datas.Length < 4) continue;

						DateTime dt파일 = ConvertUtils.DateTimeByString(fi.Name.Replace(fi.Extension, "")).Value;
						int res = dt파일.CompareTo(DateTime.Now.AddDays(-1 * BITDataManager.BitConfig.LOGSAVE_DAY));
						if (res < 0)
						{
							try
							{
								fi.Delete();
								//System.IO.File.Delete(item);
							}
							catch (System.IO.IOException ex)
							{
								TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
							}
						}
						fi = null;
					}
				}

				string 웹캠DIR = System.IO.Path.Combine(DataManager.Get캡쳐DIR(), "WEBCAM");
				if (System.IO.Directory.Exists(웹캠DIR) == true)
				{
					EventManager.DisplayLog(Log4Level.Info, "웹캡파일 조회후 오래된 파일을 정리합니다.");

					string[] items웹캠 = System.IO.Directory.GetFiles(웹캠DIR, "*.jpg", System.IO.SearchOption.TopDirectoryOnly);
					foreach (string item in items웹캠)
					{
						System.IO.FileInfo fi = new System.IO.FileInfo(item);

						DateTime dt파일 = ConvertUtils.DateTimeByString(fi.Name.Replace(fi.Extension, "")).Value;
						int res = dt파일.CompareTo(DateTime.Now.AddDays(-1 * BITDataManager.BitConfig.LOGSAVE_DAY));
						if (res < 0)
						{
							try
							{
								fi.Delete();
								//System.IO.File.Delete(item);
							}
							catch (System.IO.IOException ex)
							{
								TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
							}
						}
						fi = null;
					}
				}

				string 충격DIR = System.IO.Path.Combine(DataManager.Get캡쳐DIR(), "SHOCK");
				if (System.IO.Directory.Exists(충격DIR) == true)
				{
					EventManager.DisplayLog(Log4Level.Info, "충격영상 조회후 오래된 파일을 정리합니다.");

					List<string> items충격 = new List<string>();
					items충격.AddRange(System.IO.Directory.GetFiles(충격DIR, "*.avi", System.IO.SearchOption.TopDirectoryOnly).ToList());
					items충격.AddRange(System.IO.Directory.GetFiles(충격DIR, "*.mp4", System.IO.SearchOption.TopDirectoryOnly).ToList());
					foreach (string item in items충격)
					{
						System.IO.FileInfo fi = new System.IO.FileInfo(item);

						DateTime dt파일 = ConvertUtils.DateTimeByString(fi.Name.Replace(fi.Extension, "")).Value;
						int res = dt파일.CompareTo(DateTime.Now.AddDays(-1 * BITDataManager.BitConfig.LOGSAVE_DAY));
						if (res < 0)
						{
							try
							{
								fi.Delete();
							}
							catch (System.IO.IOException ex)
							{
								TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
							}
						}
						fi = null;
					}
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

		private void 리셋Proc()
		{
			try
			{
				EventManager.DisplayLog(Log4Level.Info, "[PC재시작] PC재부팅 명령을 전송중입니다.");

				ShutdownManager mShutdown = new ShutdownManager();
				mShutdown.PC재시작Proc();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		#region 데이터 To 화면 적용 ( 데이터 변경)


		public void Refresh도착예정정보(List<BisArrivalInformation> datas)
		{
			try
			{
				this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate
				{
					if (BITDataManager.BitConfig.IS_VERTICAL_LAYOUT)
					{
						_wVerticalHD.Refresh도착정보(datas);
					}
					else
					{
						_wHorizontalHD?.Refresh도착정보(datas);
					}
									
				}));

				if (datas != null)
				{
					foreach (BisArrivalInformation data in datas)
					{
						string disp;
						if (data.EstimatedTimeOfArrival == 0 && data.StopName == "")
						{
								disp = string.Format("[{0}] {1}행 버스가 대기중."
								, data.RouteNumber, data.DestinationName);
						}
						else
						{
							disp = string.Format("[{0}] {2,4}초후 {1}에서 {3}행 버스가 도착합니다."
								, data.RouteNumber, data.StopName, data.EstimatedTimeOfArrival, data.DestinationName);
						}
						EventManager.DisplayLog(Log4Level.Info, disp, LogSource.BusInfo);
					}
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void RefreshWeather(GwacheonWeatherResponse item)
		{
			try
			{
				EventManager.DisplayLog(Log4Level.Info, "날씨정보가 업데이트 되었습니다.");
				this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
				{
					if (BITDataManager.BitConfig.IS_VERTICAL_LAYOUT)
					{
						_wVerticalHD.RefreshWeather(item);
					}
					else
					{
						//_wHorizontalHD?.RefreshAirQuality(item);
					}

				}));
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void RefreshAirQuality(AirQualityData item)
		{
			try
			{
				EventManager.DisplayLog(Log4Level.Info, "미세먼지정보가 업데이트 되었습니다.");
				this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
				{				
					if (BITDataManager.BitConfig.IS_VERTICAL_LAYOUT)
					{
						_wVerticalHD.RefreshAirQuality(item);
					}
					else
					{
						_wHorizontalHD?.Refresh날씨정보(item);
					}

				}));
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Refresh공지사항(List<NoticeData> items)
		{
			try
			{
				EventManager.DisplayLog(Log4Level.Info, "공지사항이 추가되었습니다.");
				this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
				{
					if (BITDataManager.BitConfig.IS_VERTICAL_LAYOUT)
					{
						_wVerticalHD.Refresh공지사항(items);
					}
					else
					{
						_wHorizontalHD?.Refresh공지사항(items);
					}
				}));
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		
		private void Refresh네트워크연결해제()
		{
			try
			{
				//EventManager.DisplayLog(Log4Level.Info, "날씨정보가 업데이트 되었습니다.");
				this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
				{
					if (BITDataManager.BitConfig.IS_VERTICAL_LAYOUT)
					{
						_wVerticalHD.Refresh네트워크연결해제();
					}
					else
					{
						_wHorizontalHD?.Refresh네트워크연결해제();
					}
				}));
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Refresh화면설정(int n설정)
		{
			try
			{
				EventManager.DisplayLog(Log4Level.Info, "LCD화면설정을 변경합니다.");
				switch (n설정)
				{
					case 0: Close화면(); break;
					case 1: Show화면(); break;						
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		#endregion

		#region LCD LED 화면표시

		private void Check화면관리()
		{
			try
			{
				DateTime dt = DateTime.Now;
				var config = BITDataManager.BitENVConfig.ITEM;
				if (config == null) return;

				if (config.MonitorOnTime == null || config.MonitorOffTime == null) return;

				if (config.MonitorOnTime.Equals(config.MonitorOffTime) == true)
				{
					EventManager.DisplayLog(Log4Level.Info, "모니터 On/Off시간이 동일하여 작동하지 않습니다.");
					return;
				}
				DateTime dt모니터On = new DateTime(dt.Year, dt.Month, dt.Day, Convert.ToInt32(config.MonitorOnTime.Substring(0, 2)), Convert.ToInt32(config.MonitorOnTime.Substring(2, 2)), 0);
				int n모니터On = dt.CompareTo(dt모니터On);
				int n모니터On범위 = dt.CompareTo(dt모니터On.AddMinutes(1));
				if (n모니터On >= -1 && n모니터On범위 <= 0)
				{
					EventManager.DisplayLog(Log4Level.Info, "모니터On시간이라 화면을 표시합니다.");
					Show화면();
					return;
				}

				DateTime dt모니터Off = new DateTime(dt.Year, dt.Month, dt.Day, Convert.ToInt32(config.MonitorOffTime.Substring(0, 2)), Convert.ToInt32(config.MonitorOffTime.Substring(2, 2)), 0);
				int n모니터Off = dt.CompareTo(dt모니터Off);
				int n모니터Off범위 = dt.CompareTo(dt모니터Off.AddMinutes(1));
				if (n모니터Off >= 1 && n모니터Off범위 <= 0)
				{
					EventManager.DisplayLog(Log4Level.Info, "모니터Off시간이라 표시된 화면을 종료합니다.");
					Close화면();
					return;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}


		private void EventManager_OnRefreshDisplayEvent(int _nMode, BIT_DISPLAY _item)
		{
			try
			{
				switch (_nMode)
				{
					case 1:
						EventManager.DisplayLog(Log4Level.Info, string.Format("[{0}] 화면을 추가합니다.", _item.표시화면));
						switch (_item.화면구분)
						{
							//20230103 BHA 
							case DISPLAY구분.LCD세로형HD: ShowLCDHorizontalHD(); break;
						}
						break;

					case 2:
						EventManager.DisplayLog(Log4Level.Info, string.Format("[{0}] 화면을 변경합니다.", _item.표시화면));
						switch (_item.화면구분)
						{
							//20230103 BHA
							case DISPLAY구분.LCD세로형HD: ModifyLCDHorizontalHD(_item); break;
						}
						break;

					case 3:
						EventManager.DisplayLog(Log4Level.Info, string.Format("[{0}] 화면을 삭제합니다.", _item.표시화면));
						switch (_item.화면구분)
						{
							//20230103 BHA
							case DISPLAY구분.LCD세로형HD: DeleteLCDHorizontalHD(_item); break;
						}
						break;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		bool isVerticalDisplay = BITDataManager.BitConfig.IS_VERTICAL_LAYOUT;
		private void Show화면(int DispGBN = 0)
		{
			try
			{
				if (BITDataManager.BitConfig.IS_VERTICAL_LAYOUT)
				{
					ShowLCDVerticalHD();
				}
				else
				{					
					ShowLCDHorizontalHD();
				}		
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Close화면()
		{
			try
			{
				//_w가로형HD?.Close();
				_wHorizontalHD?.Close();
				_wVerticalHD?.Close();

				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		wndLCDHorizontal_HD _wHorizontalHD = null;
		private void ShowLCDHorizontalHD()
		{
			try
			{
				if (_wHorizontalHD == null)
				{
					_wHorizontalHD = new wndLCDHorizontal_HD();
					_wHorizontalHD.Closed += _wHorizontalHD_Closed;

					//_w가로형.SetParentWindow(this);
					//_w가로형.Owner = this;
					_wHorizontalHD.WindowStartupLocation = WindowStartupLocation.Manual;
					_wHorizontalHD.Left = 0; //item.POS_X;
					_wHorizontalHD.Top = 0; //item.POS_Y;
					_wHorizontalHD.Topmost = true;
					_wHorizontalHD.Show();
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		wndLCDVertical_HD _wVerticalHD = null;
		private void ShowLCDVerticalHD()
		{
			try
			{
				if (_wVerticalHD == null)
				{
					_wVerticalHD = new wndLCDVertical_HD();
					_wVerticalHD.Closed += _wVerticalHD_Closed;

					//_w가로형.SetParentWindow(this);
					//_w가로형.Owner = this;
					_wVerticalHD.WindowStartupLocation = WindowStartupLocation.Manual;
					_wVerticalHD.Left = 0; //item.POS_X;
					_wVerticalHD.Top = 0; //item.POS_Y;
					_wVerticalHD.Topmost = true;
					_wVerticalHD.Show();
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void _wHorizontalHD_Closed(object sender, EventArgs e)
		{
			try
			{
				if (_wHorizontalHD != null)
				{
					_wHorizontalHD.Closed -= _wHorizontalHD_Closed;
					_wHorizontalHD = null;
				}
				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void _wVerticalHD_Closed(object sender, EventArgs e)
		{
			try
			{
				if (_wVerticalHD != null)
				{
					_wVerticalHD.Closed -= _wVerticalHD_Closed;
					_wVerticalHD = null;
				}
				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void ModifyLCDHorizontalHD(BIT_DISPLAY item)
		{
			try
			{
				if (_wHorizontalHD != null)
				{
					_wHorizontalHD.Left = item.POS_X;
					_wHorizontalHD.Top = item.POS_Y;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void ModifyLCDVerticalHD(BIT_DISPLAY item)
		{
			try
			{
				if (_wVerticalHD != null)
				{
					_wVerticalHD.Left = item.POS_X;
					_wVerticalHD.Top = item.POS_Y;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void DeleteLCDHorizontalHD(BIT_DISPLAY item)
		{
			try
			{
				_wHorizontalHD?.Close();

				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void DeleteLCDVerticalHD(BIT_DISPLAY item)
		{
			try
			{
				_wVerticalHD?.Close();

				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		#endregion

		#region TTS관련

		private void EventManager_OnPlayMessageTTSEvent(string _item)
		{
			try
			{
				if (tts == null) InitTTSProc();

				tts.Play(_item);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void EventManager_OnPlayBusNoTTSEvent(List<string> _items)
		{
			try
			{
				if (tts == null) InitTTSProc();

				string message = "";
				foreach (string bus in _items)
				{
					if (string.IsNullOrWhiteSpace(bus))
					{
						continue;
					}

					//string bisNo = bus.Replace("-", "다시. ");
					string bisNo = bus;
					bisNo = bisNo.Replace("M", "엠, ");
					bisNo = bisNo.Replace("A", "에이, ");
					bisNo = bisNo.Replace("B", "비, ");
					bisNo = bisNo.Replace("C", "씨, ");
					bisNo = bisNo.Replace("G", "지, ");

					//change number spelling
					bisNo = bisNo.Replace("-1", "다시.일, ");
					bisNo = bisNo.Replace("-2", "다시.이, ");
					bisNo = bisNo.Replace("-3", "다시.삼, ");
					bisNo = bisNo.Replace("-4", "다시.사, ");
					bisNo = bisNo.Replace("-5", "다시.오, ");
					bisNo = bisNo.Replace("-6", "다시.육, ");
					bisNo = bisNo.Replace("-7", "다시.칠, ");
					bisNo = bisNo.Replace("-8", "다시.팔, ");
					bisNo = bisNo.Replace("-9", "다시.구, ");
					bisNo = bisNo.Replace("-0", "다시.영, ");
					//bisNo = bisNo.Replace("Y", "마을버스, ");

					message += string.Format("{0}, 번, ", bisNo);
				}

				message += "버스가 곧 도착 합니다, ,";
				//Console.WriteLine(message.Length);
				tts.Play(message);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public MSTTSManager tts_ms = null;
		public PowerTTS tts = null;
		
		private void InitTTSProc()
		{
			try
			{
				//ms tts
				tts_ms ??= new MSTTSManager();
				tts_ms.InitializeTTS();

				//power tts
				if (tts == null)
				{
					tts = new PowerTTS();
				}
				tts.Open();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
		private void PlayTTS(string msg)
		{
			try
			{
				//if (playMessage.Equals(msg) == true)
				//{
				//    TraceManager.AddTTSLog(string.Format("[중복생략] {0}", msg));
				//    return;
				//}
				//playMessage = msg;

				string[] busList = msg.Split(',');

				string message = "";
				foreach (string bus in busList)
				{
					if (bus.Equals("") == true) continue;

					string bisNo = bus.Replace("-", "다시, ");
					bisNo = bisNo.Replace("M", "엠, ");
					bisNo = bisNo.Replace("A", "에이, ");
					bisNo = bisNo.Replace("B", "비, ");
					bisNo = bisNo.Replace("C", "씨, ");
					//bisNo = bisNo.Replace("Y", "마을버스, ");

					message += string.Format("{0}, 번, ", bisNo);
				}

				message += "버스가 곧 도착 합니다,";
				//Console.WriteLine(message.Length);
				tts.Play(message);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		#endregion

		#region 화면밝기 조정

		private void Apply화면밝기()
		{
			try
			{
				var config = BITDataManager.BitENVConfig.ITEM;

				if (config.DefaultLCDLux > 0)
				{
					ChangeLCD밝기(config.DefaultLCDLux);

					BITDataManager.MasterBrightness = config.DefaultLCDLux;

					//_u메인.Refresh화면밝기();
				}

				if (config.itemsLux?.Count > 0)
				{
					foreach (BIT_ENV_LUX item in config.itemsLux)
					{
						bool b포함YN = CommonUtils.CHECK_시간포함여부(item.S_TIME, item.E_TIME);
						if (b포함YN == true)
						{			

							ChangeLCD밝기(item.LUX);

							BITDataManager.MasterBrightness = item.LUX;
						}
					}
				}

			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void _u메인_OnLED밝기조절Event(int _brightnessNo)
		{
			try
			{
				ChangeLCD밝기(_brightnessNo);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void ChangeLCD밝기(int bright)
		{
			try
			{
				if (DataManager.DebugYN == true) return;

				double nOpercity = bright * 0.01;

				//20230103 BHA 
				//if (_w가로형HD != null)
				//{
				//	_w가로형HD.Opacity = nOpercity;
				//	EventManager.DisplayLog(Log4Level.Info, string.Format("[밝기변경] 가로형HD LCD밝기를 {0}으로 변경합니다.", bright));
				//}
				if (BITDataManager.BitConfig.IS_VERTICAL_LAYOUT)
				{					
					if (_wVerticalHD != null)
					{
						_wVerticalHD.Opacity = nOpercity;
						EventManager.DisplayLog(Log4Level.Info, string.Format("[밝기변경] Vertical HD LCD밝기를 {0}으로 변경합니다.", bright));
					}
				}
				else
				{
					if (_wHorizontalHD != null)
					{
						_wHorizontalHD.Opacity = nOpercity;
						EventManager.DisplayLog(Log4Level.Info, string.Format("[밝기변경] Horizontal HD LCD밝기를 {0}으로 변경합니다.", bright));
					}
				}

				
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		#endregion

		#region 볼륨 조정
		

		private void Apply볼륨()
		{
			try
			{
				//if (DataManager.DebugYN == true) return;

				var config = BITDataManager.BitENVConfig.ITEM;

				EventManager.DisplayLog(Log4Level.Info, string.Format("[소리] 볼륨LV을 {0}으로 변경합니다.", config.Volume));
				VolumeService.SetVolume(config.Volume);
				BITDataManager.MasterVolume = config.Volume;

				//_u메인.Refresh볼륨값();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}


		private void _u메인_On볼륨조절Event(int _vol)
		{
			try
			{
				if (BITDataManager.MasterVolume.Equals(_vol) == false)
				{
					EventManager.DisplayLog(Log4Level.Info, string.Format("[소리] 볼륨을 {0}으로 변경합니다.", _vol));
					VolumeService.SetVolume(_vol);
					BITDataManager.MasterVolume = _vol;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		#endregion


		Timer dummyDataTimer; 

		private void btn1_Click(object sender, RoutedEventArgs e)
		{
			dummyDataTimer ??= new Timer(x => { RunSimulation(); }, null, 0, 20000);

			try
			{
				//List<BisArrivalInformation> datas = CreateDummyData();

				//Refresh도착예정정보(datas);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void RunSimulation()
		{
			List<BisArrivalInformation> datas = CreateDummyData();

			Refresh도착예정정보(datas);
		}

		private static List<BisArrivalInformation> CreateDummyData()
		{
			List<BisArrivalInformation> datas = new List<BisArrivalInformation>();

			BisArrivalInformation item1 = new BisArrivalInformation
			{
				RouteNumberT = "M5107",
				RouteDirection = 1,
				StopLocationInformation = 0,
				StopName = "신성엔에스텍",
				VehicleNumber = 123456798,
				LicensePlateNumber = "53거5532",

				NumberOfRemainingStops = 3,
				OperationStatus = 4,
				EstimatedTimeOfArrival = 200,

				BusType = 0,
				FirstAndLastTrainType = 0,
				RouteType = 1,

				OriginName = "킨텍스공영차고지",
				DestinationName = "구파발역",
				BusCongestion = 1,
				NumberOfRemainingSeats = 25,

				TimeOfArrival = DateTime.Now.AddSeconds(-20)
			};

			datas.Add(item1);


			BisArrivalInformation item2 = new BisArrivalInformation
			{
				RouteNumberT = "38(동충하교)",
				RouteDirection = 1,
				StopLocationInformation = 0,
				StopName = "신성엔에스텍",
				VehicleNumber = 123456798,
				LicensePlateNumber = "53거5531",

				NumberOfRemainingStops = 3,
				OperationStatus = 4,
				EstimatedTimeOfArrival = 50,

				BusType = 0,
				FirstAndLastTrainType = 0,
				RouteType = 3,

				OriginName = "킨텍스공영차고지",
				DestinationName = "구파발역",
				BusCongestion = 9,
				NumberOfRemainingSeats = 255,

				TimeOfArrival = DateTime.Now.AddSeconds(-20)
			};

			datas.Add(item2);

			BisArrivalInformation item3 = new BisArrivalInformation
			{
				RouteNumberT = "99",
				RouteDirection = 1,
				StopLocationInformation = 0,
				StopName = "신성엔에스텍",
				VehicleNumber = 123456798,
				LicensePlateNumber = "51거5531",

				NumberOfRemainingStops = 3,
				OperationStatus = 10,
				EstimatedTimeOfArrival = 65,

				BusType = 0,
				FirstAndLastTrainType = 0,
				RouteType = 1,

				OriginName = "킨텍스",
				DestinationName = "은평뉴타운",
				BusCongestion = 1,
				NumberOfRemainingSeats = 255,
				TimeOfArrival = DateTime.Now.AddSeconds(-30)
			};

			datas.Add(item3);

			BisArrivalInformation item4 = new BisArrivalInformation
			{
				RouteNumberT = "31",
				RouteDirection = 1,
				StopLocationInformation = 0,
				StopName = "원흥역",
				VehicleNumber = 123456798,
				LicensePlateNumber = "511거5531",

				NumberOfRemainingStops = 2,
				OperationStatus = 3,
				EstimatedTimeOfArrival = 970,

				BusType = 0,
				FirstAndLastTrainType = 0,
				RouteType = 3,

				OriginName = "킨텍스역",
				DestinationName = "구파발역",
				BusCongestion = 1,
				NumberOfRemainingSeats = 255,

				TimeOfArrival = DateTime.Now.AddSeconds(-30)
			};

			datas.Add(item4);

			BisArrivalInformation item5 = new BisArrivalInformation
			{
				RouteNumberT = "G8711",
				RouteDirection = 1,
				StopLocationInformation = 0,
				StopName = "후곡마을",
				VehicleNumber = 123456798,
				LicensePlateNumber = "512거5531",

				NumberOfRemainingStops = 15,
				OperationStatus = 4,
				EstimatedTimeOfArrival = 1970,

				BusType = 0,
				FirstAndLastTrainType = 0,
				RouteType = 1,

				OriginName = "파주역",
				DestinationName = "연신내역",
				BusCongestion = 1,
				NumberOfRemainingSeats = 255,
				TimeOfArrival = DateTime.Now.AddSeconds(-30)
			};

			datas.Add(item5);

			BisArrivalInformation item6 = new BisArrivalInformation
			{
				RouteNumberT = "G81",
				RouteDirection = 1,
				StopLocationInformation = 0,
				StopName = "후곡마을",
				VehicleNumber = 123456798,
				LicensePlateNumber = "512거5531",

				NumberOfRemainingStops = 15,
				OperationStatus = 1,
				EstimatedTimeOfArrival = 1970,

				BusType = 0,
				FirstAndLastTrainType = 0,
				RouteType = 1,

				OriginName = "파주역",
				DestinationName = "연신내역",
				BusCongestion = 1,
				NumberOfRemainingSeats = 255,
				TimeOfArrival = DateTime.Now.AddSeconds(-30)
			};

			datas.Add(item6);
			return datas;
		}


		private void btn2_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				dummyDataTimer.Dispose();
				dummyDataTimer = null;
				Refresh도착예정정보(null);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
		#region 스크린캡쳐

		public static void CaptureScreen(string name, Point start, System.Windows.Size size)
		{
			try
			{
				int w = (int)size.Width;
				int h = (int)size.Height;
				int x = (int)start.X;
				int y = (int)start.Y;
				string names = name + ".png";

				using (Bitmap bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb))
				{
					// Bitmap 이미지 변경을 위해 Graphics 객체 생성
					using (Graphics gr = Graphics.FromImage(bmp))
					{
						// 화면을 그대로 카피해서 Bitmap 메모리에 저장
						gr.CopyFromScreen(x, y, 0, 0, bmp.Size);
					}
					// Bitmap 데이타를 파일로 저장
					bmp.Save(names, ImageFormat.Png);
					System.Diagnostics.Process.Start(names);
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public static void CaptureScreen(string filename, Rect rect)
		{
			try
			{
				int W = (int)rect.Width;
				int H = (int)rect.Height;
				int X = (int)rect.X;
				int Y = (int)rect.Y;
				W = (W + 3) / 4 * 4;    // multiple of 4
				string names = filename + ".png";
				string names_bw = filename + "_bw.png";

				using (Bitmap colorBitmap = new Bitmap(W, H, PixelFormat.Format32bppArgb))  // Format32bppArgb Format16bppGrayScale Format4bppIndexed
				{
					using (Graphics gr = Graphics.FromImage(colorBitmap))
					{
						gr.CopyFromScreen(X, Y, 0, 0, colorBitmap.Size);
					}
					colorBitmap.RotateFlip(RotateFlipType.Rotate180FlipY);

					//	colorBitmap.Save(names, ImageFormat.Png);
					//	System.Diagnostics.Process.Start(names);

					BitmapData cBd = colorBitmap.LockBits(new Rectangle(0, 0, W, H), ImageLockMode.ReadOnly, colorBitmap.PixelFormat);
					int cStride = cBd.Stride;

					Bitmap grayBitmap = new Bitmap(W, H, PixelFormat.Format8bppIndexed);
					BitmapData gBd = grayBitmap.LockBits(new Rectangle(0, 0, W, H), ImageLockMode.ReadWrite, grayBitmap.PixelFormat);
					int gStride = gBd.Stride;

					unsafe
					{
						for (int y = 0; y < H; y++)
						{
							byte* cPtr = (byte*)cBd.Scan0 + (y * cStride);
							byte* gPtr = (byte*)gBd.Scan0 + (y * gStride);

							for (int x = 0; x < W; x++)
							{
								gPtr[x] = (byte)(cPtr[x * 4] * .114 + cPtr[x * 4 + 1] * .587 + cPtr[x * 4 + 2] * .299);  // Color을 Gray 값으로 변환
																														 // colorbar gPtr[x] = (byte)(x % 256);
							}
						}
						colorBitmap.UnlockBits(cBd);
						grayBitmap.UnlockBits(gBd);

						int len = W * H;
						byte[] byteArray = new byte[len];
						byte* bPtr = (byte*)gBd.Scan0;
						System.Runtime.InteropServices.Marshal.Copy((IntPtr)bPtr, byteArray, 0, len);
						//	SSENV2Manager.TCON이미지쓰기(byteArray, X, Y, W, H);

						ColorPalette colorPalette = grayBitmap.Palette;
						for (int i = 0; i < 256; i++)
						{
							colorPalette.Entries[i] = Color.FromArgb(i, i, i);
						}
						grayBitmap.Palette = colorPalette;
						grayBitmap.Save(names_bw, ImageFormat.Png);
						System.Diagnostics.Process.Start(names_bw);
					}
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		static int imageId = 0;
		static public bool startUpFine = false;
		public static void CaptureScreenVertical(Rect rect)
		{
			try
			{
				//Stopwatch watch = new Stopwatch();
				//watch.Start();			

				int W = (int)rect.Width;
				int H = (int)rect.Height;
				int X = (int)rect.X;
				int Y = (int)rect.Y;
				W = (W + 3) / 4 * 4;    // multiple of 4

				using (Bitmap colorBitmap = new Bitmap(W, H, PixelFormat.Format32bppArgb))  // Format32bppArgb Format16bppGrayScale Format4bppIndexed
				{
					using (Graphics gr = Graphics.FromImage(colorBitmap))
					{
						gr.CopyFromScreen(X, Y, 0, 0, colorBitmap.Size);
					}

					colorBitmap.RotateFlip(RotateFlipType.Rotate180FlipY);					
					

					// colorBitmap.Save("Capture.png", ImageFormat.Png);
					//	System.Diagnostics.Process.Start("Capture.png");

					BitmapData cBd = colorBitmap.LockBits(new Rectangle(0, 0, W, H), ImageLockMode.ReadOnly, colorBitmap.PixelFormat);
					int cStride = cBd.Stride;

					Bitmap grayBitmap = new Bitmap(W, H, PixelFormat.Format8bppIndexed);
					BitmapData gBd = grayBitmap.LockBits(new Rectangle(0, 0, W, H), ImageLockMode.ReadWrite, grayBitmap.PixelFormat);
					int gStride = gBd.Stride;

					

					unsafe
					{
						for (int y = 0; y < H; y++)
						{
							byte* cPtr = (byte*)cBd.Scan0 + (y * cStride);
							byte* gPtr = (byte*)gBd.Scan0 + (y * gStride);

							for (int x = 0; x < W; x++)
							{
								gPtr[x] = (byte)(cPtr[x * 4] * .114 + cPtr[x * 4 + 1] * .587 + cPtr[x * 4 + 2] * .299);  // Color을 Gray 값으로 변환
																														 // colorbar gPtr[x] = (byte)(x % 256);
							}
						}

						//edgar test
						//SaveCaptureToFile(grayBitmap);
						//edgar end

						colorBitmap.UnlockBits(cBd);
						grayBitmap.UnlockBits(gBd);

						int len = W * H;
						byte[] byteArray = new byte[len];
						byte* bPtr = (byte*)gBd.Scan0;
						System.Runtime.InteropServices.Marshal.Copy((IntPtr)bPtr, byteArray, 0, len);

						SSENV2Manager.TCON이미지쓰기(byteArray, X, Y, W, H);

						//ColorPalette colorPalette = grayBitmap.Palette;
						//for (int i = 0; i < 256; i++)
						//{
						//	colorPalette.Entries[i] = Color.FromArgb(i, i, i);
						//}
						//grayBitmap.Palette = colorPalette;
						//grayBitmap.Save("fast_gray.png", ImageFormat.Png);
						//System.Diagnostics.Process.Start("fast_gray.png");
					}
				}
				//	watch.Stop();
				//	Console.WriteLine($"캡쳐와 큐잉 시간 : {watch.ElapsedMilliseconds} ms");
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public static void CaptureScreenHorizontal(Rect rect)
		{
			try
			{
				//Stopwatch watch = new Stopwatch();
				//watch.Start();
							
					//1. take screen shot of whole app
					//2. rotate by 90 degrees
					//3. get rect to send to tcon
					using (Bitmap colorBitmap = new Bitmap(1280, 720, PixelFormat.Format32bppArgb))
					{
						using (Graphics gr = Graphics.FromImage(colorBitmap))
						{
							gr.CopyFromScreen(0, 0, 0, 0, colorBitmap.Size);
						}

						colorBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
						//colorBitmap.Save("Capture-rotate.png", ImageFormat.Png);

						int W = (int)rect.Width;
						int H = (int)rect.Height;
						int X = (int)rect.X;
						int Y = (int)rect.Y;
						W = (W + 3) / 4 * 4;    // multiple of 4

					Bitmap smallbitmap = colorBitmap.Clone(new Rectangle(X, Y, W, H), colorBitmap.PixelFormat);
					smallbitmap.RotateFlip(RotateFlipType.Rotate180FlipXY);

					BitmapData cBd = smallbitmap.LockBits(new Rectangle(0, 0, W, H), ImageLockMode.ReadOnly, smallbitmap.PixelFormat);
						int cStride = cBd.Stride;

						Bitmap grayBitmap = new Bitmap(W, H, PixelFormat.Format8bppIndexed);
						BitmapData gBd = grayBitmap.LockBits(new Rectangle(0, 0, W, H), ImageLockMode.ReadWrite, grayBitmap.PixelFormat);
						int gStride = gBd.Stride;

					//grayBitmap.RotateFlip(RotateFlipType.Rotate180FlipY);

						unsafe
						{
							for (int y = 0; y < H; y++)
							{
								byte* cPtr = (byte*)cBd.Scan0 + (y * cStride);
								byte* gPtr = (byte*)gBd.Scan0 + (y * gStride);

								for (int x = 0; x < W; x++)
								{
									gPtr[x] = (byte)(cPtr[x * 4] * .114 + cPtr[x * 4 + 1] * .587 + cPtr[x * 4 + 2] * .299);  // Color을 Gray 값으로 변환
																															 // colorbar gPtr[x] = (byte)(x % 256);
								}
							}

							//edgar test
							//SaveCaptureToFile(grayBitmap);
						//edgar end

						smallbitmap.UnlockBits(cBd);
							grayBitmap.UnlockBits(gBd);

							int len = W * H;
							byte[] byteArray = new byte[len];
							byte* bPtr = (byte*)gBd.Scan0;
							System.Runtime.InteropServices.Marshal.Copy((IntPtr)bPtr, byteArray, 0, len);

							SSENV2Manager.TCON이미지쓰기(byteArray, X, Y, W, H);

							//ColorPalette colorPalette = grayBitmap.Palette;
							//for (int i = 0; i < 256; i++)
							//{
							//	colorPalette.Entries[i] = Color.FromArgb(i, i, i);
							//}
							//grayBitmap.Palette = colorPalette;
							//grayBitmap.Save("fast_gray.png", ImageFormat.Png);
							//System.Diagnostics.Process.Start("fast_gray.png");
						}
					}
				
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
		private static unsafe void SaveCaptureToFile(Bitmap grayBitmap)
		{
			//grayBitmap.Save("plain.png");
			//File.WriteAllBytesAsync("plain.png", byteArray);
			var ms = new MemoryStream();
			grayBitmap.Save(ms, ImageFormat.Png);
			File.WriteAllBytesAsync($"capture_test_{imageId}.png", ms.ToArray());
			imageId++;
		}



		#endregion

		private void speakBtn1_Click(object sender, RoutedEventArgs e)
		{
            if (tts_ms.InitializeTTS())
            {
				tts_ms.Play("123M, 456, 7-8");				
            }
		}

		private void speakBtn2_Click(object sender, RoutedEventArgs e)
		{
			//if (tts.InitializeTTS())
			{
				PlayTTS("123M, 456, 7-8");
			}
		}

	}
}

