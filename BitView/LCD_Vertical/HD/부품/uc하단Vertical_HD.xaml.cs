using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using SSCommonNET;
using SSData;
using SSData.GwachonAPI;
using SSData.MQTT;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BitView
{
	/// <summary>
	/// ucENV2Panel.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class uc하단Vertical_HD : UserControl
	{
		int promotionInterval = BITDataManager.BitENVConfig.ITEM.PromotionRefreshInterval; //45;
		MQTTService mqttService = new MQTTService();

		//weather constants
		public const string WeatherTemperature = "TMP";
		public const string WeatherSkyCode = "SKY";
		public const string WeatherPtyCode = "PTY";

		public uc하단Vertical_HD()
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
				//pjh 하단은 변화없다.	if (_dt확인 != null) _dt확인.Start();
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

		//System.Windows.Threading.DispatcherTimer _dt확인 = null;		

		private void InitProc()
		{
			try
			{
				Version ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
				//txt버전.Text = string.Format("v{0:d2}", ver.Revision);
				//txt버전.Visibility = Visibility.Hidden;

				// txtplayer.FFontSize = 30;// DataManager.ConfigInfo.FONTS_SZ * 2;
				// txtplayer.FFontFamily = (FontFamily)FindResource("KoPubDotum");// (FontFamily)FindResource(DataManager.ConfigInfo.FONTS_NM);
				// txtplayer.Duration = 1000 * 15;

				// Refresh뉴스정보();

				//if (_dt확인 == null)
				//{
				//	_dt확인 = new System.Windows.Threading.DispatcherTimer();
				//	_dt확인.Tick += _dt확인_Tick;
				//}
				//_dt확인.Interval = TimeSpan.FromSeconds(60);
				//_dt확인.Tag = 0;


				//            if (promotionDispatcher == null)
				//            {
				//	promotionDispatcher = new System.Windows.Threading.DispatcherTimer();
				//	promotionDispatcher.Tick += PromotionDispatcher_Tick;
				//            }
				//promotionDispatcher.Interval = TimeSpan.FromMinutes(promotionInterval);
				//RefreshPromotionImages();
				//promotionDispatcher.Start();

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

		//private void PromotionDispatcher_Tick(object sender, EventArgs e)
		//{
		//	try
		//	{
		//		RefreshPromotionImages();
		//	}
		//	catch (Exception ee)
		//	{
		//		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//	}

		//}

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

		//public void DoFinal()
		//{
		//	try
		//	{
		//		if (_dt확인 != null)
		//		{
		//			_dt확인.Stop();
		//			_dt확인.Tick -= _dt확인_Tick;
		//			_dt확인 = null;
		//		}

		//  //            if (promotionDispatcher != null)
		//  //            {
		//		//	promotionDispatcher.Stop();
		//		//	promotionDispatcher.Tick -= PromotionDispatcher_Tick;
		//		//	promotionDispatcher = null;
		//		//}

		//              // txtplayer.Stop();

		//              GC.Collect();
		//	}
		//	catch (Exception ee)
		//	{
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//	}
		//}

		//private void _dt확인_Tick(object sender, EventArgs e)
		//{
		//	try
		//	{
		//		if (Convert.ToInt32(_dt확인.Tag) == 1) return;
		//		_dt확인.Tag = 1;				

		//		_dt확인.Tag = 0;
		//	}
		//	catch (Exception ee)
		//	{
		//		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		_dt확인.Tag = 0;
		//	}
		//}

		private void UserControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				Point rpt = e.GetPosition(this);
				if ((rpt.X >= 0 && rpt.X <= 75) && (rpt.Y >= 0 && rpt.Y <= 75))//this.ActualHeight)) /*0 && rpt.Y <= 75) ) //*/
				{
					if (_dtResetClickCount == null)
					{
						_dtResetClickCount = new System.Windows.Threading.DispatcherTimer();
						_dtResetClickCount.Tag = 0;
						_dtResetClickCount.Interval = TimeSpan.FromMilliseconds(1000);
						_dtResetClickCount.Tick += _dtResetClickCount_Tick;
					}
					_dtResetClickCount.Start();

					_ClickCount++;
					if (_ClickCount >= 3) //설정 화면 표시
					{
						_ClickCount = 0;

						if (_dtResetClickCount != null) _dtResetClickCount.Stop();

						if (On환경설정Event != null) On환경설정Event();
					}
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
			}
		}

		public delegate void 환경설정Handler();
		public event 환경설정Handler On환경설정Event;

		int _ClickCount = 0;
		System.Windows.Threading.DispatcherTimer _dtResetClickCount = null;
		void _dtResetClickCount_Tick(object sender, EventArgs e)
		{
			try
			{
				_ClickCount = 0;
				_dtResetClickCount.Stop();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
			}
		}

		public void RefreshWeather(GwacheonWeatherResponse weatherItems)
		{
			try
			{
				ClearWeather();

				if (weatherItems != null)
				{
					//Application.Current.Dispatcher.BeginInvoke(() =>
					//{

					List<WeatherItem> item = weatherItems.Response.Body.Items.Item;

					DateTime date = DateTime.Now;

					WeatherData todayWeatherData = PrepareWeatherData(item, 0, date);
					WeatherData dayOneWeatherData = PrepareWeatherData(item, 1, date);
					WeatherData dayTwoWeatherData = PrepareWeatherData(item, 2, date);
					WeatherData dayThreeWeatherData = PrepareWeatherData(item, 3, date);

					PopulateWeatherDataForToday(todayWeatherData);
					PopulateWeatherDataForDayOne(date, dayOneWeatherData);
					PopulateWeatherDataForDayTwo(date, dayTwoWeatherData);
					PopulateWeatherDataForDayThree(date, dayThreeWeatherData);
					//});

				}

				WeatherDoSomething();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void PopulateWeatherDataForDayThree(DateTime date, WeatherData dayThreeWeatherData)
		{
			FormatConvertedBitmap daythreenewFormatedBitmapSource = new FormatConvertedBitmap();
			daythreenewFormatedBitmapSource.BeginInit();
			daythreenewFormatedBitmapSource.Source = dayThreeWeatherData.WeatherImage;// new BitmapImage(new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/날씨/cloud.png"), UriKind.RelativeOrAbsolute));
			daythreenewFormatedBitmapSource.DestinationFormat = PixelFormats.Gray8;
			daythreenewFormatedBitmapSource.EndInit();

			dayThreeImage.Source = daythreenewFormatedBitmapSource;
			dayThreeHighTemp.Content = $"{dayThreeWeatherData.MaxTemp} °C";
			dayThreeLowTemp.Content = $"{dayThreeWeatherData.MinTemp} °C";
			dayThreeDay.Content = GetKoreanDayOfWeek(date.AddDays(3));
		}

		private void PopulateWeatherDataForDayTwo(DateTime date, WeatherData dayTwoWeatherData)
		{
			FormatConvertedBitmap daytwonewFormatedBitmapSource = new FormatConvertedBitmap();
			daytwonewFormatedBitmapSource.BeginInit();
			daytwonewFormatedBitmapSource.Source = dayTwoWeatherData.WeatherImage; //new BitmapImage(new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/날씨/rain.png"), UriKind.RelativeOrAbsolute));
			daytwonewFormatedBitmapSource.DestinationFormat = PixelFormats.Gray8;
			daytwonewFormatedBitmapSource.EndInit();

			dayTwoImage.Source = daytwonewFormatedBitmapSource;
			dayTwoHighTemp.Content = $"{dayTwoWeatherData.MaxTemp} °C";
			dayTwoLowTemp.Content = $"{dayTwoWeatherData.MinTemp} °C";
			dayTwoDay.Content = GetKoreanDayOfWeek(date.AddDays(2));
		}

		private void PopulateWeatherDataForDayOne(DateTime date, WeatherData dayOneWeatherData)
		{
			FormatConvertedBitmap dayonenewFormatedBitmapSource = new FormatConvertedBitmap();
			dayonenewFormatedBitmapSource.BeginInit();
			dayonenewFormatedBitmapSource.Source = dayOneWeatherData.WeatherImage; //new BitmapImage(new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/날씨/sun.png"), UriKind.RelativeOrAbsolute));
			dayonenewFormatedBitmapSource.DestinationFormat = PixelFormats.Gray8;
			dayonenewFormatedBitmapSource.EndInit();

			dayOneImage.Source = dayonenewFormatedBitmapSource;
			dayOneHighTemp.Content = $"{dayOneWeatherData.MaxTemp} °C";
			dayOneLowTemp.Content = $"{dayOneWeatherData.MinTemp} °C";
			dayOneDay.Content = GetKoreanDayOfWeek(date.AddDays(1));
		}

		private void PopulateWeatherDataForToday(WeatherData todayWeatherData)
		{
			tempValue.Content = $"{todayWeatherData.CurrentAirTemperature} °C";
			todayHighTemp.Content = $"{todayWeatherData.MaxTemp} °C";
			todayLowTemp.Content = $"{todayWeatherData.MinTemp} °C";
			weatherText.Content = todayWeatherData.WeatherText;
			//todayWeatherImage.Source = weatherData.WeatherImage;

			FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
			newFormatedBitmapSource.BeginInit();
			newFormatedBitmapSource.Source = todayWeatherData.WeatherImage;
			newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray8;
			newFormatedBitmapSource.EndInit();

			//txt노선종류.Text = 노선종류;
			todayWeatherImage.Source = newFormatedBitmapSource;
		}

		private static string GetKoreanDayOfWeek(DateTime dateNow)
		{
			return (dateNow.DayOfWeek) switch
			{
				DayOfWeek.Sunday => "일요일",
				DayOfWeek.Monday => "월요일",
				DayOfWeek.Tuesday => "화요일",
				DayOfWeek.Wednesday => "수요일",
				DayOfWeek.Thursday => "목요일",
				DayOfWeek.Friday => "금요일",
				DayOfWeek.Saturday => "토요일"
			};
		}

		private WeatherData PrepareWeatherData(IEnumerable<WeatherItem> data, int day, DateTime date)
		{
			var dateString = GetDateString(date, day);
			var filteredData = GetDataForDate(data, dateString);
			var todayTemp = filteredData.Where(d => d.Category.Equals(WeatherTemperature)).Select(t => t.FcstValue);
			var maxMinTemp = GetMaxMinTemp(todayTemp);
			
			WeatherData weatherData = new()
			{
				CurrentAirTemperature = filteredData.FirstOrDefault(w => w.Category.Equals(WeatherTemperature))?.FcstValue,
				MaxTemp = maxMinTemp.Item1,
				MinTemp = maxMinTemp.Item2
			};
			var pty = day == 0 ? filteredData.FirstOrDefault(w => w.Category.Equals(WeatherPtyCode))?.FcstValue : GetAverageValue(filteredData, WeatherPtyCode);
			var sky = day == 0 ? filteredData.FirstOrDefault(w => w.Category.Equals(WeatherSkyCode))?.FcstValue : GetAverageValue(filteredData, WeatherSkyCode);

			PopulateWeatherTextAndImage(weatherData, pty, sky);
			return weatherData;
		}

		private string GetAverageValue(IEnumerable<WeatherItem> filteredData, string filterCode)
		{
			return filteredData.Where(w => w.Category.Equals(filterCode))
				.Select(r => r.FcstValue)
				.GroupBy(v => v)
				.OrderByDescending(g => g.Count())
				.First()
				.Key;
		}

		private static IEnumerable<WeatherItem> GetDataForDate(IEnumerable<WeatherItem> item, string date)
		{
			return item.Where(i => i.FcstDate.Equals(date));
		}

		private static string GetDateString(DateTime date, int day)
		{
			return date.AddDays(day).ToString("yyyyMMdd");
		}

		private Tuple<int, int> GetMaxMinTemp(IEnumerable<string> todayTemp)
		{
			var numberTemp = todayTemp.ToList().Select(t => int.Parse(t)).OrderDescending();
			var maxTemp = numberTemp.First();
			var minTemp = numberTemp.Last();

			return new(maxTemp, minTemp);
		}

		private static void PopulateWeatherTextAndImage(WeatherData weatherData, string pty, string sky)
		{
			//new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/날씨/rain.png"), UriKind.RelativeOrAbsolute)
			string weatherText = null;
			BitmapSource weatherImage = null;
			try
			{
				if (pty == "0")
				{
					//sun
					switch (sky)
					{
						case "1":
							weatherText = "맑음";
							weatherImage = new BitmapImage(new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/날씨/sun.png"), UriKind.RelativeOrAbsolute));
							break;
						case "3":
							weatherText = "구름많음";
							weatherImage = new BitmapImage(new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/날씨/cloud.png"), UriKind.RelativeOrAbsolute));
							break;
						case "4":
							weatherText = "흐림";
							weatherImage = new BitmapImage(new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/날씨/sun_cloud.png"), UriKind.RelativeOrAbsolute));
							break;
					}
				}
				else if (pty == "3")
				{
					//snow
					weatherText = "눈";
					weatherImage = new BitmapImage(new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/날씨/snow.png"), UriKind.RelativeOrAbsolute));
				}
				else
				{
					//rain
					weatherText = "비";
					weatherImage = new BitmapImage(new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/날씨/rain.png"), UriKind.RelativeOrAbsolute));
				}

				weatherData.WeatherText = weatherText;
				weatherData.WeatherImage = weatherImage;
			}
			catch (Exception ex)
			{

				var err = ex.Message;
			}

		}

		public void RefreshAirQuality(AirQualityData item)
		{
			try
			{
				ClearAirQuality();

				if (item != null)
				{

					FineDustBase.Content = item.FineDust.BaseValue;
					FineDustValue.Content = item.FineDust.ActualValue;
					UltraFineDustBase.Content = item.UltraFineDust.BaseValue;
					UltraFineDustValue.Content = item.UltraFineDust.ActualValue;
					OzoneBase.Content = item.Ozone.BaseValue;
					OzoneValue.Content = item.Ozone.ActualValue;

					FineDustIndex.Text = GetWeatherIndexText(item.FineDust.Index);
					UltraFineDustIndex.Text = GetWeatherIndexText(item.UltraFineDust.Index);
					OzoneIndex.Text = GetWeatherIndexText(item.Ozone.Index);
				}

				WeatherDoSomething();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private static string GetWeatherIndexText(int index)
		{
			return 1 == index ? "좋음" : 2 == index ? "보통" : 3 == index ? "나쁨" : 4 == index ? "매우나쁨" : "";
		}

		private void ClearWeather()
		{
			try
			{
				tempValue.Content = string.Empty;
				todayHighTemp.Content = string.Empty;
				todayLowTemp.Content = string.Empty;
				weatherText.Content = string.Empty;
				todayWeatherImage.Source = null;


				dayOneImage.Source = null;
				dayOneHighTemp.Content = string.Empty;
				dayOneLowTemp.Content = string.Empty;
				dayOneDay.Content = string.Empty;

				dayTwoImage.Source = null;
				dayTwoHighTemp.Content = string.Empty;
				dayTwoLowTemp.Content = string.Empty;
				dayTwoDay.Content = string.Empty;

				dayThreeImage.Source = null;
				dayThreeHighTemp.Content = string.Empty;
				dayThreeLowTemp.Content = string.Empty;
				dayThreeDay.Content = string.Empty;

				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void ClearAirQuality()
		{
			try
			{
				FineDustBase.Content = "";
				FineDustValue.Content = "";
				UltraFineDustBase.Content = "";
				UltraFineDustValue.Content = "";
				OzoneBase.Content = "";
				OzoneValue.Content = "";

				GC.Collect();
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

			if (uc버스정보Vertical_HD.PromotionPart?.Visibility == Visibility.Visible)
			{
				MainWindow.CaptureScreenVertical(new Rect(0, 895, 720, 385)); //now update all bottom, due to issues during cold weather
			}

			//edgar need to change coords later
			//	if (MainWindow.startUpFine)
			//just weather box
			//MainWindow.CaptureScreenVertical(new Rect(545, 1025, 160, 240));   // 글자부분만 //edgar weather capture need to double check coords full box: 495, 945, 215, 320

			
		}

		//private void PromotionDoSomething()
		//{
		//	CommonUtils.WaitTime(50, true);

		//	//edgar need to change coords later
		//	//	if (MainWindow.startUpFine)
		//	MainWindow.CaptureScreenVertical(new Rect(10, 945, 480, 320));   // 글자부분만 //edgar promotion capture need to double check coords
		//}


	}
}
