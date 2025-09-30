using System;
using System.Collections.Generic;
using System.Text;

namespace SSWebcamPlayer
{
    public class ConfigFile
    {
        public int SEQ_NO { get; set; }
        public ConfigInfo ConfigInfo { get; set; }
        public ServerInfo ServerInfo{ get; set; }

        string FILE_NM = "SSNS.DAT";

		public bool IsExistConfigFile()
		{
			try
			{
				string 설정FILE = System.IO.Path.Combine(AppConfig.APPSStartPath, "CONFIG", FILE_NM);
				return System.IO.File.Exists(설정FILE);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + ex.StackTrace);
				System.Diagnostics.Debug.WriteLine(ex.Message);
				return false;
			}
		}

		/// <summary>
		/// 파일저장
		/// </summary>
		public bool Save(bool backupYN = false)
		{
			try
			{
				string 기본DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "CONFIG");
				if (System.IO.Directory.Exists(기본DIR) == false) System.IO.Directory.CreateDirectory(기본DIR);

				string FileName = System.IO.Path.Combine(기본DIR, FILE_NM);

				//20220518 bha 
				if (backupYN == true && System.IO.File.Exists(FileName) == true)
				{
					string 백업DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "CONFIG", "BACKUP");
					if (System.IO.Directory.Exists(백업DIR) == false) System.IO.Directory.CreateDirectory(백업DIR);

					string 백업FILE = System.IO.Path.Combine(AppConfig.APPSStartPath, "CONFIG", "BACKUP"
						, string.Format("SSNS_{0}.DAT", DateTime.Now.ToString("yyyyMMddHHmm")));
					try
					{
						System.IO.File.Copy(FileName, 백업FILE, true);
					}
					catch (System.IO.IOException ex)
					{
						TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
						System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
					}
				}


				System.IO.FileStream fs = new System.IO.FileStream(FileName, System.IO.FileMode.Create);
				System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(typeof(ConfigFile));
				sz.Serialize(fs, this);
				fs.Close();

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + ex.StackTrace);
				System.Diagnostics.Debug.WriteLine(ex.Message);
				return false;
				throw new Exception(ex.Message);
			}
		}


		/// <summary>
		/// 읽어오기
		/// </summary>
		/// <returns></returns>
		public ConfigFile Load()
		{
			try
			{
				string 기본DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "CONFIG");
				if (System.IO.Directory.Exists(기본DIR) == false) System.IO.Directory.CreateDirectory(기본DIR);

				string FileName = System.IO.Path.Combine(기본DIR, FILE_NM);
				if (System.IO.File.Exists(FileName) == false) Save();

				//파일이 존재하지만 empty 일경우
				System.IO.FileStream fs = new System.IO.FileStream(FileName, System.IO.FileMode.Open);
				System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(typeof(ConfigFile));
				ConfigFile result = (ConfigFile)sz.Deserialize(fs);
				fs.Close();

				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + ex.StackTrace);
				System.Diagnostics.Debug.WriteLine(ex.Message);
				return new ConfigFile();
			}
		}
	}
}