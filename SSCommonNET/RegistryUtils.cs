using Microsoft.Win32;

namespace SSCommonNET
{
	public class RegistryUtils
	{
		public static void RegClear(string root)
		{
			try
			{
				Registry.CurrentUser.CreateSubKey(root);
				Registry.LocalMachine.DeleteSubKeyTree(root);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public static void RegDelete(string root, string key)
		{
			try
			{
				Registry.CurrentUser.CreateSubKey(root).DeleteValue(key);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public static object RegReader(string root, string key)
		{
			try
			{
				return Registry.CurrentUser.CreateSubKey(root).GetValue(key);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return (object)null;
			}
		}

		public static object RegReaderByMachine(string root, string key)
		{
			try
			{
				return Registry.LocalMachine.OpenSubKey(root, false).GetValue(key);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return (object)null;
			}
		}

		public static bool IsExistKey(string root, string key)
		{
			try
			{
				return Registry.CurrentUser.CreateSubKey(root).GetValue(key) != null;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return false;
			}
		}

		public static bool RegWriter(string root, string key, object value)
		{
			try
			{
				Registry.CurrentUser.CreateSubKey(root).SetValue(key, value);
				return true;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return false;
			}
		}
	}
}
