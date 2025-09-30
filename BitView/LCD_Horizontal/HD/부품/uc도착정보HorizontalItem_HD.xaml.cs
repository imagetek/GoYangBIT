using System;
using System.Collections.Generic;
using System.Globalization;
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
	public partial class uc도착정보HorizontalItem_HD : UserControl
	{
		public uc도착정보HorizontalItem_HD()
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

		private void InitProc()
		{
			try
			{
				//	txt노선번호.SetFont설정("KoPubDotum", 30, "#09273E", 0, 700, 5);
				//	txt노선번호.SetTextAlign(2);
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
				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public void Clear도착정보()
		{
			try
			{
				//버스아이콘(大)
				img구분.Source = null;
				//img저상.Visibility = Visibility.Collapsed;
				//RouteNumber
				//txt노선번호.ClearPlayer();

				//도착예정시간 (남은시간 / 곧도착)
				예정Grid.Visibility = Visibility.Collapsed;
				txt남은시간.Text = "";
				곧도착Grid.Visibility = Visibility.Collapsed;
				//남은좌석수
				//sp좌석정보.Visibility = Visibility.Hidden;
				txt좌석수.Text = "";
				seats.Visibility = Visibility.Hidden;
				txt현위치.Text = "";
				//현위치 (남은정류장 6이하 / 6초과)
				img배경선.Source = null;
				img위치정보.Source = null;

				noOperationStatus.Visibility = Visibility.Collapsed;

				for (int i = 1; i <= 5; i++)
				{
					Image img위치 = sp위치.FindName(string.Format("img버스위치{0}", i)) as Image;
					if (img위치 == null) continue;
					img위치.Source = null;

					TextBlock txt순번 = sp순번.FindName(string.Format("txt정류장순번{0}", i)) as TextBlock;
					if (txt순번 == null) continue;
					txt순번.Text = "";
				}
				img정류장2.Visibility = Visibility.Hidden;

				txt목적지.Text = "";
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public void Refresh도착정보(BisArrivalInformation item)
		{
			try
			{
				Clear도착정보();

				int 남은정류장수 = item.NumberOfRemainingStops;
				if (남은정류장수 <= 5)
				{
					img배경선.Source = new BitmapImage(new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/BIS/위치/실선H.png"), UriKind.RelativeOrAbsolute));
					img배경선.Stretch = Stretch.Fill;
				}
				else
				{
					img배경선.Source = new BitmapImage(new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/BIS/위치/점선H.png"), UriKind.RelativeOrAbsolute));
					img배경선.Stretch = Stretch.Fill;
				}

				UpdateRouteNumberTextBlock(item);

				string 노선종류 = "";
				
				노선종류 = item.RouteType switch
				{
					
					0 => "일반버스",
					2 => "일반버스",
					1 => "광역버스",					
					3 => "마을버스G",
					//gwachon types
					11 => "직행버스",
					12 => "좌석버스",
					13 => "일반버스",
					14 => "광역버스",
					21 => "직행버스",
					22 => "좌석버스",
					23 => "일반버스",
					30 => "마을버스G",
					42 => "좌석버스",
					43 => "일반버스",
					51 => "공항버스",
					52 => "공항버스",
					53 => "공항버스",
					_ => "일반버스",
				};
				
				FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
				newFormatedBitmapSource.BeginInit();
				newFormatedBitmapSource.Source = new BitmapImage(new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/BIS/버스구분/가로/{0}.png", 노선종류), UriKind.RelativeOrAbsolute));
				newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray8;
				newFormatedBitmapSource.EndInit();

				//txt노선종류.Text = 노선종류;
				img구분.Source = newFormatedBitmapSource;
				//switch (item.BusType)
				//{
				//	case 0: img저상.Visibility = Visibility.Hidden; break;
				//	case 1: img저상.Visibility = Visibility.Visible; break;
				//}

				//ItemData item노선 = item.노선번호색상정보;
				//Color color = (Color)ColorConverter.ConvertFromString(노선색상);
				//txt노선번호.Foreground = new SolidColorBrush(color);

				//도착예정시간 (남은시간 / 곧도착)
				bool b곧도착 = BITDataManager.Check곧도착YN(item);
				if (b곧도착 == true)
				{
					예정Grid.Visibility = Visibility.Hidden;
					곧도착Grid.Visibility = Visibility.Visible;
					timeLeftGrid.Visibility = Visibility.Hidden;
					// img위치정보.Source = new BitmapImage(new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/BIS/위치정보/곧도착.png"), UriKind.RelativeOrAbsolute));
				}
				else
				{
					곧도착Grid.Visibility = Visibility.Hidden;
					예정Grid.Visibility = Visibility.Visible;
					if (item.EstimatedTimeOfArrival == 0 && item.NumberOfRemainingStops > 1)
					{
						timeLeftGrid.Visibility = Visibility.Hidden;
					}
					else
					{
						txt남은시간.Text = string.Format("{0}", Math.Ceiling((decimal)item.EstimatedTimeOfArrival / 60)); //edgar, need to round minutes up, not down
						timeLeftGrid.Visibility = Visibility.Visible;
					}
				}

				//남은좌석수
				if (item.NumberOfRemainingSeats == 255)
				{
					txt좌석수.Text = item.BusCongestion switch
					{
						1 => "여유",
						2 => "보통",
						3 => "혼잡",
						4 => "매우혼잡",
						9 => "정보없음",
						_ => "여유",
					};
					txt좌석수.Width = 60;
					txt좌석수.FontSize = CommonUtils.ReduceFontSizeIfNeeded(txt좌석수.Text, txt좌석수);
					txt좌석수.TextAlignment = TextAlignment.Center;
					//sp좌석정보.Visibility = Visibility.Hidden;
					//txt좌석수.Margin = new Thickness(0, 0, 20, 0);
				}
				else
				{
					txt좌석수.Text = $"{item.NumberOfRemainingSeats}";// string.Format("{0}", item.NumberOfRemainingSeats);
					txt좌석수.Width = 45;
					txt좌석수.FontSize = CommonUtils.ReduceFontSizeIfNeeded(txt좌석수.Text, txt좌석수);
					seats.Visibility = Visibility.Visible;
					//txt좌석수.TextAlignment = TextAlignment.Right;
					//txt좌석수.Margin = new Thickness(0, 0, 32, 0);
					//sp좌석정보.Visibility = Visibility.Visible;
				}
				if (txt좌석수.Text =="여유") //pjh too small font bug fix
				{
					txt좌석수.FontSize = 26;
					txt좌석수.TextAlignment = TextAlignment.Center;
				}
				if (txt좌석수.Text == "정보없음") //pjh no display better than "정보없음"
				{
					txt좌석수.Text = "";
				}

				//위치표시
				if (남은정류장수 <= 5)
				{
					img정류장2.Visibility = Visibility.Visible;

					for (int i = 1; i <= 5; i++)
					{
						TextBlock txt순번 = sp순번.FindName(string.Format("txt정류장순번{0}", i)) as TextBlock;
						if (txt순번 != null) txt순번.Text = string.Format("{0}", i);

						if (남은정류장수.Equals(i) == true)
						{
							Image img위치 = sp위치.FindName(string.Format("img버스위치{0}", i)) as Image;
							if (img위치 != null)
							{
								img위치.Source = new BitmapImage(new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/EPD/버스ICON.png"), UriKind.RelativeOrAbsolute));
								txt현위치.Text = item.StopName;    //pjh .s정류장명;
								txt현위치.FontSize = CommonUtils.ReduceFontSizeIfNeeded(item.StopName, txt현위치);
								//if (i == 5)
								//{
								//	if (item.StopName.Length < 8)       //pjh 글자위치 변경
								//		Canvas.SetLeft(txt현위치, 40 * (i - 2));
								//	else
								//		Canvas.SetLeft(txt현위치, 40 * (i - 3));
								//}
								//else
								//{
								//	if (item.StopName.Length < 8)       //pjh 글자위치 변경
								//		Canvas.SetLeft(txt현위치, 40 * (i - 1));
								//	else
								//		Canvas.SetLeft(txt현위치, 40 * (i - 2));
								//}
							}
						}
					}
				}
				else
				{
					txt정류장순번1.Text = "1";
					img정류장2.Visibility = Visibility.Hidden;
					for (int i = 3; i <= 5; i++)
					{
						TextBlock txt순번 = sp순번.FindName(string.Format("txt정류장순번{0}", i)) as TextBlock;
						if (txt순번 != null)
						{
							switch (i)
							{
								case 3: txt순번.Text = string.Format("{0}", 남은정류장수 - 1); break;
								case 4: txt순번.Text = string.Format("{0}", 남은정류장수); break;
								case 5: txt순번.Text = string.Format("{0}", 남은정류장수 + 1); break;
							}
						}
					}
					img버스위치4.Source = new BitmapImage(new Uri(string.Format(@"pack://application:,,,/SSResource;component/images/EPD/버스ICON.png"), UriKind.RelativeOrAbsolute));
					txt현위치.Text = item.StopName;
					//if (item.StopName.Length < 8)       //pjh 글자위치 변경
					//	Canvas.SetLeft(txt현위치, 40 * 4 - item.StopName.Length * 8);
					//else
					//	Canvas.SetLeft(txt현위치, 40 * 4 - item.StopName.Length * 9);
				}
				//if (item.DisplayDestination != 99)   //pjhpjh .행선지표출YN) 항상나오게
				{
					txt목적지.Text = item.DestinationName + " 행";
					txt목적지.FontSize = CommonUtils.ReduceFontSizeIfNeeded($"{item.DestinationName} 행", txt목적지);
				}

                if (item.OperationStatus == 1 || item.OperationStatus == 2)
                {
                    noOperationStatus.Visibility = Visibility.Visible;
					noOperationStatus.Text = GetOperationStatus(item.OperationStatus);
					예정Grid.Visibility = Visibility.Collapsed;
					곧도착Grid.Visibility = Visibility.Collapsed;
					좌석Grid.Visibility = Visibility.Collapsed;
					현위치Grid.Visibility = Visibility.Collapsed;
				} else
				{
					noOperationStatus.Visibility = Visibility.Collapsed;
					현위치Grid.Visibility = Visibility.Visible;
					좌석Grid.Visibility = Visibility.Visible;
					예정Grid.Visibility = Visibility.Visible;
				}
            }
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void UpdateRouteNumberTextBlock(BisArrivalInformation item)
		{
			txt노선번호.Width = 120; //edgar maybe text block not loaded that is why it shows small font size?
			txt노선번호.FontSize = 42;
			double textWidth = CommonUtils.GetTextWidth(item.RouteNumber.Trim(), txt노선번호);
			
			if (textWidth > txt노선번호.Width)
			{
				string str노선번호1, str노선번호2;
				//&& item.RouteNumber.Where(c => Char.IsLetter(c)).Count() == 1
				if (!item.RouteNumber.Contains("(") && item.RouteNumber.Where(Char.IsLetter).Count() < 2)
				{
					txt노선번호.Text = item.RouteNumber;
					txt노선번호.FontSize = CommonUtils.ReduceFontSizeIfNeeded(item.RouteNumber, txt노선번호); //before fixed size 40
					txt노선번호.Visibility = Visibility.Visible;
					txt노선번호1.Visibility = Visibility.Hidden;
					txt노선번호2.Visibility = Visibility.Hidden;
				}
				else if (!item.RouteNumber.Contains("(") && item.RouteNumber.Where(Char.IsLetter).Count() > 2)
				{
					if (Char.IsLetter(item.RouteNumber.First()))
					{
						var firstLetter = item.RouteNumber.Take(1);
						var numbers = item.RouteNumber.Skip(1).TakeWhile(IsDigitOrDash);
						numbers.ToList().ForEach(n => firstLetter.Append(n));
						str노선번호1 = new string(firstLetter.ToArray()); //new string(item.RouteNumber.Take(1).TakeWhile(c => Char.IsDigit(c) || c == '-').ToArray()); //edgar old Char.IsDigit(c)).ToArray()
						str노선번호2 = new string(item.RouteNumber.Skip(firstLetter.Count()).ToArray()); //new string(item.RouteNumber.SkipWhile(c => Char.IsLetterOrDigit(c) || c == '-').ToArray());
					}
					else
					{
						str노선번호1 = new string(item.RouteNumber.TakeWhile(IsDigitOrDash).ToArray());
						str노선번호2 = new string(item.RouteNumber.SkipWhile(IsDigitOrDash).ToArray());
					}

					DisplayRouteNumberInTwoLines(str노선번호1, str노선번호2);
				}
				else
				{
					str노선번호1 = new string(item.RouteNumber.TakeWhile(IsLetterOrDigitOrDash).ToArray());
					str노선번호2 = new string(item.RouteNumber.SkipWhile(IsLetterOrDigitOrDash).ToArray());

					DisplayRouteNumberInTwoLines(str노선번호1, str노선번호2);
				}
			}
			else
			{
				txt노선번호.Text = item.RouteNumber;
				txt노선번호.Visibility = Visibility.Visible;
				txt노선번호1.Visibility = Visibility.Hidden;
				txt노선번호2.Visibility = Visibility.Hidden;
			}
		}

		private static bool IsDigitOrDash(char c)
		{
			return Char.IsDigit(c) || c == '-';
		}

		private static bool IsLetterOrDigitOrDash(char c)
		{
			return Char.IsLetterOrDigit(c) || c == '-';
		}

		private void DisplayRouteNumberInTwoLines(string str노선번호1, string str노선번호2)
		{
			txt노선번호1.Text = str노선번호1;
			txt노선번호1.FontSize = CommonUtils.ReduceFontSizeIfNeeded(str노선번호1, txt노선번호1); //before fixed size
			txt노선번호2.Text = str노선번호2;
			txt노선번호2.FontSize = CommonUtils.ReduceFontSizeIfNeeded(str노선번호2, txt노선번호2); //before fixed size 

			txt노선번호1.Visibility = Visibility.Visible;
			txt노선번호2.Visibility = Visibility.Visible;
			txt노선번호.Visibility = Visibility.Hidden;
		}

		//private double ReduceFontSizeIfNeeded(string text, TextBlock textBlock)
		//{
		//	double currentWidth = GetTextWidth(text, textBlock);
		//	while (currentWidth > textBlock.Width)
		//	{
		//		textBlock.FontSize--;
		//		currentWidth = GetTextWidth(text, textBlock);
		//	}

		//	return textBlock.FontSize;
		//}

		//private double GetTextWidth(string text, TextBlock textBlock)
		//{
		//	return new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch), textBlock.FontSize, textBlock.Foreground, 200.0).Width;
		//}
		private string GetOperationStatus(int status)
		{
			// 0 == e ? "차고지대기" : -1 == e ? "회차지대기" : -2 == e ? "공차운행"	 : -3 == e ? "회송/이송"	: -4 == e ? "운행종료" : -5 == e	? "사고정지"	: -6 == e ? "차량점검": "";

			switch (status) 
			{ 
				case 0:
					return "차고지대기";
				case -1:
					return "회차지대기";
				case -2:
					return "공차운행";
				case -3:
					return "회송/이송";
				case -4:
					return "운행종료";
				case -5:
					return "사고정지";
				case -6:
					return "차량점검";
				default:
					return "차고지대기";
			}

		}
	}
}