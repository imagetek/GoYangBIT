using System;
using System.Collections.Generic;
using System.Text;

namespace SSAgent
{
    /// <summary>
    /// 로그관리 클래스
    /// </summary>
    public class TraceManager
    {
        public TraceManager()
        {

        }

        static string saveFolder = System.IO.Directory.GetCurrentDirectory();
        /// <summary>
        /// LOG를 저장할 기본 위치 (기본값 : 프로그램 시작위치)
        /// </summary>
        public static string StartupFolder
        {
            get { return saveFolder; }
            set { saveFolder = value; }
        }

        public static string HEAD_NM { get; set; }
        /// <summary>
        /// 오류를 저장한다.
        /// </summary>
        public static void AddLog(string log)
        {
            System.Diagnostics.Debug.WriteLine(log);
            try
            {
                string 기본DIR = System.IO.Path.Combine(saveFolder, "Trace");
                if (System.IO.Directory.Exists(기본DIR) == false) System.IO.Directory.CreateDirectory(기본DIR);

                string FILE_NM = System.IO.Path.Combine(기본DIR, string.Format("{0}_Agent.log", DateTime.Now.ToString("yyyyMMdd")));

                //if (System.IO.Directory.Exists(folder) == false) System.IO.Directory.CreateDirectory(folder);
                //            string fileName = System.IO.Path.Combine(folder, string.Format("log_{0}.log", DateTime.Now.ToString("yyyyMMdd")));

                DateTime dtm = DateTime.Now;
                string formatDateTime = string.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00} [{6:000}] ", dtm.Year, dtm.Month, dtm.Day, dtm.Hour, dtm.Minute, dtm.Second, dtm.Millisecond);
                log = formatDateTime + log;

                System.IO.StreamWriter sw = System.IO.File.AppendText(FILE_NM);
                sw.WriteLine(log, Encoding.UTF8);
                sw.Close();
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        /// <summary>
        /// TTS log를 저장한다.
        /// </summary>
		public static void AddAgentLog(string log , bool UseDate = true)
        {
            System.Diagnostics.Debug.WriteLine(log);
            try
            {
                string 기본DIR = System.IO.Path.Combine(saveFolder, "LOG", "AGENT");
                if (System.IO.Directory.Exists(기본DIR) == false) System.IO.Directory.CreateDirectory(기본DIR);

                string 일자DIR = System.IO.Path.Combine(기본DIR, DateTime.Now.ToString("yyyyMMdd"));
                if (System.IO.Directory.Exists(일자DIR) == false) System.IO.Directory.CreateDirectory(일자DIR);

                string FILE_NM = System.IO.Path.Combine(일자DIR, string.Format("Agent_{0:d2}.log"
                    , DateTime.Now.Hour));

                if (UseDate == true)
                {
                    DateTime dtm = DateTime.Now;
                    string formatDateTime = string.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}:{6:000} ", dtm.Year, dtm.Month, dtm.Day, dtm.Hour, dtm.Minute, dtm.Second, dtm.Millisecond);
                    log = formatDateTime + log;
                }

                System.IO.StreamWriter sw = System.IO.File.AppendText(FILE_NM);
                sw.WriteLine(log, Encoding.UTF8);
                sw.Close();
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

    }
}