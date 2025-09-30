using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SSCommonNET
{
	public class CommonUtils
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int SetSystemTime(ref CommonUtils.SYSTEMTIME systemTime);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int GetSystemTime(out CommonUtils.SYSTEMTIME systemTime);

		public static void SetSystemTime(int timeZone, DateTime sourceDateTime)
		{
			DateTime dateTime = timeZone <= 0 ? sourceDateTime.AddHours((double)timeZone) : sourceDateTime.AddHours((double)(timeZone * -1));
			CommonUtils.SYSTEMTIME systemTime = new SYSTEMTIME()
			{
				Year = Convert.ToUInt16(dateTime.Year),
				Month = Convert.ToUInt16(dateTime.Month),
				Day = Convert.ToUInt16(dateTime.Day),
				Hour = Convert.ToUInt16(dateTime.Hour),
				Minute = Convert.ToUInt16(dateTime.Minute),
				Second = Convert.ToUInt16(dateTime.Second)
			};
			
			_ = CommonUtils.SetSystemTime(ref systemTime);
		}

		public static void SetSystemTimeByUTC(DateTime sourceDateTime)
		{
			CommonUtils.SYSTEMTIME systemTime = new SYSTEMTIME()
			{
				Year = Convert.ToUInt16(sourceDateTime.Year),
				Month = Convert.ToUInt16(sourceDateTime.Month),
				Day = Convert.ToUInt16(sourceDateTime.Day),
				Hour = Convert.ToUInt16(sourceDateTime.Hour),
				Minute = Convert.ToUInt16(sourceDateTime.Minute),
				Second = Convert.ToUInt16(sourceDateTime.Second)
			};
			_ = CommonUtils.SetSystemTime(ref systemTime);
		}

		public static void SetSystemTime(DateTime sourceDateTime)
		{
			CommonUtils.SetSystemTime(9, sourceDateTime);
		}

		public static DateTime GetSystemTime(int timeZone)
		{
			CommonUtils.SYSTEMTIME systemTime = new CommonUtils.SYSTEMTIME();
			CommonUtils.GetSystemTime(out systemTime);
			DateTime dateTime = new DateTime(Convert.ToInt32(systemTime.Year), Convert.ToInt32(systemTime.Month), Convert.ToInt32(systemTime.Day), Convert.ToInt32(systemTime.Hour), Convert.ToInt32(systemTime.Minute), Convert.ToInt32(systemTime.Second), Convert.ToInt32(systemTime.Millisecond));
			return timeZone > 0 ? dateTime.AddHours((double)timeZone) : dateTime.AddHours((double)(timeZone * -1));
		}

		public static DateTime GetSystemTime() => CommonUtils.GetSystemTime(9);

		[DllImport("User32.dll")]
		private static extern bool GetLastInputInfo(ref CommonUtils.LASTINPUTINFO plii);

		public static uint GetIdleMilliSecond()
		{
			try
			{
				CommonUtils.LASTINPUTINFO plii = new CommonUtils.LASTINPUTINFO();
				plii.cbSize = (uint)Marshal.SizeOf<CommonUtils.LASTINPUTINFO>(plii);
				CommonUtils.GetLastInputInfo(ref plii);
				return (uint)Environment.TickCount - plii.dwTime;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return 0;
			}
		}

		public static bool IsNumeric(string num)
		{
			try
			{
				double result = 0.0;
				return double.TryParse(num, out result);
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static void WaitTime(int delay, bool doevent = true)
		{
			try
			{
				DateTime now = DateTime.Now;
				do
				{
					if (doevent && Application.Current != null)
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate)(() => { }));
				}
				while ((DateTime.Now - now).TotalMilliseconds <= (double)delay);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public static void DoEvents()
		{
			try
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate)(() => { }));
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public static string GetDisplaySize(double lBytes)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string str1 = "Bytes";
			if (lBytes > 1024.0)
			{
				string str2;
				float num;
				if (lBytes < 1048576.0)
				{
					str2 = "KB";
					num = Convert.ToSingle(lBytes) / 1024f;
				}
				else if (lBytes >= 1073741824.0)
				{
					str2 = "GB";
					num = (float)((double)Convert.ToSingle(lBytes) / 1024.0 / 1024.0 / 1024.0);
				}
				else
				{
					str2 = "MB";
					num = (float)((double)Convert.ToSingle(lBytes) / 1024.0 / 1024.0);
				}
				stringBuilder.AppendFormat("{0:0.0} {1}", (object)num, (object)str2);
			}
			else
			{
				float single = Convert.ToSingle(lBytes);
				stringBuilder.AppendFormat("{0:0} {1}", (object)single, (object)str1);
			}
			return stringBuilder.ToString();
		}

		public static string GetDisplayTime(double data)
		{
			try
			{
				double num1 = Math.Truncate(data / 1000.0);
				double num2 = Math.Truncate(num1 / 3600.0);
				double num3 = Math.Truncate(num1 % 3600.0 / 60.0);
				double num4 = num1 % 3600.0 % 60.0;
				double num5 = num1 % 1000.0;
				string displayTime = "";
				if (num2 > 0.0)
					displayTime += string.Format("{0:0}시간 ", (object)num2);
				if (num3 > 0.0)
					displayTime += string.Format("{0:00}분 ", (object)num3);
				if (num4 > 0.0)
					displayTime += string.Format("{0:00}초", (object)num4);
				if (num5 > 0.0)
					displayTime += string.Format("({0:000})", (object)num5);
				return displayTime;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return "";
			}
		}

		public static double GetPercent(int ix, int iy, bool percent)
		{
			try
			{
				if (ix == 0 || iy == 0)
					return 0.0;
				double num = Math.Round(Convert.ToDouble(ix) / Convert.ToDouble(iy), 3);
				return percent ? num * 100.0 : num;
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return 0.0;
			}
		}

		public static double GetPercent(double dx, double dy, bool percent)
		{
			try
			{
				if (dx == 0.0 || dy == 0.0)
					return 0.0;
				double num = Math.Round(dx / dy, 3);
				return percent ? num * 100.0 : num;
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return 0.0;
			}
		}

		public static double GetTotalFreeSpace(string driveName)
		{
			try
			{
				foreach (DriveInfo drive in DriveInfo.GetDrives())
				{
					if (drive.IsReady && drive.Name == driveName)
						return (double)drive.TotalFreeSpace;
				}
				return -1.0;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return -1.0;
			}
		}

		public static double GetTotalSpace(string driveName)
		{
			try
			{
				foreach (DriveInfo drive in DriveInfo.GetDrives())
				{
					if (drive.IsReady && drive.Name == driveName)
						return (double)drive.TotalSize;
				}
				return -1.0;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return -1.0;
			}
		}

		public static List<string> GetFontNames()
		{
			List<string> fontNames = new List<string>();
			try
			{
				using (InstalledFontCollection installedFontCollection = new InstalledFontCollection())
				{
					foreach (System.Drawing.FontFamily family in installedFontCollection.Families)
						fontNames.Add(family.Name);
				}
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
			return fontNames;
		}

		public static string GetFont화면표시(
		  string FontName,
		  double FontSize,
		  int FontStyleGBN = 0,
		  int FontWeightGBN = 400)
		{
			try
			{
				string font화면표시 = string.Format("{0}|{1}", (object)FontName, (object)Convert.ToInt32(FontSize));
				switch (FontWeightGBN)
				{
					case 100:
						font화면표시 += "|Thin";
						break;
					case 200:
						font화면표시 += "|ExtraLight";
						break;
					case 300:
						font화면표시 += "|Light";
						break;
					case 400:
						font화면표시 += "|Normal";
						break;
					case 500:
						font화면표시 += "|Medium";
						break;
					case 600:
						font화면표시 += "|DemiBold";
						break;
					case 700:
						font화면표시 += "|Bold";
						break;
					case 800:
						font화면표시 += "|ExtraBold";
						break;
					case 900:
						font화면표시 += "|Heavy";
						break;
					case 950:
						font화면표시 += "|ExtraBlack";
						break;
				}
				switch (FontStyleGBN)
				{
					case 0:
						font화면표시 += "|Normal";
						break;
					case 1:
						font화면표시 += "|Oblique";
						break;
					case 2:
						font화면표시 += "|Italic";
						break;
				}
				return font화면표시;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return "";
			}
		}

		public static List<string> GetFont설정값(string FontInfo)
		{
			try
			{
				List<string> font설정값 = new List<string>();
				string[] strArray = FontInfo.Split('|');
				if (strArray.Length == 4)
				{
					font설정값.Add(strArray[0]);
					font설정값.Add(strArray[1]);
					switch (strArray[2])
					{
						case "Bold":
							font설정값.Add("700");
							break;
						case "DemiBold":
							font설정값.Add("600");
							break;
						case "ExtraBlack":
							font설정값.Add("950");
							break;
						case "ExtraBold":
							font설정값.Add("800");
							break;
						case "ExtraLight":
							font설정값.Add("200");
							break;
						case "Heavy":
							font설정값.Add("900");
							break;
						case "Light":
							font설정값.Add("300");
							break;
						case "Medium":
							font설정값.Add("500");
							break;
						case "Normal":
							font설정값.Add("400");
							break;
						case "Thin":
							font설정값.Add("100");
							break;
					}
					switch (strArray[3])
					{
						case "Normal":
							font설정값.Add("0");
							break;
						case "Oblique":
							font설정값.Add("1");
							break;
						case "Italic":
							font설정값.Add("2");
							break;
					}
				}
				return font설정값;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return (List<string>)null;
			}
		}

		public static bool CHECK_시간포함여부(string S_TIME, string E_TIME)
		{
			try
			{
				DateTime now = DateTime.Now;
				int int32_1 = Convert.ToInt32(S_TIME.Substring(0, 2));
				int int32_2 = Convert.ToInt32(S_TIME.Substring(2));
				int int32_3 = Convert.ToInt32(E_TIME.Substring(0, 2));
				int int32_4 = Convert.ToInt32(E_TIME.Substring(2));
				DateTime dateTime1 = new DateTime(now.Year, now.Month, now.Day, int32_1, int32_2, 0);
				DateTime dateTime2 = new DateTime(now.Year, now.Month, now.Day, int32_3, int32_4, 0);
				return now >= dateTime1 && now <= dateTime2;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return false;
			}
		}

		public struct SYSTEMTIME
		{
			public ushort Year;
			public ushort Month;
			public ushort DayOfWeek;
			public ushort Day;
			public ushort Hour;
			public ushort Minute;
			public ushort Second;
			public ushort Millisecond;
		}

		internal struct LASTINPUTINFO
		{
			public uint cbSize;
			public uint dwTime;
		}

		public static double ReduceFontSizeIfNeeded(string text, TextBlock textBlock)
		{
			double currentWidth = GetTextWidth(text, textBlock);
			while (currentWidth > textBlock.Width)
			{
				textBlock.FontSize--;
				currentWidth = GetTextWidth(text, textBlock);
			}

			return textBlock.FontSize;
		}

		public static double GetTextWidth(string text, TextBlock textBlock)
		{
			return new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch), textBlock.FontSize, textBlock.Foreground, 200.0).Width;
		}
	}
}
