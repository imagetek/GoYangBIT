using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

using SSCommonNET;

namespace SSData
{
    //public class ClientSocket
    //{
    //    public class AsyncObject
    //    {
    //        public Byte[] Buffer;
    //        public Socket WorkingSocket;
    //        public AsyncObject()
    //        {
    //            Buffer = null;
    //        }
    //    }

    //    private byte[] RecvBuffer = new byte[8192];

    //    private TcpClient m_ClientSocket = null;
    //    private AsyncCallback m_fnReceiveHandler;
    //    private AsyncCallback m_fnSendHandler;

    //    private Queue<byte[]> ReceiveQueue = new Queue<byte[]>();

    //    public ClientSocket()
    //    {
    //        try
    //        {
    //            m_fnReceiveHandler = new AsyncCallback(ReceiveDataCallbackAsync);
    //            m_fnSendHandler = new AsyncCallback(SendDataCallbackAsync);

    //            ServerIPList = BITDataManager.BitConfig.SERVER.SERVER_URL_LIST.Split('|');
    //            string[] PortList = BITDataManager.BitConfig.SERVER.SERVER_PORT_LIST.Split('|');
    //            ServerPortList = System.Array.ConvertAll(PortList, int.Parse);
    //            //if (DataManager.DebugYN == true)
    //            //{
    //            //    ServerIPList[0] = "172.21.7.8";
    //            //    ServerPortList[0] = "9002";
    //            //}
    //        }
    //        catch (Exception ee)
    //        {
    //            System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //        }
    //    }

    //    private string[] ServerIPList = null;
    //    private int[] ServerPortList = null;

    //    public void UpdateServerList()
    //    {
    //        ServerIPList = BITDataManager.BitConfig.SERVER.SERVER_URL_LIST.Split('|');
    //        string[] PortList = BITDataManager.BitConfig.SERVER.SERVER_PORT_LIST.Split('|');
    //        ServerPortList = System.Array.ConvertAll(PortList, int.Parse);
    //    }

    //    private bool ConnectedYN = false;
    //    private int ServerCount = 0;
    //    public void ConnectToServer()
    //    {
    //        try
    //        {
    //            if (ConnectedYN)
    //                return;

    //            if (m_ClientSocket == null)
    //            {
    //                try
    //                {
    //                    m_ClientSocket = new TcpClient(AddressFamily.InterNetwork);
    //                    m_ClientSocket.BeginConnect(
    //                        ServerIPList[ServerCount],
    //                        ServerPortList[ServerCount],
    //                        new AsyncCallback(ConnectCallback),
    //                        this.m_ClientSocket.Client);
    //                }
    //                catch (Exception ex)
    //                {
    //                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
    //                    DisConnectAction();
    //                }
    //            }
    //            else
    //            {
    //                DisConnectAction();
    //            }
    //        }
    //        catch (Exception ee)
    //        {
    //            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //            System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //        }
    //    }

    //    public bool ConnectYN()
    //    {
    //        return ConnectedYN;
    //    }

    //    private void ConnectCallback(IAsyncResult result)
    //    {
    //        try
    //        {
    //            var sc = result.AsyncState as Socket;
    //            if (sc == null)
    //            {
    //                ConnectedYN = false;
    //            }
    //            else
    //            {
    //                if (sc.Connected == false)
    //                {
    //                    return;
    //                }
    //                sc.EndConnect(result);

    //                var callBack = new Action(ConnectCompleteCallback);
    //                callBack.Invoke();
    //            }
    //        }
    //        catch (Exception ee)
    //        {
    //            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //            System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //        }
    //    }

    //    public delegate void ConnectedHandler(bool _connYN, string msg);
    //    public event ConnectedHandler OnConnectedEvent;

    //    private void ConnectCompleteCallback()
    //    {
    //        try
    //        {
    //            if (m_ClientSocket.Connected == true)
    //            {
    //                Log4Manager.WriteSocketLog(Log4Level.Info, string.Format("[TCP] 서버연결 시작 {0}", m_ClientSocket.Client.LocalEndPoint.ToString()));
    //                // 비동기적으로 들어오는 자료를 수신하기 위해 BeginReceive 메서드 사용!
    //                ConnectedYN = true;
    //                m_ClientSocket.Client.BeginReceive(this.RecvBuffer, 0
    //                    , RecvBuffer.Length
    //                    , SocketFlags.None
    //                    , m_fnReceiveHandler
    //                    , m_ClientSocket.Client);
    //            }
    //            else
    //            {
    //                ServerCount++;
    //                if (ServerCount >= ServerIPList.Length)
    //                    ServerCount = 0;
    //            }

    //            if (OnConnectedEvent != null)
    //                OnConnectedEvent(ConnectedYN, ConnectedYN == true ? m_ClientSocket.Client.LocalEndPoint.ToString() : "미연결");
    //        }
    //        catch (Exception ee)
    //        {
    //            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //            System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //            ConnectedYN = false;
    //        }
    //    }

    //    private SocketError SendMessageError = new SocketError();
    //    AsyncObject sendAobj = null;
    //    public bool SendMessage(byte[] message)
    //    {
    //        if (message == null)
    //        {
    //            Log4Manager.WriteSocketLog(Log4Level.Error, "[전송] Send 데이터가 존재하지않습니다.");
    //            return false;
    //        }

    //        sendAobj = new AsyncObject();
    //        if (m_ClientSocket == null)
    //        {
    //            Log4Manager.WriteSocketLog(Log4Level.Error, "[전송] TCPClient가 NULL입니다.");
    //            Action tryReconnect = new Action(DisConnectAction);
    //            tryReconnect.Invoke();
    //            return false;
    //        }

    //        if (m_ClientSocket.Client == null)
    //        {
    //            Log4Manager.WriteSocketLog(Log4Level.Error, "[전송] TCPClient의 Client가 NULL입니다.");
    //            Action tryReconnect = new Action(DisConnectAction);
    //            tryReconnect.Invoke();
    //            return false;
    //        }

    //        sendAobj.Buffer = message;
    //        sendAobj.WorkingSocket = m_ClientSocket.Client;

    //        // 전송 시작!
    //        try
    //        {
    //            m_ClientSocket.Client.BeginSend(message, 0, message.Length, SocketFlags.None, out SendMessageError, m_fnSendHandler, sendAobj);
    //            if (SendMessageError != SocketError.Success)
    //            {
    //                Log4Manager.WriteSocketLog(Log4Level.Error, string.Format("[전송] Send중 오류가 발생했습니다. {0}", SendMessageError.ToString()));

    //                SendMessageError = SocketError.Success;
    //                Action tryReconnect = new Action(DisConnectAction);
    //                tryReconnect.Invoke();
    //            }
    //            return true;
    //        }
    //        catch (Exception ee)
    //        {
    //            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //            System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //            SendMessageError = SocketError.Success;
    //            Action tryReconnect = new Action(DisConnectAction);
    //            tryReconnect.Invoke();
    //            return false;
    //        }
    //    }

    //    SocketError RecvErrorData = new SocketError();
    //    private byte[] msgByte = null;
    //    private void ReceiveDataCallbackAsync(IAsyncResult ar)
    //    {
    //        var ao = ar.AsyncState as Socket;
    //        if (ao == null)
    //        {
    //            Log4Manager.WriteSocketLog(Log4Level.Error, "[Receive] Socket이 NULL입니다.");
    //            return;
    //        }

    //        int bytes = 0;
    //        try
    //        {
    //            if (ao.Connected)
    //            {
    //                if (!(ao.Poll(1, SelectMode.SelectRead) && ao.Available == 0))
    //                    bytes = ao.EndReceive(ar, out RecvErrorData);

    //                if (RecvErrorData != SocketError.Success)//bytes가 0인 경우가 있다.
    //                {
    //                    Log4Manager.WriteSocketLog(Log4Level.Error, string.Format("[Receive] Receive중 에러가 발생했습니다. {0}", RecvErrorData.ToString()));
    //                    RecvErrorData = SocketError.Success;
    //                    Action action = new Action(DisConnectAction);
    //                    action.Invoke();
    //                    return;
    //                }

    //                if (bytes > 0)
    //                {
    //                    msgByte = new byte[bytes];
    //                    Array.Copy(RecvBuffer, 0, msgByte, 0, bytes);
    //                    ReceiveQueue.Enqueue(msgByte);
    //                    m_ClientSocket.Client.BeginReceive(this.RecvBuffer, 0, RecvBuffer.Length, SocketFlags.None, m_fnReceiveHandler, m_ClientSocket.Client);
    //                }
    //                else
    //                {
    //                    if (bytes == 0 && IsCheckSocket(ao))
    //                    {
    //                        Log4Manager.WriteSocketLog(Log4Level.Error, string.Format("[Receive] Receive Byte 0byte입니다."));
    //                    }

    //                    if (IsCheckSocket(ao) == false)
    //                    {
    //                        Log4Manager.WriteSocketLog(Log4Level.Info, string.Format("[Receive] 서버와 연결이 종료되었습니다."));

    //                        Action action = new Action(DisConnectAction);
    //                        action.Invoke();
    //                        return;
    //                    }

    //                    m_ClientSocket.Client.BeginReceive(this.RecvBuffer, 0, RecvBuffer.Length, SocketFlags.None, out RecvErrorData, m_fnReceiveHandler, m_ClientSocket.Client);

    //                    if (RecvErrorData != SocketError.Success)
    //                    {
    //                        Log4Manager.WriteSocketLog(Log4Level.Info, string.Format("[Receive] 서버와 연결이 끊겼습니다."));
    //                        RecvErrorData = SocketError.Success;
    //                        Action action = new Action(DisConnectAction);
    //                        action.Invoke();
    //                        return;
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ee)
    //        {
    //            if (OnDisConnectedEvent != null) OnDisConnectedEvent();
    //            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //            System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //            return;
    //        }
    //    }

    //    private bool IsCheckSocket(Socket socket)
    //    {
    //        try
    //        {
    //            if (socket.Connected)
    //                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
    //        }
    //        catch (Exception ee)
    //        {
    //            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //            System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //            return false;
    //        }
    //        return false;
    //    }

    //    public delegate void DataReceivedHandler(byte[] receiveData);
    //    public event DataReceivedHandler OnDataReceivedEvent;
    //    public delegate void DisConnectedHandler();
    //    public event DisConnectedHandler OnDisConnectedEvent;

    //    private void DataReceive(byte[] datas)
    //    {
    //        try
    //        {
    //            if (datas == null || datas.Length == 0)
    //            {
    //                Log4Manager.WriteSocketLog(Log4Level.Error, string.Format("[Receive] 수신데이터가 NULL 또는 0byte입니다."));
    //                return;
    //            }

    //            //Log4Manager.WriteSocketLog(string.Format("[Socket] {0}byte : {1}", datas.Length, BitConvertUtils.ByteArrayToHexSTR(datas, "-")));
    //            if (OnDataReceivedEvent != null) OnDataReceivedEvent(datas);
    //        }
    //        catch (Exception ee)
    //        {
    //            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //            System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //            throw;
    //        }
    //    }


    //    SocketError SendErrorData = new SocketError();
    //    private void SendDataCallbackAsync(IAsyncResult ar)
    //    {
    //        AsyncObject _sender = ar.AsyncState as AsyncObject;
    //        if (_sender == null)
    //        {
    //            Log4Manager.WriteSocketLog(Log4Level.Error, "[Send] IAsyncResult이 NULL입니다.");
    //            throw new InvalidOperationException("Invalid IAsyncResult - Could not interpret as a socket object");
    //        }

    //        if (_sender.WorkingSocket == null)
    //            throw new InvalidOperationException("@@@@Invalid IAsyncResult - Could not interpret as a socket object");

    //        try
    //        {
    //            _sender.WorkingSocket.EndSend(ar, out SendErrorData);
    //            if (SendErrorData != SocketError.Success)
    //            {
    //                Log4Manager.WriteSocketLog(Log4Level.Error, "[Send] 에러 : " + SendErrorData.ToString());
    //                Action action = new Action(DisConnectAction);
    //                action.Invoke();
    //            }
    //            else
    //            {
    //                Log4Manager.WriteSocketLog(Log4Level.Info, string.Format("[Send] 성공 Size : {0}byte"
    //                    , _sender.Buffer.Length, BitConvertUtils.ByteArrayToHexSTR(_sender.Buffer, "-")));
    //            }
    //        }
    //        catch (Exception ee)
    //        {
    //            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //            Action action = new Action(DisConnectAction);
    //            action.Invoke();
    //            return;
    //        }
    //    }

    //    public void ClientDisConnect()
    //    {
    //        Action tryReconnect = new Action(DisConnectAction);
    //        tryReconnect.Invoke();
    //    }

    //    object Lock_ = new object();
    //    private void DisConnectAction()
    //    {
    //        lock (Lock_)
    //        {
    //            try
    //            {
    //                ConnectedYN = false;
    //                if (m_ClientSocket != null)
    //                {
    //                    if (m_ClientSocket.Connected)
    //                    {
    //                        m_ClientSocket.Client.Shutdown(SocketShutdown.Both);
    //                        m_ClientSocket.Client.BeginDisconnect(false
    //                            , new AsyncCallback(DisconnectByHostCompleteCallback)
    //                            , m_ClientSocket);
    //                    }

    //                    if (m_ClientSocket != null)
    //                    {
    //                        m_ClientSocket.Close();
    //                        m_ClientSocket = null;
    //                    }
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                Log4Manager.WriteSocketLog(Log4Level.Error, string.Format("DisConnectAction ex:{0}", ex.Message));
    //            }

    //            ReceiveQueue.Clear();

    //            //if (bitEventHandler != null)
    //            //{
    //            //    bitEventHandler(isConnected ? "ServeStatus$T" : "ServeStatus$F");
    //            //}
    //        }
    //    }

    //    private void DisconnectByHostCompleteCallback(IAsyncResult result)
    //    {
    //        lock (Lock_)
    //        {
    //            try
    //            {
    //                TcpClient tcpClient = (TcpClient)result.AsyncState;
    //                if (tcpClient != null && tcpClient.Client != null)
    //                {
    //                    tcpClient.Client.EndDisconnect(result);
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                Log4Manager.WriteSocketLog(Log4Level.Error, string.Format("callbackDisconnectByHostComplete ex:{0}", ex.Message));
    //            }
    //        }
    //    }

    //    public bool threadFlag = true;
    //    private DateTime ServerStatusTime = DateTime.Now;
    //    public void ClientSocketThread()
    //    {
    //        while (threadFlag)
    //        {
    //            if (ConnectedYN)
    //            {
    //                try
    //                {
    //                    if (ReceiveQueue.Count > 0)
    //                    {
    //                        DataReceive(ReceiveQueue.Dequeue());
    //                    }

    //                    Thread.Sleep(50);
    //                }
    //                catch (Exception ex)
    //                {
    //                    Log4Manager.WriteSocketLog(Log4Level.Error, string.Format("ClientSocketThread ex:{0}", ex.Message));
    //                    Action action = new Action(DisConnectAction);
    //                    action.Invoke();
    //                }
    //            }
    //            else
    //            {
    //                ConnectToServer();
    //                if (!ConnectedYN)
    //                    Thread.Sleep(3 * 1000);
    //            }

    //            GC.Collect();
    //            GC.WaitForPendingFinalizers();
    //        }
    //    }
    //}

}


