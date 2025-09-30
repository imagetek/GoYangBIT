using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SSCommonNET;
using SSData.DashboardAPI;

namespace SSData
{
	public class SSENV2Modules
	{
		public delegate void DataReceivedHandler(byte[] receiveData);
		public event DataReceivedHandler OnDataReceivedEvent;

		public delegate void DisConnectedHandler();
		public event DisConnectedHandler OnDisConnectedEvent;

		public delegate void SpErrorHandler();
		public event SpErrorHandler OnSpErrorEvent;

		private SerialPort sp;

		private Thread dtCheckSerialPort;
		private bool CheckSerialPortYN = false;

		public SSENV2Modules()
		{
		}

		//Return current state of Serial Port(is open or not)
		public bool IsOpen
		{
			get
			{
				if (sp != null)
				{
					return sp.IsOpen;
				}
				else
				{
					return false;
				}
			}
		}

		public bool InitProc(string strPortName, int baudRate
			, int dataBits = 8
			, StopBits stopBits = StopBits.One  //pjhpjh   .One.Two
			, Parity parity = Parity.None
			, Handshake handShake = Handshake.None)
		{
			//Start & Open Serial Port
			try
			{
				if (sp != null) return false;

				sp = new SerialPort();

				sp.PortName = strPortName;
				sp.BaudRate = baudRate;
				sp.DataBits = dataBits;
				sp.StopBits = stopBits;
				sp.Parity = parity;
				sp.Handshake = Handshake.None;

				sp.Encoding = new System.Text.ASCIIEncoding();
				sp.NewLine = "\r\n";
				sp.ErrorReceived += Sp_ErrorReceived;
				sp.DataReceived += Sp_DataReceived;
				sp.WriteTimeout = -1;
				sp.ReadTimeout = -1;
				sp.Open();

				StartCheckSerialPort();

				return true;
			}
			catch (Exception ex)
			{
				HttpService.UpdateSystemStatus(new SSNS_ENV2_STATE(), 0);
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
				Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
				return false;
			}
		}

		private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			try
			{
				Thread.Sleep(10);
				byte[] bytesBuffer = ReadSerialByteData();
				if (bytesBuffer.Length == 0) return;
				string strBuffer = Encoding.ASCII.GetString(bytesBuffer);
				Log4Manager.WriteENVLog(Log4Level.Debug, string.Format("[Rx] {0:d6}: {1}", strBuffer.Length, BitConvertUtils.ByteArrayToHexSTR(bytesBuffer, "-")));
				OnDataReceivedEvent?.Invoke(bytesBuffer);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
				Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
			}
		}

		private void Sp_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
		{
			try
			{
				TraceManager.AddLog(string.Format("### {0} : {1} ###", e.EventType.ToString(), e.ToString()));
				OnSpErrorEvent?.Invoke();
				HttpService.UpdateSystemStatus(new SSNS_ENV2_STATE(), 0);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
				Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
			}
		}

		public void DoFinal()
		{
			//Close Serial Communication
			try
			{
				//ClearCheckSerialPort();

				if (sp != null)
				{
					sp.Close();
					sp = null;
				}
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
				Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
			}
		}

		public async void SendTask(byte[] byteSendData)
		{
			try
			{
				if (sp != null && sp.IsOpen)
				{
					Task task = Task.Run(() => sp.Write(byteSendData, 0, byteSendData.Length));
					await task;
					return;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public bool SendData(byte[] byteSendData)
		{
			SendTask(byteSendData);
			return true;
		}

		public bool SendData(byte[] byteSendData, int offSet, int count)
		{
			try
			{
				if (sp != null && sp.IsOpen)
				{
					sp.Write(byteSendData, offSet, count);

					if (count > 30)
					{
						byte[] sends = new byte[30];
						Buffer.BlockCopy(byteSendData, 0, sends, 0, sends.Length);
						Log4Manager.WriteENVLog(Log4Level.Debug, string.Format("[Tx] {0:d6}: {1}", count, BitConvertUtils.ByteArrayToHexSTR(sends, "-")));
					}
					else
						Log4Manager.WriteENVLog(Log4Level.Debug, string.Format("[Tx] {0:d6}: {1}", count, BitConvertUtils.ByteArrayToHexSTR(byteSendData, "-")));

					return true;
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			return false;
		}

		byte[] ReadSerialByteData()
		{
			try
			{
				sp.ReadTimeout = 10; //pjh 100 to 10
				byte[] bytesBuffer = new byte[sp.BytesToRead];

				int bufferOffSet = 0;
				int bytesToRead = sp.BytesToRead;

				while (bytesToRead > 0)
				{
					try
					{
						int readBytes = sp.Read(bytesBuffer, bufferOffSet, bytesToRead - bufferOffSet);
						bytesToRead -= readBytes;
						bufferOffSet += readBytes;
					}
					catch (Exception ee)
					{
						TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
						Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
					}
				}

				return bytesBuffer;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
				Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
				return null;
			}
		}

		private void StartCheckSerialPort()
		{
			try
			{
				if (dtCheckSerialPort == null)
				{
					dtCheckSerialPort = new Thread(new ThreadStart(CheckSerialPortComm));
					dtCheckSerialPort.Start();
				}
				CheckSerialPortYN = true;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		//private void StopCheckSerialPort()
		//{
		//	try
		//	{
		//		if (CheckSerialPortYN)
		//		{
		//			CheckSerialPortYN = false;
		//			if (Thread.CurrentThread != dtCheckSerialPort)
		//			{
		//				dtCheckSerialPort.Join();
		//			}
		//		}
		//	}
		//	catch (Exception ee)
		//	{
		//		TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//		System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		//	}
		//}

		private void ClearCheckSerialPort()
		{
			try
			{
				if (dtCheckSerialPort != null)
				{
					if (dtCheckSerialPort.IsAlive)
					{
						//dtCheckSerialPort.Join()
						dtCheckSerialPort.Abort();
						Thread.Sleep(100);
					}
					dtCheckSerialPort = null;
				}

				CheckSerialPortYN = false;

				GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void CheckSerialPortComm()  //pjh Worker Thread
		{
			try
			{
				while (CheckSerialPortYN)
				{
					try
					{
						if (sp == null || sp.IsOpen == false)
						{
							Log4Manager.WriteENVLog(Log4Level.Debug, "SerialPort DisConnected");

							if (OnDisConnectedEvent != null)
								OnDisConnectedEvent();
							break;
						}

						Thread.Sleep(1000);
						GC.Collect();
					}
					catch (Exception ex)
					{
						TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
						System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
					}

				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
	}
}
