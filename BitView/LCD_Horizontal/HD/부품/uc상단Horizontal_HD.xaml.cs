using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
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
using Point = System.Windows.Point;
using Size = System.Windows.Size;
using System.Security.Policy;
//using System.Windows.Forms;

namespace BitView
{
	/// <summary>
	/// ucENV2Panel.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class uc상단Horizontal_HD : UserControl
	{
		public uc상단Horizontal_HD()
		{
			InitializeComponent();

			this.FontFamily = (System.Windows.Media.FontFamily)FindResource(DataManager.ConfigInfo.FONTS_NM);
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
					Load환경설정();
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

		System.Windows.Threading.DispatcherTimer _dt시간 = null;

		private void InitProc()
		{
			try
			{
				//edgar
				//string url = string.Format(@"pack://application:,,,/SSResource;component/images/EPD/날씨/{0}.png", "비");//비 맑음 흐림  "흐리고 비"
				//BitmapImage Img = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
				//img오늘날씨.Source = Img;
				//img오늘날씨.Stretch = Stretch.Fill;
				battery.Visibility = SettingsManager.GetTbSystemSettings().IS_SOLAR_MODEL ? Visibility.Visible : Visibility.Hidden;


				SSENV2Manager.On상태정보Event += SSENV2Manager_On상태정보Event;
				if (_dt시간 == null)
				{
					_dt시간 = new System.Windows.Threading.DispatcherTimer();
					_dt시간.Interval = TimeSpan.FromSeconds(1);
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

		private void SSENV2Manager_On상태정보Event(SSNS_ENV2_STATE data)
		{
			Application.Current.Dispatcher.BeginInvoke(() =>
			{
				batteryPercentText.Text = data.BattPercent.ToString();
				batteryFillRectangle.Width = Math.Round(data.BattPercent * 1.3); //edgar rectangle width is 130 so 1% is 1.3
			});
		}

		private void Load환경설정()
		{
			try
			{
				Refresh설치장소();
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
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		bool differTime = true;
		int refrashTime = 0;
		int oldHour = 0;
		int oldMinute = 0;
		private void _dt시간_Tick(object sender, EventArgs e)
		{
			try
			{
				if (Convert.ToInt32(_dt시간.Tag) == 1) return;
				_dt시간.Tag = 1;

				if (differTime)
				{
					differTime = false;
					DisplayDateTime();
				}
				else
				{
					DateTime dt = DateTime.Now;
					if (dt.Hour != oldHour)
					{
						oldHour = dt.Hour;
						TopDoSomething();
					}
					else
					{
						if (dt.Minute != oldMinute)
						{
							oldMinute = dt.Minute;
							differTime = true;
						}
					}

				}
				if (++refrashTime > 10)
				{
					refrashTime = 0;
					WeatherDoSomething();
				}

				_dt시간.Tag = 0;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				_dt시간.Tag = 0;
			}
		}

		private void DisplayDateTime()
		{	
			try
			{
				DateTime dt = DateTime.Now;
				System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ko-KR");
				txt년.Text = dt.ToString("yyyy년");
				txt월일.Text = dt.ToString("MM월 dd일");
				//txt요일.Text = dt.ToString("ddd", ci).ToUpper();
				txt시간.Text = string.Format("{0} {1:d2}:{2:d2}", dt.ToString("tt ", ci), dt.Hour > 12 ? dt.Hour - 12 : dt.Hour, dt.Minute);

				//edgar dispatch removed
				//txt시간.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => NoWaitDoSomething()));

				Task.Run(() => { NoWaitDoSomething(); });
				Task.Run(() => { TopDoSomething(); });
				//TopDoSomething();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		//int idxFn = 0;
		private void NoWaitDoSomething()
		{
			//This will get called after the UI is complete rendering
			CommonUtils.WaitTime(50, true);

			//string name = "시간"; name += idxFn++;
			//MainWindow.CaptureScreenVertical(name, new Rect(568, 50, 144, 40));
			if (MainWindow.startUpFine)
			{
				//MainWindow.CaptureScreenVertical(new Rect(5, 60, 705, 40));   // 글자부분만 //edgar changed time, need to make size smaller capture
				
				MainWindow.CaptureScreenHorizontal(new Rect(664, 0, 56, 1280)); //take all box including header part
			}
				
		}

		public void Refresh설치장소()
		{
			try
			{
				txt정류소.Text = string.Format("{0}", BITDataManager.정류장명);
				txt모바일번호.Content = string.Format("정류장ID {0}", BITDataManager.정류장모바일번호);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}


		private void Clear날씨정보()
		{
			try
			{
				//< Image x: Name = "img오늘날씨" Width = "113" Height = "86" Source = "/SSResource;component/images/날씨/맑음.png"
				//img오늘날씨.Source = null;
				//txt오늘기온.Text = "";

				//img내일날씨.Source = null;
				//txt내일기온.Text = "";

				//edgar clear dust info
				//txtFineDust.Text = "";
				//txtUltraFineDust.Text = "";

				GC.Collect();
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
				Clear날씨정보();

				if (item != null)
				{
					//string weather = "";
					//switch (item[0].RAIN)
					//{
					//	case 1: weather = "맑음"; break;
					//	case 3: weather = "구름많음"; break;
					//	case 5: weather = "흐리고 비"; break; // case 5: weather = "흐리고 가끔비"; break;
					//	//edgar need to get sky codes from raon
					//	default: weather = "흐림"; break;
					//}
					//string URL = string.Format(@"pack://application:,,,/SSResource;component/images/EPD/날씨/{0}.png", weather);
					////< Image x: Name = "img오늘날씨" Width = "113" Height = "86" Source = "/SSResource;component/images/날씨/맑음.png"
					//BitmapImage bImg = new BitmapImage(new Uri(URL, UriKind.RelativeOrAbsolute));
					//img오늘날씨.Source = bImg;
					//img오늘날씨.Stretch = Stretch.Fill;

					//99°C -99°C
					//txt오늘기온.Text = string.Format("{0}°C {1}°C", item[0].MaxTemp, item[0].MinTemp);

					//edgar display dust info
					//txtFineDust.Text = item[0].FineDust.TextValue;
					//txtUltraFineDust.Text = item[0].UltraFineDust.TextValue;
				}

				WeatherDoSomething();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
		private void WeatherDoSomething()
		{
			CommonUtils.WaitTime(50, true);

		//	if (MainWindow.startUpFine)
			//MainWindow.CaptureScreenVertical(new Rect(520, 0, 200, 110));   // 글자부분만 //edgar no need to take weather pictures
		}
		private void TopDoSomething()
		{
			CommonUtils.WaitTime(50, true);

			//	if (MainWindow.startUpFine)
			//MainWindow.CaptureScreenVertical(new Rect(0, 0, 720, 110));    // 상단화면 //edgar what is this for? capture
		}


		#region 화면 좌표 구하기 - GetScreenPoint(window, visual)

		/// <summary>
		/// 화면 좌표 구하기
		/// </summary>
		/// <param name="window">윈도우</param>
		/// <param name="visual">비주얼</param>
		/// <returns>화면 좌표</returns>
		public Point GetScreenPoint(Window window, Visual visual)
		{
			Point screenPoint = visual.PointToScreen(new Point(0, 0));

			PresentationSource source = PresentationSource.FromVisual(window);

			Point resultPoint = source.CompositionTarget.TransformFromDevice.Transform(screenPoint);

			return resultPoint;
		}

		#endregion

		
	}
}