using System;
using System.Collections.Generic;
using System.Text;

using SSCommonNET;

namespace SSData
{
    public class VersionFile
    {        
        public VersionData ITEM { get; set; }        

        string FILE_NM = "VERSION.xml";

        /// <summary>
        /// 파일저장
        /// </summary>
        public bool Save(string FILE_NM)
        {
            try
            {                
                System.IO.FileStream fs = new System.IO.FileStream(FILE_NM, System.IO.FileMode.Create);
                System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(typeof(VersionFile));
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
        public VersionFile Load(string FILE_NM)
        {
            try
            {
                //파일이 존재하지만 empty 일경우
                System.IO.FileStream fs = new System.IO.FileStream(FILE_NM, System.IO.FileMode.Open);
                System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(typeof(VersionFile));
                VersionFile result = (VersionFile)sz.Deserialize(fs);
                fs.Close();

                return result;
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(ex.Message + ex.StackTrace);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new VersionFile();
            }
        }
    }
}