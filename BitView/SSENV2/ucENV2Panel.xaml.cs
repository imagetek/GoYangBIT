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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using SSCommonNET;
using SSControlsNET;
using SSData;

namespace BitView
{
	/// <summary>
	/// ucENV2Panel.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ucENV2Panel : UserControl
	{
		public ucENV2Panel()
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
					//InitProc();

					//Load기본값();

					//Load환경설정();
				}

				//if (_dtRequest != null) _dtRequest.Start();
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
				this.IsEnabled = false;

				cmbENVGBN.ItemsSource = null;
				cmbENVGBN.DisplayMemberPath = "NAME";
				cmbENVGBN.SelectedValuePath = "nCODE";
				cmbENVGBN.ItemsSource = CodeManager.Get기초코드(기초코드Type.환경보드종류);

				cmbENV_PORT.Items.Clear();
				cmbENV_PORT.Items.Add("미사용");
				string[] sPort = System.IO.Ports.SerialPort.GetPortNames();
				if (sPort != null && sPort.Length > 0)
				{
					foreach (string port in sPort)
					{
						cmbENV_PORT.Items.Add(port);
					}
				}

				SSENV2Manager.On상태정보Event += SSENV2Manager_On상태정보Event;
				SSENV2Manager.On보드상태정보Event += SSENV2Manager_On보드상태정보Event;
				SSENV2Manager.On설정정보Event += SSENV2Manager_On설정정보Event;
				SSENV2Manager.On센서정보Event += SSENV2Manager_On센서정보Event;
				SSENV2Manager.On연결종료Event += SSENV2Manager_On연결종료Event;
				SSENV2Manager.On연결재시도Event += SSENV2Manager_On연결재시도Event;

				Refresh사용자모드(false);
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

		

		public void Initialize환경보드()
		{
			try
			{
				InitProc();

				Load기본값();

				Load환경설정();

				Load자동실행();

				//if (_dtRequest != null) _dtRequest.Start();
			}
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
		}

		private void Load환경설정()
		{
			try
			{
				var config = BITDataManager.BitSystem;

				if (config.ENV_GBN >= 0) cmbENVGBN.SelectedValue = config.ENV_GBN;

				if (config.ENV_PORT_NM != null && config.ENV_PORT_NM.Equals("") == false)
				{
					cmbENV_PORT.SelectedValue = config.ENV_PORT_NM;
				}

				//if (BITDataManager.BitConfig.ENV.REFRESH_TIME > 0)
				//{
				//	_dtRequest.Interval = TimeSpan.FromSeconds(BITDataManager.BitConfig.ENV.REFRESH_TIME);
				//}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Load자동실행()
		{
			try
			{
				if (cmbENV_PORT.SelectedValue != null)
				{
					string ComPort = cmbENV_PORT.SelectedValue.ToString();
					if (ComPort.Equals("미사용") == true) return;

					EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] 자동으로 연결합니다."));

					btnENV2연결_Click(btnENV2연결, null);

					CommonUtils.WaitTime(500, true);

					//btnENV2상태요청_Click(btnENV2상태요청, null);

					//CommonUtils.WaitTime(500, true);

					switch (nENVGBN)
					{
						case 1:
							break;

						default:
							btnENV2설정요청_Click(btnENV2설정요청, null);

							CommonUtils.WaitTime(500, true);

							SSENV2Manager.Send환경보드시간설정(DateTime.Now.ToString("yyyyMMddHHmmss"));
							break;
					}
				}
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
				chkAC1_M.IsChecked = false;
				chkDCFAN_M.IsChecked = false;
				chkLAMP_M.IsChecked = false;
				chk화면장치_M.IsChecked = false;

				//txt주기.Text = "5";
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
				//if (_w비번 != null) _w비번.Close();

				//if (_dtRequest != null)
				//{
				//	_dtRequest.Stop();
				//	_dtRequest.Tick -= _dtRequest_Tick;
				//	_dtRequest = null;
				//}

				SSENV2Manager.On상태정보Event -= SSENV2Manager_On상태정보Event;
				SSENV2Manager.On보드상태정보Event -= SSENV2Manager_On보드상태정보Event;
				SSENV2Manager.On설정정보Event -= SSENV2Manager_On설정정보Event;
				SSENV2Manager.On센서정보Event -= SSENV2Manager_On센서정보Event;
				SSENV2Manager.On연결종료Event -= SSENV2Manager_On연결종료Event;
				SSENV2Manager.On연결재시도Event -= SSENV2Manager_On연결재시도Event;
				
				시리얼DoFinal();

				GC.Collect();
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void 시리얼DoFinal()
		{
			try
			{
				SSENV2Manager.DoFinal();
				EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] 초기화중입니다."));
				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		int nENVGBN = -1;
		private void cmbENVGBN_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				ComboBox cb = sender as ComboBox;
				if (cb == null) return;

				BC_CODE item = cb.SelectedItem as BC_CODE;
				if (item == null) return;

				nENVGBN = item.nCODE;

				switch (nENVGBN)
				{
					default:
						gb신형그외.Visibility = Visibility.Visible;
						gb신형시간.Visibility = Visibility.Visible;
						Refresh사용자모드(true);
						break;

					case 1:
						gb신형그외.Visibility = Visibility.Collapsed;
						gb신형시간.Visibility = Visibility.Collapsed;
						Refresh사용자모드(true);
						break;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void chkDCFAN_M_Checked(object sender, RoutedEventArgs e)
		{
			try
			{
				CheckBox chk = sender as CheckBox;
				if (chk == null) return;

				//체크시 수동
				if (chk.IsChecked.Value == true)
				{
					txtDC_FAN_ON.IsReadOnly = false;
					txtDC_FAN_OFF.IsReadOnly = false;
				}
				else
				{
					txtDC_FAN_ON.IsReadOnly = true;
					txtDC_FAN_OFF.IsReadOnly = true;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void chkAC1_M_Checked(object sender, RoutedEventArgs e)
		{
			try
			{
				CheckBox chk = sender as CheckBox;
				if (chk == null) return;

				//체크시 수동
				if (chk.IsChecked.Value == true)
				{
					txtAC1_ON.IsReadOnly = false;
					txtAC1_OFF.IsReadOnly = false;
				}
				else
				{
					txtAC1_ON.IsReadOnly = true;
					txtAC1_OFF.IsReadOnly = true;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void chkAC2_M_Checked(object sender, RoutedEventArgs e)
		{
			try
			{
				CheckBox chk = sender as CheckBox;
				if (chk == null) return;

				//체크시 수동
				//if (chk.IsChecked.Value == true)
				//{
				//	txtAC2_ON.IsReadOnly = false;
				//	txtAC2_OFF.IsReadOnly = false;
				//}
				//else
				//{
				//	txtAC2_ON.IsReadOnly = true;
				//	txtAC2_OFF.IsReadOnly = true;
				//}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void chkLAMP_D_Checked(object sender, RoutedEventArgs e)
		{
			try
			{
				CheckBox chk = sender as CheckBox;
				if (chk == null) return;

				//체크시 사용안함
				txtLAMP_OFF_HH.IsReadOnly = !chk.IsChecked.Value;
				txtLAMP_OFF_MM.IsReadOnly = !chk.IsChecked.Value;
				txtLAMP_ON_HH.IsReadOnly = !chk.IsChecked.Value;
				txtLAMP_ON_MM.IsReadOnly = !chk.IsChecked.Value;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void chk화면장치_D_Checked(object sender, RoutedEventArgs e)
		{
			try
			{
				CheckBox chk = sender as CheckBox;
				if (chk == null) return;

				//체크시 사용안함
				txt화면장치_ON_HH.IsReadOnly = !chk.IsChecked.Value;
				txt화면장치_ON_MM.IsReadOnly = !chk.IsChecked.Value;
				txt화면장치_OFF_HH.IsReadOnly = !chk.IsChecked.Value;
				txt화면장치_OFF_MM.IsReadOnly = !chk.IsChecked.Value;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btnENV2연결_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button btn = sender as Button;	// 환경보드설정 com연결버튼 클릭시
				if (btn == null) return;

				if (cmbENVGBN.SelectedIndex < 0 || nENVGBN < 0)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미선택", "환경보드 종류를 선택해주세요.");
					return;
				}

				if (cmbENV_PORT.SelectedIndex < 0)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미선택", "시리얼포트를 선택해주세요.");
					return;
				}
				string ComPort = cmbENV_PORT.SelectedValue.ToString();
				//string ComPort = "COM6";
				if (ComPort.Equals("미사용") == true) return;

				EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] 접속을 시도중입니다. {0}", ComPort));
				SSENV2Manager.ConnectProc(ComPort, 921600);  // 신형 57600 115200 230400 460800 921600

				btnENV2연결.IsEnabled = false;
				btnENV2해제.IsEnabled = true;
				txt연결상태.Text = "연결중";
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btnENV2해제_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;

				bool ConnYN = SSENV2Manager.ConnectYN();

				EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] 접속해제를 시도중입니다. "));
				SSENV2Manager.DisConnectProc();
				
				btnENV2연결.IsEnabled = true;
				btnENV2해제.IsEnabled = false;
				txt연결상태.Text = "미연결중";
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btnENV2상태요청_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;

				if (cmbENVGBN.SelectedIndex < 0 || nENVGBN < 0)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미선택", "환경보드 종류를 선택해주세요.");
					return;
				}

				bool ConnYN = SSENV2Manager.ConnectYN();
				txt연결상태.Text = ConnYN == true ? "연결중" : "미연결중";
				if (ConnYN == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미접속", "미접속상태입니다.(상태요청)");
					return;
				}

				EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] 상태정보값을 요청중입니다."));
				SSENV2Manager.Send상태정보요청();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btnENV2설정요청_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;
				if (nENVGBN.Equals(0) == false) return;

				bool ConnYN = SSENV2Manager.ConnectYN(); 
				txt연결상태.Text = ConnYN == true ? "연결중" : "미연결중";
				if (ConnYN == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미접속", "미접속상태입니다.(설정값요청)");
					return;
				}

				EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] 환경보드 설정값을 요청중입니다."));
				SSENV2Manager.Send환경보드설정값요청();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public bool ENV연결확인()
		{
			bool ConnYN = false;
			switch (nENVGBN)
			{
				default: ConnYN = SSENV2Manager.ConnectYN(); break;
			}
			return ConnYN;
		}

		private void btn시간요청_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;

				bool ConnYN = SSENV2Manager.ConnectYN();
				txt연결상태.Text = ConnYN == true ? "연결중" : "미연결중";
				if (ConnYN == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미접속", "미접속상태입니다.(시간요청)");
					return;
				}

				EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] 보드의 시간값을 요청중입니다."));
				SSENV2Manager.Send환경보드시간요청();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btn시간설정_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;

				if (txtENV2시간.Text.Equals("") == true || txtENV2시간.Text.Length != 14)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "보드시간을 입력해주세요.");
					return;
				}

				bool ConnYN = SSENV2Manager.ConnectYN();
				txt연결상태.Text = ConnYN == true ? "연결중" : "미연결중";
				if (ConnYN == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미접속", "미접속상태입니다.(시간설정)");
					return;
				}

				EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] 보드의 시간값을 설정중입니다."));
				SSENV2Manager.Send환경보드시간설정(txtENV2시간.Text.Trim());
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btn센서설정_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;

				if (txt충격값.Text.Equals("") == true)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "충격설정값을 입력해주세요.");
					return;
				}
				if (CommonUtils.IsNumeric(txt충격값.Text) == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "충격설정값은 숫자로 입력해주세요.");
					return;
				}
				if (txt조도값.Text.Equals("") == true)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "조도설정값을 입력해주세요.");
					return;
				}
				if (CommonUtils.IsNumeric(txt조도값.Text) == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "조도설정값은 숫자로 입력해주세요.");
					return;
				}
				if (txt휘도값.Text.Equals("") == true)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "휘도설정값을 입력해주세요.");
					return;
				}
				if (CommonUtils.IsNumeric(txt휘도값.Text) == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "휘도설정값은 숫자로 입력해주세요.");
					return;
				}
				if (txt주기값.Text.Equals("") == true)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "(상태정보요청)주기설정값을 입력해주세요.");
					return;
				}
				if (CommonUtils.IsNumeric(txt주기값.Text) == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미입력", "(상태정보요청)주기설정값은 숫자로 입력해주세요.");
					return;
				}

				bool ConnYN = SSENV2Manager.ConnectYN();
				txt연결상태.Text = ConnYN == true ? "연결중" : "미연결중";
				if (ConnYN == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미접속", "미접속상태입니다.(센서설정)");
					return;
				}

				int 충격 = Convert.ToInt32(txt충격값.Text);
				int 조도 = Convert.ToInt32(txt조도값.Text);
				int 휘도 = Convert.ToInt32(txt휘도값.Text);
				int 주기 = Convert.ToInt32(txt주기값.Text);

				EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] 보드의 주기및충격값등을 설정중입니다."));
				SSENV2Manager.Send주기및충격값설정(주기, 충격, 조도, 휘도);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btn센서요청_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;

				bool ConnYN = SSENV2Manager.ConnectYN();
				txt연결상태.Text = ConnYN == true ? "연결중" : "미연결중";
				if (ConnYN == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미접속", "미접속상태입니다.(센서요청)");
					return;
				}

				//7E-7E-00-03-01-00-25-27
				//7E-7E-00-07-01-00-25-01-46-50-5A-6E 응답

				EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] 보드의 주기및충격값등을 요청중입니다."));
				SSENV2Manager.Send주기충격조도휘도값요청();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btnDCFAN설정_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;

				if (chkDCFAN_M.IsChecked.Value == true)
				{
					if (txtDC_FAN_ON.Text.Equals("") == true)
					{
						MsgDlgManager.ShowIntervalInformationDlg("미입력", "DCFAN On값을 입력해주세요.");
						return;
					}
					if (CommonUtils.IsNumeric(txtDC_FAN_ON.Text) == false)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "DCFAN On값은 숫자만 입력가능합니다.");
						return;
					}

					if (txtDC_FAN_OFF.Text.Equals("") == true)
					{
						MsgDlgManager.ShowIntervalInformationDlg("미입력", "DCFAN OFF값을 입력해주세요.");
						return;
					}
					if (CommonUtils.IsNumeric(txtDC_FAN_OFF.Text) == false)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "DCFAN OFF값은 숫자만 입력가능합니다.");
						return;
					}
				}

				bool ConnYN = SSENV2Manager.ConnectYN();
				txt연결상태.Text = ConnYN == true ? "연결중" : "미연결중";
				if (ConnYN == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미접속", "미접속상태입니다.(DCFAN설정)");
					return;
				}

				EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] DC FAN값 설정을 요청중입니다."));

				SSENV2Manager.SendDCFAN설정(chkDCFAN_M.IsChecked.Value == true ? 0 : 1
					, Convert.ToInt32(txtDC_FAN_ON.Text)
					, Convert.ToInt32(txtDC_FAN_OFF.Text));
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btnAC1설정_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;

				if (chkAC1_M.IsChecked.Value == true)
				{
					if (txtAC1_ON.Text.Equals("") == true)
					{
						MsgDlgManager.ShowIntervalInformationDlg("미입력", "AC1 On값을 입력해주세요.");
						return;
					}
					if (CommonUtils.IsNumeric(txtAC1_ON.Text) == false)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "AC1 On값은 숫자만 입력가능합니다.");
						return;
					}

					if (txtAC1_OFF.Text.Equals("") == true)
					{
						MsgDlgManager.ShowIntervalInformationDlg("미입력", "AC1 OFF값을 입력해주세요.");
						return;
					}
					if (CommonUtils.IsNumeric(txtAC1_OFF.Text) == false)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "AC1 OFF값은 숫자만 입력가능합니다.");
						return;
					}
				}

				bool ConnYN = SSENV2Manager.ConnectYN();
				txt연결상태.Text = ConnYN == true ? "연결중" : "미연결중";
				if (ConnYN == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미접속", "미접속상태입니다.(AC1설정)");
					return;
				}

				EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] 히터값 설정을 요청중입니다."));

				SSENV2Manager.Send히터설정(chkAC1_M.IsChecked.Value == true ? 0 : 1
					, Convert.ToInt32(txtAC1_ON.Text)
					, Convert.ToInt32(txtAC1_OFF.Text));
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btnLAMP설정_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;

				int ON_HH = 0;
				int ON_MM = 0;
				int OFF_HH = 0;
				int OFF_MM = 0;
				if (chkLAMP_M.IsChecked.Value == true)
				{
					if (txtLAMP_ON_HH.Text.Equals("") == true)
					{
						MsgDlgManager.ShowIntervalInformationDlg("미입력", "LAMP On시간값을 입력해주세요.");
						return;
					}
					if (CommonUtils.IsNumeric(txtLAMP_ON_HH.Text) == false)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "LAMP On시간값은 숫자만 입력가능합니다.");
						return;
					}
					ON_HH = Convert.ToInt32(txtLAMP_ON_HH.Text);
					if (ON_HH < 0 || ON_HH > 24)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "LAMP On시간값은 0~24만 입력가능합니다.");
						return;
					}

					if (txtLAMP_ON_MM.Text.Equals("") == true)
					{
						MsgDlgManager.ShowIntervalInformationDlg("미입력", "LAMP On분값을 입력해주세요.");
						return;
					}
					if (CommonUtils.IsNumeric(txtLAMP_ON_MM.Text) == false)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "LAMP On분값은 숫자만 입력가능합니다.");
						return;
					}
					ON_MM = Convert.ToInt32(txtLAMP_ON_MM.Text);
					if (ON_MM < 0 || ON_MM > 60)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "LAMP On분값은 0~60만 입력가능합니다.");
						return;
					}

					if (txtLAMP_OFF_HH.Text.Equals("") == true)
					{
						MsgDlgManager.ShowIntervalInformationDlg("미입력", "LAMP OFF시간값을 입력해주세요.");
						return;
					}
					if (CommonUtils.IsNumeric(txtLAMP_OFF_HH.Text) == false)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "LAMP OFF시간값은 숫자만 입력가능합니다.");
						return;
					}
					OFF_HH = Convert.ToInt32(txtLAMP_OFF_HH.Text);
					if (OFF_HH < 0 || OFF_HH > 24)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "LAMP OFF시간값은 0~24만 입력가능합니다.");
						return;
					}

					if (txtLAMP_OFF_MM.Text.Equals("") == true)
					{
						MsgDlgManager.ShowIntervalInformationDlg("미입력", "LAMP OFF분값을 입력해주세요.");
						return;
					}
					if (CommonUtils.IsNumeric(txtLAMP_OFF_MM.Text) == false)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "LAMP OFF분값은 숫자만 입력가능합니다.");
						return;
					}
					OFF_MM = Convert.ToInt32(txtLAMP_OFF_MM.Text);
					if (OFF_MM < 0 || OFF_MM > 60)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "LAMP OFF분값은 0~60만 입력가능합니다.");
						return;
					}
				}

				bool ConnYN = SSENV2Manager.ConnectYN();
				txt연결상태.Text = ConnYN == true ? "연결중" : "미연결중";
				if (ConnYN == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미접속", "미접속상태입니다.(LAMP설정)");
					return;
				}

				EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] LAMP 설정을 요청중입니다."));

				SSENV2Manager.SendLAMP설정(chkLAMP_M.IsChecked.Value == true ? 0 : 1
					, ON_HH, ON_MM
					, OFF_HH, OFF_MM);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btn화면장치설정_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;

				int ON_HH = 0;
				int ON_MM = 0;
				int OFF_HH = 0;
				int OFF_MM = 0;
				if (chk화면장치_M.IsChecked.Value == true)
				{
					if (txt화면장치_ON_HH.Text.Equals("") == true)
					{
						MsgDlgManager.ShowIntervalInformationDlg("미입력", "화면장치On시간값을 입력해주세요.");
						return;
					}
					if (CommonUtils.IsNumeric(txt화면장치_ON_HH.Text) == false)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "화면장치On시간값은 숫자만 입력가능합니다.");
						return;
					}
					ON_HH = Convert.ToInt32(txt화면장치_ON_HH.Text);
					if (ON_HH < 0 || ON_HH > 24)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "화면장치On시간값은 0~24만 입력가능합니다.");
						return;
					}

					if (txt화면장치_ON_MM.Text.Equals("") == true)
					{
						MsgDlgManager.ShowIntervalInformationDlg("미입력", "화면장치On분값을 입력해주세요.");
						return;
					}
					if (CommonUtils.IsNumeric(txt화면장치_ON_MM.Text) == false)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "화면장치On분값은 숫자만 입력가능합니다.");
						return;
					}
					ON_MM = Convert.ToInt32(txt화면장치_ON_MM.Text);
					if (ON_MM < 0 || ON_MM > 60)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "화면장치On분값은 0~60만 입력가능합니다.");
						return;
					}

					if (txt화면장치_OFF_HH.Text.Equals("") == true)
					{
						MsgDlgManager.ShowIntervalInformationDlg("미입력", "화면장치OFF시간값을 입력해주세요.");
						return;
					}
					if (CommonUtils.IsNumeric(txt화면장치_OFF_HH.Text) == false)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "화면장치OFF시간값은 숫자만 입력가능합니다.");
						return;
					}
					OFF_HH = Convert.ToInt32(txt화면장치_OFF_HH.Text);
					if (OFF_HH < 0 || OFF_HH > 24)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "화면장치OFF시간값은 0~24만 입력가능합니다.");
						return;
					}

					if (txt화면장치_OFF_MM.Text.Equals("") == true)
					{
						MsgDlgManager.ShowIntervalInformationDlg("미입력", "LAMP OFF분값을 입력해주세요.");
						return;
					}
					if (CommonUtils.IsNumeric(txt화면장치_OFF_MM.Text) == false)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "화면장치OFF분값은 숫자만 입력가능합니다.");
						return;
					}
					OFF_MM = Convert.ToInt32(txt화면장치_OFF_MM.Text);
					if (OFF_MM < 0 || OFF_MM > 60)
					{
						MsgDlgManager.ShowIntervalInformationDlg("입력오류", "화면장치OFF분값은 0~60만 입력가능합니다.");
						return;
					}
				}

				bool ConnYN = SSENV2Manager.ConnectYN();
				txt연결상태.Text = ConnYN == true ? "연결중" : "미연결중";
				if (ConnYN == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미접속", "미접속상태입니다.(화면장치설정)");
					return;
				}

				EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] 화면장치 설정을 요청중입니다."));

				SSENV2Manager.Send화면장치설정(chk화면장치_M.IsChecked.Value == true ? 0 : 1
					, ON_HH, ON_MM
					, OFF_HH, OFF_MM);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public delegate void 충격감지Handler(int ShockValue);
		public event 충격감지Handler On충격감지Event;

		private void Refresh사용자모드(bool AdminYN = false)
		{
			try
			{
				sp메인전원.Visibility = AdminYN == true ? Visibility.Visible : Visibility.Hidden;
				sp메인전류.Visibility = AdminYN == true ? Visibility.Visible : Visibility.Hidden;
				grp램프시간.Visibility = AdminYN == true ? Visibility.Visible : Visibility.Hidden;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		#region 화면표시
		private void Display상태정보S2(SSNS_ENV2_STATE item)
		{
			try
			{
				//txt버전.Text = item.버전정보;
				//txt메인전원.Text = item.메인전원;
				//txt보조전원.Text = item.보조전원;
				//txt온도1.Text = item.온도1;
				//txt온도2.Text = item.온도2;
				//txt습도.Text = item.습도;
				//txt조도.Text = item.조도;
				//txt충격.Text = item.충격1;
				//txt도어.Text = item.도어상태;
				//txt휘도.Text = item.휘도;
				txt버전.Text = item.버전정보;
				txt메인전원.Text = item.MainVoltage.ToString();
				txt메인전류.Text = item.MainCurrent.ToString();
				txt온도1.Text = item.온도1;
				txt온도2.Text = item.온도2;
				txt습도.Text = item.습도;
				txt조도.Text = item.조도;
				txt충격.Text = item.충격1;
				txt도어.Text = item.도어상태;
				txt휘도.Text = item.휘도;
				txtBattPsnt.Text = item.BattPercent.ToString();
				txtBattRemain.Text = item.RemainBat.ToString();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Display센서정보S2(SSNS_ENV2_SENSOR item)
		{
			try
			{
				txt충격값.Text = string.Format("{0}", item.SetShock);
				txt휘도값.Text = string.Format("{0}", item.SetLuminance);
				txt조도값.Text = string.Format("{0}", item.SetIllumination);
				txt주기값.Text = string.Format("{0}", item.SetPeriod);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Display보드상태S2(SSNS_ENV2_BOARD_STATE item)
		{
			try
			{
				txtENV2시간.Text = item.mBoardDateTime;
				txtENV2On시간.Text = item.mActiveDateTime;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Display설정정보S2(SSNS_ENV2_CONFIG item)
		{
			try
			{
				txtDC_FAN_ON.Text = string.Format("{0}", item.nFANOnTemp);
				txtDC_FAN_OFF.Text = string.Format("{0}", item.nFANOffTemp);
				chkDCFAN_M.IsChecked = item.nFanMode == 1 ? false : true;

				txtAC1_ON.Text = string.Format("{0}", item.nHeaterOnTemp);
				txtAC1_OFF.Text = string.Format("{0}", item.nHeaterOffTemp);
				chkAC1_M.IsChecked = item.nHeaterMode == 1 ? false : true;

				txtLAMP_ON_HH.Text = string.Format("{0}", item.nLampOnHour);
				txtLAMP_ON_MM.Text = string.Format("{0}", item.nLampOnMin);
				txtLAMP_OFF_HH.Text = string.Format("{0}", item.nLampOffHour);
				txtLAMP_OFF_MM.Text = string.Format("{0}", item.nLampOffMin);
				chkLAMP_M.IsChecked = item.nLampMode == 1 ? false : true;

				txt화면장치_ON_HH.Text = string.Format("{0}", item.nScreenOnHour);
				txt화면장치_ON_MM.Text = string.Format("{0}", item.nScreenOnMin);
				txt화면장치_OFF_HH.Text = string.Format("{0}", item.nScreenOffHour);
				txt화면장치_OFF_MM.Text = string.Format("{0}", item.nScreenOffMin);
				chk화면장치_M.IsChecked = item.nScreenMode == 1 ? false : true;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		#endregion

		private void SSENV2Manager_On이벤트정보Event(SSNS_ENV2_EVENT_TYPE _data)
		{
			try
			{
				switch (_data)
				{
					case SSNS_ENV2_EVENT_TYPE.SHOCK_EVENT:
						MsgDlgManager.ShowIntervalInformationDlg("이벤트", "충격감지", 1000);
						break;

					case SSNS_ENV2_EVENT_TYPE.DOOR_EVENT:
						MsgDlgManager.ShowIntervalInformationDlg("이벤트", "문열림감지", 1000);
						break;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void SSENV2Manager_On상태정보Event(SSNS_ENV2_STATE _data)
		{
			try
			{
				if (_data != null)
				{
					this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
					{
						Display상태정보S2(_data);

						BITDataManager.Set환경보드값(_data);

						Check환경보드S2(_data);
					}));
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void SSENV2Manager_On연결종료Event()
		{
			try
			{
				this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
				{
					MsgDlgManager.ShowIntervalInformationDlg("알림", "연결이 종료되었습니다.");
					EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] 연결이 종료되었습니다."));
					btnENV2연결.IsEnabled = true;
					btnENV2해제.IsEnabled = false;
					txt연결상태.Text = "미연결중";
				}));
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void SSENV2Manager_On연결재시도Event()	//pjh
		{
			try
			{
				this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
				{
					//MsgDlgManager.ShowIntervalInformationDlg("알림", "연결이 재시도 되었습니다.");
					EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] 연결이 재시도 되었습니다."));
					btnENV2연결_Click(btnENV2연결, null);
				}));
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void SSENV2Manager_On센서정보Event(SSNS_ENV2_SENSOR _data)
		{
			try
			{
				if (_data != null)
				{
					this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
					{
						Display센서정보S2(_data);
					}));
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void SSENV2Manager_On설정정보Event(SSNS_ENV2_CONFIG _data)
		{
			try
			{
				if (_data != null)
				{
					this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
					{
						Display설정정보S2(_data);
					}));
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void SSENV2Manager_On보드상태정보Event(SSNS_ENV2_BOARD_STATE _data)
		{
			try
			{
				if (_data != null)
				{
					this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
					{
						Display보드상태S2(_data);
					}));
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public void Set설정값변경()
		{
			try
			{
				//히터 , 팬 , 동작감지 , 충격값
				var config = BITDataManager.BitENVConfig.ITEM;

				txtDC_FAN_ON.Text = string.Format("{0:d4}", config.FANMaxTemp);
				txtDC_FAN_OFF.Text = string.Format("{0:d4}", config.FANMinTemp);

				txtAC1_ON.Text = string.Format("{0:d4}", config.HeaterMaxTemp);
				txtAC1_OFF.Text = string.Format("{0:d4}", config.HeaterMinTemp);

				//txt충격값.Text = string.Format("{0}", config.ShockDetectValue);
				txt충격값.Text = string.Format("{0}", config.ShockDetectValue);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void Check환경보드S2(SSNS_ENV2_STATE item)
		{
			try
			{
				var config = BITDataManager.BitENVConfig.ITEM;
				if (config == null) return;

				int ShockDetectValue = BITDataManager.BitConfig.SHOCK_DETECT_NO;
				if (ShockDetectValue > 0 && item.Shock1 > ShockDetectValue)
				{
					//MsgDlgManager.ShowIntervalInformationDlg("충격감지",
					//	string.Format("충격값을 감지했습니다.\r\n{0}/{1}", item.Shock1, config.ShockDetectValue));
					//ShareMemoryManager.SendWebcamMessage("SHOCK");

					EventManager.DisplayLog(Log4Level.Info, string.Format("[ENV] 충격값 감지 현재:{0}/설정:{1}", item.Shock1, ShockDetectValue));

					if (On충격감지Event != null) On충격감지Event(item.Shock1);
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		#region 관리자모드

		//wnd관리자비번 _w비번 = null;
		//private void btn관리자모드_Click(object sender, RoutedEventArgs e)
		//{
		//	try
		//	{
		//		Button btn = sender as Button;
		//		if (btn == null) return;

		//		if (_w비번 != null) _w비번.Close();

		//		if (_w비번 == null)
		//		{
		//			_w비번 = new wnd관리자비번();
		//			_w비번.Closed += _w비번_Closed;
		//			_w비번.On비밀번호일치Event += _w비번_On비밀번호일치Event;
		//		}
		//		_w비번.SetParentWindow(_p);
		//		_w비번.Owner = _p;
		//		_w비번.WindowStartupLocation = WindowStartupLocation.CenterOwner;
		//		_w비번.Topmost = true;
		//		_w비번.ShowDialog();
		//	}
		//	catch (Exception ee)
		//	{
		//		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//	}
		//}

		//private void _w비번_On비밀번호일치Event()
		//{
		//	try
		//	{
		//		if (_w비번 != null) _w비번.Close();

		//		Refresh사용자모드(true);
		//	}
  //          catch (Exception ee)
  //          {
  //              TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
  //              System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
  //          }
		//}

		//private void _w비번_Closed(object sender, EventArgs e)
		//{
		//	try
		//	{
		//		if (_w비번 != null)
		//		{
		//			_w비번.Closed -= _w비번_Closed;
		//			_w비번.On비밀번호일치Event -= _w비번_On비밀번호일치Event;
		//			_w비번 = null;
		//		}
		//		GC.Collect();
		//	}
  //          catch (Exception ee)
  //          {
  //              TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
  //              System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
  //          }
		//}

		#endregion

	}
}
