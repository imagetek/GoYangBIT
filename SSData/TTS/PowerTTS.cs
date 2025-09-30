using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SSCommonNET;

namespace SSData
{
	public class PowerTTS
	{
		//public string Message { get; set; }
		private const string DLLFileName = @"PowerTTS_M.dll";

		public const string OEMKeyNumber = "u5j6ANPZtNdBs5XZplfZWQAAu$fZu5fZu5fZuWBeOW4THbpku5d8ZbdfEpw@3NfZ2T$bG5f@!!zp@PPq$7Tv$mfgy#jlzO!mw6Dbw8^^";

		[DllImport(DLLFileName, EntryPoint = "PTTS_Initialize")]
		public static extern int PTTS_Initialize();

		[DllImport(DLLFileName)]
		public static extern void PTTS_UnInitialize();

		[DllImport(DLLFileName)]
		public static extern void PTTS_SetOemKey(string OemKey);

		[DllImport(DLLFileName)]
		public static extern int PTTS_LoadEngine(int Language, string DbDir, int bLoadInMemory);

		[DllImport(DLLFileName)]
		public static extern void PTTS_UnLoadEngine(int Language);

		[DllImport(DLLFileName)]
		public static extern int PTTS_PlayTTS(IntPtr hUsrWnd, uint uUsrMsg, string TextBuf, string tagString, int Language, int SpeakerID);

		[DllImport(DLLFileName)]
		public static extern int PTTS_StopTTS();

		[DllImport(DLLFileName)]
		public static extern int PTTS_PauseTTS();

		[DllImport(DLLFileName)]
		public static extern int PTTS_RestartTTS();

		[DllImport(DLLFileName)]
		public static extern int PTTS_TextToFile(int Language, int SpeakerID, int Format, int Encoding, int SamplingRate,
					  string TextBuf, string OutFileName, string tagString, string UserDictFileName);

		System.Windows.Threading.DispatcherTimer _dt재생 = null;
		public void Open()
		{
			try
			{
				// ThreadExitEvent = new AutoResetEvent(false);                

				PTTS_Initialize();
				PTTS_SetOemKey(OEMKeyNumber);
				PTTS_LoadEngine(0, "PowerTTS_M_DB\\KO_KR\\", 0);
				PTTS_PlayTTS(new IntPtr(), 0, "tts가 시작되었습니다.", "", 0, 0);

				_dt재생 ??= new System.Windows.Threading.DispatcherTimer();
				_dt재생.Tick += _dt재생_Tick;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
			}
		}

		void Close()
		{
			try
			{
				PTTS_UnLoadEngine(0);
				PTTS_UnInitialize();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
			}
		}

		public void DoFinal()
		{
			try
			{
				Close();

				GC.Collect();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
			}
		}

		bool IsSoundPlay = false;
		public void Play(string Message)
		{
			try
			{
				if (IsSoundPlay == true)
				{
					//TraceManager.AddInfoLog(string.Format("현재 TTS재생중이라 생략 : {0}", Message));
				}
				else
				{
					IsSoundPlay = true;

					PTTS_PlayTTS(new IntPtr(), 0, Message, "", 0, 0);

					_dt재생.Interval = TimeSpan.FromMilliseconds((0.18 * Message.Length) * 1000);
					_dt재생.Start();
				}
			}
			catch (AccessViolationException ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				Close();
				Open();
			}
		}

		private void _dt재생_Tick(object sender, EventArgs e)
		{
			try
			{
				_dt재생.Stop();
				IsSoundPlay = false;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

	}
}



