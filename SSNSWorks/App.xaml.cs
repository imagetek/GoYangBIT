using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Input;
using SSCommonNET;
using SSData;

namespace SSNSWorks
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

                AppConfig.APPSStartPath = System.IO.Directory.GetCurrentDirectory();

                //로그는 실행폴더에 저장한다.
                //TraceManager.StartupFolder = AppConfig.APPSStartPath;                
                //프로그램 환경설정 체크
                //if (DataManager.Initialize(프로그램Type.CONFIG_M) == false)
                //{
                //    MessageBox.Show("데이터초기화 작업중 에러가 발생했습니다.", "초기화에러", MessageBoxButton.OK, MessageBoxImage.Error);
                //    System.Windows.Forms.Application.ExitThread();
                //    System.Environment.Exit(0);
                //    return;
                //}

                System.Windows.EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyHandleMouseButton), true);
                System.Windows.EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);

                Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;

    //            //기본파일복사
    //            CommonProc.Init기초파일();

    //            TraceManager.AddInfoLog(string.Format("PAJUBitView 프로그램을 시작합니다. v{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
    //            bool isYN접속 = DatabaseManager.IS_YN_접속확인();
				//if (isYN접속 == false)
				//{
				//	wnd환경설정 wConfig = new wnd환경설정();                    
    //                wConfig.WindowStartupLocation = WindowStartupLocation.CenterScreen;                    
    //                wConfig.ShowDialog();
				//	wConfig.Close();

				//	MessageBox.Show("프로그램이 종료됩니다.", "설정완료", MessageBoxButton.OK, MessageBoxImage.Error);
				//	System.Windows.Forms.Application.ExitThread();
				//	System.Environment.Exit(0);
				//}

				//DataManager.IPAddress = NetworkUtils.GetIPAddress();
    //            DataManager.MacAddress = NetworkUtils.GetMacAddress(DataManager.IPAddress);

				//#region 내부적으로 CLIENT_ID를 관리할 경우 사용
    //            /*
				//AppConfig.CLIENT_ID= RegisterManager.GET_CLIENT_ID(프로그램Type.BIT);
				//if (AppConfig.CLIENT_ID.Equals("") == true)
				//{
    //                //AppConfig.CLIENT_ID = "SSPAJU001";

    //                wnd로그인 _w로그인 = new wnd로그인();
				//	_w로그인.SetWindowStartLocation(WindowStartupLocation.CenterScreen);
				//	if (_w로그인.ShowDialog().Value == false)
				//	{
				//		TraceManager.AddLog("로그인 실패");
				//		System.Windows.Forms.Application.ExitThread();
				//		System.Environment.Exit(1);
				//		return;
				//	}
				//}
				//else
				//{
				//	TB_KIOSK item기기 = DatabaseManager.Select키오스크정보(AppConfig.KIOSK_ID);
				//	if (item기기 != null)
				//	{
				//		DataManager.Set키오스크정보(item기기, false);
				//	}
				//	//DataManager.Update키오스크정보(AppConfig.KIOSK_ID);
				//}
    //            */
				//#endregion

				wnd초기화면.Show초기화면();

                var mainWindow = new MainWindow();
                Current.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
                Current.MainWindow = mainWindow;
                mainWindow.Closed += mainWindow_Closed;
                mainWindow.Show();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
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
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
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
                        MessageBox.Show("프로그램이 중복 실행되었습니다.", "중복실행", MessageBoxButton.OK, MessageBoxImage.Error);
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
