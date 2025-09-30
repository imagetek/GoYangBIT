using System;

using SSCommonNET;
using System.Speech.Synthesis;

//SpeechSynthesizer ts = new SpeechSynthesizer();

namespace SSData
{
    public class MSTTSManager
	{
        static MSTTSManager()
        {
        }
        
        static SpeechSynthesizer ts = null;
		public bool InitializeTTS()
		{
			try
			{
				ts ??= new SpeechSynthesizer();

				// 보이스를 선택하지 않아도 처리됨
				ts.SelectVoice("Microsoft Heami Desktop");
				ts.SetOutputToDefaultAudioDevice();
				//ts.SpeakCompleted += Ts_SpeakCompleted;
				return true;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
		}


		public void Play(string message)
		{
			//string Message = "";
			//string m = "";
			//foreach (string bis in bisInfo)
			//{
			//	string bisNo = bis.Replace("-", "다시,");
			//	bisNo = bisNo.Replace("M", "엠, ");
			//	bisNo = bisNo.Replace("A", "에이, ");
			//	bisNo = bisNo.Replace("B", "비, ");
			//	bisNo = bisNo.Replace("C", "씨, ");
			//	Message += string.Format("{0}번, , ", bisNo);
			//	 m = string.Format("{0}번, ", bisNo);
			//	//edgar if use 번 then sound is strage
			//	ts.SpeakAsync(bisNo);
			//	//ts.Speak("번");
			//}

			//Message += "버스가 곧 도착 예정 입니다";
			//m = "버스가 곧 도착 예정 입니다";
			//Console.WriteLine("TTS : {0}", Message);

			try
			{
				//ts.Volume = 100;
				ts.Speak(message);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public void DoFinal()
		{
			ts.Dispose();
		}		
	}
}



