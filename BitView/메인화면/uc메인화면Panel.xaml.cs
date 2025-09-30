using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using SSCommonNET;
using SSData;
using SSData.DashboardAPI;

namespace BitView
{
	/// <summary>
	/// ucENV2Panel.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class uc메인화면Panel : UserControl
	{
		public uc메인화면Panel()
		{
			InitializeComponent();
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

					//Load기본값();
					//Load환경설정();
				}

				if (_dtRefresh != null)
				{
					_dtRefresh.Start();
					_dtRefresh_Tick(_dtRefresh, null);
				}
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

		DispatcherTimer _dtRefresh = null;
		private void InitProc()
		{
			try
			{
				EventManager.OnDisplayLogEvent += EventManager_OnDisplayLogEvent;
				SSENV2Manager.OnDisplayLogEvent += EventManager_OnDisplayLogEvent;   //pjh add
				SSENV2Manager.On상태정보Event += SSENV2Manager_On상태정보Event;
				//CommonUtils.GetCPU();

				PowerShellService.OnTaskEnabledEvent += PowerShellService_OnTaskEnabledEvent;
				PowerShellService.OnTaskDisabledEvent += PowerShellService_OnTaskDisabledEvent;
				PowerShellService.OnTaskNotRegisteredEvent += PowerShellService_OnTaskNotRegisteredEvent;
				PowerShellService.OnTaskErrorEvent += PowerShellService_OnTaskErrorEvent;
				PowerShellService.OnTaskRegisteredEvent += PowerShellService_OnTaskRegisteredEvent;
				PowerShellService.OnTaskUnregisteredEvent += PowerShellService_OnTaskUnregisteredEvent;

				if (_dtRefresh == null)
				{
					_dtRefresh = new DispatcherTimer();
					_dtRefresh.Tick += _dtRefresh_Tick;
				}
				_dtRefresh.Tag = 0;
				_dtRefresh.Interval = TimeSpan.FromMinutes(10);

				ManagementObjectSearcher MS1 = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
				var collection = MS1.Get();
				foreach (ManagementObject MO in MS1.Get().Cast<ManagementObject>())
				{
					txtCPU이름.Text = MO["Name"].ToString();
				}

				ManagementObjectSearcher MS4 = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
				foreach (ManagementObject MO in MS4.Get().Cast<ManagementObject>())
				{
					txtOS이름.Text = MO["Caption"].ToString();
				}

				//PowerShellService.CheckSSAgentStatus();
				Task.Run(() =>
				{
					PowerShellService.CreateAndRunProcess(PsScriptType.CheckStatus);
				});
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void PowerShellService_OnTaskUnregisteredEvent()
		{
			this.Dispatcher.BeginInvoke(new Action(() =>
			{
				registerScheduledTask.Content = "Register SSAgent";
				toggleScheduledTask.IsEnabled = false;
			}));
			
			isAgentRegistered = false;
			EventManager.DisplayLog(Log4Level.Info, "SSAgent unregistered!", LogSource.Other);
		}

		private void PowerShellService_OnTaskRegisteredEvent()
		{
			this.Dispatcher.BeginInvoke(new Action(() =>
			{
				registerScheduledTask.Content = "Unregister SSAgent";
				toggleScheduledTask.IsEnabled = true;
			}));
			
			isAgentRegistered = true;
			EventManager.DisplayLog(Log4Level.Info, "SSAgent registered!", LogSource.Other);
		}

		private void PowerShellService_OnTaskErrorEvent(string error)
		{
			EventManager.DisplayLog(Log4Level.Error, error, LogSource.Other);
		}

		private void PowerShellService_OnTaskNotRegisteredEvent()
		{
			this.Dispatcher.BeginInvoke(new Action(() => 
			{
				registerScheduledTask.Content = "Register SSAgent";
				toggleScheduledTask.IsEnabled = false;
			}));
			
			isAgentRegistered = false;
			EventManager.DisplayLog(Log4Level.Info, "Please register SSAgent.", LogSource.Other);
		}

		private void PowerShellService_OnTaskDisabledEvent()
		{			
			this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => 
			{
				toggleScheduledTask.IsEnabled = true;
				toggleScheduledTask.Content = "Enable SSAgent";
				registerScheduledTask.Content = "Unregister SSAgent";
			}));
			isAgentEnabled = false;
			isAgentRegistered = true;
			EventManager.DisplayLog(Log4Level.Info, "SSAgent is disabled.", LogSource.Other);
		}

		private void PowerShellService_OnTaskEnabledEvent()
		{
			this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => 
			{
				toggleScheduledTask.IsEnabled = true;
				toggleScheduledTask.Content = "Disable SSAgent";
				registerScheduledTask.Content = "Unregister SSAgent";
			}));

			isAgentEnabled = true;
			isAgentRegistered = true;
			EventManager.DisplayLog(Log4Level.Info, "SSAgent is enabled.", LogSource.Other);
		}

		private void SSENV2Manager_On상태정보Event(SSNS_ENV2_STATE _data)
		{
			this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal ,() =>
			{
				UpdateBoardStatus(_data);
			});
		}
		private void UpdateBoardStatus(SSNS_ENV2_STATE boardStatus)
		{
			mainVoltageTxt.Text = boardStatus.MainVoltage.ToString();
			mainCurrentTxt.Text = boardStatus.MainCurrent.ToString();
			batteryCurrentTxt.Text = boardStatus.BattCurrent.ToString();
			solarVoltageTxt.Text= boardStatus.PanlVoltage.ToString();
			solarCurrentTxt.Text= boardStatus.PanlCurrent.ToString();
			tempTxt.Text = boardStatus.Temparature1.ToString();
			temp2Txt.Text = boardStatus.Temparature2.ToString();
			
			humidityTxt.Text = boardStatus.Humidity.ToString();
			brightnessTxt.Text = boardStatus.Luminance.ToString();
			shockTxt.Text = boardStatus.Shock1.ToString();
			doorTxt.Text = boardStatus.Door.ToString();
			frontLightTxt.Text = boardStatus.Illumination.ToString();
			battPercentTxt.Text = boardStatus.BattPercent.ToString();
			battRemainTxt.Text = boardStatus.RemainBat.ToString();
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			try
			{
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
				EventManager.OnDisplayLogEvent -= EventManager_OnDisplayLogEvent;

				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void _dtRefresh_Tick(object sender, EventArgs e)
		{
			try
			{
				if (Convert.ToInt32(_dtRefresh.Tag) == 1) return;
				_dtRefresh.Tag = 1;

				SelectPC정보();

				_dtRefresh.Tag = 0;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				_dtRefresh.Tag = 0;
			}
		}

		private void SelectPC정보()
		{
			try
			{
				txt정류장정보.Text = string.Format("[{0}] {1}({2})", BITDataManager.BIT_ID, BITDataManager.정류장명, BITDataManager.정류장모바일번호);

				ManagementClass cls = new ManagementClass("Win32_OperatingSystem");
				ManagementObjectCollection memory = cls.GetInstances();

				double szTotalMemory = 0;       // 총 메모리 
				double szFreeMemory = 0;       // 사용가능 메모리
				foreach (ManagementObject instance in memory)
				{
					szTotalMemory = double.Parse(instance["TotalVisibleMemorySize"].ToString());
					szFreeMemory = double.Parse(instance["FreePhysicalMemory"].ToString());
				}

				double RAMFreePercent = CommonUtils.GetPercent(szFreeMemory, szTotalMemory, true);
				p메모리.Value = RAMFreePercent;

				txt메모리.Text = string.Format("{0} %", Math.Round(RAMFreePercent, 2));
				txt메모리정보.Text = string.Format("( {0} / {1} )", CommonUtils.GetDisplaySize(szFreeMemory * 1024), CommonUtils.GetDisplaySize(szTotalMemory * 1024));


				//var wmi = new ManagementObjectSearcher($"select * from Win32_PerfFormattedData_PerfProc_Process where Name !='_Total' ");// '{System.Diagnostics.Process.GetCurrentProcess().ProcessName}'");
				//var procTime = wmi.Get().Cast<ManagementObject>().Select(mo => (long)(ulong)mo["PercentProcessorTime"]).FirstOrDefault();
				//double procUsage = procTime / Environment.ProcessorCount;
				string ProcNM = Process.GetCurrentProcess().ProcessName;
				PerformanceCounter pcPGM = new PerformanceCounter("Process", "% Processor Time", ProcNM);

				pcPGM.NextValue();
				Thread.Sleep(1000);
				float procUsage = pcPGM.NextValue() / Environment.ProcessorCount;

				pCPU.Value = procUsage;
				txtCPU.Text = string.Format("{0} %", Math.Round(procUsage, 2));

				string log = string.Format("[PC상태] CPU : {0} , RAM : {1}%", procUsage, RAMFreePercent);

				Log4Manager.WriteLog(Log4Level.Info, log);

				HDD용량관리();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private async void HDD용량관리()
		{
			try
			{
				string HDD = System.IO.Directory.GetDirectoryRoot(AppConfig.APPSStartPath);
				double szTotalHDD = CommonUtils.GetTotalSpace(HDD);
				double szFreeHDD = CommonUtils.GetTotalFreeSpace(HDD);

				double HDDFreePercent = CommonUtils.GetPercent(szFreeHDD, szTotalHDD, true);
				p디스크.Value = HDDFreePercent;
				txt디스크.Text = string.Format("{0} %", Math.Round(HDDFreePercent, 2));
				txt디스크정보.Text = string.Format("( {0} / {1} )", CommonUtils.GetDisplaySize(szFreeHDD), CommonUtils.GetDisplaySize(szTotalHDD));

				//	if (HDDFreePercent < BITDataManager.BitConfig.LOGSAVE_PERCENT)
				if (HDDFreePercent < 5) //pjh
				{
					EventManager.DisplayLog(Log4Level.Info, string.Format("[폴더정리] HDD용량이 지정:{0}% 현재:{1}% 폴더정리를 시작합니다.", BITDataManager.BitConfig.LOGSAVE_PERCENT, HDDFreePercent));

					int MaxDay = BITDataManager.BitConfig.LOGSAVE_DAY;

					while (true)
					{
						if (MaxDay < 5) break;

						EventManager.DisplayLog(Log4Level.Info, string.Format("[폴더정리] HDD남은용량이 현재:{0}%이라 폴더내 {1}일전 파일을 정리합니다."
							, HDDFreePercent, MaxDay));

						double bFreeSize = await Clear폴더정리(HDD, MaxDay);
						HDDFreePercent = CommonUtils.GetPercent(bFreeSize, szTotalHDD, true);
						if (HDDFreePercent > BITDataManager.BitConfig.LOGSAVE_PERCENT) break;

						MaxDay--;
					}
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private async Task<double> Clear폴더정리(string HDD, int nDAY)
		{
			try
			{
				DateTime dt = DateTime.Now.AddDays(-1 * nDAY);
				string 화면캡쳐DIR = System.IO.Path.Combine(DataManager.Get캡쳐DIR(), "SCREEN");
				if (System.IO.Directory.Exists(화면캡쳐DIR) == true)
				{
					List<string> items화면캡쳐 = new List<string>();
					items화면캡쳐.AddRange(System.IO.Directory.GetFiles(화면캡쳐DIR, string.Format("{0}*.jpg", dt.ToString("yyyyMMdd")), System.IO.SearchOption.TopDirectoryOnly).ToList());
					items화면캡쳐.AddRange(System.IO.Directory.GetFiles(화면캡쳐DIR, string.Format("{0}*.png", dt.ToString("yyyyMMdd")), System.IO.SearchOption.TopDirectoryOnly).ToList());
					if (items화면캡쳐 != null && items화면캡쳐.Count > 0)
					{
						foreach (string item in items화면캡쳐)
						{
							try
							{
								//fi.Delete();
								await Task.Run(() =>
								{
									System.IO.File.Delete(item);
								});
							}
							catch (System.IO.IOException ex)
							{
								TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
							}
						}
					}
				}

				string 웹캠DIR = System.IO.Path.Combine(DataManager.Get캡쳐DIR(), "WEBCAM");
				if (System.IO.Directory.Exists(웹캠DIR) == true)
				{
					List<string> items웹캠 = new List<string>();
					items웹캠.AddRange(System.IO.Directory.GetFiles(웹캠DIR, string.Format("{0}*.jpg", dt.ToString("yyyyMMdd")), System.IO.SearchOption.TopDirectoryOnly).ToList());
					items웹캠.AddRange(System.IO.Directory.GetFiles(웹캠DIR, string.Format("{0}*.png", dt.ToString("yyyyMMdd")), System.IO.SearchOption.TopDirectoryOnly).ToList());

					foreach (string item in items웹캠)
					{
						try
						{
							//fi.Delete();
							await Task.Run(() =>
							{
								System.IO.File.Delete(item);
							});
						}
						catch (System.IO.IOException ex)
						{
							TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
						}
					}
				}

				string 충격DIR = System.IO.Path.Combine(DataManager.Get캡쳐DIR(), "SHOCK");
				if (System.IO.Directory.Exists(충격DIR) == true)
				{
					List<string> items충격 = new List<string>();
					items충격.AddRange(System.IO.Directory.GetFiles(충격DIR, string.Format("{0}*.avi", dt.ToString("yyyyMMdd")), System.IO.SearchOption.TopDirectoryOnly).ToList());
					items충격.AddRange(System.IO.Directory.GetFiles(충격DIR, string.Format("{0}*.mp4", dt.ToString("yyyyMMdd")), System.IO.SearchOption.TopDirectoryOnly).ToList());

					foreach (string item in items충격)
					{
						try
						{
							//fi.Delete();
							await Task.Run(() =>
							{
								System.IO.File.Delete(item);
							});
						}
						catch (System.IO.IOException ex)
						{
							TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
						}
					}
				}
				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			return CommonUtils.GetTotalFreeSpace(HDD);
		}

		private void Clear파일정리()
		{
			try
			{
				string 화면캡쳐DIR = System.IO.Path.Combine(DataManager.Get캡쳐DIR(), "SCREEN");
				if (System.IO.Directory.Exists(화면캡쳐DIR) == true)
				{
					List<string> items화면캡쳐 = new List<string>();
					items화면캡쳐.AddRange(System.IO.Directory.GetFiles(화면캡쳐DIR, "*.jpg", System.IO.SearchOption.TopDirectoryOnly).ToList());
					items화면캡쳐.AddRange(System.IO.Directory.GetFiles(화면캡쳐DIR, "*.png", System.IO.SearchOption.TopDirectoryOnly).ToList());
					if (items화면캡쳐 != null && items화면캡쳐.Count > 0)
					{
						foreach (string item in items화면캡쳐)
						{
							//string[] datas = item.Split('_');
							System.IO.FileInfo fi = new System.IO.FileInfo(item);
							//if (datas.Length < 4) continue;

							DateTime dt파일 = ConvertUtils.DateTimeByString(fi.Name.Replace(fi.Extension, "")).Value;
							int res = dt파일.CompareTo(DateTime.Now.AddDays(-1 * BITDataManager.BitConfig.LOGSAVE_DAY));
							if (res < 0)
							{
								try
								{
									fi.Delete();
									//System.IO.File.Delete(item);
								}
								catch (System.IO.IOException ex)
								{
									TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
								}
							}
							fi = null;
						}
					}
				}

				string 웹캠DIR = System.IO.Path.Combine(DataManager.Get캡쳐DIR(), "WEBCAM");
				if (System.IO.Directory.Exists(웹캠DIR) == true)
				{
					string[] items웹캠 = System.IO.Directory.GetFiles(웹캠DIR, "*.jpg", System.IO.SearchOption.TopDirectoryOnly);
					foreach (string item in items웹캠)
					{
						System.IO.FileInfo fi = new System.IO.FileInfo(item);

						DateTime dt파일 = ConvertUtils.DateTimeByString(fi.Name.Replace(fi.Extension, "")).Value;
						int res = dt파일.CompareTo(DateTime.Now.AddDays(-1 * BITDataManager.BitConfig.LOGSAVE_DAY));
						if (res < 0)
						{
							try
							{
								fi.Delete();
								//System.IO.File.Delete(item);
							}
							catch (System.IO.IOException ex)
							{
								TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
							}
						}
						fi = null;
					}
				}

				string 충격DIR = System.IO.Path.Combine(DataManager.Get캡쳐DIR(), "SHOCK");
				if (System.IO.Directory.Exists(충격DIR) == true)
				{
					string[] items충격 = System.IO.Directory.GetFiles(충격DIR, "*.avi", System.IO.SearchOption.TopDirectoryOnly);
					foreach (string item in items충격)
					{
						System.IO.FileInfo fi = new System.IO.FileInfo(item);

						DateTime dt파일 = ConvertUtils.DateTimeByString(fi.Name.Replace(fi.Extension, "")).Value;
						int res = dt파일.CompareTo(DateTime.Now.AddDays(-1 * BITDataManager.BitConfig.LOGSAVE_DAY));
						if (res < 0)
						{
							try
							{
								fi.Delete();
							}
							catch (System.IO.IOException ex)
							{
								TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
							}
						}
						fi = null;
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

		ItemCollection logHistory = new DataGrid().Items;
		private void EventManager_OnDisplayLogEvent(Log4Data log4)
		{
			try
			{
				this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate
				{
					//int CNT = dg로그.Items.Count;
					int CNT = logHistory.Count;
					if (CNT > 1000)
					{
						//dg로그.Items.Clear();
						logHistory.Clear();
						CommonUtils.WaitTime(50, true);
					}
					//dg로그.Items.Insert(0, log4);
					logHistory.Insert(0, log4);
					if (CNT % 25 == 0)
					{
						dg로그.ScrollIntoView(log4);
						CommonUtils.WaitTime(50, true);
					}

					bool shoulUpdateView = IsValidLogSource(log4);
					if (shoulUpdateView)
					{
						FilterLogItems();
					}

				}));				
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private bool IsValidLogSource(Log4Data log4)
		{
			return (log4.LogSource == LogSource.Other && (bool)otherCheck.IsChecked)
				|| (log4.LogSource == LogSource.BusInfo && (bool)businfoCheck.IsChecked)
				|| (log4.LogSource == LogSource.Board && (bool)boardCheck.IsChecked);
		}

		private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			try
			{
				Console.WriteLine("UserControl_SizeChanged {0}x{1}", e.NewSize.Width, e.NewSize.Height);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btn초기화_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate
				{
					dg로그.Items.Clear();
				}));
				logHistory.Clear();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btn버전확인_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;

				TraceManager.AddInfoLog("프로그램 버전확인 메세지를 Agent에 전송했습니다.");
				EventManager.DisplayLog(Log4Level.Info, "프로그램 버전확인 메세지를 Agent에 전송했습니다.");
				ShareMemoryManager.SendAgentMessage("UPDATE");
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btn로그업로드_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;

				EventManager.DisplayLog(Log4Level.Info, "로그 업로드 요청메세지를 Agent에 전송했습니다.");
				ShareMemoryManager.SendAgentMessage("LOG");
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void filterLogBtn_Click(object sender, RoutedEventArgs e)
		{
			FilterLogItems();

		}

		private void FilterLogItems()
		{
			try
			{
				Log4Data[] temp = new Log4Data[logHistory.Count];
				List<Log4Data> itemsToShow = [];
				logHistory.CopyTo(temp, 0);

				if ((bool)businfoCheck.IsChecked)
				{
					itemsToShow.AddRange(temp?.Where(l => l.LogSource == LogSource.BusInfo));
				}
				if ((bool)boardCheck.IsChecked)
				{
					itemsToShow.AddRange(temp?.Where(l => l.LogSource == LogSource.Board));
				}
				if ((bool)otherCheck.IsChecked)
				{
					itemsToShow.AddRange(temp?.Where(l => l.LogSource == LogSource.Other));
				}

				dg로그.Items.Clear();
				itemsToShow?.OrderBy(i => i.REGDATE)?.ToList()?.ForEach(i => dg로그.Items.Insert(0, i));

				this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate
				{
					dg로그.Items.Refresh();
				}));
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		bool isAgentEnabled = false;
		bool isAgentRegistered = false;
		private void toggleScheduledTask_Click(object sender, RoutedEventArgs e)
		{
			if (isAgentEnabled)
			{
				//PowerShellService.DisableSSAgent();
				Task.Run(() =>
				{
					//PowerShellService.DisableSSAgent();
					PowerShellService.CreateAndRunProcess(PsScriptType.Disable);
				});
			}
			else 
			{
				//PowerShellService.EnableSSAgent();
				Task.Run(() =>
				{
					//PowerShellService.EnableSSAgent();
					PowerShellService.CreateAndRunProcess(PsScriptType.Enable);
				});
			}

        }

		private void registerScheduledTask_Click(object sender, RoutedEventArgs e)
		{
			if (isAgentRegistered)
			{
				Task.Run(() =>
				{
					//PowerShellService.UnregisterSSAgent();
					PowerShellService.CreateAndRunProcess(PsScriptType.Unregister);
				});
			}
			else
			{
				Task.Run(() =>
				{
					//PowerShellService.RegisterSSAgent();
					PowerShellService.CreateAndRunProcess(PsScriptType.Register);
				});
			}
			//PowerShellService.RegisterSSAgent();
			
		}
	}

	public class BoardStatus
	{
		public int MainVoltage { get; set; }
		public int MainCurrent { get; set; }
		public int BatteryCurrent { get; set; }
		public int SolarVoltage { get; set; }
		public int SolarCurrent { get; set; }
		public int Temp { get; set; }
		public int Humidity { get; set; }
		public int Brightness { get; set; }
		public int Shock { get; set; }
		public int Door { get; set; }
		public int FrontLight { get; set; }
	}
}
