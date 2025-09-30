using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SSCommonNET;
using SSControlsNET;
using SSData;

namespace BitView
{
	/// <summary>
	/// ucENV2Panel.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class uc프로그램설정 : UserControl
	{
		public uc프로그램설정()
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
				nLOGSAVE_DAY.Maximum = 180;
				nLOGSAVE_DAY.Minimum = 1;		
				nLOGSAVE_PERCENT.Maximum = 100;				
				nLOGSAVE_PERCENT.Minimum = 1;
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
				var config = BITDataManager.BitConfig;
				if (config == null) return;

				nLOGSAVE_DAY.Value = config.LOGSAVE_DAY;
				nLOGSAVE_PERCENT.Value = config.LOGSAVE_PERCENT;
				vertical.IsChecked = config.IS_VERTICAL_LAYOUT;
				horizontal.IsChecked = !config.IS_VERTICAL_LAYOUT;
				solar.IsChecked = config.IS_SOLAR_MODEL;
				electric.IsChecked = !config.IS_SOLAR_MODEL;
				showRoute.IsChecked = config.SHOW_BUS_ROUTE;
				hideRoute.IsChecked = !config.SHOW_BUS_ROUTE;
				maxRouteCount.Text = config.MAX_ROUTE_COUNT.ToString();
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

				BITDataManager.Refresh설정값(5);

				MsgDlgManager.ShowIntervalInformationDlg("기본설정 저장", "기본설정이 저장되었습니다.");
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private bool Save설정값()
		{
			try
			{			

				TB_SYSTEM tbSystem = SettingsManager.GetTbSystemSettings();
				tbSystem.LOGSAVE_DAY = Convert.ToInt32(nLOGSAVE_DAY.Value);
				tbSystem.LOGSAVE_PERCENT = Convert.ToInt32(nLOGSAVE_PERCENT.Value);
				tbSystem.IS_VERTICAL_LAYOUT = (bool)vertical.IsChecked;
				tbSystem.IS_SOLAR_MODEL = (bool)solar.IsChecked;
				tbSystem.SHOW_BUS_ROUTE = (bool)showRoute.IsChecked;
				tbSystem.MAX_ROUTE_COUNT = int.TryParse(maxRouteCount.Text, out int maxCount) ? maxCount : 3; 
				return SettingsManager.SaveTbSystemSettings(tbSystem);
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

		private void RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			RadioButton radioButton = (RadioButton)sender;
			
			
		}
	}
}
