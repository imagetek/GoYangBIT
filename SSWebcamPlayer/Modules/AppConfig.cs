using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSWebcamPlayer
{
    public class AppConfig
    {
        public const string APPS_CD = "SSPAJU";
        public const string REG_KEY = "Software\\SSNS\\PAJUBIT";
        public const string CRYPT_KEY = "SSNSBIT_2022_PAJU";
        public const string APP_KEY = "SSNST2022PAJU001";
        public static string APPSStartPath { get; set; }
        public static string CLIENT_ID { get; set; }
    }
}
