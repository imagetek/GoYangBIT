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
    public partial class wndBIT설정 : Window
    {
        public wndBIT설정()
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
                //cmbDISP_GBN.ItemsSource = null;
                //cmbDISP_GBN.DisplayMemberPath = "NAME";
                //cmbDISP_GBN.SelectedValuePath = "nCODE";

                cmbENV2_PORT.Items.Clear();

                cmbFTP_GBN.ItemsSource = null;
                cmbFTP_GBN.DisplayMemberPath = "NAME";
                cmbFTP_GBN.SelectedValuePath = "nCODE";
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
                cmbENV2_PORT.Items.Add("미사용");
				string[] sPort = System.IO.Ports.SerialPort.GetPortNames();
                foreach (string port in sPort) cmbENV2_PORT.Items.Add(port);

                DsDevice[] device = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
                cmbWEBCAM.Items.Clear();
                cmbWEBCAM.Items.Add("미사용");
                foreach (DsDevice dev in device) cmbWEBCAM.Items.Add(dev.Name);

                cmbSHOCKCAM.Items.Clear();
                cmbSHOCKCAM.Items.Add("미사용");
                foreach (DsDevice dev in device) cmbSHOCKCAM.Items.Add(dev.Name);

                //cmbDISP_GBN.ItemsSource = CodeManager.Get기초코드(기초코드Type.DISPLAY구분);
                
                cmbFTP_GBN.ItemsSource = CodeManager.Get기초코드(기초코드Type.FTP구분);
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
                //var config = BITDataManager.BitSystem;
                //txtBIT_ID.Text = string.Format("{0}", config.BIT_ID);
                //txtMOBILE_NO.Text = string.Format("{0}", config.MOBILE_NO); 
                //txtSTATION_NM.Text = config.STATION_NM;
                ////cmbDISP_GBN.SelectedValue = config.DISP_GBN;

                ////환경보드
                //if (config.ENV_PORT_NM.Equals("") == false)
                //{
                //    cmbENV2_PORT.SelectedValue = config.ENV_PORT_NM;
                //}
                //txtENV_BAUD_RATE.Text = string.Format("{0}", config.ENV_BAUD_RATE);
                ////txtENV_REFRES_TIME.Text = string.Format("{0}", config.REFRESH_TIME);

                //string WEBCAM = config.WEBCAM_NM;
                //if (WEBCAM != null && WEBCAM.Equals("") == false) cmbWEBCAM.SelectedValue = config.WEBCAM_NM;

                //string SHOCK = config.SHOCKCAM_NM;
                //if (SHOCK != null && SHOCK.Equals("") == false) cmbSHOCKCAM.SelectedValue = config.SHOCKCAM_NM;

                ////서버
                ////txtSERVER_IP.Text = string.Join("|", configS.Select(data => data.SERVER_URL));
                ////txtSERVER_PORT.Text = string.Join("|", configS.Select(data => data.SERVER_PORT));
                //txtSERVER_IPLIST.Text = config.SERVER_URL;
                //txtSERVER_PORTLIST.Text= config.SERVER_PORT;

                ////HTTP
                //txtHTTP_IP.Text = config.HTTP_URL;
                //txtHTTP_PORT.Text = string.Format("{0}", config.HTTP_PORT);

                ////FTP
                //cmbFTP_GBN.SelectedValue = config.FTP_GBN;
                //txtFTP_IP.Text = config.FTP_IP;
                //txtFTP_PORT.Text = string.Format("{0}", config.FTP_PORT);
                //txtFTP_ID.Text = config.FTP_USERID;
                //txtFTP_PWD.Password = config.FTP_USERPWD;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public delegate void 환경설정변경Handler();
        public event 환경설정변경Handler On설정변경Event;
        private void btn저장_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IS_CHECK_ITEM() == false) return;

                bool 진행YN = MsgDlgManager.ShowQuestionDlg("저장 확인", "설정한 환경 정보를 저장하시겠습니까?");

                if (진행YN == false) return;

                if (Save설정값() == false)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("환경설정 저장실패", "환경설정 저장작업중 에러가 발생했습니다.");
                    return;
                }

                if (On설정변경Event != null) On설정변경Event();
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
				if (txtBIT_ID.Text.Trim().Equals("") == true)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "BIT ID를 입력해주세요.");
					txtBIT_ID.Focus();
					return false;
				}

				if (CommonUtils.IsNumeric(txtBIT_ID.Text.Trim()) == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("입력오류", "BIT ID는 숫자로 입력해주세요.");
					txtBIT_ID.Text = "0";
					txtBIT_ID.Focus();
					return false;
				}

                if (txtMOBILE_NO.Text.Trim().Equals("") == true)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("미입력", "모바일번호를 입력해주세요.");
                    txtMOBILE_NO.Focus();
                    return false;
                }

                if (CommonUtils.IsNumeric(txtBIT_ID.Text.Trim()) == false)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("입력오류", "모바일번호는 숫자로 입력해주세요.");
                    txtMOBILE_NO.Text = "0";
                    txtMOBILE_NO.Focus();
                    return false;
                }

                if (txtSTATION_NM.Text.Trim().Equals("") == true)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "정류장명을 입력해주세요.");
					txtSTATION_NM.Focus();
					return false;
				}

    //            if ( cmbDISP_GBN.SelectedIndex < 0)
				//{
    //                MsgDlgManager.ShowIntervalInformationDlg("미선택", "디스플레이를 선택해주세요.");
    //                cmbDISP_GBN.Focus();
    //                return false;
    //            }

                if (cmbENV2_PORT.SelectedIndex < 0)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("미선택", "환경보드를 선택해주세요.");
                    cmbENV2_PORT.Focus();
                    return false;
                }

                if (txtENV_BAUD_RATE.Text.Trim().Equals("") == true)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("미입력", "환경보드 통신속도를 입력해주세요.");
                    txtENV_BAUD_RATE.Focus();
                    return false;
                }

                if (CommonUtils.IsNumeric(txtENV_BAUD_RATE.Text.Trim()) == false)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("입력오류", "환경보드 통신속도는 숫자로 입력해주세요.");
                    txtENV_BAUD_RATE.Text = "57600";
                    txtENV_BAUD_RATE.Focus();
                    return false;
                }

                //if (txtENV_REFRES_TIME.Text.Trim().Equals("") == true)
                //{
                //    MsgDlgManager.ShowIntervalInformationDlg("미입력", "환경보드 업데이트간격을 입력해주세요.");
                //    txtENV_REFRES_TIME.Focus();
                //    return false;
                //}

                //if (CommonUtils.IsNumeric(txtENV_REFRES_TIME.Text.Trim()) == false)
                //{
                //    MsgDlgManager.ShowIntervalInformationDlg("입력오류", "환경보드 업데이트간격는 숫자로 입력해주세요.");
                //    txtENV_REFRES_TIME.Text = "5000";
                //    txtENV_REFRES_TIME.Focus();
                //    return false;
                //}

                if (cmbWEBCAM.SelectedIndex < 0)
                {
                    MsgDlgManager.ShowIntervalInformationDlg("미선택", "웹캠을 선택해주세요.");
                    cmbWEBCAM.Focus();
                    return false;
                }

                if (txtSERVER_IPLIST.Text.Trim().Equals("") == true)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "서버주소를 입력해주세요.");
                    txtSERVER_IPLIST.Focus();
					return false;
				}

				if (txtSERVER_PORTLIST.Text.Trim().Equals("") == true)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "서버포트를 입력해주세요.");
                    txtSERVER_PORTLIST.Focus();
					return false;
				}

                string[] portlist = txtSERVER_PORTLIST.Text.Trim().Split('|');
                foreach (string port in portlist)
                {
                    if (CommonUtils.IsNumeric(port) == false)
                    {
                        MsgDlgManager.ShowIntervalInformationDlg("입력오류", "입력한 포트중 숫자가 아닌값이 존재합니다.");
                        txtSERVER_PORTLIST.Text = "";
                        txtSERVER_PORTLIST.Focus();
                        return false;
                    }
                }

                if (txtHTTP_IP.Text.Trim().Equals("") == true)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "HTTP주소를 입력해주세요.");
					txtHTTP_IP.Focus();
					return false;
				}

				if (txtHTTP_PORT.Text.Trim().Equals("") == true)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "HTTP포트를 입력해주세요.");
					txtHTTP_PORT.Focus();
					return false;
				}

				if (CommonUtils.IsNumeric(txtHTTP_PORT.Text.Trim()) == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("입력오류", "HTTP포트는 숫자로 입력해주세요.");
					txtHTTP_PORT.Text = "";
					txtHTTP_PORT.Focus();
					return false;
				}

				if (txtFTP_IP.Text.Trim().Equals("") == true)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "FTP주소를 입력해주세요.");
					txtFTP_IP.Focus();
					return false;
				}

				if (txtFTP_PORT.Text.Trim().Equals("") == true)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "FTP포트를 입력해주세요.");
					txtFTP_PORT.Focus();
					return false;
				}

				if (CommonUtils.IsNumeric(txtFTP_PORT.Text.Trim()) == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("입력오류", "FTP포트는 숫자로 입력해주세요.");
					txtFTP_PORT.Text = "";
					txtFTP_PORT.Focus();
					return false;
				}

				if (txtFTP_ID.Text.Trim().Equals("") == true)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "FTP 사용자ID를 입력해주세요.");
					txtFTP_ID.Focus();
					return false;
				}

				if (txtFTP_PWD.Password.Trim().Equals("") == true)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "FTP 비밀번호를 입력해주세요.");
					txtFTP_PWD.Focus();
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

        private bool Save설정값()
        {
            try
            {
                bool UpdateYN = false;
                BIT_SYSTEM item설정 = new BIT_SYSTEM();
                if (BITDataManager.BitSystem != null && BITDataManager.BitSystem.SEQ_NO > 0) //Update
                {
                    UpdateYN = true;
                    item설정.SEQ_NO = BITDataManager.BitSystem.SEQ_NO;
                }

                //item설정.BIT_ID = Convert.ToInt32(txtBIT_ID.Text);
                //item설정.MOBILE_NO = Convert.ToInt32(txtMOBILE_NO.Text);
                //item설정.STATION_NM = txtSTATION_NM.Text.Trim();

                //item설정.ENV_PORT_NM = "";
                //if (cmbENV2_PORT.SelectedIndex >= 0) item설정.ENV_PORT_NM = cmbENV2_PORT.SelectedValue.ToString();
                //item설정.ENV_BAUD_RATE = Convert.ToInt32(txtENV_BAUD_RATE.Text);
                //item설정.WEBCAM_NM = "";
                //if (cmbWEBCAM.SelectedIndex >= 0) item설정.WEBCAM_NM = cmbWEBCAM.SelectedValue.ToString();
                //item설정.SHOCKCAM_NM = "";
                //if (cmbSHOCKCAM.SelectedIndex >= 0) item설정.SHOCKCAM_NM = cmbSHOCKCAM.SelectedValue.ToString();

                //item설정.SERVER_URL = txtSERVER_IPLIST.Text.Trim();
                //item설정.SERVER_PORT = txtSERVER_PORTLIST.Text.Trim();

                //BC_CODE itemFTP = cmbFTP_GBN.SelectedItem as BC_CODE;
                //if (itemFTP != null) item설정.FTP_GBN = itemFTP.nCODE;
                //item설정.FTP_IP = txtFTP_IP.Text.Trim();
                //item설정.FTP_PORT = Convert.ToInt32(txtFTP_PORT.Text);
                //item설정.FTP_USERID = txtFTP_ID.Text.Trim();
                //item설정.FTP_USERPWD = txtFTP_PWD.Password.Trim();

                //item설정.HTTP_URL = txtHTTP_IP.Text.Trim();
                //item설정.HTTP_PORT = Convert.ToInt32(txtHTTP_PORT.Text);

                bool 결과YN = false;
                if (UpdateYN == false)
                {
                    결과YN = DatabaseManager.INSERT_BIT_SYSTEM(item설정);
                }
                else
                {
                    결과YN = DatabaseManager.UPDATE_BIT_SYSTEM(item설정);
                }

                return 결과YN;

    //            BITDataManager.BitBasic.SEQ_NO++;

				//config.BIT_ID = Convert.ToInt32(txtBIT_ID.Text);
				//config.MOBILE_NO= txtMOBILE_NO.Text.Trim();
				//config.STATION_NM = txtSTATION_NM.Text.Trim();
    //            //BC_CODE item화면 = cmbDISP_GBN.SelectedValue as BC_CODE;
    //            //if (item화면 != null) config.DISP_GBN = item화면.nCODE;

    //            config.PORT_NM = "";
    //            if (cmbENV2_PORT.SelectedIndex >= 0) config.PORT_NM = cmbENV2_PORT.SelectedValue.ToString();
    //            config.BAUD_RATE = Convert.ToInt32(txtENV_BAUD_RATE.Text);
    //            //config.REFRESH_TIME= Convert.ToInt32(txtENV_REFRES_TIME.Text);                
    //            config.WEBCAM_NM = "";
    //            if (cmbWEBCAM.SelectedIndex >= 0) config.WEBCAM_NM = cmbWEBCAM.SelectedValue.ToString();
    //            config.SHOCKCAM_NM = "";
    //            if (cmbSHOCKCAM.SelectedIndex >= 0) config.SHOCKCAM_NM = cmbSHOCKCAM.SelectedValue.ToString();

    //            config.SERVER_URL_LIST= txtSERVER_IPLIST.Text.Trim();
				//config.SERVER_PORT_LIST= txtSERVER_PORTLIST.Text.Trim();

    //            BC_CODE itemFTP= cmbFTP_GBN.SelectedValue as BC_CODE;
    //            if (itemFTP != null) config.FTP_GBN= itemFTP.nCODE;
    //            config.FTP_IP = txtFTP_IP.Text.Trim();
    //            config.FTP_PORT = Convert.ToInt32(txtFTP_PORT.Text);
    //            config.FTP_USERID= txtFTP_ID.Text.Trim();
    //            config.FTP_PASSWD= txtFTP_PWD.Password.Trim();

				//config.HTTP_URL= txtHTTP_IP.Text.Trim();
				//config.HTTP_PORT = Convert.ToInt32(txtHTTP_PORT.Text);

    //            return BITDataManager.BitBasic.Save(true);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

	}
}


