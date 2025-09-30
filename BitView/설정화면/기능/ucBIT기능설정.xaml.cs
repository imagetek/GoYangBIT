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
using SSData.Utils;

namespace BitView
{
	/// <summary>
	/// ucENV2Panel.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ucBIT기능설정 : UserControl
	{
		public ucBIT기능설정()
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
				BITOrderGBN.ItemsSource = null;
				BITOrderGBN.DisplayMemberPath = "NAME";
				BITOrderGBN.SelectedValuePath = "nCODE";

				ArriveSoonGBN.ItemsSource = null;
				ArriveSoonGBN.DisplayMemberPath = "NAME";
				ArriveSoonGBN.SelectedValuePath = "nCODE";
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		void Load기본값()
		{
			try
			{
				ArriveSoonGBN.ItemsSource = CodeManager.Get기초코드(기초코드Type.잠시후도착조건);
				BITOrderGBN.ItemsSource = CodeManager.Get기초코드(기초코드Type.BIT정보정렬방식);

				//rbFontSize1.IsChecked = true;
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
				//설정
				var config = BITDataManager.BitENVConfig.ITEM;
				if (config == null) return;

				nVolume.Value = config.Volume;
				//nDefaultLCDLux.Value = config.DefaultLCDLux;
				BITOrderGBN.SelectedValue = config.BITOrderGBN;
				//switch (config.BITFontSize)
				//{
				//	case 0: rbFontSize0.IsChecked = true; break;
				//	case 1: rbFontSize1.IsChecked = true; break;
				//	case 2: rbFontSize2.IsChecked = true; break;
				//}
				//WebcamSendPeriod.Text = string.Format("{0}", config.WebcamSendPeriod);
				StateSendPeriod.Text = string.Format("{0}", config.StateSendPeriod);
				//ScreenCaptureSendPeriod.Text = string.Format("{0}", config.ScreenCaptureSendPeriod);
				//PromoteSoundPlayYN.IsChecked = config.PromoteSoundPlayYN == 1;
				//TestOperationDisplayYN.IsChecked = config.TestOperationDisplayYN == 1;
				//ForeignDisplayYN.IsChecked = config.ForeignDisplayYN == 1;
				//ForeignDisplayTime.Text = string.Format("{0}", config.ForeignDisplayTime);
				//잠시후 도착
				ArriveSoonGBN.SelectedValue = config.ArriveSoonGBN;
				ArriveSoonTimeGBN.Text = string.Format("{0}", config.ArriveSoonTimeGBN);
				ArriveSoonStationGBN.Text = string.Format("{0}", config.ArriveSoonStationGBN);
				//환경보드
				MonitorOnTime.Text = string.Format("{0}", config.MonitorOnTime);
				MonitorOffTime.Text = string.Format("{0}", config.MonitorOffTime);
				FANMaxTemp.Text = string.Format("{0}", config.FANMaxTemp);
				FANMinTemp.Text = string.Format("{0}", config.FANMinTemp);
				HeaterMaxTemp.Text = string.Format("{0}", config.HeaterMaxTemp);
				HeaterMinTemp.Text = string.Format("{0}", config.HeaterMinTemp);
				ShockDetectValue.Text = string.Format("{0}", config.ShockDetectValue);
				UseDetectSensor.IsChecked = config.UseDetectSensor == 1;
				DetectSensorServiceTime.Text = string.Format("{0}", config.DetectSensorServiceTime);
				////지하철관련 //20221102 bha 
				//SubwayDisplayYN.IsChecked = config.SubwayDisplayYN == 1;
				//SubwayLineNo.Text = string.Format("{0}", config.SubwayLineNo);
				//SubwayStationNo.Text = string.Format("{0}", config.SubwayStationNo);

				//if (config.LuxCount > 0) dg밝기.ItemsSource = config.itemsLux;

				PromotionRefresh.Text = config.PromotionRefreshInterval.ToString();
				BusArrivalRefresh.Text = config.BusArrivalInfoRefreshInterval.ToString();
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
				//if (cmbWEBCAM.SelectedIndex < 0)
				//{
				//	MsgDlgManager.ShowMessageDlg("미선택", "웹캠을 선택해주세요.");
				//	cmbWEBCAM.Focus();
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
				Button btn = sender as Button;
				if (btn == null) return;

				if (IS_CHECK_ITEM() == false) return;

				bool 진행YN = MsgDlgManager.ShowQuestionDlg("저장 확인", "설정한 BIT환경 정보를 저장하시겠습니까?");

				if (진행YN == false) return;

				if (Save설정값() == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("BIT설정 저장실패", "BIT설정 저장작업중 에러가 발생했습니다.");
					return;
				}

				BITDataManager.Refresh설정값(3);

				MsgDlgManager.ShowIntervalInformationDlg("BIT설정 저장", "BIT설정이 저장되었습니다.");
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
				var config = BITDataManager.BitENVConfig.ITEM;

				//config.SEQ_NO++;
				config.BIT_ID = BITDataManager.BIT_ID;
				config.Volume =  Convert.ToInt32(nVolume.Value);
				VolumeService.SetVolume(config.Volume);
				//config.DefaultLCDLux = Convert.ToInt32(nDefaultLCDLux.Value);
				//int FontSize = 0;
				//if (rbFontSize1.IsChecked.Value == true) FontSize = 1; 
				//else if (rbFontSize2.IsChecked.Value == true) FontSize = 2;
				//config.BITFontSize = FontSize;

				BC_CODE itemBITOrderGBN= BITOrderGBN.SelectedItem as BC_CODE;
				config.BITOrderGBN = itemBITOrderGBN.nCODE;

				//config.WebcamSendPeriod = Convert.ToInt32(WebcamSendPeriod.Text);
				config.StateSendPeriod = Convert.ToInt32(StateSendPeriod.Text);
				//config.ScreenCaptureSendPeriod = Convert.ToInt32(ScreenCaptureSendPeriod.Text);
				//config.PromoteSoundPlayYN = PromoteSoundPlayYN.IsChecked.Value == true ? 1 : 0;
				//config.TestOperationDisplayYN = PromoteSoundPlayYN.IsChecked.Value == true ? 1 : 0;
				//config.ForeignDisplayYN = ForeignDisplayYN.IsChecked.Value == true ? 1 : 0;
				//config.ForeignDisplayTime = Convert.ToInt32(ForeignDisplayTime.Text);
				BC_CODE itemArriveSoonGBN = ArriveSoonGBN.SelectedItem as BC_CODE;
				config.ArriveSoonGBN = itemArriveSoonGBN.nCODE;
				config.ArriveSoonTimeGBN = Convert.ToInt32(ArriveSoonTimeGBN.Text);
				config.ArriveSoonStationGBN = Convert.ToInt32(ArriveSoonStationGBN.Text);
				config.MonitorOnTime = string.Format("{0:d4}", Convert.ToInt32(MonitorOnTime.Text));
				config.MonitorOffTime = string.Format("{0:d4}", Convert.ToInt32(MonitorOffTime.Text));
				config.FANMaxTemp = Convert.ToInt32(FANMaxTemp.Text);
				config.FANMinTemp = Convert.ToInt32(FANMinTemp.Text);
				config.HeaterMaxTemp = Convert.ToInt32(HeaterMaxTemp.Text);
				config.HeaterMinTemp = Convert.ToInt32(HeaterMinTemp.Text);
				config.ShockDetectValue = Convert.ToInt32(ShockDetectValue.Text);
				config.DetectSensorServiceTime = Convert.ToInt32(DetectSensorServiceTime.Text);
				config.UseDetectSensor = UseDetectSensor.IsChecked.Value == true ? 1 : 0;
				//config.SubwayDisplayYN = SubwayDisplayYN.IsChecked.Value == true ? 1 : 0;
				//config.SubwayLineNo = Convert.ToInt32(SubwayLineNo.Text);
				//config.SubwayStationNo = Convert.ToInt32(SubwayStationNo.Text);

				//return DatabaseManager.INSERT_BIT_ENV_SETTING(config);
				config.BusArrivalInfoRefreshInterval = Convert.ToInt32(BusArrivalRefresh.Text);
				config.PromotionRefreshInterval = Convert.ToInt32(PromotionRefresh.Text);

				return SettingsManager.SaveBitEnvSettings(config);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
		}

		

		//private void btn설정삭제_Click(object sender, RoutedEventArgs e)
		//{
		//	try
		//	{
		//		Button btn = sender as Button;
		//		if (btn == null) return;

		//		bool 삭제YN = MsgDlgManager.ShowQuestionDlg("확인", string.Format("저장된 BIT기능설정을 삭제하시겠습니까?"));
		//		if (삭제YN == false) return;

		//		bool 저장YN = BITDataManager.BitSystem.Delete();
		//		if (저장YN == false)
		//		{
		//			MsgDlgManager.ShowMessageDlg("BIT기능설정 삭제실패", "BIT기능설정 삭제작업중 에러가 발생했습니다.");
		//			return;
		//		}

		//		MsgDlgManager.ShowMessageDlg("BIT기능설정 삭제", "BIT기능설정이 삭제되었습니다.");

		//		string query = string.Format("SELECT * FROM BIT_ENV_CONFIG WHERE BIT_ID={0] ORDER BY SEQ_NO DESC LIMIT 1", BITDataManager.BitConfig.BASIC.BIT_ID);
		//		List<BIT_ENV_CONFIG> items설정 = DatabaseManager.SELECT_BIT_ENV_CONFIG_BY_QUERY(query);
		//		if (items설정 != null && items설정.Count > 0)
		//		{
		//			BITDataManager.BitSystem.CONFIG = items설정.First();

		//			Load설정값();
		//		}
		//	}
		//	catch (Exception ee)
		//	{
		//		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//	}
		//}

	}
}
