using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;

namespace SSData
{
    public class setSSNSFiles
    {
        const string FOLDER_NM = "CONFIG";
        const string CONFIG_NM= "SSNS.DAT";
        private string FILE_NM = "";
        private string APPSStartPath = "";

        public setSSNSFiles()
        {
            
        }

        Config.dsSSNS _dsSSNS= new Config.dsSSNS();
        public Config.dsSSNS DsSSNS
        {
            get { return _dsSSNS; }
        }

        public bool AddConfigInfo(Config.dsSSNS.dtConfigRow row)
        {
            try
            {
                _dsSSNS.dtConfig.AdddtConfigRow(row);
                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public bool ModifyConfigInfo(Config.dsSSNS.dtConfigRow row)
        {
            try
            {
                Config.dsSSNS.dtConfigRow arow = _dsSSNS.dtConfig.FindByidx(row.idx);

                arow = row;

                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
                return false;
            }
        }

		public bool AddServerInfo(Config.dsSSNS.dtServerRow row)
		{
			try
			{
				_dsSSNS.dtServer.AdddtServerRow(row);
				return true;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
				return false;
			}
		}

		public bool ModifyServerInfo(Config.dsSSNS.dtServerRow row)
		{
			try
			{
				Config.dsSSNS.dtServerRow arow = _dsSSNS.dtServer.FindByidx(row.idx);

				arow = row;
				return true;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
				return false;
			}
		}

		public bool Save()
        {
            try
            {
                if (APPSStartPath == null) APPSStartPath = AppConfig.APPSStartPath;

                string saveFolder = System.IO.Path.Combine(APPSStartPath, FOLDER_NM);
                if (System.IO.Directory.Exists(saveFolder) == false) System.IO.Directory.CreateDirectory(saveFolder);

                FILE_NM = System.IO.Path.Combine(APPSStartPath, FOLDER_NM, CONFIG_NM);

                _dsSSNS.WriteXml(FILE_NM);

                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
                return false;
            }
        }

        /// <summary>
        /// 데이터읽어오기
        /// </summary>
        public bool Load()
        {
            try
            {
                if (APPSStartPath == null || APPSStartPath.Equals("") == true) APPSStartPath = AppConfig.APPSStartPath;

                string FILE_NM = System.IO.Path.Combine(APPSStartPath, FOLDER_NM, CONFIG_NM);

                string ConfigDIR = System.IO.Path.GetDirectoryName(FILE_NM);
                if (System.IO.Directory.Exists(ConfigDIR) == false) System.IO.Directory.CreateDirectory(ConfigDIR);
                if (System.IO.File.Exists(FILE_NM))
                {
                    _dsSSNS.Clear();
                    _dsSSNS.ReadXml(FILE_NM);
                }
                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
                return false;
            }
        }
    }
}
