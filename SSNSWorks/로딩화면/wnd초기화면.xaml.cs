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

namespace SSNSWorks
{
    /// <summary>
    /// wndSplash.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wnd초기화면 : Window
    {
        private static wnd초기화면 _form = null;
        public static wnd초기화면 SplashWindow
        {
            get { return _form; }
            set { _form = value; }
        }

        public wnd초기화면()
        {
            try
            {
                InitializeComponent();
                this.board.Completed += OnAnimationCompleted;

                pgrMain.Minimum = 0;
                pgrMain.Maximum = 100;
                pgrMain.Value = 0;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public static void Show초기화면()
        {
            try
            {
                if (_form == null)
                {
                    _form = new wnd초기화면();
                    _form.Show();
                }
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
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
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        #region ISplashScreen
        
        public void SetProgress(double value, string _msg="")
        {
            try
            {
                pgrMain.Value = value;
                lblMsg.Content = _msg;                
                CommonUtils.WaitTime(10, true);
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public void SetMessage(string _msg)
        {
            try
            {
                lblMsg.Content = _msg;
                CommonUtils.WaitTime(10, true);
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public void CloseSplashScreen()
        {
            this.board.Begin(this);
        }
        public void SetProgressState(bool isIndeterminate)
        {
            pgrMain.IsIndeterminate = isIndeterminate;
        }

        #endregion

        #region Event Handlers

        void OnAnimationCompleted(object sender, EventArgs e)
        {
            try
            {
                this.board.Completed -= OnAnimationCompleted;
                this.Close();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        #endregion

        private void btnok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
