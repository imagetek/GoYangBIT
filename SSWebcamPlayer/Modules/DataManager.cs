using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SSWebcamPlayer
{
	/// <summary>
	/// 프로그램의 기본적인 설정
	/// </summary>
	public class DataManager
	{
		static DataManager()
		{
		}

		/// <summary>
		/// 현재 사용중인 프로그램종류
		/// </summary>
		/// 
		#region 환경설정 관련 

		// WEATHER_URL : 기상청 RSS 주소 https://www.weather.go.kr/w/pop/rss-guide.do
		// INIT_X / INIT_Y : 구글지도 이용 위경도
		// AIR_PLACE_NM : 대기측정소정보 https://www.airkorea.or.kr/web/stationInfo?pMENU_NO=93
		static ConfigInfo _configInfo = new ConfigInfo()
		{
			ANIME_DELAY = 500,
			RADIUS = 500,
			FONTS_NM = "NanumBarunGothic",
			FONTS_SZ = 13,
			PGM_GBN = 0,
			VOLUME = 0,
			WAIT_TIME = 180,
			INIT_X = 126.8201,
			INIT_Y = 37.713,
			SGG_CD = "",
			SGG_NM = "",
			USE_WEATHER_RSS = false,
			WEATHER_URL = "http://www.kma.go.kr/wid/queryDFSRSS.jsp?zone=4128560000",
			USE_NEWS_RSS = false,
			NEWS_URL = "https://news.google.co.kr/news?cf=all&hl=ko&pz=1&ned=kr&output=rss",
			USE_AIR_PLACE = false,
			AIR_PLACE_NM = "식사동",
		};

		public static ConfigInfo ConfigInfo
		{
			get { return _configInfo; }
			set { _configInfo = value; }
		}

		static ServerInfo _serverInfo = new ServerInfo()
		{
			DB_GBN = 1,
			DB_IP = "127.0.0.1",
			DB_PORT = 0,
			DB_URL = @"C:\PAJUBitView\BIT.db3",
			DB_USERID = "",
			DB_PASSWD = "",
		};

		public static ServerInfo ServerInfo
		{
			get { return _serverInfo; }
			set { _serverInfo = value; }
		}

		//private static setSSNSFiles setSSNSFile = new setSSNSFiles();
		//public static setSSNSFiles SetSSNSFile
		//{
		//    get { return setSSNSFile; }
		//    set { setSSNSFile = value; }
		//}

		private static ConfigFile setSSNSFile = new ConfigFile();
		public static ConfigFile SetSSNSFile
		{
			get { return setSSNSFile; }
			set { setSSNSFile = value; }
		}

		#endregion

		public static bool Initialize()
		{
			try
			{
				if (System.IO.Directory.Exists(AppConfig.APPSStartPath) == false) System.IO.Directory.CreateDirectory(AppConfig.APPSStartPath);

				bool saveOk = false;
				setSSNSFile = setSSNSFile.Load();
				if (setSSNSFile == null || setSSNSFile.SEQ_NO == 0)
				{
					setSSNSFile.SEQ_NO = 1;

					if (setSSNSFile.ConfigInfo == null) setSSNSFile.ConfigInfo = new ConfigInfo();

					setSSNSFile.ConfigInfo.PGM_GBN = _configInfo.PGM_GBN;
					setSSNSFile.ConfigInfo.ANIME_DELAY = _configInfo.ANIME_DELAY;
					setSSNSFile.ConfigInfo.RADIUS = _configInfo.RADIUS;
					setSSNSFile.ConfigInfo.FONTS_SZ = _configInfo.FONTS_SZ;
					setSSNSFile.ConfigInfo.FONTS_NM = _configInfo.FONTS_NM;
					setSSNSFile.ConfigInfo.SGG_CD = _configInfo.SGG_CD;
					setSSNSFile.ConfigInfo.SGG_NM = _configInfo.SGG_NM;
					setSSNSFile.ConfigInfo.VOLUME = _configInfo.VOLUME;
					setSSNSFile.ConfigInfo.WAIT_TIME = _configInfo.WAIT_TIME;
					setSSNSFile.ConfigInfo.INIT_X = _configInfo.INIT_X;
					setSSNSFile.ConfigInfo.INIT_Y = _configInfo.INIT_Y;
					setSSNSFile.ConfigInfo.USE_WEATHER_RSS = _configInfo.USE_WEATHER_RSS;
					setSSNSFile.ConfigInfo.WEATHER_URL = _configInfo.WEATHER_URL;
					setSSNSFile.ConfigInfo.USE_NEWS_RSS = _configInfo.USE_NEWS_RSS;
					setSSNSFile.ConfigInfo.NEWS_URL = _configInfo.NEWS_URL;
					setSSNSFile.ConfigInfo.USE_AIR_PLACE = _configInfo.USE_AIR_PLACE;
					setSSNSFile.ConfigInfo.AIR_PLACE_NM = _configInfo.AIR_PLACE_NM;

					if (setSSNSFile.ServerInfo == null) setSSNSFile.ServerInfo = new ServerInfo();

					setSSNSFile.ServerInfo.DB_GBN = _serverInfo.DB_GBN;
					setSSNSFile.ServerInfo.DB_IP = _serverInfo.DB_IP;
					setSSNSFile.ServerInfo.DB_PORT = _serverInfo.DB_PORT;
					setSSNSFile.ServerInfo.DB_URL = _serverInfo.DB_URL;
					//setSSNSFile.ServerInfo.DB_USERID = CryptionUtils.ENCRYPT_BY_AES256(_serverInfo.DB_USERID, AppConfig.CRYPT_KEY, Encoding.UTF8);
					//setSSNSFile.ServerInfo.DB_PASSWD = CryptionUtils.ENCRYPT_BY_AES256(_serverInfo.DB_PASSWD, AppConfig.CRYPT_KEY, Encoding.UTF8);
					saveOk = true;
				}
				else
				{
					_configInfo = setSSNSFile.ConfigInfo;
					_serverInfo = setSSNSFile.ServerInfo;
				}

				if (saveOk)
					return setSSNSFile.Save();

				InitBIT_SYSTEM();

				return true;
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
		}

		public static string Get캡쳐DIR()
		{
			try
			{
				string DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "CAPTURE");

				return DIR;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return "";
			}
		}

		private static BIT_SYSTEM _bitSystem = new BIT_SYSTEM();
		public static BIT_SYSTEM BitSystem
		{
			get { return _bitSystem; }
			set { _bitSystem = value; }
		}
		private static void InitBIT_SYSTEM()
		{
			try
			{
				SQLiteManager mSQL = new SQLiteManager();
				mSQL.SetConnectFile(_serverInfo.DB_URL);
				if (mSQL.ISConnect() == false)
				{
					TraceManager.AddWebCamLog("DB접속실패 프로그램 종료");
					return;
				}

				string query = string.Format("SELECT * FROM BIT_SYSTEM ORDER BY SEQ_NO DESC LIMIT 1");
				List<BIT_SYSTEM> items설정 = mSQL.SELECT_BIT_SYSTEM_BY_QUERY(query);
				if (items설정 != null && items설정.Count > 0)
				{
					_bitSystem = items설정.First();
				}
				else
				{
					_bitSystem.SEQ_NO = 0;

					_bitSystem.BIT_ID = 102;
					_bitSystem.MOBILE_NO = 31672;
					_bitSystem.STATION_NM = "운정광역보건지소(중)";

					_bitSystem.SERVER_URL = "172.8.1.61";
					_bitSystem.SERVER_PORT = "32100";

					_bitSystem.FTP_GBN = 2;
					_bitSystem.FTP_IP = "172.8.1.61";
					_bitSystem.FTP_PORT = 2223;
					_bitSystem.FTP_USERID = "pjbit";
					_bitSystem.FTP_USERPWD = "pjbit@8732";

					_bitSystem.HTTP_URL = "172.8.1.60";
					_bitSystem.HTTP_PORT = 8080;

					_bitSystem.ENV_PORT_NM = "COM3";
					_bitSystem.ENV_BAUD_RATE = 57600;
				}

				mSQL = null;
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

	}
}
