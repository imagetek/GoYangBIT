using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;

namespace SSData
{
    public class RegisterManager
    {
        public static string GET_APPS_DIR(프로그램Type item)
        {
            try
            {
                string KEY = "";
				switch (item)
				{
                    default: KEY = AppConfig.REG_KEY; break;
				}
                object objDIR = RegistryUtils.RegReader(KEY, "APP_DIR");
                if (objDIR != null && objDIR.ToString().Equals("") == false)
                {
                    return objDIR.ToString();
                }

                return "";
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return "";
            }
        }

        public static bool SET_APPS_DIR(프로그램Type item,string DIR)
        {
            try
            {
                string KEY = "";
                switch (item)
                {                    
                    default: KEY = AppConfig.REG_KEY; break;
                }
                return RegistryUtils.RegWriter(KEY, "APP_DIR", DIR);
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }
            
        public static string GET_CLIENT_ID(프로그램Type item)
        {
            try
            {
                string KEY = "";
                switch (item)
                {
                    default: KEY = AppConfig.REG_KEY; break;
                }

                object objDIR = RegistryUtils.RegReader(KEY, "CLIENT_ID");
                if (objDIR != null && objDIR.ToString().Equals("") == false)
                {
                    return objDIR.ToString();
                }

                return "";
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return "";
            }
        }

        public static bool SET_CLIENT_ID(프로그램Type item, string CLIENT_ID)
        {
            try
            {
                string KEY = "";
                switch (item)
                {
                    default: KEY = AppConfig.REG_KEY; break;
                }

                return RegistryUtils.RegWriter(KEY, "CLIENT_ID", CLIENT_ID);
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static List<int> GET_PGM_POS(프로그램Type item)
        {
            try
            {
                string KEY = "";
                switch (item)
                {
                    default: KEY = AppConfig.REG_KEY; break;
                }

                List<int> items = new List<int>();
                object objX= RegistryUtils.RegReader(KEY, "POS_X");
                if (objX != null && objX.ToString().Equals("") == false)
                {
                    items.Add(Convert.ToInt32(objX));
                }

                object objY = RegistryUtils.RegReader(KEY, "POS_Y");
                if (objY != null && objY.ToString().Equals("") == false)
                {
                    items.Add(Convert.ToInt32(objY));
                }

                if (items != null && items.Count == 2) return items;

                return null;
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        public static bool SET_PGM_POS(프로그램Type item, int POS_X , int POS_Y)
        {
            try
            {
                string KEY = "";
                switch (item)
                {
                    default: KEY = AppConfig.REG_KEY; break;
                }

                bool 저장YN = RegistryUtils.RegWriter(KEY, "POS_X", POS_X);
                if ( 저장YN == false) return false;

                저장YN = RegistryUtils.RegWriter(KEY, "POS_Y", POS_Y);
                if (저장YN == false) return false;

                return 저장YN;
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static string GET_OS_NAME()
        {
            try
            {
                //public const string REG_KEY = "Software\\DYNAMAX\\LANDMAXS5";
                return RegistryUtils.RegReaderByMachine("Software\\Microsoft\\Windows NT\\CurrentVersion", "ProductName").ToString();
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        //public static string GET_PASSWD(프로그램Type item)
        //{
        //    try
        //    {
        //        string KEY = "";
        //        switch (item)
        //        {
        //            case 프로그램Type.KIOSK: KEY = AppConfig.REG_KEY_KIOSK; break;
        //            case 프로그램Type.TICKET: KEY = AppConfig.REG_KEY_TICKET; break;
        //            case 프로그램Type.MANAGER: KEY = AppConfig.REG_KEY_MANAGE; break;
        //        }

        //        object objDIR = RegistryUtils.RegReader(KEY, "PASSWD");
        //        if (objDIR != null && objDIR.ToString().Equals("") == false)
        //        {
        //            string item비번 = CryptionUtils.DECRYPT_BY_AES256(objDIR.ToString(), AppConfig.CRYPT_KEY, Encoding.UTF8);
        //            return item비번;
        //        }

        //        return "";
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return "";
        //    }
        //}

        //public static bool SET_PASSWD(프로그램Type item, string SERVICE_ID)
        //{
        //    try
        //    {
        //        string KEY = "";
        //        switch (item)
        //        {
        //            case 프로그램Type.KIOSK: KEY = AppConfig.REG_KEY_KIOSK; break;
        //            case 프로그램Type.TICKET: KEY = AppConfig.REG_KEY_TICKET; break;
        //            case 프로그램Type.MANAGER: KEY = AppConfig.REG_KEY_MANAGE; break;
        //        }

        //        string PASSWD = CryptionUtils.ENCRYPT_BY_AES256(SERVICE_ID, AppConfig.CRYPT_KEY, Encoding.UTF8);
        //        return RegistryUtils.RegWriter(KEY, "PASSWD", PASSWD);
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return false;
        //    }
        //}

    }
}
