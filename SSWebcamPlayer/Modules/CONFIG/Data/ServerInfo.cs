using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSWebcamPlayer
{
    public class ServerInfo
    {
        public int SEQ_NO{ get; set; }
        /// <summary>
        /// DB종류 0: 미사용 , 1 SQLite , 2 Firebird , 3 MSSQL, 4 MySQL ,  5 OracleDB
        /// </summary>
        public int DB_GBN { get; set; }
        public string DB_IP { get; set; }
        public string DB_URL { get; set; }        
        public int DB_PORT { get; set; }
        public string DB_USERID { get; set; }
        public string DB_PASSWD { get; set; }
        //public int FTP_GBN { get; set; }
        //public string FTP_IP { get; set; }
        //public int FTP_PORT { get; set; }
        //public string FTP_USERID { get; set; }
        //public string FTP_PASSWD { get; set; }        
        //public string SERVER_URL { get; set; }
        //public int SERVER_PORT{ get; set; }
    }
}