using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;
using SSData;

namespace BitView
{
    public class EventManager
    {
        private EventManager _shared;

        public EventManager Shared
        {
            get
            {
                if (_shared == null) _shared = new EventManager();

                return _shared;
            }
        }

        /// <summary>
        /// 로그파일
        /// </summary>
        public delegate void DisplayLogHandler(Log4Data log4);
        public static event DisplayLogHandler OnDisplayLogEvent;
        public static void DisplayLog(Log4Level lv, string log, LogSource logSource = LogSource.Other)
        {
			Log4Data log4 = new()
			{
				Level = lv,
				REGDATE = DateTime.Now,
				로그내용 = log,
				LogSource = logSource
			};

			OnDisplayLogEvent?.Invoke(log4);

			//TraceManager.AddInfoLog(log);
			Log4Manager.WriteLog(lv, log);
        }

        /// <summary>
        /// 화면조정
        /// </summary>
        public delegate void RefreshDisplayHandler(int _nMode, BIT_DISPLAY _item);
        public static event RefreshDisplayHandler OnRefreshDisplayEvent;
        /// <summary>
        /// 화면변경 Mode 1 : Insert , 2: Update , 3: Delete
        /// </summary>
        public static void RefreshDisplay(int nMode, BIT_DISPLAY item)
        {
			OnRefreshDisplayEvent?.Invoke(nMode, item);
		}

        /// <summary>
        /// TTS재생
        /// </summary>
        public delegate void PlayTTSBusNoHandler(List<string> _items);
        public static event PlayTTSBusNoHandler OnPlayBusNoTTSEvent;

        public delegate void PlayTTSMessageHandler(string _item);
        public static event PlayTTSMessageHandler OnPlayMessageTTSEvent;

        public static void PlayBusNoTTS(List<string> _items)
        {
			OnPlayBusNoTTSEvent?.Invoke(_items);
		}

        //public static void PlayMessageTTS(string _item)
        //{
        //    if (OnPlayMessageTTSEvent != null) OnPlayMessageTTSEvent(_item);
        //}
    }
}
