using System;
using System.Windows.Media;

namespace SSCommonNET
{
	public class ConvertUtils
	{
		public static DateTime? DateTimeByString(string YMD)
		{
			try
			{
				DateTime dateTime = new DateTime();
				if (YMD.Equals("") || YMD.Equals("00000000"))
					return new DateTime?();
				switch (YMD.Length)
				{
					case 6:
						dateTime = new DateTime(Convert.ToInt32(YMD.Substring(0, 2)) + 2000, Convert.ToInt32(YMD.Substring(2, 2)), Convert.ToInt32(YMD.Substring(4, 2)));
						break;
					case 8:
						dateTime = new DateTime(Convert.ToInt32(YMD.Substring(0, 4)), Convert.ToInt32(YMD.Substring(4, 2)), Convert.ToInt32(YMD.Substring(6, 2)));
						break;
					case 10:
						dateTime = new DateTime(Convert.ToInt32(YMD.Substring(0, 4)), Convert.ToInt32(YMD.Substring(4, 2)), Convert.ToInt32(YMD.Substring(6, 2)), Convert.ToInt32(YMD.Substring(8, 2)), 0, 0);
						break;
					case 12:
						dateTime = new DateTime(Convert.ToInt32(YMD.Substring(0, 4)), Convert.ToInt32(YMD.Substring(4, 2)), Convert.ToInt32(YMD.Substring(6, 2)), Convert.ToInt32(YMD.Substring(8, 2)), Convert.ToInt32(YMD.Substring(10, 2)), 0);
						break;
					case 14:
						dateTime = new DateTime(Convert.ToInt32(YMD.Substring(0, 4)), Convert.ToInt32(YMD.Substring(4, 2)), Convert.ToInt32(YMD.Substring(6, 2)), Convert.ToInt32(YMD.Substring(8, 2)), Convert.ToInt32(YMD.Substring(10, 2)), Convert.ToInt32(YMD.Substring(12, 2)));
						break;
					case 19:
						dateTime = Convert.ToDateTime(YMD);
						break;
					default:
						dateTime = Convert.ToDateTime(YMD);
						break;
				}
				return new DateTime?(dateTime);
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return new DateTime?();
			}
		}

		public static string StringByDateTime(DateTime dtYMD)
		{
			try
			{
				return dtYMD.ToString("yyyyMMdd");
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return "00000000";
			}
		}

		public static string StringByObject(object objData)
		{
			try
			{
				return objData == DBNull.Value || objData == null ? "" : objData.ToString();
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return "";
			}
		}

		public static bool BoolByObject(object objData)
		{
			try
			{
				if (objData == DBNull.Value || objData == null || objData.ToString().ToUpper().Equals("F"))
					return false;
				return objData.ToString().ToUpper().Equals("T") || Convert.ToBoolean(objData);
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return false;
			}
		}

		public static string StringByBool(bool _isYN)
		{
			try
			{
				return _isYN ? "T" : "F";
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return "";
			}
		}

		public static int IntByBool(bool isYN)
		{
			try
			{
				return isYN ? 1 : 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return 0;
			}
		}

		public static int IntByObject(object objData)
		{
			try
			{
				return objData == DBNull.Value || objData == null || objData.ToString().Equals("") || objData.ToString().Equals(" ") || objData.Equals((object)"-") ? 0 : Convert.ToInt32(objData);
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return 0;
			}
		}

		public static int IntByString(string strData)
		{
			try
			{
				switch (strData)
				{
					case null:
						return 0;
					case "":
						return 0;
					default:
						return Convert.ToInt32(strData);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return 0;
			}
		}

		public static double DoubleByObject(object objData)
		{
			try
			{
				return objData == DBNull.Value || objData == null ? 0.0 : Convert.ToDouble(objData);
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return 0.0;
			}
		}

		public static DateTime DateTimeByObject(object objData)
		{
			try
			{
				if (objData == DBNull.Value)
					return new DateTime();
				if (objData == null)
					return new DateTime();
				if (objData.Equals((object)""))
					return new DateTime();
				DateTime now = DateTime.Now;
				return Convert.ToDateTime(objData);
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return new DateTime();
			}
		}

		public static SolidColorBrush ColorByRGBCode(string ArgbCode)
		{
			try
			{
				return (SolidColorBrush)new BrushConverter().ConvertFrom(!ArgbCode.Substring(0, 1).Equals("#") ? (object)string.Format("#{0}", (object)ArgbCode) : (object)ArgbCode);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return new SolidColorBrush();
			}
		}
	}
}
