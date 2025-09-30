using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;

namespace SSData
{
	public class PAJUBISPacket
	{		
		const byte STX = 0x02;
		const byte ETX = 0x03;

		protected PAJU_BIS_PROTOCOL CreatePAJU프로토콜ByteArray(byte[] bytes)
		{
			try
			{
				if (bytes == null || bytes.Length <= 14) return new PAJU_BIS_PROTOCOL() { RESULT_CD = 0, RESULT_MSG = "ByteArray가 NULL 또는 길이가 14byte이하입니다." };

				PAJU_BIS_PROTOCOL item = new PAJU_BIS_PROTOCOL();

				item.STX = bytes[0];

				item.OPCODE = bytes[1];
				item.OP_GBN = (PAJU_BIS_OPCODE_TYPE)item.OPCODE;

				byte[] bSRCnt = new byte[2];
				bSRCnt[0] = bytes[2];
				bSRCnt[1] = bytes[3];
				item.SR_CNT = BitConvertUtils.ToInt16(bSRCnt, false);

				byte[] bDeviceID = new byte[4];
				bDeviceID[0] = bytes[4];
				bDeviceID[1] = bytes[5];
				bDeviceID[2] = bytes[6];
				bDeviceID[3] = bytes[7];
				item.DeviceID = BitConvertUtils.ToInt32(bDeviceID, false);

				byte[] bDataLength = new byte[4];
				bDataLength[0] = bytes[8];
				bDataLength[1] = bytes[9];
				bDataLength[2] = bytes[10];
				bDataLength[3] = bytes[11];
				item.DataLength = BitConvertUtils.ToInt32(bDataLength, false);

				item.DATA = new byte[item.DataLength];

				int idxNo = 0;
				while (true)
				{
					if (item.DataLength.Equals(idxNo) == true) break;
					//Console.WriteLine("## {0} : {1} ##", idxNo, idxNo + 12);
					item.DATA[idxNo] = bytes[idxNo + 12];
					idxNo++;
				}
				item.CheckSum = bytes[bytes.Length - 2];
				item.ETX = bytes[bytes.Length - 1];

				//데이터 체크부분
				if (item.STX.Equals(STX) == false)
				{
					Console.WriteLine("SP Receive STX 미일치 생략");
					return new PAJU_BIS_PROTOCOL() { RESULT_CD = 11, RESULT_MSG = "STX 미일치" };
				}

				if (item.ETX.Equals(ETX) == false)
				{
					Console.WriteLine("SP Receive ETX 미일치 생략");
					return new PAJU_BIS_PROTOCOL() { RESULT_CD = 13, RESULT_MSG = "ETX 미일치" };
				}

				byte bCheckSum = BitConvertUtils.CreateCheckSum(bytes, bytes.Length - 2);
				if (item.CheckSum.Equals(bCheckSum) == false)
				{
					Console.WriteLine("CheckSum 미일치 잘못된 데이터");
					return new PAJU_BIS_PROTOCOL() { RESULT_CD = 12, RESULT_MSG = "CHECKSUM 미일치" };
				}

				item.RESULT_CD = 1;
				item.RESULT_MSG = "변환완료";
				return item;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return new PAJU_BIS_PROTOCOL() { RESULT_CD = -1, RESULT_MSG = "Exception" };
			}
		}

		short SRCount = 0;

		#region 단말기 → 센터

		/// <summary>
		/// 0xB1  = 0:정상
		/// </summary>
		protected byte[] Create단말기상태제어수신완료(byte b수신완료)
		{
			try
			{
				List<byte> itemsHEADER = new List<byte>();
				List<byte> itemsDATA = new List<byte>();

				//HEADER
				itemsHEADER.Add(STX);
				//OPCode
				PAJU_BIS_OPCODE_TYPE OPType = PAJU_BIS_OPCODE_TYPE.단말기상태제어수신완료ACK;
				itemsHEADER.Add((byte)OPType);
				//SRCNT 2byte
				byte[] byteCnt = BitConvertUtils.FromInt16(SRCount++);
				itemsHEADER.AddRange(byteCnt);
				//DeviceID 4byte
				byte[] byteDeviceID = BitConvertUtils.FromSTR(BITDataManager.BIT_ID, BITDataManager.BIT_ID.Length);
				itemsHEADER.AddRange(byteDeviceID);

				//DATA
				DateTime dt = DateTime.Now;
				int n전송일자 = Convert.ToInt32(dt.ToString("yyyyMMdd"));
				int n전송시간 = Convert.ToInt32(dt.ToString("HHmmss"));

				//전송일자 4byte
				byte[] byte전송일자 = BitConvertUtils.FromInt32(n전송일자);
				itemsDATA.AddRange(byte전송일자);

				//전송시간 4byte
				byte[] byte전송시간 = BitConvertUtils.FromInt32(n전송시간);
				itemsDATA.AddRange(byte전송시간);

				//수신완료
				itemsDATA.Add(b수신완료);

				//HEADER의 DATA길이
				int DataLength = itemsDATA.Count;
				byte[] byte길이 = BitConvertUtils.FromInt32(DataLength);
				itemsHEADER.AddRange(byte길이);

				//전송할 byte Arrray생성 (헤더길이 + 데이터길이 + 테일길이(2byte)
				byte[] bytes전송 = new byte[itemsHEADER.Count + itemsDATA.Count + 2];

				//HEADER + DATA를 추가한다.
				System.Buffer.BlockCopy(itemsHEADER.ToArray(), 0, bytes전송, 0, itemsHEADER.Count);
				System.Buffer.BlockCopy(itemsDATA.ToArray(), 0, bytes전송, itemsHEADER.Count, itemsDATA.Count);

				byte bCheckSum = BitConvertUtils.CreateCheckSum(bytes전송, bytes전송.Length);

				//체크섬값 입력
				bytes전송[bytes전송.Length - 2] = bCheckSum;
				bytes전송[bytes전송.Length - 1] = ETX;

				//전송로그                
				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes전송, "-");

				Log4Manager.WriteSocketLog(Log4Level.Info, string.Format("[{0:X2}] {1} {2}byte", (byte)OPType, OPType.ToString(), bytes전송.Length));
				Log4Manager.WriteSocketLog(Log4Level.Debug, hexString);

				return bytes전송;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}

		/// <summary>
		/// 0xB2  = 0:정상
		/// </summary>
		protected byte[] Create시정홍보자료수신응답(byte b수신여부)
		{
			try
			{
				List<byte> itemsHEADER = new List<byte>();
				List<byte> itemsDATA = new List<byte>();

				//HEADER
				itemsHEADER.Add(STX);
				//OPCode
				PAJU_BIS_OPCODE_TYPE OPType = PAJU_BIS_OPCODE_TYPE.시정홍보자료수신완료ACK;
				itemsHEADER.Add((byte)OPType);
				//SRCNT 2byte
				byte[] byteCnt = BitConvertUtils.FromInt16(SRCount++);
				itemsHEADER.AddRange(byteCnt);
				//DeviceID 4byte
				byte[] byteDeviceID = BitConvertUtils.FromSTR(BITDataManager.BIT_ID, BITDataManager.BIT_ID.Length); ;
				itemsHEADER.AddRange(byteDeviceID);

				//DATA
				DateTime dt = DateTime.Now;
				int n전송일자 = Convert.ToInt32(dt.ToString("yyyyMMdd"));
				int n전송시간 = Convert.ToInt32(dt.ToString("HHmmss"));

				//전송일자 4byte
				byte[] byte전송일자 = BitConvertUtils.FromInt32(n전송일자);
				itemsDATA.AddRange(byte전송일자);

				//전송시간 4byte
				byte[] byte전송시간 = BitConvertUtils.FromInt32(n전송시간);
				itemsDATA.AddRange(byte전송시간);

				//변경상태
				itemsDATA.Add(b수신여부);

				//HEADER의 DATA길이
				int DataLength = itemsDATA.Count;
				byte[] byte길이 = BitConvertUtils.FromInt32(DataLength);
				itemsHEADER.AddRange(byte길이);

				//전송할 byte Arrray생성 (헤더길이 + 데이터길이 + 테일길이(2byte)
				byte[] bytes전송 = new byte[itemsHEADER.Count + itemsDATA.Count + 2];

				//HEADER + DATA를 추가한다.
				System.Buffer.BlockCopy(itemsHEADER.ToArray(), 0, bytes전송, 0, itemsHEADER.Count);
				System.Buffer.BlockCopy(itemsDATA.ToArray(), 0, bytes전송, itemsHEADER.Count, itemsDATA.Count);

				byte bCheckSum = BitConvertUtils.CreateCheckSum(bytes전송, bytes전송.Length);

				//체크섬값 입력
				bytes전송[bytes전송.Length - 2] = bCheckSum;
				bytes전송[bytes전송.Length - 1] = ETX;

				//전송로그                
				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes전송, "-");

				Log4Manager.WriteSocketLog(Log4Level.Info, string.Format("[{0:X2}] {1} {2}byte", (byte)OPType, OPType.ToString(), bytes전송.Length));
				Log4Manager.WriteSocketLog(Log4Level.Debug, hexString);

				return bytes전송;				
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}

		/// <summary>
		/// 0xC0
		/// </summary>
		//byte[] Send부팅정보()
		protected byte[] Create부팅정보()
		{
			try
			{
				List<byte> itemsHEADER = new List<byte>();
				List<byte> itemsDATA = new List<byte>();

				//HEADER
				itemsHEADER.Add(STX);
				//OPCode
				PAJU_BIS_OPCODE_TYPE OPType = PAJU_BIS_OPCODE_TYPE.부팅정보ACK;
				itemsHEADER.Add((byte)OPType);
				//SRCNT 2byte
				byte[] byteCnt = BitConvertUtils.FromInt16(SRCount++);
				itemsHEADER.AddRange(byteCnt);
				//DeviceID 4byte
				byte[] byteDeviceID = BitConvertUtils.FromSTR(BITDataManager.BIT_ID, BITDataManager.BIT_ID.Length); 
				itemsHEADER.AddRange(byteDeviceID);

				//DATA
				DateTime dt = DateTime.Now;
				int n전송일자 = Convert.ToInt32(dt.ToString("yyyyMMdd"));
				int n전송시간 = Convert.ToInt32(dt.ToString("HHmmss"));

				//전송일자 4byte
				byte[] byte전송일자 = BitConvertUtils.FromInt32(n전송일자);
				itemsDATA.AddRange(byte전송일자);

				//전송시간 4byte
				byte[] byte전송시간 = BitConvertUtils.FromInt32(n전송시간);
				itemsDATA.AddRange(byte전송시간);

				//APP개정번호 8byte
				byte[] byte개정번호 = BitConvertUtils.FromSTR(null, 8);
				itemsDATA.AddRange(byte개정번호);

				//BIT유형 1byte
				int BIT_GBN = 0;
				if (BITDataManager.BitDisplays != null && BITDataManager.BitDisplays.Count > 0)
				{
					BIT_DISPLAY item = BITDataManager.BitDisplays.First();
					switch (item.화면구분)
					{
						case DISPLAY구분.LCD가로형:
						case DISPLAY구분.LCD세로형:
							BIT_GBN = 0; break;

						case DISPLAY구분.LED3X6:
						case DISPLAY구분.LED3X8:
							BIT_GBN = 1; break;
					}
				}

				byte bBIT유형 = (byte)BIT_GBN;
				itemsDATA.Add(bBIT유형);

				//IP주소 VPN IP 제외
				string IP = DataManager.IPAddress;
				byte[] byteIP주소 = BitConvertUtils.FromSTR(IP, 20);
				itemsDATA.AddRange(byteIP주소);

				//개발자예약 : 4byte
				byte[] byte예약 = BitConvertUtils.FromSTR(null, 4);
				itemsDATA.AddRange(byte예약);

				//HEADER의 DATA길이
				int DataLength = itemsDATA.Count;
				byte[] byte길이 = BitConvertUtils.FromInt32(DataLength);
				itemsHEADER.AddRange(byte길이);

				//전송할 byte Arrray생성 (헤더길이 + 데이터길이 + 테일길이(2byte)
				byte[] bytes전송 = new byte[itemsHEADER.Count + itemsDATA.Count + 2];

				//HEADER + DATA를 추가한다.
				System.Buffer.BlockCopy(itemsHEADER.ToArray(), 0, bytes전송, 0, itemsHEADER.Count);
				System.Buffer.BlockCopy(itemsDATA.ToArray(), 0, bytes전송, itemsHEADER.Count, itemsDATA.Count);

				byte bCheckSum = BitConvertUtils.CreateCheckSum(bytes전송, bytes전송.Length);

				//체크섬값 입력
				bytes전송[bytes전송.Length - 2] = bCheckSum;
				bytes전송[bytes전송.Length - 1] = ETX;

				//전송로그                
				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes전송, "-");
				Log4Manager.WriteSocketLog(Log4Level.Info, string.Format("[{0:X2}] {1} {2}byte", (byte)OPType, OPType.ToString(), bytes전송.Length));
				Log4Manager.WriteSocketLog(Log4Level.Debug, hexString);

				return bytes전송;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}

		/// <summary>
		/// 0xC2 단말기상태정보
		/// </summary>
		protected byte[] Create단말기상태정보(PAJU_BIT_단말기상태 item)
		{
			try
			{
				List<byte> itemsHEADER = new List<byte>();
				List<byte> itemsDATA = new List<byte>();

				//HEADER
				itemsHEADER.Add(STX);
				//OPCode
				PAJU_BIS_OPCODE_TYPE OPType = PAJU_BIS_OPCODE_TYPE.단말기상태정보ACK;
				itemsHEADER.Add((byte)OPType);
				//SRCNT 2byte
				byte[] byteCnt = BitConvertUtils.FromInt16(SRCount++);
				itemsHEADER.AddRange(byteCnt);
				//DeviceID 4byte
				byte[] byteDeviceID = BitConvertUtils.FromSTR(BITDataManager.BIT_ID, BITDataManager.BIT_ID.Length);
				itemsHEADER.AddRange(byteDeviceID);

				//DATA
				DateTime dt = DateTime.Now;
				int n전송일자 = Convert.ToInt32(dt.ToString("yyyyMMdd"));
				int n전송시간 = Convert.ToInt32(dt.ToString("HHmmss"));

				//전송일자 4byte
				byte[] byte전송일자 = BitConvertUtils.FromInt32(n전송일자);
				itemsDATA.AddRange(byte전송일자);

				//전송시간 4byte
				byte[] byte전송시간 = BitConvertUtils.FromInt32(n전송시간);
				itemsDATA.AddRange(byte전송시간);

				//상태값
				itemsDATA.Add(item.bLCDOnOff상태);
				itemsDATA.Add(item.bFAN동작상태);
				itemsDATA.Add(item.b웹카메라동작상태);
				itemsDATA.Add(item.b히터동작상태);
				itemsDATA.Add((byte)item.n온도);

				itemsDATA.Add((byte)item.n습도);
				itemsDATA.Add((byte)item.n도어상태);
				itemsDATA.Add((byte)item.n동작감지센서LCDOnOFF);
				itemsDATA.Add((byte)item.nLCD전류감지);
				itemsDATA.Add((byte)item.n시험운영중표출여부);

				//HEADER의 DATA길이
				int DataLength = itemsDATA.Count;
				byte[] byte길이 = BitConvertUtils.FromInt32(DataLength);
				itemsHEADER.AddRange(byte길이);

				//전송할 byte Arrray생성 (헤더길이 + 데이터길이 + 테일길이(2byte)
				byte[] bytes전송 = new byte[itemsHEADER.Count + itemsDATA.Count + 2];

				//HEADER + DATA를 추가한다.
				System.Buffer.BlockCopy(itemsHEADER.ToArray(), 0, bytes전송, 0, itemsHEADER.Count);
				System.Buffer.BlockCopy(itemsDATA.ToArray(), 0, bytes전송, itemsHEADER.Count, itemsDATA.Count);

				byte bCheckSum = BitConvertUtils.CreateCheckSum(bytes전송, bytes전송.Length);

				//체크섬값 입력
				bytes전송[bytes전송.Length - 2] = bCheckSum;
				bytes전송[bytes전송.Length - 1] = ETX;

				//전송로그                
				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes전송, "-");

				Log4Manager.WriteSocketLog(Log4Level.Info, string.Format("[{0:X2}] {1} {2}byte", (byte)OPType, OPType.ToString(), bytes전송.Length));
				Log4Manager.WriteSocketLog(Log4Level.Debug, hexString);

				return bytes전송;				
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}

		/// <summary>
		/// 0xD8= 0:정상
		/// </summary>
		protected byte[] CreateBIT충격영상전송(int 충격센터값, string FILE_NM)
		{
			try
			{
				List<byte> itemsHEADER = new List<byte>();
				List<byte> itemsDATA = new List<byte>();

				//HEADER
				itemsHEADER.Add(STX);
				//OPCode
				PAJU_BIS_OPCODE_TYPE OPType = PAJU_BIS_OPCODE_TYPE.BIT충격영상전송;
				itemsHEADER.Add((byte)OPType);
				//SRCNT 2byte
				byte[] byteCnt = BitConvertUtils.FromInt16(SRCount++);
				itemsHEADER.AddRange(byteCnt);
				//DeviceID 4byte
				byte[] byteDeviceID = BitConvertUtils.FromSTR(BITDataManager.BIT_ID, BITDataManager.BIT_ID.Length);
				itemsHEADER.AddRange(byteDeviceID);

				//DATA
				DateTime dt = DateTime.Now;
				int n전송일자 = Convert.ToInt32(dt.ToString("yyyyMMdd"));
				int n전송시간 = Convert.ToInt32(dt.ToString("HHmmss"));

				//전송일자 4byte
				byte[] byte전송일자 = BitConvertUtils.FromInt32(n전송일자);
				itemsDATA.AddRange(byte전송일자);

				//전송시간 4byte
				byte[] byte전송시간 = BitConvertUtils.FromInt32(n전송시간);
				itemsDATA.AddRange(byte전송시간);

				byte b충격값 = (byte)충격센터값;
				itemsDATA.Add(b충격값);

				byte[] byte파일명 = BitConvertUtils.FromSTR(FILE_NM, 50);//, BitDummyType.NULL, Encoding.ASCII);
				itemsDATA.AddRange(byte파일명);

				//HEADER의 DATA길이
				int DataLength = itemsDATA.Count;
				byte[] byte길이 = BitConvertUtils.FromInt32(DataLength);
				itemsHEADER.AddRange(byte길이);

				//전송할 byte Arrray생성 (헤더길이 + 데이터길이 + 테일길이(2byte)
				byte[] bytes전송 = new byte[itemsHEADER.Count + itemsDATA.Count + 2];

				//HEADER + DATA를 추가한다.
				System.Buffer.BlockCopy(itemsHEADER.ToArray(), 0, bytes전송, 0, itemsHEADER.Count);
				System.Buffer.BlockCopy(itemsDATA.ToArray(), 0, bytes전송, itemsHEADER.Count, itemsDATA.Count);

				byte bCheckSum = BitConvertUtils.CreateCheckSum(bytes전송, bytes전송.Length);

				//체크섬값 입력
				bytes전송[bytes전송.Length - 2] = bCheckSum;
				bytes전송[bytes전송.Length - 1] = ETX;

				//전송로그                
				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes전송, "-");

				Log4Manager.WriteSocketLog(Log4Level.Info, string.Format("[{0:X2}] {1} {2}byte", (byte)OPType, OPType.ToString(), bytes전송.Length));
				Log4Manager.WriteSocketLog(Log4Level.Debug, hexString);

				return bytes전송;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}

		/// <summary>
		/// 0xF5  = 0:정상
		/// </summary>
		protected byte[] Create파라메타변경(byte b변경결과)
		{
			try
			{
				List<byte> itemsHEADER = new List<byte>();
				List<byte> itemsDATA = new List<byte>();

				//HEADER
				itemsHEADER.Add(STX);
				//OPCode
				PAJU_BIS_OPCODE_TYPE OPType = PAJU_BIS_OPCODE_TYPE.파라메터변경ACK;
				itemsHEADER.Add((byte)OPType);
				//itemsHEADER.Add((byte)PAJU_BIS_OPCODE_TYPE.파라메터변경ACK);
				//SRCNT 2byte
				byte[] byteCnt = BitConvertUtils.FromInt16(SRCount++);
				itemsHEADER.AddRange(byteCnt);
				//DeviceID 4byte
				byte[] byteDeviceID = BitConvertUtils.FromSTR(BITDataManager.BIT_ID, BITDataManager.BIT_ID.Length);
				itemsHEADER.AddRange(byteDeviceID);

				//DATA
				DateTime dt = DateTime.Now;
				int n전송일자 = Convert.ToInt32(dt.ToString("yyyyMMdd"));
				int n전송시간 = Convert.ToInt32(dt.ToString("HHmmss"));

				//전송일자 4byte
				byte[] byte전송일자 = BitConvertUtils.FromInt32(n전송일자);
				itemsDATA.AddRange(byte전송일자);

				//전송시간 4byte
				byte[] byte전송시간 = BitConvertUtils.FromInt32(n전송시간);
				itemsDATA.AddRange(byte전송시간);

				//변경상태
				itemsDATA.Add(b변경결과);

				//HEADER의 DATA길이
				int DataLength = itemsDATA.Count;
				byte[] byte길이 = BitConvertUtils.FromInt32(DataLength);
				itemsHEADER.AddRange(byte길이);

				//전송할 byte Arrray생성 (헤더길이 + 데이터길이 + 테일길이(2byte)
				byte[] bytes전송 = new byte[itemsHEADER.Count + itemsDATA.Count + 2];

				//HEADER + DATA를 추가한다.
				System.Buffer.BlockCopy(itemsHEADER.ToArray(), 0, bytes전송, 0, itemsHEADER.Count);
				System.Buffer.BlockCopy(itemsDATA.ToArray(), 0, bytes전송, itemsHEADER.Count, itemsDATA.Count);

				byte bCheckSum = BitConvertUtils.CreateCheckSum(bytes전송, bytes전송.Length);

				//체크섬값 입력
				bytes전송[bytes전송.Length - 2] = bCheckSum;
				bytes전송[bytes전송.Length - 1] = ETX;

				//전송로그                
				string hexString = BitConvertUtils.ByteArrayToHexSTR(bytes전송, "-");
				//02-F5-00-00-93-02-00-00-09-00-00-00-6D-8A-34-01-AA-6F-01-00-00-79-03
				//02-F5-01-00-93-02-00-00-09-00-00-00-6F-8A-34-01-0B-99-02-00-00-2E-03
				Log4Manager.WriteSocketLog(Log4Level.Info, string.Format("[{0:X2}] {1} {2}byte", (byte)OPType, OPType.ToString(), bytes전송.Length));
				Log4Manager.WriteSocketLog(Log4Level.Debug, hexString);

				return bytes전송;				
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}

		#endregion

		#region 센터 → 단말기

		/// <summary>
		/// 0xD2
		/// </summary>
		protected BisExpectedArrivalInformation Recv도착예정정보(byte[] bytes)
		{
			try
			{
				BisExpectedArrivalInformation item = new BisExpectedArrivalInformation();

				if (bytes.Length < 14)
				{
					return null;
				}

				byte[] b전송일자 = new byte[4];
				b전송일자[0] = bytes[0];
				b전송일자[1] = bytes[1];
				b전송일자[2] = bytes[2];
				b전송일자[3] = bytes[3];
				item.TransmissionDate = BitConvertUtils.ToInt32(b전송일자, false);

				byte[] b전송시간 = new byte[4];
				b전송시간[0] = bytes[4];
				b전송시간[1] = bytes[5];
				b전송시간[2] = bytes[6];
				b전송시간[3] = bytes[7];
				item.TransmissionTime = BitConvertUtils.ToInt32(b전송시간, false);

				byte[] bBIT_ID = new byte[4];
				bBIT_ID[0] = bytes[8];
				bBIT_ID[1] = bytes[9];
				bBIT_ID[2] = bytes[10];
				bBIT_ID[3] = bytes[11];
				item.BitId = BitConvertUtils.ToSTR(bBIT_ID);

				byte[] bDataCount = new byte[2];
				bDataCount[0] = bytes[12];
				bDataCount[1] = bytes[13];
				item.NumberOfOperationInformation = BitConvertUtils.ToInt16(bDataCount, false);

				if (item.NumberOfOperationInformation == 0) return item;

				//20221108 bha BIT_ID 체크부분 추가
				if (BITDataManager.BIT_ID.Equals(item.BitId) == false)
				{
					Log4Manager.WriteSocketLog(Log4Level.Debug, string.Format("[도착정보] 설정된 BIT_ID({0})와 수신된 BIT_ID ({1})가 일치하지 않습니다."
						, BITDataManager.BIT_ID, item.BitId));
					return null;
				}

				item.ArrivalInformationList = new List<BisArrivalInformation>();
				int OffsetCount = 14;

				for (int i = 0; i < item.NumberOfOperationInformation; i++)
				{
					byte[] bytes정보 = new byte[908];
					Buffer.BlockCopy(bytes, OffsetCount + (i * 908), bytes정보, 0, 908);

					BisArrivalInformation item정보 = Convert도착정보(bytes정보);
					if (item정보 != null)
					{
						if (item정보.RouteNumber.Length > 8 && CommonUtils.IsNumeric(item정보.RouteNumber) == true)
						{
							Log4Manager.WriteSocketLog(Log4Level.Debug, string.Format("[도착정보] RouteNumber 오류 : {0}", item정보.RouteNumber));
						}
						else
						{
							item.ArrivalInformationList.Add(item정보);
						}
					}
				}

				if (item.ArrivalInformationList.Count.Equals(item.NumberOfOperationInformation) == false)
				{
					Log4Manager.WriteSocketLog(Log4Level.Debug,
						string.Format("[도착정보] 변환중 오류 : 운행정보 갯수 {0}건 / 변환데이터수 : {1}건"
						, item.NumberOfOperationInformation, item.ArrivalInformationList.Count));
				}
				return item;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}

		/// <summary>
		/// 0xE5
		/// </summary>
		protected BIT_ENV_SETTING Recv파라메터변경(byte[] bytes)
		{
			try
			{
				if (bytes.Length < 77)
				{
					return null;
				}

				BIT_ENV_SETTING item = new BIT_ENV_SETTING();

				byte[] b전송일자 = new byte[4];
				b전송일자[0] = bytes[0];
				b전송일자[1] = bytes[1];
				b전송일자[2] = bytes[2];
				b전송일자[3] = bytes[3];

				int n전송일자 = BitConvertUtils.ToInt32(b전송일자, false);
				string 전송일자 = string.Format("{0}", BitConvertUtils.ToInt32(b전송일자, false));

				byte[] b전송시간 = new byte[4];
				b전송시간[0] = bytes[4];
				b전송시간[1] = bytes[5];
				b전송시간[2] = bytes[6];
				b전송시간[3] = bytes[7];

				int n전송시간= BitConvertUtils.ToInt32(b전송시간, false);
				string 전송시간 = string.Format("{0:d6}", n전송시간);

				DateTime dt전송 = ConvertUtils.DateTimeByString(전송일자 + 전송시간).Value;

				//item.USE_YMD = dt전송;
				item.REGDATE = dt전송;

				item.Volume = (int)bytes[8];
				item.ArriveSoonGBN = (int)bytes[9];

				byte[] b잠시후도착시간조건 = new byte[2];
				b잠시후도착시간조건[0] = bytes[10];
				b잠시후도착시간조건[1] = bytes[11];
				item.ArriveSoonTimeGBN = BitConvertUtils.ToInt16(b잠시후도착시간조건, false);

				byte[] b잠시후도착정류장조건 = new byte[2];
				b잠시후도착정류장조건[0] = bytes[12];
				b잠시후도착정류장조건[1] = bytes[13];
				item.ArriveSoonStationGBN = BitConvertUtils.ToInt16(b잠시후도착정류장조건, false);

				byte[] b모니터ON = new byte[2];
				b모니터ON[0] = bytes[14];
				b모니터ON[1] = bytes[15];
				item.MonitorOnTime = string.Format("{0:d4}", BitConvertUtils.ToInt16(b모니터ON, false));

				byte[] b모니터OFF = new byte[2];
				b모니터OFF[0] = bytes[16];
				b모니터OFF[1] = bytes[17];
				item.MonitorOffTime = string.Format("{0:d4}", BitConvertUtils.ToInt16(b모니터OFF, false));

				byte[] b기본조도값 = new byte[2];
				b기본조도값[0] = bytes[18];
				b기본조도값[1] = bytes[19];
				item.DefaultLCDLux = BitConvertUtils.ToInt16(b기본조도값, false);

				byte[] b조도데이터수 = new byte[2];
				b조도데이터수[0] = bytes[20];
				b조도데이터수[1] = bytes[21];
				item.LuxCount = BitConvertUtils.ToInt16(b조도데이터수, false);

				int OffSetCount = 22;
				item.itemsLux = new List<BIT_ENV_LUX>();
				for (int i = 0; i < item.LuxCount; i++)
				{
					BIT_ENV_LUX data = new BIT_ENV_LUX();
					byte[] b시작 = new byte[2];
					b시작[0] = bytes[OffSetCount + 0];//22 28
					b시작[1] = bytes[OffSetCount + 1];
					data.S_TIME = string.Format("{0:d4}", BitConvertUtils.ToInt16(b시작, false));

					byte[] b종료 = new byte[2];
					b종료[0] = bytes[OffSetCount + 2];
					b종료[1] = bytes[OffSetCount + 3];//25
					data.E_TIME = string.Format("{0:d4}", BitConvertUtils.ToInt16(b종료, false));

					byte[] b값 = new byte[2];
					b값[0] = bytes[OffSetCount + 4];
					b값[1] = bytes[OffSetCount + 5];//27
					data.LUX = BitConvertUtils.ToInt16(b값, false);

					//item.USE_YMD = dt전송;
					item.REGDATE = dt전송;

					item.itemsLux.Add(data);
					OffSetCount += 6;
				}

				byte[] b상태정보 = new byte[2];
				b상태정보[0] = bytes[OffSetCount + 0];
				b상태정보[1] = bytes[OffSetCount + 1];
				item.StateSendPeriod = BitConvertUtils.ToInt16(b상태정보, false);

				byte[] b영상정보 = new byte[2];
				b영상정보[0] = bytes[OffSetCount + 2];
				b영상정보[1] = bytes[OffSetCount + 3];
				item.WebcamSendPeriod = BitConvertUtils.ToInt16(b영상정보, false);

				byte[] b스크린 = new byte[2];
				b스크린[0] = bytes[OffSetCount + 4];
				b스크린[1] = bytes[OffSetCount + 5];
				item.ScreenCaptureSendPeriod = BitConvertUtils.ToInt16(b스크린, false);

				item.BITOrderGBN = (int)bytes[OffSetCount + 6];
				item.UseDetectSensor = (int)bytes[OffSetCount + 7];

				byte[] b동작감시 = new byte[2];
				b동작감시[0] = bytes[OffSetCount + 8];
				b동작감시[1] = bytes[OffSetCount + 9];
				item.DetectSensorServiceTime = BitConvertUtils.ToInt16(b동작감시, false);

				item.FANMaxTemp = (int)bytes[OffSetCount + 10];
				item.FANMinTemp = (int)bytes[OffSetCount + 11];
				item.HeaterMaxTemp = (int)bytes[OffSetCount + 12];
				item.HeaterMinTemp = (int)bytes[OffSetCount + 13];

				item.SubwayDisplayYN = (int)bytes[OffSetCount + 14];
				byte[] b지하철 = new byte[2];
				b지하철[0] = bytes[OffSetCount + 15];
				b지하철[1] = bytes[OffSetCount + 16];
				item.SubwayLineNo = BitConvertUtils.ToInt16(b지하철, false);
				item.SubwayStationNo = (int)bytes[OffSetCount + 17];

				item.ForeignDisplayYN = (int)bytes[OffSetCount + 18];
				item.ForeignDisplayTime = (int)bytes[OffSetCount + 19];
				item.ShockDetectValue= (int)bytes[OffSetCount + 20];

				byte[] b정류소번호 = new byte[4];
				b정류소번호[0] = bytes[OffSetCount + 21];
				b정류소번호[1] = bytes[OffSetCount + 22];
				b정류소번호[2] = bytes[OffSetCount + 23];
				b정류소번호[3] = bytes[OffSetCount + 24];
				item.StationMobileNo = BitConvertUtils.ToInt32(b정류소번호, false);

				byte[] b정류소이름 = new byte[30];
				System.Buffer.BlockCopy(bytes, OffSetCount + 25, b정류소이름, 0, 30);
				item.StationName= BitConvertUtils.ToSTR(b정류소이름, Encoding.Default);
				item.PromoteSoundPlayYN = (int)bytes[OffSetCount + 55];
				item.BITFontSize = (int)bytes[OffSetCount + 56];
				item.TestOperationDisplayYN = (int)bytes[OffSetCount + 57];

				byte[] b예비 = new byte[4];
				b예비[0] = bytes[OffSetCount + 58];
				b예비[1] = bytes[OffSetCount + 59];
				b예비[2] = bytes[OffSetCount + 60];
				b예비[3] = bytes[OffSetCount + 61];
				item.Reserve1 = BitConvertUtils.ToInt32(b예비, false);

				return item;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}

		////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// 0xA1
		/// </summary>
		protected PAJU_BIS_단말기상태제어 Recv단말기상태제어(byte[] bytes)
		{
			try
			{
				if (bytes.Length < 10)
				{
					return null;
				}

				PAJU_BIS_단말기상태제어 item = new PAJU_BIS_단말기상태제어();

				byte[] b전송일자 = new byte[4];
				b전송일자[0] = bytes[0];
				b전송일자[1] = bytes[1];
				b전송일자[2] = bytes[2];
				b전송일자[3] = bytes[3];
				item.n전송일자 = BitConvertUtils.ToInt32(b전송일자, false);

				byte[] b전송시간 = new byte[4];
				b전송시간[0] = bytes[4];
				b전송시간[1] = bytes[5];
				b전송시간[2] = bytes[6];
				b전송시간[3] = bytes[7];
				item.n전송시간 = BitConvertUtils.ToInt32(b전송시간, false);

				byte[] b제어수 = new byte[2];
				b제어수[0] = bytes[8];
				b제어수[1] = bytes[9];
				item.n총제어수 = BitConvertUtils.ToInt16(b제어수, false);

				int OffSetCount = 10;
				item.items상태제어 = new List<PajuBisStatusControl>();
				for (int i = 0; i < item.n총제어수; i++)
				{
					PajuBisStatusControl data = new PajuBisStatusControl();
					byte[] b코드 = new byte[2];
					b코드[0] = bytes[OffSetCount + 0];//22 28
					b코드[1] = bytes[OffSetCount + 1];
					data.ControlCode = BitConvertUtils.ToInt16(b코드, false);

					byte[] b제어값 = new byte[4];
					b제어값[0] = bytes[OffSetCount + 2];
					b제어값[1] = bytes[OffSetCount + 3];
					b제어값[2] = bytes[OffSetCount + 4];
					b제어값[3] = bytes[OffSetCount + 5];
					data.ControlValue = BitConvertUtils.ToInt32(b제어값, false);

					item.items상태제어.Add(data);
					OffSetCount += 6;
				}

				return item;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}

		/// <summary>
		/// 0xA2
		/// </summary>
		protected PAJU_BIS_시정홍보자료전송 Recv시정홍보자료(byte[] bytes)
		{
			try
			{
				if (bytes.Length < 8)
				{
					return null;
				}

				PAJU_BIS_시정홍보자료전송 item = new PAJU_BIS_시정홍보자료전송();

				byte[] b전송일자 = new byte[4];
				b전송일자[0] = bytes[0];
				b전송일자[1] = bytes[1];
				b전송일자[2] = bytes[2];
				b전송일자[3] = bytes[3];
				item.n전송일자 = BitConvertUtils.ToInt32(b전송일자, false);

				byte[] b전송시간 = new byte[4];
				b전송시간[0] = bytes[4];
				b전송시간[1] = bytes[5];
				b전송시간[2] = bytes[6];
				b전송시간[3] = bytes[7];
				item.n전송시간 = BitConvertUtils.ToInt32(b전송시간, false);

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

		protected BisArrivalInformation Convert도착정보(byte[] bytes)
		{
			try
			{
				BisArrivalInformation item = new BisArrivalInformation();
				if (bytes.Length < 908)
				{
					return null;
				}

				byte[] bID = new byte[4];
				bID[0] = bytes[0];
				bID[1] = bytes[1];
				bID[2] = bytes[2];
				bID[3] = bytes[3];
				item.RouteId = BitConvertUtils.ToInt32(bID, false);

				byte[] b번호 = new byte[16];
				System.Buffer.BlockCopy(bytes, 4, b번호, 0, 16);
				item.RouteNumberT = BitConvertUtils.ToSTR(b번호 , Encoding.Default);
				if (item.RouteNumberT.Length > 8)
				{

				}
					//Console.WriteLine(item.RouteNumberT);
					
				item.RouteNumberDisplayColor = (int)bytes[20];
				item.RouteDirection = (int)bytes[21];
				item.DisplayDestination = (int)bytes[22];

				//byte[] bytes노선 = new byte[23];
				//Buffer.BlockCopy(bytes, 0, bytes노선, 0, 23);
				//item.item노선정보 = Convert노선정보(bytes노선);

				item.StopLocationInformation = (int)bytes[23];
				item.StopLocationInformationColorCode = (int)bytes[24];

				byte[] b정류장_ID = new byte[4];
				b정류장_ID[0] = bytes[25];
				b정류장_ID[1] = bytes[26];
				b정류장_ID[2] = bytes[27];
				b정류장_ID[3] = bytes[28];
				item.StopId = BitConvertUtils.ToInt32(b정류장_ID, false);

				byte[] bytes정류장명 = new byte[30];
				Buffer.BlockCopy(bytes, 29, bytes정류장명, 0, 30);
				item.StopName = BitConvertUtils.ToSTR(bytes정류장명, Encoding.Default);

				item.StopNameColorCode = (int)bytes[59];

				byte[] b차량번호 = new byte[4];
				b차량번호[0] = bytes[60];
				b차량번호[1] = bytes[61];
				b차량번호[2] = bytes[62];
				b차량번호[3] = bytes[63];
				item.VehicleNumber = BitConvertUtils.ToInt32(b차량번호, false);

				byte[] bytes번호판 = new byte[12];
				Buffer.BlockCopy(bytes, 64, bytes번호판, 0, 12);
				item.LicensePlateNumber = BitConvertUtils.ToSTR(bytes번호판, Encoding.Default);

				byte[] b외국어Count = new byte[2];
				b외국어Count[0] = bytes[76];
				b외국어Count[1] = bytes[77];
				item.ForeignLanguageCount = BitConvertUtils.ToInt16(b외국어Count, false);

				byte[] b외국어타입1 = new byte[2];
				b외국어타입1[0] = bytes[78];
				b외국어타입1[1] = bytes[79];
				item.ForeignLanguageTypeCode1 = BitConvertUtils.ToInt16(b외국어타입1, false);

				byte[] bytes외국어정류소명1 = new byte[80];
				Buffer.BlockCopy(bytes, 80, bytes외국어정류소명1, 0, 80);
				item.ForeignLanguageStopName1 = BitConvertUtils.ToSTR(bytes외국어정류소명1, Encoding.UTF8);

				byte[] bytes외국어기점명1 = new byte[80];
				Buffer.BlockCopy(bytes, 160, bytes외국어기점명1, 0, 80);
				item.ForeignLanguageOriginName1 = BitConvertUtils.ToSTR(bytes외국어기점명1, Encoding.UTF8);

				byte[] bytes외국어행선지명1 = new byte[80];
				Buffer.BlockCopy(bytes, 240, bytes외국어행선지명1, 0, 80);
				item.ForeignLanguageDestinationName1 = BitConvertUtils.ToSTR(bytes외국어행선지명1, Encoding.UTF8);

				byte[] b외국어타입2 = new byte[2];
				b외국어타입2[0] = bytes[320];
				b외국어타입2[1] = bytes[321];
				item.ForeignLanguageTypeCode2 = BitConvertUtils.ToInt16(b외국어타입2, false);

				byte[] bytes외국어정류소명2 = new byte[80];
				Buffer.BlockCopy(bytes, 322, bytes외국어정류소명2, 0, 80);
				item.ForeignLanguageStopName2 = BitConvertUtils.ToSTR(bytes외국어정류소명2, Encoding.UTF8);

				byte[] bytes외국어기점명2 = new byte[80];
				Buffer.BlockCopy(bytes, 402, bytes외국어기점명2, 0, 80);
				item.ForeignLanguageOriginName2 = BitConvertUtils.ToSTR(bytes외국어기점명2, Encoding.UTF8);

				byte[] bytes외국어행선지명2 = new byte[80];
				Buffer.BlockCopy(bytes, 482, bytes외국어행선지명2, 0, 80);
				item.ForeignLanguageDestinationName2 = BitConvertUtils.ToSTR(bytes외국어행선지명2, Encoding.UTF8);

				byte[] b외국어타입3 = new byte[2];
				b외국어타입3[0] = bytes[562];
				b외국어타입3[1] = bytes[563];
				item.ForeignLanguageTypeCode3 = BitConvertUtils.ToInt16(b외국어타입3, false);

				byte[] bytes외국어정류소명3 = new byte[80];
				Buffer.BlockCopy(bytes, 564, bytes외국어정류소명3, 0, 80);
				item.ForeignLanguageStopName3 = BitConvertUtils.ToSTR(bytes외국어정류소명3, Encoding.UTF8);

				byte[] bytes외국어기점명3 = new byte[80];
				Buffer.BlockCopy(bytes, 644, bytes외국어기점명3, 0, 80);
				item.ForeignLanguageOriginName3 = BitConvertUtils.ToSTR(bytes외국어기점명3, Encoding.UTF8);

				byte[] bytes외국어행선지명3 = new byte[80];
				Buffer.BlockCopy(bytes, 724, bytes외국어행선지명3, 0, 80);
				item.ForeignLanguageDestinationName3 = BitConvertUtils.ToSTR(bytes외국어행선지명3, Encoding.UTF8);

				byte[] b남은정류장수 = new byte[2];
				b남은정류장수[0] = bytes[804];
				b남은정류장수[1] = bytes[805];
				item.NumberOfRemainingStops = BitConvertUtils.ToInt16(b남은정류장수, false);

				item.OperationStatus = (int)bytes[806];

				byte[] b도착예정 = new byte[2]; 
				b도착예정[0] = bytes[807];
				b도착예정[1] = bytes[808];
				item.EstimatedTimeOfArrival = BitConvertUtils.ToInt16(b도착예정, false);
				Console.WriteLine("#### {0}번의 도착예정시간 {1}초후 ####", item.RouteNumberT, item.EstimatedTimeOfArrival);
				item.EstimatedArrivalTimeDisplayColor = (int)bytes[809];

				byte[] b버스유형 = new byte[2];
				b버스유형[0] = bytes[810];
				b버스유형[1] = bytes[811];
				item.BusType = BitConvertUtils.ToInt16(b버스유형, false);

				item.FirstAndLastTrainType = (int)bytes[812];
				item.RouteType = (int)bytes[813];

				byte[] bytes기점명 = new byte[30];
				Buffer.BlockCopy(bytes, 814, bytes기점명, 0, 30);

				item.OriginName = BitConvertUtils.ToSTR(bytes기점명, Encoding.Default);
				item.OriginNameExpressionColor = (int)bytes[844];

				byte[] bytes행선지명 = new byte[30];
				Buffer.BlockCopy(bytes, 845, bytes행선지명, 0, 30);
				item.DestinationName = BitConvertUtils.ToSTR(bytes행선지명, Encoding.Default);
				item.DestinationNameDisplayColor = (int)bytes[875];

				item.BusCongestion = (int)bytes[876];
				item.NumberOfRemainingSeats = (int)bytes[877];
				//20221017 BHA 
				if (BITDataManager.BitSystem.SERVER_TYPE == 0) //구형서버
				{
					switch (item.RouteType)
					{
						case 13: //일반형시내버스
						case 22: //좌석형농어촌버스
						case 23: //일반형농어촌버스
						case 30: //마을버스
							item.BusCongestion = 9;
							break;
					}
				}
				else //신형서버
				{
					//20220928 BHA 
					if (item.NumberOfRemainingSeats.Equals(255) == true && item.IsVillageBus == true) item.BusCongestion = 9;
				}

				byte[] bytes예약 = new byte[30];
				 Buffer.BlockCopy(bytes, 878, bytes예약, 0, 30);
				item.Reservation = BitConvertUtils.ToSTR(bytes예약, Encoding.Default);

				//20221101 bha
				item.TimeOfArrival = DateTime.Now;

				return item;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
		}


		//static PAJU_BIS_노선정보 Convert노선정보(byte[] bytes)
		//{
		//    try
		//    {
		//        PAJU_BIS_노선정보 item = new PAJU_BIS_노선정보();

		//        if (bytes.Length < 23)
		//        {
		//            return null;
		//        }

		//        byte[] bID= new byte[4];
		//        bID[0] = bytes[0];
		//        bID[1] = bytes[1];
		//        bID[2] = bytes[2];
		//        bID[3] = bytes[3];
		//        item.RouteId = BitConvertUtils.ToInt32(bID, false);

		//        byte[] b번호= new byte[16];
		//        System.Buffer.BlockCopy(bytes, 4, b번호, 0, 16);
		//        item.RouteNumberT = BitConvertUtils.ToSTR(b번호);

		//        item.RouteNumberDisplayColor = (int)bytes[20];
		//        item.RouteDirection= (int)bytes[21];
		//        item.DisplayDestination= (int)bytes[22];

		//        return item;
		//    }
		//    catch (Exception ee)
		//    {
		//        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//        return null;
		//    }
		//}

	}
}
