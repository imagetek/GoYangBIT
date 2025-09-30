using System;
using System.Collections.Generic;
using System.Linq;
using SSCommonNET;

namespace SSData
{
	/// <summary>
	/// BIT의 기본적인 설정 및 데이터관리
	/// </summary>
	public class BITDataManager
	{
		static BITDataManager()
		{
		}

		private static BIT_SYSTEM _bitSystem = new BIT_SYSTEM();
		public static BIT_SYSTEM BitSystem
		{
			get { return _bitSystem; }
			set { _bitSystem = value; }
		}

		private static List<BIT_DISPLAY> _bitDisplay = new List<BIT_DISPLAY>();
		public static List<BIT_DISPLAY> BitDisplays
		{
			get { return _bitDisplay; }
			set { _bitDisplay = value; }
		}

		private static BITENVConfigInfo _bitENVConfig = new BITENVConfigInfo();
		public static BITENVConfigInfo BitENVConfig
		{
			get { return _bitENVConfig; }
			set { _bitENVConfig = value; }
		}

		private static TB_SYSTEM _bitConfig = new TB_SYSTEM();
		public static TB_SYSTEM BitConfig
		{
			get { return _bitConfig; }
			set { _bitConfig = value; }
		}

		public static void Initialize()
		{
			try
			{
				if (System.IO.Directory.Exists(AppConfig.APPSStartPath) == false) System.IO.Directory.CreateDirectory(AppConfig.APPSStartPath);

				//DatabaseManager.InitializeDatabase();

				//edgar original way
				InitBIT_SYSTEM();				

				InitBIT_ENV_SETTING();

				InitSystemInfo();

				//InitBIT_DISPLAY();
				//edgar, new way

				//List<Task> tasks = [];
				//Task systemInit = Task.Run(() => InitBIT_SYSTEM());
				////Task displayInit = Task.Run(() => InitBIT_DISPLAY());
				//Task envSettingInit = Task.Run(() => InitBIT_ENV_SETTING());				
				//Task systemInfoInit = Task.Run(() => InitSystemInfo());

				//tasks.AddRange([systemInit, envSettingInit, systemInfoInit]);
				//Task task = Task.WhenAll(tasks);

				//task.Wait();
				//InitBIT_DISPLAY();
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		/// <summary>
		///  설정값 변경 1: System , 2 : 디스플레이 , 3: ENV설정 , 4 : LED설정 , 5 : 프로그램기본설정
		/// </summary>
		public static bool Refresh설정값(int nMode)
		{
			try
			{
				switch (nMode)
				{
					case 1: InitBIT_SYSTEM(); break;
					//case 2: InitBIT_DISPLAY(); break;
					case 3: InitBIT_ENV_SETTING(); break;
					//case 4: InitBIT_LEDConfig(); break;
					case 5: InitSystemInfo(); break;

				}
				return true;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
		}

		private static int _masterBrightness;
		public static int MasterBrightness { 
			get => _masterBrightness == 0 ? 99 : _masterBrightness; 
			set => _masterBrightness = value;
		}
		public static int MasterVolume{ get; set; }

		//public static bool IsExistLCD화면()
		//{
		//	try
		//	{
		//		List<BIT_DISPLAY> items = _bitDisplay.Where(data => data.DISP_GBN < 10).ToList();

		//		if (items != null && items.Count > 0) return true;

		//		return false;
		//	}
		//	catch (Exception ee)
		//	{
		//		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		return false;
		//	}
		//}

		private static void InitBIT_SYSTEM()
		{
			try
			{
				//string query = string.Format("SELECT * FROM BIT_SYSTEM ORDER BY SEQ_NO DESC LIMIT 1");

				//EDGAR this takes data from DB, I commented it out and assign hard coded value below, with new server ip an port
				//List<BIT_SYSTEM> items설정 = DatabaseManager.SELECT_BIT_SYSTEM_BY_QUERY(query);
				//List<BIT_SYSTEM> items설정 = new List<BIT_SYSTEM>();
				BIT_SYSTEM bitSystem = SettingsManager.GetBitSystemSettings();
				//if (items설정 != null && items설정.Count > 0)
				//{
				//	_bitSystem = items설정.First();
				//}
				if (bitSystem != null)
				{
					_bitSystem = bitSystem;
				}
				else
				{
					_bitSystem.SEQ_NO = 0;

					//_bitSystem.BIT_ID = 102;
					//_bitSystem.MOBILE_NO = 31672;
					//_bitSystem.STATION_NM = "운정광역보건지소(중)";

					// edgar new from raon
					_bitSystem.BIT_ID = "659";
					_bitSystem.MOBILE_NO = 30301;
					_bitSystem.STATION_NM = "금촌1동";

					//edgar dummy server 
					_bitSystem.SERVER_URL = "127.0.0.1";
					_bitSystem.SERVER_PORT = "13000";
					_bitSystem.SERVER_TYPE = 1;
					_bitSystem.FTP_GBN = 2;
					_bitSystem.FTP_IP = "172.8.1.61";
					_bitSystem.FTP_PORT = 2223;
					_bitSystem.FTP_USERID = "pjbit";
					_bitSystem.FTP_USERPWD = "pjbit@8732";
					_bitSystem.HTTP_URL = "172.8.1.60";
					_bitSystem.HTTP_PORT = 8080;
					_bitSystem.ENV_PORT_NM = "COM6";
					_bitSystem.ENV_BAUD_RATE = 57600;
				}
				//var s = Properties.Settings.Default;
				//DataProcessor.SaveDataToJsonFile(_bitSystem, "BitSystem.json");
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		//private static void InitBIT_DISPLAY()
		//{
		//	try
		//	{
		//		string query = string.Format("SELECT * FROM BIT_DISPLAY WHERE BIT_ID IN (0,{0}) ORDER BY SEQ_NO", BIT_ID);
		//		List<BIT_DISPLAY> items화면 = DatabaseManager.SELECT_BIT_DISPLAY_BY_QUERY(query);
		//		if (items화면 != null && items화면.Count > 0)
		//		{
		//			_bitDisplay = items화면;
		//		}
		//		else
		//		{
		//			_bitDisplay = new List<BIT_DISPLAY>();
		//		}
		//	}
		//	catch (Exception ee)
		//	{
		//		Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//	}
		//}

		private static void InitBIT_ENV_SETTING()
		{
			try
			{
				//string query = string.Format("SELECT * FROM BIT_ENV_SETTING WHERE BIT_ID IN (0,{0}) ORDER BY SEQ_NO DESC", BIT_ID);
				//List<BIT_ENV_SETTING> items설정 = DatabaseManager.SELECT_BIT_ENV_SETTING_BY_QUERY(query);
				//if (items설정 != null && items설정.Count > 0)
				//{
				//	_bitENVConfig.ITEM = items설정.First();					
				//}

				BIT_ENV_SETTING bitEnvSetting = SettingsManager.GetBitEnvSettings();

				if (bitEnvSetting != null)
				{
					_bitENVConfig.ITEM = bitEnvSetting;
				}
				else
				{
					_bitENVConfig.SEQ_NO = 0;
					_bitENVConfig.ITEM = new BIT_ENV_SETTING();
				}
				//DataProcessor.SaveDataToJsonFile(_bitENVConfig, "BitEnvConfig.json");
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private static void InitSystemInfo()
		{
			try
			{
				//string query = string.Format("SELECT * FROM TB_SYSTEM ORDER BY SEQ_NO DESC");
				//List<TB_SYSTEM> items설정= DatabaseManager.SELECT_TB_SYSTEM_BY_QUERY(query);
				//if (items설정 != null && items설정.Count > 0)
				//{
				//	_bitConfig = items설정.First();
				//}
				TB_SYSTEM tbSystem = SettingsManager.GetTbSystemSettings();
				if (tbSystem != null)
				{
					_bitConfig = tbSystem;
				}
				else
				{
					_bitConfig = new TB_SYSTEM();
					_bitConfig.LOGSAVE_DAY = 30;
					_bitConfig.LOGSAVE_PERCENT = 70;
				}
				//DataProcessor.SaveDataToJsonFile(_bitConfig, "BitConfig.json");
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public static string BIT_ID
		{
			get
			{
				return _bitSystem.BIT_ID;
			}
		}

		public static string 정류장명
		{
			get
			{
				return _bitSystem.STATION_NM;
			}
		}

		public static string 정류장모바일번호
		{
			get
			{
				return _bitSystem.MOBILE_NO.ToString();
			}
		}

		public static List<BisArrivalInformation> Get곧도착예정정보(List<BisArrivalInformation> datas)
		{
			try
			{
				List<BisArrivalInformation> items = datas.Where(data => data.OperationStatus.Equals(4) || data.OperationStatus.Equals(3)).ToList(); //edgar in gwachon  normal operation is 3 or 4

				var item설정 = _bitENVConfig.ITEM;
				switch (item설정.ArriveSoonGBN)
				{
					case 0://시간
						items = items.Where(data => data.EstimatedTimeOfArrival <= item설정.ArriveSoonTimeGBN).ToList();
						break;
					case 1://정류장
						items = items.Where(data => data.NumberOfRemainingStops <= item설정.ArriveSoonStationGBN).ToList();
						break;

					case 2://시간+정류장
						items = items.Where(data => data.EstimatedTimeOfArrival <= item설정.ArriveSoonTimeGBN
						&& data.NumberOfRemainingStops <= item설정.ArriveSoonStationGBN).ToList();
						break;

					case 3://시간 or 정류장
						items = items.Where(data => data.EstimatedTimeOfArrival <= item설정.ArriveSoonTimeGBN
						|| data.NumberOfRemainingStops <= item설정.ArriveSoonStationGBN).ToList();
						break;
				}

				switch (item설정.BITOrderGBN)
				{
					case 0: //시간순
						items = items.OrderByDescending(data => data.OperationStatus).ThenBy(data => data.EstimatedTimeOfArrival).ToList();
						break;

					case 1: //정류장순
						items = items.OrderByDescending(data => data.OperationStatus).ThenBy(data => data.NumberOfRemainingStops).ToList();
						break;

					case 2: //정류장순
						items = items.OrderByDescending(data => data.OperationStatus).ThenBy(data => data.EstimatedTimeOfArrival).ThenBy(data => data.NumberOfRemainingStops).ToList();
						break;
				}

				return items;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}

		public static bool Check곧도착YN(BisArrivalInformation data)
		{
			try
			{
				bool 곧도착YN = false;
				if (data.OperationStatus == 1 || data.OperationStatus == 2) return false;

				var item설정 = _bitENVConfig.ITEM;
				switch (item설정.ArriveSoonGBN)
				{
					case 0://시간
						if (data.EstimatedTimeOfArrival <= item설정.ArriveSoonTimeGBN)
						{
							곧도착YN = true;
						}
						break;
					case 1://정류장
						if (data.NumberOfRemainingStops <= item설정.ArriveSoonStationGBN)
						{
							곧도착YN = true;
						}
						break;

					case 2://시간+정류장
						if (data.NumberOfRemainingStops <= item설정.ArriveSoonStationGBN && data.EstimatedTimeOfArrival <= item설정.ArriveSoonTimeGBN)
						{
							곧도착YN = true;
						}
						break;

					case 3://시간 or 정류장
						if (data.NumberOfRemainingStops <= item설정.ArriveSoonStationGBN || data.EstimatedTimeOfArrival <= item설정.ArriveSoonTimeGBN)
						{
							곧도착YN = true;
						}
						break;
				}

				return 곧도착YN;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
		}

		public static List<BisArrivalInformation> Get도착정보목록(List<BisArrivalInformation> datas)
		{
			try
			{
				DateTime dt = DateTime.Now;

				List <BisArrivalInformation> itemsBIS = datas.Where(data => dt.CompareTo(data.TimeOfArrival.AddSeconds(35)) < 0).ToList();

				List<BisArrivalInformation> items곧도착 = [];
				List<BisArrivalInformation> items곧도착외 = [];
				var item설정 = _bitENVConfig.ITEM;
				foreach (BisArrivalInformation itemBIS in itemsBIS)
				{
					bool b곧도착YN = Check곧도착YN(itemBIS);
					if (b곧도착YN == true)
					{
						items곧도착.Add(itemBIS);
					}
					else
					{
						items곧도착외.Add(itemBIS);
					}
				}

				List<BisArrivalInformation> items = [];


				//edgar here first we add arriving soon buses to the list, and then put buses that are not in operation mode. if we want to show buses not in operation mode always on top, then should change below part. 
				//maybe even get rid of lines above and only apply order and thenby, without checking if bus is arriving soon.
				switch (item설정.BITOrderGBN)
				{
					case 0: //시간순
						if (items곧도착 != null && items곧도착.Count > 0) items.AddRange(items곧도착.OrderByDescending(data => data.OperationStatus).ThenBy(data => data.EstimatedTimeOfArrival));
						if (items곧도착외 != null && items곧도착외.Count > 0) items.AddRange(items곧도착외.OrderByDescending(data => data.OperationStatus).ThenBy(data => data.EstimatedTimeOfArrival));
						break;

					case 1: //정류장순						
						if (items곧도착 != null && items곧도착.Count > 0) items.AddRange(items곧도착.OrderByDescending(data => data.OperationStatus).ThenBy(data => data.NumberOfRemainingStops));
						if (items곧도착외 != null && items곧도착외.Count > 0) items.AddRange(items곧도착외.OrderByDescending(data => data.OperationStatus).ThenBy(data => data.NumberOfRemainingStops));
						break;

					case 2: //정류장순
						if (items곧도착 != null && items곧도착.Count > 0) items.AddRange(items곧도착.OrderByDescending(data => data.OperationStatus).ThenBy(data => data.EstimatedTimeOfArrival).ThenBy(data => data.NumberOfRemainingStops));
						if (items곧도착외 != null && items곧도착외.Count > 0) items.AddRange(items곧도착외.OrderByDescending(data => data.OperationStatus).ThenBy(data => data.EstimatedTimeOfArrival).ThenBy(data => data.NumberOfRemainingStops));
						break;
				}
				return items;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}

		#region 단말기환경보드

		public static bool Set파라메터변경(BIT_ENV_SETTING data)
		{
			try
			{
				if (data == null) return false;

				if (_bitENVConfig == null || _bitENVConfig.SEQ_NO == 0)
				{
					data.SEQ_NO = 0;
					_bitENVConfig.ITEM = data;
					//return DatabaseManager.INSERT_BIT_ENV_SETTING(data);
					return SettingsManager.SaveBitEnvSettings(data);
				}
				else
				{
					var item설정 = _bitENVConfig.ITEM;

					int nGBN = data.REGDATE.CompareTo(item설정.REGDATE);
					if (nGBN > 0)
					{
						item설정 = data;
						Log4Manager.WriteSocketLog(Log4Level.Info, "[BIT] 센터에서 수신한 상태제어값을 저장했습니다.");

						//return DatabaseManager.INSERT_BIT_ENV_SETTING(data);
						return SettingsManager.SaveBitEnvSettings(data);
					}
					else
					{
						Log4Manager.WriteSocketLog(Log4Level.Info, "[BIT] 센터에서 수신한 상태제어값을 저장을 생략했습니다.");
					}
				}
				return true;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
		}

		#endregion

		#region 환경보드 관련

		public static SSNS_ENV2_STATE env환경 = null;
		public static void Set환경보드값(SSNS_ENV2_STATE data)
		{
			try
			{
				if (data == null) return;
				if (env환경 == null) env환경 = new SSNS_ENV2_STATE();

				env환경 = data;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public static PAJU_BIT_단말기상태 Get단말기상태보고()
		{
			try
			{
				PAJU_BIT_단말기상태 bit상태 = new PAJU_BIT_단말기상태();
				if (env환경 == null)
				{
					//20220928 BHA 수정
					TraceManager.AddInfoLog("환경보드와 연결이 되지않아 기본값으로 단말기상태보고합니다.");
					bit상태.n온도 = 0;
					bit상태.n습도 = 0;
					bit상태.n도어상태 = 0;

					bit상태.n동작감지센서LCDOnOFF = 0;
					bit상태.nLCD전류감지 = (byte)0x00;//0 :미사용
					bit상태.n시험운영중표출여부 = 0;

					bit상태.bLCDOnOff상태 = (byte)0x00;
					bit상태.bFAN동작상태 = (byte)0x01; //1 : 중지중
					bit상태.b웹카메라동작상태 = bWebCamAciveYN == true ? (byte)0x01 : (byte)0x00; //0 : 중지중
					bit상태.b히터동작상태 = (byte)0x01;//1 : 중지중
				}
				else
				{
					bit상태.n온도 = env환경.Temparature1;
					bit상태.n습도 = env환경.Humidity;
					bit상태.n도어상태 = env환경.Door == 1 ? 0 : 1;

					bit상태.n동작감지센서LCDOnOFF = 0;
					bit상태.nLCD전류감지 = (byte)0x00;//0 :미사용
					bit상태.n시험운영중표출여부 = _bitENVConfig.ITEM.TestOperationDisplayYN;

					bit상태.bLCDOnOff상태 = (byte)0x00;
					bit상태.bFAN동작상태 = (byte)0x01; //1 : 중지중
					bit상태.b웹카메라동작상태 = bWebCamAciveYN == true ? (byte)0x01 : (byte)0x00; //0 : 중지중
					bit상태.b히터동작상태 = (byte)0x01;//1 : 중지중

					if (env환경.Controls != null)
					{
						bit상태.bLCDOnOff상태 = env환경.Controls.bDC12V1 ? (byte)0x01 : (byte)0x00;
						bit상태.bFAN동작상태 = env환경.Controls.bDCFAN ? (byte)0x01 : (byte)0x00;
						bit상태.b히터동작상태 = env환경.Controls.bAC1 ? (byte)0x01 : (byte)0x00;
					}
				}
				return bit상태;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}

		#endregion

		public static bool bWebCamAciveYN { get; set; }
		
	}
}
