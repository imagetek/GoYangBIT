using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;

namespace SSData
{
    public class DatabaseManager
    {
        static DatabaseManager()
        {

        }

        public static void InitializeDatabase()
        {
            try
            {
				switch (DataManager.ServerInfo.DB_GBN)
				{
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        mSQLite.InitializeDatabase();
                        break;
                }

			}
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public static bool IS_YN_접속확인()
        {
            try
            {
                bool isYN접속 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN접속 = mSQLite.ISConnect();
                        break;
                }

                return isYN접속;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool SetConnection(string ip, int port, string database, string id, string pwd)
        {
            try
            {
                bool isYN접속 = false;
                //switch (DataManager.ServerInfo.DB_GBN)
                //{                
                //    case 2:
                //        MSSQLManager msSQL = new MSSQLManager();
                //        msSQL.SetConnectString(ip, port, database, id, pwd);
                //        isYN접속 = msSQL.ISConnect();
                //        break;
                //}

                return isYN접속;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool SetConnectionFile(string file)
        {
            try
            {
                bool isYN접속 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        mSQLite.SetConnectFile(file);
                        isYN접속 = mSQLite.ISConnect();
                        break;
                }

                return isYN접속;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        //public static List<string> Select테이블목록()
        //{
        //    try
        //    {
        //        List<string> items = new List<string>();
        //        switch (DataManager.SetSERVER.DB_GBN)
        //        {
        //            case 1:
        //                FirebirdManager mFirebird = new FirebirdManager();
        //                items = mFirebird.GET_TABLE_NM_BY_ALL();
        //                break;

        //            //case 2:
        //            //    MSSQLManager msSQL = new MSSQLManager();
        //            //    msSQL.SetConnectString(ip, port, database, id, pwd);
        //            //    isYN접속 = msSQL.ISConnect();
        //            //    break;

        //            //case 3:
        //            //    MySQLManager mySQL = new MySQLManager();
        //            //    mySQL.SetConnectString(ip, port, database, id, pwd);
        //            //    isYN접속 = mySQL.ISConnect();
        //            //    break;
        //        }

        //        return items;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region 기초정보 BC_CODE

        public static List<BC_CODE> SELECT_BC_CODE_BY_QUERY(string query)
        {
            try
            {
                List<BC_CODE> items = new List<BC_CODE>();
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        items = mSQLite.SELECT_BC_CODE_BY_QUERY(query);
                        break;

                        //case 2:
                        //    MSSQLManager msSQL = new MSSQLManager();
                        //    items = msSQL.SELECT_BC_CODE_BY_QUERY(query);
                        //    break;

                        //case 3:
                        //    MySQLManager mySQL = new MySQLManager();
                        //    items = mySQL.SELECT_BC_CODE_BY_QUERY(query);
                        //    break;
                }

                return items;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        public static bool INSERT_BC_CODE(BC_CODE item)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.INSERT_BC_CODE(item);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool UPDATE_BC_CODE(BC_CODE item)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.UPDATE_BC_CODE(item);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool DELETE_BC_CODE(BC_CODE item)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.DELETE_BC_CODE(item);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        #endregion

        //#region TB_CONFIG
        //public static List<TB_CONFIG> SELECT_TB_CONFIG_BY_QUERY(string query)
        //{
        //    try
        //    {
        //        List<TB_CONFIG> items = new List<TB_CONFIG>();
        //        switch (DataManager.ServerInfo.DB_GBN)
        //        {
        //            case 1:
        //                SQLiteManager mSQLite = new SQLiteManager();
        //                items = mSQLite.SELECT_TB_CONFIG_BY_QUERY(query);
        //                break;
        //        }

        //        return items;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        //public static bool INSERT_TB_CONFIG(TB_CONFIG item)
        //{
        //    try
        //    {
        //        bool isYN결과 = false;
        //        switch (DataManager.ServerInfo.DB_GBN)
        //        {
        //            case 1:
        //                SQLiteManager mSQLite = new SQLiteManager();
        //                isYN결과 = mSQLite.INSERT_TB_CONFIG(item);
        //                break;
        //        }

        //        return isYN결과;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return false;
        //    }
        //}

        //public static bool UPDATE_TB_CONFIG(TB_CONFIG item)
        //{
        //    try
        //    {
        //        bool isYN결과 = false;
        //        switch (DataManager.ServerInfo.DB_GBN)
        //        {
        //            case 1:
        //                SQLiteManager mSQLite = new SQLiteManager();
        //                isYN결과 = mSQLite.UPDATE_TB_CONFIG(item);
        //                break;
        //        }

        //        return isYN결과;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return false;
        //    }
        //}

        //public static bool DELETE_TB_CONFIG(TB_CONFIG item)
        //{
        //    try
        //    {
        //        bool isYN결과 = false;
        //        switch (DataManager.ServerInfo.DB_GBN)
        //        {
        //            case 1:
        //                SQLiteManager mSQLite = new SQLiteManager();
        //                isYN결과 = mSQLite.DELETE_TB_CONFIG(item);
        //                break;
        //        }

        //        return isYN결과;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return false;
        //    }
        //}


        //#endregion

        #region BIT_ENV_SETTING

        public static List<BIT_ENV_SETTING> SELECT_BIT_ENV_SETTING_BY_QUERY(string query)
        {
            try
            {
                List<BIT_ENV_SETTING> items = new List<BIT_ENV_SETTING>();
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        items = mSQLite.SELECT_BIT_ENV_SETTING_BY_QUERY(query);
                        break;

                        //case 2:
                        //    MSSQLManager msSQL = new MSSQLManager();
                        //    items = msSQL.SELECT_BIT_ENV_SETTING_BY_QUERY(query);
                        //    break;

                        //case 3:
                        //    MySQLManager mySQL = new MySQLManager();
                        //    items = mySQL.SELECT_BIT_ENV_SETTING_BY_QUERY(query);
                        //    break;
                }

                return items;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        public static bool INSERT_BIT_ENV_SETTING(BIT_ENV_SETTING item)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.INSERT_BIT_ENV_SETTING(item);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        //public static bool UPDATE_BIT_ENV_SETTING(BIT_ENV_SETTING item)
        //{
        //    try
        //    {
        //        bool isYN결과 = false;
        //        switch (DataManager.ServerInfo.DB_GBN)
        //        {
        //            case 1:
        //                SQLiteManager mSQLite = new SQLiteManager();
        //                isYN결과 = mSQLite.UPDATE_BIT_ENV_SETTING(item);
        //                break;
        //        }

        //        return isYN결과;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return false;
        //    }
        //}

        public static bool DELETE_BIT_ENV_SETTING(BIT_ENV_SETTING item)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.DELETE_BIT_ENV_SETTING(item);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        #endregion

        #region BIT_ENV_LUX

        public static List<BIT_ENV_LUX> SELECT_BIT_ENV_LUX_BY_QUERY(string query)
        {
            try
            {
                List<BIT_ENV_LUX> items = new List<BIT_ENV_LUX>();
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        items = mSQLite.SELECT_BIT_ENV_LUX_BY_QUERY(query);
                        break;
                }

                return items;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        public static bool INSERT_BIT_ENV_LUX(BIT_ENV_LUX item, int CONFIG_NO)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.INSERT_BIT_ENV_LUX(item, CONFIG_NO);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        //public static bool UPDATE_BIT_ENV_LUX(BIT_ENV_LUX item)
        //{
        //    try
        //    {
        //        bool isYN결과 = false;
        //        switch (DataManager.ServerInfo.DB_GBN)
        //        {
        //            case 1:
        //                SQLiteManager mSQLite = new SQLiteManager();
        //                isYN결과 = mSQLite.UPDATE_BIT_ENV_LUX(item);
        //                break;
        //        }

        //        return isYN결과;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return false;
        //    }
        //}

        public static bool DELETE_BIT_ENV_LUX(BIT_ENV_LUX item)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.DELETE_BIT_ENV_LUX(item);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool DELETE_BIT_ENV_LUX_CONFIG_NO(int CONFIG_NO)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.DELETE_BIT_ENV_LUX(CONFIG_NO);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        #endregion

        #region BIT_DISPLAY
        public static List<BIT_DISPLAY> SELECT_BIT_DISPLAY_BY_QUERY(string query)
        {
            try
            {
                List<BIT_DISPLAY> items = new List<BIT_DISPLAY>();
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        items = mSQLite.SELECT_BIT_DISPLAY_BY_QUERY(query);
                        break;
                }

                return items;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        public static int INSERT_BIT_DISPLAY(BIT_DISPLAY item)
        {
            try
            {
                int retSEQ = 0;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        retSEQ = mSQLite.INSERT_BIT_DISPLAY(item);
                        break;
                }

                return retSEQ;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return -9;
            }
        }

        public static bool UPDATE_BIT_DISPLAY(BIT_DISPLAY item)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.UPDATE_BIT_DISPLAY(item);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool DELETE_BIT_DISPLAY(BIT_DISPLAY item)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.DELETE_BIT_DISPLAY(item);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }


        #endregion

        #region LED_ITEM
        public static List<LED_ITEM> SELECT_LED_ITEM_BY_QUERY(string query)
        {
            try
            {
                List<LED_ITEM> items = new List<LED_ITEM>();
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        items = mSQLite.SELECT_LED_ITEM_BY_QUERY(query);
                        break;
                }

                return items;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        public static bool INSERT_LED_ITEM(LED_ITEM item, string TABLE_NM)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.INSERT_LED_ITEM(item, TABLE_NM);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool BULK_INSERT_LED_ITEM(List<LED_ITEM> items, string TABLE_NM)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.BULK_INSERT_LED_ITEM(items, TABLE_NM);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool UPDATE_LED_ITEM(LED_ITEM item, string TABLE_NM)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.UPDATE_LED_ITEM(item, TABLE_NM);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool DELETE_LED_ITEM(LED_ITEM item, string TABLE_NM)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.DELETE_LED_ITEM(item, TABLE_NM);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }


        #endregion

        #region LED_BUS_ARRIVESOON

        //public static List<LED_BUS_ARRIVESOON> SELECT_LED_BUS_ARRIVESOON_BY_QUERY(string query)
        //{
        //    try
        //    {
        //        List<LED_BUS_ARRIVESOON> items = new List<LED_BUS_ARRIVESOON>();
        //        switch (DataManager.ServerInfo.DB_GBN)
        //        {
        //            case 1:
        //                SQLiteManager mSQLite = new SQLiteManager();
        //                items = mSQLite.SELECT_LED_BUS_ARRIVESOON_BY_QUERY(query);
        //                break;
        //        }

        //        return items;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        //public static bool INSERT_LED_BUS_ARRIVESOON(LED_BUS_ARRIVESOON item)
        //{
        //    try
        //    {
        //        bool isYN결과 = false;
        //        switch (DataManager.ServerInfo.DB_GBN)
        //        {
        //            case 1:
        //                SQLiteManager mSQLite = new SQLiteManager();
        //                isYN결과 = mSQLite.INSERT_LED_BUS_ARRIVESOON(item);
        //                break;
        //        }

        //        return isYN결과;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return false;
        //    }
        //}

        //public static bool UPDATE_LED_BUS_ARRIVESOON(LED_BUS_ARRIVESOON item)
        //{
        //    try
        //    {
        //        bool isYN결과 = false;
        //        switch (DataManager.ServerInfo.DB_GBN)
        //        {
        //            case 1:
        //                SQLiteManager mSQLite = new SQLiteManager();
        //                isYN결과 = mSQLite.UPDATE_LED_BUS_ARRIVESOON(item);
        //                break;
        //        }

        //        return isYN결과;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return false;
        //    }
        //}

        //public static bool DELETE_LED_BUS_ARRIVESOON(LED_BUS_ARRIVESOON item)
        //{
        //    try
        //    {
        //        bool isYN결과 = false;
        //        switch (DataManager.ServerInfo.DB_GBN)
        //        {
        //            case 1:
        //                SQLiteManager mSQLite = new SQLiteManager();
        //                isYN결과 = mSQLite.DELETE_LED_BUS_ARRIVESOON(item);
        //                break;
        //        }

        //        return isYN결과;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return false;
        //    }
        //}


        #endregion

        #region BIT_SYSTEM

        public static List<BIT_SYSTEM> SELECT_BIT_SYSTEM_BY_QUERY(string query)
        {
            try
            {
                List<BIT_SYSTEM> items = new List<BIT_SYSTEM>();
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        items = mSQLite.SELECT_BIT_SYSTEM_BY_QUERY(query);
                        break;
                }

                return items;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        public static bool INSERT_BIT_SYSTEM(BIT_SYSTEM item)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.INSERT_BIT_SYSTEM(item);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool UPDATE_BIT_SYSTEM(BIT_SYSTEM item)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.UPDATE_BIT_SYSTEM(item);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool DELETE_BIT_SYSTEM(BIT_SYSTEM item)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.DELETE_BIT_SYSTEM(item);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }


        #endregion

        #region TB_SYSTEM
        public static List<TB_SYSTEM> SELECT_TB_SYSTEM_BY_QUERY(string query)
        {
            try
            {
                List<TB_SYSTEM> items = new List<TB_SYSTEM>();
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        items = mSQLite.SELECT_TB_SYSTEM_BY_QUERY(query);
                        break;
                }

                return items;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        public static bool INSERT_TB_SYSTEM(TB_SYSTEM item)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.INSERT_TB_SYSTEM(item);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool UPDATE_TB_SYSTEM(TB_SYSTEM item)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.UPDATE_TB_SYSTEM(item);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool DELETE_TB_SYSTEM(TB_SYSTEM item)
        {
            try
            {
                bool isYN결과 = false;
                switch (DataManager.ServerInfo.DB_GBN)
                {
                    case 1:
                        SQLiteManager mSQLite = new SQLiteManager();
                        isYN결과 = mSQLite.DELETE_TB_SYSTEM(item);
                        break;
                }

                return isYN결과;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }


        #endregion
    }
}



