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
using SSData;
using SSData.DashboardAPI;

namespace BitView
{
    /// <summary>
    /// ucMessageBar.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class uc상태바 : UserControl
    {
        public uc상태바()
        {
            InitializeComponent();

			//this.FontFamily = (FontFamily)FindResource(DataManager.ManagerInfo.FONTS_NM);
			//this.FontSize = DataManager.ManagerInfo.FONTS_SZ;
		}

		System.Windows.Threading.DispatcherTimer _dtTimer = null;
        bool _loaded = false;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_loaded)
                {
					SSENV2Manager.OnSerialPortErrorEvent += SSENV2Manager_OnSerialPortErrorEvent;	//pjh
					
					if (_dtTimer == null)
                    {
                        _dtTimer = new System.Windows.Threading.DispatcherTimer();
                        _dtTimer.Tag = 0;
                        _dtTimer.Interval = TimeSpan.FromSeconds(1);
                        _dtTimer.Tick += _dtTimer_Tick;
                    }
                    _dtTimer_Tick(null, null);
                    _dtTimer.Start();
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
            finally
            {
                _loaded = true;
            }
        }

        public void RefreshData()
        {
            try
            {
                //txt사용자.Text  = "시흥시청";
                //txtUser.Text = string.Format("{0} : [{1}] {2}", SiteNM, item.USER_ID, item.USER_NM);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        void _dtTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(_dtTimer.Tag) == 1) return;
                _dtTimer.Tag = 1;

                txtDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:sss");
                //if (txt사용자.Text.Trim().Equals(""))
                //{
                //    RefreshData();
                //}

				_dtTimer.Tag = 0;
            }
            catch (Exception ee)
            {
                _dtTimer.Tag = 0;
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }
	
		static int spErrorCount = 0;
		DateOnly currentErrorCountDate = DateOnly.FromDateTime(DateTime.Now);
		private void SSENV2Manager_OnSerialPortErrorEvent()
		{
			DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
			if (currentErrorCountDate == currentDate)
			{
				spErrorCount++;
			}

			if (currentDate > currentErrorCountDate)
			{
				currentErrorCountDate = currentDate;
				spErrorCount = 0;
			}

			spErrorCountTxt.Text = $"{spErrorCount}";

			if (spErrorCount > 10)
			{
				spErrorCountTxt.Foreground = System.Windows.Media.Brushes.Red;
			}
			else
			{
				spErrorCountTxt.Foreground = System.Windows.Media.Brushes.Black;
			}
			HttpService.UpdateSystemStatus(new(), 0);
		}
	}
}
