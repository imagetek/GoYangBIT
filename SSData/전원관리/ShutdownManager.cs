using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;
using System.Windows.Threading;

namespace SSData
{
    public class ShutdownManager
    {
        public ShutdownManager() 
        {
            try
            {
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public void InitProc()
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
        
        int 재시도횟수 = 0;
        public void PC전원종료Proc()
        {
            try
            {
                TraceManager.AddInfoLog("[전원종료] 전원종료 작업을 시작합니다.");
                if (System.Diagnostics.Debugger.IsAttached == true)
                {
                    return;
                }

                TraceManager.AddInfoLog(string.Format("[전원종료] 전원종료를 시도합니다. {0}회", 재시도횟수++));
                System.Diagnostics.Process.Start("shutdown", "-s -f -t 30");

                DispatcherTimer _dtPowerOff = new DispatcherTimer();
                _dtPowerOff.Tag = 0;
                _dtPowerOff.Interval = TimeSpan.FromMinutes(1.0);
                _dtPowerOff.Tick += (object ss, EventArgs ee) =>
                {

                    if (Convert.ToInt32(_dtPowerOff.Tag) == 1) return;
                    _dtPowerOff.Stop();
                    _dtPowerOff.Tag = 1;

                    TraceManager.AddInfoLog(string.Format("[전원종료] 전원종료를 시도합니다. {0}회", 재시도횟수++));
                    System.Diagnostics.Process.Start("shutdown", "-s -f -t 30");

                    _dtPowerOff.Tag = 0;
                    _dtPowerOff.Start();
                };
                _dtPowerOff.Start();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public void PC재시작Proc()
        {
            try
            {
                TraceManager.AddInfoLog("[재시작] PC재시작 작업을 시작합니다.");
                if (System.Diagnostics.Debugger.IsAttached == true)
                {
                    return;
                }

                TraceManager.AddInfoLog(string.Format("[재시작] PC재시작을 시도합니다. {0}회", 재시도횟수++));
                System.Diagnostics.Process.Start("shutdown", "-r -f -t 30");

                DispatcherTimer _dtPowerOff = new DispatcherTimer();
                _dtPowerOff.Tag = 0;
                _dtPowerOff.Interval = TimeSpan.FromMinutes(1.0);
                _dtPowerOff.Tick += (object ss, EventArgs ee) =>
                {

                    if (Convert.ToInt32(_dtPowerOff.Tag) == 1) return;
                    _dtPowerOff.Stop();
                    _dtPowerOff.Tag = 1;

                    TraceManager.AddInfoLog(string.Format("[재시작] PC재시작을 시도합니다. {0}회", 재시도횟수++));
                    System.Diagnostics.Process.Start("shutdown", "-r -f -t 30");

                    _dtPowerOff.Tag = 0;
                    _dtPowerOff.Start();
                };
                _dtPowerOff.Start();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public void Add종료기록()
        {
            try
            {
                string folder = System.IO.Path.Combine(AppConfig.APPSStartPath, "전원종료");
                if (System.IO.Directory.Exists(folder) == false) System.IO.Directory.CreateDirectory(folder);

                string fileName = System.IO.Path.Combine(folder, string.Format("{0}.log", DateTime.Now.ToString("yyyyMMdd")));
                
                List<string> logs = new List<string>() { "PC종료" , DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") ,  };
                string log = string.Join("##", logs.ToArray());

                System.IO.StreamWriter sw = System.IO.File.AppendText(fileName);
                sw.WriteLine(log);
                sw.Close();
                sw = null;
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
            }
        }

        public bool Get종료기록()
        {
            try
            {
                string folder = System.IO.Path.Combine(AppConfig.APPSStartPath, "전원종료");
                if (System.IO.Directory.Exists(folder) == false) return false;

                string fileName = System.IO.Path.Combine(folder, string.Format("{0}.log", DateTime.Now.ToString("yyyyMMdd")));
                return System.IO.File.Exists(fileName);
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
                return false;
            }
        }
    }
}
