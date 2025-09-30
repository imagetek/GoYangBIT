using SSCommonNET;
using System;

namespace SSData.Utils
{
	public static class VolumeService
	{
		[System.Runtime.InteropServices.DllImport("winmm.dll")]
		public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
		public static void SetVolume(int vol)
		{
			try
			{
				int NewVolume = ((ushort.MaxValue / 100) * (vol * 10));
				uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
				waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);

			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
	}
}
