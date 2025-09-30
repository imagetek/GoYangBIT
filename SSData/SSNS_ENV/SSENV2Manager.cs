using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using SSCommonNET;

namespace SSData
{
	public class SSENV2Manager
	{
		static SSENV2Manager()
		{
			try
			{
				if (_dtStatusResponse == null)
				{
					_dtStatusResponse = new System.Windows.Threading.DispatcherTimer();
					_dtStatusResponse.Interval = TimeSpan.FromSeconds(1 * 60); //pjh  1 min send period;
					_dtStatusResponse.Tick += _dtStatusResponse_Tick;
				}
				_dtStatusResponse.Start();
				_dtStatusResponse_Tick(null, null);
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

		public delegate void 연결재시도Handler();
		public static event 연결종료Handler On연결재시도Event;
		
		public delegate void SerialPortErrorHandler();
		public static event SerialPortErrorHandler OnSerialPortErrorEvent;

		static System.Windows.Threading.DispatcherTimer _dt시간 = null;
		static System.Windows.Threading.DispatcherTimer _dtStatusInfo = null;
		static System.Windows.Threading.DispatcherTimer _dtStatusResponse = null;

		public static void ConnectProc(string strPortName, int baudRate)
		{
			try
			{
				if (mENV2 == null)
				{
					mENV2 = new SSENV2Modules();
					mENV2.OnDataReceivedEvent += mENV2_OnDataReceivedEvent;
					mENV2.OnDisConnectedEvent += mENV2_OnDisConnectedEvent;
					mENV2.OnSpErrorEvent += MENV2_OnSpErrorEvent;
				}

				mENV2.InitProc(strPortName, baudRate);

				if (_dt시간 == null)
				{
					_dt시간 = new System.Windows.Threading.DispatcherTimer();
					_dt시간.Interval = TimeSpan.FromMilliseconds(100); //pjh  send period;
					_dt시간.Tick += _dt시간_Tick;
					_dt시간.Tag = 0;
				}
				_dt시간.Start();
				_dt시간_Tick(null, null);

				if (_dtStatusInfo == null)
				{
					_dtStatusInfo = new System.Windows.Threading.DispatcherTimer();
					_dtStatusInfo.Interval = TimeSpan.FromSeconds(30);// 60); //pjhpjh  1 min send period;
					_dtStatusInfo.Tick += _dtStatusInfo_Tick;
				}
				_dtStatusInfo.Start();
				_dtStatusInfo_Tick(null, null);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private static void MENV2_OnSpErrorEvent()
		{
			OnSerialPortErrorEvent?.Invoke();
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
					mENV2.DoFinal();
					mENV2 = null;
				}

				if (_dt시간 != null)
				{
					_dt시간.Stop();
					_dt시간.Tick -= _dt시간_Tick;
					_dt시간 = null;
				}

				if (_dtStatusInfo != null)
				{
					_dtStatusInfo.Stop();
					_dtStatusInfo.Tick -= _dtStatusInfo_Tick;
					_dtStatusInfo = null;
				}

				return true;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
		}

		private static void _dt시간_Tick(object sender, EventArgs e)  // 100mS
		{
			try
			{
				if (Convert.ToInt32(_dt시간.Tag) == 1) return;
				_dt시간.Tag = 1;

				TickTCON이미지전송();

				_dt시간.Tag = 0;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				_dt시간.Tag = 0;
			}
		}

		private static void _dtStatusInfo_Tick(object sender, EventArgs e)  // 60 Sec
		{
			try
			{
				Send상태정보요청();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		static int processKillTimerSecondCount = 0;
		private static void _dtStatusResponse_Tick(object sender, EventArgs e)  // 60 Sec
		{
			if (processKillTimerSecondCount++ >= 2) //5분
			{
				processKillTimerSecondCount = 0;

				// Log4Manager.WriteENVLog(Log4Level.Debug, "상태보고가 오지않아 프로그램이 종료됩니다.");
				// TraceManager.AddLog(string.Format("### {0} ###", "상태보고가 오지않아 프로그램이 종료됩니다."));
				//System.Diagnostics.Process.GetCurrentProcess().Kill();

				//pjhpjh 5miniutes later, reopen that port. not to kill CurrentProcess.
				DisConnectProc();
				Log4Manager.WriteENVLog(Log4Level.Debug, "상태보고가 오지않아 연결을 재시도 합니다.");
				TraceManager.AddLog(string.Format("### {0} ###", "상태보고가 오지않아 연결을 재시도 합니다."));
				On연결재시도Event?.Invoke();
			}
		}

		private static void mENV2_OnDisConnectedEvent()
		{
			try
			{
				Log4Manager.WriteENVLog(Log4Level.Debug, "연결이 종료되었습니다.");
				On연결종료Event?.Invoke();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		#region PC To ENV

		static bool bResponseTconOk = true;
		static int nNoResponseTime = 0;
		static int nResponseRestartCnt = 0;
		static int nSendCnt = 0;
		static int nTotalDiscardQue = 0;

		static Queue<Array> arrayTx = new Queue<Array>();

		/// <summary>
		/// TickTCON이미지전송
		/// </summary>
		public static void TickTCON이미지전송()   //pjh
		{
			try
			{
				Array aTx;
				if (true == bResponseTconOk)    //pjh if there is response, then start send it
				{
					if (arrayTx.Count > 0)
					{
						if (SearchNewItem())
						{
							nNoResponseTime = 0;
							bResponseTconOk = false;
							nSendCnt++;

							aTx = arrayTx.Dequeue();  // InvalidOperationException
							mENV2.SendData((byte[])aTx, 0, ((byte[])aTx).Length);

							Console.WriteLine("Tx Queue Count = {0}, Length = {1}", arrayTx.Count, aTx.Length);
							DisplayLog(Log4Level.Info, string.Format("[UART] Tx Queue Count = {0}, Length = {1}", arrayTx.Count, aTx.Length), LogSource.Board);
						}
					}
					else { }	// 큐 없으면 끝
				}
				else  // Wait for response
				{
					if (++nNoResponseTime >= 200)  //pjh //200*0.1=20초  1*60=60초=1분
					{
						nNoResponseTime = 0;
						while (9 < arrayTx.Count)
						{
							aTx = arrayTx.Dequeue();
							nTotalDiscardQue++;
							DisplayLog(Log4Level.Info, string.Format("[UART] Tx nTotalDiscardQue Count = {0}", nTotalDiscardQue), LogSource.Board);
						}

						bResponseTconOk = true; // forced to restart
						nResponseRestartCnt++;

						Console.WriteLine("###### Tx Forced to Restart-Count = {0} Send-Count = {1} 버린큐 = {2}", nResponseRestartCnt, nSendCnt, nTotalDiscardQue);
						DisplayLog(Log4Level.Info, string.Format("[UART] ###### Forced to Restart-Count = {0}", nResponseRestartCnt), LogSource.Board);
						OnSerialPortErrorEvent?.Invoke();
					}
					else
						return;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
		private static bool SearchNewItem()
		{
			if (arrayTx.Count == 1)
				return true;

			int txlen = arrayTx.Peek().GetLength(0);
			bool find = false;
			int index = 0;
			foreach (Array member in arrayTx)
			{
				if (index++ == 0)
					continue;
				if (txlen == member.GetLength(0))    // if same length, remove it.
				{
					find = true;
					break;
				}
			}
			if (find)
			{
				arrayTx.Dequeue();
				return false;
			}
			else
				return true;
		}


		/// <summary>
		/// 로그파일
		/// </summary>
		public delegate void DisplayLogHandler(Log4Data log4);
		public static event DisplayLogHandler OnDisplayLogEvent;
		public static void DisplayLog(Log4Level lv, string log, LogSource logSource = LogSource.Other)
		{
			Log4Data log4 = new Log4Data();
			log4.Level = lv;
			log4.REGDATE = DateTime.Now;
			log4.로그내용 = log;
			log4.LogSource = logSource;
			OnDisplayLogEvent?.Invoke(log4);
			//TraceManager.AddInfoLog(log);
			//Log4Manager.WriteLog(lv, log);
		}

		/// <summary>
		/// TCON이미지쓰기
		/// </summary>
		public static void TCON이미지쓰기(byte[] bStr, int X, int Y, int W, int H)	//
		{
			try
			{
				if (bStr.Length > (2560 * 1440) / 4)
				{
					throw new Exception("TCON쓰기에서 바이트 크기가 초과하였습니다.");
				}

				List<byte> bytes = new List<byte>();
				bytes.Add(STX);
				bytes.Add(STX);

				int nSize = (int)(3 + bStr.Length + 8);
				byte[] byteSize = BitConvertUtils.FromInt32(nSize, true);
				bytes.Add(byteSize[2]);
				bytes.Add(byteSize[3]);

				bytes.Add((byte)SSNS_ENV2_MACHINE_TYPE.ENV_BOARD);
				bytes.Add(byteSize[1]);	//bytes.Add((byte)SSNS_ENV2_EVENT_TYPE.DEFAULT);
				bytes.Add((byte)(SSNS_ENV2_OPCODE_TYPE.TCON_비트맵_쓰기));

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

				arrayTx.Enqueue(bytes.ToArray());
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
					arrayTx.Enqueue(bytes.ToArray());
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
					arrayTx.Enqueue(bytes.ToArray());
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
					arrayTx.Enqueue(bytes.ToArray());
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
					arrayTx.Enqueue(bytes.ToArray());
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
					arrayTx.Enqueue(bytes.ToArray());
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
					arrayTx.Enqueue(bytes.ToArray());
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
					arrayTx.Enqueue(bytes.ToArray());
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
					arrayTx.Enqueue(bytes.ToArray());
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
					arrayTx.Enqueue(bytes.ToArray());
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
					arrayTx.Enqueue(bytes.ToArray());
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
				byte bLuminance = (byte)조도값;
				byte bIllumination = (byte)휘도값;
				bytes.Add(bPeriod);
				bytes.Add(bShock);
				bytes.Add(bLuminance);
				bytes.Add(bIllumination);

				bytes.Add(BitConvertUtils.CreateCheckSum(bytes.ToArray(), bytes.Count));

				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes.ToArray(), "-");
				Console.WriteLine(hexString);
				if (mENV2 == null)
				{
					Log4Manager.WriteENVLog(Log4Level.Error, string.Format("[주기값등설정] 환경보드 미접속 : {0}", hexString));
				}
				else
				{
					arrayTx.Enqueue(bytes.ToArray());
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
				bytes.Add((byte)SSNS_ENV2_OPCODE_TYPE.ENV주기정보요청);
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
					arrayTx.Enqueue(bytes.ToArray());
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
				if (bytes.Length < 22)
					return null;

				SSNS_ENV2_STATE item = new SSNS_ENV2_STATE();

				int sIndex = 0; //pjhpjh
				item.nVersion = BitConverter.ToInt16(bytes, sIndex);	sIndex += 2;

				item.MainVoltage = ((float)BitConverter.ToInt16(bytes, sIndex)) / 100; sIndex += 2; //메인전원
				item.MainCurrent = ((float)BitConverter.ToInt16(bytes, sIndex)) / 100; sIndex += 2; //메인전류
				item.BattVoltage = ((float)BitConverter.ToInt16(bytes, sIndex)) / 100; sIndex += 2; //배터리전원
				item.BattCurrent = ((float)BitConverter.ToInt16(bytes, sIndex)) / 100; sIndex += 2; //배터리전류
				item.PanlVoltage = ((float)BitConverter.ToInt16(bytes, sIndex)) / 100; sIndex += 2; //패널전압
				item.PanlCurrent = ((float)BitConverter.ToInt16(bytes, sIndex)) / 100; sIndex += 2; //패널전류

				item.Temparature1 = (sbyte)bytes[sIndex]; sIndex += 1;
				item.Humidity = (sbyte)bytes[sIndex]; sIndex += 1;
				item.Luminance = (sbyte)bytes[sIndex]; sIndex += 1;
				item.Shock1 = (sbyte)bytes[sIndex]; sIndex += 1;
				item.Door = (sbyte)bytes[sIndex]; sIndex += 1;
				item.Illumination = (sbyte)bytes[sIndex]; sIndex += 1;
				item.Temparature2 = (sbyte)bytes[sIndex]; sIndex += 1;
				item.BattPercent = (sbyte)bytes[sIndex]; sIndex += 1;
				item.RemainBat = BitConverter.ToInt16(bytes, sIndex); sIndex += 2;

				//System.Collections.BitArray ba1 = new System.Collections.BitArray(BitConverter.GetBytes((short)item.baControl1).ToArray());
				//System.Collections.BitArray ba2 = new System.Collections.BitArray(BitConverter.GetBytes((short)item.baControl2).ToArray());

				//SSNS_ENV2_CONTROL data = new SSNS_ENV2_CONTROL();
				//for (int i = 0; i < ba1.Length; i++)
				//{
				//	switch (i)
				//	{
				//		case 0: data.bAC5 = ba1[i]; break;
				//		case 1: data.bDCADJ = ba1[i]; break;
				//		case 7: data.bDC12V3 = ba1[i]; break;
				//	}
				//}

				//item.Controls = data;

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
				item.nFANOnTemp = (int)bytes[1];
				item.nFANOffTemp = (int)bytes[2];

				item.nHeaterMode = (int)bytes[3];
				item.nHeaterOnTemp = (int)bytes[4];
				item.nHeaterOffTemp = (int)bytes[5];

				item.nLampMode = (int)bytes[6];
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

		//public delegate void 이벤트정보Handler(SSNS_ENV2_EVENT_TYPE _data);
		//public static event  이벤트정보Handler On이벤트정보Event;

		static int 재시도횟수 = 0;
		private static void mENV2_OnDataReceivedEvent(byte[] receiveData)
		{
			try
			{
				SSNS_ENV2_PROTOCOL data = CreateENV2프로토콜ByteArray(receiveData);	// parsing
				if (data.RESULT_CD.Equals(1) == false)
				{
					Log4Manager.WriteENVLog(Log4Level.Debug, string.Format("[{0}] {1}", data.RESULT_CD, data.RESULT_MSG));
					return;
				}

				Log4Manager.WriteENVLog(Log4Level.Debug, string.Format("     {0:X2}=>{1} Len:{2}", data.OPCODE, data.OP_GBN.ToString(), data.DATA.Length));

				switch (data.OP_GBN)
				{
					case SSNS_ENV2_OPCODE_TYPE.ENV보드시간요청:
						SSNS_ENV2_BOARD_STATE item보드상태 = Convert보드상태정보ByteArray(data.DATA);
						if (item보드상태 != null && On보드상태정보Event != null) On보드상태정보Event(item보드상태);
						break;

					case SSNS_ENV2_OPCODE_TYPE.볼륨상태설정:
					case SSNS_ENV2_OPCODE_TYPE.볼륨상태요청:
						if (data.DATA.Length > 0)
						{}
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

						//pjh add this line for watchdog process kill
						processKillTimerSecondCount = 0;
						break;

					case SSNS_ENV2_OPCODE_TYPE.ENV보드설정값:
						SSNS_ENV2_CONFIG item설정 = RecvENV보드설정값(data.DATA);
						if (item설정 != null && On설정정보Event != null) On설정정보Event(item설정);
						break;

					case SSNS_ENV2_OPCODE_TYPE.ENV주기정보요청:	//pjhpjh
					case SSNS_ENV2_OPCODE_TYPE.ENV주기값등설정:
						SSNS_ENV2_SENSOR item센서 = Convert충격값정보ByteArray(data.DATA);
						if (item센서 != null && On센서정보Event != null) On센서정보Event(item센서);
						break;
					case SSNS_ENV2_OPCODE_TYPE.PC_PWR_OFF_EVENT:   //pjhpjh
						if (data.DATA[0] == 0xa5)	// magic
						{
							TraceManager.AddInfoLog(string.Format("[전원종료] 전원종료를 시도합니다. {0}회", 재시도횟수++));
							System.Diagnostics.Process.Start("shutdown", "-s -f -t 10");    // after 10sec, pc off
						}
						break;

					case SSNS_ENV2_OPCODE_TYPE.ENV보드설정FAN:
						if (data.DATA.Length > 0)
						{}
						break;
						
					case SSNS_ENV2_OPCODE_TYPE.TCON_비트맵_쓰기:
						DisplayLog(Log4Level.Info, string.Format("[UART] Rx TCON Count = {0}, Ack = {1}", arrayTx.Count, data.DATA[0]), LogSource.Board);
						break;
				
				}
				bResponseTconOk = true; //pjh
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}


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
				if (bytes.Length < 8)
				{
					Console.WriteLine("SP Receive 수신byte가 짧습니다.");
					return new SSNS_ENV2_PROTOCOL() { RESULT_CD = 13, RESULT_MSG = "수신된 데이터가 9byte이하입니다." };
				}

				while (true)	//pjh stx 까지 스킵.
				{
					if (bytes == null || bytes.Length < 7) //stx .. opcode = 7
					{
						Console.WriteLine("SP Receive STX1 , STX2 미일치 생략");
						return new SSNS_ENV2_PROTOCOL() { RESULT_CD = 11, RESULT_MSG = "STX 미일치" };
					}

					if (bytes[0].Equals(STX) == true && bytes[1].Equals(STX) == true)
						break;
					bytes = bytes.Skip(1).ToArray();
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


	}
}
