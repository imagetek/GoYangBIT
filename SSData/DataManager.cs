using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;

namespace SSData
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
        public static 프로그램Type 현재PGM = 프로그램Type.NONE;

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
            VOLUME = 9,
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
            USE_DEBUG_LOG = false,
            USE_REBOOT = true,
            REBOOT_TIME = "0400",
            USE_SYNCTIME = true,
            SYNC_URL = "time.kriss.re.kr",	//pjh
            USE_LOCAL_MEDIAL = true,
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
			DB_URL = @"data\BIT.db3",
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

		public static bool Initialize(프로그램Type pGBN)
        {
            try
            {
                bool isYN결과 = false;

                switch (pGBN)
                {
					case 프로그램Type.AGENT:
						isYN결과 = InitializeViewer();
						break;

					case 프로그램Type.BIT:
                        isYN결과 = InitializeViewer();
                        break;                    
                }

                현재PGM = pGBN;

                return isYN결과;
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        static bool InitializeViewer()
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
                    //20220728 BHA 신규
                    setSSNSFile.ConfigInfo.USE_DEBUG_LOG = _configInfo.USE_DEBUG_LOG;
                    setSSNSFile.ConfigInfo.USE_REBOOT = _configInfo.USE_REBOOT;
                    setSSNSFile.ConfigInfo.REBOOT_TIME = _configInfo.REBOOT_TIME;
                    setSSNSFile.ConfigInfo.USE_SYNCTIME = _configInfo.USE_SYNCTIME;
                    setSSNSFile.ConfigInfo.SYNC_URL = _configInfo.SYNC_URL;
                    //20220928 bha 신규
                    setSSNSFile.ConfigInfo.USE_LOCAL_MEDIAL = _configInfo.USE_LOCAL_MEDIAL;

					if (setSSNSFile.ServerInfo == null) setSSNSFile.ServerInfo = new ServerInfo();

                    setSSNSFile.ServerInfo.DB_GBN = _serverInfo.DB_GBN;
                    setSSNSFile.ServerInfo.DB_IP = _serverInfo.DB_IP;
                    setSSNSFile.ServerInfo.DB_PORT = _serverInfo.DB_PORT;
                    setSSNSFile.ServerInfo.DB_URL = _serverInfo.DB_URL;
                    setSSNSFile.ServerInfo.DB_USERID = CryptionUtils.ENCRYPT_BY_AES256(_serverInfo.DB_USERID, AppConfig.CRYPT_KEY, Encoding.UTF8);
                    setSSNSFile.ServerInfo.DB_PASSWD = CryptionUtils.ENCRYPT_BY_AES256(_serverInfo.DB_PASSWD, AppConfig.CRYPT_KEY, Encoding.UTF8);
                    saveOk = true;
                }
                else
                {
                    _configInfo = setSSNSFile.ConfigInfo;
                    _serverInfo = setSSNSFile.ServerInfo;
                }

				#region 주석처리

				//                #region 시스템기본 관련

				//                if (setSSNSFile.DsSSNS.dtConfig.Rows.Count > 0)
				//            {
				//                Config.dsSSNS.dtConfigRow row = setSSNSFile.DsSSNS.dtConfig.Rows[0] as Config.dsSSNS.dtConfigRow;

				//                _configInfo.PGM_GBN = row.IsPGM_GBNNull() ? 0 : row.PGM_GBN;
				//                _configInfo.SGG_CD = row.IsSGG_CDNull() ? "00000" : row.SGG_CD;
				//                _configInfo.SGG_NM = row.IsSGG_NMNull() ? "신성엔에스텍" : row.SGG_NM;
				//                _configInfo.ANIME_DELAY = row.IsANIME_DELAYNull() ? 500 : row.ANIME_DELAY;
				//                _configInfo.RADIUS = row.IsRADIUSNull() ? 500 : row.RADIUS;
				//                _configInfo.FONTS_SZ = row.IsFONTS_SZNull() ? 13 : row.FONTS_SZ;
				//                _configInfo.FONTS_NM = row.IsFONTS_NMNull() ? "NanumBarunGothic" : row.FONTS_NM;
				//                _configInfo.VOLUME = row.IsVOLUMENull() ? 0 : row.VOLUME;
				//                _configInfo.WAIT_TIME = row.IsWAIT_TIMENull() ? 180 : row.WAIT_TIME;
				//                _configInfo.INIT_X = row.IsINIT_XNull() ? 126.8201 : row.INIT_X;
				//                _configInfo.INIT_Y = row.IsINIT_YNull() ? 37.713: row.INIT_Y;

				//                _configInfo.USE_WEATHER_RSS = row.IsUSE_WEATHER_RSSNull() ? false : row.USE_WEATHER_RSS;
				//                _configInfo.WEATHER_URL = row.IsWEATHER_URLNull() ? "http://www.kma.go.kr/wid/queryDFSRSS.jsp?zone=4128560000" : row.WEATHER_URL;

				//                _configInfo.USE_NEWS_RSS= row.IsUSE_NEWS_RSSNull() ? false : row.USE_NEWS_RSS;
				//                _configInfo.NEWS_URL = row.IsNEWS_URLNull() ? "https://news.google.co.kr/news?cf=all&hl=ko&pz=1&ned=kr&output=rss" : row.NEWS_URL;

				//                _configInfo.USE_AIR_PLACE = row.IsUSE_AIR_PLACENull() ? false : row.USE_AIR_PLACE;
				//                _configInfo.AIR_PLACE_NM = row.IsAIR_PLACE_NMNull() ? "식사동" : row.AIR_PLACE_NM;
				//            }
				//            else
				//            {
				//                Config.dsSSNS.dtConfigRow row = setSSNSFile.DsSSNS.dtConfig.NewdtConfigRow();

				//                row.PGM_GBN = _configInfo.PGM_GBN;
				//                row.ANIME_DELAY = _configInfo.ANIME_DELAY;
				//                row.RADIUS = _configInfo.RADIUS;
				//                row.FONTS_SZ = _configInfo.FONTS_SZ;
				//                row.FONTS_NM = _configInfo.FONTS_NM;
				//                row.SGG_CD = _configInfo.SGG_CD;
				//	row.SGG_NM = _configInfo.SGG_NM;
				//                row.VOLUME = _configInfo.VOLUME;
				//                row.WAIT_TIME = _configInfo.WAIT_TIME;
				//                row.INIT_X = _configInfo.INIT_X;
				//                row.INIT_Y = _configInfo.INIT_Y;

				//                row.USE_WEATHER_RSS= _configInfo.USE_WEATHER_RSS;
				//                row.WEATHER_URL = _configInfo.WEATHER_URL;
				//                row.USE_NEWS_RSS= _configInfo.USE_NEWS_RSS;
				//                row.NEWS_URL = _configInfo.NEWS_URL;
				//                row.USE_AIR_PLACE = _configInfo.USE_AIR_PLACE;
				//                row.AIR_PLACE_NM = _configInfo.AIR_PLACE_NM;

				//                if (setSSNSFile.AddConfigInfo(row) == false) return false;
				//                saveOk = true;
				//            }

				//            if (System.Diagnostics.Debugger.IsAttached == true) DataManager.ConfigInfo.WAIT_TIME = 30;

				//#endregion

				//#region SERVER 관련

				////List<TB_CONFIG> items서버 = DatabaseManager.SELECT_TB_CONFIG_BY_QUERY("SELECT * FROM TB_CONFIG ORDER BY SEQ_NO DESC LIMIT 1");
				////if (items서버 != null && items서버.Count > 0)
				////{
				////    _serverInfo.SEQ_NO = items서버.First().SEQ_NO;
				////    _serverInfo.DB_GBN = items서버.First().DB_GBN;
				////    _serverInfo.DB_IP = items서버.First().DB_IP;
				////    _serverInfo.DB_PORT = items서버.First().DB_PORT;
				////    _serverInfo.DB_URL = items서버.First().DB_URL;
				////    _serverInfo.DB_USERID = items서버.First().DB_USERID;
				////    _serverInfo.DB_PASSWD = items서버.First().DB_USERPWD;
				////}
				////            else
				////{

				////}
				//if (setSSNSFile.DsSSNS.dtServer.Rows.Count > 0)
				//{
				//	Config.dsSSNS.dtServerRow row = setSSNSFile.DsSSNS.dtServer.Rows[0] as Config.dsSSNS.dtServerRow;

				//	_serverInfo.DB_GBN = row.IsDB_GBNNull() ? 1 : row.DB_GBN;
				//	_serverInfo.DB_IP = row.IsDB_IPNull() ? "127.0.0.1" : row.DB_IP;
				//	_serverInfo.DB_PORT = row.IsDB_PORTNull() ? 0 : row.DB_PORT;
				//                _serverInfo.DB_URL = row.IsDB_URLNull() ? "BIT.db3" : row.DB_URL;
				//	_serverInfo.DB_USERID = row.IsDB_USERIDNull() ? "" : CryptionUtils.DECRYPT_BY_AES256(row.DB_USERID, AppConfig.CRYPT_KEY, Encoding.UTF8);
				//	_serverInfo.DB_PASSWD = row.IsDB_PASSWDNull() ? "" : CryptionUtils.DECRYPT_BY_AES256(row.DB_PASSWD, AppConfig.CRYPT_KEY, Encoding.UTF8);

				//	//_serverInfo.SERVER_URL = row.IsSERVER_URLNull() ? "127.0.0.1" : row.SERVER_URL;
				//	//_serverInfo.SERVER_PORT = row.IsSERVER_PORTNull() ? 80 : row.SERVER_PORT;
				//}
				//else
				//{
				//	Config.dsSSNS.dtServerRow row = setSSNSFile.DsSSNS.dtServer.NewdtServerRow();

				//	row.DB_GBN = _serverInfo.DB_GBN;
				//	row.DB_IP = _serverInfo.DB_IP;
				//	row.DB_PORT = _serverInfo.DB_PORT;
				//	row.DB_URL = CryptionUtils.ENCRYPT_BY_AES256(_serverInfo.DB_URL, AppConfig.CRYPT_KEY, Encoding.UTF8);
				//	row.DB_USERID = CryptionUtils.ENCRYPT_BY_AES256(_serverInfo.DB_USERID, AppConfig.CRYPT_KEY, Encoding.UTF8);
				//	row.DB_PASSWD = CryptionUtils.ENCRYPT_BY_AES256(_serverInfo.DB_PASSWD, AppConfig.CRYPT_KEY, Encoding.UTF8);

				//	//row.FTP_IP = setSERVER.FTP_IP;
				//	//row.FTP_PORT = setSERVER.FTP_PORT;
				//	//row.FTP_USERID = CryptionUtils.ENCRYPT_BY_AES256(setSERVER.FTP_USERID, AppConfig.CRYPT_KEY, Encoding.UTF8);
				//	//row.FTP_PASSWD = CryptionUtils.ENCRYPT_BY_AES256(setSERVER.FTP_PASSWD, AppConfig.CRYPT_KEY, Encoding.UTF8);
				//	//row.FTP_BASE_PATH = setSERVER.FTP_BASE_PATH;

				//	//row.SERVER_URL = _serverInfo.SERVER_URL;
				//	//row.SERVER_PORT = _serverInfo.SERVER_PORT;

				//	if (setSSNSFile.AddServerInfo(row) == false) return false;

				//	saveOk = true;
				//}

				//#endregion

				#endregion

				if (saveOk)
                    return setSSNSFile.Save();

                //SCode불러오기
                //SCodeManager.Initialize(System.IO.Path.Combine(AppConfig.APPSStartPath, "DB"));

                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static string Get컨텐츠DIR()
        {
            try
            {
                string DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "CONTENTS");

                return DIR;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return "";
            }
        }

        public static string Get업데이트DIR()
        {
            try
            {
                string DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "UPDATE");                

                return DIR;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return "";
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


        public static string IPAddress { get; set; }
        public static string MacAddress { get; set; }
        public static bool DebugYN
        {
            get
            {
                return System.Diagnostics.Debugger.IsAttached;
            }
        }

    }
}
