using System;
using System.Collections.Generic;
using System.Text;

namespace SSAgent
{
    public class ShareMemoryManager
    {

        public delegate void ShareMemoryHandler(int GBN, string ShareMsg);
        public static event ShareMemoryHandler OnShareMemoryEvent;
        public static void ShareMemoryEvent(int GBN, string ShareMsg)
        {
            if (OnShareMemoryEvent != null) OnShareMemoryEvent(GBN, ShareMsg);
        }
    }
}