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

namespace SSAgent
{
    /// <summary>
    /// wndLogin.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wnd설치경로 : Window
    {
        public wnd설치경로()
        {
            InitializeComponent();
        }

        public void SetWindowStartLocation(System.Windows.WindowStartupLocation sl)
        {
            this.WindowStartupLocation = sl;
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
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
            finally
            {
                _isLoaded = true;
                this.IsEnabled = true;
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

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = false;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }
    }
}
