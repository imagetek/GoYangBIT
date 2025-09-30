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
using SSControls;
using SSData;
using DirectShowLib;

namespace PAJUBitView
{
	/// <summary>
	/// ucENV2Panel.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ucWebCamPanel : UserControl
	{
		public ucWebCamPanel()
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

					//Load자동실행();
				}

				Load자동실행();

				if (_dt기능 != null) _dt기능.Start();
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

				for (int i = 0; i < 360; i += 90)
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

		public void Initialize웹캠()
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
				var config = BITDataManager.BitSystem;

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
				//if (config.WEBCAM_NM != null && config.WEBCAM_NM.Equals("") == false)
				//{
				//	cmbWebCam1.SelectedValue = config.WEBCAM_NM;
				//}

				//if (config.SHOCKCAM_NM != null && config.SHOCKCAM_NM.Equals("") == false)
				//{
				//	cmbWebCam2.SelectedValue = config.SHOCKCAM_NM;
				//}
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
					EventManager.DisplayLog(Log4Level.Info, string.Format("[화면캡쳐용] 1번카메라를 자동으로 연결합니다."));

					btn1번연결_Click(btn1번연결, null);
				}

				if (cmbWebCam2.SelectedValue != null)
				{
					//string ComPort = cmbWebCam2.SelectedValue.ToString();
					//if (ComPort.Equals("미사용") == true) return;

					EventManager.DisplayLog(Log4Level.Info, string.Format("[충격용] 2번카메라를 자동으로 연결합니다."));

					btn2번연결_Click(btn2번연결, null);
				}
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

		private void btn1번연결_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (cmbWebCam1.SelectedIndex < 0) return;
				//string webcam = cmbWebCam1.SelectedValue.ToString();
				//DsDevice[] device = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
				//int idxNo = 0;
				//foreach( DsDevice device2 in device)
				//{
				//	if (webcam.Equals(device2.Name)== true)
				//	{
				//		break;
				//	}
				//	idxNo++;
				//}
			//	webcam1.InitProc(cmbWebCam1.SelectedIndex, cmbCAM_NO1_ROTATE.SelectedIndex);
				BITDataManager.bWebCamAciveYN = true;
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
				//string webcam = cmbWebCam2.SelectedValue.ToString();
				//DsDevice[] device = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
				//int idxNo = 0;
				//foreach (DsDevice device2 in device)
				//{
				//	if (webcam.Equals(device2.Name) == true)
				//	{
				//		break;
				//	}
				//	idxNo++;
				//}
				//webcam2.InitProc(cmbWebCam2.SelectedIndex);
			//	webcam2.InitProc(cmbWebCam2.SelectedIndex, cmbCAM_NO2_ROTATE.SelectedIndex);
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
				BITDataManager.bWebCamAciveYN = false;
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
				//ShockEvent();
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
		public void ShockEvent(int nShock)
		{
			try
			{
				if (RecordShockYN == true) return;
				RecordShockYN = true;
				btn충격영상.IsEnabled = false;
				string 저장DIR = System.IO.Path.Combine(DataManager.Get캡쳐DIR(), "SHOCK");
				if (System.IO.Directory.Exists(저장DIR) == false) System.IO.Directory.CreateDirectory(저장DIR);
				string filename = string.Format("{0}.mp4", DateTime.Now.ToString("yyyyMMddHHmm"));

				webcam2.StartRecod(System.IO.Path.Combine(저장DIR, filename));

				System.Windows.Threading.DispatcherTimer _dt녹화 = new System.Windows.Threading.DispatcherTimer();
				_dt녹화.Interval = TimeSpan.FromSeconds(30);
				_dt녹화.Tick += delegate (object ds, EventArgs de)
				{
					_dt녹화.Stop();

					webcam2.StopRecod();

					RecordShockYN = false;
					btn충격영상.IsEnabled = true;

					//if (On영상저장Event != null) On영상저장Event(저장DIR, filename, nShock);
				};
				_dt녹화.Start();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
	}
}
