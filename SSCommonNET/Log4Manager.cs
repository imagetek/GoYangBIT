using log4net;
using log4net.Config;
using System.IO;

namespace SSCommonNET
{
	public static class Log4Manager
	{
		private static ILog log = LogManager.GetLogger("SSNS");

		static Log4Manager()
		{
			XmlConfigurator.Configure(new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "Log4Net.xml")));
		}

		public static void WriteLog(Log4Level e, string logMessage)
		{
			Log4Manager.log = LogManager.GetLogger("SSNS");
			switch (e)
			{
				case Log4Level.Trace:
					Log4Manager.Trace(logMessage);
					break;
				case Log4Level.Error:
					Log4Manager.Error(logMessage);
					break;
				case Log4Level.Fatal:
					Log4Manager.Fatal(logMessage);
					break;
				case Log4Level.Info:
					Log4Manager.Info(logMessage);
					break;
				case Log4Level.Debug:
					Log4Manager.Debug(logMessage);
					break;
				default:
					Log4Manager.Trace(logMessage);
					break;
			}
		}

		public static void WriteSocketLog(Log4Level e, string logMessage)
		{
			Log4Manager.log = LogManager.GetLogger("SOCKET");
			switch (e)
			{
				case Log4Level.Trace:
					Log4Manager.Trace(logMessage);
					break;
				case Log4Level.Error:
					Log4Manager.Error(logMessage);
					break;
				case Log4Level.Fatal:
					Log4Manager.Fatal(logMessage);
					break;
				case Log4Level.Info:
					Log4Manager.Info(logMessage);
					break;
				case Log4Level.Debug:
					Log4Manager.Debug(logMessage);
					break;
				default:
					Log4Manager.Trace(logMessage);
					break;
			}
		}

		public static void WriteSerialLog(Log4Level e, string logMessage)
		{
			Log4Manager.log = LogManager.GetLogger("SERIAL");
			switch (e)
			{
				case Log4Level.Trace:
					Log4Manager.Trace(logMessage);
					break;
				case Log4Level.Error:
					Log4Manager.Error(logMessage);
					break;
				case Log4Level.Fatal:
					Log4Manager.Fatal(logMessage);
					break;
				case Log4Level.Info:
					Log4Manager.Info(logMessage);
					break;
				case Log4Level.Debug:
					Log4Manager.Debug(logMessage);
					break;
				default:
					Log4Manager.Trace(logMessage);
					break;
			}
		}

		public static void WriteENVLog(Log4Level e, string logMessage)
		{
			Log4Manager.log = LogManager.GetLogger("ENV");
			switch (e)
			{
				case Log4Level.Trace:
					Log4Manager.Trace(logMessage);
					break;
				case Log4Level.Error:
					Log4Manager.Error(logMessage);
					break;
				case Log4Level.Fatal:
					Log4Manager.Fatal(logMessage);
					break;
				case Log4Level.Info:
					Log4Manager.Info(logMessage);
					break;
				case Log4Level.Debug:
					Log4Manager.Debug(logMessage);
					break;
				default:
					Log4Manager.Trace(logMessage);
					break;
			}
		}

		public static void WriteFTPLog(Log4Level e, string logMessage)
		{
			Log4Manager.log = LogManager.GetLogger("FTP");
			switch (e)
			{
				case Log4Level.Trace:
					Log4Manager.Trace(logMessage);
					break;
				case Log4Level.Error:
					Log4Manager.Error(logMessage);
					break;
				case Log4Level.Fatal:
					Log4Manager.Fatal(logMessage);
					break;
				case Log4Level.Info:
					Log4Manager.Info(logMessage);
					break;
				case Log4Level.Debug:
					Log4Manager.Debug(logMessage);
					break;
				default:
					Log4Manager.Trace(logMessage);
					break;
			}
		}

		public static void WriteHTTPLog(Log4Level e, string logMessage)
		{
			Log4Manager.log = LogManager.GetLogger("HTTP");
			switch (e)
			{
				case Log4Level.Trace:
					Log4Manager.Trace(logMessage);
					break;
				case Log4Level.Error:
					Log4Manager.Error(logMessage);
					break;
				case Log4Level.Fatal:
					Log4Manager.Fatal(logMessage);
					break;
				case Log4Level.Info:
					Log4Manager.Info(logMessage);
					break;
				case Log4Level.Debug:
					Log4Manager.Debug(logMessage);
					break;
				default:
					Log4Manager.Trace(logMessage);
					break;
			}
		}

		private static void Error(string ErrorLog) => Log4Manager.log.Error((object)ErrorLog);

		private static void Fatal(string FatalLog) => Log4Manager.log.Fatal((object)FatalLog);

		private static void Info(string InfoLog) => Log4Manager.log.Info((object)InfoLog);

		private static void Debug(string DebugLog) => Log4Manager.log.Debug((object)DebugLog);

		private static void Trace(string TraceLog)
		{
		}
	}

	public enum Log4Level
	{
		Trace,
		Error,
		Fatal,
		Info,
		Debug,
	}
}
