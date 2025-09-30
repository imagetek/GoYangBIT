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
using System.Windows.Navigation;
using System.Windows.Shapes;

using SSCommonNET;
using SSControlsNET;
using SSData;

namespace BitView
{
    /// <summary>
    /// ucMessageBar.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class uc메뉴바 : UserControl
    {
        public uc메뉴바()
        {
            InitializeComponent();

			this.FontFamily = (FontFamily)FindResource(DataManager.ConfigInfo.FONTS_NM);
			this.FontSize = DataManager.ConfigInfo.FONTS_SZ;
		}

        bool _isLoaded = false;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_isLoaded == false)
                {
                    InitProc();
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
            finally
            {
                _isLoaded = true;
            }
        }

        public delegate void 메뉴선택Handler(프로그램메뉴Type mnu선택);
        public event 메뉴선택Handler On메뉴선택Event;
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

        프로그램메뉴Type Sel메뉴 = 프로그램메뉴Type.NONE;
        public void Set메뉴(프로그램메뉴Type mnu, bool isEvent = false)
        {
            try
            {
                Sel메뉴 = mnu;

                Clear메뉴();

                int mnuNo = Convert.ToInt32(mnu);
                ssToggleButton btn = sp메뉴.FindName(string.Format("btnM{0}", mnuNo)) as ssToggleButton;
                if (btn != null && btn.IsChecked.Value == false)
                {
                    btn.IsChecked = true;
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public void Clear메뉴()
        {
            try
            {
                _ignoreEvent = true;

                foreach (UIElement child in sp메뉴.Children)
                {
                    ssToggleButton cbtn = child as ssToggleButton;
                    if (cbtn == null) continue;
                    if (cbtn.IsChecked.Value) cbtn.IsChecked = false;
                }

                _ignoreEvent = false;

                Sel메뉴 = 프로그램메뉴Type.NONE;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        bool _ignoreEvent = false;
        
        private void btnM1_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ssToggleButton btn = sender as ssToggleButton;
                if (btn == null) return;
                if (btn.Tag == null) return;

                //이벤트 무시
                if (_ignoreEvent) return;

                //버튼이 체크되어 있는데 한번더 눌렀을 경우 미체크를 방지하기 위함.
                if (!btn.IsChecked.Value)
                {
                    _ignoreEvent = true;

                    btn.IsChecked = true;

                    _ignoreEvent = false;
                    return;
                }

                //이벤트 무시
                _ignoreEvent = true;

                //모든 버튼을 미체크
                foreach (UIElement child in sp메뉴.Children)
                {
                    ssToggleButton cbtn = child as ssToggleButton;
                    if (cbtn == null) continue;
                    if (cbtn.IsChecked.Value) cbtn.IsChecked = false;
                }

                this.IsEnabled = false;
                int tagNo = Convert.ToInt32(btn.Tag);
                프로그램메뉴Type item = (프로그램메뉴Type)tagNo;
                if (Sel메뉴.Equals(item) == true) return;

                if (On메뉴선택Event != null) On메뉴선택Event(item);

                Sel메뉴 = item;
                
                //선택 버튼 체크
                btn.IsChecked = true;

                _ignoreEvent = false;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                _ignoreEvent = false;
            }
            finally
            {
                btn닫기_Click(btn닫기, null);
                this.IsEnabled = true;
            }
        }

        private void Show상세메뉴()
        {
            try
            {
                if (DetailGrid.Visibility == Visibility.Visible) return;

                double 이동전 = 35;
                double 이동후 = 150;

                DoubleAnimation dasShow = new DoubleAnimation(이동전, 이동후, new Duration(TimeSpan.FromMilliseconds(DataManager.ConfigInfo.ANIME_DELAY)), FillBehavior.HoldEnd);
                dasShow.AccelerationRatio = 0.2;
                dasShow.DecelerationRatio = 0.8;
                dasShow.Completed += delegate (object ds, EventArgs de)
                {
                    SlimGrid.Visibility = Visibility.Collapsed;
                    DetailGrid.Visibility = Visibility.Visible;
                };
                this.BeginAnimation(UserControl.WidthProperty, dasShow);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void Hide상세메뉴()
        {
            try
            {
                if (SlimGrid.Visibility == Visibility.Visible) return;

                double 이동전 = 150;
                double 이동후 = 35;

                DoubleAnimation dasShow = new DoubleAnimation(이동전, 이동후, new Duration(TimeSpan.FromMilliseconds(DataManager.ConfigInfo.ANIME_DELAY)), FillBehavior.HoldEnd);
                dasShow.AccelerationRatio = 0.2;
                dasShow.DecelerationRatio = 0.8;
                dasShow.Completed += delegate (object ds, EventArgs de)
                {
                    DetailGrid.Visibility = Visibility.Collapsed;
                    SlimGrid.Visibility = Visibility.Visible;
                };
                this.BeginAnimation(UserControl.WidthProperty, dasShow);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void btn이전_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                ssButton btn = sender as ssButton;
                if (btn == null) return;

                Hide상세메뉴();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

		private void btnC1_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                ssButton btn = sender as ssButton;
                if (btn == null) return;
                if (btn.Tag == null) return;

                int tagNo = Convert.ToInt32(btn.Tag);
                프로그램메뉴Type item = (프로그램메뉴Type)tagNo;
                if (On메뉴선택Event != null) On메뉴선택Event(item);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

		private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
            try
			{
                if (e.LeftButton != MouseButtonState.Pressed) return;
                TextBlock txt = sender as TextBlock;
                if (txt == null) return;
                if (txt.Tag == null) return;

                int tagNo = Convert.ToInt32(txt.Tag);
                프로그램메뉴Type item = (프로그램메뉴Type)tagNo;
                switch (item)
                {
                    case 프로그램메뉴Type.이전으로:
                        break;

                    case 프로그램메뉴Type.HOME:
                        Clear메뉴();
                        btnM1.IsChecked = true;
                        break;

                    case 프로그램메뉴Type.BIT정보:
                        Clear메뉴();
                        btnM2.IsChecked = true;
                        break;

                    case 프로그램메뉴Type.환경보드:
                        Clear메뉴();
                        btnM3.IsChecked = true;
                        break;

                    //case 프로그램메뉴Type.웹캠:
                    //    Clear메뉴();
                    //    btnM4.IsChecked = true;
                    //    break;

                    case 프로그램메뉴Type.BIT설정:
                        if (On메뉴선택Event != null) On메뉴선택Event(프로그램메뉴Type.BIT설정);
                        break;

                    case 프로그램메뉴Type.환경설정:
                        if (On메뉴선택Event != null) On메뉴선택Event(프로그램메뉴Type.환경설정);
                        break;
                }

                btn닫기_Click(btn닫기, null);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
		}

		private void btn열기_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                ssButton btn = sender as ssButton;
                if (btn == null) return;

                Show상세메뉴();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

		private void btn닫기_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                ssButton btn = sender as ssButton;
                if (btn == null) return;

                Hide상세메뉴();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }
	}
}
