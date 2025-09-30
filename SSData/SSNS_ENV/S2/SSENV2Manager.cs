using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;
using FluentFTP.Helpers;
using Newtonsoft.Json.Serialization;
using SSCommon;

namespace SSData
{
	public class SSENV2Manager
	{
		static SSENV2Manager()
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

		static SSENV2Modules mENV2 = null;

		public delegate void 보드상태정보Handler(SSNS_ENV2_BOARD_STATE _data);
		public static event 보드상태정보Handler On보드상태정보Event;

		public delegate void 상태정보Handler(SSNS_ENV2_STATE _data);
		public static event 상태정보Handler On상태정보Event;

		public delegate void 설정정보Handler(SSNS_ENV2_CONFIG _data);
		public static event 설정정보Handler On설정정보Event;

		public delegate void 센서정보Handler(SSNS_ENV2_SENSOR _data);
		public static event 센서정보Handler On센서정보Event;

		public delegate void 연결종료Handler();
		public static event 연결종료Handler On연결종료Event;

		public static void ConnectProc(string strPortName, int baudRate)
		{
			try
			{
				if (mENV2 == null)
				{
					mENV2 = new SSENV2Modules();
					mENV2.OnDataReceivedEvent += mENV2_OnDataReceivedEvent;
					mENV2.OnDisConnectedEvent += mENV2_OnDisConnectedEvent;
				}

				mENV2.InitProc(strPortName, baudRate);
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
				if (mENV2 == null)
				{
					Log4Manager.WriteENVLog(Log4Level.Error, "SerialPort가 연결되어 있지않습니다.");
					return;
				}

				mENV2.DoFinal();
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
				if (mENV2 == null) return false;

				return mENV2.IsOpen;
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
				if (mENV2 != null)
				{
					//mENV2.CloseSerialProc();
					mENV2.DoFinal();
					mENV2 = null;
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

		private static void mENV2_OnDisConnectedEvent()
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
		/// TCON이미지쓰기 0x40 
		/// </summary>
		public static void TCON이미지쓰기(byte[] bStr, int X, int Y, int W, int H)	//pjh
		{
			try
			{
				if (bStr.Length > (2560*1440)/4)
				{
					throw new Exception("TCON이미지쓰기에서 바이트 크기가 초과하였습니다.");
				}
				// Thread.Yield();
				Thread.Sleep(0);
				// [출처] C# - Thread.Yield와 Thread.Sleep(0)의 차이점(?)|작성자 techshare

				Stopwatch watch = new Stopwatch();
				watch.Start();

				Thread.Sleep(3000);   // 1000은 1초

				watch.Stop();
				Console.WriteLine($"T캡쳐와Thread.Sleep : {watch.ElapsedMilliseconds} ms");


				List<byte> bytes = new List<byte>();
				bytes.Add(STX);
				bytes.Add(STX);

				int nSize = (int)(3 + bStr.Length + 8);
				byte[] byteSize = BitConvertUtils.FromInt32(nSize, true);
				bytes.Add(byteSize[2]);
				bytes.Add(byteSize[3]);

				bytes.Add((byte)SSNS_ENV2_MACHINE_TYPE.ENV_BOARD);
				bytes.Add(byteSize[1]);	//bytes.Add((byte)SSNS_ENV2_EVENT_TYPE.DEFAULT);
				bytes.Add((byte)SSNS_ENV2_OPCODE_TYPE.TCON_비트맵_쓰기);

				byteSize = BitConvertUtils.FromInt16((short)X, true);
				bytes.AddRange(byteSize);
				byteSize = BitConvertUtils.FromInt16((short)Y, true);
				bytes.AddRange(byteSize);
				byteSize = BitConvertUtils.FromInt16((short)W, true);
				bytes.AddRange(byteSize);
				byteSize = BitConvertUtils.FromInt16((short)H, true);
				bytes.AddRange(byteSize);

				bytes.AddRange(bStr);

				bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));

				//string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
				//Console.WriteLine(hexString);

				if (mENV2 == null)
				{
				//	Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[TCON이미지쓰기] 환경보드 미접속 : {0}", hexString));
				}
				else
				{
					if (mENV2.SendData(bytes.ToArray()) == false)
					{
						Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[TCON이미지쓰기] SendData전송실패"));
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
				byte[] byteSize = BitConvertUtils.FromInt16(3, true);
				bytes.AddRange(byteSize);
				bytes.Add((byte)SSNS_ENV2_MACHINE_TYPE.ENV_BOARD);
				bytes.Add((byte)SSNS_ENV2_EVENT_TYPE.DEFAULT);
				bytes.Add((byte)SSNS_ENV2_OPCODE_TYPE.상태정보요청);

				//DATA NULL
				bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));

				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
				Console.WriteLine(hexString);
				if (mENV2 == null)
				{
					Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[상태정보요청] 환경보드 미접속 : {0}", hexString));
				}
				else
				{
					if (mENV2.SendData(bytes.ToArray()) == false)
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
		public static void Send장치설정요청(byte bControl1, byte bControl2, int 주기값, int PC제어)
		{
			try
			{
				List<byte> bytes = new List<byte>();
				bytes.Add(STX);
				bytes.Add(STX);
				short nSize = 3 + 4;
				byte[] byteSize = BitConvertUtils.FromInt16(nSize, true);
				bytes.AddRange(byteSize);
				bytes.Add((byte)SSNS_ENV2_MACHINE_TYPE.ENV_BOARD);
				bytes.Add((byte)SSNS_ENV2_EVENT_TYPE.DEFAULT);
				bytes.Add((byte)SSNS_ENV2_OPCODE_TYPE.장치제어설정);
				//DATA 
				byte bCycle = (byte)주기값;
				byte bPC = (byte)PC제어;
				bytes.Add(bControl1);
				bytes.Add(bControl2);
				bytes.Add(bCycle);
				bytes.Add(bPC);

				bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));
				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
				Console.WriteLine(hexString);
				if (mENV2 == null)
				{
					Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[장치설정] 환경보드 미접속 : {0}", hexString));
				}
				else
				{
					if (mENV2.SendData(bytes.ToArray()) == false)
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

		/// <summary>
		/// 장치볼륨상태요청 0x15
		/// </summary>
		public static void Send볼륨상태요청()
		{
			try
			{
				List<byte> bytes = new List<byte>();
				bytes.Add(STX);
				bytes.Add(STX);
				short nSize = 3;
				byte[] byteSize = BitConvertUtils.FromInt16(nSize, true);
				bytes.AddRange(byteSize);
				bytes.Add((byte)SSNS_ENV2_MACHINE_TYPE.ENV_BOARD);
				bytes.Add((byte)SSNS_ENV2_EVENT_TYPE.DEFAULT);
				bytes.Add((byte)SSNS_ENV2_OPCODE_TYPE.볼륨상태요청);
				//DATA NULL

				bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));

				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
				Console.WriteLine(hexString);
				if (mENV2 == null)
				{
					Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[볼륨상태요청] 환경보드 미접속 : {0}", hexString));
				}
				else
				{
					if (mENV2.SendData(bytes.ToArray()) == false)
					{
						Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[볼륨상태요청] SendData전송실패 : {0}", hexString));
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
		/// 장치볼륨설정 0x16 - 볼륨크기 0~10
		/// </summary>
		public static void Send볼륨설정(int 볼륨크기)
		{
			try
			{
				List<byte> bytes = new List<byte>();
				bytes.Add(STX);
				bytes.Add(STX);
				short nSize = 3 + 1;
				byte[] byteSize = BitConvertUtils.FromInt16(nSize, true);
				bytes.AddRange(byteSize);
				bytes.Add((byte)SSNS_ENV2_MACHINE_TYPE.ENV_BOARD);
				bytes.Add((byte)SSNS_ENV2_EVENT_TYPE.DEFAULT);
				bytes.Add((byte)SSNS_ENV2_OPCODE_TYPE.볼륨상태설정);
				//DATA 
				byte bVolumns = (byte)볼륨크기;
				bytes.Add(bVolumns);

				bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));

				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
				Console.WriteLine(hexString);
				if (mENV2 == null)
				{
					Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[볼륨상태설정] 환경보드 미접속 : {0}", hexString));
				}
				else
				{
					if (mENV2.SendData(bytes.ToArray()) == false)
					{
						Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[볼륨상태설정] SendData전송실패 : {0}", hexString));
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
		/// 환경보드상태요청->보드시간요청 0x17
		/// </summary>
		public static void Send환경보드시간요청()
		{
			try
			{
				List<byte> bytes = new List<byte>();
				bytes.Add(STX);
				bytes.Add(STX);
				short nSize = 3;
				byte[] byteSize = BitConvertUtils.FromInt16(nSize, true);
				bytes.AddRange(byteSize);
				bytes.Add((byte)SSNS_ENV2_MACHINE_TYPE.ENV_BOARD);
				bytes.Add((byte)SSNS_ENV2_EVENT_TYPE.DEFAULT);
				bytes.Add((byte)SSNS_ENV2_OPCODE_TYPE.ENV보드시간요청);
				//DATA NULL
				bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));

				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
				Console.WriteLine(hexString);
				if (mENV2 == null)
				{
					Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[보드시간요청] 환경보드 미접속 : {0}", hexString));
				}
				else
				{
					if (mENV2.SendData(bytes.ToArray()) == false)
					{
						Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[보드시간요청] SendData전송실패 : {0}", hexString));
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
		/// 환경보드상태요청->보드시간요청 0x18
		/// </summary>
		public static void Send환경보드시간설정(string mDateTime)
		{
			try
			{
				if (mDateTime.Length != 14) return;

				//7E-7E-00-11-01-00-18-32-30-32-32-30-35-31-32-31-30-30-36-32-32-0B
				List<byte> bytes = new List<byte>();
				bytes.Add(STX);
				bytes.Add(STX);
				short nSize = 3 + 14;
				byte[] byteSize = BitConvertUtils.FromInt16(nSize, true);
				bytes.AddRange(byteSize);
				bytes.Add((byte)SSNS_ENV2_MACHINE_TYPE.ENV_BOARD);
				bytes.Add((byte)SSNS_ENV2_EVENT_TYPE.DEFAULT);
				bytes.Add((byte)SSNS_ENV2_OPCODE_TYPE.ENV보드시간설정);
				//DATA
				byte[] bRTC = Encoding.Default.GetBytes(mDateTime);
				bytes.AddRange(bRTC);
				bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));
				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
				Console.WriteLine(hexString);
				if (mENV2 == null)
				{
					Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[보드시간설정] 환경보드 미접속 : {0}", hexString));
				}
				else
				{
					if (mENV2.SendData(bytes.ToArray()) == false)
					{
						Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[보드시간설정] SendData전송실패 : {0}", hexString));
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
		/// 환경보드설정값요청 0x19
		/// </summary>
		public static void Send환경보드설정값요청()
		{
			try
			{
				List<byte> bytes = new List<byte>();
				bytes.Add(STX);
				bytes.Add(STX);
				short nSize = 3;
				byte[] byteSize = BitConvertUtils.FromInt16(nSize, true);
				bytes.AddRange(byteSize);
				bytes.Add((byte)SSNS_ENV2_MACHINE_TYPE.ENV_BOARD);
				bytes.Add((byte)SSNS_ENV2_EVENT_TYPE.DEFAULT);
				bytes.Add((byte)SSNS_ENV2_OPCODE_TYPE.ENV보드설정값);
				//DATA NULL
				bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));

				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
				Console.WriteLine(hexString);
				if (mENV2 == null)
				{
					Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[ENV설정값요청] 환경보드 미접속 : {0}", hexString));
				}
				else
				{
					if (mENV2.SendData(bytes.ToArray()) == false)
					{
						Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[ENV설정값요청] SendData전송실패 : {0}", hexString));
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
		/// DC FAN설정 0x1A - 0 : 수동 , 1 : 자동  , 온도는 -128 ~ 127
		/// </summary>
		public static void SendDCFAN설정(int 동작Mode, int ON온도, int OFF온도)
		{
			try
			{
				//7E-7E-00-06-01-00-1A-00-1B-19-1F : DC FAN 설정                
				List<byte> bytes = new List<byte>();
				bytes.Add(STX);
				bytes.Add(STX);
				short nSize = 3 + 3;
				byte[] byteSize = BitConvertUtils.FromInt16(nSize, true);
				bytes.AddRange(byteSize);
				bytes.Add((byte)SSNS_ENV2_MACHINE_TYPE.ENV_BOARD);
				bytes.Add((byte)SSNS_ENV2_EVENT_TYPE.DEFAULT);
				bytes.Add((byte)SSNS_ENV2_OPCODE_TYPE.ENV보드설정FAN);
				//DATA 
				byte bMode = (byte)동작Mode;
				byte bOn = (byte)ON온도;
				byte bOff = (byte)OFF온도;
				bytes.Add(bMode);
				bytes.Add(bOn);
				bytes.Add(bOff);
				//
				bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));
				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
				Console.WriteLine(hexString);
				if (mENV2 == null)
				{
					Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[DCFAN설정] 환경보드 미접속 : {0}", hexString));
				}
				else
				{
					if (mENV2.SendData(bytes.ToArray()) == false)
					{
						Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[DCFAN설정] SendData전송실패 : {0}", hexString));
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
		/// 히터설정 0x1B -  0 : 수동 , 1 : 자동  , 온도는 -128 ~ 127
		/// </summary>
		public static void Send히터설정(int 동작Mode, int ON온도, int OFF온도)
		{
			try
			{
				//7E-7E-00-06-01-00-1B-01-0A-32-25 : AC1 히터온도설정
				List<byte> bytes = new List<byte>();
				bytes.Add(STX);
				bytes.Add(STX);
				short nSize = 3 + 3;
				byte[] byteSize = BitConvertUtils.FromInt16(nSize, true);
				bytes.AddRange(byteSize);
				bytes.Add((byte)SSNS_ENV2_MACHINE_TYPE.ENV_BOARD);
				bytes.Add((byte)SSNS_ENV2_EVENT_TYPE.DEFAULT);
				bytes.Add((byte)SSNS_ENV2_OPCODE_TYPE.ENV보드설정히터);
				//DATA 
				byte bMode = (byte)동작Mode;
				byte bOn = (byte)ON온도;
				byte bOff = (byte)OFF온도;
				bytes.Add(bMode);
				bytes.Add(bOn);
				bytes.Add(bOff);
				//
				bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));
				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
				Console.WriteLine(hexString);
				if (mENV2 == null)
				{
					Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[히터설정] 환경보드 미접속 : {0}", hexString));
				}
				else
				{
					if (mENV2.SendData(bytes.ToArray()) == false)
					{
						Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[히터설정] SendData전송실패 : {0}", hexString));
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
		/// LAMP설정 0x1C - 0 : 수동 , 1 : 자동 
		/// </summary>
		public static void SendLAMP설정(int 동작Mode, int ON시간, int ON분, int OFF시간, int OFF분)
		{
			try
			{
				//7E-7E-00-06-01-00-1B-01-0A-32-25 : AC1 히터온도설정
				List<byte> bytes = new List<byte>();
				bytes.Add(STX);
				bytes.Add(STX);
				short nSize = 3 + 5;
				byte[] byteSize = BitConvertUtils.FromInt16(nSize, true);
				bytes.AddRange(byteSize);
				bytes.Add((byte)SSNS_ENV2_MACHINE_TYPE.ENV_BOARD);
				bytes.Add((byte)SSNS_ENV2_EVENT_TYPE.DEFAULT);
				bytes.Add((byte)SSNS_ENV2_OPCODE_TYPE.ENV보드설정LAMP);
				//DATA 
				byte bMode = (byte)동작Mode;
				byte bOnHH = (byte)ON시간;
				byte bOnMM = (byte)ON분;
				byte bOffHH = (byte)OFF시간;
				byte bOffMM = (byte)OFF분;
				bytes.Add(bMode);
				bytes.Add(bOnHH);
				bytes.Add(bOnMM);
				bytes.Add(bOffHH);
				bytes.Add(bOffMM);
				//
				bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));
				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
				Console.WriteLine(hexString);
				if (mENV2 == null)
				{
					Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[LAMP설정] 환경보드 미접속 : {0}", hexString));
				}
				else
				{
					if (mENV2.SendData(bytes.ToArray()) == false)
					{
						Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[LAMP설정] SendData전송실패 : {0}", hexString));
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
		/// 화면장치설정 0x1D - 0 : 수동 , 1 : 자동 
		/// </summary>
		public static void Send화면장치설정(int 동작Mode, int ON시간, int ON분, int OFF시간, int OFF분)
		{
			try
			{
				//7E-7E-00-06-01-00-1B-01-0A-32-25 : AC1 히터온도설정
				List<byte> bytes = new List<byte>();
				bytes.Add(STX);
				bytes.Add(STX);
				short nSize = 3 + 5;
				byte[] byteSize = BitConvertUtils.FromInt16(nSize, true);
				bytes.AddRange(byteSize);
				bytes.Add((byte)SSNS_ENV2_MACHINE_TYPE.ENV_BOARD);
				bytes.Add((byte)SSNS_ENV2_EVENT_TYPE.DEFAULT);
				bytes.Add((byte)SSNS_ENV2_OPCODE_TYPE.ENV보드설정화면장치);
				//DATA 
				byte bMode = (byte)동작Mode;
				byte bOnHH = (byte)ON시간;
				byte bOnMM = (byte)ON분;
				byte bOffHH = (byte)OFF시간;
				byte bOffMM = (byte)OFF분;
				bytes.Add(bMode);
				bytes.Add(bOnHH);
				bytes.Add(bOnMM);
				bytes.Add(bOffHH);
				bytes.Add(bOffMM);
				//
				bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));
				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
				Console.WriteLine(hexString);
				if (mENV2 == null)
				{
					Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[화면장치설정] 환경보드 미접속 : {0}", hexString));
				}
				else
				{
					if (mENV2.SendData(bytes.ToArray()) == false)
					{
						Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[화면장치설정] SendData전송실패 : {0}", hexString));
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
		/// 주기값외 설정 0x24
		/// </summary>
		public static void Send주기및충격값설정(int 주기값, int 충격값, int 조도값, int 휘도값)
		{
			try
			{
				List<byte> bytes = new List<byte>();
				bytes.Add(STX);
				bytes.Add(STX);
				byte[] byteSize = BitConvertUtils.FromInt16(3 + 4, true); //데이터 4byte
				bytes.AddRange(byteSize);
				bytes.Add((byte)SSNS_ENV2_MACHINE_TYPE.ENV_BOARD);
				bytes.Add((byte)SSNS_ENV2_EVENT_TYPE.DEFAULT);
				bytes.Add((byte)SSNS_ENV2_OPCODE_TYPE.ENV주기값등설정);
				//DATA 
				byte bPeriod = (byte)주기값;
				byte bShock = (byte)충격값;
				byte bIllumination = (byte)조도값;
				byte bLuminance = (byte)휘도값;
				bytes.Add(bPeriod);
				bytes.Add(bShock);
				bytes.Add(bIllumination);
				bytes.Add(bLuminance);

				bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));

				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
				Console.WriteLine(hexString);
				if (mENV2 == null)
				{
					Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[주기값등설정] 환경보드 미접속 : {0}", hexString));
				}
				else
				{
					if (mENV2.SendData(bytes.ToArray()) == false)
					{
						Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[주기값등설정] SendData전송실패 : {0}", hexString));
					}
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public static void Send주기충격조도휘도값요청()
		{
			try
			{
				List<byte> bytes = new List<byte>();
				bytes.Add(STX);
				bytes.Add(STX);
				byte[] byteSize = BitConvertUtils.FromInt16(3, true); //데이터 4byte
				bytes.AddRange(byteSize);
				bytes.Add((byte)SSNS_ENV2_MACHINE_TYPE.ENV_BOARD);
				bytes.Add((byte)SSNS_ENV2_EVENT_TYPE.DEFAULT);
				bytes.Add((byte)SSNS_ENV2_OPCODE_TYPE.충격센서값설정);
				//DATA              

				bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));

				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
				Console.WriteLine(hexString);
				if (mENV2 == null)
				{
					Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[주기값등요청] 환경보드 미접속 : {0}", hexString));
				}
				else
				{
					if (mENV2.SendData(bytes.ToArray()) == false)
					{
						Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[주기값등요청] SendData전송실패 : {0}", hexString));
					}
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
		#endregion

		#region ENV To PC 
		static SSNS_ENV2_STATE Recv상태정보(byte[] bytes)
		{
			try
			{
				if (bytes.Length != 17)
				{
					return null;
				}

				SSNS_ENV2_STATE item = new SSNS_ENV2_STATE();

				item.nVersion = (int)bytes[0];
				item.nMainPower = (int)bytes[1];
				item.nBattPower = (int)bytes[2];
				item.sbTemparature1 = (sbyte)bytes[3];
				item.sbTemparature2 = (sbyte)bytes[4];
				item.sbTemparature3 = (sbyte)bytes[5];
				item.nHumidity = (int)bytes[6];
				item.nLuminance = (int)bytes[7];
				item.nShock1 = (int)bytes[8];
				item.nIllumination = (int)bytes[9];
				item.nShock2 = (int)bytes[10];
				item.nNone1 = (int)bytes[11];
				item.nNone2 = (int)bytes[12];
				item.nDoor = (int)bytes[13];
				item.baControl1 = bytes[14];
				item.baControl2 = bytes[15];
				item.nPeriod = (int)bytes[16];

				System.Collections.BitArray ba1 = new System.Collections.BitArray(BitConverter.GetBytes(item.baControl1).ToArray());
				System.Collections.BitArray ba2 = new System.Collections.BitArray(BitConverter.GetBytes(item.baControl2).ToArray());

				SSNS_ENV2_CONTROL data = new SSNS_ENV2_CONTROL();
				for (int i = 0; i < ba1.Length; i++)
				{
					switch (i)
					{
						case 0: data.bAC5 = ba1[i]; break;
						case 1: data.bDCADJ = ba1[i]; break;
						case 2: data.bDC5V1 = ba1[i]; break;
						case 3: data.bDC5V2 = ba1[i]; break;
						case 4: data.bDC5V3 = ba1[i]; break;
						case 5: data.bDC12V1 = ba1[i]; break;
						case 6: data.bDC12V2 = ba1[i]; break;
						case 7: data.bDC12V3 = ba1[i]; break;
					}
				}

				for (int i = 0; i < ba2.Length; i++)
				{
					switch (i)
					{
						case 0: data.bDC24V1 = ba2[i]; break;
						case 1: data.bDC24V2 = ba2[i]; break;
						case 2: data.bDCFAN = ba2[i]; break;
						case 3: data.bAC1 = ba2[i]; break;
						case 4: data.bAC2 = ba2[i]; break;
						case 5: data.bAC3 = ba2[i]; break;
						case 6: data.bAC4 = ba2[i]; break;
						case 7: data.bDC12V4 = ba2[i]; break;
					}
					//Console.WriteLine(ba2[i]);
				}

				item.Controls = data;

				return item;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}

		static SSNS_ENV2_CONFIG RecvENV보드설정값(byte[] bytes)
		{
			try
			{
				if (bytes.Length != 16)
				{
					return null;
				}
				SSNS_ENV2_CONFIG item = new SSNS_ENV2_CONFIG();

				item.nFanMode = (int)bytes[0];
				item.nHeaterMode = (int)bytes[1];
				item.nLampMode = (int)bytes[2];
				item.nFANOnTemp = (int)bytes[3];
				item.nFANOffTemp = (int)bytes[4];

				item.nHeaterOnTemp = (int)bytes[5];
				item.nHeaterOffTemp = (int)bytes[6];
				item.nLampOnHour = (int)bytes[7];
				item.nLampOnMin = (int)bytes[8];
				item.nLampOffHour = (int)bytes[9];

				item.nLampOffMin = (int)bytes[10];
				item.nScreenMode = (int)bytes[11];
				item.nScreenOnHour = (int)bytes[12];
				item.nScreenOnMin = (int)bytes[13];
				item.nScreenOffHour = (int)bytes[14];

				item.nScreenOffMin = (int)bytes[15];

				return item;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}
		static SSNS_ENV2_SENSOR Convert충격값정보ByteArray(byte[] bytes)
		{
			try
			{
				if (bytes.Length != 4)
				{
					return null;
				}

				SSNS_ENV2_SENSOR item = new SSNS_ENV2_SENSOR();

				item.SetPeriod = (int)bytes[0];
				item.SetShock = (int)bytes[1];
				item.SetIllumination = (int)bytes[2];
				item.SetLuminance = (int)bytes[3];
				return item;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}

		static SSNS_ENV2_BOARD_STATE Convert보드상태정보ByteArray(byte[] bytes)
		{
			try
			{
				if (bytes.Length != 30)
				{
					return null;
				}

				SSNS_ENV2_BOARD_STATE item = new SSNS_ENV2_BOARD_STATE();

				byte[] data1 = new byte[14];
				byte[] data2 = new byte[14];
				item.nNone1 = (int)bytes[14];
				item.nNone2 = (int)bytes[15];
				Buffer.BlockCopy(bytes, 0, data1, 0, data1.Length);
				Buffer.BlockCopy(bytes, 16, data2, 0, data2.Length);

				item.mActiveDateTime = Encoding.Default.GetString(data1);
				item.mBoardDateTime = Encoding.Default.GetString(data2);

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
				mENV2_OnDataReceivedEvent(bytes);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		//public delegate void 이벤트정보Handler(SSNS_ENV2_EVENT_TYPE _data);
		//public static event 이벤트정보Handler On이벤트정보Event;


		private static void mENV2_OnDataReceivedEvent(byte[] receiveData)
		{
			try
			{
				Console.WriteLine("SP Receive {0}byte", receiveData.Length);

				SSNS_ENV2_PROTOCOL data = CreateENV2프로토콜ByteArray(receiveData);	//pjh parsing
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
					case SSNS_ENV2_OPCODE_TYPE.ENV보드시간요청:
				//	case SSNS_ENV2_OPCODE_TYPE.ENV보드시간요청:
						SSNS_ENV2_BOARD_STATE item보드상태 = Convert보드상태정보ByteArray(data.DATA);
						if (item보드상태 != null && On보드상태정보Event != null) On보드상태정보Event(item보드상태);
						break;

					case SSNS_ENV2_OPCODE_TYPE.볼륨상태설정:
					case SSNS_ENV2_OPCODE_TYPE.볼륨상태요청:
						if (data.DATA.Length > 0)
						{
						}
						break;

					case SSNS_ENV2_OPCODE_TYPE.상태정보응답:
						switch (data.EVENT_GBN)
						{
							case SSNS_ENV2_EVENT_TYPE.SHOCK_EVENT:
								SSNS_ENV2_STATE item상태2 = Recv상태정보(data.DATA);
								if (item상태2 != null && On상태정보Event != null) On상태정보Event(item상태2);
								break;

							case SSNS_ENV2_EVENT_TYPE.DOOR_EVENT:
								SSNS_ENV2_STATE item상태3 = Recv상태정보(data.DATA);
								if (item상태3 != null && On상태정보Event != null) On상태정보Event(item상태3);
								break;

							default:
								SSNS_ENV2_STATE item상태 = Recv상태정보(data.DATA);
								if (item상태 != null && On상태정보Event != null) On상태정보Event(item상태);
								break;
						}
						break;

					case SSNS_ENV2_OPCODE_TYPE.ENV보드설정값:
						SSNS_ENV2_CONFIG item설정 = RecvENV보드설정값(data.DATA);
						if (item설정 != null && On설정정보Event != null) On설정정보Event(item설정);
						break;

					case SSNS_ENV2_OPCODE_TYPE.충격센서값설정:
					case SSNS_ENV2_OPCODE_TYPE.ENV주기값등설정:
						SSNS_ENV2_SENSOR item센서 = Convert충격값정보ByteArray(data.DATA);
						if (item센서 != null && On센서정보Event != null) On센서정보Event(item센서);
						break;

					case SSNS_ENV2_OPCODE_TYPE.ENV보드설정FAN:
						if (data.DATA.Length > 0)
						{
						}
						break;

					case SSNS_ENV2_OPCODE_TYPE.TCON_비트맵_쓰기:	//pjh
						if (data.DATA.Length > 0)
						{
							if (data.DATA[0] == 0x06)
								Console.WriteLine($"캡쳐와 전송 Success !!");
							else
								Console.WriteLine($"캡쳐와 전송 Fail !!");
						}
						else
							Console.WriteLine($"캡쳐와 전송 Fail !!");
						break;
					
					}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		//static void ToRTU처리(SSNS_ENV2_PROTOCOL data)
		//{
		//	try
		//	{
		//		switch (data.OP_GBN)
		//		{
		//			case SSNS_ENV2_OPCODE_TYPE.상태정보:
		//				break;
		//		}
		//	}
		//	catch (Exception ee)
		//	{
		//		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//	}
		//}

		//      static void FromRTU이벤트처리(SSNS_ENV2_PROTOCOL data)
		//      {
		//          try
		//          {
		//              switch (data.OP_GBN)
		//              {
		//                  case SSNS_ENV2_OPCODE_TYPE.상태정보:
		//                      SSNS_ENV2_STATE item상태 = Convert상태정보ByteArray(data.DATA);
		//                      DataManager.SetENV2상태정보(item상태);
		//                      //if (On상태정보Event != null) On상태정보Event(item상태);
		//                      break;

		//                  case SSNS_ENV2_OPCODE_TYPE.충격값정보:
		//                      SSNS_ENV2_SENSOR item센서 = Convert충격값정보ByteArray(data.DATA);
		//                      DataManager.SetENV2센서정보(item센서);
		//                      //if (On센서설정값정보Event != null) On센서설정값정보Event(item센서);
		//                      break;

		//                  case SSNS_ENV2_OPCODE_TYPE.ENV보드상태:
		//                      SSNS_ENV2_BOARD_STATE item보드상태 = Convert보드상태정보ByteArray(data.DATA);
		//                      DataManager.SetENV2보드상태(item보드상태);
		//                      //if (On센서설정값정보Event != null) On센서설정값정보Event(item센서);
		//                      break;

		//                  case SSNS_ENV2_OPCODE_TYPE.ENV보드시간설정:
		//                      //SSNS_ENV2_BOARD_STATE item보드상태 = Convert보드상태정보ByteArray(data.DATA);
		//                      //DataManager.SetENV2보드상태정보(item보드상태);
		//                      //if (On센서설정값정보Event != null) On센서설정값정보Event(item센서);
		//                      break;
		//              }
		//          }
		//          catch (Exception ee)
		//          {
		//              TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//              System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//          }
		//      }

		//      static void FromRTU처리(SSNS_ENV2_PROTOCOL data)
		//      {
		//          try
		//          {
		//              switch (data.OP_GBN)
		//              {
		//                  case SSNS_ENV2_OPCODE_TYPE.상태정보:
		//                      SSNS_ENV2_STATE item상태 = Convert상태정보ByteArray(data.DATA);
		//                      DataManager.SetENV2상태정보(item상태);
		//                      //if (On상태정보Event != null) On상태정보Event(item상태);
		//                      break;

		//                  case SSNS_ENV2_OPCODE_TYPE.충격값정보:
		//                      SSNS_ENV2_SENSOR item센서 =Convert충격값정보ByteArray(data.DATA);
		//                      DataManager.SetENV2센서정보(item센서);
		//                      //if (On센서설정값정보Event != null) On센서설정값정보Event(item센서);
		//                      break;

		//                  case SSNS_ENV2_OPCODE_TYPE.ENV보드상태:
		//                      SSNS_ENV2_BOARD_STATE item보드상태 = Convert보드상태정보ByteArray(data.DATA);
		//                      DataManager.SetENV2보드상태(item보드상태);
		//                      //if (On센서설정값정보Event != null) On센서설정값정보Event(item센서);
		//                      break;

		//                  case SSNS_ENV2_OPCODE_TYPE.ENV보드시간설정:
		//                      Convert보드상태정보ByteArray(data.DATA);
		//                      //SSNS_ENV2_BOARD_STATE item보드상태 = Convert보드상태정보ByteArray(data.DATA);
		//                      //DataManager.SetENV2보드상태정보(item보드상태);
		//                      //if (On센서설정값정보Event != null) On센서설정값정보Event(item센서);
		//                      break;
		//              }
		//          }
		//          catch (Exception ee)
		//          {
		//              TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//              System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//          }
		//      }


		const byte STX = 0x7E;
		public static SSNS_ENV2_PROTOCOL CreateENV2프로토콜ByteArray(byte[] bytes)
		{
			try
			{
				if (bytes == null || bytes.Length == 0)
				{
					return new SSNS_ENV2_PROTOCOL() { RESULT_CD = 14, RESULT_MSG = "수신된 데이터가 null또는 0byte입니다." };
				}
				//데이터 체크부분
				if (bytes.Length < 9)
				{
					Console.WriteLine("SP Receive 수신byte가 짧습니다.");
					return new SSNS_ENV2_PROTOCOL() { RESULT_CD = 13, RESULT_MSG = "수신된 데이터가 9byte이하입니다." };
				}

				while (true)	//pjh stx 까지 스킵.
				{
					if (bytes == null || bytes.Length == 0)
					{
						Console.WriteLine("SP Receive STX1 , STX2 미일치 생략");
						return new SSNS_ENV2_PROTOCOL() { RESULT_CD = 11, RESULT_MSG = "STX 미일치" };
					}

					if (bytes[0].Equals(STX) == true)
						break;
					bytes = bytes.Skip(1).ToArray();
				}

				if (bytes[0].Equals(STX) == false || bytes[1].Equals(STX) == false)
				{
					Console.WriteLine("SP Receive STX1 , STX2 미일치 생략");
					return new SSNS_ENV2_PROTOCOL() { RESULT_CD = 11, RESULT_MSG = "STX 미일치" };
				}

				SSNS_ENV2_PROTOCOL item = new SSNS_ENV2_PROTOCOL();
				item.STX1 = bytes[0];
				item.STX2 = bytes[1];
				item.SIZE = new byte[2];
				item.SIZE[0] = bytes[2];
				item.SIZE[1] = bytes[3];
				int SZ = BitConvertUtils.ToInt16(item.SIZE) - 3;
				item.MACHINE_CODE = bytes[4];
				item.MC_GBN = (SSNS_ENV2_MACHINE_TYPE)item.MACHINE_CODE;
				item.CTRL_OPCODE = bytes[5];
				item.EVENT_GBN = (SSNS_ENV2_EVENT_TYPE)item.CTRL_OPCODE;
				item.OPCODE = bytes[6];
				item.OP_GBN = (SSNS_ENV2_OPCODE_TYPE)item.OPCODE;
				item.DATA = new byte[SZ];

				int idxNo = 0;
				while (true)
				{
					if (SZ.Equals(idxNo) == true) break;
					item.DATA[idxNo] = bytes[idxNo + 7];
					idxNo++;
				}
				item.CHECK_SUM = bytes[bytes.Length - 1];

				//데이터 체크부분
				if (item.STX1.Equals(STX) == false || item.STX2.Equals(STX) == false)
				{
					Console.WriteLine("SP Receive STX1 , STX2 미일치 생략");
					return new SSNS_ENV2_PROTOCOL() { RESULT_CD = 11, RESULT_MSG = "STX 미일치" };
				}

				byte bCheckSum = BitConvertUtils.CreateCheckSum(bytes, bytes.Length - 1);
				if (item.CHECK_SUM.Equals(bCheckSum) == false)
				{
					Console.WriteLine("체크섬 미일치 잘못된 데이터");
					return new SSNS_ENV2_PROTOCOL() { RESULT_CD = 12, RESULT_MSG = "CHECKSUM 미일치" };
				}

				item.RESULT_CD = 1;
				item.RESULT_MSG = "변환완료";
				return item;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return new SSNS_ENV2_PROTOCOL() { RESULT_CD = -1, RESULT_MSG = "Exception" };
			}
		}


		//public byte[] Get상태정보()
		//{
		//    //장치의 상태값을 요청할떄 사용하는 값을 정리해서 반환            
		//    try
		//    {
		//        SSNS_ENV2_PACKET env2Packet = new SSNS_ENV2_PACKET();
		//        env2Packet.STX1 = STX;
		//        env2Packet.STX2 = STX;
		//        env2Packet.SIZE = new byte[2];
		//        env2Packet.SIZE[0] = 0x00;
		//        env2Packet.SIZE[1] = 0x03;
		//        env2Packet.MACHINE_CODE = (byte)SSNS_ENV2_MACHINE.ENV_BOARD; //Machine Code
		//        env2Packet.CTRL_OPCODE = (byte)SSNS_ENV2_EVENT.DEFAULT;
		//        env2Packet.OPCODE = (byte)SSNS_ENV2_OPCODE.상태정보ToRTU;
		//        env2Packet.DATA = null;
		//        env2Packet.CHECK_SUM = 0x00;

		//        return CreateENV2프로토콜(env2Packet);
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        return null;
		//    }
		//}

		//public byte[] CreateENV2프로토콜(SSNS_ENV2_PACKET dataPacket)
		//{
		//    try
		//    {
		//        //패킷에 담아놓은 데이터를 byte[]에 담는다.
		//        int index = 0;
		//        byte[] returnValue = new byte[8 + (dataPacket.DATA != null ? dataPacket.DATA.Length : 0)];
		//        returnValue[index++] = dataPacket.STX1;
		//        returnValue[index++] = dataPacket.STX2;
		//        returnValue[index++] = dataPacket.SIZE[0];
		//        returnValue[index++] = dataPacket.SIZE[1];
		//        returnValue[index++] = dataPacket.MACHINE_CODE;
		//        returnValue[index++] = dataPacket.CTRL_OPCODE;
		//        returnValue[index++] = dataPacket.OPCODE;

		//        //패킷의 데이터 부분을 체크해서 데이트를 byte[]에 담아준다.
		//        if (dataPacket.DATA != null)
		//        {
		//            for (int i = 0; i < dataPacket.DATA.Length; i++)
		//            {
		//                returnValue[index++] = dataPacket.DATA[i];
		//            }
		//        }

		//        //CheckSum, need to ^ operate to check all bytes
		//        for (int j = 0; j < returnValue.Length - 1; j++)
		//        {
		//            dataPacket.CHECK_SUM = (byte)(dataPacket.CHECK_SUM ^ returnValue[j]);
		//        }

		//        returnValue[index++] = dataPacket.CHECK_SUM;
		//        return returnValue;
		//    }
		//    catch (Exception ex)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
		//        return null;
		//    }
		//}

		//ENV2장비의 각각의 Part를 제어할 때 사용하는 함수로 byte배열 연산전에 사용
		//public byte[] ControlEnv2Power(SSNS_ENV2_DEVICE Env2DeviceByte opcode, int period = 5, bool IsEnvReset = false)
		//{
		//    try
		//    {
		//        byte controlReset = 0x00;
		//        byte controlPower = 0x00;

		//        byte bytePeriod = Convert.ToByte(period);
		//        byte byteEnvReset;

		//        if(IsEnvReset)
		//        {
		//            byteEnvReset = (byte)1;
		//        }
		//        else
		//        {
		//            byteEnvReset = (byte)0;
		//        }

		//        //Atx
		//        controlReset |= opcode.byteAtx;
		//        //DC-ADJ
		//        controlReset |= (byte)(opcode.byteDCAdj << 1);
		//        //DC5v1
		//        controlReset |= (byte)(opcode.byteDC1 << 1);
		//        //DC5v2
		//        controlReset |= (byte)(opcode.byteDC2 << 1);
		//        //DC5v3
		//        controlReset |= (byte)(opcode.byteDC3 << 1);
		//        //DC12v1
		//        controlReset |= (byte)(opcode.byteDC12_1 << 1);
		//        //DC12v2
		//        controlReset |= (byte)(opcode.byteDC12_2 << 1);
		//        //DC12v3
		//        controlReset |= (byte)(opcode.byteDC12_3 << 1);

		//        //DC24v1
		//        controlPower |= opcode.byteDC24_1;
		//        //DC24v2
		//        controlPower |= (byte)(opcode.byteDC24_2 << 1);
		//        //DC FAN
		//        controlPower |= (byte)(opcode.byteDC_Fan << 1);
		//        //AC1
		//        controlPower |= (byte)(opcode.byteAC1 << 1);
		//        //AC2
		//        controlPower |= (byte)(opcode.byteAC2 << 1);
		//        //AC3
		//        controlPower |= (byte)(opcode.byteAC3 << 1);
		//        //AC4
		//        controlPower |= (byte)(opcode.byteAC4 << 1);
		//        //DC12v4
		//        controlPower |= (byte)(opcode.byteDC12_4 << 1);

		//        SSNS_ENV2_PACKET env2Packet = new SSNS_ENV2_PACKET();
		//        env2Packet.STX1 = 0x7E;
		//        env2Packet.STX2 = 0x7E;
		//        env2Packet.SIZE1 = 0x00;
		//        env2Packet.SIZE2 = 0x03;
		//        env2Packet.M_OPCODE = 0x01; //Machine Code
		//        env2Packet.C_OPCODE = 0x00;
		//        env2Packet.OPCODE = 0x12;
		//        env2Packet.DATA = new byte[4] { controlReset, controlPower, bytePeriod, byteEnvReset };
		//        env2Packet.CHECK_SUM = 0x00;

		//        Log.WriteLog(Log.Level.Info, String.Format("ENV2.cs Send Data : 0x{0:X} - 0x{1:X} ", controlReset, controlPower));

		//        return makeByteData(env2Packet);
		//    }
		//    catch (Exception ex)
		//    {
		//        Log.WriteLog(Log.Level.Error, "ENV2.cs, ControlEnv2Power(ENV2_DEVICE opcode, bool IsOn) Exception : " + ex.ToString());
		//        return null;
		//    }
		//}

		////ENV2 장비로 부터 들어오는 값들을 PC에서 사용할 수 있도록 나누어줌
		//public static SSNS_ENV2_STATE ENV2상태정보FromByte(byte[] deviceData)
		//{
		//    try
		//    {
		//        SSNS_ENV2_STATE deviceStatus = new SSNS_ENV2_STATE();

		//        //각 byte에 들어있는 값을 확인
		//        //STX1, STX2확인
		//        if(deviceData.Length == 25)
		//        {
		//            if (deviceData[0] == 0x7E && deviceData[1] == 0x7E)
		//            {
		//                Log.WriteLog(Log.Level.Info, "ENV2.cs, GetENV2Value STX1 STX2 확인");
		//                //if (deviceData[4] == 0x01)
		//                {
		//                    int index = 6;
		//                    if (deviceData[index++] == 0x13)
		//                    {
		//                        //장비 상태 정보 응답
		//                        Log.WriteLog(Log.Level.Info, "ENV2.cs, GetENV2Value 상태정보 응답");
		//                        deviceStatus.iVersionInfo = deviceData[index++]; // index 7
		//                        deviceStatus.iMainPower = deviceData[index++];  // index 8
		//                        deviceStatus.strMainPower = string.Format("{0:2}", ((deviceStatus.iMainPower * 4) * 30) / 1024 + 0.5);
		//                        deviceStatus.iBatteryPower = deviceData[index++]; // index 9
		//                        deviceStatus.strBatteryPower = string.Format("{0:2}", ((deviceStatus.iBatteryPower * 4) * 30) / 1024 + 0.5);
		//                        deviceStatus.sbTemparature1 = (sbyte)deviceData[index++]; // index 10
		//                        deviceStatus.sbTemparature2 = (sbyte)deviceData[index++]; // index 11
		//                        deviceStatus.sbTemparature3 = (sbyte)deviceData[index++]; // index 12
		//                        deviceStatus.iHumidity = deviceData[index++]; // index 13
		//                        deviceStatus.iLuminance = deviceData[index++]; // index 14
		//                        deviceStatus.iShock = deviceData[index++]; // index 15
		//                        Log.WriteLog(Log.Level.Info, "ENV2.cs, GetENV2Value 상태정보 충격값 확인 : " + deviceStatus.iShock.ToString());
		//                        deviceStatus.iCDS = deviceData[index++]; // index 16
		//                        Log.WriteLog(Log.Level.Info, "ENV2.cs, GetENV2Value 상태정보 충격값? 조도? 확인 : " + deviceStatus.iCDS.ToString());
		//                        deviceStatus.iFanSpeed1 = deviceData[index++]; // index 17
		//                        deviceStatus.iFanSpeed2 = deviceData[index++]; // index 18
		//                        deviceStatus.iFanSpeed3 = deviceData[index++]; // index 19
		//                        deviceStatus.iFanSpeed4 = deviceData[index++]; // index 20
		//                        deviceStatus.byteMachineStatus1 = deviceData[index++];  //장치상태 1 --> DC, AC등의 상태값을 가져옴, index 21
		//                        deviceStatus.byteMachineStatus2 = deviceData[index++]; //장치상태 2 --> DC, AC등의 상태값을 가져옴, index 22
		//                        deviceStatus.iStatusPeriod = deviceData[index++]; // index 23

		//                        //여기서 부터 DC, AC등의 동작을 boolean값으로 처리한다.
		//                        deviceStatus.boolAtx = ((byte)(deviceStatus.byteMachineStatus1 & 0x01)) == 0 ? false : true; //ATX Power

		//                        if (deviceStatus.boolAtx)
		//                        {
		//                            deviceStatus.strAtx = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strAtx = "OFF";
		//                        }

		//                        Log.WriteLog(Log.Level.Info, "ENV2.cs, ATX : " + deviceStatus.strAtx);

		//                        deviceStatus.boolDCAdj = ((byte)(deviceStatus.byteMachineStatus1 >> 1 & 0x01)) == 0 ? false : true; //DCAdj

		//                        if (deviceStatus.boolDCAdj)
		//                        {
		//                            deviceStatus.strDCAdj = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strDCAdj = "OFF";
		//                        }

		//                        deviceStatus.boolDC1 = ((byte)(deviceStatus.byteMachineStatus1 >> 2 & 0x01)) == 0 ? false : true; //DC1

		//                        if (deviceStatus.boolDC1)
		//                        {
		//                            deviceStatus.strDC1 = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strDC1 = "OFF";
		//                        }

		//                        deviceStatus.boolDC2 = ((byte)(deviceStatus.byteMachineStatus1 >> 3 & 0x01)) == 0 ? false : true; //DC2

		//                        if (deviceStatus.boolDC2)
		//                        {
		//                            deviceStatus.strDC2 = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strDC2 = "OFF";
		//                        }

		//                        deviceStatus.boolDC3 = ((byte)(deviceStatus.byteMachineStatus1 >> 4 & 0x01)) == 0 ? false : true; //DC3

		//                        if (deviceStatus.boolDC3)
		//                        {
		//                            deviceStatus.strDC3 = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strDC3 = "OFF";
		//                        }

		//                        deviceStatus.boolDC12_1 = ((byte)(deviceStatus.byteMachineStatus1 >> 5 & 0x01)) == 0 ? false : true; //DC12_1

		//                        if (deviceStatus.boolDC12_1)
		//                        {
		//                            deviceStatus.strDC12_1 = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strDC12_1 = "OFF";
		//                        }

		//                        deviceStatus.boolDC12_2 = ((byte)(deviceStatus.byteMachineStatus1 >> 6 & 0x01)) == 0 ? false : true; //DC12_2

		//                        if (deviceStatus.boolDC12_2)
		//                        {
		//                            deviceStatus.strDC12_2 = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strDC12_2 = "OFF";
		//                        }

		//                        deviceStatus.boolDC12_3 = ((byte)(deviceStatus.byteMachineStatus1 >> 7 & 0x01)) == 0 ? false : true; //DC12_3

		//                        if (deviceStatus.boolDC12_3)
		//                        {
		//                            deviceStatus.strDC12_3 = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strDC12_3 = "OFF";
		//                        }


		//                        deviceStatus.boolDC24_1 = ((byte)(deviceStatus.byteMachineStatus2 & 0x01)) == 0 ? false : true; //DC24_1

		//                        if (deviceStatus.boolDC24_1)
		//                        {
		//                            deviceStatus.strDC24_1 = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strDC24_1 = "OFF";
		//                        }

		//                        deviceStatus.boolDC24_2 = ((byte)(deviceStatus.byteMachineStatus2 >> 1 & 0x01)) == 0 ? false : true; //DC24_2

		//                        if (deviceStatus.boolDC24_2)
		//                        {
		//                            deviceStatus.strDC24_2 = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strDC24_2 = "OFF";
		//                        }

		//                        deviceStatus.boolDC_Fan = ((byte)(deviceStatus.byteMachineStatus2 >> 2 & 0x01)) == 0 ? false : true; //DC_FAN

		//                        if (deviceStatus.boolDCAdj)
		//                        {
		//                            deviceStatus.strDCAdj = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strDCAdj = "OFF";
		//                        }

		//                        deviceStatus.boolAC1 = ((byte)(deviceStatus.byteMachineStatus2 >> 3 & 0x01)) == 0 ? false : true; //AC1

		//                        if (deviceStatus.boolAC1)
		//                        {
		//                            deviceStatus.strAC1 = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strAC1 = "OFF";
		//                        }

		//                        deviceStatus.boolAC2 = ((byte)(deviceStatus.byteMachineStatus2 >> 4 & 0x01)) == 0 ? false : true; //AC2

		//                        if (deviceStatus.boolAC2)
		//                        {
		//                            deviceStatus.strAC2 = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strAC2 = "OFF";
		//                        }

		//                        deviceStatus.boolAC3 = ((byte)(deviceStatus.byteMachineStatus2 >> 5 & 0x01)) == 0 ? false : true; //AC3

		//                        if (deviceStatus.boolAC3)
		//                        {
		//                            deviceStatus.strAC3 = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strAC3 = "OFF";
		//                        }

		//                        deviceStatus.boolAC4 = ((byte)(deviceStatus.byteMachineStatus2 >> 6 & 0x01)) == 0 ? false : true; //AC4

		//                        if (deviceStatus.boolAC4)
		//                        {
		//                            deviceStatus.strAC4 = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strAC4 = "OFF";
		//                        }

		//                        deviceStatus.boolDC12_4 = ((byte)(deviceStatus.byteMachineStatus2 >> 7 & 0x01)) == 0 ? false : true; //DC12_4 (예비?)

		//                        if (deviceStatus.boolDC12_4)
		//                        {
		//                            deviceStatus.strDC12_4 = "ON";
		//                        }
		//                        else
		//                        {
		//                            deviceStatus.strDC12_4 = "OFF";
		//                        }
		//                    }
		//                    else
		//                    {
		//                        Log.WriteLog(Log.Level.Info, "ENV2.cs, GetENV2Value 상태정보 응답 else");
		//                    }

		//                }
		//            }
		//        }

		//        return deviceStatus;

		//    }
		//    catch(Exception ex)
		//    {
		//        Log.WriteLog(Log.Level.Error, "ENV2.cs, GetENV2Value(byte[] deviceData) Exception : " + ex.ToString());
		//        return null;
		//    }
		//}

		////OpCode

		//    case 0x15:
		//       //"장치볼륨상태요청응답"

		//        break;
		//    case 0x16:
		//        //"장치볼륨상태제어응답"
		//        break;
		//    case 0x18:
		//        //"환경보드시간설정응답"
		//        break;
		//    case 0x17:
		//        //"환경보드상태요청응답"
		//        //{
		//        //    this.Invoke((MethodInvoker)delegate
		//        //    {
		//        //        int index = 0;
		//        //        byte[] 동작시각 = new byte[14];
		//        //        byte[] RTU시각 = new byte[14];
		//        //        Buffer.BlockCopy(Data.DATA, index, 동작시각, 0, 14);
		//        //        index += 14;
		//        //        index++;
		//        //        index++;
		//        //        Buffer.BlockCopy(Data.DATA, index, RTU시각, 0, 14);
		//        //        index += 14;
		//        //        lb_동작시각.Text = Encoding.Default.GetString(동작시각);
		//        //        lb_환경보드시각.Text = Encoding.Default.GetString(RTU시각);
		//        //    });
		//        //}
		//        break;
		//    case 0x19:
		//        //"환경보드상태요청응답"
		//        //{
		//        //    this.Invoke((MethodInvoker)delegate
		//        //    {
		//        //        int index = 0;
		//        //        byte data;
		//        //        //FAN동작모드
		//        //        data = Data.DATA[index++];
		//        //        if (data == 0x00)
		//        //            cbk_FanMode.Checked = false;
		//        //        else
		//        //            cbk_FanMode.Checked = true;

		//        //        cbk_FanMode.Text = cbk_FanMode.Checked ? "자동모드" : "수동모드";

		//        //        //히터동작모드
		//        //        data = Data.DATA[index++];
		//        //        if (data == 0x00)
		//        //            cbk_히터Mode.Checked = false;
		//        //        else
		//        //            cbk_히터Mode.Checked = true;

		//        //        cbk_히터Mode.Text = cbk_히터Mode.Checked ? "자동모드" : "수동모드";

		//        //        //LAMP동작모드
		//        //        data = Data.DATA[index++];
		//        //        if (data == 0x00)
		//        //            checkBox3.Checked = false;
		//        //        else
		//        //            checkBox3.Checked = true;

		//        //        checkBox3.Text = checkBox3.Checked ? "자동" : "수동";
		//        //        //FAN자동온도값
		//        //        data = Data.DATA[index++];
		//        //        tbFanON.Text = ((sbyte)data).ToString();
		//        //        //tbFanOFF.Text = ((sbyte)(data - 5)).ToString();

		//        //        //히터자동온도값
		//        //        data = Data.DATA[index++];
		//        //        tbHeaterON.Text = ((sbyte)data).ToString();
		//        //        //tbHeaterOFF.Text = ((sbyte)(data + 5)).ToString();

		//        //        tb_lampOnTime.Text = string.Format("{0}{1}", Data.DATA[index++].ToString().PadLeft(2, '0'), Data.DATA[index++].ToString().PadLeft(2, '0'));
		//        //        tb_lampOffTime.Text = string.Format("{0}{1}", Data.DATA[index++].ToString().PadLeft(2, '0'), Data.DATA[index++].ToString().PadLeft(2, '0'));

		//        //        //LCD동작모드
		//        //        data = Data.DATA[index++];
		//        //        if (data == 0x00)
		//        //            checkBox4.Checked = false;
		//        //        else
		//        //            checkBox4.Checked = true;

		//        //        checkBox4.Text = checkBox4.Checked ? "자동" : "수동";

		//        //        textBox3.Text = string.Format("{0}{1}", Data.DATA[index++].ToString().PadLeft(2, '0'), Data.DATA[index++].ToString().PadLeft(2, '0'));
		//        //        textBox4.Text = string.Format("{0}{1}", Data.DATA[index++].ToString().PadLeft(2, '0'), Data.DATA[index++].ToString().PadLeft(2, '0'));

		//        //    });
		//        //}

		//        break;
		//    case 0x1A:
		//        //"FAN제어응답"
		//        break;
		//    case 0x1B:
		//        //"HEATHER제어응답"
		//        break;
		//    case 0x1C:
		//        //"LAMP동작시간설정응답"
		//        break;
		//    case 0x1D:
		//        //"LCD동작시간설정응답"
		//        break;
		//    case 0x1E:
		//        //"전류값 왔습니다.
		//        break;
		//    case 0x32:
		//        //"키패드 이벤트"
		//        //{
		//        //    this.Invoke((MethodInvoker)delegate
		//        //    {
		//        //        if (KeyEventTimer != null)
		//        //            KeyEventTimer.Change(1000, 1000);

		//        //        if (Data.DATA[0] == 0x01)
		//        //        {
		//        //            lb_Key메뉴.BackColor = Color.Red;
		//        //        }
		//        //        else if (Data.DATA[0] == 0x02)
		//        //        {
		//        //            lb_Key왼쪽.BackColor = Color.Red;
		//        //        }
		//        //        else if (Data.DATA[0] == 0x03)
		//        //        {
		//        //            lb_Key위로.BackColor = Color.Red;
		//        //        }
		//        //        else if (Data.DATA[0] == 0x04)
		//        //        {
		//        //            lb_Key아래.BackColor = Color.Red;
		//        //        }
		//        //        else if (Data.DATA[0] == 0x05)
		//        //        {
		//        //            lb_Key오른쪽.BackColor = Color.Red;
		//        //        }
		//        //        else if (Data.DATA[0] == 0x06)
		//        //        {
		//        //            lb_Key확인.BackColor = Color.Red;
		//        //        }
		//        //        else if (Data.DATA[0] == 0x07)
		//        //        {
		//        //            lb_Key음성.BackColor = Color.Red;
		//        //        }

		//        //    });
		//        //}
		//        break;
		//    case 0x30:
		//        //"충격 이벤트"
		//        //{
		//        //    this.Invoke((MethodInvoker)delegate
		//        //    {
		//        //        if (byte.Parse(textBox1.Text) <= Data.DATA[0])
		//        //        {
		//        //            lb_충격발생.Text = DateTime.Now.ToString();
		//        //        }
		//        //    });
		//        //}
		//        break;
		//public static int GetDetectedShock(byte[] deviceData)
		//{
		//    try
		//    {
		//        int iShock = 0;
		//        //각 byte에 들어있는 값을 확인
		//        //STX1, STX2확인
		//        //if (deviceData.Length == 25)
		//        {
		//            if (deviceData[0] == 0x7E && deviceData[1] == 0x7E)
		//            {
		//                Log.WriteLog(Log.Level.Info, "ENV2.cs, GetDetectedShock STX1 STX2 확인");
		//                //if (deviceData[4] == 0x01)
		//                {
		//                    int index = 6;
		//                    if (deviceData[index++] == 0x30)
		//                    {
		//                        //장비 상태 정보 응답
		//                        Log.WriteLog(Log.Level.Info, "ENV2.cs, GetDetectedShock 상태정보 응답");
		//                        iShock = (int)deviceData[7];
		//                    }
		//                }
		//            }
		//        }
		//        return iShock;
		//    }
		//    catch(Exception ex)
		//    {
		//        Log.WriteLog(Log.Level.Error, "ENV2.cs, GetDetectedShock Exception : " + ex.ToString());
		//        return 0;
		//    }
		//}
		//    case 0x31:
		//        //"도어 이벤트"
		//        //{
		//        //    this.Invoke((MethodInvoker)delegate
		//        //    {
		//        //        textRpm4.Text = String.Format("{0}", (int)Data.DATA[0]);
		//        //        if (int.Parse(textRpm4.Text) == 1)
		//        //        {
		//        //            chkDOOR.Checked = true;
		//        //        }
		//        //        else
		//        //        {
		//        //            chkDOOR.Checked = false;
		//        //        }
		//        //        if (chkDOOR.Checked)
		//        //        {
		//        //            textRpm4.Text = "열림";
		//        //        }
		//        //        else
		//        //        {
		//        //            textRpm4.Text = "닫힘";
		//        //        }

		//        //    });
		//        //}
		//        break;
		//}


		//장비로 부터 수신한 바이트 체크
		//public void CheckSTXETX()

	}
}
