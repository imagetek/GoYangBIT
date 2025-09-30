using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;

namespace SSData
{
    public class CommonProc
    {  
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        static IntPtr wow64Value = IntPtr.Zero;

        public static bool CloseKeyboardProc()
        {
            try
            {
                System.Diagnostics.Process[] myProcesses = System.Diagnostics.Process.GetProcessesByName("osk");
                foreach (System.Diagnostics.Process myProcess in myProcesses)
                {
                    myProcess.CloseMainWindow();
                    CommonUtils.WaitTime(50, true);
                    myProcess.Kill();
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

        public static bool StartKeyboardProc()
        {
            try
            {
                System.Diagnostics.Process[] myProcesses = System.Diagnostics.Process.GetProcessesByName("osk");
                if (myProcesses.Length == 0)
                {
                    if (Environment.Is64BitOperatingSystem == true)
                    {
                        Wow64DisableWow64FsRedirection(ref wow64Value);
                    }
                    string oskFilePath = Environment.GetFolderPath(Environment.SpecialFolder.System);
                    Console.WriteLine(oskFilePath);
                    string run = System.IO.Path.Combine(oskFilePath, "osk.exe");
                    if (System.IO.File.Exists(run) == true)
                    {
                        System.Diagnostics.Process.Start(run);
                    }

                    if (Environment.Is64BitOperatingSystem == true)
                    {
                        Wow64RevertWow64FsRedirection(wow64Value);
                    }
                }

                return true;
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool InitDirectory정리()
        {
            try
            {
                string tempDIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "TEMP");
                if (System.IO.Directory.Exists(tempDIR) == true)
                {
                    try
                    {
                        System.IO.Directory.Delete(tempDIR, true);
                    }
                    catch (System.IO.IOException ex)
                    {
                        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                    }
                }

				Console.WriteLine("[폴더정리] temp_download 하위폴더");
				string 임시다운DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_download");
				if (System.IO.Directory.Exists(임시다운DIR) == true)
				{
					try
					{
						System.IO.Directory.Delete(임시다운DIR, true);
					}
					catch (System.IO.IOException ex)
					{
						System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
					}
				}				

				Console.WriteLine("[폴더정리] temp_mapdata");
				string temp지도데이터 = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_mapdata");
				if (System.IO.Directory.Exists(temp지도데이터) == true)
				{
					try
					{
						System.IO.Directory.Delete(temp지도데이터, true);
					}
					catch (System.IO.IOException ex)
					{
						System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
					}
				}

				Console.WriteLine("[폴더정리] 다음로드뷰관련 roadview.swf삭제");

				string OS_VER_NM = RegisterManager.GET_OS_NAME();
				if (OS_VER_NM != null && OS_VER_NM.Equals("") == false)
				{
					if (OS_VER_NM.Contains("Windows 7") == true)
					{
						try
						{
							Console.WriteLine("[폴더정리] 다음로드뷰관련 - 윈도우7");
							string uprofileFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
							string rvPath = System.IO.Path.Combine(uprofileFolder, "AppData/Local/Microsoft/Windows/Temporary Internet Files");
							string[] files = System.IO.Directory.GetFiles(rvPath, "roadview*.swf*", System.IO.SearchOption.AllDirectories);

							foreach (string file in files)
								System.IO.File.Delete(file);
						}
						catch (System.IO.IOException ex)
						{
							System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
						}
					}
					else if (OS_VER_NM.Contains("Windows 10") == true)
					{
						try
						{
							Console.WriteLine("[폴더정리] 다음로드뷰관련 - 윈도우 10");
							string uprofileFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
							string rvPath = System.IO.Path.Combine(uprofileFolder, "Microsoft\\Windows\\INetCache\\IE");
							//C: \Users\DEVPC\AppData\Local\Microsoft\Windows\INetCache\Low\IE\IK7AN81A
							//C:\Users\DEVPC\AppData\Local\Microsoft\Windows\INetCache\IE\4KDNTD22
							string[] files = System.IO.Directory.GetFiles(rvPath, "roadview*.swf*", System.IO.SearchOption.AllDirectories);

							foreach (string file in files)
								System.IO.File.Delete(file);
						}
						catch (System.IO.IOException ex)
						{
							System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
						}
					}
				}

				Console.WriteLine("[폴더정리] temp_updata 하위폴더");
				string updataDIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_updata");
				if (System.IO.Directory.Exists(updataDIR) == true)
				{
					try
					{
						System.IO.Directory.Delete(updataDIR, true);
					}
					catch (System.IO.IOException ex)
					{
						System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
					}
				}
				return true;
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
                return false;
            }
        }

		public static void Init기초파일()
		{
			try
			{
				string 기본DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "DB");
				if (System.IO.Directory.Exists(기본DIR) == false) System.IO.Directory.CreateDirectory(기본DIR);

				//string SCode파일 = System.IO.Path.Combine(AppConfig.APPSStartPath, "DB", "SCode.DAT");
				string DB파일 = System.IO.Path.Combine(AppConfig.APPSStartPath, "DB", "BIT.db3");
				//string mSCode파일 = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "data", "SCode.DAT");
				//if (System.IO.File.Exists(SCode파일) == false)
				//{
				//	try
				//	{
				//		System.IO.File.Copy(mSCode파일, SCode파일);
				//		TraceManager.AddInfoLog("[파일복사] SCode파일을 저장했습니다.");

				//		SCodeManager.Initialize(기본DIR);
				//	}
				//	catch (System.IO.IOException ex)
				//	{
				//		Console.WriteLine(string.Format("{0}\r\n{1}", ex.StackTrace, ex.Message));
				//	}
				//}

				string mDB파일 = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "data", "BIT.db3");
				if (System.IO.File.Exists(DB파일) == false)
				{
					try
					{						
						System.IO.File.Copy(mDB파일, DB파일);
						TraceManager.AddInfoLog("[파일복사] DB파일을 저장했습니다.");
					}
					catch (System.IO.IOException ex)
					{
						Console.WriteLine(string.Format("{0}\r\n{1}", ex.StackTrace, ex.Message));
					}
				}

				//string log4Net파일 = System.IO.Path.Combine(AppConfig.APPSStartPath, "Log4Net.xml");
				//if (System.IO.File.Exists(log4Net파일) == false)
				//{
				//	try
				//	{
				//		string mlog파일 = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "data", "Log4Net.xml");
				//		System.IO.File.Copy(mlog파일, log4Net파일);
				//		TraceManager.AddInfoLog("[파일복사] log4설정파일을 저장했습니다.");
				//	}
				//	catch (System.IO.IOException ex)
				//	{
				//		Console.WriteLine(string.Format("{0}\r\n{1}", ex.StackTrace, ex.Message));
				//	}
				//}
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
			}
		}

		//하위Directory제거
		public static bool ClearLowDirectory(string directory, int days)
		{
			try
			{
				if (System.IO.Directory.Exists(directory) == false) return false;

				DateTime dt확인= DateTime.Now.AddDays(-1 * days);
				string[] files = System.IO.Directory.GetFiles(directory, "*.log", System.IO.SearchOption.AllDirectories);
				foreach (string file in files)
				{
					System.IO.FileInfo fi = new System.IO.FileInfo(file);
					string[] datas = fi.Name.Split('_');
					if (datas.Length > 1)
					{
						DateTime dt파일 = ConvertUtils.DateTimeByString(datas[0].Replace("-", "")).Value;
						int res = dt파일.CompareTo(dt확인);
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
					}
					fi = null;
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

		//지정Directory정리
		public static bool ClearDirectoryRemove(string directory, int days)
		{
			try
			{
				if (System.IO.Directory.Exists(directory) == false) return false;

				string[] _dirs = System.IO.Directory.GetDirectories(directory, "*.*", System.IO.SearchOption.AllDirectories);
				foreach (string _dir in _dirs)
				{
					System.IO.DirectoryInfo _di = new System.IO.DirectoryInfo(_dir);
					DateTime dt확인= DateTime.Now.AddDays(-1 * days);
					DateTime dt파일 = ConvertUtils.DateTimeByString(_di.Name).Value;
					int res = dt파일.CompareTo(dt확인);
					if (res < 0)
					{
						try
						{
							_di.Delete(true);
						}
						catch (Exception ex)
						{
							TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
						}
					}
					_di = null;
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

		public static void 프로세서종료Proc(string PRC_NM)
		{
			try
			{
				System.Diagnostics.Process[] processesByName = System.Diagnostics.Process.GetProcessesByName(PRC_NM);
				if (processesByName.Length > 0)
				{
					for (int i = 0; i < processesByName.Length; i++)
					{
						processesByName[i].Kill();
						CommonUtils.WaitTime(50, true);
					}
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
			}
		}

		/// <summary>
		/// 20220729 bha 프로그램 업데이트 체크
		/// </summary>
		public static bool Check프로그램업데이트(string DIR)
		{
			try
			{
				string 기본DIR = System.IO.Path.Combine(DIR, "update_apps");
				if (System.IO.Directory.Exists(기본DIR) == true)
				{
					List<string> items파일 = System.IO.Directory.GetFiles(기본DIR, "*.dll", System.IO.SearchOption.AllDirectories).ToList();
					items파일.AddRange(System.IO.Directory.GetFiles(기본DIR, "*.exe", System.IO.SearchOption.AllDirectories).ToList());

					if (items파일 != null && items파일.Count > 0)
					{						
						return true;
					}
				}
				return false;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
				return false;
			}
		}

		public static bool IS_TIME_YN(string TIME, bool IS_OVERTIME = false, int interval = 10)
		{
			try
			{
				if (TIME.Equals("9999") == true) return true;

				DateTime dtNow = DateTime.Now;
				int nHOUR = Convert.ToInt32(TIME.Substring(0, 2));
				int nMIN = Convert.ToInt32(TIME.Substring(2, 1)) * 10;
				DateTime _dtMIN = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, nHOUR, nMIN, 0);
				DateTime _dtMAX = _dtMIN.AddMinutes(interval);
				if (dtNow >= _dtMIN && dtNow <= _dtMAX)
				{
					TraceManager.AddLog(string.Format("[종료시간확인] 설정시간 포함: {0}", TIME));
					return true;
				}

				if (dtNow >= _dtMAX && IS_OVERTIME == true)
				{
					TraceManager.AddLog(string.Format("[종료시간확인] 설정시간 초과: {0}", TIME));
					return true;
				}

				return false;
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
		}

		public static int IS_DAY_YN(string DAYS)
		{
			try
			{
				DateTime dtNow = DateTime.Now;
				int todayNo = Convert.ToInt32(dtNow.DayOfWeek);

				string[] _days = DAYS.Split('|');
				int idxNo = 0;
				foreach (string day in _days)
				{
					idxNo++;
					if (day.Equals("") == true) continue;
					if (CommonUtils.IsNumeric(day) == false) continue;
					if (Convert.ToInt32(day) == todayNo)
					{
						return idxNo - 1;
					}
				}
				return -1;
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return -1;
			}
		}

		public static string Compress로그정보()
		{
			try
			{
				Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile();

				string 기본DIR = System.IO.Directory.GetCurrentDirectory();
				string 로그DIR = System.IO.Path.Combine(기본DIR, "LOG");
				string 에러DIR = System.IO.Path.Combine(기본DIR, "Trace");

				if (System.IO.Directory.Exists(로그DIR) == true)
				{
					zip.AddDirectory(로그DIR, "LOG");
				}

				if (System.IO.Directory.Exists(에러DIR) == true)
				{
					zip.AddDirectory(에러DIR, "Trace");
				}

				string 압축DIR = System.IO.Path.Combine(DataManager.Get업데이트DIR(), "LOG");
				if (System.IO.Directory.Exists(압축DIR) == false) System.IO.Directory.CreateDirectory(압축DIR);

				string 압축FILE = System.IO.Path.Combine(압축DIR, string.Format("{0}.zip", DateTime.Now.ToString("yyyyMMddHHmm")));
				if (System.IO.File.Exists(압축FILE) == true)
				{
					try
					{
						System.IO.File.Delete(압축FILE);
					}
					catch (System.IO.IOException ex)
					{
						Console.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
					}
				}
				zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
				//zip.CompressionMethod = Ionic.Zip.CompressionMethod.
				zip.AlternateEncoding = Encoding.UTF8;
				zip.Save(압축FILE);

				return 압축FILE;
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return "";
			}
		}

		#region 미디어 확장자 

		static List<string> 동영상Extension = new List<string>();
		static List<string> 이미지Extension = new List<string>();
		public static List<string> MovieExtension
		{
			get
			{
				if (동영상Extension == null || 동영상Extension.Count == 0) Int미디어확장자Proc();

				return 동영상Extension;
			}
		}
		public static List<string> ImageExtension
		{
			get
			{
				if (이미지Extension == null || 이미지Extension.Count == 0) Int미디어확장자Proc();

				return 이미지Extension;
			}
		}

		private static void Int미디어확장자Proc()
		{
			try
			{
				if (동영상Extension == null) 동영상Extension = new List<string>();
				동영상Extension.Clear();
				동영상Extension.Add(".mpeg");
				동영상Extension.Add(".mpg");
				동영상Extension.Add(".mp4");
				동영상Extension.Add(".mov");
				동영상Extension.Add(".mpeg");
				동영상Extension.Add(".avi");
				동영상Extension.Add(".asf");
				동영상Extension.Add(".asx");
				동영상Extension.Add(".wmv");

				if (이미지Extension == null) 이미지Extension = new List<string>();
				이미지Extension.Clear();

				이미지Extension.Add(".jpeg");
				이미지Extension.Add(".jpg");
				//이미지Extension.Add(".gif");
				이미지Extension.Add(".bmp");
				이미지Extension.Add(".png");
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		/// <summary>
		/// 파일 확장자 검색 / 이미지 1 , 동영상 2 , GIF 3
		/// </summary>
		public static int Search파일확장자(string ext)
		{
			try
			{
				string 확장자 = ext.ToLower();
				if (ImageExtension.Contains(확장자) == true) return 1;
				if (MovieExtension.Contains(확장자) == true) return 2;
				if (확장자.Equals(".gif") == true) return 3;

				return 0;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return -1;
			}
		}

		#endregion

		//public static bool Clear임시DIR()
		//{
		//    try
		//    {
		//        Console.WriteLine("[폴더정리] temp_download 하위폴더");
		//        string 임시다운DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_download");
		//        if (System.IO.Directory.Exists(임시다운DIR) == true)
		//        {
		//            try
		//            {
		//                System.IO.Directory.Delete(임시다운DIR, true);
		//            }
		//            catch (System.IO.IOException ex)
		//            {
		//                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
		//            }
		//        }

		//        Console.WriteLine("[폴더정리] temp");
		//        string tempDIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp");
		//        if (System.IO.Directory.Exists(tempDIR) == true)
		//        {
		//            try
		//            {
		//                System.IO.Directory.Delete(tempDIR, true);
		//            }
		//            catch (System.IO.IOException ex)
		//            {
		//                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
		//            }
		//        }

		//        Console.WriteLine("[폴더정리] temp_mapdata");
		//        string temp지도데이터 = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_mapdata");
		//        if (System.IO.Directory.Exists(temp지도데이터) == true)
		//        {
		//            try
		//            {
		//                System.IO.Directory.Delete(temp지도데이터, true);
		//            }
		//            catch (System.IO.IOException ex)
		//            {
		//                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
		//            }
		//        }

		//        Console.WriteLine("[폴더정리] 다음로드뷰관련 roadview.swf삭제");

		//        string OS_VER_NM = RegisterManager.GET_OS_NAME();
		//        if (OS_VER_NM != null && OS_VER_NM.Equals("") == false)
		//        {
		//            if (OS_VER_NM.Contains("Windows 7") == true)
		//            {
		//                try
		//                {
		//                    Console.WriteLine("[폴더정리] 다음로드뷰관련 - 윈도우7");
		//                    string uprofileFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		//                    string rvPath = System.IO.Path.Combine(uprofileFolder, "AppData/Local/Microsoft/Windows/Temporary Internet Files");
		//                    string[] files = System.IO.Directory.GetFiles(rvPath, "roadview*.swf*", System.IO.SearchOption.AllDirectories);

		//                    foreach (string file in files)
		//                        System.IO.File.Delete(file);
		//                }
		//                catch (System.IO.IOException ex)
		//                {
		//                    System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
		//                }
		//            }
		//            else if (OS_VER_NM.Contains("Windows 10") == true)
		//            {
		//                try
		//                {
		//                    Console.WriteLine("[폴더정리] 다음로드뷰관련 - 윈도우 10");
		//                    string uprofileFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		//                    string rvPath = System.IO.Path.Combine(uprofileFolder, "Microsoft\\Windows\\INetCache\\IE");
		//                    //C: \Users\DEVPC\AppData\Local\Microsoft\Windows\INetCache\Low\IE\IK7AN81A
		//                    //C:\Users\DEVPC\AppData\Local\Microsoft\Windows\INetCache\IE\4KDNTD22
		//                    string[] files = System.IO.Directory.GetFiles(rvPath, "roadview*.swf*", System.IO.SearchOption.AllDirectories);

		//                    foreach (string file in files)
		//                        System.IO.File.Delete(file);
		//                }
		//                catch (System.IO.IOException ex)
		//                {
		//                    System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
		//                }
		//            }
		//        }

		//        Console.WriteLine("[폴더정리] backup 하위폴더");
		//        string backupDIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "backup");
		//        if (System.IO.Directory.Exists(backupDIR) == true)
		//        {
		//            try
		//            {
		//                Clear하위DIR(backupDIR, 10);
		//            }
		//            catch (System.IO.IOException ex)
		//            {
		//                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
		//            }
		//        }

		//        Console.WriteLine("[폴더정리] shp 하위폴더");
		//        string shpDIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "SHP");
		//        if (System.IO.Directory.Exists(shpDIR) == true)
		//        {
		//            try
		//            {
		//                Clear하위DIR(shpDIR, 10);
		//            }
		//            catch (System.IO.IOException ex)
		//            {
		//                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
		//            }
		//        }

		//        //Console.WriteLine("[폴더정리] mapdata 하위폴더");
		//        //string 지도DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "mapdata");
		//        //if (System.IO.Directory.Exists(지도DIR) == true)
		//        //{
		//        //    try
		//        //    {
		//        //        Clear지도백업DIR(지도DIR, 10);
		//        //    }
		//        //    catch (System.IO.IOException ex)
		//        //    {
		//        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
		//        //    }
		//        //}

		//        return true;
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
		//        return false;
		//    }
		//}

		//public static bool Clear업데이트DIR()
		//{
		//    try
		//    {
		//        Console.WriteLine("[폴더정리] update_mapdata 하위폴더");
		//        string 지도DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "update_mapdata");
		//        if (System.IO.Directory.Exists(지도DIR) == true)
		//        {
		//            try
		//            {
		//                System.IO.Directory.Delete(지도DIR, true);
		//            }
		//            catch (System.IO.IOException ex)
		//            {
		//                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
		//            }
		//        }

		//        Console.WriteLine("[폴더정리] update_media 하위폴더");
		//        string 미디어DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "update_media");
		//        if (System.IO.Directory.Exists(미디어DIR) == true)
		//        {
		//            try
		//            {
		//                System.IO.Directory.Delete(미디어DIR, true);
		//            }
		//            catch (System.IO.IOException ex)
		//            {
		//                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
		//            }
		//        }

		//        Console.WriteLine("[폴더정리] update_bgdata 하위폴더");
		//        string 배경DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "update_bgdata");
		//        if (System.IO.Directory.Exists(배경DIR) == true)
		//        {
		//            try
		//            {
		//                System.IO.Directory.Delete(배경DIR, true);
		//            }
		//            catch (System.IO.IOException ex)
		//            {
		//                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
		//            }
		//        }

		//        Console.WriteLine("[폴더정리] temp_contents 하위폴더");
		//        string 컨텐츠DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_contents");
		//        if (System.IO.Directory.Exists(컨텐츠DIR) == true)
		//        {
		//            try
		//            {
		//                System.IO.Directory.Delete(컨텐츠DIR, true);
		//            }
		//            catch (System.IO.IOException ex)
		//            {
		//                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
		//            }
		//        }

		//        Console.WriteLine("[폴더정리] temp_waitscreen 하위폴더");
		//        string 대기화면DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_waitscreen");
		//        if (System.IO.Directory.Exists(대기화면DIR) == true)
		//        {
		//            try
		//            {
		//                System.IO.Directory.Delete(대기화면DIR, true);
		//            }
		//            catch (System.IO.IOException ex)
		//            {
		//                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
		//            }
		//        }

		//        return true;
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        return false;
		//    }
		//}



		//public static string Get파일명(string FILE_NM)
		//{
		//    try
		//    {
		//        System.IO.FileInfo fi = new System.IO.FileInfo(FILE_NM);

		//        string NM = fi.Name.Replace(fi.Extension, "");

		//        fi = null;

		//        return NM;
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        return "";
		//    }
		//}

		//public static bool 압축해제Proc(string zipfile, string saveDIR, bool 삭제YN = true)
		//{
		//    try
		//    {
		//        if (System.IO.File.Exists(zipfile) == true)
		//        {
		//            System.IO.FileInfo fi = new System.IO.FileInfo(zipfile);
		//            Ionic.Zip.ZipFile _unzip = new Ionic.Zip.ZipFile(fi.FullName);

		//            if (System.IO.Directory.Exists(saveDIR) == false)
		//            {
		//                System.IO.Directory.CreateDirectory(saveDIR);
		//                CommonUtils.WaitTime(50, true);
		//            }

		//            _unzip.ExtractAll(saveDIR, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
		//            TraceManager.AddLine(string.Format("[압축해제] 파일명 : {0}", zipfile));
		//            CommonUtils.WaitTime(50, true);

		//            _unzip.Dispose();
		//            _unzip = null;
		//            CommonUtils.WaitTime(50, true);

		//            if (삭제YN == true) fi.Delete();
		//            fi = null;
		//            CommonUtils.WaitTime(50, true);
		//            return true;
		//        }
		//        return false;
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        return false;
		//    }
		//}

		//public static bool Update파일Proc(TCP명령어Type item)
		//{
		//    try
		//    {
		//        string 원본DIR = DataManager.Get원본DIR(item);
		//        string 저장DIR = DataManager.Get저장DIR(item);

		//        if (System.IO.Directory.Exists(원본DIR) == true)
		//        {
		//            TraceManager.AddLine(string.Format("[업데이트] 원본폴더 확인 {0}", 원본DIR));

		//            string 삭제목록NM = System.IO.Path.Combine(원본DIR, "DFILE.DAT");
		//            if (System.IO.File.Exists(삭제목록NM) == true)
		//            {
		//                Delete목록Proc(삭제목록NM);
		//                CommonUtils.WaitTime(50, true);
		//            }

		//            Move폴더Proc(원본DIR, 저장DIR);
		//            TraceManager.AddLine(string.Format("[업데이트] {0}→{1} 완료", 원본DIR, 저장DIR));
		//        }
		//        return true;
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
		//        return false;
		//    }
		//}

		//public static bool Delete파일목록Proc(TCP명령어Type item, List<string> files)
		//{
		//    try
		//    {
		//        if (files.Count <= 1) return false;

		//        TraceManager.AddLine(string.Format("[업데이트] 파일삭제 시작 {0}", files.Count));
		//        string 원본DIR = DataManager.Get원본DIR(item);

		//        if (System.IO.Directory.Exists(원본DIR) == false)
		//        {
		//            System.IO.Directory.CreateDirectory(원본DIR);
		//            CommonUtils.WaitTime(50, true);
		//        }

		//        string FILE_NM = System.IO.Path.Combine(원본DIR, "DFILE.DAT");
		//        System.IO.StreamWriter sw = System.IO.File.AppendText(FILE_NM);
		//        foreach (string file in files)
		//        {
		//            //if (file.Equals("DELETE_NOTICE_FILE") == true) continue;
		//            System.IO.FileInfo fi = new System.IO.FileInfo(file);
		//            try
		//            {
		//                fi.Delete();
		//                TraceManager.AddLine(string.Format("삭제완료 : {0}", file), "파일삭제");
		//            }
		//            catch (System.IO.IOException ex)
		//            {
		//                sw.WriteLine(fi.FullName);
		//                TraceManager.AddLine(string.Format("삭제실패&삭제목록추가 : {0}", file), "파일삭제");
		//                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));

		//            }
		//            fi = null;
		//            CommonUtils.WaitTime(50, true);
		//        }
		//        sw.Close();

		//        return true;
		//    }
		//    catch (Exception ee)
		//    {
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        return false;
		//    }
		//}

		//static bool Delete목록Proc(string item)
		//{
		//    try
		//    {
		//        string[] 삭제Files = System.IO.File.ReadAllLines(item);
		//        foreach (string file in 삭제Files)
		//        {
		//            try
		//            {
		//                System.IO.File.Delete(file);
		//            }
		//            catch (System.IO.IOException ex)
		//            {
		//                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
		//            }
		//        }

		//        try
		//        {
		//            System.IO.File.Delete(item);
		//        }
		//        catch (System.IO.IOException ex)
		//        {
		//            System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
		//        }

		//        return true;
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
		//        return false;
		//    }
		//}

		//static bool Move폴더Proc(string 원본DIR, string 목표DIR)
		//{
		//    try
		//    {
		//        string[] items업데이트 = System.IO.Directory.GetFiles(원본DIR, "*.*", System.IO.SearchOption.TopDirectoryOnly);
		//        foreach (string item in items업데이트)
		//        {
		//            System.IO.FileInfo fi = new System.IO.FileInfo(item);
		//            string 목표_FILE = System.IO.Path.Combine(목표DIR, fi.Name);
		//            if (System.IO.File.Exists(목표_FILE) == true)
		//            {
		//                string 백업기본DIR = System.IO.Path.Combine(목표DIR, "BACKUP");//
		//                string 백업상세DIR = System.IO.Path.Combine(백업기본DIR, string.Format("{0}", DateTime.Now.ToString("yyyyMMdd")));
		//                if (System.IO.Directory.Exists(백업상세DIR) == false)
		//                {
		//                    System.IO.Directory.CreateDirectory(백업상세DIR);
		//                    CommonUtils.WaitTime(50, true);
		//                }
		//                Console.WriteLine("## 기존파일 백업폴더로 이동 : {0}", 목표_FILE);
		//                try
		//                {
		//                    System.IO.FileInfo fi백업 = new System.IO.FileInfo(목표_FILE);

		//                    fi백업.Name.Replace(fi.Extension, "");
		//                    string 백업FILE_NM = string.Format("{0}_{1}{2}"
		//                        , fi백업.Name.Replace(fi.Extension, "")
		//                        , DateTime.Now.ToString("HHmm")
		//                        , fi백업.Extension);

		//                    string 백업FILE_NM_V2 = System.IO.Path.Combine(백업상세DIR, 백업FILE_NM);
		//                    if (System.IO.File.Exists(백업FILE_NM_V2) == false)
		//                    {
		//                        fi백업.MoveTo(백업FILE_NM_V2);
		//                        Console.WriteLine("## 미디어 데이터 백업 : {0}", 백업FILE_NM_V2);
		//                    }
		//                    else
		//                    {
		//                        fi백업.Delete();
		//                        Console.WriteLine("## 미디어 데이터 삭제 : {0}", 백업FILE_NM_V2);
		//                    }
		//                    fi백업 = null;
		//                    CommonUtils.WaitTime(50, true);
		//                }
		//                catch (System.IO.IOException ex)
		//                {
		//                    TraceManager.AddLog(string.Format("## {0}\r\n{1} ##", ex.StackTrace, ex.Message));
		//                }
		//                try
		//                {
		//                    fi.MoveTo(목표_FILE);
		//                    Console.WriteLine("## 미디어 업데이트 : {0}", 목표_FILE);
		//                }
		//                catch (System.IO.IOException ex)
		//                {
		//                    TraceManager.AddLog(string.Format("## {0}\r\n{1} ##", ex.StackTrace, ex.Message));
		//                }
		//            }
		//            else
		//            {
		//                try
		//                {
		//                    if (System.IO.Directory.Exists(목표DIR) == false)
		//                    {
		//                        System.IO.Directory.CreateDirectory(목표DIR);
		//                        CommonUtils.WaitTime(50, true);
		//                    }
		//                    fi.MoveTo(목표_FILE);
		//                    Console.WriteLine("## 미디어 업데이트 : {0}", 목표_FILE);
		//                }
		//                catch (System.IO.IOException ex)
		//                {
		//                    TraceManager.AddLog(string.Format("## {0}\r\n{1} ##", ex.StackTrace, ex.Message));
		//                }
		//            }
		//            fi = null;
		//            CommonUtils.WaitTime(50, true);
		//        }

		//        return true;
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
		//        return false;
		//    }
		//}



		//#region 광역시 행정코드 목록

		//static List<string> 광역시Code = new List<string>();
		//public static List<string> Get광역시Code
		//{
		//	get
		//	{
		//		if (광역시Code == null || 광역시Code.Count == 0) Int광역시코드Proc();

		//		return 광역시Code;
		//	}
		//}
		//private static void Int광역시코드Proc()
		//{
		//	try
		//	{
		//		if (광역시Code == null) 광역시Code = new List<string>();

		//		광역시Code.Add("4111"); //경기 수원
		//		광역시Code.Add("4113"); //경기 성남
		//		광역시Code.Add("4117"); //경기 안양
		//		광역시Code.Add("4127"); //경기 안산
		//		광역시Code.Add("4128"); //경기 고양
		//		광역시Code.Add("4146"); //경기 용인
		//		광역시Code.Add("4311"); //충북 청주
		//		광역시Code.Add("4413"); //충남 천안
		//		광역시Code.Add("4511"); //전북 전주
		//		광역시Code.Add("4711"); //경북 포항
		//		광역시Code.Add("4812"); //경남 창원             
		//	}
		//	catch (Exception ee)
		//	{
		//		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//	}
		//}

		//#endregion

		////public static double GET_INTERSECT_AREA(List<PBPoint> _pt1, List<PBPoint> _pt2)
		////{
		////    try
		////    {
		////        PathGeometry pg1 = PolyGeometry(_pt1, true);
		////        PathGeometry pg2 = PolyGeometry(_pt2, true);
		////        PathGeometry rpath = Geometry.Combine(pg1, pg2, GeometryCombineMode.Intersect, null);
		////        //Console.WriteLine(rpath.GetArea());
		////        return Math.Round(rpath.GetArea(), 2);
		////    }
		////    catch (Exception ee)
		////    {
		////        Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		////        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		////        return -1;
		////    }
		////}

		////public static PathGeometry PolyGeometry(List<PBPoint> vertexes, bool isClosed)
		////{
		////    if (vertexes.Count <= 0) return null;
		////    try
		////    {
		////        PathGeometry pathGeometry = new PathGeometry();
		////        PathFigure figure = new PathFigure();
		////        figure.IsClosed = isClosed;
		////        figure.StartPoint = new Point() { X = vertexes[0].X, Y = vertexes[0].Y };
		////        for (int i = 1; i < vertexes.Count; i++)
		////        {
		////            LineSegment seqment = new LineSegment(new Point() { X = vertexes[i].X, Y = vertexes[i].Y }, true);
		////            seqment.IsSmoothJoin = true;
		////            figure.Segments.Add(seqment);
		////        }
		////        pathGeometry.Figures.Add(figure);

		////        return pathGeometry;
		////    }
		////    catch (Exception ee)
		////    {
		////        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		////        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		////        return null;
		////    }
		////}

		//public static void ForcedExit()
		//{
		//    try
		//    {
		//        string ffile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fclose.info");
		//        System.IO.File.WriteAllText(ffile, "fclose");
		//    }
		//    catch (Exception ee)
		//    {
		//        System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
		//    }
		//}

		//public static void 프로세서종료Proc(string PRC_NM)
		//{
		//    try
		//    {
		//        System.Diagnostics.Process[] processesByName = System.Diagnostics.Process.GetProcessesByName(PRC_NM);
		//        if (processesByName.Length > 0)
		//        {
		//            for (int i = 0; i < processesByName.Length; i++)
		//            {
		//                processesByName[i].Kill();
		//                CommonUtils.WaitTime(50, true);
		//            }
		//        }
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
		//    }
		//}

		//public static string Get비밀번호암호화(string USER_ID, string USER_PWD)
		//{
		//    try
		//    {
		//        return CryptionUtils.ENCRYPT_BY_AES256(USER_PWD, string.Format("{0}{1}", AppConfig.CRYPT_KEY, USER_ID), Encoding.UTF8);
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        return "";
		//    }
		//}

		//public static string Get비밀번호복호화(string USER_ID, string 암호화Data)
		//{
		//    try
		//    {
		//        return CryptionUtils.DECRYPT_BY_AES256(암호화Data, string.Format("{0}{1}", AppConfig.CRYPT_KEY, USER_ID), Encoding.UTF8);
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        return "";
		//    }
		//}

		//#region 번호판 관련

		//public static List<string> Analyze번호판(string PLATE_NM)
		//{
		//    List<string> items = new List<string>();
		//    try
		//    {
		//        //Console.WriteLine("## 분석번호판 {0} ##", PLATE_NM);

		//        List<int> items한글위치 = new List<int>();
		//        for (int i = 0; i < PLATE_NM.Length; i++)
		//        {
		//            if (CHECK_한글여부(PLATE_NM.Substring(i, 1)) == true)
		//            {
		//                items한글위치.Add(i);
		//            }
		//        }

		//        switch (items한글위치.Count)
		//        {
		//            //513거5532
		//            case 1:
		//                items.Add(PLATE_NM.Substring(0, items한글위치[0]));
		//                items.Add(PLATE_NM.Substring(items한글위치[0], 1));
		//                items.Add(PLATE_NM.Substring(items한글위치[0] + 1));
		//                break;

		//            //경기23거2123
		//            case 3:
		//                items.Add(PLATE_NM.Substring(0, 2));
		//                items.Add(PLATE_NM.Substring(2, items한글위치[2] - 2));
		//                items.Add(PLATE_NM.Substring(items한글위치[2], 1));
		//                items.Add(PLATE_NM.Substring(items한글위치[2] + 1));
		//                break;
		//        }
		//    }
		//    catch (Exception ee)
		//    {
		//        Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        items = null;
		//    }
		//    return items;
		//}

		//public static int CHECK_차량종류(string PLATE_NM)
		//{
		//    int CAR_GBN = 0;
		//    try
		//    {
		//        int PLATE_NO = Convert.ToInt32(PLATE_NM);

		//        if (PLATE_NO >= 1 && PLATE_NO <= 69) //승용차
		//        {
		//            CAR_GBN = 1;
		//        }
		//        else if (PLATE_NO >= 70 && PLATE_NO <= 79) //승합차
		//        {
		//            CAR_GBN = 2;
		//        }
		//        else if (PLATE_NO >= 80 && PLATE_NO <= 97) //화물차
		//        {
		//            CAR_GBN = 3;
		//        }
		//        else if (PLATE_NO >= 98 && PLATE_NO <= 99) //특수차량
		//        {
		//            CAR_GBN = 4;
		//        }
		//        if (PLATE_NO >= 100 && PLATE_NO <= 699) //승용차
		//        {
		//            CAR_GBN = 1;
		//        }
		//        else if (PLATE_NO >= 700 && PLATE_NO <= 799) //승합차
		//        {
		//            CAR_GBN = 2;
		//        }
		//        else if (PLATE_NO >= 800 && PLATE_NO <= 979) //화물차
		//        {
		//            CAR_GBN = 3;
		//        }
		//        else if (PLATE_NO >= 980 && PLATE_NO <= 997) //특수차량
		//        {
		//            CAR_GBN = 4;
		//        }
		//        else if (PLATE_NO >= 998 && PLATE_NO <= 999) //특수차량
		//        {
		//            CAR_GBN = 5;
		//        }
		//    }
		//    catch (Exception ee)
		//    {
		//        Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        CAR_GBN = 0;
		//    }
		//    return CAR_GBN;
		//}

		//public static int CHECK_차량용도(string PLATE_NM)
		//{
		//    int PLATE_GBN = 0;
		//    try
		//    {
		//        System.Text.RegularExpressions.Regex 자가용Regex = new System.Text.RegularExpressions.Regex(@"[가-마|거-저|고-조|구-주]");
		//        System.Text.RegularExpressions.Regex 영업용Regex = new System.Text.RegularExpressions.Regex(@"[바|사|아|자]");
		//        System.Text.RegularExpressions.Regex 영업용택배Regex = new System.Text.RegularExpressions.Regex(@"[배]");
		//        System.Text.RegularExpressions.Regex 렌터카Regex = new System.Text.RegularExpressions.Regex(@"[허|하|호]");

		//        if (자가용Regex.IsMatch(PLATE_NM) == true)
		//        {
		//            PLATE_GBN = 1;
		//        }
		//        else if (영업용Regex.IsMatch(PLATE_NM) == true)
		//        {
		//            PLATE_GBN = 2;
		//        }
		//        else if (영업용택배Regex.IsMatch(PLATE_NM) == true)
		//        {
		//            PLATE_GBN = 3;
		//        }
		//        else if (렌터카Regex.IsMatch(PLATE_NM) == true)
		//        {
		//            PLATE_GBN = 4;
		//        }
		//    }
		//    catch (Exception ee)
		//    {
		//        Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        PLATE_GBN = 0;
		//    }
		//    return PLATE_GBN;
		//}

		//static bool CHECK_한글여부(string letter)
		//{
		//    bool IsCheck = true;
		//    System.Text.RegularExpressions.Regex hangulRegex = new System.Text.RegularExpressions.Regex(@"[가-힣]");
		//    Boolean ismatch = hangulRegex.IsMatch(letter);
		//    if (!ismatch)
		//    {
		//        IsCheck = false;
		//    }
		//    return IsCheck;
		//}

		//#endregion

		//public static string Get잔여시간(TimeSpan ts)
		//{
		//    try
		//    {
		//        double timeData = System.Math.Truncate(ts.TotalMilliseconds / 1000); // 몫
		//        double hour = System.Math.Truncate(timeData / 3600);
		//        double minute = System.Math.Truncate(timeData % 3600 / 60);
		//        double seconds = timeData % 3600 % 60;

		//        string display시간 = ""; string.Format("{0:0}분", minute);
		//        if (hour > 0)
		//        {
		//            display시간 = string.Format("{0:0}시간 {1:00}분 {2:00}초", hour, minute, seconds);
		//        }
		//        else if ( minute > 0)
		//        {
		//            display시간 = string.Format("{0:00}분 {1:00}초", minute, seconds);
		//        }
		//        else
		//        {
		//            display시간 = string.Format("{0:00}초", seconds);
		//        }

		//        return display시간;
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        return "";
		//    }
		//}       

		//public static int Get현재나이(DateTime dt)
		//{
		//    try
		//    {
		//        int nAge = DateTime.Now.Year - dt.Year;
		//        if (dt > DateTime.Now.AddYears(nAge * -1)) nAge -= 1;

		//        return nAge;
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        return 0;
		//    }
		//}

		//#region 공지사항

		//static List<DID_MESSAGE> items공지 = new List<DID_MESSAGE>();
		//public static List<DID_MESSAGE> Get공지사항()
		//{
		//    return items공지;
		//}

		//public static void Update공지사항()
		//{
		//    try
		//    {
		//        if (items공지 == null) items공지 = new List<DID_MESSAGE>();
		//        items공지.Clear();

		//        //items공지 = DatabaseManager.SELECT_DID_MESSAGE_BY_ALL(true);
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//    }
		//}

		//#endregion

	}
}
