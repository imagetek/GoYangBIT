using System;
using System.Collections.Generic;
using System.Text;

namespace BitView
{
    public class ShareMemoryManager
    {

        public delegate void ShareMemoryHandler(int GBN, string ShareMsg);
        public static event ShareMemoryHandler OnShareMemoryEvent;
		//public static void SendWebcamMessage(string ShareMsg)
		//{
		//	if (OnShareMemoryEvent != null) OnShareMemoryEvent(5, ShareMsg);
		//}

		public static void SendBITMessage(string ShareMsg)
        {
            if (OnShareMemoryEvent != null) OnShareMemoryEvent(2, ShareMsg);
        }

        public static void SendAgentMessage(string ShareMsg)
        {
            if (OnShareMemoryEvent != null) OnShareMemoryEvent(1, ShareMsg);
        }
    }
}