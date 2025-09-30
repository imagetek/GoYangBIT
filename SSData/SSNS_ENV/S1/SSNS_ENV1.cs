using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommon;

namespace SSData
{
    public struct SSNS_ENV1_PROTOCOL
    {
        public byte STX1;
        public byte STX2;
        /// <summary>
        /// SIZE : 2Byte
        /// </summary>
        public byte[] SIZE;
        public byte MACHINE_CODE;
        public byte OPCODE;
        /// <summary>
        /// DATA : NByte
        /// </summary>
        public byte[] DATA;
        public byte CHECK_SUM;

        //내부용
        public int RESULT_CD;
        public string RESULT_MSG;
        public SSNS_ENV1_MACHINE_TYPE MC_GBN;
        public SSNS_ENV1_OPCODE_TYPE OP_GBN;
        //public SSNS_ENV1_ToRTU_OPCODE_TYPE TO_GBN;
        //public SSNS_ENV1_FromRTU_OPCODE_TYPE FROM_GBN;
    }

    public enum SSNS_ENV1_MACHINE_TYPE : byte
    {
        SBC = 0x00,
        ENV_BOARD = 0x1,
        PDA = 0x2,
        LED = 0xD1
    }

    public enum SSNS_ENV1_OPCODE_TYPE : byte
    {
        /// <summary>
        /// 상태정보 요청 To RTU : ACK 0x13
        /// </summary>
        상태정보요청 = 0x11,
        /// <summary>
        /// 장치제어 요청 To RTU : ACK 0x13
        /// </summary>
        장치제어설정 = 0x12,
        /// <summary>
        /// 상태정보 전송 To PC 
        /// </summary>
        상태정보응답 = 0x13,
        //내부용
        DEFAULT = 0x00,
    }

    /// <summary> 
    /// 0x13 장치상태
    /// </summary>
    public class SSNS_ENV1_STATE
    {
        public int nVersion { get; set; }
		public string 버전정보
		{
			get
			{
				string Ver = nVersion.ToString();
				if (Ver.Length == 1) Ver += "0";

				string disp = string.Format("v{0}.{1}", Ver.Substring(0, 1), Ver.Substring(1, 1));
				return disp;
			}
		}
		public int nMainPower { get; set; }
		public string 메인전원
		{
			get
			{
				//string disp = string.Format("{0:2} V", nMainPower * 24.17/255);
    //            Console.WriteLine("메인전원 V1 : {0}v", disp);
                string disp = string.Format("{0:2} V", (nMainPower * 4 * 30 / 1024) + 0.5);
                return disp;
			}
		}
		public int nBatteryPower { get; set; }
		public string 배터리전원
		{
			get
			{
                //string disp = string.Format("{0:2} V", nMainPower * 24.17 / 255);
                string disp = string.Format("{0:2} v", (nBatteryPower * 4 * 30 / 1024) + 0.5);
				return disp;
			}
		}
		public int nTemparature1 { get; set; }
		public string 온도1
		{
			get
			{
				string disp = string.Format("{0} °C", nTemparature1);
				return disp;
			}
		}
		/// <summary>
		/// 온도2 
		/// </summary>
		public int nTemparature2 { get; set; }
		public string 온도2
		{
			get
			{
				string disp = string.Format("{0} °C", nTemparature1);
				return disp;
			}
		}
		/// <summary>
		/// 온도3 미사용
		/// </summary>
		public int nTemparature3 { get; set; }
		public string 온도3
		{
			get
			{
				string disp = string.Format("{0} °C", nTemparature3);
				return disp;
			}
		}
		public int nHumidity { get; set; }
		public string 습도
		{
			get
			{
				string disp = string.Format("{0} %", nHumidity);
				return disp;
			}
		}
        public int nLuminance { get; set; }
        public string 휘도
        {
            get
            {
                string disp = string.Format("{0} nit", nLuminance);
                return disp;
            }
        }
        public int nShock1 { get; set; }
		public string 충격1
		{
			get
			{
				string disp = string.Format("{0} kg·m/s", nShock1);
				return disp;
			}
		}
		//public int nShock2 { get; set; }
		//public string 충격2
		//{
		//	get
		//	{
		//		string disp = string.Format("{0} kg·m/s", nShock2);
		//		return disp;
		//	}
		//}
		public int nIllumination { get; set; }
		public string 조도
		{
			get
			{
				string disp = string.Format("{0} lx", nIllumination);
				return disp;
			}
		}

		public int nFanSpeed1 { get; set; }
		public string 팬속도1
		{
			get
			{
				string disp = string.Format("{0} RPM", nFanSpeed1 * 30);
				return disp;
			}
		}

		public int nFanSpeed2 { get; set; }
        public string 팬속도2
        {
            get
            {
                string disp = string.Format("{0} RPM", nFanSpeed2 * 30);
                return disp;
            }
        }
        public int nFanSpeed3 { get; set; }
        public string 팬속도3
        {
            get
            {
                string disp = string.Format("{0} RPM", nFanSpeed3 * 30);
                return disp;
            }
        }
        public int nFanSpeed4 { get; set; }
        public string 팬속도4
        {
            get
            {
                string disp = string.Format("{0} RPM", nFanSpeed4 * 30);
                return disp;
            }
        }
        public byte bState1 { get; set; }
        public byte bState2 { get; set; }
        public SSNS_ENV1_DEVICE_STATE DEVICE_STATE { get; set; }
        public int nPeriod { get; set; }
		public string 주기제어
		{
			get
			{
				string disp = string.Format("{0}분", nPeriod);
				return disp;
			}
		}
		public int nWatchDog { get; set; }
    }
    public class SSNS_ENV1_DEVICE_STATE
    {
        public bool bACLAMP { get; set; }
        public string ACLAMP전원
        {
            get
            {
                return bACLAMP == false ? "OFF" : "ON";
            }
        }
        public bool bACFAN { get; set; }
        public string ACFAN전원
        {
            get
            {
                return bACFAN == false ? "OFF" : "ON";
            }
        }
        public bool bHEATER { get; set; }
        public string HEATER전원
        {
            get
            {
                return bHEATER == false ? "OFF" : "ON";
            }
        }
        public bool bCDMA { get; set; }
        public string CDMA전원
        {
            get
            {
                return bCDMA == false ? "OFF" : "ON";
            }
        }
        public bool bMODEM { get; set; }
        public string MODEM전원
        {
            get
            {
                return bMODEM == false ? "OFF" : "ON";
            }
        }

        public bool bFAN { get; set; }
        public string FAN전원
        {
            get
            {
                return bFAN == false ? "OFF" : "ON";
            }
        }

        public bool bLCD { get; set; }
        public string LCD전원
        {
            get
            {
                return bLCD == false ? "OFF" : "ON";
            }
        }

        public bool bDOOR { get; set; }
        public string 도어상태
        {
            get
            {
                return bDOOR == false ? "OPEN" : "CLOSE";
            }
        }
        /// <summary>
        /// 2번째 7번째
        /// </summary>
        public bool bCAMERA { get; set; }
        public string 카메라전원
        {
            get
            {
                return bCAMERA == false ? "OFF" : "ON";
            }
        }
    }
}
