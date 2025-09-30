using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommon;

namespace SSData
{
    public class SSENV1Manager
    {
        static SSENV1Manager()
        {
            try
            {
                //RegisterCommands();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        //public ICommand 도주차량수동등록Command { get; private set; }
        private void RegisterCommands()
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

        static SSENV1Modules mENV1 = null;
        

        public delegate void 상태정보Handler(SSNS_ENV1_STATE _data);
        public static event 상태정보Handler On상태정보Event;

        //public delegate void 설정정보Handler(SSNS_ENV1_CONFIG _data);
        //public static event 설정정보Handler On설정정보Event;

        //public delegate void 센서정보Handler(SSNS_ENV1_SENSOR _data);
        //public static event 센서정보Handler On센서정보Event;

        public delegate void 연결종료Handler();
        public static event 연결종료Handler On연결종료Event;

        public static void ConnectProc(string strPortName, int baudRate)
		{
            try
			{
                if (mENV1 == null)
                {
                    mENV1 = new SSENV1Modules();
                    mENV1.OnDataReceivedEvent += mENV1_OnDataReceivedEvent;
                    mENV1.OnDisConnectedEvent += mENV1_OnDisConnectedEvent;
                }

                mENV1.InitProc(strPortName, baudRate);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
		}

        public static void DisConnectProc()
        {
            try
            {
                if (mENV1 == null)
                {
                    Log4Manager.WriteENVLog(Log4Level.Error, "SerialPort가 연결되어 있지않습니다.");
                    return;
                }

                mENV1.DoFinal();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public static bool ConnectYN()
        {
            try
            {
                if (mENV1 == null) return false;

                return mENV1.IsOpen;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool DoFinal()
        {
            try
            {
                if (mENV1 != null)
                {
                    //mENV1.CloseSerialProc();
                    mENV1.DoFinal();
                    mENV1 = null;
                }

                GC.Collect();

                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        private static void mENV1_OnDisConnectedEvent()
		{
			try
			{
                
                Log4Manager.WriteENVLog(Log4Level.Debug, "연결이 종료되었습니다.");
                if (On연결종료Event != null) On연결종료Event();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
		}

		#region PC To ENV

        /// <summary>
        /// 상태정보 요청 0x11
        /// </summary>
		public static void Send상태정보요청()
        {
            try
            {
                //7E-7E-00-03-01-00-11-13
                List<byte> bytes = new List<byte>();
                bytes.Add(STX);
                bytes.Add(STX);
                byte[] byteSize = BitConvertUtils.FromInt16(2 , true);
                bytes.AddRange(byteSize);
                bytes.Add((byte)SSNS_ENV1_MACHINE_TYPE.ENV_BOARD);
                bytes.Add((byte)SSNS_ENV1_OPCODE_TYPE.상태정보요청);
                
                //DATA NULL
                bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));

                string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
                Console.WriteLine(hexString);
                if (mENV1 == null)
                {
                    Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[상태정보요청] 환경보드 미접속 : {0}", hexString));
                }
                else
                {
                    if (mENV1.SendData(bytes.ToArray()) == false)
                    {
                        Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[상태정보요청] SendData전송실패 : {0}", hexString));
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        /// <summary>
        /// 장치설정요청 0x12 - PC제어 : 기본 (0) , PC리셋 (1)
        /// </summary>
        public static void Send장치설정요청(byte bControl1 , byte bControl2)
        {
            try
            {
				byte bCycle = (byte)5;
				byte bPC = (byte)0;

				List<byte> bytes = new List<byte>();
                bytes.Add(STX);
                bytes.Add(STX);
                short nSize = 2 + 4;
                byte[] byteSize = BitConvertUtils.FromInt16(nSize, true);
                bytes.AddRange(byteSize);
                bytes.Add((byte)SSNS_ENV1_MACHINE_TYPE.ENV_BOARD);
                bytes.Add((byte)SSNS_ENV1_OPCODE_TYPE.장치제어설정);
                //DATA                 
                bytes.Add(bControl1);
                bytes.Add(bControl2);
                bytes.Add(bCycle);
                bytes.Add(bPC);

                bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));
                string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
                Console.WriteLine(hexString);
                if (mENV1 == null)
                {
                    Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[장치설정] 환경보드 미접속 : {0}", hexString));
                }
                else
                {
                    if (mENV1.SendData(bytes.ToArray()) == false)
                    {
                        Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[장치설정] SendData전송실패 : {0}", hexString));
                    }
                }

                //Log4Manager.WriteENVLog(string.Format("[장치설정] 설정값 전송 후 확인"));

                //Send환경보드설정값요청();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }
       
        #endregion

        #region ENV To PC 
        static SSNS_ENV1_STATE Recv상태정보(byte[] bytes)
        {
			try
			{
				if (bytes.Length < 17)
				{
					return null;
				}

				SSNS_ENV1_STATE item = new SSNS_ENV1_STATE();

				item.nVersion = (int)bytes[0];
				Console.WriteLine("{0:x2}", item.nVersion);
				item.nMainPower = (int)bytes[1];
				item.nBatteryPower = (int)bytes[2];
				item.nTemparature1 = (int)bytes[3];
				item.nTemparature2 = (int)bytes[4]; //10
				item.nTemparature3 = (int)bytes[5];
				item.nHumidity = (int)bytes[6];

				item.nLuminance = (int)bytes[7]; //13
				item.nShock1 = (int)bytes[8]; //14
				item.nIllumination = (int)bytes[9]; //15
				item.nFanSpeed1 = (int)bytes[10];
				item.nFanSpeed2 = (int)bytes[11];
				item.nFanSpeed3 = (int)bytes[12];
				item.nFanSpeed4 = (int)bytes[13]; //19
				item.bState1 = bytes[14];
				item.bState2 = bytes[15];
				item.nPeriod = (int)bytes[16];
				//item.nWatchDog = (int)bytes[17];

				SSNS_ENV1_DEVICE_STATE data = new SSNS_ENV1_DEVICE_STATE();

				string mState1 = Convert.ToString((int)item.bState1, 2);
				mState1 = mState1.PadLeft(8, '0');
				for (int i = 0; i < mState1.Length; i++)
				{
					bool bYN = mState1.Substring(i, 1).Equals("1");
					switch (i)
					{
                        case 0: data.bACLAMP = bYN; break;
                        case 1: data.bACFAN = bYN; break;
                        case 2: data.bHEATER = bYN; break;
                        case 3: data.bCDMA = bYN; break;
                        case 4: data.bMODEM = bYN; break;
                        case 5: data.bFAN = bYN; break;
                        case 6: data.bLCD = bYN; break;
                        case 7: data.bDOOR = bYN; break;
      //                  case 0: data.bDOOR = bYN; break;
						//case 1: data.bLCD = bYN; break;
						//case 2: data.bFAN = bYN; break;
						//case 3: data.bCDMA = bYN; break;
						//case 4: data.bMODEM = bYN; break;
						//case 5: data.bHEATER = bYN; break;
						//case 6: data.bACFAN = bYN; break;
						//case 7: data.bACLAMP = bYN; break;
					}
				}

				string mState2 = Convert.ToString((int)item.bState2, 2);
				mState2 = mState2.PadLeft(8, '0');
				for (int i = 0; i < mState2.Length; i++)
				{
					bool bYN = mState2.Substring(i, 1).Equals("1");
					switch (i)
					{
						//case 0: data.bACLAMP = bYN; break;
						//case 1: data.bACFAN = bYN; break;
						//case 2: data.bHEATER = bYN; break;
						//case 3: data.bCDMA = bYN; break;
						//case 4: data.bMODEM = bYN; break;
						//case 5: data.bFAN = bYN; break;
						//case 6: data.bLCD = bYN; break;
						case 7: data.bCAMERA = bYN; break;
					}
				}

				item.DEVICE_STATE = data;

				return item;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
        }

        #endregion

        public static void 테스트Proc(byte[] bytes)
        {
            try
            {
                mENV1_OnDataReceivedEvent(bytes);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        //public delegate void 이벤트정보Handler(SSNS_ENV1_EVENT_TYPE _data);
        //public static event 이벤트정보Handler On이벤트정보Event;

       
        private static void mENV1_OnDataReceivedEvent(byte[] receiveData)
		{
			try
			{
                Console.WriteLine("SP Receive {0}byte", receiveData.Length);

                SSNS_ENV1_PROTOCOL data = CreateENV1프로토콜ByteArray(receiveData);
                if (data.RESULT_CD.Equals(1) == false)
                {
                    Log4Manager.WriteENVLog(Log4Level.Debug, string.Format("[{0}] {1}", data.RESULT_CD, data.RESULT_MSG));
                    return;
                }

                Log4Manager.WriteENVLog(Log4Level.Debug, string.Format("[{0:X2}] {1} Data:{2}byte"
                    , data.OPCODE
                    , data.OP_GBN.ToString()
                    , data.DATA.Length));

				switch (data.OP_GBN)
				{
					case SSNS_ENV1_OPCODE_TYPE.상태정보응답:
						SSNS_ENV1_STATE item상태 = Recv상태정보(data.DATA);
						if (item상태 != null && On상태정보Event != null) On상태정보Event(item상태);
						break;

					default:
						if (data.DATA != null && data.DATA.Length > 0)
						{

						}
						break;
				}
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
		}

		const byte STX = 0x7E;
        public static SSNS_ENV1_PROTOCOL CreateENV1프로토콜ByteArray(byte[] bytes)
        {
            try
            {
                if (bytes == null || bytes.Length == 0)
                {
                    return new SSNS_ENV1_PROTOCOL() { RESULT_CD = 14, RESULT_MSG = "수신된 데이터가 null또는 0byte입니다." };
                }
                //데이터 체크부분
                if (bytes.Length <= 8)
                {
                    Console.WriteLine("SP Receive 수신byte가 짧습니다.");
                    return new SSNS_ENV1_PROTOCOL() { RESULT_CD = 13, RESULT_MSG = "수신된 데이터가 9byte이하입니다." };
                }
                SSNS_ENV1_PROTOCOL item = new SSNS_ENV1_PROTOCOL();
                item.STX1 = bytes[0];
                item.STX2 = bytes[1];
                item.SIZE = new byte[2];
                item.SIZE[0] = bytes[2];
                item.SIZE[1] = bytes[3];
                int SZ = BitConvertUtils.ToInt16(item.SIZE) - 2;
                item.MACHINE_CODE = bytes[4];
                item.MC_GBN = (SSNS_ENV1_MACHINE_TYPE)item.MACHINE_CODE;
                item.OPCODE = bytes[5];
                item.OP_GBN = (SSNS_ENV1_OPCODE_TYPE)item.OPCODE;
                item.DATA = new byte[SZ];

                int idxNo = 0;
                while (true)
                {
                    if (SZ.Equals(idxNo) == true) break;
                    item.DATA[idxNo] = bytes[idxNo + 6];
                    idxNo++;
                }
                item.CHECK_SUM = bytes[bytes.Length - 1];

                //데이터 체크부분
                if (item.STX1.Equals(STX) == false || item.STX2.Equals(STX) == false)
                {
                    Console.WriteLine("SP Receive STX1 , STX2 미일치 생략");
                    return new SSNS_ENV1_PROTOCOL() { RESULT_CD = 11, RESULT_MSG = "STX 미일치" };
                }

                byte bCheckSum = BitConvertUtils.CreateCheckSum(bytes, bytes.Length - 1);
                if (item.CHECK_SUM.Equals(bCheckSum) == false)
                {
                    Console.WriteLine("체크섬 미일치 잘못된 데이터");
                    return new SSNS_ENV1_PROTOCOL() { RESULT_CD = 12, RESULT_MSG = "CHECKSUM 미일치" };
                }

                item.RESULT_CD = 1;
                item.RESULT_MSG = "변환완료";
                return item;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return new SSNS_ENV1_PROTOCOL() { RESULT_CD = -1, RESULT_MSG = "Exception" };
            }
        }
	}
}

