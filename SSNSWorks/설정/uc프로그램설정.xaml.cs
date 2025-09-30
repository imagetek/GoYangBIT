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

namespace SSNSWorks
{
	/// <summary>
	/// ucENV2Panel.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class uc프로그램설정 : UserControl
	{
		public uc프로그램설정()
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
				nLOGSAVE_DAY.Maximum = 180;
				nLOGSAVE_DAY.Minimum = 1;			

				nLOGSAVE_PERCENT.Maximum = 100;				
				nLOGSAVE_PERCENT.Minimum = 1;

				nLED_PAGE_CHGTIME.Minimum = 2;
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
				nLOGSAVE_PERCENT.Value = 50;
				nLOGSAVE_DAY.Value = 30;

				nLED_PAGE_CHGTIME.Value = 5;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		TB_SYSTEM item = null;
		public void Load설정값(TB_SYSTEM config)
		{
			try
			{
				if (config == null) return;
				item = config;

				nLOGSAVE_DAY.Value = config.LOGSAVE_DAY;
				nLOGSAVE_PERCENT.Value = config.LOGSAVE_PERCENT;
				nLED_PAGE_CHGTIME.Value = config.LED_PAGE_CHGTIME;

				USE_BUSNO_COLOR.IsChecked = config.LED_USE_BUSNO_COLOR;
				USE_BUSNO_LOW.IsChecked = config.LED_USE_BUSNO_LOW;
				USE_ARRIVESOON_COLOR.IsChecked = config.LED_USE_ARRIVESOON_COLOR;
				USE_ARRIVESOON_LOW.IsChecked = config.LED_USE_ARRIVESOON_LOW;

				USE_FAN_MANUAL.IsChecked = config.ENV_USE_FAN_MANUAL;
				USE_HEATER_MANUAL.IsChecked = config.ENV_USE_HEATER_MANUAL;
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
				//if (txtBIT_ID.Text.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "BIT ID를 입력해주세요.");
				//	txtBIT_ID.Focus();
				//	return false;
				//}

				//if (CommonUtils.IsNumeric(txtBIT_ID.Text.Trim()) == false)
				//{
				//	MsgDlgManager.ShowMessageDlg("입력오류", "BIT ID는 숫자로 입력해주세요.");
				//	txtBIT_ID.Text = "0";
				//	txtBIT_ID.Focus();
				//	return false;
				//}

				//if (txtMOBILE_NO.Text.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "모바일번호를 입력해주세요.");
				//	txtMOBILE_NO.Focus();
				//	return false;
				//}

				//if (CommonUtils.IsNumeric(txtBIT_ID.Text.Trim()) == false)
				//{
				//	MsgDlgManager.ShowMessageDlg("입력오류", "모바일번호는 숫자로 입력해주세요.");
				//	txtMOBILE_NO.Text = "0";
				//	txtMOBILE_NO.Focus();
				//	return false;
				//}

				//if (txtSTATION_NM.Text.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "정류장명을 입력해주세요.");
				//	txtSTATION_NM.Focus();
				//	return false;
				//}

				////            if ( cmbDISP_GBN.SelectedIndex < 0)
				////{
				////                MsgDlgManager.ShowMessageDlg("미선택", "디스플레이를 선택해주세요.");
				////                cmbDISP_GBN.Focus();
				////                return false;
				////            }

				//if (cmbENV2_PORT.SelectedIndex < 0)
				//{
				//	MsgDlgManager.ShowMessageDlg("미선택", "환경보드를 선택해주세요.");
				//	cmbENV2_PORT.Focus();
				//	return false;
				//}

				//if (txtENV_BAUD_RATE.Text.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "환경보드 통신속도를 입력해주세요.");
				//	txtENV_BAUD_RATE.Focus();
				//	return false;
				//}

				//if (CommonUtils.IsNumeric(txtENV_BAUD_RATE.Text.Trim()) == false)
				//{
				//	MsgDlgManager.ShowMessageDlg("입력오류", "환경보드 통신속도는 숫자로 입력해주세요.");
				//	txtENV_BAUD_RATE.Text = "57600";
				//	txtENV_BAUD_RATE.Focus();
				//	return false;
				//}
								
				//if (cmbWEBCAM.SelectedIndex < 0)
				//{
				//	MsgDlgManager.ShowMessageDlg("미선택", "웹캠을 선택해주세요.");
				//	cmbWEBCAM.Focus();
				//	return false;
				//}

				//if (txtSERVER_IPLIST.Text.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "서버주소를 입력해주세요.");
				//	txtSERVER_IPLIST.Focus();
				//	return false;
				//}

				//if (txtSERVER_PORTLIST.Text.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "서버포트를 입력해주세요.");
				//	txtSERVER_PORTLIST.Focus();
				//	return false;
				//}

				//string[] portlist = txtSERVER_PORTLIST.Text.Trim().Split('|');
				//foreach (string port in portlist)
				//{
				//	if (CommonUtils.IsNumeric(port) == false)
				//	{
				//		MsgDlgManager.ShowMessageDlg("입력오류", "입력한 포트중 숫자가 아닌값이 존재합니다.");
				//		txtSERVER_PORTLIST.Text = "";
				//		txtSERVER_PORTLIST.Focus();
				//		return false;
				//	}
				//}

				//if (txtHTTP_IP.Text.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "HTTP주소를 입력해주세요.");
				//	txtHTTP_IP.Focus();
				//	return false;
				//}

				//if (txtHTTP_PORT.Text.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "HTTP포트를 입력해주세요.");
				//	txtHTTP_PORT.Focus();
				//	return false;
				//}

				//if (CommonUtils.IsNumeric(txtHTTP_PORT.Text.Trim()) == false)
				//{
				//	MsgDlgManager.ShowMessageDlg("입력오류", "HTTP포트는 숫자로 입력해주세요.");
				//	txtHTTP_PORT.Text = "";
				//	txtHTTP_PORT.Focus();
				//	return false;
				//}

				//if (txtFTP_IP.Text.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "FTP주소를 입력해주세요.");
				//	txtFTP_IP.Focus();
				//	return false;
				//}

				//if (txtFTP_PORT.Text.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "FTP포트를 입력해주세요.");
				//	txtFTP_PORT.Focus();
				//	return false;
				//}

				//if (CommonUtils.IsNumeric(txtFTP_PORT.Text.Trim()) == false)
				//{
				//	MsgDlgManager.ShowMessageDlg("입력오류", "FTP포트는 숫자로 입력해주세요.");
				//	txtFTP_PORT.Text = "";
				//	txtFTP_PORT.Focus();
				//	return false;
				//}

				//if (txtFTP_ID.Text.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "FTP 사용자ID를 입력해주세요.");
				//	txtFTP_ID.Focus();
				//	return false;
				//}

				//if (txtFTP_PWD.Password.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "FTP 비밀번호를 입력해주세요.");
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
					MsgDlgManager.ShowMessageDlg("기본설정 저장실패", "기본설정 저장작업중 에러가 발생했습니다.");
					return;
				}

				MsgDlgManager.ShowMessageDlg("기본설정 저장", "기본설정이 저장되었습니다.");
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public delegate bool 데이터변경Handler(int nGBN, TB_SYSTEM _item);
		public event 데이터변경Handler On데이터변경Event;
		private bool Save설정값()
		{
			try
			{
				bool UpdateYN = false;
				TB_SYSTEM item설정 = new TB_SYSTEM();
				if (item != null && item.SEQ_NO > 0) //Update
				{
					UpdateYN = true;
					item설정.SEQ_NO = item.SEQ_NO;
				}

				item설정.LOGSAVE_DAY = Convert.ToInt32(nLOGSAVE_DAY.Value);
				item설정.LOGSAVE_PERCENT= Convert.ToInt32(nLOGSAVE_PERCENT.Value);
				item설정.LED_PAGE_CHGTIME= Convert.ToInt32(nLED_PAGE_CHGTIME.Value);
				item설정.LED_USE_BUSNO_LOW = USE_BUSNO_LOW.IsChecked.Value;
				item설정.LED_USE_BUSNO_COLOR= USE_BUSNO_COLOR.IsChecked.Value;
				item설정.LED_USE_ARRIVESOON_LOW= USE_ARRIVESOON_LOW.IsChecked.Value;
				item설정.LED_USE_ARRIVESOON_COLOR= USE_ARRIVESOON_COLOR.IsChecked.Value;

				item설정.ENV_USE_FAN_MANUAL = USE_FAN_MANUAL.IsChecked.Value;
				item설정.ENV_USE_HEATER_MANUAL = USE_HEATER_MANUAL.IsChecked.Value;

				bool 결과YN = false;
				if (UpdateYN == false)
				{
					if (On데이터변경Event != null)결과YN = On데이터변경Event(1, item설정);
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
