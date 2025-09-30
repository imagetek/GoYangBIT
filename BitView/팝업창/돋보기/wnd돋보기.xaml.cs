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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using SSCommonNET;
using SSControlsNET;
using SSData;

namespace BitView
{
    /// <summary>
    /// wnd계산기.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wnd돋보기 : Window
    {
        public wnd돋보기()
        {
            try
            {
                InitializeComponent();

                this.FontFamily = (FontFamily)FindResource(DataManager.ConfigInfo.FONTS_NM);
                this.FontSize = DataManager.ConfigInfo.FONTS_SZ;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
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
                }

                //if (DataManager.ConfigInfo.SGG_CD.Equals("") == true)
                //{
                //    cmbDO.SelectedIndex = 0;
                //}
                //else
                //{
                //    cmbDO.SelectedValue = DataManager.ConfigInfo.SGG_CD.Substring(0, 2);
                //}                
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
                        //btn닫기_Click(btn닫기, null);
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

        #region 윈도우 사이즈 변경

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

                //Console.WriteLine("{0}X{1}", this.ActualWidth, this.ActualHeight);
                //DxMapper.DxMapperManager.SetSizeChangedEvent();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        #endregion

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
                if (_dt확인 != null)
                {
                    _dt확인.Stop();
                    _dt확인.Tick -= _dt확인_Tick;
                    _dt확인 = null;
                }

                GC.Collect();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        System.Windows.Threading.DispatcherTimer _dt확인 = null;
        private void InitProc()
        {
            try
            {
                this.Width = 640;
                this.Height = 480;

                if (_dt확인 == null)
                {
                    _dt확인 = new System.Windows.Threading.DispatcherTimer();
                    _dt확인.Tick += _dt확인_Tick;
                }
                _dt확인.Interval = TimeSpan.FromSeconds(1);
                _dt확인.Tag = 0;
                _dt확인.Start();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void _dt확인_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(_dt확인.Tag) == 1) return;
                _dt확인.Tag = 1;

                Display정보();

                _dt확인.Tag = 0;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                _dt확인.Tag = 0;
            }
        }

        double dbRate = 2.0;
        private void Display정보()
        {
            try
            {
                txt배율.Text = string.Format("배율 : x{0}", dbRate);


                int nWidth = Convert.ToInt32(img.Width);
                int nHeight = Convert.ToInt32(img.Height);

                if (nWidth > 0 && nHeight > 0)
                {
                    System.Drawing.Size size = new System.Drawing.Size(nWidth, nHeight);
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)(size.Width / dbRate), (int)(size.Height / dbRate));
                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);

                    Point p = Mouse.GetPosition(this);
                    if (p != null)
                    {
                        graphics.CopyFromScreen((int)(p.X - size.Width / (dbRate * 2)), (int)(p.Y - size.Height / (dbRate * 2)), 0, 0, size, System.Drawing.CopyPixelOperation.SourceCopy);
                        img.Source = ImageUtils.ConvertBitmapToBitmapSource(bitmap);
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void btn확대_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dbRate < 10)
                {
                    dbRate += 0.1;
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void btn축소_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dbRate > 0.5)
                {
                    dbRate -= 0.1;
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }

        }

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
            try
			{
				if ( e.WidthChanged == true)
				{
                    img.Width = e.NewSize.Width - 10;
				}

                if (e.HeightChanged == true)
                {
                    img.Height = e.NewSize.Height - 85 - 10;
                }
            }
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
	}
    
}

