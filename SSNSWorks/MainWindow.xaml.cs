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

using SSCommonNET;
using SSControlsNET;
using SSData;

namespace SSNSWorks
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public bool ResultDialog { get; set; }

        bool _isLoaded = false;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //System.Windows.Threading.DispatcherTimer _dt초기화 = new System.Windows.Threading.DispatcherTimer();
            try
            {
                txt제목.Text = string.Format("SSNSWorks 파주시BIT v.{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString());

                if (_isLoaded == false)
                {
                    InitProc();
                }

				InitTTSProc();
				//_dt초기화.Interval = TimeSpan.FromSeconds(5);
				//_dt초기화.Tick += delegate (object ds, EventArgs de)
				//{
				//    _dt초기화.Stop();

				//    if (_dt한시간간격 != null) _dt한시간간격.Start();
				//    if (_dt일분간격 != null) _dt일분간격.Start();
				//    if (_dt일초간격 != null) _dt일초간격.Start();

				//    //if (DataManager.DebugYN == true)
				//    {
				//        _dt한시간간격_Tick(_dt한시간간격, null);
				//        _dt일분간격_Tick(_dt일분간격, null);
				//        _dt일초간격_Tick(_dt일초간격, null);
				//    }
				//};
				wnd초기화면.SplashWindow.SetProgress(100, "완료되었습니다. 프로그램이 시작됩니다.");
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
            finally
            {
                if (wnd초기화면.SplashWindow != null)
                {
                    wnd초기화면.SplashWindow.CloseSplashScreen();
                    CommonUtils.WaitTime(50, true);
                }
                //_dt초기화.Start();
                this.Show();
                this.Activate();

                _isLoaded = true;
            }
        }

        private void InitProc()
        {
            try
            {
                //wnd초기화면.SplashWindow.SetProgress(10, "관련 프로그램을 초기화중입니다.");

                wnd초기화면.SplashWindow.SetProgress(20, "프로그램을 초기화중입니다.");

            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

		public MSTTSManager tts = null;
		private void InitTTSProc()
		{
			try
			{
				if (tts == null)
				{
					tts = new MSTTSManager();
				}
				tts.InitializeTTS();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
		private void PlayTTS(string msg)
		{
			try
			{
				//if (playMessage.Equals(msg) == true)
				//{
				//    TraceManager.AddTTSLog(string.Format("[중복생략] {0}", msg));
				//    return;
				//}
				//playMessage = msg;

				string[] busList = msg.Split(',');

				string message = "";
				foreach (string bus in busList)
				{
					if (string.IsNullOrWhiteSpace(bus))
					{
						continue;
					}

					//string bisNo = bus.Replace("-", "다시. ");
					string bisNo = bus;
					bisNo = bisNo.Replace("M", "엠, ");
					bisNo = bisNo.Replace("A", "에이, ");
					bisNo = bisNo.Replace("B", "비, ");
					bisNo = bisNo.Replace("C", "씨, ");
					bisNo = bisNo.Replace("G", "지, ");

					//change number spelling
					bisNo = bisNo.Replace("-1", "다시.일, ");
					bisNo = bisNo.Replace("-2", "다시.이, ");
					bisNo = bisNo.Replace("-3", "다시.삼, ");
					bisNo = bisNo.Replace("-4", "다시.사, ");
					bisNo = bisNo.Replace("-5", "다시.오, ");
					bisNo = bisNo.Replace("-6", "다시.육, ");
					bisNo = bisNo.Replace("-7", "다시.칠, ");
					bisNo = bisNo.Replace("-8", "다시.팔, ");
					bisNo = bisNo.Replace("-9", "다시.구, ");
					bisNo = bisNo.Replace("-0", "다시.영, ");
					//bisNo = bisNo.Replace("Y", "마을버스, ");

					message += string.Format("{0}, 번,", bisNo);
				}

				message += "버스가 곧 도착 예정 입니다.";
				//Console.WriteLine(message.Length);
				tts.Play(message);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public void DoFinal()
        {
            try
            {
                GC.Collect();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }


        //bool 자동종료YN = false;
        //private void _dt로그아웃_Tick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (_dt로그아웃 == null) return;
        //        if (Convert.ToInt32(_dt로그아웃.Tag) == 1) return;
        //        if (_dt로그아웃 != null) _dt로그아웃.Tag = 1;

        //        if (CommonUtils.GetIdleTime() > 180 * 1000)
        //        {
        //            if (_dt로그아웃 != null) _dt로그아웃.Stop();

        //            TraceManager.AddLog("[로그아웃] 장시간 미사용");

        //            자동종료YN = true;
        //            ResultDialog = true;

        //            this.Close();
        //        }

        //        if (_dt로그아웃 != null) _dt로그아웃.Tag = 0;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
        //    }
        //}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                string CloseMsg = string.Format("관리프로그램을 종료하시겠습니까?");

                if (DialogResult.Value == false && !MsgDlgManager.ShowQuestionDlg("종료확인", CloseMsg))
                {
                    e.Cancel = true;
                    // this.IsEnabled = true;
                    return;
                }

                DoFinal();

                //if (자동종료YN == false)
                //{
                //    string CloseMsg = string.Format("국립해양과학관 관리프로그램을 종료하시겠습니까?");

                //    if (DialogResult.Value == false && !MsgDlgManager.ShowQuestionDlg("종료확인", CloseMsg))
                //    {
                //        e.Cancel = true;
                //        // this.IsEnabled = true;
                //        return;
                //    }
                //}

                //if (_dt로그아웃 != null)
                //{
                //    _dt로그아웃.Stop();
                //    _dt로그아웃.Tick -= _dt로그아웃_Tick;
                //    _dt로그아웃 = null;
                //}

                MsgDlgManager.ShowInformationDlg("알림", "프로그램이 종료중입니다.\r\n잠시만 기다려 주세요.");
                CommonUtils.WaitTime(50, true);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                //System.Diagnostics.Process[] processesByName = System.Diagnostics.Process.GetProcessesByName("dxvworldmapper");
                //if (processesByName.Length > 0)
                //{
                //    for (int i = 0; i < processesByName.Length; i++)
                //        processesByName[i].Kill();
                //}
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        //Alt + F4 방지
        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    try
        //    {
        //        if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt || e.SystemKey == Key.F4)
        //        {
        //            e.Handled = true;
        //            return;
        //        }

        //        Console.WriteLine("MainWindows KeyDown {0}", e.Key);
        //        base.OnKeyDown(e);
        //    }
        //    catch (Exception ee)
        //    {
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //    }
        //}

        #region 최대화 최소화
        private void ContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                btnMaximize_Click(null, null);
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
                WindowState = System.Windows.WindowState.Minimized;
                //if (_w가로형 != null) _w가로형.WindowState = WindowState.Normal;
                //this.Hide();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.WindowState == System.Windows.WindowState.Normal)
                {
                    this.WindowState = System.Windows.WindowState.Maximized;
                    icNormal.Visibility = System.Windows.Visibility.Collapsed;
                    icMaximize.Visibility = System.Windows.Visibility.Visible;
                }
                else if (this.WindowState == System.Windows.WindowState.Maximized)
                {
                    this.WindowState = System.Windows.WindowState.Normal;
                    icNormal.Visibility = System.Windows.Visibility.Visible;
                    icMaximize.Visibility = System.Windows.Visibility.Collapsed;
                }
                CommonUtils.WaitTime(10, true);
                // DxMapper.DxMapperManager.SetSizeChangedEvent();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        #endregion

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

        Point _curGridSP;
        Size _curSize;

        private void gridGrip_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (this.WindowState != System.Windows.WindowState.Normal) return;

                if (e.ButtonState == MouseButtonState.Pressed)
                {
                    _curGridSP = e.GetPosition(this);
                    _curSize = new Size(this.Width, this.Height);

                    gridGrip.CaptureMouse();
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }

        }

        private void gridGrip_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.WindowState != System.Windows.WindowState.Normal || _prevExtRect != null) return;

                if (e.LeftButton == MouseButtonState.Pressed)
                {

                    Vector gap = e.GetPosition(this) - _curGridSP;

                    double w = _curSize.Width + gap.X;
                    if (w < this.MinWidth) w = this.MinWidth;
                    double h = _curSize.Height + gap.Y;
                    if (h < this.MinHeight) h = this.MinHeight;

                    this.Width = w;
                    this.Height = h;
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }

        }

        private void gridGrip_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (this.WindowState != System.Windows.WindowState.Normal) return;

                gridGrip.ReleaseMouseCapture();

                //DxMapper.DxMapperManager.SetSizeChangedEvent();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        System.Drawing.Rectangle? _prevExtRect = null;


        #endregion

        private void Check기초코드()
        {
            try
            {
                //int 코드Ver = CodeManager.CodeConfigFile.CodeVersion;
                //Console.WriteLine("CodeVersion : {0}", 코드Ver);

                //string query = "SELECT * FROM BC_CODE WHERE CD_GBN_NO=0 ";
                //List<BC_CODE> items코드 = DatabaseManager.SELECT_BC_CODE_BY_QUERY(query);
                //if (items코드 == null || items코드.Count == 0) return;

                //int 서버Ver = items코드.First().CD;
                //if (코드Ver.Equals(0) == true || 코드Ver < 서버Ver)
                //{
                //    query = "SELECT * FROM BC_CODE WHERE CD_GBN_NO>0 ";

                //    List<BC_CODE> items신규 = DatabaseManager.SELECT_BC_CODE_BY_QUERY(query);
                //    if (items신규 != null && items신규.Count > 0)
                //    {
                //        TraceManager.AddLine(string.Format("[기초코드] 업데이트 기존 {0}→{1}", 코드Ver, 서버Ver));
                //        CodeManager.CodeConfigFile.ITEMS = items신규;
                //        CodeManager.CodeConfigFile.CodeVersion = 서버Ver;
                //        CodeManager.CodeConfigFile.Save();
                //    }
                //}
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void btn설정저장_Click(object sender, RoutedEventArgs e)
        {
            PlayTTS("M9758-1");
        }
    }
}
