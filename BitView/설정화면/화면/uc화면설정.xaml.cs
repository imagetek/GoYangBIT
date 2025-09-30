using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SSCommonNET;
using SSControlsNET;
using SSData;

namespace BitView
{
	/// <summary>
	/// ucENV2Panel.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class uc화면설정 : UserControl
	{
		public uc화면설정()
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

					Refresh화면();
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

		void Refresh화면()
		{
			string guid = "";
			try
			{				
				this.IsEnabled = false;
				dg화면.ItemsSource = null;
				guid = MsgDlgManager.ShowInformationDlg("조회중", "등록된 화면종류를 검색중입니다.");

				string query = string.Format("SELECT * FROM BIT_DISPLAY WHERE BIT_ID={0}  ORDER BY SEQ_NO", BITDataManager.BIT_ID);

				List<BIT_DISPLAY> items = DatabaseManager.SELECT_BIT_DISPLAY_BY_QUERY(query);
				if (items != null && items.Count > 0)
				{
					dg화면.ItemsSource = items;
				}
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
				this.IsEnabled = true;
			}
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
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
				if (sender is not ComboBox cmb) return;

				if (cmb.SelectedItem is not BC_CODE item) return;

				nPOS_X.Value = 0;
				nPOS_Y.Value = 0;
				
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void btn화면추가_Click(object sender, RoutedEventArgs e)
		{
			string guid = "";
			try
			{
				if (sender is not Button btn) return;

				if (IS_CHECK_ITEM() == false) return;

				if (cmbDISP_GBN.SelectedItem is not BC_CODE item화면)
				{
					MsgDlgManager.ShowIntervalInformationDlg("선택오류", "화면종류선택중 오류가 발생했습니다.");
					return;
				}

				BIT_DISPLAY item = new()
				{
					BIT_ID = BITDataManager.BIT_ID,
					DISP_GBN = item화면.nCODE,
					POS_X = Convert.ToInt32(nPOS_X.Value),
					POS_Y = Convert.ToInt32(nPOS_Y.Value)
				};

				guid = MsgDlgManager.ShowInformationDlg("등록중", "화면정보를 등록중입니다.");

				int retNo = DatabaseManager.INSERT_BIT_DISPLAY(item);
				if (guid.Equals("") == false)
				{
					MsgDlgManager.CloseInformationDlg(guid);
					guid = "";
					CommonUtils.WaitTime(50, true);
				}
				if (retNo <= 0)
				{
					MsgDlgManager.ShowIntervalInformationDlg("등록실패", "화면 등록중 오류가 발생했습니다.");
					return;
				}

				MsgDlgManager.ShowIntervalInformationDlg("등록완료", "화면이 등록되었습니다.");

				item.SEQ_NO = retNo;
				EventManager.RefreshDisplay(1, item);

				Refresh화면();
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

				bool 결과YN = DatabaseManager.UPDATE_BIT_DISPLAY(item);
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

				EventManager.RefreshDisplay(2, item);

				Refresh화면();
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
					MsgDlgManager.ShowIntervalInformationDlg("미선택", "삭제할 화면을 선택해주세요.");
					return;
				}

				BIT_DISPLAY item = dg화면.SelectedItem as BIT_DISPLAY;
				if (item == null)
				{
					MsgDlgManager.ShowIntervalInformationDlg("선택오류", "화면선택중 오류가 발생했습니다.");
					return;
				}

				bool 삭제YN = MsgDlgManager.ShowQuestionDlg("확인", string.Format("화면 {0}를 삭제하시겠습니까?", item.DISP_NM));
				if (삭제YN == false) return;

				guid = MsgDlgManager.ShowInformationDlg("삭제중", "화면정보를 삭제중입니다.");

				bool 결과YN =DatabaseManager.DELETE_BIT_DISPLAY(item);
				if (guid.Equals("") == false)
				{
					MsgDlgManager.CloseInformationDlg(guid);
					guid = "";
					CommonUtils.WaitTime(50, true);
				}
				if ( 결과YN== false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("삭제실패", "화면삭제중 오류가 발생했습니다.");
					return;
				}

				MsgDlgManager.ShowIntervalInformationDlg("삭제완료", "선택한 화면이 삭제되었습니다.");

				EventManager.RefreshDisplay(3, item);

				Refresh화면();
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
					MsgDlgManager.ShowIntervalInformationDlg("미선택", "화면종류를 선택해주세요.");
					cmbDISP_GBN.Focus();
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

		private void dg화면_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			try
			{
				if (dg화면.SelectedIndex < 0) return;
				if (dg화면.ItemsSource == null) return;

				if (dg화면.SelectedItem is not BIT_DISPLAY item) return;

				cmbDISP_GBN.SelectedValue = item.DISP_GBN;

				nPOS_X.Value = item.POS_X;
				nPOS_Y.Value = item.POS_Y;				
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
	}
}