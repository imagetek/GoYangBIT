using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SSBITUpdater
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                if (IsUseProcess() == false)
                {
                    Console.WriteLine("중복실행으로 종료됩니다.");
                    System.Windows.Forms.Application.ExitThread();
                    System.Environment.Exit(0);
                    return;
                }                

                //외부변수받기
                string[] mArgs= e.Args;
                if (mArgs == null || mArgs.Length == 0)
                {
                    //Console.WriteLine("매개변수 미존재");
                    TraceManager.AddUpdaterLog("매개변수 미존재");
                    System.Windows.Forms.Application.ExitThread();
                    System.Environment.Exit(0);
                    return;
                }

                if (mArgs.Length > 0)
				{
                    string mAPPS= mArgs[0];
                    if (mAPPS.Equals("SSNS") == false)
                    {
                        TraceManager.AddUpdaterLog("호출변수 미일치");
                        System.Windows.Forms.Application.ExitThread();
                        System.Environment.Exit(0);
                        return;
                    }
				}

                if (mArgs.Length > 1)
                {
                    string mPATH = mArgs[1].Replace(@"\\", @"\");
                    if (System.IO.Directory.Exists(mPATH) == false)
                    {
                        TraceManager.AddUpdaterLog("시작경로 미존재");
                        System.Windows.Forms.Application.ExitThread();
                        System.Environment.Exit(0);
                        return;
                    }
                    TraceManager.AddUpdaterLog(string.Format("시작경로 : {0}", mPATH));
                    UpdateManager.APPSStartPath = mPATH;
                }

                
                TraceManager.AddUpdaterLog(string.Format("SSBITUpdater 프로그램을 시작합니다. v{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));

                var mainWindow = new MainWindow();
                Current.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
                Current.MainWindow = mainWindow;
                //mainWindow.Closed += mainWindow_Closed;
                mainWindow.Show();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            try
            {
                switch (e.ReasonSessionEnding)
                {
                    case ReasonSessionEnding.Shutdown:
                        System.Environment.Exit(1);
                        break;
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        /* 프로그램 중복실행 방지 */
        private static bool IsUseProcess()
        {
            try
            {

                System.Diagnostics.Process[] processesByName = System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
                int num = 0;
                System.Diagnostics.Process[] processArray2 = processesByName;
                for (int i = 0; i < processArray2.Length; i++)
                {
                    System.Diagnostics.Process process1 = processArray2[i];
                    num++;
                }

                if (System.Diagnostics.Debugger.IsAttached == false)
                {
                    if (num > 1)
                    {
                        //MessageBox.Show("프로그램이 중복 실행되었습니다.", "중복실행", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
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
    }
}
