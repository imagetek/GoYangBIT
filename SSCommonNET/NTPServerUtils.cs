using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SSCommonNET
{
	public class NTPServerUtils
	{
		public static DateTime? GetUTCTime(string URL)
		{
			try
			{
				string hostNameOrAddress = URL;
				byte[] buffer = new byte[48];
				buffer[0] = (byte)27;
				IPEndPoint remoteEP = new IPEndPoint(Dns.GetHostEntry(hostNameOrAddress).AddressList[0], 123);
				using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
				{
					socket.Connect((EndPoint)remoteEP);
					socket.ReceiveTimeout = 3000;
					socket.Send(buffer);
					socket.Receive(buffer);
					socket.Close();
				}
				long uint32_1 = (long)BitConverter.ToUInt32(buffer, 40);
				ulong uint32_2 = (ulong)BitConverter.ToUInt32(buffer, 44);
				return new DateTime?(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((double)(long)((ulong)((long)NTPServerUtils.SwapEndianness((ulong)uint32_1) * 1000L) + (ulong)NTPServerUtils.SwapEndianness(uint32_2) * 1000UL / 4294967296UL)));
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return new DateTime?();
			}
		}

		public static DateTime? GetLocalTime(string URL)
		{
			try
			{
				string hostNameOrAddress = URL;
				byte[] buffer = new byte[48];
				buffer[0] = (byte)27;
				IPEndPoint remoteEP = new IPEndPoint(Dns.GetHostEntry(hostNameOrAddress).AddressList[0], 123);
				using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
				{
					socket.Connect((EndPoint)remoteEP);
					socket.ReceiveTimeout = 3000;
					socket.Send(buffer);
					socket.Receive(buffer);
					socket.Close();
				}
				long uint32_1 = (long)BitConverter.ToUInt32(buffer, 40);
				ulong uint32_2 = (ulong)BitConverter.ToUInt32(buffer, 44);
				return new DateTime?(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((double)(long)((ulong)((long)NTPServerUtils.SwapEndianness((ulong)uint32_1) * 1000L) + (ulong)NTPServerUtils.SwapEndianness(uint32_2) * 1000UL / 4294967296UL)).ToLocalTime());
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return new DateTime?();
			}
		}

		public static void SynchronizieNTPServer(string URL)
		{
			try
			{
				DateTime? utcTime = NTPServerUtils.GetUTCTime(URL);
				if (!utcTime.HasValue || !utcTime.HasValue)
				{
					TraceManager.AddLog(string.Format("[NTP] NTP서버에서 시간정보를 가져오지 못했습니다. {0}", (object)URL));
				}
				else
				{
					TraceManager.AddLog(string.Format("[NTP] NTP서버({0})와 동기화했습니다. {1}", (object)URL, (object)utcTime.Value.ToString("yyyy-MM-dd HH:mm:sss")));
					CommonUtils.SetSystemTimeByUTC(utcTime.Value);
				}
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private static uint SwapEndianness(ulong x)
		{
			return (uint)((ulong)((((long)x & (long)byte.MaxValue) << 24) + (((long)x & 65280L) << 8)) + ((x & 16711680UL) >> 8) + ((x & 4278190080UL) >> 24));
		}
	}
}
