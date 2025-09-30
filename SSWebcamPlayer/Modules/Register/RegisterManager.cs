using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommon;

namespace SSWebcamPlayer
{
    public class RegisterManager
    {
        public static string GET_APPS_DIR()
        {
            try
            {
                string KEY = AppConfig.REG_KEY;
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
    }
}
