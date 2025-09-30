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
    public partial class wnd환경설정 : Window
    {
        public wnd환경설정()
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

                    Load기본값();

                    Load설정값();
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


        #region 애니메이션 이벤트 미사용

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

        #endregion

        private void InitProc()
        {
            try
            {
                //폰트종류
                cmbFONTS_GBN.ItemsSource = null;
                cmbFONTS_GBN.DisplayMemberPath = "NAME";
                cmbFONTS_GBN.SelectedValuePath = "S_NM";

                //프로그램종류
                cmbPGM_GBN.ItemsSource = null;
                cmbPGM_GBN.DisplayMemberPath = "NAME";
                cmbPGM_GBN.SelectedValuePath = "nCODE";

                ////도
                //cmbDO.ItemsSource = null;
                //cmbDO.DisplayMemberPath = "DO_NM";
                //cmbDO.SelectedValuePath = "DO_CD";

                ////시군구
                //cmbSGG.ItemsSource = null;
                //cmbSGG.DisplayMemberPath = "SGG_NM";
                //cmbSGG.SelectedValuePath = "SGG_CD";

                //DB종류
                //cmbDB_GBN.ItemsSource = null;
                //cmbDB_GBN.DisplayMemberPath = "NAME";
                //cmbDB_GBN.SelectedValuePath = "nCODE";
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
                cmbFONTS_GBN.ItemsSource = CodeManager.Get기초코드(기초코드Type.FONTS종류);

                //도
                cmbPGM_GBN.ItemsSource = CodeManager.Get기초코드(기초코드Type.프로그램종류);
                //cmbDO.ItemsSource = SCodeManager.Select전체시도(true);

                //DB종류
               // cmbDB_GBN.ItemsSource = CodeManager.Get기초코드(기초코드Type.DB종류);
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
                #region 시스템 설정 

                cmbPGM_GBN.SelectedValue = DataManager.ConfigInfo.PGM_GBN;
                txtVOLUME.Text = string.Format("{0}", DataManager.ConfigInfo.VOLUME);
                txtWAIT_TIME.Text = string.Format("{0}", DataManager.ConfigInfo.WAIT_TIME);
                txtRADIUS.Text = string.Format("{0}", DataManager.ConfigInfo.RADIUS);
                txtINIT_X.Text = string.Format("{0}", DataManager.ConfigInfo.INIT_X);
                txtINIT_Y.Text = string.Format("{0}", DataManager.ConfigInfo.INIT_Y);
                txtANIME_DELAY.Text = string.Format("{0}", DataManager.ConfigInfo.ANIME_DELAY);
                txtFONTS_SZ.Text = string.Format("{0}", DataManager.ConfigInfo.FONTS_SZ);
                cmbFONTS_GBN.SelectedValue = DataManager.ConfigInfo.FONTS_NM;

                //chkWEATHER_RSS_YN.IsChecked = DataManager.ConfigInfo.USE_WEATHER_RSS;
                //txtWEATHER_URL.Text = DataManager.ConfigInfo.WEATHER_URL;

                //chkNEWS_RSS_YN.IsChecked = DataManager.ConfigInfo.USE_NEWS_RSS;
                //txtNEWS_URL.Text = DataManager.ConfigInfo.NEWS_URL;

                //chkAIR_PLACE_YN.IsChecked = DataManager.ConfigInfo.USE_AIR_PLACE;
                //txtAIR_PLACE_NM.Text = DataManager.ConfigInfo.AIR_PLACE_NM;

                chkUSE_DEBUG_LOG.IsChecked = DataManager.ConfigInfo.USE_DEBUG_LOG;
                chkUSE_LOCAL_MEDIA.IsChecked = DataManager.ConfigInfo.USE_LOCAL_MEDIAL;

				#endregion

				#region 서버 설정 

				//cmbDB_GBN.SelectedValue = DataManager.ServerInfo.DB_GBN;
    //            txtDB_IP.Text = DataManager.ServerInfo.DB_IP;
    //            txtDB_PORT.Text = string.Format("{0}", DataManager.ServerInfo.DB_PORT);
    //            txtDB_URL.Text = DataManager.ServerInfo.DB_URL;
    //            txtDB_USERID.Text = DataManager.ServerInfo.DB_USERID;
    //            txtDB_PASSWD.Password = DataManager.ServerInfo.DB_PASSWD;

                //txtFTP_IP.Text = DataManager.SetSERVER.FTP_IP;
                //txtFTP_PORT.Text = string.Format("{0}", DataManager.SetSERVER.FTP_PORT);
                //txtFTP_USERID.Text = DataManager.SetSERVER.FTP_USERID;
                //txtFTP_PASSWD.Password = DataManager.SetSERVER.FTP_PASSWD;

                //txtSERVER_IP.Text = DataManager.ServerInfo.SERVER_URL;
                //txtSERVER_PORT.Text = string.Format("{0}", DataManager.ServerInfo.SERVER_PORT);

                #endregion

                //20220729 bha 신규
                chkUSE_REBOOT.IsChecked = DataManager.ConfigInfo.USE_REBOOT;
                txtREBOOT_TIME.Text = DataManager.ConfigInfo.REBOOT_TIME;

                chkUSE_SYNC_TIME.IsChecked = DataManager.ConfigInfo.USE_SYNCTIME;
                txtSYNC_URL.Text = DataManager.ConfigInfo.SYNC_URL;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

   //     private void cmbDO_SelectionChanged(object sender, SelectionChangedEventArgs e)
   //     {
   //         try
			//{
   //             ComboBox cmb = sender as ComboBox;
   //             if (cmb == null) return;

   //             DO_CODE item = cmbDO.SelectedItem as DO_CODE;
   //             if (item == null) return;

   //             cmbSGG.ItemsSource = null;
   //             List<SGG_CODE> items = SCodeManager.Select시군구By시도(item.DO_CD, true);
   //             if (item != null && items.Count > 0)
   //             {
   //                 cmbSGG.ItemsSource = items;
   //                 if (_isLoaded == false) cmbSGG.SelectedValue = DataManager.ConfigInfo.SGG_CD;
   //             }
   //         }
   //         catch (Exception ee)
   //         {
   //             TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
   //             System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
   //         }
   //     }

        bool IS_CHECK_ITEM()
        {
            try
            {
                if (txtANIME_DELAY.Text.Trim().Equals("") == true)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("미입력", "애니메이션 효과 시간을 입력해주세요.");
                    txtANIME_DELAY.Focus();
                    return false;
                }

                if (CommonUtils.IsNumeric(txtANIME_DELAY.Text.Trim()) == false)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("입력오류", "애니메이션 효과 시간은 숫자만 입력할수 있습니다.");
                    txtANIME_DELAY.Text = "500";
                    txtANIME_DELAY.Focus();
                    return false;
                }

                if (txtRADIUS.Text.Trim().Equals("") == true)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("미입력", "반경거리 값을 입력해주세요.");
                    txtRADIUS.Focus();
                    return false;
                }

                if (CommonUtils.IsNumeric(txtRADIUS.Text.Trim()) == false)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("입력오류", "반경거리는 숫자만 입력할수 있습니다.");
                    txtRADIUS.Text = "500";
                    txtRADIUS.Focus();
                    return false;
                }

                if (txtFONTS_SZ.Text.Trim().Equals("") == true)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("미입력", "폰트 사이즈 값을 입력해주세요.");
                    txtFONTS_SZ.Focus();
                    return false;
                }

                if (CommonUtils.IsNumeric(txtFONTS_SZ.Text.Trim()) == false)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("입력오류", "폰트크기는 숫자만 입력할수 있습니다.");
                    txtFONTS_SZ.Text = "12";
                    txtFONTS_SZ.Focus();
                    return false;
                }

                if (cmbFONTS_GBN.SelectedIndex < 0)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("미선택", "폰트를 선택해주세요.");
                    cmbFONTS_GBN.Focus();
                    return false;
                }

                if (txtVOLUME.Text.Trim().Equals("") == true)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("미입력", "볼륨값을 입력해주세요.");
                    txtVOLUME.Focus();
                    return false;
                }

                if (CommonUtils.IsNumeric(txtVOLUME.Text.Trim()) == false)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("입력오류", "볼륨은 숫자만 입력할수 있습니다.");
                    txtVOLUME.Text = "0";
                    txtVOLUME.Focus();
                    return false;
                }

                if (txtWAIT_TIME.Text.Trim().Equals("") == true)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("미입력", "대기시간값을 입력해주세요.");
                    txtWAIT_TIME.Focus();
                    return false;
                }

                if (CommonUtils.IsNumeric(txtWAIT_TIME.Text.Trim()) == false)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("입력오류", "대기시간은 숫자만 입력할수 있습니다.");
                    txtWAIT_TIME.Text = "0";
                    txtWAIT_TIME.Focus();
                    return false;
                }

                if (txtINIT_X.Text.Trim().Equals("") == true)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("미입력", "경도값을 입력해주세요.");
                    txtINIT_X.Focus();
                    return false;
                }

                if (CommonUtils.IsNumeric(txtWAIT_TIME.Text.Trim()) == false)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("입력오류", "경도값은 숫자만 입력할수 있습니다.");
                    txtINIT_X.Text = "126.8201";
                    txtINIT_X.Focus();
                    return false;
                }

                if (txtINIT_Y.Text.Trim().Equals("") == true)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("미입력", "위도값을 입력해주세요.");
                    txtINIT_Y.Focus();
                    return false;
                }

                if (CommonUtils.IsNumeric(txtWAIT_TIME.Text.Trim()) == false)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("입력오류", "위도값은 숫자만 입력할수 있습니다.");
                    txtINIT_Y.Text = "37.713";
                    txtINIT_Y.Focus();
                    return false;
                }

                if ( txtREBOOT_TIME.Text.Equals("") == true )
				{
                    MsgDlgManager.ShowIntervalInformationDlg("미입력", "재부팅시간을 입력해주세요.");
                    txtREBOOT_TIME.Focus();
                    return false;
                }

                if (CommonUtils.IsNumeric(txtREBOOT_TIME.Text.Trim()) == false)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("입력오류", "재부팅시간은 숫자만 입력할수 있습니다.");
                    txtREBOOT_TIME.Text = "0400";
                    txtREBOOT_TIME.Focus();
                    return false;
                }

                if (txtREBOOT_TIME.Text.Length !=4)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("입력오류", "재부팅시간은 HHmm형태로만 입력할수 있습니다.");
                    txtREBOOT_TIME.Text = "0400";
                    txtREBOOT_TIME.Focus();
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

        public delegate void 환경설정변경Handler();
        public event 환경설정변경Handler On설정변경Event;
        bool Save환경설정()
        {
            try
            {
                #region 시스템 설정 

                if (DataManager.SetSSNSFile == null || DataManager.SetSSNSFile.SEQ_NO == 0) return false;

                BC_CODE itemPGM = cmbPGM_GBN.SelectedItem as BC_CODE;
                if (itemPGM == null) return false;

                DataManager.SetSSNSFile.ConfigInfo.PGM_GBN = itemPGM.nCODE;
                //SGG_CODE itemSGG= cmbSGG.SelectedItem as SGG_CODE;
                //if (itemSGG != null)
                //{
                //    row.SGG_CD= itemSGG.SGG_CD;
                //    row.SGG_NM = string.Format("{0} {1}", itemSGG.DO_NM, itemSGG.SGG_NM);
                //}

                DataManager.SetSSNSFile.ConfigInfo.VOLUME = Convert.ToInt32(txtVOLUME.Text);
                DataManager.SetSSNSFile.ConfigInfo.WAIT_TIME = Convert.ToInt32(txtWAIT_TIME.Text);
                DataManager.SetSSNSFile.ConfigInfo.RADIUS = Convert.ToInt32(txtRADIUS.Text);
                DataManager.SetSSNSFile.ConfigInfo.INIT_X = Convert.ToDouble(txtINIT_X.Text);
                DataManager.SetSSNSFile.ConfigInfo.INIT_Y = Convert.ToDouble(txtINIT_Y.Text);
                DataManager.SetSSNSFile.ConfigInfo.ANIME_DELAY = Convert.ToInt32(txtANIME_DELAY.Text);
                BC_CODE item폰트 = cmbFONTS_GBN.SelectedItem as BC_CODE;
                if (item폰트 != null) DataManager.SetSSNSFile.ConfigInfo.FONTS_NM = item폰트.S_NM;
                DataManager.SetSSNSFile.ConfigInfo.FONTS_SZ = Convert.ToInt32(txtFONTS_SZ.Text);
                //DataManager.SetSSNSFile.ConfigInfo.AIR_PLACE_NM = txtAIR_PLACE_NM.Text.Trim();
                //DataManager.SetSSNSFile.ConfigInfo.WEATHER_URL = txtWEATHER_URL.Text.Trim();
                //DataManager.SetSSNSFile.ConfigInfo.NEWS_URL = txtNEWS_URL.Text.Trim();
                //DataManager.SetSSNSFile.ConfigInfo.USE_AIR_PLACE = chkAIR_PLACE_YN.IsChecked.Value;
                //DataManager.SetSSNSFile.ConfigInfo.USE_NEWS_RSS = chkNEWS_RSS_YN.IsChecked.Value;
                //DataManager.SetSSNSFile.ConfigInfo.USE_WEATHER_RSS = chkWEATHER_RSS_YN.IsChecked.Value;
                DataManager.SetSSNSFile.ConfigInfo.USE_DEBUG_LOG = chkUSE_DEBUG_LOG.IsChecked.Value;

                //20220729 BHA
                DataManager.SetSSNSFile.ConfigInfo.USE_REBOOT = chkUSE_REBOOT.IsChecked.Value;
                DataManager.SetSSNSFile.ConfigInfo.REBOOT_TIME = txtREBOOT_TIME.Text.Trim();
                DataManager.SetSSNSFile.ConfigInfo.USE_SYNCTIME = chkUSE_SYNC_TIME.IsChecked.Value;
                DataManager.SetSSNSFile.ConfigInfo.SYNC_URL = txtSYNC_URL.Text.Trim();
				//20220928 BHA 
				DataManager.SetSSNSFile.ConfigInfo.USE_LOCAL_MEDIAL= chkUSE_LOCAL_MEDIA.IsChecked.Value;

				//BC_CODE itemDB = cmbDB_GBN.SelectedItem as BC_CODE;
               // if (itemDB != null) DataManager.SetSSNSFile.ServerInfo.DB_GBN = itemDB.nCODE;
                //DataManager.SetSSNSFile.ServerInfo.DB_IP = txtDB_IP.Text.Trim();
                //DataManager.SetSSNSFile.ServerInfo.DB_PORT = Convert.ToInt32(txtDB_PORT.Text);
                //DataManager.SetSSNSFile.ServerInfo.DB_URL = txtDB_URL.Text.Trim();
                //DataManager.SetSSNSFile.ServerInfo.DB_USERID = CryptionUtils.ENCRYPT_BY_AES256(txtDB_USERID.Text.Trim(), AppConfig.CRYPT_KEY, Encoding.UTF8);
                //DataManager.SetSSNSFile.ServerInfo.DB_PASSWD = CryptionUtils.ENCRYPT_BY_AES256(txtDB_PASSWD.Password.Trim(), AppConfig.CRYPT_KEY, Encoding.UTF8);

                #endregion

                //        }
                //                if (DataManager.SetSSNSFile.DsSSNS.dtConfig.Rows.Count > 0)
                //            {
                //                SSData.Config.dsSSNS.dtConfigRow row = DataManager.SetSSNSFile.DsSSNS.dtConfig.Rows[0] as SSData.Config.dsSSNS.dtConfigRow;



                //                if (DataManager.SetSSNSFile.ModifyConfigInfo(row) == false) return false;

                //                //DataManager.ConfigInfo.PGM_GBN = row.PGM_GBN;
                //                DataManager.ConfigInfo.SGG_NM = row.SGG_NM;
                //                DataManager.ConfigInfo.SGG_CD = row.SGG_CD;
                //                DataManager.ConfigInfo.VOLUME = row.VOLUME;
                //                DataManager.ConfigInfo.WAIT_TIME = row.WAIT_TIME;
                //                DataManager.ConfigInfo.WEATHER_URL = row.WEATHER_URL;
                //                DataManager.ConfigInfo.NEWS_URL = row.NEWS_URL;
                //                DataManager.ConfigInfo.RADIUS = row.RADIUS;
                //                DataManager.ConfigInfo.INIT_X = row.INIT_X;
                //                DataManager.ConfigInfo.INIT_Y = row.INIT_Y;
                //                DataManager.ConfigInfo.ANIME_DELAY = row.ANIME_DELAY;
                //                DataManager.ConfigInfo.FONTS_NM = row.FONTS_NM;
                //                DataManager.ConfigInfo.FONTS_SZ = row.FONTS_SZ;
                //                DataManager.ConfigInfo.AIR_PLACE_NM = row.AIR_PLACE_NM;
                //                DataManager.ConfigInfo.USE_WEATHER_RSS = row.USE_WEATHER_RSS;
                //                DataManager.ConfigInfo.USE_NEWS_RSS = row.USE_NEWS_RSS;
                //                DataManager.ConfigInfo.USE_AIR_PLACE = row.USE_AIR_PLACE;
                //            }

                //#endregion

                //#region 서버 설정 

                ////bool UpdateYN = false;
                ////TB_CONFIG item서버 = new TB_CONFIG();
                ////if (DataManager.ServerInfo != null && DataManager.ServerInfo.SEQ_NO > 0) //Update
                ////{
                ////    UpdateYN = true;
                ////    item서버.SEQ_NO = DataManager.ServerInfo.SEQ_NO;
                ////}

                ////BC_CODE itemDB = cmbDB_GBN.SelectedItem as BC_CODE;
                ////if (itemDB != null) item서버.DB_GBN = itemDB.nCODE;
                ////item서버.DB_IP = txtDB_IP.Text.Trim();
                ////item서버.DB_PORT = Convert.ToInt32(txtDB_PORT.Text);
                ////item서버.DB_URL = txtDB_URL.Text.Trim();// CryptionUtils.ENCRYPT_BY_AES256(txtDB_URL.Text.Trim(), AppConfig.CRYPT_KEY, Encoding.UTF8);
                ////item서버.DB_USERID = txtDB_USERID.Text.Trim();//CryptionUtils.ENCRYPT_BY_AES256(txtDB_USERID.Text.Trim(), AppConfig.CRYPT_KEY, Encoding.UTF8);
                ////item서버.DB_USERPWD = txtDB_PASSWD.Password.Trim();//CryptionUtils.ENCRYPT_BY_AES256(txtDB_PASSWD.Password.Trim(), AppConfig.CRYPT_KEY, Encoding.UTF8);


                ////DataManager.ServerInfo.DB_GBN = item서버.DB_GBN;
                ////DataManager.ServerInfo.DB_IP = item서버.DB_IP;
                ////DataManager.ServerInfo.DB_PORT = item서버.DB_PORT;
                ////DataManager.ServerInfo.DB_USERID = item서버.DB_USERID;
                ////DataManager.ServerInfo.DB_PASSWD = item서버.DB_USERPWD;
                ////DataManager.ServerInfo.DB_URL = item서버.DB_URL;

                ////bool 결과YN = false;
                ////if (UpdateYN == false)
                ////{

                ////    결과YN = DatabaseManager.INSERT_TB_CONFIG(item서버);
                ////}
                ////else
                ////{
                ////    결과YN = DatabaseManager.UPDATE_TB_CONFIG(item서버);
                ////}
                //if (DataManager.SetSSNSFile.DsSSNS.dtServer.Rows.Count > 0)
                //{
                //	SSData.Config.dsSSNS.dtServerRow row = DataManager.SetSSNSFile.DsSSNS.dtServer.Rows[0] as SSData.Config.dsSSNS.dtServerRow;

                //	BC_CODE itemDB = cmbDB_GBN.SelectedItem as BC_CODE;
                //	if (itemDB != null) row.DB_GBN = itemDB.nCODE;
                //	row.DB_IP = txtDB_IP.Text.Trim();
                //	row.DB_PORT = Convert.ToInt32(txtDB_PORT.Text);
                //                row.DB_URL = txtDB_URL.Text.Trim();
                //	row.DB_USERID = CryptionUtils.ENCRYPT_BY_AES256(txtDB_USERID.Text.Trim(), AppConfig.CRYPT_KEY, Encoding.UTF8);
                //	row.DB_PASSWD = CryptionUtils.ENCRYPT_BY_AES256(txtDB_PASSWD.Password.Trim(), AppConfig.CRYPT_KEY, Encoding.UTF8);

                //	//row.FTP_IP = txtFTP_IP.Text.Trim();
                //	//row.FTP_PORT = Convert.ToInt32(txtFTP_PORT.Text);
                //	//row.FTP_USERID = CryptionUtils.ENCRYPT_BY_AES256(txtFTP_USERID.Text.Trim(), AppConfig.CRYPT_KEY, Encoding.UTF8);
                //	//row.FTP_PASSWD = CryptionUtils.ENCRYPT_BY_AES256(txtFTP_PASSWD.Password.Trim(), AppConfig.CRYPT_KEY, Encoding.UTF8);

                //	//row.SERVER_URL = txtSERVER_IP.Text.Trim();
                //	//row.SERVER_PORT = Convert.ToInt32(txtSERVER_PORT.Text);

                //	if (DataManager.SetSSNSFile.ModifyServerInfo(row) == false) return false;

                //	DataManager.ServerInfo.DB_GBN = row.DB_GBN;
                //	DataManager.ServerInfo.DB_IP = row.DB_IP;
                //	DataManager.ServerInfo.DB_PORT = row.DB_PORT;
                //                DataManager.ServerInfo.DB_URL = row.DB_URL;
                //	DataManager.ServerInfo.DB_USERID = CryptionUtils.DECRYPT_BY_AES256(row.DB_USERID, AppConfig.CRYPT_KEY, Encoding.UTF8);
                //	DataManager.ServerInfo.DB_PASSWD = CryptionUtils.DECRYPT_BY_AES256(row.DB_PASSWD, AppConfig.CRYPT_KEY, Encoding.UTF8);

                //	//DataManager.SetSERVER.FTP_IP = row.FTP_IP;
                //	//DataManager.SetSERVER.FTP_PORT = row.FTP_PORT;
                //	//DataManager.SetSERVER.FTP_USERID = CryptionUtils.DECRYPT_BY_AES256(row.FTP_USERID, AppConfig.CRYPT_KEY, Encoding.UTF8);
                //	//DataManager.SetSERVER.FTP_PASSWD = CryptionUtils.DECRYPT_BY_AES256(row.FTP_PASSWD, AppConfig.CRYPT_KEY, Encoding.UTF8);

                //	//DataManager.SetSERVER.SERVER_URL = row.SERVER_URL;
                //	//DataManager.SetSERVER.SERVER_PORT = row.SERVER_PORT;
                //}

                //#endregion

                ////if (결과YN == false) return false;

                return DataManager.SetSSNSFile.Save(true);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

		private void btn저장_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                if (IS_CHECK_ITEM() == false) return;

                bool 진행YN = MsgDlgManager.ShowQuestionDlg("저장 확인", "설정한 환경 정보를 저장하시겠습니까?");
                if (진행YN == false) return;

                if (Save환경설정() == false)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("환경설정 저장실패", "환경설정 저장작업중 에러가 발생했습니다.");
                    return;
                }

                DataManager.ConfigInfo = DataManager.SetSSNSFile.ConfigInfo;
                DataManager.ServerInfo = DataManager.SetSSNSFile.ServerInfo;


                if (On설정변경Event != null) On설정변경Event();
                //ShareMemoryManager.ShareMemoryEvent("CONFIG");

                this.Close();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }
	}
    
}

