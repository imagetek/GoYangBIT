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
using SSData.GwachonAPI;
using static SSData.ClientEngine;

namespace BitView
{
	public partial class uc버스정보Vertical_HD : UserControl
	{
		public uc버스정보Vertical_HD()
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

		public void Refresh도착정보(List<BisArrivalInformation> items, uc하단Vertical_HD _u하단 = null, uc곧도착Vertical_HD _u도착 = null)
		{
			try
			{

				if (PromotionPart is null && _u하단 is not null)
				{
					PromotionPart = _u하단;
				}

				if (arriveSoonPart is null && _u도착 is not null)
				{
					arriveSoonPart = _u도착;
				}

				items정보 ??= new List<BisArrivalInformation>();
				items정보.Clear();

				//idxNo = 0; //edgar removed this to keep showing  correct page when new data arrives, otherwise when new data arrives page 0 will be shown
				items정보.AddRange(items); //edgar

				if (items정보 != null && items정보.Count > 0)
				{
					Refresh버스정보창(LCD화면Type.BIS정보);

					if (items정보.Count <= BITDataManager.BitConfig.MAX_ROUTE_COUNT && BITDataManager.BitConfig.SHOW_BUS_ROUTE)
					{
						TotalPage수 = items정보.Count;
					}
					else
					{
						bool 나머지YN = items정보.Count % items수 > 0;
						TotalPage수 = items정보.Count / items수 + (나머지YN == true ? 1 : 0);
					}
					//idxNo = 0; //edgar added for test
					//Display도착정보(); // edgar, removed this to keep showing pages from dispatcher timer. otherwise when new data arrives current displayed page will be refreshed immediatelly, and not after timer

					if (TotalPage수 > 1)
					{
						if ((bool)!_dt이동?.IsEnabled)
						{
							_dt이동?.Start();
						}						
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

							if (BITDataManager.BitConfig.SHOW_BUS_ROUTE && items정보.Count <= BITDataManager.BitConfig.MAX_ROUTE_COUNT)
							{
								
								busAndRoute.Visibility = Visibility.Visible;
								sp정보.Visibility = Visibility.Collapsed;
							}
							else
							{
								busAndRoute.Visibility = Visibility.Collapsed;
								sp정보.Visibility = Visibility.Visible;
							}
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

					if (busAndRoute.Visibility == Visibility.Visible)
					{
						busAndRoute.Visibility = Visibility.Collapsed;
					}
					
					item정보old = null;

					//edgar refresh bus arrival
					idxNo = 0;
					if ((bool)_dt이동?.IsEnabled)
					{
						_dt이동?.Stop();
					}				

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
				if (items정보.Count <= BITDataManager.BitConfig.MAX_ROUTE_COUNT && BITDataManager.BitConfig.SHOW_BUS_ROUTE)
				{
					busArrival.Clear도착정보();
					busArrival.Visibility = Visibility.Hidden;
				}
				else
				{
					foreach (UIElement ctrl in sp정보.Children)
					{
						uc도착정보VerticalItem_HD item = ctrl as uc도착정보VerticalItem_HD;
						if (item == null) continue;
						item.Clear도착정보();
						item.Visibility = Visibility.Hidden;
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

		BisArrivalInformation item정보old = null;	// 비교용, 첫줄
		uc도착정보VerticalItem_HD ctrl_old = null;     // 마지막 유효 obj
		readonly List<BisArrivalInformation> oldArrivalList = [];
		private void Display도착정보()
		{
			try
			{
				Clear도착정보();

				List<bool> changed = [];

				int S_NO = idxNo * items수;
				int E_NO = (idxNo + 1) * items수;

				if (items정보.Count <= BITDataManager.BitConfig.MAX_ROUTE_COUNT && BITDataManager.BitConfig.SHOW_BUS_ROUTE)
				{
					BisArrivalInformation item정보 = items정보[idxNo];
					if (item정보 != null)
					{
						busArrival.Refresh도착정보(item정보);
						busArrival.Visibility = Visibility.Visible;
						DrawRoute(item정보.RouteId);						
					}
				}
				else
				{
					for (int i = 1; i <= items수; i++)
					{
						uc도착정보VerticalItem_HD ctrl = sp정보.FindName(string.Format("_u정보{0}", i)) as uc도착정보VerticalItem_HD;
						if (ctrl == null) continue;
						int SEQ_NO = S_NO + i - 1;
						if (SEQ_NO >= items정보.Count) continue;
						BisArrivalInformation item정보 = items정보[SEQ_NO];
						if (item정보 != null)
						{
							ctrl.Refresh도착정보(item정보);
							ctrl.Visibility = Visibility.Visible;
						
						}
					}
				}
				
            
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
		private void Ctrl_Changed(uc도착정보VerticalItem_HD ctrl)  //ctrl_old
		{
			ctrl.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => DoSomething()));
		}
		private void DoSomething()
		{
			//This will get called after the UI is complete rendering
			CommonUtils.WaitTime(1000, true);
			
			if (MainWindow.startUpFine)
			{
				//MainWindow.CaptureScreenVertical(new Rect(5, 315, 705, 580));   // 글자부분만 //edgar changed bus info capture
				double imageHeight = 660;

				//if (PromotionPart.Visibility == Visibility.Collapsed &&	arriveSoonPart.Visibility == Visibility.Collapsed)
				//{
				//	imageHeight += 380 + 130;
				//}
				if (PromotionPart.Visibility == Visibility.Collapsed)
				{
					imageHeight += 380;
				}
				MainWindow.CaptureScreenVertical(new Rect(0, 235, 720, imageHeight)); //take all box image
			}

		}

		private void SaveCanvasToFile()
		{
			RenderTargetBitmap bmp = new RenderTargetBitmap((int)RouteCanvas.ActualWidth, (int)RouteCanvas.ActualHeight, 96, 96, PixelFormats.Pbgra32);
			bmp.Render(RouteCanvas);

			BitmapEncoder pngEncoder = new PngBitmapEncoder();
			pngEncoder.Frames.Add(BitmapFrame.Create(bmp));

			using (var fs = System.IO.File.OpenWrite("test.png"))
			{
				pngEncoder.Save(fs);
			}
		}

		//Route drawing
		public static uc하단Vertical_HD PromotionPart = null;
		uc곧도착Vertical_HD arriveSoonPart = null;
		private readonly List<Station> busStops = [];
		//private readonly List<int> oldBusLocation = [];
		private readonly List<int> busLocation = [];
		private int columnCount = 4; // You can change this dynamically if needed
		private double columnWidth = 0;


		List<Image> busImages = [];
		private void DrawRoute(int routeId)
		{

			if (TotalPage수 > 1)
			{
				RouteCanvas.Children.Clear();
			}
			else
			{
				busImages.ForEach(RouteCanvas.Children.Remove);
			}

			busStops.Clear();
			busLocation.Clear();
			busImages.Clear();

			busStops.AddRange(GwacheonBusStationService.StationRouteMap.FirstOrDefault(r => r.RouteId == routeId)?.StationList);
			busLocation.AddRange(GwacheonBusStationService.BusLocations.FirstOrDefault(l => l.RouteId == routeId)?.Locations);
			//busStops.AddRange(SiheungStationService.GetStationsForBus(routeId));

			
			//if (busStops.Count > 100)
			//{
			//	RouteCanvas.Height = 980;
			//	PromotionPart.Visibility = Visibility.Collapsed;
			//	arriveSoonPart.Visibility = Visibility.Collapsed;
			//	arriveSoonPart.Height = 0;
			//}
			if (busStops.Count > 70)
			{
				RouteCanvas.Height = 850;
				PromotionPart.Visibility = Visibility.Collapsed;
				arriveSoonPart.Visibility = Visibility.Visible;
				arriveSoonPart.Height = 130;
			}
			else
			{
				RouteCanvas.Height = 470;
				PromotionPart.Visibility = Visibility.Visible;
				arriveSoonPart.Visibility = Visibility.Visible;
				arriveSoonPart.Height = 130;
			}

			double width = RouteCanvas.ActualWidth;
			double height = RouteCanvas.Height;

			if (width == 0 || height == 0 || busStops.Count == 0) return;

			int stopsPerColumn = (int)Math.Ceiling((double)busStops.Count / columnCount);
			double columnSpacing = width / (columnCount);
			double rowSpacing = height / (stopsPerColumn + 1);
			columnWidth = columnSpacing;
			List<Point> stopPositions = [];
			

			int index = 0;

			for (int col = 0; col < columnCount; col++)
			{
				bool isDownColumn = col % 2 == 0;

				for (int row = 0; row < stopsPerColumn && index < busStops.Count; row++)
				{
					int actualRow = isDownColumn ? row : (stopsPerColumn - 1 - row);
					double x = columnSpacing * (col) + 10;
					double y = rowSpacing * (actualRow + 1);
					bool isLastRow = row == stopsPerColumn - 1;

					if (index == 0)
					{
						DrawArrow(isDownColumn, x, y);
					}

					DrawStopDot(x, y, busStops[index], isDownColumn, isLastRow, rowSpacing);

					stopPositions.Add(new Point(x, y));
					index++;
				}
			}

			// Draw lines between the stops
			for (int i = 0; i < stopPositions.Count - 1; i++)
			{
				var line = new Line
				{
					X1 = stopPositions[i].X,
					Y1 = stopPositions[i].Y,
					X2 = stopPositions[i + 1].X,
					Y2 = stopPositions[i + 1].Y,
					Stroke = Brushes.Black,
					StrokeThickness = 3
				};
				RouteCanvas.Children.Add(line);
			}

			busImages.ForEach(i => RouteCanvas.Children.Add(i));

			//SaveCanvasToFile();
		}

		private void DrawArrow(bool isDownColumn, double x, double y)
		{
			if (isDownColumn)
			{
				RotateTransform rotateTransform = new RotateTransform();
				rotateTransform.Angle = 90;
				var arrow = new Path();
				arrow.Stretch = Stretch.Fill;
				arrow.Data = Geometry.Parse("M 0,110 70,110 45,90 75,90 120,120 75,150 45,150 70,130 0,130 Z");
				arrow.Fill = Brushes.Black;
				arrow.Width = 20;
				arrow.Height = 20;
				arrow.RenderTransformOrigin = new Point(0.5, 0.5);
				arrow.RenderTransform = rotateTransform;
				Canvas.SetLeft(arrow, x - 10);
				Canvas.SetTop(arrow, y - 30);
				RouteCanvas.Children.Add(arrow);
			}
		}

		private void RouteCanvas_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var s = sender;
		}

		private void DrawStopDot(double x, double y, Station station, bool isDownColumn, bool isLastRow, double rowSpacing)
		{
			int position = 6;
			double dotLeft = x - position;
			double dotTop = y - position;

			var dot = new Ellipse
			{
				Width = 12,
				Height = 12,
				Fill = Brushes.Black
			};

			
			Canvas.SetLeft(dot, x - position);
			Canvas.SetTop(dot, y - position);
			RouteCanvas.Children.Add(dot);

			

			if (station.StationId == BITDataManager.BitSystem.STATION_ID)
			{
				var extraDot = new Ellipse
				{
					Width = 24,
					Height = 24,
					StrokeThickness = 3,
					Stroke = Brushes.Black
				};

				Canvas.SetLeft(extraDot, x - 12);
				Canvas.SetTop(extraDot, y - 12);
				RouteCanvas.Children.Add(extraDot);
			}

			var text = new TextBlock
			{
				Text = station.StationName,
				FontSize = station.StationId == BITDataManager.BitSystem.STATION_ID ? 16 : 13,
				//TextWrapping = TextWrapping.Wrap,
				Width = columnWidth - 20,
				FontWeight = station.StationId == BITDataManager.BitSystem.STATION_ID ? FontWeights.Bold : FontWeights.Normal,
				//Foreground = busLocation.Contains(station.StationSequence) ? Brushes.Red : Brushes.Black,
				
			};

			double textLeft = station.StationId == BITDataManager.BitSystem.STATION_ID ? 15 : 10;

			Canvas.SetLeft(text, x + 15);
			Canvas.SetTop(text, y - 20);
			RouteCanvas.Children.Add(text);

			if (busLocation.Contains(station.StationSequence))
			{
				if (station.StationSequence == busStops.Count)
				{
					return;
				}
				//var bus = new Rectangle
				//{
				//	Width = 20,
				//	Height = 20,
				//	Fill = Brushes.Red
				//};

				dotLeft = x - 10;
				dotTop = y - 10;

				Image busIcon;
				busIcon = new Image();
				busIcon.Width = 20;
				busIcon.Height = 20;
				FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
				newFormatedBitmapSource.BeginInit();
				newFormatedBitmapSource.Source = new BitmapImage(new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/EPD/버스ICON.png"), UriKind.RelativeOrAbsolute));
				newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray2;
				newFormatedBitmapSource.EndInit();

				//txt노선종류.Text = 노선종류;
				busIcon.Source = newFormatedBitmapSource;
				double leftPosition = isLastRow ? (dotLeft + (columnWidth / 2)) : dotLeft; //if it is last row and last column then draw differently
				double topPosition = isDownColumn ? (dotTop + (rowSpacing / 2)) : (dotTop - (rowSpacing / 2));
				Canvas.SetLeft(busIcon, leftPosition);
				Canvas.SetTop(busIcon, topPosition);
				//RouteCanvas.Children.Add(busIcon);
				busImages.Add(busIcon);
			}

		}
	}
}
