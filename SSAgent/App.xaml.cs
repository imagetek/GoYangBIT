using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using SSData;

namespace SSAgent
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

				#region 시작폴더 지정

				string _tempDIR = RegisterManager.GET_APPS_DIR(프로그램Type.AGENT);
				if (_tempDIR == null || _tempDIR.Equals("") == true)
				{
					//시작폴더 설정
					Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
					wnd설치경로 _w설치 = new wnd설치경로();
					_w설치.SetWindowStartLocation(WindowStartupLocation.CenterScreen);
					if (_w설치.ShowDialog().Value == true)
					{
						if (_w설치 != null) _w설치.Close();

						System.Windows.Forms.FolderBrowserDialog _fb = new System.Windows.Forms.FolderBrowserDialog();
						_fb.ShowNewFolderButton = true;
						_fb.SelectedPath = @"C:\BitView";
						if (_fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
						{
							AppConfig.APPSStartPath = _fb.SelectedPath;
							if (RegisterManager.SET_APPS_DIR(프로그램Type.AGENT, AppConfig.APPSStartPath) == true)
							{
								Console.WriteLine("[REG] 프로그램 시작경로를 저장했습니다.");
							}
							else
							{
								Console.WriteLine("[REG] 프로그램 시작경로를 저장에 실패했니다.");
							}
						}
					}
					else
					{
						if (_w설치 != null) _w설치.Close();

						AppConfig.APPSStartPath = @"C:\BitView";
						if (System.IO.Directory.Exists(AppConfig.APPSStartPath) == false) System.IO.Directory.CreateDirectory(AppConfig.APPSStartPath);

						if (RegisterManager.SET_APPS_DIR(프로그램Type.AGENT, AppConfig.APPSStartPath) == true)
						{
							Console.WriteLine("[REG] 프로그램 시작경로를 저장했습니다.");
						}
						else
						{
							Console.WriteLine("[REG] 프로그램 시작경로를 저장에 실패했니다.");
						}
					}
				}
				else
				{
					AppConfig.APPSStartPath = _tempDIR;
				}

				#endregion

				TraceManager.AddAgentLog(string.Format("SSAgent 프로그램을 시작합니다. v{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));

                //edgar removed for now, since we dont do updates
                //if (CommonProc.Check프로그램업데이트(AppConfig.APPSStartPath) == true)
                //{
                //    TraceManager.AddAgentLog("프로그램 업데이트가 존재합니다. 프로그램을 종료합니다.");

                //    string updateEXE = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SSBITUpdater.exe");
                //    if (System.IO.File.Exists(updateEXE) == true)
                //    {
                //        TraceManager.AddAgentLog("업데이트 프로그램을 시작합니다.");
                //        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(updateEXE);
                //        startInfo.Arguments = string.Format("SSNS {0}" , AppConfig.APPSStartPath);
                //        System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo);
                //        process.PriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal;
                //        System.Threading.Thread.Sleep(3000);
                //        process.Dispose();
                //    }
                //    System.Windows.Forms.Application.ExitThread();
                //    System.Environment.Exit(0);
                //    return;
                //}

				//환경설정 체크
				if (DataManager.Initialize(프로그램Type.AGENT) == false)
				{
					MessageBox.Show("데이터초기화 작업중 에러가 발생했습니다.", "초기화에러", MessageBoxButton.OK, MessageBoxImage.Error);
					System.Windows.Forms.Application.ExitThread();
					System.Environment.Exit(0);
					return;
				}

				System.Windows.EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyHandleMouseButton), true);
				System.Windows.EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);

				Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;

                //edgar removed this, because moved setting to application settings and json file
                //기본파일복사
                //CommonProc.Init기초파일();

                //edgar removed this, we dont use db to load data
                //bool isYN접속 = DatabaseManager.IS_YN_접속확인();
                //if (isYN접속 == false)
                //{
                //    MessageBox.Show("DB에 접속할수 없습니다..", "DB미존재", MessageBoxButton.OK, MessageBoxImage.Error);
                //    System.Windows.Forms.Application.ExitThread();
                //    System.Environment.Exit(0);
                //}

                #region 내부적으로 CLIENT_ID를 관리할 경우 사용
                /*
				AppConfig.CLIENT_ID= RegisterManager.GET_CLIENT_ID(프로그램Type.BIT);
				if (AppConfig.CLIENT_ID.Equals("") == true)
				{
                    //AppConfig.CLIENT_ID = "SSPAJU001";

                    wnd로그인 _w로그인 = new wnd로그인();
					_w로그인.SetWindowStartLocation(WindowStartupLocation.CenterScreen);
					if (_w로그인.ShowDialog().Value == false)
					{
						TraceManager.AddLog("로그인 실패");
						System.Windows.Forms.Application.ExitThread();
						System.Environment.Exit(1);
						return;
					}
				}
				else
				{
					TB_KIOSK item기기 = DatabaseManager.Select키오스크정보(AppConfig.KIOSK_ID);
					if (item기기 != null)
					{
						DataManager.Set키오스크정보(item기기, false);
					}
					//DataManager.Update키오스크정보(AppConfig.KIOSK_ID);
				}
                */
                #endregion

                wnd초기화면.Show초기화면();

                var mainWindow = new MainWindow();
                Current.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
                Current.MainWindow = mainWindow;
                mainWindow.Closed += mainWindow_Closed;
                mainWindow.Show();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        void mainWindow_Closed(object sender, EventArgs e)
        {
            MainWindow w = sender as MainWindow;
            if (w == null) return;

            if (w.ResultDialog)
            {
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            }
        }

        private static void SelectivelyHandleMouseButton(object sender, MouseButtonEventArgs e)
        {
            var textbox = (sender as TextBox);
            if (textbox != null && !textbox.IsKeyboardFocusWithin)
            {
                if (e.OriginalSource.GetType().Name == "TextBoxView")
                {
                    e.Handled = true; textbox.Focus();
                }
            }
        }
        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null) textBox.SelectAll();
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
                        MessageBox.Show("Agent프로그램이 이미실행중입니다.", "중복실행", MessageBoxButton.OK, MessageBoxImage.Error);
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
