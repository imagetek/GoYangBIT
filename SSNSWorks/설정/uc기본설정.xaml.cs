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
using SSControlsNET;
using SSData;

using DirectShowLib;

namespace SSNSWorks
{
	/// <summary>
	/// ucENV2Panel.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class uc기본설정 : UserControl
	{
		public uc기본설정()
		{
			InitializeComponent();
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

				cmbFTP_GBN.ItemsSource = null;
				cmbFTP_GBN.DisplayMemberPath = "NAME";
				cmbFTP_GBN.SelectedValuePath = "nCODE";				
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

				DsDevice[] device = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
				cmbCAM_NO1.Items.Clear();
				cmbCAM_NO2.Items.Clear();
				int idx = 0;
				foreach (DsDevice dev in device)
				{
					string dispNM = string.Format("[{0}] {1}", idx, dev.Name);
					cmbCAM_NO1.Items.Add(dispNM);
					cmbCAM_NO2.Items.Add(dispNM);
					idx++;
				}

				cmbCAM_NO1_ROTATE.Items.Clear();
				cmbCAM_NO2_ROTATE.Items.Clear();

				for (int i = 0; i < 360; i += 90)
				{
					cmbCAM_NO1_ROTATE.Items.Add(string.Format("{0}", i));
					cmbCAM_NO2_ROTATE.Items.Add(string.Format("{0}", i));
				}

				cmbCAM_NO1_ROTATE.SelectedIndex = 0;
				cmbCAM_NO2_ROTATE.SelectedIndex = 0;

				cmbFTP_GBN.ItemsSource = CodeManager.Get기초코드(기초코드Type.FTP구분);

				txtBIT_ID.Text= "102";
				txtMOBILE_NO.Text = "31672";
				txtSTATION_NM.Text = "운정광역보건지소(중)";

				txtSERVER_IPLIST.Text = "172.8.1.61";
				txtSERVER_PORTLIST.Text = "32100";

				cmbFTP_GBN.SelectedValue = 2;
				txtFTP_IP.Text = "172.8.1.61";
				txtFTP_PORT.Text = "2223";
				txtFTP_ID.Text = "pjbit";
				txtFTP_PWD.Password= "pjbit@8732";

				txtHTTP_IP.Text = "172.8.1.60";
				txtHTTP_PORT.Text = "8080";

				cmbENV2_PORT.SelectedValue = "COM3";
				txtENV_BAUD_RATE.Text = "57600";
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		BIT_SYSTEM item = null;
		public void Load설정값(BIT_SYSTEM config)
		{
			try
			{
				if (config == null) return;
				item = config;

				txtBIT_ID.Text = string.Format("{0}", config.BIT_ID);
				txtMOBILE_NO.Text = string.Format("{0}", config.MOBILE_NO);
				txtSTATION_NM.Text = config.STATION_NM;

				if (config.ENV_PORT_NM!= null && config.ENV_PORT_NM.Equals("") == false) cmbENV2_PORT.SelectedValue = config.ENV_PORT_NM;
				txtENV_BAUD_RATE.Text = string.Format("{0}", config.ENV_BAUD_RATE);
				
				//string WEBCAM = config.WEBCAM_NM;
				//if (WEBCAM != null && WEBCAM.Equals("") == false) cmbWEBCAM.SelectedIndex = config.WEBCAM_NO;
				if (config.CAM_NO1 >=0) cmbCAM_NO1.SelectedIndex = config.CAM_NO1;
				cmbCAM_NO1_ROTATE.SelectedIndex = config.CAM_NO1_ROTATE;
				if (config.CAM_NO2>= 0) cmbCAM_NO2.SelectedIndex = config.CAM_NO2;
				cmbCAM_NO2_ROTATE.SelectedIndex = config.CAM_NO2_ROTATE;
				//string SHOCK = config.SHOCKCAM_NM;
				//if (SHOCK != null && SHOCK.Equals("") == false) cmbSHOCKCAM.SelectedIndex = config.SHOCKCAM_NO;

				//서버
				txtSERVER_IPLIST.Text = config.SERVER_URL;
				txtSERVER_PORTLIST.Text = config.SERVER_PORT;

				//HTTP
				txtHTTP_IP.Text = config.HTTP_URL;
				txtHTTP_PORT.Text = string.Format("{0}", config.HTTP_PORT);

				//FTP
				cmbFTP_GBN.SelectedValue = config.FTP_GBN;
				txtFTP_IP.Text = config.FTP_IP;
				txtFTP_PORT.Text = string.Format("{0}", config.FTP_PORT);
				txtFTP_ID.Text = config.FTP_USERID;
				txtFTP_PWD.Password = config.FTP_USERPWD;
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
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public delegate bool 데이터변경Handler(int nGBN , BIT_SYSTEM _item);
		public event 데이터변경Handler On데이터변경Event;
		private bool Save설정값()
		{
			try
			{
				bool UpdateYN = false;
				BIT_SYSTEM item설정 = new BIT_SYSTEM();
				if (item != null && item.SEQ_NO > 0) //Update
				{
					UpdateYN = true;
					item설정.SEQ_NO = item.SEQ_NO;
				}

				item설정.BIT_ID = Convert.ToInt32(txtBIT_ID.Text);
				item설정.MOBILE_NO = Convert.ToInt32(txtMOBILE_NO.Text);
				item설정.STATION_NM = txtSTATION_NM.Text.Trim();

				item설정.ENV_PORT_NM = "";
				if (cmbENV2_PORT.SelectedIndex >= 0) item설정.ENV_PORT_NM = cmbENV2_PORT.SelectedValue.ToString();
				item설정.ENV_BAUD_RATE = Convert.ToInt32(txtENV_BAUD_RATE.Text);
				//item설정.WEBCAM_NM = "";
				//if (cmbWEBCAM.SelectedIndex >= 0) item설정.WEBCAM_NM = cmbWEBCAM.SelectedValue.ToString();
				//item설정.SHOCKCAM_NM = "";
				//if (cmbSHOCKCAM.SelectedIndex >= 0) item설정.SHOCKCAM_NM = cmbSHOCKCAM.SelectedValue.ToString();
				item설정.CAM_NO1 = -1;
				if (cmbCAM_NO1.SelectedIndex >= 0) item설정.CAM_NO1 = cmbCAM_NO1.SelectedIndex;
				item설정.CAM_NO1_ROTATE = cmbCAM_NO1_ROTATE.SelectedIndex;

				item설정.CAM_NO2= -1;
				if (cmbCAM_NO2.SelectedIndex >= 0) item설정.CAM_NO2= cmbCAM_NO2.SelectedIndex;
				item설정.CAM_NO2_ROTATE = cmbCAM_NO2_ROTATE.SelectedIndex;

				item설정.SERVER_URL = txtSERVER_IPLIST.Text.Trim();
				item설정.SERVER_PORT = txtSERVER_PORTLIST.Text.Trim();

				BC_CODE itemFTP = cmbFTP_GBN.SelectedItem as BC_CODE;
				if (itemFTP != null) item설정.FTP_GBN = itemFTP.nCODE;
				item설정.FTP_IP = txtFTP_IP.Text.Trim();
				item설정.FTP_PORT = Convert.ToInt32(txtFTP_PORT.Text);
				item설정.FTP_USERID = txtFTP_ID.Text.Trim();
				item설정.FTP_USERPWD = txtFTP_PWD.Password.Trim();

				item설정.HTTP_URL = txtHTTP_IP.Text.Trim();
				item설정.HTTP_PORT = Convert.ToInt32(txtHTTP_PORT.Text);

				bool 결과YN = false;
				if (UpdateYN == false)
				{
					if (On데이터변경Event != null) 결과YN =On데이터변경Event(1, item설정);
					//결과YN = DatabaseManager.INSERT_BIT_SYSTEM(item설정);
				}
				else
				{
					if (On데이터변경Event != null) 결과YN = On데이터변경Event(2, item설정);
				}

				return 결과YN;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
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
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
	}
}
