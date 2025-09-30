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
	public partial class uc메인화면 : UserControl
	{
		public uc메인화면()
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

		SQLiteManager mSQL = null;
		private void InitProc()
		{
			try
			{
				CodeManager.Initialize();
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
		
        private void btnDB찾기_Click(object sender, RoutedEventArgs e)
        {
			try
			{
				Microsoft.Win32.OpenFileDialog oFile = new Microsoft.Win32.OpenFileDialog();

				// Set filter for file extension and default file extension 
				oFile.DefaultExt = ".db3";
				oFile.Filter = "DB Files|*.db3;*.db";
				oFile.InitialDirectory = @"C:\PAJUView\DB";
				if (oFile.ShowDialog().Value == true)
				{
					txtDBURL.Text = oFile.FileName;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
        }

        private void btnDB연결_Click(object sender, RoutedEventArgs e)
        {
			try
			{
				if (txtDBURL.Text.Trim().Equals("") == false)
				{
					if (mSQL == null) mSQL = new SQLiteManager();
					mSQL.SetConnectFile(txtDBURL.Text);
				}

				if (mSQL.ISConnect() == true)
				{
					MsgDlgManager.ShowIntervalInformationDlg("확인", "DB에 접속되었습니다.");
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
        }

        private void btn정보가져오기_Click(object sender, RoutedEventArgs e)
        {
			try
			{
				if (mSQL == null || mSQL.ISConnect() == false)
				{
					MsgDlgManager.ShowIntervalInformationDlg("실패", "접속상태가 아닙니다.");
					return;
				}

				List<BIT_SYSTEM> items기본 = mSQL.SELECT_BIT_SYSTEM_BY_QUERY("SELECT * FROM BIT_SYSTEM ORDER BY SEQ_NO DESC");
				if (items기본 != null && items기본.Count > 0)
				{
					_u기본.Load설정값(items기본.First());
				}

				List<TB_SYSTEM> items프로그램= mSQL.SELECT_TB_SYSTEM_BY_QUERY("SELECT * FROM TB_SYSTEM ORDER BY SEQ_NO DESC");
				if (items프로그램 != null && items프로그램.Count > 0)
				{
					_u프로그램.Load설정값(items프로그램.First());
				}

				List<BIT_DISPLAY> items화면= mSQL.SELECT_BIT_DISPLAY_BY_QUERY("SELECT * FROM BIT_DISPLAY ORDER BY SEQ_NO DESC");
				if (items화면 != null && items화면.Count > 0)
				{
					_u화면.Load설정값(items화면);
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

        private bool _u기본_On데이터변경Event(int nGBN, BIT_SYSTEM _item)
        {
			bool bResult = false;
			try
			{
				switch (nGBN)
				{
					case 1: bResult = mSQL.INSERT_BIT_SYSTEM(_item); break;
					case 2: bResult = mSQL.UPDATE_BIT_SYSTEM(_item); break;
					case 3: bResult = mSQL.DELETE_BIT_SYSTEM(_item); break;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				bResult = false;
			}
			return bResult;
        }

        private bool _u프로그램_On데이터변경Event(int nGBN, TB_SYSTEM _item)
        {
			bool bResult = false;
			try
			{
				switch (nGBN)
				{
					case 1: bResult = mSQL.INSERT_TB_SYSTEM(_item); break;
					case 2: bResult = mSQL.UPDATE_TB_SYSTEM(_item); break;
					case 3: bResult = mSQL.DELETE_TB_SYSTEM(_item); break;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				bResult = false;
			}
			return bResult;
		}

        private bool _u화면_On데이터변경Event(int nGBN, BIT_DISPLAY _item)
        {
			bool bResult = false;
			try
			{
				switch (nGBN)
				{
					case 1: bResult = mSQL.INSERT_BIT_DISPLAY(_item) > 0; break;
					case 2: bResult = mSQL.UPDATE_BIT_DISPLAY(_item); break;
					case 3: bResult = mSQL.DELETE_BIT_DISPLAY(_item); break;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				bResult = false;
			}
			return bResult;
		}

        private void _u화면_On데이터요청Event()
        {
			try
            {
				List<BIT_DISPLAY> items화면 = mSQL.SELECT_BIT_DISPLAY_BY_QUERY("SELECT * FROM BIT_DISPLAY ORDER BY SEQ_NO DESC");
				if (items화면 != null && items화면.Count > 0)
				{
					_u화면.Load설정값(items화면);
				}
			}
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

		private void btn패킷분석_Click(object sender, RoutedEventArgs e)
		{
			string mPacket = txt패킷.Text.Trim();

			byte[] items = BitConvertUtils.HexStringToByteArray(mPacket, '-');

			PAJUBISManager manager = new PAJUBISManager();
			manager.TestRecivePacket(items);

		}
	}
}
