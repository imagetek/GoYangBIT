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
using System.Windows.Shapes;

using SSCommonNET;
using SSControlsNET;
using SSData;
using DirectShowLib;

namespace BitView
{
    public partial class wnd관리자비번 : Window
    {
        public wnd관리자비번()
        {
            InitializeComponent();

            this.FontFamily = (FontFamily)FindResource(DataManager.ConfigInfo.FONTS_NM);
            this.FontSize = DataManager.ConfigInfo.FONTS_SZ;
        }

        Window _p = null;
        public void SetParentWindow(Window p)
        {
            try
            {
                _p = p;
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        bool _isLoaded = false;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_isLoaded == false)
                {
                    InitProc();

                    Load기본값();

                    Load설정값();
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
            finally
            {
                _isLoaded = true;
            }
        }

        #region 윈도우창 기본 코드 

        public override void OnApplyTemplate()
        {
            try
            {
                Grid gridMain = GetTemplateChild("PART_TITLEBAR") as Grid;
                if (gridMain != null)
                {
                    gridMain.MouseDown += Grid_MouseLeftButtonDown;
                    gridMain.MouseMove += Grid_MouseMove;
                    gridMain.MouseUp += Grid_MouseLeftButtonUp;
                }

                Button btnClose = GetTemplateChild("btnClose") as Button;
                if (btnClose != null)
                {
                    btnClose.Click += btnMainClose_Click;
                }

                Button btnMainClose = GetTemplateChild("btnMainClose") as Button;
                if (btnMainClose != null)
                {
                    btnMainClose.Click += btnMainClose_Click;
                }
                //  
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("## {0}\r\n{1} ##", ee.StackTrace, ee.Message));
            }
            base.OnApplyTemplate();
        }

        private void btnMainClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region 단축키 이벤트

        protected override void OnKeyDown(KeyEventArgs e)
        {
            try
            {
                //Console.WriteLine("## override OnKeyDown : 환경설정 ##");
                if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt || e.SystemKey == Key.F4)
                {
                    e.Handled = true;
                    return;
                }
                switch (e.Key)
                {
                    case Key.Escape:
                        btn닫기_Click(btn닫기, null);
                        break;
                }
                base.OnKeyDown(e);
            }
            catch (Exception ee)
            {
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
                //Console.WriteLine("## Grid_MouseLeftButtonDown : 환경설정 ##");
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

        System.Drawing.Rectangle? _prevExtRect = null;
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_prevExtRect != null) return;
                //Console.WriteLine("## Grid_MouseMove : 환경설정 ##");
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
                //Console.WriteLine("## Grid_MouseLeftButtonUp : 환경설정 ##");
                ((Grid)sender).ReleaseMouseCapture();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        #endregion

        //#region 윈도우 사이즈 변경

        //Point _curGridSP;
        //Size _curSize;
        //private void gridGrip_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //	try
        //	{
        //		if (this.WindowState != System.Windows.WindowState.Normal) return;

        //		if (e.ButtonState == MouseButtonState.Pressed)
        //		{
        //			_curGridSP = e.GetPosition(this);
        //			_curSize = new Size(this.Width, this.Height);

        //			gridGrip.CaptureMouse();
        //		}
        //	}
        //	catch (Exception ee)
        //	{
        //		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //	}
        //}

        //private void gridGrip_MouseMove(object sender, MouseEventArgs e)
        //{
        //	try
        //	{
        //		if (this.WindowState != System.Windows.WindowState.Normal || _prevExtRect != null) return;

        //		if (e.LeftButton == MouseButtonState.Pressed)
        //		{

        //			Vector gap = e.GetPosition(this) - _curGridSP;

        //			double w = _curSize.Width + gap.X;
        //			if (w < this.MinWidth) w = this.MinWidth;
        //			double h = _curSize.Height + gap.Y;
        //			if (h < this.MinHeight) h = this.MinHeight;

        //			this.Width = w;
        //			this.Height = h;
        //		}
        //	}
        //	catch (Exception ee)
        //	{
        //		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //	}
        //}

        //private void gridGrip_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //	try
        //	{
        //		if (this.WindowState != System.Windows.WindowState.Normal) return;

        //		gridGrip.ReleaseMouseCapture();

        //		//Console.WriteLine("{0}X{1}", this.ActualWidth, this.ActualHeight);
        //		//DxMapper.DxMapperManager.SetSizeChangedEvent();
        //	}
        //	catch (Exception ee)
        //	{
        //		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //	}
        //}

        //#endregion

        #endregion

        private void btn닫기_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                DoFinal();
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

        private void InitProc()
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

        private void Load기본값()
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

        private void Load설정값()
        {
            try
            {
                txtPASSWD.Password = "";
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public string AdminPassword = "6809";

        public delegate void 비밀번호일치Handler();
        public event 비밀번호일치Handler On비밀번호일치Event;
        private void btn저장_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IS_CHECK_ITEM() == false) return;
                                                
                if (On비밀번호일치Event != null) On비밀번호일치Event();

                this.Close();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        bool IS_CHECK_ITEM()
        {
            try
            {
                if (txtPASSWD.Password.Trim().Equals("") == true)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("미입력", "비밀번호를 입력해주세요.");
                    txtPASSWD.Focus();
                    return false;
                }

                if (AdminPassword.Equals(txtPASSWD.Password.Trim()) == false)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("미일치", "비밀번호가 일치하지 않습니다.");
                    txtPASSWD.Password = "";
                    txtPASSWD.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

    }
}


