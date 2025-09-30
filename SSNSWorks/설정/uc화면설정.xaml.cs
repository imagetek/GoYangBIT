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
	public partial class uc화면설정 : UserControl
	{
		public uc화면설정()
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
				cmbDISP_GBN.ItemsSource = null;
				cmbDISP_GBN.DisplayMemberPath = "NAME";
				cmbDISP_GBN.SelectedValuePath = "nCODE";

				dg화면.ItemsSource = null;
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
				cmbDISP_GBN.ItemsSource = CodeManager.GetDISPLAY구분();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public void Load설정값(List<BIT_DISPLAY> items)
		{
			try
			{
				dg화면.ItemsSource = items;
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
				dg화면.ItemsSource = null;

				GC.Collect();
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void cmbDISP_GBN_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				ComboBox cmb = sender as ComboBox;
				if (cmb == null) return;

				BC_CODE item = cmb.SelectedItem as BC_CODE;
				if (item == null) return;

				nPOS_X.Value = 0;
				nPOS_Y.Value = 0;

				DISPLAY구분 item화면 = (DISPLAY구분)item.nCODE;
				switch (item화면)
				{
					case DISPLAY구분.LCD가로형:
						nSZ_W.Value = 1920;
						nSZ_H.Value = 1080;
						break;

					case DISPLAY구분.LCD세로형:
						nSZ_W.Value = 1080;
						nSZ_H.Value = 1920;
						break;

					case DISPLAY구분.LED3X6:
						nSZ_W.Value = 6 * 16;
						nSZ_H.Value = 3 * 16;
						break;

					case DISPLAY구분.LED3X8:
						nSZ_W.Value = 8 * 16;
						nSZ_H.Value = 3 * 16;
						break;
				}

				nSZ_W.IsReadOnly = true;
				nSZ_H.IsReadOnly = true;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public delegate bool 데이터변경Handler(int nGBN, BIT_DISPLAY _item);
		public event 데이터변경Handler On데이터변경Event;

		public delegate void 데이터요청Handler();
		public event 데이터요청Handler On데이터요청Event;

		private void btn화면추가_Click(object sender, RoutedEventArgs e)
		{
			string guid = "";
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;

				if (IS_CHECK_ITEM() == false) return;

				BC_CODE item화면 = cmbDISP_GBN.SelectedItem as BC_CODE;
				if (item화면 == null)
				{
					MsgDlgManager.ShowMessageDlg("선택오류", "화면종류선택중 오류가 발생했습니다.");
					return;
				}

				BIT_DISPLAY item = new BIT_DISPLAY();
				item.BIT_ID = BITDataManager.BIT_ID;
				item.DISP_GBN = item화면.nCODE;
				item.POS_X = Convert.ToInt32(nPOS_X.Value);
				item.POS_Y = Convert.ToInt32(nPOS_Y.Value);
				item.SZ_W = Convert.ToInt32(nSZ_W.Value);
				item.SZ_H = Convert.ToInt32(nSZ_H.Value);
				item.DISP_NM = txtDISP_NM.Text.Trim();

				guid = MsgDlgManager.ShowInformationDlg("등록중", "화면정보를 등록중입니다.");

				bool result = false;
				if (On데이터변경Event != null) result=On데이터변경Event(1, item);

				if (guid.Equals("") == false)
				{
					MsgDlgManager.CloseInformationDlg(guid);
					guid = "";
					CommonUtils.WaitTime(50, true);
				}

				if (result == false)
				{
					MsgDlgManager.ShowMessageDlg("등록실패", "화면 등록중 오류가 발생했습니다.");
					return;
				}

				MsgDlgManager.ShowMessageDlg("등록완료", "화면이 등록되었습니다.");
				if (On데이터요청Event != null) On데이터요청Event();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			finally
			{
				if (guid.Equals("") == false)
				{
					MsgDlgManager.CloseInformationDlg(guid);
					guid = "";
					CommonUtils.WaitTime(50, true);
				}
			}
		}

		private void btn화면수정_Click(object sender, RoutedEventArgs e)
		{
			string guid = "";
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;

				if (dg화면.SelectedIndex < 0)
				{
					MsgDlgManager.ShowIntervalInformationDlg("미선택", "변경할 화면을 선택해주세요.");
					return;
				}

				BIT_DISPLAY item = dg화면.SelectedItem as BIT_DISPLAY;
				if (item == null)
				{
					MsgDlgManager.ShowIntervalInformationDlg("선택오류", "수정할 화면을 선택해주세요.");
					return;
				}

				BC_CODE item화면 = cmbDISP_GBN.SelectedItem as BC_CODE;
				if (item화면 == null)
				{
					MsgDlgManager.ShowIntervalInformationDlg("선택오류", "화면종류선택중 오류가 발생했습니다.");
					return;
				}

				guid = MsgDlgManager.ShowInformationDlg("변경중", "화면정보를 삭제중입니다.");

				item.BIT_ID = BITDataManager.BIT_ID;
				item.DISP_GBN = item화면.nCODE;
				item.POS_X = Convert.ToInt32(nPOS_X.Value);
				item.POS_Y = Convert.ToInt32(nPOS_Y.Value);
				item.SZ_W = Convert.ToInt32(nSZ_W.Value);
				item.SZ_H = Convert.ToInt32(nSZ_H.Value);

				item.DISP_NM = txtDISP_NM.Text.Trim();

				bool 결과YN = false;
				if (On데이터변경Event != null) 결과YN = On데이터변경Event(2, item);

				if (guid.Equals("") == false)
				{
					MsgDlgManager.CloseInformationDlg(guid);
					guid = "";
					CommonUtils.WaitTime(50, true);
				}
				if (결과YN == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("변경실패", "화면변경중 오류가 발생했습니다.");
					return;
				}

				MsgDlgManager.ShowIntervalInformationDlg("변경완료", "화면정보가 변경되었습니다.");
				if (On데이터요청Event != null) On데이터요청Event();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			finally
			{
				if (guid.Equals("") == false)
				{
					MsgDlgManager.CloseInformationDlg(guid);
					guid = "";
					CommonUtils.WaitTime(50, true);
				}
			}
		}

		private void btn화면삭제_Click(object sender, RoutedEventArgs e)
		{
			string guid = "";
			try
			{
				Button btn = sender as Button;
				if (btn == null) return;

				if (dg화면.SelectedIndex < 0)
				{
					MsgDlgManager.ShowMessageDlg("미선택", "삭제할 화면을 선택해주세요.");
					return;
				}

				BIT_DISPLAY item = dg화면.SelectedItem as BIT_DISPLAY;
				if (item == null)
				{
					MsgDlgManager.ShowMessageDlg("선택오류", "화면선택중 오류가 발생했습니다.");
					return;
				}

				bool 삭제YN = MsgDlgManager.ShowQuestionDlg("확인", string.Format("화면 {0}를 삭제하시겠습니까?", item.DISP_NM));
				if (삭제YN == false) return;

				guid = MsgDlgManager.ShowInformationDlg("삭제중", "화면정보를 삭제중입니다.");

				bool 결과YN = false;
				if (On데이터변경Event != null) 결과YN = On데이터변경Event(3, item);

				if (guid.Equals("") == false)
				{
					MsgDlgManager.CloseInformationDlg(guid);
					guid = "";
					CommonUtils.WaitTime(50, true);
				}
				if (결과YN == false)
				{
					MsgDlgManager.ShowMessageDlg("삭제실패", "화면삭제중 오류가 발생했습니다.");
					return;
				}

				MsgDlgManager.ShowMessageDlg("삭제완료", "선택한 화면이 삭제되었습니다.");
				if (On데이터요청Event != null) On데이터요청Event();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			finally
			{
				if (guid.Equals("") == false)
				{
					MsgDlgManager.CloseInformationDlg(guid);
					guid = "";
					CommonUtils.WaitTime(50, true);
				}
			}
		}

		bool IS_CHECK_ITEM()
		{
			try
			{
				if (cmbDISP_GBN.SelectedIndex < 0)
				{
					MsgDlgManager.ShowMessageDlg("미선택", "화면종류를 선택해주세요.");
					cmbDISP_GBN.Focus();
					return false;
				}

				//if (txtPOS_X.Text.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "가로좌표(X)를 입력해주세요.");
				//	txtPOS_X.Focus();
				//	return false;
				//}

				//if (CommonUtils.IsNumeric(txtPOS_X.Text.Trim()) == false)
				//{
				//	MsgDlgManager.ShowMessageDlg("입력오류", "가로좌표(X)는 숫자로 입력해주세요.");
				//	txtPOS_X.Focus();
				//	return false;
				//}

				//if (txtPOS_Y.Text.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "세로좌표(Y)를 입력해주세요.");
				//	txtPOS_Y.Focus();
				//	return false;
				//}

				//if (CommonUtils.IsNumeric(txtPOS_Y.Text.Trim()) == false)
				//{
				//	MsgDlgManager.ShowMessageDlg("입력오류", "세로좌표(Y)는 숫자로 입력해주세요.");
				//	txtPOS_Y.Focus();
				//	return false;
				//}

				//if (txtSZ_W.Text.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "가로길이를 입력해주세요.");
				//	txtSZ_W.Focus();
				//	return false;
				//}

				//if (CommonUtils.IsNumeric(txtSZ_W.Text.Trim()) == false)
				//{
				//	MsgDlgManager.ShowMessageDlg("입력오류", "가로길이는 숫자로 입력해주세요.");
				//	txtSZ_W.Focus();
				//	return false;
				//}


				//if (txtSZ_H.Text.Trim().Equals("") == true)
				//{
				//	MsgDlgManager.ShowMessageDlg("미입력", "세로길이를 입력해주세요.");
				//	txtSZ_H.Focus();
				//	return false;
				//}

				//if (CommonUtils.IsNumeric(txtSZ_H.Text.Trim()) == false)
				//{
				//	MsgDlgManager.ShowMessageDlg("입력오류", "세로길이는 숫자로 입력해주세요.");
				//	txtSZ_H.Focus();
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
			//string guid = "";
			//try
			//{
			//	Button btn = sender as Button;
			//	if (btn == null) return;

			//	if (dg화면.ItemsSource == null)
			//	{
			//		MsgDlgManager.ShowMessageDlg("미존재", "화면정보가 존재하지않습니다.");
			//		return;
			//	}

			//	List<BIT_DISPLAY> items = dg화면.ItemsSource as List<BIT_DISPLAY>;
			//	if (items == null || items.Count == 0)
			//	{
			//		MsgDlgManager.ShowMessageDlg("미존재", "화면정보가 존재하지않습니다.");
			//		return;
			//	}
			//	guid = MsgDlgManager.ShowInformationDlg("등록중", "화면을 등록중입니다.");

			//	//BITDataManager.BitDisplays= items;
			//	bool 저장YN = false;
			//	foreach (BIT_DISPLAY item in items)
			//	{
			//		if (화면SEQ.Contains(item.SEQ_NO) == false)
			//		{
			//			저장YN = DatabaseManager.INSERT_BIT_DISPLAY(item);
			//		}
			//		else
			//		{
			//			저장YN = DatabaseManager.UPDATE_BIT_DISPLAY(item);
			//		}

			//		if (저장YN == false)
			//		{
			//			if (guid.Equals("") == false)
			//			{
			//				MsgDlgManager.CloseInformationDlg(guid);
			//				guid = "";
			//				CommonUtils.WaitTime(50, true);
			//			}

			//			MsgDlgManager.ShowMessageDlg("오류", "화면저장중 오류가 발생했습니다.");
			//			return;
			//		}
			//	}

			//	if (guid.Equals("") == false)
			//	{
			//		MsgDlgManager.CloseInformationDlg(guid);
			//		guid = "";
			//		CommonUtils.WaitTime(50, true);
			//	}
			//	MsgDlgManager.ShowMessageDlg("화면설정 저장", "화면설정이 저장되었습니다.");
			//	//BITDataManager.BitDisplays = items;
			//	Load설정값();
			//}
			//catch (Exception ee)
			//{
			//	TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			//	System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			//}
			//finally
			//{
			//	if (guid.Equals("") == false)
			//	{
			//		MsgDlgManager.CloseInformationDlg(guid);
			//		guid = "";
			//		CommonUtils.WaitTime(50, true);
			//	}
			//}
		}

		private void dg화면_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			try
			{
				if (dg화면.SelectedIndex < 0) return;
				if (dg화면.ItemsSource == null) return;

				BIT_DISPLAY item = dg화면.SelectedItem as BIT_DISPLAY;
				if (item == null) return;

				cmbDISP_GBN.SelectedValue = item.DISP_GBN;

				txtDISP_NM.Text = item.DISP_NM;
				nPOS_X.Value = item.POS_X;
				nPOS_Y.Value = item.POS_Y;
				nSZ_W.Value = item.SZ_W;
				nSZ_H.Value = item.SZ_H;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		

		//private void btn설정삭제_Click(object sender, RoutedEventArgs e)
		//{
		//	try
		//	{
		//		Button btn = sender as Button;
		//		if (btn == null) return;

		//		bool 삭제YN = MsgDlgManager.ShowQuestionDlg("확인", string.Format("저장된 화면설정을 삭제하시겠습니까?"));
		//		if (삭제YN == false) return;

		//		bool 저장YN = BITDataManager.BitDisplay.Delete();
		//		if (저장YN == false)
		//		{
		//			MsgDlgManager.ShowMessageDlg("화면설정 삭제실패", "화면설정 삭제작업중 에러가 발생했습니다.");
		//			return;
		//		}

		//		MsgDlgManager.ShowMessageDlg("화면설정 삭제", "화면설정이 삭제되었습니다.");

		//		if (BITDataManager.BitDisplay.DISPLAYS == null) BITDataManager.BitDisplay.DISPLAYS = new List<BIT_DISPLAY>();
		//		BITDataManager.BitDisplay.DISPLAYS.Clear();

		//		Load설정값();
		//	}
		//	catch (Exception ee)
		//	{
		//		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//	}
		//}
	}
}