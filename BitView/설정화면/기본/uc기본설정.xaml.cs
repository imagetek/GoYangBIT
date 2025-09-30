using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SSCommonNET;
using SSControlsNET;
using SSData;
using SSData.Utils;

namespace BitView
{
	/// <summary>
	/// ucENV2Panel.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class uc기본설정 : UserControl
	{
		public uc기본설정()
		{
			InitializeComponent();
			this.FontFamily = (FontFamily)FindResource(DataManager.ConfigInfo.FONTS_NM);
			this.FontSize = DataManager.ConfigInfo.FONTS_SZ;
		}

		Window _p = null;
		public void SetParentWindow(Window p)
		{
			_p = p;
		}

		bool _isLoaded = false;
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				if (_isLoaded == false)
				{
					InitProc();

					Load기본값();
				}

				Load설정값();
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

		private void InitProc()
		{
			try
			{
				cmbENV2_PORT.Items.Clear();

				//cmbFTP_GBN.ItemsSource = null;
				//cmbFTP_GBN.DisplayMemberPath = "NAME";
				//cmbFTP_GBN.SelectedValuePath = "nCODE";

				cmbENV_GBN.ItemsSource = null;
				cmbENV_GBN.DisplayMemberPath = "NAME";
				cmbENV_GBN.SelectedValuePath = "nCODE";

				
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			finally
			{
				this.IsEnabled = true;
			}
		}

		private void Load기본값()
		{
			try
			{
				cmbENV2_PORT.Items.Add("미사용");
				string[] sPort = System.IO.Ports.SerialPort.GetPortNames();
				foreach (string port in sPort) cmbENV2_PORT.Items.Add(port);

				//DsDevice[] device = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
				////cmbCAM_NO1.Items.Clear();
				////cmbCAM_NO2.Items.Clear();
				//int idx = 0;
				//foreach (DsDevice dev in device)
				//{
				//	string dispNM = string.Format("[{0}] {1}", idx, dev.Name);
				//	//cmbCAM_NO1.Items.Add(dispNM);
				//	//cmbCAM_NO2.Items.Add(dispNM);
				//	idx++;
				//}

				////cmbCAM_NO1_ROTATE.Items.Clear();
				////cmbCAM_NO2_ROTATE.Items.Clear();

				//for (int i = 0; i < 360; i += 90)
				//{
				//	//cmbCAM_NO1_ROTATE.Items.Add(string.Format("{0}", i));
				//	//cmbCAM_NO2_ROTATE.Items.Add(string.Format("{0}", i));
				//}

				////cmbCAM_NO1_ROTATE.SelectedIndex = 0;
				////cmbCAM_NO2_ROTATE.SelectedIndex = 0;

				cmbENV_GBN.ItemsSource = CodeManager.Get기초코드(기초코드Type.환경보드종류);
				//cmbFTP_GBN.ItemsSource = CodeManager.Get기초코드(기초코드Type.FTP구분);

				rbST1.IsChecked = true;
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
				var config = BITDataManager.BitSystem;
				if (config == null) return;

				txtBIT_ID.Text = string.Format("{0}", config.BIT_ID);
				txtMOBILE_NO.Text = string.Format("{0}", config.MOBILE_NO);
				txtSTATION_NM.Text = config.STATION_NM;
				stationId.Text = config.STATION_ID;

				if (config.ENV_GBN >= 0) cmbENV_GBN.SelectedValue = config.ENV_GBN;

				if (config.ENV_PORT_NM!= null && config.ENV_PORT_NM.Equals("") == false) cmbENV2_PORT.SelectedValue = config.ENV_PORT_NM;
				txtENV_BAUD_RATE.Text = string.Format("{0}", config.ENV_BAUD_RATE);
				
				//string WEBCAM = config.WEBCAM_NM;
				//if (WEBCAM != null && WEBCAM.Equals("") == false) cmbWEBCAM.SelectedIndex = config.WEBCAM_NO;
				//if (config.CAM_NO1 >=0) cmbCAM_NO1.SelectedIndex = config.CAM_NO1;
				//cmbCAM_NO1_ROTATE.SelectedIndex = config.CAM_NO1_ROTATE;
				//if (config.CAM_NO2>= 0) cmbCAM_NO2.SelectedIndex = config.CAM_NO2;
				//cmbCAM_NO2_ROTATE.SelectedIndex = config.CAM_NO2_ROTATE;
				//string SHOCK = config.SHOCKCAM_NM;
				//if (SHOCK != null && SHOCK.Equals("") == false) cmbSHOCKCAM.SelectedIndex = config.SHOCKCAM_NO;

				//서버
				txtSERVER_IPLIST.Text = config.SERVER_URL;
				txtSERVER_PORTLIST.Text = config.SERVER_PORT;

				//HTTP
				txtHTTP_IP.Text = config.HTTP_URL;
				txtHTTP_PORT.Text = string.Format("{0}", config.HTTP_PORT);

				//FTP
				//cmbFTP_GBN.SelectedValue = config.FTP_GBN;
				//txtFTP_IP.Text = config.FTP_IP;
				//txtFTP_PORT.Text = string.Format("{0}", config.FTP_PORT);
				//txtFTP_ID.Text = config.FTP_USERID;
				//txtFTP_PWD.Password = config.FTP_USERPWD;

				//20221017 bha
				switch (config.SERVER_TYPE)
				{
					case 0: rbST0.IsChecked = true; break;
					default: rbST1.IsChecked = true; break;
				}
				//edgar get vpn ip if possible
				//txtDEVICE_IP.Text = JSON.JsonService.GetVpnIp();

			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
		
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			try
			{
				//DoFinal();
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
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		bool IS_CHECK_ITEM()
		{
			try
			{
				if (string.IsNullOrWhiteSpace(txtBIT_ID.Text))
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

				if (string.IsNullOrWhiteSpace(txtMOBILE_NO.Text))
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

				if (string.IsNullOrWhiteSpace(txtSTATION_NM.Text))
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

				//if (cmbENV2_PORT.SelectedIndex < 0)
				//{
				//	MsgDlgManager.ShowIntervalInformationDlg("미선택", "환경보드를 선택해주세요.");
				//	cmbENV2_PORT.Focus();
				//	return false;
				//}

				if (string.IsNullOrWhiteSpace(txtENV_BAUD_RATE.Text))
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
								
				//if (cmbCAM_NO1.SelectedIndex < 0)
				//{
				//	MsgDlgManager.ShowIntervalInformationDlg("미선택", "1번카메라 선택해주세요.");
				//	cmbCAM_NO1.Focus();
				//	return false;
				//}

				//if (cmbCAM_NO2.SelectedIndex < 0)
				//{
				//	MsgDlgManager.ShowIntervalInformationDlg("미선택", "2번카메라 선택해주세요.");
				//	cmbCAM_NO2.Focus();
				//	return false;
				//}

				if (string.IsNullOrWhiteSpace(txtSERVER_IPLIST.Text))
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "서버주소를 입력해주세요.");
					txtSERVER_IPLIST.Focus();
					return false;
				}

				if (string.IsNullOrWhiteSpace(txtSERVER_PORTLIST.Text))
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

				if (string.IsNullOrWhiteSpace(txtHTTP_IP.Text))
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "HTTP주소를 입력해주세요.");
					txtHTTP_IP.Focus();
					return false;
				}

				if (string.IsNullOrWhiteSpace(txtHTTP_PORT.Text))
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

				//edgar FTP
				//if (string.IsNullOrWhiteSpace(txtFTP_IP.Text))
				//{
				//	MsgDlgManager.ShowIntervalInformationDlg("미입력", "FTP주소를 입력해주세요.");
				//	txtFTP_IP.Focus();
				//	return false;
				//}

				//if (string.IsNullOrWhiteSpace(txtFTP_PORT.Text))
				//{
				//	MsgDlgManager.ShowIntervalInformationDlg("미입력", "FTP포트를 입력해주세요.");
				//	txtFTP_PORT.Focus();
				//	return false;
				//}

				//if (CommonUtils.IsNumeric(txtFTP_PORT.Text.Trim()) == false)
				//{
				//	MsgDlgManager.ShowIntervalInformationDlg("입력오류", "FTP포트는 숫자로 입력해주세요.");
				//	txtFTP_PORT.Text = "";
				//	txtFTP_PORT.Focus();
				//	return false;
				//}

				//if (string.IsNullOrWhiteSpace(txtFTP_ID.Text))
				//{
				//	MsgDlgManager.ShowIntervalInformationDlg("미입력", "FTP 사용자ID를 입력해주세요.");
				//	txtFTP_ID.Focus();
				//	return false;
				//}

				//if (string.IsNullOrWhiteSpace(txtFTP_PWD.Password))
				//{
				//	MsgDlgManager.ShowIntervalInformationDlg("미입력", "FTP 비밀번호를 입력해주세요.");
				//	txtFTP_PWD.Focus();
				//	return false;
				//}


				return true;
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
		}

		private void btn설정저장_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (IS_CHECK_ITEM() == false) return;

				bool 진행YN = MsgDlgManager.ShowQuestionDlg("저장 확인", "설정한 환경 정보를 저장하시겠습니까?");

				if (진행YN == false) return;

				if (Save설정값() == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("기본설정 저장실패", "기본설정 저장작업중 에러가 발생했습니다.");
					return;
				}

				BITDataManager.Refresh설정값(1);
				MsgDlgManager.ShowIntervalInformationDlg("기본설정 저장", "기본설정이 저장되었습니다.");
			}
			catch (Exception ee)
			{
				LogWriterService.WriteToLog(ee);
			}
		}

		private bool Save설정값()
		{
			try
			{
				bool UpdateYN = false;
				BIT_SYSTEM item설정 = SettingsManager.GetBitSystemSettings();
				//if (BITDataManager.BitSystem != null && BITDataManager.BitSystem.SEQ_NO > 0) //Update
				//{
				//	UpdateYN = true;
				//	item설정.SEQ_NO = BITDataManager.BitSystem.SEQ_NO;
				//}

				item설정.BIT_ID = txtBIT_ID.Text;
				item설정.MOBILE_NO = Convert.ToInt32(txtMOBILE_NO.Text);
				item설정.STATION_NM = txtSTATION_NM.Text.Trim();
				item설정.STATION_ID = stationId.Text;

				item설정.ENV_GBN = 0;
				if (cmbENV2_PORT.SelectedIndex >= 0)
				{
					BC_CODE itemENV = cmbENV_GBN.SelectedItem as BC_CODE;
					if (itemENV != null) item설정.ENV_GBN = itemENV.nCODE;
				}

				item설정.ENV_PORT_NM = "";
				if (cmbENV2_PORT.SelectedIndex >= 0) item설정.ENV_PORT_NM = cmbENV2_PORT.SelectedValue.ToString();
				item설정.ENV_BAUD_RATE = Convert.ToInt32(txtENV_BAUD_RATE.Text);
				//item설정.WEBCAM_NM = "";
				//if (cmbWEBCAM.SelectedIndex >= 0) item설정.WEBCAM_NM = cmbWEBCAM.SelectedValue.ToString();
				//item설정.SHOCKCAM_NM = "";
				//if (cmbSHOCKCAM.SelectedIndex >= 0) item설정.SHOCKCAM_NM = cmbSHOCKCAM.SelectedValue.ToString();
				//item설정.CAM_NO1 = -1;
				//if (cmbCAM_NO1.SelectedIndex >= 0) item설정.CAM_NO1 = cmbCAM_NO1.SelectedIndex;
				//item설정.CAM_NO1_ROTATE = cmbCAM_NO1_ROTATE.SelectedIndex;

				//item설정.CAM_NO2= -1;
				//if (cmbCAM_NO2.SelectedIndex >= 0) item설정.CAM_NO2= cmbCAM_NO2.SelectedIndex;
				//item설정.CAM_NO2_ROTATE = cmbCAM_NO2_ROTATE.SelectedIndex;

				item설정.SERVER_URL = txtSERVER_IPLIST.Text.Trim();

				//20221017 bha
				int SVR_TYPE = 1;
				if (rbST0.IsChecked.Value == true) SVR_TYPE = 0;
				item설정.SERVER_TYPE = SVR_TYPE;

				item설정.SERVER_PORT = txtSERVER_PORTLIST.Text.Trim();

				//edgar FTP
				//BC_CODE itemFTP = cmbFTP_GBN.SelectedItem as BC_CODE;
				//if (itemFTP != null) item설정.FTP_GBN = itemFTP.nCODE;
				//item설정.FTP_IP = txtFTP_IP.Text.Trim();
				//item설정.FTP_PORT = Convert.ToInt32(txtFTP_PORT.Text);
				//item설정.FTP_USERID = txtFTP_ID.Text.Trim();
				//item설정.FTP_USERPWD = txtFTP_PWD.Password.Trim();

				item설정.HTTP_URL = txtHTTP_IP.Text.Trim();
				item설정.HTTP_PORT = Convert.ToInt32(txtHTTP_PORT.Text);

				//bool 결과YN = false;
				//if (UpdateYN == false)
				//{
				//	결과YN = DatabaseManager.INSERT_BIT_SYSTEM(item설정);
				//}
				//else
				//{
				//	결과YN = DatabaseManager.UPDATE_BIT_SYSTEM(item설정);
				//}

				//edgar saving device_ip to local settings file
				//JSON.Settings settings = JSON.JsonService.GetJsonSettings();
				//settings.device_ip = txtDEVICE_IP.Text.Trim();
				//JSON.JsonService.WriteToJsonSettings(settings);

				return SettingsManager.SaveBitSystemSettings(item설정);
			}
			catch (Exception ee)
			{
				LogWriterService.WriteToLog(ee);
				return false;
			}
		}

		private void btn설정삭제_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				//DoFinal();
			}
			catch (Exception ee)
			{
				LogWriterService.WriteToLog(ee);
			}
		}
	}
}
