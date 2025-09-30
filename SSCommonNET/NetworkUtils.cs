using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace SSCommonNET
{
	public class NetworkUtils
	{
		public static bool IsNumeric(string num)
		{
			try
			{
				double result = 0.0;
				return double.TryParse(num, out result);
			}
			catch (Exception ex)
			{
				Console.WriteLine("{0}\r\n{1}", (object)ex.StackTrace, (object)ex.Message);
				return false;
			}
		}

		public static string GetHostName() => Dns.GetHostName();

		public static string GetIPAddress()
		{
			string ipAddress = "";
			try
			{
				IPAddress[] addressList = Dns.GetHostEntry(NetworkUtils.GetHostName()).AddressList;
				for (int index = 0; index < addressList.Length; ++index)
				{
					if (addressList[index].AddressFamily == AddressFamily.InterNetwork)
					{
						ipAddress = addressList[index].ToString();
						if (ipAddress.Split('.').Length >= 4)
							break;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("{0}\r\n{1}", (object)ex.StackTrace, (object)ex.Message);
				return "";
			}
			return ipAddress;
		}

		public static string GetMacAddress(string s_ip)
		{
			string macAddress = NetworkUtils.GetMacFromIP(IPAddress.Parse(s_ip)).ToString();
			for (int startIndex = 2; startIndex < macAddress.Length; startIndex += 3)
				macAddress = macAddress.Insert(startIndex, ":");
			return macAddress;
		}

		private static PhysicalAddress GetMacFromIP(IPAddress IP)
		{
			int destIpAddress = IP.AddressFamily == AddressFamily.InterNetwork ? NetworkUtils.IpToInt(IP) : throw new ArgumentException("supports just IPv4 addresses");
			int num1 = NetworkUtils.IpToInt(IP);
			byte[] address1 = new byte[6];
			int length = address1.Length;
			int srcIpAddress = num1;
			byte[] macAddress = address1;
			ref int local = ref length;
			int num2 = NetworkUtils.SendArp(destIpAddress, srcIpAddress, macAddress, ref local);
			byte[] address2 = new byte[12];
			return num2 != 0 ? new PhysicalAddress(address2) : new PhysicalAddress(address1);
		}

		private static int IpToInt(IPAddress IP) => BitConverter.ToInt32(IP.GetAddressBytes(), 0);

		[DllImport("Iphlpapi.dll", EntryPoint = "SendARP")]
		internal static extern int SendArp(
		  int destIpAddress,
		  int srcIpAddress,
		  byte[] macAddress,
		  ref int macAddressLength);

		public static bool CheckIPPort(string ip, int port, int timeout = 5000)
		{
			DateTime now = DateTime.Now;
			try
			{
				TimeSpan timeout1 = TimeSpan.FromMilliseconds((double)timeout);
				IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);
				Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IAsyncResult asyncResult = socket.BeginConnect((EndPoint)remoteEP, (AsyncCallback)null, (object)null);
				int num = asyncResult.AsyncWaitHandle.WaitOne(timeout1, true) ? 1 : 0;
				if (num != 0)
				{
					socket.EndConnect(asyncResult);
					socket.Close();
				}
				else
					socket.Close();
				Console.WriteLine("[CheckIPPort : {0}ms]", (object)CommonUtils.GetDisplayTime((DateTime.Now - now).TotalMilliseconds));
				return num != 0;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return false;
			}
		}
	}
}
