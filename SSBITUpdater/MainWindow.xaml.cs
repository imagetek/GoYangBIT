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

namespace SSBITUpdater
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
                this.Title = string.Format("BIT프로그램 업데이터 v.{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString());

                if (_isLoaded == false)
                {
                    InitProc();
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
            finally
            {
                _isLoaded = true;
                this.IsEnabled = true;
            }
        }

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
                this.Hide();
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
                GC.Collect();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

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


        private void InitProc()
        {
            try
            {
                DISP_LOG(0, "실행프로그램을 체크중입니다.");

                Check실행프로그램();

                DISP_LOG(5, "프로그램 업데이트를 시작합니다.");

                bool bResult = 프로그램Update();

                TraceManager.AddUpdaterLog(string.Format("업데이트 종료 :  결과 : {0}", bResult.ToString()));

                DISP_LOG(99, "Agent프로그램을 다시 실행합니다.");

                try
				{
                    string DIR = System.IO.Path.Combine(UpdateManager.APPSStartPath, "update_apps");
                    System.IO.Directory.Delete(DIR, true);
                }
                catch ( System.IO.IOException ex)
				{
                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                }

                Agent프로그램실행Proc();

                System.Threading.Thread.Sleep(1000);

                System.Windows.Forms.Application.ExitThread();
                System.Environment.Exit(0);

            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void DISP_LOG(double dbPercent, string msg)
        {
            try
            {
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                {
                    pgrMain.Value = dbPercent;
                    txt메세지.Text = msg;
                }));
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
		}

        private void Check실행프로그램()
        {
            try
            {
                프로그램종료Proc("BitView");
                System.Threading.Thread.Sleep(1000);
                프로그램종료Proc("SSAgent");
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private bool 프로그램종료Proc(string PROC_NM)
        {
            try
            {
                System.Diagnostics.Process[] processesByName = System.Diagnostics.Process.GetProcessesByName(PROC_NM);
                bool flag = false;
                foreach (System.Diagnostics.Process process in processesByName)
                {
                    TraceManager.AddUpdaterLog(string.Format("[{0}] 프로그램종료를 실행합니다.", PROC_NM));

                    process.Kill();
                    process.WaitForExit(3000);
                    process.Close();
                    process.Dispose();
                    flag = true;
                }

                if (flag)
                {
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

        private bool Agent프로그램실행Proc()
        {
            try
            {
                string mEXE = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("SSAgent.exe"));
                if (System.IO.File.Exists(mEXE) == true)
                {
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(mEXE);
                    System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo);
                    //process.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
                    process.PriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal;
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

        private bool 프로그램Update()
        {
            try
            {
                TraceManager.AddUpdaterLog("업데이트 시작합니다.");

                string 원본DIR = System.IO.Path.Combine(UpdateManager.APPSStartPath, "update_apps");
                if (System.IO.Directory.Exists(원본DIR) == false) return false;

                string 저장DIR = System.IO.Directory.GetCurrentDirectory();

                List<string> items파일 = System.IO.Directory.GetFiles(원본DIR, "*.*", System.IO.SearchOption.AllDirectories).ToList();
                if (items파일 == null || items파일.Count == 0) return false;

                int idxNo = 1;
                int TotalCNT = items파일.Count;
                int SuccessCNT = 0;
                int ErrorCNT = 0;

                foreach (string item파일 in items파일)
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(item파일);
                    double PER = GetPercent(idxNo, items파일.Count);
                    DISP_LOG(PER, string.Format("[업데이트중] {0}", fi.Name));
                    bool bCopySuccess = false;
                    try
                    {
                        string DestFile = "";
                        if (fi.DirectoryName.Equals(원본DIR) == true)
                        {
                            DestFile = System.IO.Path.Combine(저장DIR, fi.Name);
                        }
                        else
                        {
                            string SubPath = fi.DirectoryName.Replace(원본DIR, "").Replace(@"\", "");
                            SubPath = System.IO.Path.Combine(저장DIR, SubPath);
                            if (System.IO.Directory.Exists(SubPath) == false) System.IO.Directory.CreateDirectory(SubPath);

                            DestFile = System.IO.Path.Combine(SubPath, fi.Name);
                        }
                        
                        fi.CopyTo(DestFile, true);
                        bCopySuccess = true;
                        SuccessCNT++;
                    }
                    catch (System.IO.IOException ex)
                    {
                        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                        ErrorCNT++;
                    }
                    idxNo++;
                    try
                    {
                        if (bCopySuccess == true && System.Diagnostics.Debugger.IsAttached == false) fi.Delete();
                    }
                    catch (System.IO.IOException ex)
                    {
                        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                    }
                    fi = null;
                }

                TraceManager.AddUpdaterLog(string.Format("[결과] 총 {0}건 업데이트되었습니다. [완료: {1}건 오류: {2}건]", TotalCNT, SuccessCNT, ErrorCNT));
                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        double GetPercent(int ix, int iy)
        {
            try
            {
                if (ix == 0 || iy == 0) return 0;

                double dx = Convert.ToDouble(ix);
                double dy = Convert.ToDouble(iy);

                double ret = Math.Round(dx / dy, 3);
                return ret * 100;
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return 0;
            }
        }

    }
}
