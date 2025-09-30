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
using System.Windows.Media.Animation;

namespace BitView
{
    /// <summary>
    /// wnd비밀번호입력.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wnd숫자입력창 : Window
    {
        public wnd숫자입력창()
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.Left = 0;
                //this.Top = 0;

                //ShowAnimation();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        //private void ShowAnimation()
        //{
        //    try
        //    {
        //        double 이동전 = mainGrid.Height * -1;
        //        double 이동후 = (this.Height - mainGrid.Height) / 2;

        //        DoubleAnimation dasShow = new DoubleAnimation(이동전, 이동후, new Duration(TimeSpan.FromMilliseconds(DataManager.ConfigInfo.ANIME_DELAY)), FillBehavior.HoldEnd);
        //        dasShow.AccelerationRatio = 0.2;
        //        dasShow.DecelerationRatio = 0.8;
        //        dasShow.Completed += delegate (object ds, EventArgs de)
        //        {
        //            //Init정보();
        //        };
        //        mainGrid.BeginAnimation(Canvas.TopProperty, dasShow);
        //    }
        //    catch (Exception ee)
        //    {
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //    }
        //}


        ///// <summary>
        ///// 종료시 위로 이동
        ///// </summary>
        //private void HideAnimation()
        //{
        //    try
        //    {
        //        double 이동후 = mainGrid.Height * -1;
        //        double 이동전 = (this.Height - mainGrid.Height) / 2;

        //        DoubleAnimation daSearch = new DoubleAnimation(이동전, 이동후, new Duration(TimeSpan.FromMilliseconds(DataManager.ConfigInfo.ANIME_DELAY)), FillBehavior.HoldEnd);
        //        daSearch.AccelerationRatio = 0.2;
        //        daSearch.DecelerationRatio = 0.8;

        //        mainGrid.BeginAnimation(Canvas.TopProperty, daSearch);

        //        CommonUtils.WaitTime(DataManager.ConfigInfo.ANIME_DELAY + 100, true);
        //    }
        //    catch (Exception ee)
        //    {
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //    }
        //}
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

        int ssns = 6809;
        //public void SET_PIN_NO(string passWord)
        //{
        //    try
        //    {
        //        PIN_NO = passWord;
        //    }
        //    catch (Exception ee)
        //    {
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //    }
        //}

        string _formular = "";
        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ssButton btn = sender as ssButton;
                if (btn == null) return;
                if (btn.Tag == null) return;

                string key = btn.Tag.ToString();
                switch (key)
                {
                    case "0":
                        if (_formular.Length.Equals(1) && _formular[0].Equals('0')) return;
                        _formular += "0";
                        break;
                    case "1":
                        _formular += "1";
                        break;
                    case "2":
                        _formular += "2";
                        break;
                    case "3":
                        _formular += "3";
                        break;
                    case "4":
                        _formular += "4";
                        break;
                    case "5":
                        _formular += "5";
                        break;
                    case "6":
                        _formular += "6";
                        break;
                    case "7":
                        _formular += "7";
                        break;
                    case "8":
                        _formular += "8";
                        break;
                    case "9":
                        _formular += "9";
                        break;
                    case ".": //소수점
                        _formular += ".";                       
                        break;                   
                    case "r": //다시 입력
                        _formular = "";
                        break;
                    case "d": //지우기
                        if (_formular.Length > 0) _formular = _formular.Remove(_formular.Length - 1, 1);
                        break;
                     case "OK":
                        Check비밀번호(_formular);
                        break;
                    case "Close": //닫기                        
                        DoFinal();
                        this.Close();
                        break;
                }
                txtPassword.Password = _formular;
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public void DoFinal()
		{
            try
			{
                //HideAnimation();
                GC.Collect();
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            GC.Collect();
        }

        public delegate void 번호일치Handler(bool matchYN);
        public event 번호일치Handler On번호일치Event;

        private void Check비밀번호(string mNum)
        {
            try
            {
                if (CommonUtils.IsNumeric(mNum) == false)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("입력오류", "비밀번호는 숫자만 가능합니다.");
                    txtPassword.Password = "";
                    return;
                }

                int num = Convert.ToInt32(mNum);
                if (num.Equals(ssns) == true)
                {
                    if (On번호일치Event != null) On번호일치Event(true);
                    this.Close();
                }
                else
                {
                    MsgDlgManager.ShowIntervalInformationDlg("미일치", "비밀번호가 일치하지 않습니다.");
                    _formular = "";
                    txtPassword.Password = "";
                    return;
                }
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }
    }
}