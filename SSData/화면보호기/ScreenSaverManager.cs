using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SSCommonNET;

namespace SSData
{
    public class ScreenSaverManager
    {
        #region 절전모드금지

        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "SetThreadExecutionState", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE flags);

        public enum EXECUTION_STATE : uint
        {
            ES_SYSTEM_REQUIRED = 0x00000001,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_USER_PRESENT = 0x00000004,
            ES_CONTINUOUS = 0x80000000
        }

        // 절전/대기 모드 진입 금지
        public static void NotUse절전대기모드()
        {
            try
            {
                SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        // 절전/대기 모드 진입 허용
        public static void Use절전대기모드()
        {
            try
            {
                SetThreadExecutionState(~EXECUTION_STATE.ES_DISPLAY_REQUIRED & EXECUTION_STATE.ES_CONTINUOUS & ~EXECUTION_STATE.ES_SYSTEM_REQUIRED);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        //// 절전 모드 관련 define
        //private const int WM_POWERBROADCAST = 0x0218;
        //private const int PBT_APMPOWERSTATUSCHANGE = 0x000A; // Power status has changed.
        //private const int PBT_APMQUERYSUSPEND = 0x0000; // Request for permission to suspend.
        //private const int PBT_APMQUERYSTANDBY = 0x0001;
        //private const int PBT_APMQUERYSUSPENDFAILED = 0x0002; // Suspension request denied.
        //private const int PBT_APMQUERYSTANDBYFAILED = 0x0003; // Standby request denied.
        //private const int PBT_APMSUSPEND = 0x0004; // System is suspending operation.
        //private const int PBT_APMSTANDBY = 0x0005; // System is standby operation.
        //private const int BROADCAST_QUERY_DENY = 0x424D5144; // Return this value to deny a query.

        //protected override void WndProc(ref Message message)
        //{
        //    base.WndProc(ref message);

        //    switch (message.Msg)
        //    {
        //        case WM_POWERBROADCAST: // Power Broadcast 메시지
        //            switch (message.WParam.ToInt32())
        //            {
        //                case PBT_APMPOWERSTATUSCHANGE:
        //                    System.Diagnostics.Debug.WriteLine("[WM_POWERBROADCAST] PBT_APMPOWERSTATUSCHANGE  : 파워 상태 변경 메시지~!!!");
        //                    break;

        //                case PBT_APMQUERYSUSPENDFAILED:
        //                    System.Diagnostics.Debug.WriteLine("[WM_POWERBROADCAST] PBT_APMQUERYSUSPENDFAILED  : 절전모드 해제됨~!!!");
        //                    break;

        //                case PBT_APMSUSPEND:
        //                    System.Diagnostics.Debug.WriteLine("[WM_POWERBROADCAST] PBT_APMSUSPEND  : 절전모드 상태 진입~!!!");
        //                    break;

        //                case PBT_APMSTANDBY:
        //                    System.Diagnostics.Debug.WriteLine("[WM_POWERBROADCAST] PBT_APMSTANDBY : 대기모드 상태 진입~!!!");
        //                    break;

        //                case PBT_APMQUERYSUSPEND:
        //                    System.Diagnostics.Debug.WriteLine("[WM_POWERBROADCAST] PBT_APMQUERYSUSPEND : 절전모드로 진입할까요?");
        //                    if (message.LParam.ToInt32() == PBT_APMQUERYSTANDBY) // 대기모드 진입 메시지
        //                    {
        //                        DialogResult res = MessageBox.Show("대기모드로 진입할까요?", "대기모드", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //                        System.Diagnostics.Debug.WriteLine("대기모드(PBT_APMQUERYSTANDBY) 메시지 수신");
        //                        if (res == DialogResult.No)
        //                        {
        //                            IntPtr deny = new IntPtr(BROADCAST_QUERY_DENY);
        //                            message.Result = deny;
        //                        }
        //                        else
        //                        {
        //                            message.Result = (IntPtr)1;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        System.Diagnostics.Debug.WriteLine("절전모드(PBT_APMQUERYSUSPEND) 메시지 거부~");
        //                        IntPtr deny = new IntPtr(BROADCAST_QUERY_DENY);
        //                        message.Result = deny;
        //                    }
        //                    break;
        //            }
        //    }
        //}

        #endregion
    }
}
