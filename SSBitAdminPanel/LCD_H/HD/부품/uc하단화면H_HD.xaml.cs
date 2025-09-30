using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using SSCommonNET;
using SSData;
using SSData.MQTT;
using Path = System.IO.Path;

namespace SSBitUI
{
	/// <summary>
	/// ucENV2Panel.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class uc하단화면H_HD : UserControl
	{
		int promotionInterval = 45;
		MQTTService mqttService = new MQTTService();
		

		public uc하단화면H_HD()
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

		System.Windows.Threading.DispatcherTimer _dt확인 = null;

		System.Windows.Threading.DispatcherTimer promotionDispatcher = null;

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

				if (_dt확인 == null)
				{
					_dt확인 = new System.Windows.Threading.DispatcherTimer();
					_dt확인.Tick += _dt확인_Tick;
				}
				_dt확인.Interval = TimeSpan.FromSeconds(60);
				_dt확인.Tag = 0;


                if (promotionDispatcher == null)
                {
					promotionDispatcher = new System.Windows.Threading.DispatcherTimer();
					promotionDispatcher.Tick += PromotionDispatcher_Tick;
                }
				promotionDispatcher.Interval = TimeSpan.FromSeconds(promotionInterval);
				promotionDispatcher.Start();
				
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

		private void PromotionDispatcher_Tick(object sender, EventArgs e)
		{
			try
			{
				RefreshPromotionImages();
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
				if (_dt확인 != null)
				{
					_dt확인.Stop();
					_dt확인.Tick -= _dt확인_Tick;
					_dt확인 = null;
				}

                if (promotionDispatcher != null)
                {
					promotionDispatcher.Stop();
					promotionDispatcher.Tick -= PromotionDispatcher_Tick;
					promotionDispatcher = null;
				}

                // txtplayer.Stop();

                GC.Collect();
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void _dt확인_Tick(object sender, EventArgs e)
		{
			try
			{
				if (Convert.ToInt32(_dt확인.Tag) == 1) return;
				_dt확인.Tag = 1;

				//if (items뉴스 == null || items뉴스.Count == 0)
				//{
				//	Refresh뉴스정보();
				//}

				_dt확인.Tag = 0;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				_dt확인.Tag = 0;
			}
		}

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


		int currentImageIndex = 0;
		public void RefreshPromotionImages()
		{
			string fileName = "image_data.json";
			string promotionFolder = Path.Combine(Directory.GetCurrentDirectory(), "Promotion");

			//var images = Directory.GetFiles(promotionFolder);

			string path = Path.Combine(promotionFolder, fileName);

            if (!File.Exists(path))
            {
				return;
            }

            var imageData = JsonSerializer.Deserialize<List<ImageData>>(File.ReadAllText(path));

			List<ImageData> missingImages = [];

            foreach (var item in imageData)
            {
                if (!File.Exists(Path.Combine(promotionFolder, item.ImageName)))
                {
					missingImages.Add(item);
                }
            }

            if (missingImages.Count > 0)
            {
				MQTTService.DownLoadMissingImages(missingImages);
            }

			DeleteOldImages(imageData, promotionFolder);

            FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
			if (imageData.Count > currentImageIndex)
            {
				try
				{
					if (File.Exists(Path.Combine(promotionFolder, imageData[currentImageIndex].ImageName)))
					{
						var image = File.ReadAllBytes(Path.Combine(promotionFolder, imageData[currentImageIndex].ImageName));
						this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate
						{
							newFormatedBitmapSource.BeginInit();
							newFormatedBitmapSource.Source = new BitmapImage(new Uri(string.Format(Path.Combine(promotionFolder, imageData[currentImageIndex].ImageName)), UriKind.RelativeOrAbsolute));
							newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray4;
							newFormatedBitmapSource.EndInit();
							promotionImage.Source = newFormatedBitmapSource;
						}));

						PromotionDoSomething();
						
					}
					currentImageIndex++;
				}
				catch (Exception ee)
				{
					System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				}
                
                
			} 
			else 
            {
                currentImageIndex = 0;
            }
            
        }

		private void DeleteOldImages(List<ImageData> imageData, string promotionFolder)
		{
			var files = Directory.GetFiles(promotionFolder);
			List<string> filesToDelete = [];
			foreach (var file in files) 
			{
				string fileName = Path.GetFileName(file);
				if (imageData.Any(d => d.ImageName.Equals(fileName, StringComparison.InvariantCultureIgnoreCase) || fileName.Contains("json"))) 
				{ 
					continue;
				} else
				{
					filesToDelete.Add(file);
				}
			}
			filesToDelete.ForEach(File.Delete);
		}

		public void Refresh날씨정보(WeatherData item)
		{
			try
			{
				Clear날씨정보();

				if (item != null)
				{

					FineDustBase.Content = item.FineDust.BaseValue;
					FineDustValue.Content = item.FineDust.ActualValue;
					UltraFineDustBase.Content = item.UltraFineDust.BaseValue;
					UltraFineDustValue.Content = item.UltraFineDust.ActualValue;
					OzoneBase.Content = item.Ozone.BaseValue;
					OzoneValue.Content = item.Ozone.ActualValue;

					//edgar index mapping
					//var a = 0 == i ? "좋음" : 1 == i ? "보통" : 2 == i ? "나쁨" : 3 == i ? "매우나쁨" : null;
					FineDustIndex.Content = GetWeatherIndexText(item.FineDust.Index);
					UltraFineDustIndex.Content = GetWeatherIndexText(item.UltraFineDust.Index);
					OzoneIndex.Content = GetWeatherIndexText(item.Ozone.Index);
				}

				WeatherDoSomething();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private string GetWeatherIndexText(int index)
		{
			return 0 == index ? "좋음" : 1 == index ? "보통" : 2 == index ? "나쁨" : 3 == index ? "매우나쁨" : "";
		}

		private void Clear날씨정보()
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

			//edgar need to change coords later
			//	if (MainWindow.startUpFine)
			MainWindow.CaptureScreen(new Rect(545, 1025, 160, 240));   // 글자부분만 //edgar weather capture need to double check coords full box: 495, 945, 215, 320
		}

		private void PromotionDoSomething()
		{
			CommonUtils.WaitTime(50, true);

			//edgar need to change coords later
			//	if (MainWindow.startUpFine)
			MainWindow.CaptureScreen(new Rect(10, 945, 480, 320));   // 글자부분만 //edgar promotion capture need to double check coords
		}


	}
}
