using SSCommonNET;
using System.Windows;

namespace SSControlsNET
{
	public class MsgDlgManager
	{
		private static Dictionary<string, wndInformation> _winform = new Dictionary<string, wndInformation>();
		private static Dictionary<string, wndTouchInformation> _winformTouch = new Dictionary<string, wndTouchInformation>();
		private static wndProgress _wpgb = (wndProgress)null;

		public static void ShowMessageDlg(string title, string msg, bool isShow가운데 = false)
		{
			try
			{
				wndMessage wndMessage = new wndMessage(title, msg);
				if (!isShow가운데)
				{
					wndMessage.Owner = Application.Current.MainWindow;
					wndMessage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				}
				else
				{
					wndMessage.Owner = Application.Current.MainWindow;
					wndMessage.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				}
				wndMessage.ShowDialog();
				wndMessage.Close();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public static void ShowTouchMessageDlg(string title, string msg, bool isShow가운데 = false)
		{
			try
			{
				wndTouchMessage wndTouchMessage = new wndTouchMessage(title, msg);
				if (!isShow가운데)
				{
					wndTouchMessage.Owner = Application.Current.MainWindow;
					wndTouchMessage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				}
				else
				{
					wndTouchMessage.Owner = Application.Current.MainWindow;
					wndTouchMessage.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				}
				wndTouchMessage.ShowDialog();
				wndTouchMessage.Close();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public static bool ShowQuestionDlg(string title, string msg, bool isShow가운데 = false)
		{
			try
			{
				wndQuestionCheck wndQuestionCheck = new wndQuestionCheck(title, msg);
				if (!isShow가운데)
				{
					wndQuestionCheck.Owner = Application.Current.MainWindow;
					wndQuestionCheck.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				}
				else
				{
					wndQuestionCheck.Owner = Application.Current.MainWindow;
					wndQuestionCheck.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				}
				bool flag = wndQuestionCheck.ShowDialog().Value;
				wndQuestionCheck.Close();
				return flag;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return false;
			}
		}

		public static string ShowInformationDlg(string title, string msg, bool isShow가운데 = false)
		{
			try
			{
				string key = Guid.NewGuid().ToString();
				if (MsgDlgManager._winform.ContainsKey(key))
					return (string)null;
				wndInformation wndInformation = new wndInformation(title, msg);
				if (!isShow가운데)
				{
					wndInformation.Owner = Application.Current.MainWindow;
					wndInformation.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				}
				else
				{
					wndInformation.Owner = Application.Current.MainWindow;
					wndInformation.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				}
				wndInformation.Topmost = true;
				wndInformation.Show();
				if (!MsgDlgManager._winform.ContainsKey(key))
					MsgDlgManager._winform.Add(key, wndInformation);
				CommonUtils.WaitTime(300);
				return key;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return (string)null;
			}
		}

		public static string ShowInformationDlg(string guid, string title, string msg, bool isShow가운데 = false)
		{
			try
			{
				if (!MsgDlgManager._winform.ContainsKey(guid))
					return (string)null;
				wndInformation wndInformation = MsgDlgManager._winform[guid];
				if (!isShow가운데)
				{
					wndInformation.Owner = Application.Current.MainWindow;
					wndInformation.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				}
				else
				{
					wndInformation.Owner = Application.Current.MainWindow;
					wndInformation.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				}
				if (!title.Trim().Equals(""))
					wndInformation.Title = title;
				wndInformation.txtMessage.Text = msg;
				wndInformation.Show();
				CommonUtils.WaitTime(300);
				return guid;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return (string)null;
			}
		}

		public static void CloseInformationDlg(string guid)
		{
			try
			{
				if (!MsgDlgManager._winform.ContainsKey(guid))
					return;
				MsgDlgManager._winform[guid].Close();
				MsgDlgManager._winform.Remove(guid);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public static void ShowIntervalInformationDlg(
		  string title,
		  string msg,
		  int interval = 1500,
		  bool isShow가운데 = false)
		{
			try
			{
				string guid = MsgDlgManager.ShowInformationDlg(title, msg, isShow가운데);
				CommonUtils.WaitTime(interval);
				MsgDlgManager.CloseInformationDlg(guid);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("{0}\r\n{1}", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public static void ShowTouchIntervalInformationDlg(
		  string title,
		  string msg,
		  int interval = 1500,
		  bool isShow가운데 = false)
		{
			try
			{
				string guid = MsgDlgManager.ShowTouchInformationDlg(title, msg, isShow가운데);
				CommonUtils.WaitTime(interval);
				MsgDlgManager.CloseTouchInformationDlg(guid);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("{0}\r\n{1}", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public static string ShowTouchInformationDlg(string title, string msg, bool isShow가운데 = false)
		{
			try
			{
				string key = Guid.NewGuid().ToString();
				if (MsgDlgManager._winformTouch.ContainsKey(key))
					return (string)null;
				wndTouchInformation touchInformation = new wndTouchInformation(title, msg);
				if (!isShow가운데)
				{
					touchInformation.Owner = Application.Current.MainWindow;
					touchInformation.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				}
				else
				{
					touchInformation.Owner = Application.Current.MainWindow;
					touchInformation.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				}
				touchInformation.Topmost = true;
				touchInformation.Show();
				if (!MsgDlgManager._winformTouch.ContainsKey(key))
					MsgDlgManager._winformTouch.Add(key, touchInformation);
				CommonUtils.WaitTime(300);
				return key;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return (string)null;
			}
		}

		public static string ShowTouchInformationDlg(
		  string guid,
		  string title,
		  string msg,
		  bool isShow가운데 = false)
		{
			try
			{
				if (!MsgDlgManager._winformTouch.ContainsKey(guid))
					return (string)null;
				wndTouchInformation touchInformation = MsgDlgManager._winformTouch[guid];
				if (!isShow가운데)
				{
					touchInformation.Owner = Application.Current.MainWindow;
					touchInformation.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				}
				else
				{
					touchInformation.Owner = Application.Current.MainWindow;
					touchInformation.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				}
				if (!title.Trim().Equals(""))
					touchInformation.Title = title;
				touchInformation.txtMessage.Text = msg;
				touchInformation.Show();
				CommonUtils.WaitTime(300);
				return guid;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return (string)null;
			}
		}

		public static void CloseTouchInformationDlg(string guid)
		{
			try
			{
				if (!MsgDlgManager._winformTouch.ContainsKey(guid))
					return;
				MsgDlgManager._winformTouch[guid].Close();
				MsgDlgManager._winformTouch.Remove(guid);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public static bool ShowTouchQuestionDlg(string title, string msg, bool isShow가운데 = false)
		{
			try
			{
				wndTouchQuestionCheck touchQuestionCheck = new wndTouchQuestionCheck(title, msg);
				if (!isShow가운데)
				{
					touchQuestionCheck.Owner = Application.Current.MainWindow;
					touchQuestionCheck.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				}
				else
				{
					touchQuestionCheck.Owner = Application.Current.MainWindow;
					touchQuestionCheck.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				}
				bool flag = touchQuestionCheck.ShowDialog().Value;
				touchQuestionCheck.Close();
				return flag;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return false;
			}
		}

		public static void ShowPrograssDlg(string title, string msg, bool useProgBar = true, bool isShow가운데 = false)
		{
			try
			{
				if (MsgDlgManager._wpgb != null)
					return;
				MsgDlgManager._wpgb = new wndProgress(title, msg, useProgBar);
				if (!isShow가운데)
				{
					MsgDlgManager._wpgb.Owner = Application.Current.MainWindow;
					MsgDlgManager._wpgb.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				}
				else
				{
					MsgDlgManager._wpgb.Owner = Application.Current.MainWindow;
					MsgDlgManager._wpgb.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				}
				MsgDlgManager._wpgb.Show();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public static void SetPrograssValue(double per)
		{
			try
			{
				if (MsgDlgManager._wpgb == null)
					return;
				MsgDlgManager._wpgb.SetPrograss("", per);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public static void SetPrograssValue(string msg, double per)
		{
			try
			{
				if (MsgDlgManager._wpgb == null)
					return;
				MsgDlgManager._wpgb.SetPrograss(msg, per);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public static void SetPrograssValue(string title, string msg, double per)
		{
			try
			{
				if (MsgDlgManager._wpgb == null)
					return;
				MsgDlgManager._wpgb.SetTitle(title);
				MsgDlgManager._wpgb.SetPrograss(msg, per);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public static void ClosePrograssDlg()
		{
			try
			{
				if (MsgDlgManager._wpgb == null)
					return;
				MsgDlgManager._wpgb.Close();
				MsgDlgManager._wpgb = (wndProgress)null;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}
	}
}
