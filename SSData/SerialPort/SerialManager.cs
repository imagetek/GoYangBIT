using System;
using System.IO.Ports;
using SSCommonNET;

namespace SSData
{
    public class SerialManager
    {
        SerialPort _sp = null;
        public SerialManager()
        {
           try
            {
                //InitProc();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        public void InitProc(string S_PORT)
        {
            try
            {
                if (_sp == null)
                {
                    _sp = new SerialPort();
                    _sp.DataReceived += _sp_DataReceived;
                    _sp.BaudRate = 57600;
                    _sp.PortName = S_PORT;
                    _sp.DataBits = 8;
                    _sp.Parity = Parity.None;
                    _sp.StopBits = StopBits.One;
                    _sp.ReadTimeout = 1000;
                    _sp.WriteTimeout = 1000;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        byte STX = 0x02;
        byte ETX = 0x03;

        byte CMD_ON = 0xFF;
        byte CMD_OFF = 0x00;
        
        //List<string> datas = new List<string>();
        //bool isRecv = false;
        //bool IsRecvedData = false;
        //public delegate void OnRecieveHandler(bool _isData);
        //public event OnRecieveHandler OnRecieveEvent;

        void _sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //isRecv = true;
                //Console.WriteLine("센서데이터 수신");

                int size = _sp.BytesToRead;
                byte [] btSP = new byte[size];
                //int data = _sp.Read(btSP, 0, size);
                string tmp = BitConvertUtils.ByteArrayToHexSTR(btSP , "-");
                Console.WriteLine(string.Format("[센서수신] {0}", tmp));
                if (size > 0)
                {
                    //IsRecvedData = true;
                    //if (OnRecieveEvent != null) OnRecieveEvent(IsRecvedData);                    
                }

                _sp.DiscardInBuffer();
                _sp.DiscardOutBuffer();                
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
            finally
            {
              //  isRecv = false;
            }
        }

        public bool IsOn()
        {
            try
            {
                if (_sp == null) return false;

                ClearBuffer();

                Console.WriteLine(string.Format("센서인식 {0} ", DateTime.Now.ToString("HH:mm:ss")));
                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public bool Open()
        {
            try
            {
                if (_sp == null) return false;

                _sp.Open();
                return true;
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public bool IsConnected()
        {
            if (_sp == null) return false;

            return _sp.IsOpen;
        }

        public bool Close()
        {
           try
            {
                if (_sp == null) return false;

                if (_sp.IsOpen == true)
                {
                    ClearBuffer();
                    _sp.Close();                    
                }
                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public void Send명령ON()
        {
            try
            {
                if (IsConnected() == false) Open();
                
                ClearBuffer();

                byte[] SendData = new byte[3];
                SendData[0] = STX;
                SendData[1] = CMD_ON;
                SendData[2] = ETX;

                _sp.Write(SendData, 0, SendData.Length);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public void Send명령OFF()
        {
            try
            {
                if (IsConnected() == false) Open();

                ClearBuffer();

                byte[] SendData = new byte[3];
                SendData[0] = STX;
                SendData[1] = CMD_OFF;
                SendData[2] = ETX;
                                
                _sp.Write(SendData, 0, SendData.Length);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public void ClearBuffer()
        {            
            try
            {
                if (_sp == null) return;

                if (_sp.IsOpen == true)
                {
                    //datas.Clear();
                    //IsRecvedData = false;
                    _sp.DiscardInBuffer();
                    _sp.DiscardOutBuffer();
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));             
            }
        }

    }
}
