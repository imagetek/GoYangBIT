using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommon;

namespace SSWebcamPlayer
{
	public class SQLiteManager
	{
		SQLiteHelper mSQL = null;
		string ConnectString = "";
		public bool SetConnectString(string IP, int PORT, string DATABASE, string USER_ID, string USER_PASS)
		{
			try
			{
				ConnectString = string.Format("UserID={0};Password={1};Database={2};Datasource={3};Port={4};", USER_ID, USER_PASS, DATABASE, IP, PORT);

				return true;
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
		}

		public bool SetConnectFile(string FILE_NM)
		{
			try
			{
				ConnectString = FILE_NM;

				return true;
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
		}

		#region DB연결 해제등 기본

		bool Connect()
		{
			try
			{
				if (ConnectString.Equals("") == true)
				{
					if (System.IO.File.Exists(DataManager.ServerInfo.DB_URL) == false) return false;

					SetConnectFile(DataManager.ServerInfo.DB_URL);
				}

				if (mSQL == null)
				{
					mSQL = new SQLiteHelper(ConnectString);
				}

				if (mSQL.Connect() == false) return false;

				return true;
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
		}

		public bool ISConnect()
		{
			return Connect();
		}

		bool DisConnect()
		{
			try
			{
				if (mSQL != null)
				{
					mSQL.DisConnect();
					mSQL = null;
				}

				GC.Collect();

				return true;
			}
			catch (Exception ee)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
		}

		#endregion

		public List<BIT_SYSTEM> SELECT_BIT_SYSTEM_BY_QUERY(string query)
		{
			try
			{
				if (Connect() == false)
				{
					return null;
				}

				List<BIT_SYSTEM> items = new List<BIT_SYSTEM>();
				System.Data.DataTable _dt = mSQL.ExecuteDataTableByQuery(query);
				if (_dt != null)
				{
					foreach (System.Data.DataRow row in _dt.Rows)
					{
						BIT_SYSTEM item = new BIT_SYSTEM();
						try
						{
							item.SEQ_NO = ConvertUtils.IntByObject(row["SEQ_NO"]);

							item.BIT_ID = ConvertUtils.IntByObject(row["BIT_ID"]);
							item.MOBILE_NO = ConvertUtils.IntByObject(row["MOBILE_NO"]);
							item.STATION_NM = ConvertUtils.StringByObject(row["STATION_NM"]);

							//item.DB_GBN = ConvertUtils.IntByObject(row["DB_GBN"]);
							//item.DB_IP = ConvertUtils.StringByObject(row["DB_IP"]);
							//item.DB_URL = ConvertUtils.StringByObject(row["DB_URL"]);
							//item.DB_PORT = ConvertUtils.IntByObject(row["DB_PORT"]);
							//item.DB_USERID = ConvertUtils.StringByObject(row["DB_USERID"]);
							//item.DB_USERPWD = ConvertUtils.StringByObject(row["DB_USERPWD"]);

							item.SERVER_URL = ConvertUtils.StringByObject(row["SERVER_URL"]);
							item.SERVER_PORT = ConvertUtils.StringByObject(row["SERVER_PORT"]);

							item.FTP_GBN = ConvertUtils.IntByObject(row["FTP_GBN"]);
							item.FTP_IP = ConvertUtils.StringByObject(row["FTP_IP"]);
							item.FTP_PORT = ConvertUtils.IntByObject(row["FTP_PORT"]);
							item.FTP_USERID = ConvertUtils.StringByObject(row["FTP_USERID"]);
							item.FTP_USERPWD = ConvertUtils.StringByObject(row["FTP_USERPWD"]);

							item.HTTP_URL = ConvertUtils.StringByObject(row["HTTP_URL"]);
							item.HTTP_PORT = ConvertUtils.IntByObject(row["HTTP_PORT"]);

							item.ENV_PORT_NM = ConvertUtils.StringByObject(row["ENV_PORT_NM"]);
							item.ENV_BAUD_RATE = ConvertUtils.IntByObject(row["ENV_BAUD_RATE"]);
							//item.WEBCAM_NM = ConvertUtils.StringByObject(row["WEBCAM_NM"]);
							//item.SHOCKCAM_NM = ConvertUtils.StringByObject(row["SHOCKCAM_NM"]);
							item.CAM_NO1 = ConvertUtils.IntByObject(row["CAM_NO1"]);
							item.CAM_NO1_ROTATE = ConvertUtils.IntByObject(row["CAM_NO1_ROTATE"]);
							item.CAM_NO2 = ConvertUtils.IntByObject(row["CAM_NO2"]);
							item.CAM_NO2_ROTATE = ConvertUtils.IntByObject(row["CAM_NO2_ROTATE"]);

							item.REGDATE = ConvertUtils.DateTimeByObject(row["REGDATE"]);
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
							item = null;
						}
						if (item == null) continue;
						items.Add(item);
					}
				}

				if (items != null && items.Count > 0)
				{
					return items;
				}

				return null;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
			finally
			{
				DisConnect();
			}
		}
	}
}
