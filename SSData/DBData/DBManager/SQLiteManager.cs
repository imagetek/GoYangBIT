using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;

namespace SSData
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

		public void InitializeDatabase()
		{
			try
			{
				if (Connect() == false) return;

				List<string> items테이블 = new List<string>();
				string query = "SELECT name FROM sqlite_schema WHERE  type ='table' AND name NOT LIKE 'sqlite_%';";
				System.Data.DataTable _dt = mSQL.ExecuteDataTableByQuery(query);
				if (_dt != null)
				{
					foreach (System.Data.DataRow row in _dt.Rows)
					{
						string itemTBL = ConvertUtils.StringByObject(row["name"]);
						if (itemTBL.Equals("") == true) continue;
						items테이블.Add(itemTBL);
					}
				}

				//DB테이블체크
				#region TB_SYSTEM

				string TBL_NM = "TB_SYSTEM";
				if (items테이블.Contains(TBL_NM) == true)
				{
					List<string> items컬럼 = new List<string>();
					query = string.Format("PRAGMA table_info('{0}');", TBL_NM);
					_dt.Clear();
					_dt = mSQL.ExecuteDataTableByQuery(query);
					if (_dt != null)
					{
						foreach (System.Data.DataRow row in _dt.Rows)
						{
							string itemCol = ConvertUtils.StringByObject(row["name"]);
							if (itemCol.Equals("") == true) continue;
							items컬럼.Add(itemCol);
						}
					}

					string ColNM = "ENV_USE_FAN_MANUAL";
					if (items컬럼.Contains(ColNM) == false)
					{
						string querySQL = string.Format("ALTER TABLE {0} ADD COLUMN {1} integer DEFAULT 0;", TBL_NM, ColNM);
						if (mSQL.ExecuteNonQuery(querySQL) != -1)
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] TB_SYSTEM의 컬럼({0})추가 완료", ColNM));
						}
						else
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] TB_SYSTEM의 컬럼({0})추가실패", ColNM));
						}
					}

					ColNM = "ENV_USE_HEATER_MANUAL";
					if (items컬럼.Contains(ColNM) == false)
					{
						string querySQL = string.Format("ALTER TABLE {0} ADD COLUMN {1} integer DEFAULT 0;", TBL_NM, ColNM);
						if (mSQL.ExecuteNonQuery(querySQL) != -1)
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] TB_SYSTEM의 컬럼({0})추가 완료", ColNM));
						}
						else
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] TB_SYSTEM의 컬럼({0})추가실패", ColNM));
						}
					}

					ColNM = "SUBWAY_DISPLAY_YN";
					if (items컬럼.Contains(ColNM) == false)
					{
						string querySQL = string.Format("ALTER TABLE {0} ADD COLUMN {1} integer DEFAULT 0;", TBL_NM, ColNM);
						if (mSQL.ExecuteNonQuery(querySQL) != -1)
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] TB_SYSTEM의 컬럼({0})추가 완료", ColNM));
						}
						else
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] TB_SYSTEM의 컬럼({0})추가실패", ColNM));
						}
					}

					ColNM = "SUBWAY_LINE_NO";
					if (items컬럼.Contains(ColNM) == false)
					{
						string querySQL = string.Format("ALTER TABLE {0} ADD COLUMN {1} integer DEFAULT 0;", TBL_NM, ColNM);
						if (mSQL.ExecuteNonQuery(querySQL) != -1)
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] TB_SYSTEM의 컬럼({0})추가 완료", ColNM));
						}
						else
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] TB_SYSTEM의 컬럼({0})추가실패", ColNM));
						}
					}

					ColNM = "SUBWAY_STATION_NO";
					if (items컬럼.Contains(ColNM) == false)
					{
						string querySQL = string.Format("ALTER TABLE {0} ADD COLUMN {1} integer DEFAULT 0;", TBL_NM, ColNM);
						if (mSQL.ExecuteNonQuery(querySQL) != -1)
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] TB_SYSTEM의 컬럼({0})추가 완료", ColNM));
						}
						else
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] TB_SYSTEM의 컬럼({0})추가실패", ColNM));
						}
					}

					//20221129 bha 
					ColNM = "SHOCK_DETECT_NO";
					if (items컬럼.Contains(ColNM) == false)
					{
						string querySQL = string.Format("ALTER TABLE {0} ADD COLUMN {1} integer DEFAULT 0;", TBL_NM, ColNM);
						if (mSQL.ExecuteNonQuery(querySQL) != -1)
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] TB_SYSTEM의 컬럼({0})추가 완료", ColNM));
						}
						else
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] TB_SYSTEM의 컬럼({0})추가실패", ColNM));
						}
					}
				}

				#endregion

				#region BIT_SYTEM

				TBL_NM = "BIT_SYSTEM";
				if (items테이블.Contains(TBL_NM) == true)
				{
					List<string> items컬럼 = new List<string>();
					query = string.Format("PRAGMA table_info('{0}');", TBL_NM);
					_dt.Clear();
					_dt = mSQL.ExecuteDataTableByQuery(query);
					if (_dt != null)
					{
						foreach (System.Data.DataRow row in _dt.Rows)
						{
							string itemCol = ConvertUtils.StringByObject(row["name"]);
							if (itemCol.Equals("") == true) continue;
							items컬럼.Add(itemCol);
						}
					}

					string ColNM = "ENV_GBN";
					if (items컬럼.Contains(ColNM) == false)
					{
						string querySQL = string.Format("ALTER TABLE {0} ADD COLUMN {1} integer DEFAULT 0;", TBL_NM, ColNM);
						if (mSQL.ExecuteNonQuery(querySQL) != -1)
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] BIT_SYSTEM의 컬럼({0})추가 완료", ColNM));
						}
						else
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] BIT_SYSTEM의 컬럼({0})추가실패", ColNM));
						}
					}

					ColNM = "SERVER_TYPE";
					if (items컬럼.Contains(ColNM) == false)
					{
						string querySQL = string.Format("ALTER TABLE {0} ADD COLUMN {1} integer DEFAULT 1;", TBL_NM, ColNM);
						if (mSQL.ExecuteNonQuery(querySQL) != -1)
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] BIT_SYSTEM의 컬럼({0})추가 완료", ColNM));
						}
						else
						{
							TraceManager.AddInfoLog(string.Format("[SQLite] BIT_SYSTEM의 컬럼({0})추가실패", ColNM));
						}
					}

					#endregion
					////20220728 BHA 
					//ColNM = "ENV_GBN";
					//if (items컬럼.Contains(ColNM) == false)
					//{
					//	string querySQL = string.Format("ALTER TABLE {0} ADD COLUMN {1} integer DEFAULT 0;", TBL_NM, ColNM);
					//	if (mSQL.ExecuteNonQuery(querySQL) != -1)
					//	{
					//		TraceManager.AddInfoLog(string.Format("[SQLite] TB_SYSTEM의 컬럼({0})추가 완료", ColNM));
					//	}
					//	else
					//	{
					//		TraceManager.AddInfoLog(string.Format("[SQLite] TB_SYSTEM의 컬럼({0})추가실패", ColNM));
					//	}
					//}
				}
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			finally
			{
				DisConnect();
			}
		}
		

		#region BC_CODE 기초정보

		public List<BC_CODE> SELECT_BC_CODE_BY_QUERY(string query)
		{
			try
			{
				if (Connect() == false)
				{
					return null;
				}

				List<BC_CODE> items = new List<BC_CODE>();
				System.Data.DataTable _dt = mSQL.ExecuteDataTableByQuery(query);
				if (_dt != null)
				{
					foreach (System.Data.DataRow row in _dt.Rows)
					{
						BC_CODE item = MatchManager.BC_CODE_BY_ROW(row);
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

		public bool INSERT_BC_CODE(BC_CODE item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = "INSERT INTO BC_CODE ";
				query += "(CD_GBN_NO,CD_GBN_NM,CODE,NAME,S_NM,";
				query += "DISP_YN,USE_YN) ";
				query += "VALUES ";
				query += "(@CD_GBN_NO,@CD_GBN_NM,@CODE,@NAME,@S_NM,";
				query += "@DISP_YN,@USE_YN) ";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CD_GBN_NO", item.CD_GBN_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CD_GBN_NM", item.CD_GBN_NM));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CODE", item.CODE));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@NAME", item.NAME));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@S_NM", item.S_NM));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_YN", item.DISP_YN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@USE_YN", item.USE_YN));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret == -1) return false;

				return true;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool BULK_INSERT_BC_CODE(List<BC_CODE> items)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				mSQL.SetTransaction();

				foreach (BC_CODE item in items)
				{
					string query = "INSERT INTO BC_CODE ";
					query += "(CD_GBN_NO,CD_GBN_NM,CODE,NAME,S_NM,";
					query += "DISP_YN,USE_YN) ";
					query += "VALUES ";
					query += "(@CD_GBN_NO,@CD_GBN_NM,@CODE,@NAME,@S_NM,";
					query += "@DISP_YN,@USE_YN) ";

					List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

					parameters.Add(new System.Data.SQLite.SQLiteParameter("@CD_GBN_NO", item.CD_GBN_NO));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@CD_GBN_NM", item.CD_GBN_NM));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@CODE", item.CODE));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@NAME", item.NAME));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@S_NM", item.S_NM));

					parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_YN", item.DISP_YN));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@USE_YN", item.USE_YN));

					if (mSQL.ExecuteNonQuery(query, parameters) == -1)
					{
						mSQL.SetRollBack();
						return false;
					}
				}
				return mSQL.SetCommit();
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool UPDATE_BC_CODE(BC_CODE item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = "UPDATE BC_CODE SET ";
				query += "NAME=@NAME,S_NM=@S_NM,DISP_YN=@DISP_YN,USE_YN=@USE_YN ";
				query += "WHERE CD_GBN_NO=@CD_GBN_NO AND CODE=@CODE ";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@NAME", item.NAME));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@S_NM", item.S_NM));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_YN", item.DISP_YN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@USE_YN", item.USE_YN));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CD_GBN_NO", item.CD_GBN_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CODE", item.CODE));


				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret != -1) return true;

				return false;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool DELETE_BC_CODE(BC_CODE item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = "DELETE FROM BC_CODE WHERE CD_GBN_NO=@CD_GBN_NO AND CODE=@CODE";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CD_GBN_NO", item.CD_GBN_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CODE", item.CODE));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret != -1) return true;

				return false;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		#endregion

		#region BIT_ENV_SETTING 기초정보

		public List<BIT_ENV_SETTING> SELECT_BIT_ENV_SETTING_BY_QUERY(string query)
		{
			try
			{
				if (Connect() == false)
				{
					Console.WriteLine("SQLiteDB 접속실패");
					return null;
				}
				Console.WriteLine("SQLiteDB 접속");

				List<BIT_ENV_SETTING> items = new List<BIT_ENV_SETTING>();
				System.Data.DataTable _dt = mSQL.ExecuteDataTableByQuery(query);
				if (_dt != null)
				{
					foreach (System.Data.DataRow row in _dt.Rows)
					{
						BIT_ENV_SETTING item = MatchManager.BIT_ENV_SETTING_BY_ROW(row);
						if (item == null) continue;
						items.Add(item);
					}
				}

				if (items != null && items.Count > 0)
				{
					return items;
				}

				Console.WriteLine("[SQLiteDB] q:{0} 데이터미존재", query);

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

		int GET_MAX_NO_BIT_ENV_SETTING()
		{
			try
			{
				string query = "SELECT MAX(SEQ_NO) FROM BIT_ENV_SETTING";

				int result = mSQL.ExecuteScalarByQuery(query);
				if (result == -1) return 1;

				return result + 1;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return 1;
			}
		}

		public bool INSERT_BIT_ENV_SETTING(BIT_ENV_SETTING item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				int MAX_NO = GET_MAX_NO_BIT_ENV_SETTING();

				string query = "INSERT INTO BIT_ENV_SETTING ";
				query += "(SEQ_NO,BIT_ID,Volume,ArriveSoonGBN,ArriveSoonTimeGBN,ArriveSoonStationGBN,";
				query += "MonitorOnTime,MonitorOffTime,DefaultLCDLux,LuxCount,StateSendPeriod,";
				query += "WebcamSendPeriod,ScreenCaptureSendPeriod,BITOrderGBN,UseDetectSensor,DetectSensorServiceTime,";
				query += "FANMaxTemp,FANMinTemp,HeaterMaxTemp,HeaterMinTemp,SubwayDisplayYN,";
				query += "SubwayLineNo,SubwayStationNo,ForeignDisplayYN,ForeignDisplayTime,ShockDetectValue,";
				query += "StationMobileNo,StationName,PromoteSoundPlayYN,BITFontSize,TestOperationDisplayYN,";
				query += "Reserve1,REGDATE) ";
				query += "VALUES ";
				query += "(@SEQ_NO,@BIT_ID,@Volume,@ArriveSoonGBN,@ArriveSoonTimeGBN,@ArriveSoonStationGBN,";
				query += "@MonitorOnTime,@MonitorOffTime,@DefaultLCDLux,@LuxCount,@StateSendPeriod,";
				query += "@WebcamSendPeriod,@ScreenCaptureSendPeriod,@BITOrderGBN,@UseDetectSensor,@DetectSensorServiceTime,";
				query += "@FANMaxTemp,@FANMinTemp,@HeaterMaxTemp,@HeaterMinTemp,@SubwayDisplayYN,";
				query += "@SubwayLineNo,@SubwayStationNo,@ForeignDisplayYN,@ForeignDisplayTime,@ShockDetectValue,";
				query += "@StationMobileNo,@StationName,@PromoteSoundPlayYN,@BITFontSize,@TestOperationDisplayYN,";
				query += "@Reserve1,@REGDATE) ";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", MAX_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@BIT_ID", item.BIT_ID));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@Volume", item.Volume));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ArriveSoonGBN", item.ArriveSoonGBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ArriveSoonTimeGBN", item.ArriveSoonTimeGBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ArriveSoonStationGBN", item.ArriveSoonStationGBN));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@MonitorOnTime", item.MonitorOnTime));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@MonitorOffTime", item.MonitorOffTime));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@DefaultLCDLux", item.DefaultLCDLux));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LuxCount", item.LuxCount));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@StateSendPeriod", item.StateSendPeriod));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@WebcamSendPeriod", item.WebcamSendPeriod));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ScreenCaptureSendPeriod", item.ScreenCaptureSendPeriod));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@BITOrderGBN", item.BITOrderGBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@UseDetectSensor", item.UseDetectSensor));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@DetectSensorServiceTime", item.DetectSensorServiceTime));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FANMaxTemp", item.FANMaxTemp));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FANMinTemp", item.FANMinTemp));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@HeaterMaxTemp", item.HeaterMaxTemp));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@HeaterMinTemp", item.HeaterMinTemp));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SubwayDisplayYN", item.SubwayDisplayYN));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SubwayLineNo", item.SubwayLineNo));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SubwayStationNo", item.SubwayStationNo));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ForeignDisplayYN", item.ForeignDisplayYN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ForeignDisplayTime", item.ForeignDisplayTime));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ShockDetectValue", item.ShockDetectValue));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@StationMobileNo", item.StationMobileNo));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@StationName", item.StationName));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@PromoteSoundPlayYN", item.PromoteSoundPlayYN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@BITFontSize", item.BITFontSize));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TestOperationDisplayYN", item.TestOperationDisplayYN));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@Reserve1", item.Reserve1));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@REGDATE", DateTime.Now));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret == -1) return false;

				//조도값입력
				if (item.LuxCount > 0 && item.itemsLux.Count > 0)
				{
					return BULK_INSERT_BIT_ENV_LUX(item.itemsLux, MAX_NO);
				}

				return true;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		//public bool BULK_INSERT_BIT_ENV_SETTING(List<BIT_ENV_SETTING> items)
		//{
		//	try
		//	{
		//		if (Connect() == false)
		//		{
		//			return false;
		//		}

		//		mSQL.SetTransaction();

		//		foreach (BIT_ENV_SETTING item in items)
		//		{
		//			string query = "INSERT INTO BIT_ENV_SETTING ";
		//			query += "(CD_GBN_NO,CD_GBN_NM,CODE,NAME,S_NM,";
		//			query += "DISP_YN,USE_YN) ";
		//			query += "VALUES ";
		//			query += "(@CD_GBN_NO,@CD_GBN_NM,@CODE,@NAME,@S_NM,";
		//			query += "@DISP_YN,@USE_YN) ";

		//			List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

		//			parameters.Add(new System.Data.SQLite.SQLiteParameter("@CD_GBN_NO", item.CD_GBN_NO));
		//			parameters.Add(new System.Data.SQLite.SQLiteParameter("@CD_GBN_NM", item.CD_GBN_NM));
		//			parameters.Add(new System.Data.SQLite.SQLiteParameter("@CODE", item.CODE));
		//			parameters.Add(new System.Data.SQLite.SQLiteParameter("@NAME", item.NAME));
		//			parameters.Add(new System.Data.SQLite.SQLiteParameter("@S_NM", item.S_NM));

		//			parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_YN", item.DISP_YN));
		//			parameters.Add(new System.Data.SQLite.SQLiteParameter("@USE_YN", item.USE_YN));

		//			if (mSQL.ExecuteNonQuery(query, parameters) == -1)
		//			{
		//				mSQL.SetRollBack();
		//				return false;
		//			}
		//		}
		//		return mSQL.SetCommit();
		//	}
		//	catch (Exception ee)
		//	{
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		return false;
		//	}
		//	finally
		//	{
		//		DisConnect();
		//	}
		//}

		//public bool UPDATE_BIT_ENV_SETTING(BIT_ENV_SETTING item)
		//{
		//	try
		//	{
		//		if (Connect() == false)
		//		{
		//			return false;
		//		}

		//		string query = "UPDATE BIT_ENV_SETTING SET ";
		//		query += "NAME=@NAME,S_NM=@S_NM,DISP_YN=@DISP_YN,USE_YN=@USE_YN ";
		//		query += "WHERE CD_GBN_NO=@CD_GBN_NO AND CODE=@CODE ";

		//		List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

		//		parameters.Add(new System.Data.SQLite.SQLiteParameter("@NAME", item.NAME));
		//		parameters.Add(new System.Data.SQLite.SQLiteParameter("@S_NM", item.S_NM));
		//		parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_YN", item.DISP_YN));
		//		parameters.Add(new System.Data.SQLite.SQLiteParameter("@USE_YN", item.USE_YN));

		//		parameters.Add(new System.Data.SQLite.SQLiteParameter("@CD_GBN_NO", item.CD_GBN_NO));
		//		parameters.Add(new System.Data.SQLite.SQLiteParameter("@CODE", item.CODE));


		//		int ret = mSQL.ExecuteNonQuery(query, parameters);
		//		if (ret != -1) return true;

		//		return false;
		//	}
		//	catch (Exception ee)
		//	{
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		return false;
		//	}
		//	finally
		//	{
		//		DisConnect();
		//	}
		//}

		public bool DELETE_BIT_ENV_SETTING(BIT_ENV_SETTING item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = "DELETE FROM BIT_ENV_SETTING WHERE SEQ_NO=@SEQ_NO ";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", item.SEQ_NO));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret != -1) return true;

				return false;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		#endregion

		#region BIT_ENV_LUX 기초정보

		public List<BIT_ENV_LUX> SELECT_BIT_ENV_LUX_BY_QUERY(string query)
		{
			try
			{
				if (Connect() == false)
				{
					return null;
				}

				List<BIT_ENV_LUX> items = new List<BIT_ENV_LUX>();
				System.Data.DataTable _dt = mSQL.ExecuteDataTableByQuery(query);
				if (_dt != null)
				{
					foreach (System.Data.DataRow row in _dt.Rows)
					{
						BIT_ENV_LUX item = MatchManager.BIT_ENV_LUX_BY_ROW(row);
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

		int GET_MAX_NO_BIT_ENV_LUX()
		{
			try
			{
				string query = "SELECT MAX(SEQ_NO) FROM BIT_ENV_LUX";

				int result = mSQL.ExecuteScalarByQuery(query);
				if (result == -1) return 1;

				return result + 1;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return 1;
			}
		}

		public bool INSERT_BIT_ENV_LUX(BIT_ENV_LUX item, int ENV_CONFIG_NO)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				int MAX_NO = GET_MAX_NO_BIT_ENV_LUX();

				string query = "INSERT INTO BIT_ENV_LUX ";
				query += "(SEQ_NO,BIT_ID,ENV_CONFIG_NO,S_TIME,E_TIME,LUX,";
				query += "REGDATE) ";
				query += "VALUES ";
				query += "(@SEQ_NO,@BIT_ID,@ENV_CONFIG_NO,@S_TIME,@E_TIME,@LUX,";
				query += "@REGDATE) ";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", MAX_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@BIT_ID", item.BIT_ID));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ENV_CONFIG_NO", item.ENV_CONFIG_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@S_TIME", item.S_TIME));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@E_TIME", item.E_TIME));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LUX", item.LUX));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@REGDATE", DateTime.Now));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret == -1) return false;

				return true;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool BULK_INSERT_BIT_ENV_LUX(List<BIT_ENV_LUX> items, int ConfigNo)
		{
			try
			{
				int MAX_NO = GET_MAX_NO_BIT_ENV_LUX();

				mSQL.SetTransaction();

				foreach (BIT_ENV_LUX item in items)
				{
					string query = "INSERT INTO BIT_ENV_LUX ";
					query += "(SEQ_NO,BIT_ID,ENV_CONFIG_NO,S_TIME,E_TIME,LUX,";
					query += "REGDATE) ";
					query += "VALUES ";
					query += "(@SEQ_NO,@BIT_ID,@ENV_CONFIG_NO,@S_TIME,@E_TIME,@LUX,";
					query += "@REGDATE) ";

					List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

					parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", MAX_NO++));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@BIT_ID", item.BIT_ID));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@ENV_CONFIG_NO", item.ENV_CONFIG_NO));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@S_TIME", item.S_TIME));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@E_TIME", item.E_TIME));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@LUX", item.LUX));

					parameters.Add(new System.Data.SQLite.SQLiteParameter("@REGDATE", DateTime.Now));


					if (mSQL.ExecuteNonQuery(query, parameters) == -1)
					{
						mSQL.SetRollBack();
						return false;
					}
				}
				return mSQL.SetCommit();
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			//finally
			//{
			//	DisConnect();
			//}
		}

		//public bool UPDATE_BIT_ENV_LUX(BIT_ENV_LUX item)
		//{
		//	try
		//	{
		//		if (Connect() == false)
		//		{
		//			return false;
		//		}

		//		string query = "UPDATE BIT_ENV_LUX SET ";
		//		query += "NAME=@NAME,S_NM=@S_NM,DISP_YN=@DISP_YN,USE_YN=@USE_YN ";
		//		query += "WHERE CD_GBN_NO=@CD_GBN_NO AND CODE=@CODE ";

		//		List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

		//		parameters.Add(new System.Data.SQLite.SQLiteParameter("@NAME", item.NAME));
		//		parameters.Add(new System.Data.SQLite.SQLiteParameter("@S_NM", item.S_NM));
		//		parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_YN", item.DISP_YN));
		//		parameters.Add(new System.Data.SQLite.SQLiteParameter("@USE_YN", item.USE_YN));

		//		parameters.Add(new System.Data.SQLite.SQLiteParameter("@CD_GBN_NO", item.CD_GBN_NO));
		//		parameters.Add(new System.Data.SQLite.SQLiteParameter("@CODE", item.CODE));


		//		int ret = mSQL.ExecuteNonQuery(query, parameters);
		//		if (ret != -1) return true;

		//		return false;
		//	}
		//	catch (Exception ee)
		//	{
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		return false;
		//	}
		//	finally
		//	{
		//		DisConnect();
		//	}
		//}

		public bool DELETE_BIT_ENV_LUX(BIT_ENV_LUX item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = "DELETE FROM BIT_ENV_LUX WHERE SEQ_NO=@SEQ_NO ";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", item.SEQ_NO));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret != -1) return true;

				return false;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool DELETE_BIT_ENV_LUX(int EVN_CONFIG_NO)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = "DELETE FROM BIT_ENV_LUX WHERE EVN_CONFIG_NO=@EVN_CONFIG_NO ";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@EVN_CONFIG_NO", EVN_CONFIG_NO));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret != -1) return true;

				return false;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		#endregion

		#region BIT_DISPLAY 기초정보

		public List<BIT_DISPLAY> SELECT_BIT_DISPLAY_BY_QUERY(string query)
		{
			try
			{
				if (Connect() == false)
				{
					return null;
				}

				List<BIT_DISPLAY> items = new List<BIT_DISPLAY>();
				System.Data.DataTable _dt = mSQL.ExecuteDataTableByQuery(query);
				if (_dt != null)
				{
					foreach (System.Data.DataRow row in _dt.Rows)
					{
						BIT_DISPLAY item = MatchManager.BIT_DISPLAY_BY_ROW(row);
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

		int GET_MAX_NO_BIT_DISPLAY()
		{
			try
			{
				string query = "SELECT MAX(SEQ_NO) FROM BIT_DISPLAY";

				int result = mSQL.ExecuteScalarByQuery(query);
				if (result == -1) return 1;

				return result + 1;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return 1;
			}
		}

		public int INSERT_BIT_DISPLAY(BIT_DISPLAY item)
		{
			try
			{
				if (Connect() == false)
				{
					return -1;
				}

				int MAX_NO = GET_MAX_NO_BIT_DISPLAY();

				string query = "INSERT INTO BIT_DISPLAY ";
				query += "(SEQ_NO,BIT_ID,DISP_GBN,DISP_NM,POS_X,";
				query += "POS_Y,SZ_W,SZ_H,REGDATE) ";
				query += "VALUES ";
				query += "(@SEQ_NO,@BIT_ID,@DISP_GBN,@DISP_NM,@POS_X,";
				query += "@POS_Y,@SZ_W,@SZ_H,@REGDATE) ";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", MAX_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@BIT_ID", item.BIT_ID));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_GBN", item.DISP_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_NM", item.DISP_NM));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@POS_X", item.POS_X));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@POS_Y", item.POS_Y));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SZ_W", item.SZ_W));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SZ_H", item.SZ_H));				
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@REGDATE", DateTime.Now));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret == -1) return -1;

				return MAX_NO;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return -9;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool BULK_INSERT_BIT_DISPLAY(List<BIT_DISPLAY> items)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				int MAX_NO = GET_MAX_NO_BIT_DISPLAY();

				mSQL.SetTransaction();

				foreach (BIT_DISPLAY item in items)
				{
					string query = "INSERT INTO BIT_DISPLAY ";
					query += "(SEQ_NO,BIT_ID,DISP_GBN,DISP_NM,POS_X,";
					query += "POS_Y,SZ_W,SZ_H,REGDATE) ";
					query += "VALUES ";
					query += "(@SEQ_NO,@BIT_ID,@DISP_GBN,@DISP_NM,@POS_X,";
					query += "@POS_Y,@SZ_W,@SZ_H,@REGDATE) ";

					List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

					parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", MAX_NO++));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@BIT_ID", item.BIT_ID));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_GBN", item.DISP_GBN));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_NM", item.DISP_NM));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@POS_X", item.POS_X));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@POS_Y", item.POS_Y));

					parameters.Add(new System.Data.SQLite.SQLiteParameter("@SZ_W", item.SZ_W));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@SZ_H", item.SZ_H));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@REGDATE", DateTime.Now));

					//int ret = mSQL.ExecuteNonQuery(query, parameters);

					if (mSQL.ExecuteNonQuery(query, parameters) == -1)
					{
						mSQL.SetRollBack();
						return false;
					}
				}
				return mSQL.SetCommit();
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool UPDATE_BIT_DISPLAY(BIT_DISPLAY item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = "UPDATE BIT_DISPLAY SET ";
				query += "DISP_GBN=@DISP_GBN,DISP_NM=@DISP_NM,POS_X=@POS_X,POS_Y=@POS_Y,SZ_W=@SZ_W,";
				query += "SZ_H=@SZ_H ";
				query += "WHERE SEQ_NO=@SEQ_NO";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_GBN", item.DISP_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_NM", item.DISP_NM));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@POS_X", item.POS_X));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@POS_Y", item.POS_Y));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SZ_W", item.SZ_W));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SZ_H", item.SZ_H));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", item.SEQ_NO));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret != -1) return true;

				return false;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool DELETE_BIT_DISPLAY(BIT_DISPLAY item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = "DELETE FROM BIT_DISPLAY WHERE SEQ_NO=@SEQ_NO ";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", item.SEQ_NO));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret != -1) return true;

				return false;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}


		#endregion

		#region LED_ITEM 기초정보

		public List<LED_ITEM> SELECT_LED_ITEM_BY_QUERY(string query)
		{
			try
			{
				if (Connect() == false)
				{
					return null;
				}

				List<LED_ITEM> items = new List<LED_ITEM>();
				System.Data.DataTable _dt = mSQL.ExecuteDataTableByQuery(query);
				if (_dt != null)
				{
					foreach (System.Data.DataRow row in _dt.Rows)
					{
						LED_ITEM item = MatchManager.LED_ITEM_BY_ROW(row);
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

		int GET_MAX_NO_LED_ITEM(string TABLE_NM)
		{
			try
			{
				string query = string.Format("SELECT MAX(SEQ_NO) FROM {0}", TABLE_NM);

				int result = mSQL.ExecuteScalarByQuery(query);
				if (result == -1) return 1;

				return result + 1;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return 1;
			}
		}

		public bool INSERT_LED_ITEM(LED_ITEM item, string TABLE_NM)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				int MAX_NO = GET_MAX_NO_LED_ITEM(TABLE_NM);

				string query = "INSERT INTO";
				query += string.Format(" {0} ", TABLE_NM);
				query += "(SEQ_NO,BIT_ID,DISP_GBN,USE_YN,CELL_NO,";
				query += "POS_X,POS_Y,SZ_W,SZ_H,DISP_TEXT,";
				query += "FONT_NM,FONT_SZ,FONT_ARGB,FONT_STYLE_GBN,FONT_WEIGHT_GBN,";
				query += "FONT_ALIGN_GBN,REGDATE) ";
				query += "VALUES ";
				query += "(@SEQ_NO,@BIT_ID,@DISP_GBN,@USE_YN,@CELL_NO,";
				query += "@POS_X,@POS_Y,@SZ_W,@SZ_H,@DISP_TEXT,";
				query += "@FONT_NM,@FONT_SZ,@FONT_ARGB,@FONT_STYLE_GBN,@FONT_WEIGHT_GBN,";
				query += "@FONT_ALIGN_GBN,@REGDATE) ";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", MAX_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@BIT_ID", item.BIT_ID));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_GBN", item.DISP_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@USE_YN", item.USE_YN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CELL_NO", item.CELL_NO));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@POS_X", item.POS_X));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@POS_Y", item.POS_Y));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SZ_W", item.SZ_W));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SZ_H", item.SZ_H));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_TEXT", item.DISP_TEXT));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_NM", item.FONT_NM));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_SZ", item.FONT_SZ));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_ARGB", item.FONT_ARGB));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_STYLE_GBN", item.FONT_STYLE_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_WEIGHT_GBN", item.FONT_WEIGHT_GBN));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_ALIGN_GBN", item.FONT_ALIGN_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@REGDATE", DateTime.Now));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret == -1) return false;

				return true;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool BULK_INSERT_LED_ITEM(List<LED_ITEM> items, string TABLE_NM)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				int MAX_NO = GET_MAX_NO_LED_ITEM(TABLE_NM);

				mSQL.SetTransaction();

				foreach (LED_ITEM item in items)
				{
					string query = "INSERT INTO";
					query += string.Format(" {0} ", TABLE_NM);
					query += "(SEQ_NO,BIT_ID,DISP_GBN,USE_YN,CELL_NO,";
					query += "POS_X,POS_Y,SZ_W,SZ_H,DISP_TEXT,";
					query += "FONT_NM,FONT_SZ,FONT_ARGB,FONT_STYLE_GBN,FONT_WEIGHT_GBN,";
					query += "FONT_ALIGN_GBN,REGDATE) ";
					query += "VALUES ";
					query += "(@SEQ_NO,@BIT_ID,@DISP_GBN,@USE_YN,@CELL_NO,";
					query += "@POS_X,@POS_Y,@SZ_W,@SZ_H,@DISP_TEXT,";
					query += "@FONT_NM,@FONT_SZ,@FONT_ARGB,@FONT_STYLE_GBN,@FONT_WEIGHT_GBN,";
					query += "@FONT_ALIGN_GBN,@REGDATE) ";

					List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

					parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", MAX_NO++));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@BIT_ID", item.BIT_ID));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_GBN", item.DISP_GBN));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@USE_YN", item.USE_YN));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@CELL_NO", item.CELL_NO));

					parameters.Add(new System.Data.SQLite.SQLiteParameter("@POS_X", item.POS_X));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@POS_Y", item.POS_Y));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@SZ_W", item.SZ_W));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@SZ_H", item.SZ_H));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_TEXT", item.DISP_TEXT));

					parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_NM", item.FONT_NM));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_SZ", item.FONT_SZ));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_ARGB", item.FONT_ARGB));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_STYLE_GBN", item.FONT_STYLE_GBN));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_WEIGHT_GBN", item.FONT_WEIGHT_GBN));

					parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_ALIGN_GBN", item.FONT_ALIGN_GBN));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@REGDATE", DateTime.Now));

					//int ret = mSQL.ExecuteNonQuery(query, parameters);

					if (mSQL.ExecuteNonQuery(query, parameters) == -1)
					{
						mSQL.SetRollBack();
						return false;
					}
				}
				return mSQL.SetCommit();
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool UPDATE_LED_ITEM(LED_ITEM item, string TABLE_NM)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = string.Format("UPDATE {0} SET ", TABLE_NM);
				query += "USE_YN=@USE_YN,CELL_NO=@CELL_NO,";
				query += "POS_X=@POS_X,POS_Y=@POS_Y,SZ_W=@SZ_W,SZ_H=@SZ_H,DISP_TEXT=@DISP_TEXT,";
				query += "FONT_NM=@FONT_NM,FONT_SZ=@FONT_SZ,FONT_ARGB=@FONT_ARGB,FONT_STYLE_GBN=@FONT_STYLE_GBN,FONT_WEIGHT_GBN=@FONT_WEIGHT_GBN,";
				query += "POS_X=@POS_X,POS_Y=@POS_Y,SZ_W=@SZ_W,SZ_H=@SZ_H,DISP_TEXT=@DISP_TEXT,";
				query += "FONT_ALIGN_GBN = @FONT_ALIGN_GBN ";
				query += "WHERE SEQ_NO=@SEQ_NO";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@USE_YN", item.USE_YN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CELL_NO", item.CELL_NO));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@POS_X", item.POS_X));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@POS_Y", item.POS_Y));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SZ_W", item.SZ_W));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SZ_H", item.SZ_H));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_TEXT", item.DISP_TEXT));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_NM", item.FONT_NM));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_SZ", item.FONT_SZ));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_ARGB", item.FONT_ARGB));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_STYLE_GBN", item.FONT_STYLE_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_WEIGHT_GBN", item.FONT_WEIGHT_GBN));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FONT_ALIGN_GBN", item.FONT_ALIGN_GBN));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", item.SEQ_NO));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret != -1) return true;

				return false;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool DELETE_LED_ITEM(LED_ITEM item,string TABLE_NM)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = string.Format("DELETE FROM {0} WHERE SEQ_NO=@SEQ_NO", TABLE_NM);

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", item.SEQ_NO));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret != -1) return true;

				return false;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}


		#endregion

		#region LED_BUS_ARRIVESOON 기초정보

		public List<LED_BUS_ARRIVESOON> SELECT_LED_BUS_ARRIVESOON_BY_QUERY(string query)
		{
			try
			{
				if (Connect() == false)
				{
					return null;
				}

				List<LED_BUS_ARRIVESOON> items = new List<LED_BUS_ARRIVESOON>();
				System.Data.DataTable _dt = mSQL.ExecuteDataTableByQuery(query);
				if (_dt != null)
				{
					foreach (System.Data.DataRow row in _dt.Rows)
					{
						LED_BUS_ARRIVESOON item = MatchManager.LED_BUS_ARRIVESOON_BY_ROW(row);
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

		int GET_MAX_NO_LED_BUS_ARRIVESOON()
		{
			try
			{
				string query = string.Format("SELECT MAX(SEQ_NO) FROM LED_BUS_ARRIVESOON");

				int result = mSQL.ExecuteScalarByQuery(query);
				if (result == -1) return 1;

				return result + 1;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return 1;
			}
		}

		public bool INSERT_LED_BUS_ARRIVESOON(LED_BUS_ARRIVESOON item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				int MAX_NO = GET_MAX_NO_LED_BUS_ARRIVESOON();

				string query = "INSERT INTO LED_BUS_ARRIVESOON ";
				query += "(SEQ_NO,BIT_ID,DISP_GBN,USE_YN,TITLE_TEXT,";
				query += "TITLE_POS_X,TITLE_POS_Y,TITLE_SZ_W,TITLE_SZ_H,TITLE_FONT_NM,";
				query += "TITLE_FONT_SZ,TITLE_FONT_ARGB,TITLE_FONT_STYLE_GBN,TITLE_FONT_WEIGHT_GBN,TITLE_FONT_ALIGN_GBN,";
				query += "CONT_POS_X,CONT_POS_Y,CONT_SZ_W,CONT_SZ_H,CONT_FONT_NM,";
				query += "CONT_FONT_SZ,CONT_FONT_ARGB,CONT_FONT_STYLE_GBN,CONT_FONT_WEIGHT_GBN,";
				query += "REGDATE) ";
				query += "VALUES ";
				query += "(@SEQ_NO,@BIT_ID,@DISP_GBN,@USE_YN,@TITLE_TEXT,";
				query += "@TITLE_POS_X,@TITLE_POS_Y,@TITLE_SZ_W,@TITLE_SZ_H,@TITLE_FONT_NM,";
				query += "@TITLE_FONT_SZ,@TITLE_FONT_ARGB,@TITLE_FONT_STYLE_GBN,@TITLE_FONT_WEIGHT_GBN,@TITLE_FONT_ALIGN_GBN,";
				query += "@CONT_POS_X,@CONT_POS_Y,@CONT_SZ_W,@CONT_SZ_H,@CONT_FONT_NM,";
				query += "@CONT_FONT_SZ,@CONT_FONT_ARGB,@CONT_FONT_STYLE_GBN,@CONT_FONT_WEIGHT_GBN,";
				query += "@REGDATE) ";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", MAX_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@BIT_ID", item.BIT_ID));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_GBN", item.DISP_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@USE_YN", item.USE_YN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_TEXT", item.TITLE_TEXT));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_POS_X", item.TITLE_POS_X));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_POS_Y", item.TITLE_POS_Y));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_SZ_W", item.TITLE_SZ_W));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_SZ_H", item.TITLE_SZ_H));				
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_NM", item.TITLE_FONT_NM));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_SZ", item.TITLE_FONT_SZ));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_ARGB", item.TITLE_FONT_ARGB));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_STYLE_GBN", item.TITLE_FONT_STYLE_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_WEIGHT_GBN", item.TITLE_FONT_WEIGHT_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_ALIGN_GBN", item.TITLE_FONT_ALIGN_GBN));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_POS_X", item.CONT_POS_X));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_POS_Y", item.CONT_POS_Y));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_SZ_W", item.CONT_SZ_W));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_SZ_H", item.CONT_SZ_H));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_FONT_NM", item.CONT_FONT_NM));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_FONT_SZ", item.CONT_FONT_SZ));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_FONT_ARGB", item.CONT_FONT_ARGB));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_FONT_STYLE_GBN", item.CONT_FONT_STYLE_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_FONT_WEIGHT_GBN", item.CONT_FONT_WEIGHT_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@REGDATE", DateTime.Now));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret == -1) return false;

				return true;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool BULK_INSERT_LED_BUS_ARRIVESOON(List<LED_BUS_ARRIVESOON> items)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				int MAX_NO = GET_MAX_NO_LED_BUS_ARRIVESOON();

				mSQL.SetTransaction();

				foreach (LED_BUS_ARRIVESOON item in items)
				{
					string query = "INSERT INTO LED_BUS_ARRIVESOON ";
					query += "(SEQ_NO,BIT_ID,DISP_GBN,USE_YN,TITLE_TEXT,";
					query += "TITLE_POS_X,TITLE_POS_Y,TITLE_SZ_W,TITLE_SZ_H,TITLE_FONT_NM,";
					query += "TITLE_FONT_SZ,TITLE_FONT_ARGB,TITLE_FONT_STYLE_GBN,TITLE_FONT_WEIGHT_GBN,TITLE_FONT_ALIGN_GBN,";
					query += "CONT_POS_X,CONT_POS_Y,CONT_SZ_W,CONT_SZ_H,CONT_FONT_NM,";
					query += "CONT_FONT_SZ,CONT_FONT_ARGB,CONT_FONT_STYLE_GBN,CONT_FONT_WEIGHT_GBN,";
					query += "REGDATE) ";
					query += "VALUES ";
					query += "(@SEQ_NO,@BIT_ID,@DISP_GBN,@USE_YN,@TITLE_TEXT,";
					query += "@TITLE_POS_X,@TITLE_POS_Y,@TITLE_SZ_W,@TITLE_SZ_H,@TITLE_FONT_NM,";
					query += "@TITLE_FONT_SZ,@TITLE_FONT_ARGB,@TITLE_FONT_STYLE_GBN,@TITLE_FONT_WEIGHT_GBN,@TITLE_FONT_ALIGN_GBN,";
					query += "@CONT_POS_X,@CONT_POS_Y,@CONT_SZ_W,@CONT_SZ_H,@CONT_FONT_NM,";
					query += "@CONT_FONT_SZ,@CONT_FONT_ARGB,@CONT_FONT_STYLE_GBN,@CONT_FONT_WEIGHT_GBN,";
					query += "REGDATE) ";

					List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

					parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", MAX_NO++));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@BIT_ID", item.BIT_ID));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@DISP_GBN", item.DISP_GBN));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@USE_YN", item.USE_YN));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_TEXT", item.TITLE_TEXT));

					parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_POS_X", item.TITLE_POS_X));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_POS_Y", item.TITLE_POS_Y));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_SZ_W", item.TITLE_SZ_W));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_SZ_H", item.TITLE_SZ_H));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_NM", item.TITLE_FONT_NM));

					parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_SZ", item.TITLE_FONT_SZ));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_ARGB", item.TITLE_FONT_ARGB));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_STYLE_GBN", item.TITLE_FONT_STYLE_GBN));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_WEIGHT_GBN", item.TITLE_FONT_WEIGHT_GBN));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_ALIGN_GBN", item.TITLE_FONT_ALIGN_GBN));

					parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_POS_X", item.CONT_POS_X));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_POS_Y", item.CONT_POS_Y));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_SZ_W", item.CONT_SZ_W));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_SZ_H", item.CONT_SZ_H));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_FONT_NM", item.CONT_FONT_NM));

					parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_FONT_SZ", item.CONT_FONT_SZ));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_FONT_ARGB", item.CONT_FONT_ARGB));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_FONT_STYLE_GBN", item.CONT_FONT_STYLE_GBN));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_FONT_WEIGHT_GBN", item.CONT_FONT_WEIGHT_GBN));
					parameters.Add(new System.Data.SQLite.SQLiteParameter("@REGDATE", DateTime.Now));

					//int ret = mSQL.ExecuteNonQuery(query, parameters);

					if (mSQL.ExecuteNonQuery(query, parameters) == -1)
					{
						mSQL.SetRollBack();
						return false;
					}
				}
				return mSQL.SetCommit();
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool UPDATE_LED_BUS_ARRIVESOON(LED_BUS_ARRIVESOON item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = string.Format("UPDATE LED_BUS_ARRIVESOON SET ");
				query += "USE_YN=@USE_YN,TITLE_TEXT=@TITLE_TEXT,";
				query += "TITLE_POS_X=@TITLE_POS_X,TITLE_POS_Y=@TITLE_POS_Y,TITLE_SZ_W=@TITLE_SZ_W,TITLE_SZ_H=@TITLE_SZ_H,TITLE_FONT_NM=@TITLE_FONT_NM,";
				query += "TITLE_FONT_SZ=@TITLE_FONT_SZ,TITLE_FONT_ARGB=@TITLE_FONT_ARGB,TITLE_FONT_STYLE_GBN=@TITLE_FONT_STYLE_GBN,TITLE_FONT_WEIGHT_GBN=@TITLE_FONT_WEIGHT_GBN,TITLE_FONT_ALIGN_GBN=@TITLE_FONT_ALIGN_GBN,";
				query += "CONT_POS_X=@CONT_POS_X,CONT_POS_Y=@CONT_POS_Y,CONT_SZ_W=@CONT_SZ_W,CONT_SZ_H=@CONT_SZ_H,CONT_FONT_NM=@CONT_FONT_NM,";
				query += "CONT_FONT_SZ=@CONT_FONT_SZ,CONT_FONT_ARGB=@CONT_FONT_ARGB,CONT_FONT_STYLE_GBN=@CONT_FONT_STYLE_GBN,CONT_FONT_WEIGHT_GBN=@CONT_FONT_WEIGHT_GBN ";
				query += "WHERE SEQ_NO=@SEQ_NO";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@USE_YN", item.USE_YN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_TEXT", item.TITLE_TEXT));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_POS_X", item.TITLE_POS_X));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_POS_Y", item.TITLE_POS_Y));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_SZ_W", item.TITLE_SZ_W));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_SZ_H", item.TITLE_SZ_H));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_NM", item.TITLE_FONT_NM));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_SZ", item.TITLE_FONT_SZ));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_ARGB", item.TITLE_FONT_ARGB));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_STYLE_GBN", item.TITLE_FONT_STYLE_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_WEIGHT_GBN", item.TITLE_FONT_WEIGHT_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@TITLE_FONT_ALIGN_GBN", item.TITLE_FONT_ALIGN_GBN));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_POS_X", item.CONT_POS_X));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_POS_Y", item.CONT_POS_Y));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_SZ_W", item.CONT_SZ_W));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_SZ_H", item.CONT_SZ_H));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_FONT_NM", item.CONT_FONT_NM));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_FONT_SZ", item.CONT_FONT_SZ));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_FONT_ARGB", item.CONT_FONT_ARGB));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_FONT_STYLE_GBN", item.CONT_FONT_STYLE_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CONT_FONT_WEIGHT_GBN", item.CONT_FONT_WEIGHT_GBN));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", item.SEQ_NO));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret != -1) return true;

				return false;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool DELETE_LED_BUS_ARRIVESOON(LED_BUS_ARRIVESOON item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = string.Format("DELETE FROM LED_BUS_ARRIVESOON WHERE SEQ_NO=@SEQ_NO");

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", item.SEQ_NO));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret != -1) return true;

				return false;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}


		#endregion
			
		#region BIT_SYSTEM 기초정보

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
						BIT_SYSTEM item = MatchManager.BIT_SYSTEM_BY_ROW(row);
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

		int GET_MAX_NO_BIT_SYSTEM()
		{
			try
			{
				string query = string.Format("SELECT MAX(SEQ_NO) FROM BIT_SYSTEM");

				int result = mSQL.ExecuteScalarByQuery(query);
				if (result == -1) return 1;

				return result + 1;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return 1;
			}
		}

		public bool INSERT_BIT_SYSTEM(BIT_SYSTEM item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				int MAX_NO = GET_MAX_NO_BIT_SYSTEM();

				string query = "INSERT INTO BIT_SYSTEM ";
				query += "(SEQ_NO,BIT_ID,MOBILE_NO,STATION_NM,SERVER_URL,SERVER_TYPE,";
				query += "SERVER_PORT,FTP_GBN,FTP_IP,FTP_PORT,FTP_USERID,";
				query += "FTP_USERPWD,HTTP_URL,HTTP_PORT,ENV_GBN,ENV_PORT_NM,ENV_BAUD_RATE,";
				query += "CAM_NO1,CAM_NO1_ROTATE,CAM_NO2,CAM_NO2_ROTATE,REGDATE) ";
				query += "VALUES ";
				query += "(@SEQ_NO,@BIT_ID,@MOBILE_NO,@STATION_NM,@SERVER_URL,@SERVER_TYPE,";
				query += "@SERVER_PORT,@FTP_GBN,@FTP_IP,@FTP_PORT,@FTP_USERID,";
				query += "@FTP_USERPWD,@HTTP_URL,@HTTP_PORT,@ENV_GBN,@ENV_PORT_NM,@ENV_BAUD_RATE,";
				query += "@CAM_NO1,@CAM_NO1_ROTATE,@CAM_NO2,@CAM_NO2_ROTATE,@REGDATE) ";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", MAX_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@BIT_ID", item.BIT_ID));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@MOBILE_NO", item.MOBILE_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@STATION_NM", item.STATION_NM));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SERVER_URL", item.SERVER_URL));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SERVER_TYPE", item.SERVER_TYPE));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SERVER_PORT", item.SERVER_PORT));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FTP_GBN", item.FTP_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FTP_IP", item.FTP_IP));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FTP_PORT", item.FTP_PORT));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FTP_USERID", item.FTP_USERID));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FTP_USERPWD", item.FTP_USERPWD));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@HTTP_URL", item.HTTP_URL));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@HTTP_PORT", item.HTTP_PORT));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ENV_GBN", item.ENV_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ENV_PORT_NM", item.ENV_PORT_NM));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ENV_BAUD_RATE", item.ENV_BAUD_RATE));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CAM_NO1", item.CAM_NO1));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CAM_NO1_ROTATE", item.CAM_NO1_ROTATE));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CAM_NO2", item.CAM_NO2));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CAM_NO2_ROTATE", item.CAM_NO2_ROTATE));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@REGDATE", DateTime.Now));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret == -1) return false;

				return true;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}
		public bool UPDATE_BIT_SYSTEM(BIT_SYSTEM item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = string.Format("UPDATE BIT_SYSTEM SET ");
				query += "BIT_ID=@BIT_ID,MOBILE_NO=@MOBILE_NO,STATION_NM=@STATION_NM,SERVER_URL=@SERVER_URL,SERVER_TYPE=@SERVER_TYPE,SERVER_PORT=@SERVER_PORT,";
				query += "FTP_GBN=@FTP_GBN,FTP_IP=@FTP_IP,FTP_PORT=@FTP_PORT,FTP_USERID=@FTP_USERID,FTP_USERPWD=@FTP_USERPWD,";
				query += "HTTP_URL=@HTTP_URL,HTTP_PORT=@HTTP_PORT,ENV_GBN=@ENV_GBN,ENV_PORT_NM=@ENV_PORT_NM,ENV_BAUD_RATE=@ENV_BAUD_RATE,";
				query += "CAM_NO1=@CAM_NO1,CAM_NO1_ROTATE=@CAM_NO1_ROTATE,CAM_NO2=@CAM_NO2,CAM_NO2_ROTATE=@CAM_NO2_ROTATE ";
				query += "WHERE SEQ_NO=@SEQ_NO";

				
				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@BIT_ID", item.BIT_ID));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@MOBILE_NO", item.MOBILE_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@STATION_NM", item.STATION_NM));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SERVER_URL", item.SERVER_URL));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SERVER_TYPE", item.SERVER_TYPE));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SERVER_PORT", item.SERVER_PORT));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FTP_GBN", item.FTP_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FTP_IP", item.FTP_IP));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FTP_PORT", item.FTP_PORT));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FTP_USERID", item.FTP_USERID));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@FTP_USERPWD", item.FTP_USERPWD));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@HTTP_URL", item.HTTP_URL));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@HTTP_PORT", item.HTTP_PORT));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ENV_GBN", item.ENV_GBN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ENV_PORT_NM", item.ENV_PORT_NM));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ENV_BAUD_RATE", item.ENV_BAUD_RATE));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CAM_NO1", item.CAM_NO1));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CAM_NO1_ROTATE", item.CAM_NO1_ROTATE));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CAM_NO2", item.CAM_NO2));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@CAM_NO2_ROTATE", item.CAM_NO2_ROTATE));
				//parameters.Add(new System.Data.SQLite.SQLiteParameter("@SHOCKCAM_NO", item.SHOCKCAM_NO));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", item.SEQ_NO));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret != -1) return true;

				return false;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool DELETE_BIT_SYSTEM(BIT_SYSTEM item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = string.Format("DELETE FROM BIT_SYSTEM WHERE SEQ_NO=@SEQ_NO");

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", item.SEQ_NO));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret != -1) return true;

				return false;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}


		#endregion

		#region TB_SYSTEM 기초정보

		public List<TB_SYSTEM> SELECT_TB_SYSTEM_BY_QUERY(string query)
		{
			try
			{
				if (Connect() == false)
				{
					return null;
				}

				List<TB_SYSTEM> items = new List<TB_SYSTEM>();
				System.Data.DataTable _dt = mSQL.ExecuteDataTableByQuery(query);
				if (_dt != null)
				{
					foreach (System.Data.DataRow row in _dt.Rows)
					{
						TB_SYSTEM item = MatchManager.TB_SYSTEM_BY_ROW(row);
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

		int GET_MAX_NO_TB_SYSTEM()
		{
			try
			{
				string query = string.Format("SELECT MAX(SEQ_NO) FROM TB_SYSTEM");

				int result = mSQL.ExecuteScalarByQuery(query);
				if (result == -1) return 1;

				return result + 1;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return 1;
			}
		}

		public bool INSERT_TB_SYSTEM(TB_SYSTEM item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				int MAX_NO = GET_MAX_NO_TB_SYSTEM();

				string query = "INSERT INTO TB_SYSTEM ";
				query += "(SEQ_NO,LOGSAVE_DAY,LOGSAVE_PERCENT,LED_PAGE_CHGTIME,LED_USE_BUSNO_COLOR,";
				query += "LED_USE_BUSNO_LOW,LED_USE_ARRIVESOON_LOW,LED_USE_ARRIVESOON_COLOR,ENV_USE_FAN_MANUAL,ENV_USE_HEATER_MANUAL,";
				query += "SUBWAY_DISPLAY_YN,SUBWAY_LINE_NO,SUBWAY_STATION_NO,SHOCK_DETECT_NO,REGDATE) ";
				query += "VALUES ";
				query += "(@SEQ_NO,@LOGSAVE_DAY,@LOGSAVE_PERCENT,@LED_PAGE_CHGTIME,@LED_USE_BUSNO_COLOR,";
				query += "@LED_USE_BUSNO_LOW,@LED_USE_ARRIVESOON_LOW,@LED_USE_ARRIVESOON_COLOR,@ENV_USE_FAN_MANUAL,@ENV_USE_HEATER_MANUAL,";
				query += "@SUBWAY_DISPLAY_YN,@SUBWAY_LINE_NO,@SUBWAY_STATION_NO,@SHOCK_DETECT_NO,@REGDATE) ";


				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", MAX_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LOGSAVE_DAY", item.LOGSAVE_DAY));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LOGSAVE_PERCENT", item.LOGSAVE_PERCENT));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LED_PAGE_CHGTIME", item.LED_PAGE_CHGTIME));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LED_USE_BUSNO_COLOR", item.LED_USE_BUSNO_COLOR ? 1 : 0));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LED_USE_BUSNO_LOW", item.LED_USE_BUSNO_LOW ? 1 : 0));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LED_USE_ARRIVESOON_LOW", item.LED_USE_ARRIVESOON_LOW ? 1 : 0));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LED_USE_ARRIVESOON_COLOR", item.LED_USE_ARRIVESOON_COLOR ? 1 : 0));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ENV_USE_FAN_MANUAL", item.ENV_USE_FAN_MANUAL ? 1 : 0));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ENV_USE_HEATER_MANUAL", item.ENV_USE_HEATER_MANUAL ? 1 : 0));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SUBWAY_DISPLAY_YN", item.SUBWAY_DISPLAY_YN));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SUBWAY_LINE_NO", item.SUBWAY_LINE_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SUBWAY_STATION_NO", item.SUBWAY_STATION_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SHOCK_DETECT_NO", item.SHOCK_DETECT_NO));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@REGDATE", DateTime.Now));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret == -1) return false;

				return true;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}
		public bool UPDATE_TB_SYSTEM(TB_SYSTEM item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = string.Format("UPDATE TB_SYSTEM SET ");
				query += "LOGSAVE_DAY=@LOGSAVE_DAY,LOGSAVE_PERCENT=@LOGSAVE_PERCENT,LED_PAGE_CHGTIME=@LED_PAGE_CHGTIME,LED_USE_BUSNO_COLOR=@LED_USE_BUSNO_COLOR,LED_USE_BUSNO_LOW=@LED_USE_BUSNO_LOW,";
				query += "LED_USE_ARRIVESOON_LOW=@LED_USE_ARRIVESOON_LOW,LED_USE_ARRIVESOON_COLOR=@LED_USE_ARRIVESOON_COLOR,ENV_USE_FAN_MANUAL=@ENV_USE_FAN_MANUAL,ENV_USE_HEATER_MANUAL=@ENV_USE_HEATER_MANUAL,SUBWAY_DISPLAY_YN=@SUBWAY_DISPLAY_YN,";
				query += "SUBWAY_LINE_NO=@SUBWAY_LINE_NO,SUBWAY_STATION_NO=@SUBWAY_STATION_NO,SHOCK_DETECT_NO=@SHOCK_DETECT_NO ";
				query += "WHERE SEQ_NO=@SEQ_NO";

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LOGSAVE_DAY", item.LOGSAVE_DAY));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LOGSAVE_PERCENT", item.LOGSAVE_PERCENT));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LED_PAGE_CHGTIME", item.LED_PAGE_CHGTIME));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LED_USE_BUSNO_COLOR", item.LED_USE_BUSNO_COLOR ? 1 : 0));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LED_USE_BUSNO_LOW", item.LED_USE_BUSNO_LOW ? 1 : 0));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LED_USE_ARRIVESOON_LOW", item.LED_USE_ARRIVESOON_LOW ? 1 : 0));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@LED_USE_ARRIVESOON_COLOR", item.LED_USE_ARRIVESOON_COLOR ? 1 : 0));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ENV_USE_FAN_MANUAL", item.ENV_USE_FAN_MANUAL ? 1 : 0));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@ENV_USE_HEATER_MANUAL", item.ENV_USE_HEATER_MANUAL ? 1 : 0));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SUBWAY_DISPLAY_YN", item.SUBWAY_DISPLAY_YN));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SUBWAY_LINE_NO", item.SUBWAY_LINE_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SUBWAY_STATION_NO", item.SUBWAY_STATION_NO));
				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SHOCK_DETECT_NO", item.SHOCK_DETECT_NO));

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", item.SEQ_NO));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret != -1) return true;

				return false;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}

		public bool DELETE_TB_SYSTEM(TB_SYSTEM item)
		{
			try
			{
				if (Connect() == false)
				{
					return false;
				}

				string query = string.Format("DELETE FROM TB_SYSTEM WHERE SEQ_NO=@SEQ_NO");

				List<System.Data.SQLite.SQLiteParameter> parameters = new List<System.Data.SQLite.SQLiteParameter>();

				parameters.Add(new System.Data.SQLite.SQLiteParameter("@SEQ_NO", item.SEQ_NO));

				int ret = mSQL.ExecuteNonQuery(query, parameters);
				if (ret != -1) return true;

				return false;
			}
			catch (Exception ee)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			finally
			{
				DisConnect();
			}
		}


		#endregion
	}
}
