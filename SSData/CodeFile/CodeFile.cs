using System;
using System.Collections.Generic;
using System.Text;

using SSCommonNET;

namespace SSData
{
    public class CodeFile
    {        
        public int CodeVersion { get; set; }
        public List<BC_CODE> ITEMS { get; set; }        

        string FILE_NM = "CODE.DAT";

        /// <summary>
        /// 파일저장
        /// </summary>
        public bool Save()
        {
            try
            {
                string 기본DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "CONFIG");
                if (System.IO.Directory.Exists(기본DIR) == false) System.IO.Directory.CreateDirectory(기본DIR);

                string FileName = System.IO.Path.Combine(기본DIR, FILE_NM);

                System.IO.FileStream fs = new System.IO.FileStream(FileName, System.IO.FileMode.Create);
                System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(typeof(CodeFile));
                sz.Serialize(fs, this);
                fs.Close();

                return true;
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(ex.Message + ex.StackTrace);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 읽어오기
        /// </summary>
        /// <returns></returns>
        public CodeFile Load()
        {
            try
            {
                string 기본DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "CONFIG");
                if (System.IO.Directory.Exists(기본DIR) == false) System.IO.Directory.CreateDirectory(기본DIR);

                string FileName = System.IO.Path.Combine(기본DIR, FILE_NM);
                if (System.IO.File.Exists(FileName) == false) Save();

                //파일이 존재하지만 empty 일경우
                System.IO.FileStream fs = new System.IO.FileStream(FileName, System.IO.FileMode.Open);
                System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(typeof(CodeFile));
                CodeFile result = (CodeFile)sz.Deserialize(fs);
                fs.Close();

                return result;
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(ex.Message + ex.StackTrace);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new CodeFile();
            }
        }
    }
}