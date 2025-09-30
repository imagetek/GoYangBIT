using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using SSData;

namespace SSAgent
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.IsEnabled = false;
        }

        public bool ResultDialog { get; set; }

        bool _isLoaded = false;
        private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				txt제목.Text = string.Format("SSAgent Build.{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString());

				if (_isLoaded == false)
				{
					InitProc();
				}

				if (_dt한시간간격 != null) _dt한시간간격.Start();
				if (_dt십분간격 != null) _dt십분간격.Start();
				if (_dt프로그램체크 != null) _dt프로그램체크.Start();

				if (DataManager.DebugYN == true)
				{
					if (_dt한시간간격 != null) _dt한시간간격_Tick(_dt한시간간격, null);
					if (_dt십분간격 != null) _dt십분간격_Tick(_dt십분간격, null);
				}

				chk업로드.IsChecked = false;
			}
			catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
            finally
            {
                if (wnd초기화면.SplashWindow != null)
                {
                    wnd초기화면.SplashWindow.CloseSplashScreen();
                    //CommonUtils.WaitTime(50, true);
                }

                _isLoaded = true;
                this.IsEnabled = true;
            }
        }

        private void InitProc()
        {
            try
            {
                wnd초기화면.SplashWindow.SetProgress(20, "프로그램을 초기화중입니다.");

                //ShortcutManager.CreateShotcut();
                wnd초기화면.SplashWindow.SetProgress(30, "바로가기에 등록중입니다.");

                //CodeManager.Initialize();

               wnd초기화면.SplashWindow.SetProgress(40, "트레이아이콘을 생성중입니다.");

                Init트레이아이콘();

				string ffile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "forceA.info");
				if (System.IO.File.Exists(ffile)) System.IO.File.Delete(ffile);

				wnd초기화면.SplashWindow.SetProgress(50, "현재시간을 설정중입니다.");
                Init타이머();

                ////190627 BHA
                wnd초기화면.SplashWindow.SetProgress(70, "임시폴더를 정리중입니다.");
                Init임시폴더();

				mShareA.ReceiveHandler += MShareA_ReceiveHandler;

                txtBIT_STATE.Text = "미작동중";

                var VersionNo = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                txtVERSION.Text = string.Format("v{0}.{1}.{2}", VersionNo.Major, VersionNo.Minor, VersionNo.Build);

                ScreenSaverManager.NotUse절전대기모드();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void MShareA_ReceiveHandler(byte[] item)
        {
            try
            {
                string msg = Encoding.UTF8.GetString(item);
                msg = msg.Replace('\0', ' ');
                msg = msg.Trim();
                TraceManager.AddAgentLog(string.Format("[IPC수신] BIT→Agent :{0}", msg));
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate ()
                {
                    switch (msg)
                    {
                        case "EXIT":
                            System.Windows.Forms.Application.ExitThread();
                            System.Environment.Exit(0);
                            break;

                        case "UPDATE":
                            Check업데이트();
                            break;

                        case "LOG":
                            Update로그();
                            break;
                    }
                });
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }
                 
        #region ShareMemory

        //SSMemory.SharedMemory mShare = new SSMemory.SharedMemory("BIT", 4096);
        SSMemory.SharedMemory mShareA = new SSMemory.SharedMemory("BITAgent", 4096);
        private void ShareMemoryManager_OnShareMemoryEvent(string ShareMsg)
        {
            try
            {
                //byte[] btDatas = ASCIIEncoding.UTF8.GetBytes(ShareMsg);
                //if (btDatas != null && btDatas.Length > 0)
                //{
                //    mShareA.Write(btDatas);
                //    TraceManager.AddLine(string.Format("[IPC송신] S6→S6A : {0}", ShareMsg));
                //}
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        #endregion

        private void Init임시폴더()
        {
            try
            {
                string tmppath = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp");
                if (System.IO.Directory.Exists(tmppath) == true)
                {
                    string[] tmpfiles = System.IO.Directory.GetFiles(tmppath, "*.*", System.IO.SearchOption.TopDirectoryOnly);

                    TraceManager.AddLog(string.Format("임시폴더내 파일 수 , 총{0}개 ", tmpfiles.Length));
                    int count_err = 0;
                    int count = 0;
                    foreach (string file in tmpfiles)
                    {
                        try
                        {
                            System.IO.File.Delete(file);
                            count++;
                        }
                        catch (System.IO.IOException ex)
                        {
                            TraceManager.AddLog(string.Format("[임시폴더] 삭제실패 : {0} / 이유 : {1}", file, ex.Message));
                            System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
                            count_err++;
                        }
                    }
                    
                    Add상태정보(string.Format("[임시폴더] 파일정리, 성공:{0}개 실패:{1}개", count, count_err));
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        DispatcherTimer _dt프로그램체크= null;
        DispatcherTimer _dt한시간간격= null;
        DispatcherTimer _dt십분간격= null;
        private void Init타이머()
        {
            try
            {
                if (_dt프로그램체크 == null)
                {
                    _dt프로그램체크 = new DispatcherTimer();
					_dt프로그램체크.Tick += _dt프로그램체크_Tick;
                }
                _dt프로그램체크.Interval = TimeSpan.FromSeconds(30);
                _dt프로그램체크.Tag = 0;
                /////////////////////////////////////////////////////////////////////               
                if (_dt한시간간격 == null)
                {
                    _dt한시간간격 = new DispatcherTimer();
					_dt한시간간격.Tick += _dt한시간간격_Tick;
                }
                _dt한시간간격.Interval = TimeSpan.FromHours(1);
                _dt한시간간격.Tag = 0;
                /////////////////////////////////////////////////////////////////////          
                if (_dt십분간격 == null)
                {
                    _dt십분간격 = new DispatcherTimer();
					_dt십분간격.Tick += _dt십분간격_Tick;
                }
                _dt십분간격.Interval = TimeSpan.FromMinutes(10);
                _dt십분간격.Tag = 0;
                /////////////////////////////////////////////////////////////////////         
                txtDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

		#region 트레이 아이콘 초기화

		System.Windows.Forms.NotifyIcon _trayMain = null;

        private void Init트레이아이콘()
        {
            try
            {
                //TraceManager.AddLine(string.Format("트레이 아이콘을 초기화중입니다."));

                if (_trayMain == null)
                {
                    _trayMain = new System.Windows.Forms.NotifyIcon();
                    _trayMain.Visible = true;
                    _trayMain.Icon = Properties.Resources.Siheung;
                    _trayMain.Text = "SSAgent";
                    _trayMain.MouseDoubleClick += _trayMain_MouseDoubleClick;
                }

                System.Windows.Forms.ToolStripMenuItem tsMnuItem9 = new System.Windows.Forms.ToolStripMenuItem();
                tsMnuItem9.Text = "종료";
                tsMnuItem9.Tag = 9;
                tsMnuItem9.Click += TsMnuItem1_Click;

                _trayMain.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
                //_trayMain.ContextMenuStrip.Items.Add(tsMnuItem1);
                //_trayMain.ContextMenuStrip.Items.Add(tsMnuItem2);
                _trayMain.ContextMenuStrip.Items.Add(tsMnuItem9);

             //   TraceManager.AddLine(string.Format("트레이 아이콘을 초기화완료"));
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        void _trayMain_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                System.Windows.Forms.MouseEventArgs args = e as System.Windows.Forms.MouseEventArgs;
                if (args != null)
                {
                    if (args.Button == System.Windows.Forms.MouseButtons.Right) return;
                    if (this.WindowState != WindowState.Normal)
                    {
                        Console.WriteLine("X = {0} , Y = {1}", this.Left, this.Top);
                        this.WindowState = System.Windows.WindowState.Normal;
                        //this.Show();
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //CommonProc.프로세서종료Proc("DxMapConvert");
                DoFinal();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                //MsgDlgManager.ShowInformationDlg("알림", "프로그램이 종료중입니다.\r\n잠시만 기다려 주세요.");
                //CommonUtils.WaitTime(300, true);

                //MainWindow w = sender as MainWindow;
                //if (w == null) return;

                //if (w.ResultDialog)
                //{
                //    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                //}
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        //Alt + F4 방지
        protected override void OnKeyDown(KeyEventArgs e)
        {
            try
            {
                if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt || e.SystemKey == Key.F4)
                {
                    e.Handled = true;
                    return;
                }
                base.OnKeyDown(e);
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void ContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                btnMinimize_Click(null, null);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void btnMainClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowState = System.Windows.WindowState.Minimized;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        #region 마우스창 이동 기능 

        //마우스로 창 이동 ==========================
        private Point mousePoint;
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Point pnt = e.GetPosition(this);
                mousePoint = new Point(pnt.X, pnt.Y);

                ((Grid)sender).CaptureMouse();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_prevExtRect != null) return;

                if (((Grid)sender).IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
                {
                    Point pnt = e.GetPosition(this);
                    this.Left = this.Left - (mousePoint.X - pnt.X);
                    this.Top = this.Top - (mousePoint.Y - pnt.Y);
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ((Grid)sender).ReleaseMouseCapture();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        //윈도우즈 사이즈 변경
        System.Drawing.Rectangle? _prevExtRect = null;

        #endregion

        private void DoFinal()
        {
            try
            {
                if (_dt프로그램체크 != null)
                {
                    _dt프로그램체크.Stop();
                    _dt프로그램체크.Tick -= _dt프로그램체크_Tick;
                    _dt프로그램체크 = null;
                }

                if (_dt십분간격!= null)
                {
                    _dt십분간격.Stop();
                    _dt십분간격.Tick -= _dt십분간격_Tick;
                    _dt십분간격 = null;
                }

                if (_dt한시간간격 != null)
                {
                    _dt한시간간격.Stop();
                    _dt한시간간격.Tick -= _dt한시간간격_Tick;
                    _dt한시간간격 = null;
                }

                GC.Collect();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void TsMnuItem1_Click(object sender, EventArgs e)
        {
            try
            {
				System.Windows.Forms.ToolStripMenuItem item = sender as System.Windows.Forms.ToolStripMenuItem;
				if (item == null) return;

				int tagNo = Convert.ToInt32(item.Tag);
				switch (tagNo)
				{
					case 9:
						if (프로그램종료Proc() == true)
						{
                            DoFinal();
							System.Windows.Forms.Application.ExitThread();
							System.Environment.Exit(0);
						}
						//bool ret = MsgDlgManager.ShowQuestionDlg("확인", "프로그램을 종료하시겠습니까?");
						//if (ret == true)
						//{
							
						//}
						break;

					//case 2:
					//	History업데이트Proc();
					//	break;
				}
			}
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }


        #region 프로그램 실행관련 

        private bool 프로그램종료Proc()
        {
            try
            {
                System.Diagnostics.Process[] processesByName = System.Diagnostics.Process.GetProcessesByName("BitView");
                bool flag = false;
                foreach (System.Diagnostics.Process process in processesByName)
                {
                    process.Kill();
                    process.WaitForExit(3000);
                    process.Close();
                    process.Dispose();
                    flag = true;
                }

                if (flag)
                {
                    Add상태정보("BIT프로그램이 종료되었습니다.");
                    System.Threading.Thread.Sleep(1000);
                }

                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        private bool 프로그램실행Proc()
        {
            try
            {
                string BITEXE = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BitView.exe");
                if (System.IO.File.Exists(BITEXE) == true)
                {
                    Add상태정보("BIT프로그램이 실행되었습니다.");
                    btnMinimize_Click(null, null);
                    //CommonUtils.WaitTime(100, true);

                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(BITEXE);
                    System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo);
                    process.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
                    System.Threading.Thread.Sleep(3000);
                    process.Dispose();
                }
                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        private void 프로그램확인Proc()//object sender, DoWorkEventArgs e)
        {
            try
            {
                //txtCheckTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                int retyCnt = 0;
                while (true)
                {
                    System.Diagnostics.Process[] processesByName = System.Diagnostics.Process.GetProcessesByName("BitView");
                    System.Diagnostics.Process process = null;
                    foreach (System.Diagnostics.Process process2 in processesByName)
                    {
                        process = process2;
                    }
                    System.Threading.Thread.Sleep(500);
                    if (process == null)
                    {
                        txtBIT_STATE.Text = "미작동중";
                    }

                    if (process != null)
                    {
                        txtBIT_STATE.Text = "실행중";
                        if (process.Responding)
                        {
                            retyCnt = 0;
                            break;
                        }
                        else
                        {
                            if (retyCnt < 4)
                            {
                                retyCnt++;
                              //  CommonUtils.WaitTime(100, true);
                                continue;
                            }

                            if (CheckForcedExit()) return;
                            프로그램재실행Proc();
                        }
                     //   CommonUtils.WaitTime(100, true);
                    }
                    else
                    {
                        if (CheckForcedExit()) return;

                        프로그램실행Proc();
                      //  CommonUtils.WaitTime(100, true);
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void 프로그램재실행Proc()
        {
            try
            {
                프로그램종료Proc();
                //CommonUtils.WaitTime(500, true);
                프로그램실행Proc();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private bool CheckForcedExit()
        {
            try
            {
                string ffile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fclose.info");
                if (System.IO.File.Exists(ffile))
                {
                    System.IO.File.Delete(ffile);
                 //   CommonUtils.WaitTime(100, true);
                    System.Windows.Forms.Application.ExitThread();
                    System.Environment.Exit(0);
                    return true;
                }

                return false;
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                return false;
            }
        }

		#endregion

		//private void _dt한시간간격_Tick(object sender, EventArgs e)
		//{
		//	try
		//	{
		//		if (Convert.ToInt32(_dt한시간간격.Tag) == 1) return;
		//		if (_dt한시간간격 != null) _dt한시간간격.Stop();
		//		_dt한시간간격.Tag = 1;

		//		History업데이트Proc();

		//		//이메일전송 체크 및 업데이트
		//		SendIPCMessage("HELLO");
		//		//이메일대기열확인Proc();

		//		_dt한시간간격.Tag = 0;
		//	}
		//	catch (Exception ee)
		//	{
		//		_dt한시간간격.Tag = 0;
		//		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//	}
		//	finally
		//	{
		//		if (_dt한시간간격 != null) _dt한시간간격.Start();
		//	}
		//}

		



		//private void 다운로드Proc(List<TB_PUBLISH> items)
		//{
		//    try
		//    {
		//        Add상태정보(string.Format("컨텐츠 다운로드 시작"));
		//        List<Task> taskList = new List<Task>();

		//        Task<bool> taskDown = Task<bool>.Factory.StartNew(() => DownloadManager.Download컨텐츠(items));
		//        taskList.Add(taskDown);

		//        Task.WaitAll(taskList.ToArray(), TimeSpan.FromSeconds(3));
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//    }
		//}

		public void Add상태정보(string _msg)
        {
            try
            {
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                {
                    //lv상태.Items.Add(_msg);//.Insert(0, _msg);
                    lv상태.Items.Insert(0, _msg);
                    TraceManager.AddAgentLog(_msg);
                    //lv상태.SelectedIndex = lv상태.Items.Count - 1;
                }));
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void _dt십분간격_Tick(object sender, EventArgs e)
        {
			try
			{
				if (Convert.ToInt32(_dt십분간격.Tag) == 1) return;
				if (_dt십분간격 != null) _dt십분간격.Stop();
                _dt십분간격.Tag = 1;

                if (DataManager.ConfigInfo.USE_REBOOT == true)
                {
                    Check재부팅();
                }
                
                _dt십분간격.Tag = 0;
			}
			catch (Exception ee)
			{
                _dt십분간격.Tag = 0;
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			finally
			{
				if (_dt십분간격 != null) _dt십분간격.Start();
			}
		}

        private void _dt프로그램체크_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(_dt프로그램체크.Tag) == 1) return;
                if (_dt프로그램체크 != null) _dt프로그램체크.Stop();
                _dt프로그램체크.Tag = 1;

                txtDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                
                if ( DataManager.DebugYN == false)프로그램확인Proc();               

                _dt프로그램체크.Tag = 0;
            }
            catch (Exception ee)
            {
                _dt프로그램체크.Tag = 0;
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
            finally
            {
                if (_dt프로그램체크 != null) _dt프로그램체크.Start();
            }
        }


        private void _dt한시간간격_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(_dt한시간간격.Tag) == 1) return;
                if (_dt한시간간격 != null) _dt한시간간격.Stop();
                _dt한시간간격.Tag = 1;

                //프로그램 버전체크
                //Check업데이트();

                _dt한시간간격.Tag = 0;
            }
            catch (Exception ee)
            {
                _dt한시간간격.Tag = 0;
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
            finally
            {
                if (_dt한시간간격 != null) _dt한시간간격.Start();
            }
        }

		private void btn등록시작메뉴_Click(object sender, RoutedEventArgs e)
		{
            try
			{
                Button btn = sender as Button;
                if (btn == null) return;
                               
                ShortcutManager.CreateShotcut();

                MessageBox.Show("시작메뉴에 등록되었습니다.");
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

        SQLiteManager mSQL = null;
        BIT_SYSTEM config = null;
        private void Init시스템()
        {
            try
            {
                //edgar removed
                //if (mSQL == null || mSQL.ISConnect() == false)
                //{
                //    mSQL = new SQLiteManager();
                //    mSQL.SetConnectFile(DataManager.ServerInfo.DB_URL);
                //    if (mSQL.ISConnect() == false)
                //    {
                //        Add상태정보("DB에 접속할수 없습니다.");
                //        Add상태정보("버전확인을 진행할 수 없습니다.");
                //        return;
                //    }
                //}

                if (config == null)
                {
                    //string query = string.Format("SELECT * FROM BIT_SYSTEM ORDER BY SEQ_NO DESC LIMIT 1");
                    //List<BIT_SYSTEM> items설정 = DatabaseManager.SELECT_BIT_SYSTEM_BY_QUERY(query);
                    //if (items설정 != null && items설정.Count > 0)
                    //{
                    //    config = items설정.First();
                    //}
                    BIT_SYSTEM bitSystem = SettingsManager.GetBitSystemSettings();
                    if (bitSystem != null) 
                    { 
                        config = bitSystem;
                    }
                }

                if (DataManager.DebugYN == true)
                {
                    config.FTP_GBN = 2;
                    config.FTP_IP = "ruka332.myds.me";
                    config.FTP_PORT = 32700;
                    config.FTP_USERID = "ruka332";
                    config.FTP_USERPWD = "vl117wjd!";
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void Check재부팅()
        {
            try
            {
                string REBOOT_TIME = DataManager.ConfigInfo.REBOOT_TIME;

                if (REBOOT_TIME.Equals("") == true || REBOOT_TIME.Length != 4) return;

                bool IS_POWEROFF_YN = CommonProc.IS_TIME_YN(REBOOT_TIME, false);
                if (IS_POWEROFF_YN == true)
                {
                    Add상태정보("재부팅 시간입니다.");
                    ShutdownManager mPower = new ShutdownManager();
                    if (mPower.Get종료기록() == false)
                    {
                        프로그램종료Proc();
                        mPower.Add종료기록();
                        System.Threading.Thread.Sleep(500);
                        mPower.PC재시작Proc();
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void Check업데이트()
        {
            try
            { 
                if (chk업로드.IsChecked.Value == true)
                {
                    Add상태정보("버전을 확인합니다.");

                    Init시스템();

                    bool b업데이트YN = false;

                    Download버전정보();

                    VersionData ver = Get버전정보();
                    if (ver != null)
                    {
                        b업데이트YN = Check버전비교(ver);
                    }

                    if (b업데이트YN == true)
                    {
                        Add상태정보("업데이트 파일을 다운로드합니다.");
                        Download업데이트파일(ver);
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

		#region 프로그램 버전확인 & 업데이트 

		private async void Download버전정보()
        {
            try
            {
                string 저장DIR = System.IO.Path.Combine(DataManager.Get업데이트DIR(), "VERSION");
                if (System.IO.Directory.Exists(저장DIR) == false) System.IO.Directory.CreateDirectory(저장DIR);
                string 저장FILE_NM = string.Format("VER_SSBIT_{0}.xml", DateTime.Now.ToString("yyyyMMddHH"));

                switch (config.FTP_GBN)
                {
                    case 1:
						FTPManager mFTP = new FTPManager();
						if (System.IO.File.Exists(System.IO.Path.Combine(저장DIR, 저장FILE_NM)) == false)
						{
							int resultNo = await mFTP.Agent다운로드FileAsync(config, 저장DIR, 저장FILE_NM, "/UPDATE/", "VER_SSBIT.xml");
							if (resultNo == 1)
							{
                                Add상태정보(string.Format("[FTP] 버전정보파일 다운로드 완료 : {0}", 저장FILE_NM));
							}
							else
							{
                                Add상태정보(string.Format("[FTP] 버전정보파일 다운로드 오류 : {0}", 저장FILE_NM));
							}
						}
						else
						{
                            Add상태정보(string.Format("[FTP] 버전정보파일 존재 생략 : {0}", 저장FILE_NM));
						}
						break;

                    case 2:
                        SFTPManager mSFTP = new SFTPManager();
                        if (System.IO.File.Exists(System.IO.Path.Combine(저장DIR, 저장FILE_NM)) == false)
                        {                            
                            int resultNo = await mSFTP.Agent다운로드FileAsync(config, 저장DIR, 저장FILE_NM, "/UPDATE/", "VER_SSBIT.xml");
                            if (resultNo == 1)
                            {
                                Add상태정보(string.Format("[SFTP] 버전정보파일 다운로드 완료 : {0}", 저장FILE_NM));
                            }
                            else
                            {
                                Add상태정보(string.Format("[SFTP] 버전정보파일 다운로드 오류 : {0}", 저장FILE_NM));
                            }
                        }
                        else
                        {
                            Add상태정보(string.Format("[SFTP] 버전정보파일 존재 생략 : {0}", 저장FILE_NM));
                        }
                        break;
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private VersionData Get버전정보()
        {
            try
            {
                string 저장DIR = System.IO.Path.Combine(DataManager.Get업데이트DIR(), "VERSION");
                if (System.IO.Directory.Exists(저장DIR) == false) System.IO.Directory.CreateDirectory(저장DIR);
                string 저장FILE_NM = string.Format("VER_SSBIT_{0}.xml", DateTime.Now.ToString("yyyyMMddHH"));

                string FILE_NM = System.IO.Path.Combine(저장DIR, 저장FILE_NM);
                if (System.IO.File.Exists(FILE_NM) == false) return null;

                VersionFile mFile = new VersionFile();
                mFile = mFile.Load(FILE_NM);

                if (mFile.ITEM != null) return mFile.ITEM;

                return null;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        private bool Check버전비교(VersionData 서버VER)
		{
            try
			{
                //버전체크
                var 내부VER = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                if (내부VER.Major < 서버VER.MAJOR_VER)
                {
                    TraceManager.AddAgentLog(string.Format("[업데이트] 메인버전비교 : 프로그램={0} / 서버={1}", 내부VER.Major, 서버VER.MAJOR_VER));
                    return true;
                }

                if (내부VER.Minor < 서버VER.MINOR_VER)
                {
                    TraceManager.AddAgentLog(string.Format("[업데이트] 부버전비교 : 프로그램={0} / 서버={1}", 내부VER.Minor, 서버VER.MINOR_VER));
                    return true;
                }

                if (내부VER.Build < 서버VER.BUILD_VER)
                {
                    TraceManager.AddAgentLog(string.Format("[업데이트] 빌드버전비교 : 프로그램={0} / 서버={1}", 내부VER.Build, 서버VER.BUILD_VER));
                    return true;
                }

                if (DataManager.DebugYN == true) return true;

                return false;
            }
            catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
		}

        private async void Download업데이트파일(VersionData 서버VER)
        {
            try
            {                
                switch (config.FTP_GBN)
                {
                    case 1:
                        FTPManager mFTP = new FTPManager();
                        List<FileData> results = await mFTP.Agent업데이트FileAsync(config, 서버VER);
                        if (results == null || results.Count == 0)
                        {
                            TraceManager.AddAgentLog(string.Format("[FTP] 업데이트파일 다운로드 오류"));
                        }
                        else
                        {
                            string log = string.Format("[FTP] 업데이트파일 다운로드 총 {0}건 중 성공 {1}건", 서버VER.FILES.Count, results.Count);
                            TraceManager.AddAgentLog(log);
                        }
                        break;
                         
                    case 2:
                        SFTPManager mSFTP = new SFTPManager();
                        List<Task> taskList = new List<Task>();
                        Task<List<FileData>> task업데이트= Task<List<FileData>>.Factory.StartNew(() => mSFTP.Agent업데이트Files(config , 서버VER));
                        taskList.Add(task업데이트);
                        Task.WaitAll(taskList.ToArray(), TimeSpan.FromSeconds(5));              
                        break;
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        #endregion

        private void Update로그()
        {
            try
            {
                if (chk업로드.IsChecked.Value == true)
                {
                    Init시스템();

                    string 로그FILE = CommonProc.Compress로그정보();
                    if (로그FILE.Equals("") == false)
                    {
                        Add상태정보("로그파일을 업로드합니다.");
						Upload로그파일(로그FILE, int.Parse(config.BIT_ID));
					}
				}
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private async void Upload로그파일(string FILE_NM, int BIT_ID)
        {
            try
            {
                string 서버FILE = string.Format("LOG_{0}_{1}.zip", DateTime.Now.ToString("yyyyMMddHHmm"), BIT_ID);
                switch (config.FTP_GBN)
                {
                    case 1:
                        FTPManager mFTP = new FTPManager();
                        int resultNo = await mFTP.Agent업로드FileAsync(config, FILE_NM, "/UPDATE/", 서버FILE);
                        if (resultNo == 1)
                        {
                            Add상태정보(string.Format("[FTP] 로그파일 업로드 완료 : {0}", FILE_NM));
                        }
                        else
                        {
                            Add상태정보(string.Format("[FTP] 로그파일 업로드 오류 : {0}", FILE_NM));
                        }
                        break;

                    case 2:
                        SFTPManager mSFTP = new SFTPManager();

                        List<Task> taskList = new List<Task>();
                        Task<int> taskUpdate = Task<int>.Factory.StartNew(() => mSFTP.Agent업로드File(config, FILE_NM, "/UPDATE/", 서버FILE));
                        taskList.Add(taskUpdate);
                        Task.WaitAll(taskList.ToArray(), TimeSpan.FromSeconds(3));
                        //taskUpdate.Wait(1000);
                        int resultSFTP = taskUpdate.Result;
                        if (resultSFTP == 1)
                        {
                            Add상태정보(string.Format("[SFTP] 로그파일 업로드 완료 : {0}", FILE_NM));
                        }
                        else
                        {
                            Add상태정보(string.Format("[SFTP] 로그파일 업로드 오류 : {0}", FILE_NM));
                        }
                        break;
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void btn버전확인_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                Button btn = sender as Button;
                if (btn == null) return;

                Check업데이트();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

		private void btn로그등록_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                Button btn = sender as Button;
                if (btn == null) return;
                               
                Update로그();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void Make버전파일()
		{
            try
			{
                VersionData item = new VersionData();
                item.MAJOR_VER = 1;
                item.MINOR_VER = 1;
                item.BUILD_VER = 2;
                item.START_YMD = new DateTime(2022, 8, 1);
                item.END_YMD = new DateTime(2022, 8, 31);
                item.FILES = new List<FileData>();
                item.FILES.Add(new FileData()
                {
                    SEQ_NO = 1,
                    FILE_EXT = ".zip",
                    FILE_NM = "BIT_V1.1.2.zip",
                    FILE_SZ = 0,
                    LOCAL_URL = "update_apps",
                    REMOTE_URL = "UPDATE/BIT_V1.1.2.zip"
				});

                VersionFile mFile = new VersionFile();
                mFile.ITEM = item;
                mFile.Save(@"D:\VER.xml");
            }
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void chk업로드_Checked(object sender, RoutedEventArgs e)
		{
            try
            {
                CheckBox chk = sender as CheckBox;
                if (chk == null) return;

                btn로그등록.IsEnabled = chk.IsChecked.Value;
                btn버전확인.IsEnabled = chk.IsChecked.Value;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
		}
	}
}
