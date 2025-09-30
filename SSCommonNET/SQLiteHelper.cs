//using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.SQLite;

namespace SSCommonNET
{
	public class SQLiteHelper
	{
		private string connString = "";
		private SQLiteConnection conn;
		private SQLiteCommand cmd;
		private SQLiteTransaction transaction;
		private bool isConnection;

		public static bool IsDBConnection(string fullfilepath, string pwd, string tblname)
		{
			try
			{
				string commandText = string.Format("Select count(*) From sqlite_master Where type = 'table' and tbl_name = '{0}'", (object)tblname);
				SQLiteConnection sqLiteConnection = new SQLiteConnection(string.Format("Data Source={0};Password={1}", (object)fullfilepath, (object)pwd));
				SQLiteConnection connection = sqLiteConnection;
				SQLiteCommand sqLiteCommand = new SQLiteCommand(commandText, connection);
				if (sqLiteConnection.State == ConnectionState.Closed)
					sqLiteConnection.Open();
				int int32 = Convert.ToInt32(sqLiteCommand.ExecuteScalar());
				if (sqLiteConnection.State == ConnectionState.Open)
				{
					sqLiteConnection.Close();
					sqLiteConnection.Dispose();
					sqLiteCommand.Cancel();
					sqLiteCommand.Dispose();
				}
				return int32 != 0;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return false;
			}
		}

		public SQLiteHelper(string fullfilepath, string pwd)
		{
			this.connString = string.Format("Data Source={0};Password={1}", (object)fullfilepath, (object)pwd);
		}

		public SQLiteHelper(string fullfilepath)
		{
			this.connString = string.Format("Data Source={0}", (object)fullfilepath);
		}

		public bool IsConnection
		{
			get => this.isConnection;
			set => this.isConnection = value;
		}

		public bool Connect()
		{
			try
			{
				if (this.isConnection)
					return true;
				if (this.conn == null)
					this.conn = new SQLiteConnection(this.connString);
				if (this.cmd == null)
				{
					this.cmd = new SQLiteCommand();
					this.cmd.Connection = this.conn;
				}
				this.conn.Open();
				this.isConnection = true;
				return true;
			}
			catch (Exception ex)
			{
				this.isConnection = false;
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				Console.WriteLine(ex.Message + ex.StackTrace);
				return false;
			}
		}

		public void DisConnect()
		{
			try
			{
				if (this.conn != null)
				{
					this.conn.Close();
					this.conn.Dispose();
					this.conn = (SQLiteConnection)null;
					this.isConnection = false;
				}
				this.isConnection = false;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public bool SetTransaction()
		{
			try
			{
				if (this.cmd.Connection.State != ConnectionState.Open)
					return false;
				this.transaction = this.conn.BeginTransaction();
				this.cmd.Transaction = this.transaction;
				return true;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return false;
			}
		}

		public bool SetCommit()
		{
			try
			{
				this.transaction.Commit();
				return true;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return false;
			}
		}

		public bool SetRollBack()
		{
			try
			{
				this.transaction.Rollback();
				return true;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return false;
			}
		}

		public DataTable ExecuteDataTableByQuery(string query)
		{
			try
			{
				if (this.cmd.Connection.State != ConnectionState.Open)
					return (DataTable)null;
				this.cmd.CommandText = query;
				this.cmd.CommandType = CommandType.Text;
				DataTable dataTable = new DataTable();
				new SQLiteDataAdapter(this.cmd).Fill(dataTable);
				return dataTable;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return (DataTable)null;
			}
		}

		public DataTable ExecuteDataTableByQuery(string query, List<SQLiteParameter> paramerters)
		{
			try
			{
				if (this.cmd.Connection.State != ConnectionState.Open)
					return (DataTable)null;
				this.cmd.CommandText = query;
				this.cmd.CommandType = CommandType.Text;
				this.cmd.Parameters.Clear();
				this.cmd.Parameters.AddRange(paramerters.ToArray());
				DataTable dataTable = new DataTable();
				new SQLiteDataAdapter(this.cmd).Fill(dataTable);
				return dataTable;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return (DataTable)null;
			}
		}

		public int ExecuteScalarByQuery(string qry)
		{
			try
			{
				if (this.cmd.Connection.State != ConnectionState.Open)
					return -1;
				this.cmd.CommandText = qry;
				this.cmd.CommandType = CommandType.Text;
				return Convert.ToInt32(this.cmd.ExecuteScalar());
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return -1;
			}
		}

		public int ExecuteIntByQuery(string qry)
		{
			try
			{
				if (this.cmd.Connection.State != ConnectionState.Open)
					return -1;
				this.cmd.CommandText = qry;
				this.cmd.CommandType = CommandType.Text;
				return this.cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return -1;
			}
		}

		public int ExecuteNonQuery(string qry, List<SQLiteParameter> paramerters)
		{
			try
			{
				if (this.cmd.Connection.State != ConnectionState.Open)
					return -1;
				this.cmd.CommandText = qry;
				this.cmd.CommandType = CommandType.Text;
				this.cmd.Parameters.Clear();
				this.cmd.Parameters.AddRange(paramerters.ToArray());
				return this.cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return -1;
			}
		}

		public int ExecuteNonQuery(string qry)
		{
			try
			{
				if (this.cmd.Connection.State != ConnectionState.Open)
					return -1;
				this.cmd.CommandText = qry;
				this.cmd.CommandType = CommandType.Text;
				this.cmd.Parameters.Clear();
				return this.cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return -1;
			}
		}

		public DataTable SelectTableLists()
		{
			try
			{
				return this.cmd.Connection.State != ConnectionState.Open ? (DataTable)null : this.conn.GetSchema("Tables");
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return (DataTable)null;
			}
		}
	}
}
