using SSCommonNET;
using System;
using System.Diagnostics;

namespace SSData.Utils
{
	public static class LogWriterService
	{
		public static void WriteToLog(Exception ee)
		{
			TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		}
	}
}
