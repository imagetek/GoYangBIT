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

using SSCommon;
using SSMemory;
using DirectShowLib;

namespace SSWebcamPlayer
{
	/// <summary>
	/// MainWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		public bool ResultDialog { get; set; }

		bool _isLoaded = false;
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				this.Left= SystemParameters.FullPrimaryScreenWidth - this.Width - 10;
				this.Top= SystemParameters.FullPrimaryScreenHeight  - this.Height - 10;

				txt제목.Text = string.Format("SSWebCamPlayer v.{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString());
				if (_isLoaded == false)
				{
					Initialize웹캠();
				}

				mShareWebcam.ReceiveHandler += MShareWebcam_ReceiveHandler;

				if (_dt기능 != null) _dt기능.Start();

				this.WindowState = System.Windows.WindowState.Minimized;
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


		private void Window_Closed(object sender, EventArgs e)
		{
			try
			{
			
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
			}
		}


		System.Windows.Threading.DispatcherTimer _dt기능 = null;
		private void InitProc()
		{
			try
			{
				cmbWebCam1.Items.Clear();
				cmbWebCam2.Items.Clear();
				DsDevice[] device = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
				for (int idx = 0; idx < device.Length; idx++)
				{
					cmbWebCam1.Items.Add(device[idx].Name);
					cmbWebCam2.Items.Add(device[idx].Name);
				}

				cmbCAM_NO1_ROTATE.Items.Clear();
				cmbCAM_NO2_ROTATE.Items.Clear();

				for (int i = 0; i <= 180; i += 90)
				{
					cmbCAM_NO1_ROTATE.Items.Add(string.Format("{0}", i));
					cmbCAM_NO2_ROTATE.Items.Add(string.Format("{0}", i));
				}

				cmbCAM_NO1_ROTATE.SelectedIndex = 0;
				cmbCAM_NO2_ROTATE.SelectedIndex = 0;

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

		#region ShareMemory

		//SSMemory.SharedMemory mShare = new SSMemory.SharedMemory(AppConfig.APPS_CD_KIOSK, 4096);
		//SSMemory.SharedMemory mShare = new SSMemory.SharedMemory("BIT", 4096);
		SharedMemory mShareWebcam= new SharedMemory("BITWEBCAM", 4096);

		private void MShareWebcam_ReceiveHandler(byte[] item)
		{
			try
			{
				string msg = Encoding.UTF8.GetString(item);
				Console.WriteLine(msg);
				msg = msg.Replace('\0', ' ');
				msg = msg.Trim();
				TraceManager.AddWebCamLog(string.Format("[IPC수신] BITViewer→WebCam :{0}", msg));
				switch (msg)
				{
					case "EXIT":
						System.Windows.Forms.Application.ExitThread();
						System.Environment.Exit(0);
						break;

					case "SHOCK":
						ShockEvent();
						//btn충격영상_Click(btn충격영상, null);
						break;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}		

		#endregion

		void Initialize웹캠()
		{
			try
			{
				InitProc();

				Load환경설정();

				Load자동실행();

				if (_dt기능 != null) _dt기능.Start();
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
				var config = DataManager.BitSystem;

				if (config.CAM_NO1 >= 0)
				{
					cmbWebCam1.SelectedIndex = config.CAM_NO1;
				}

				if (config.CAM_NO2 >= 0)
				{
					cmbWebCam2.SelectedIndex = config.CAM_NO2;
				}

				cmbCAM_NO1_ROTATE.SelectedIndex = config.CAM_NO1_ROTATE;
				cmbCAM_NO2_ROTATE.SelectedIndex = config.CAM_NO2_ROTATE;
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
				if (cmbWebCam1.SelectedValue != null)
				{
					//EventManager.DisplayLog(Log4Level.Info, string.Format("[화면캡쳐용] 1번카메라를 자동으로 연결합니다."));
					btn1번연결_Click(btn1번연결, null);
				}

				if (cmbWebCam2.SelectedValue != null)
				{
					//EventManager.DisplayLog(Log4Level.Info, string.Format("[충격용] 2번카메라를 자동으로 연결합니다."));

					btn2번연결_Click(btn2번연결, null);
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
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
				TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
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
				base.OnKeyDown(e);
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
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
				TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
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
				TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
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
				TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
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
				TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
			}
		}

		System.Drawing.Rectangle? _prevExtRect = null;


		#endregion

		public void DoFinal()
		{
			try
			{
				mShareWebcam.ReceiveHandler -= MShareWebcam_ReceiveHandler;

				if (_dt기능 != null)
				{
					_dt기능.Stop();
					_dt기능.Tick -= _dt기능_Tick;
					_dt기능 = null;
				}

				if (webcam1 != null)
				{
					webcam1.DoFinal();
					webcam1 = null;
				}

				if (webcam2 != null)
				{
					webcam2.DoFinal();
					webcam2 = null;
				}

				GC.Collect();
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btn1번연결_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (cmbWebCam1.SelectedIndex < 0) return;				
				webcam1.InitProc(cmbWebCam1.SelectedIndex, cmbCAM_NO1_ROTATE.SelectedIndex);				
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btn2번연결_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (cmbWebCam2.SelectedIndex < 0) return;
				webcam2.InitProc(cmbWebCam2.SelectedIndex, cmbCAM_NO2_ROTATE.SelectedIndex);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btn1번해제_Click(object sender, RoutedEventArgs e)
		{
			try
			{				
				webcam1.Stop();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btn2번해제_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				webcam2.Stop();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void _dt기능_Tick(object sender, EventArgs e)
		{
			try
			{
				if (Convert.ToInt32(_dt기능.Tag) == 1) return;
				_dt기능.Tag = 1;

				if (webcam1 != null && webcam1.IsPlay() == true)
				{
					CaptureWebCam();
				}
				CommonUtils.WaitTime(100, true);
				CapturePCScreen();

				_dt기능.Tag = 0;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				_dt기능.Tag = 0;
			}
		}

		private void CapturePCScreen()
		{
			try
			{
				//string 저장DIR = System.IO.Path.Combine(DataManager.Get컨텐츠DIR(), "Capture");
				string 저장DIR = System.IO.Path.Combine(DataManager.Get캡쳐DIR(), "SCREEN");
				if (System.IO.Directory.Exists(저장DIR) == false) System.IO.Directory.CreateDirectory(저장DIR);

				string filename = string.Format("{0}.jpg", DateTime.Now.ToString("yyyyMMddHHmmss"));

				System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
				System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bm);
				g.CopyFromScreen(0, 0, 0, 0, bm.Size);

				var encoder = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid);
				var encParams = new System.Drawing.Imaging.EncoderParameters() { Param = new[] { new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L) } };
				bm.Save(System.IO.Path.Combine(저장DIR, filename), encoder, encParams);

				//bm.Save(System.IO.Path.Combine(저장DIR, filename));
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void CaptureWebCam()
		{
			try
			{
				if (webcam1 != null)// && webcam.IsPlaying == true)
				{
					string 저장DIR = System.IO.Path.Combine(DataManager.Get캡쳐DIR(), "WEBCAM");
					if (System.IO.Directory.Exists(저장DIR) == false) System.IO.Directory.CreateDirectory(저장DIR);

					ImageSource img = webcam1.GetSnapshot();
					if (img != null)
					{
						System.Drawing.Image img저장 = ImageUtils.ConvertImageSourceToImage(img);
						if (img저장 != null)
						{
							string filename = string.Format("{0}.jpg", DateTime.Now.ToString("yyyyMMddHHmmss"));

							var encoder = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid);
							var encParams = new System.Drawing.Imaging.EncoderParameters()
							{
								Param = new[]
							{ new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L) }
							};
							img저장.Save(System.IO.Path.Combine(저장DIR, filename), encoder, encParams);
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

		private void btn충격영상_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				ShockEvent();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		//public delegate void 영상저장Handler(string dir, string _fileName, int nShockValue);
		//public event 영상저장Handler On영상저장Event;
		bool RecordShockYN = false;
		public void ShockEvent()
		{
			try
			{
				this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate ()
				{
					if (RecordShockYN == true)
					{
						TraceManager.AddWebCamLog(string.Format("충격영상저장중이라 처리할수 없습니다."));
						return;
					}
					RecordShockYN = true;
					btn충격영상.IsEnabled = false;
					string 저장DIR = System.IO.Path.Combine(DataManager.Get캡쳐DIR(), "SHOCK");
					if (System.IO.Directory.Exists(저장DIR) == false)
					{
						System.IO.Directory.CreateDirectory(저장DIR);
						TraceManager.AddWebCamLog(string.Format("충격영상 저장폴더를 생성합니다. {0}", 저장DIR));
					}
					string filename = string.Format("{0}.mp4", DateTime.Now.ToString("yyyyMMddHHmm"));
					webcam2.StartRecod(System.IO.Path.Combine(저장DIR, filename));
					TraceManager.AddWebCamLog(string.Format("[SHOCK] 저장시작 : {0}", System.IO.Path.Combine(저장DIR, filename)));
					System.Windows.Threading.DispatcherTimer _dt녹화 = new System.Windows.Threading.DispatcherTimer();
					_dt녹화.Interval = TimeSpan.FromSeconds(30);
					_dt녹화.Tick += delegate (object ds, EventArgs de)
					{
						_dt녹화.Stop();
						TraceManager.AddWebCamLog(string.Format("[SHOCK] 저장완료 : {0}", filename));

						webcam2.StopRecod();

						RecordShockYN = false;
						btn충격영상.IsEnabled = true;
					};
					_dt녹화.Start();
					
				});
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
	}
}
