using System;
using System.Collections.Generic;
using System.Text;

using SSCommonNET;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Globalization;

namespace SSData
{
    public class WOLManager : UdpClient
    {
        public WOLManager() : base()
        { }
        //this is needed to send broadcast packet
        public void SetClientToBrodcastMode()
        {
            if (this.Active)
                this.Client.SetSocketOption(SocketOptionLevel.Socket,
                                          SocketOptionName.Broadcast, 0);
        }

        public void WakeFunction(string MAC_ADDRESS)
        {
            try
            {
                WOLManager client = new WOLManager();
                client.Connect(new
                   IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                   0x2fff); // port=12287 let's use this one 
                client.SetClientToBrodcastMode();
                //set sending bites
                int counter = 0;
                //buffer to be send
                byte[] bytes = new byte[1024];   // more than enough :-)
                                                 //first 6 bytes should be 0xFF
                for (int y = 0; y < 6; y++)
                    bytes[counter++] = 0xFF;
                //now repeate MAC 16 times
                for (int y = 0; y < 16; y++)
                {
                    int i = 0;
                    for (int z = 0; z < 6; z++)
                    {
                        bytes[counter++] =
                            byte.Parse(MAC_ADDRESS.Substring(i, 2),
                            NumberStyles.HexNumber);
                        i += 2;
                    }
                }

                //now send wake up packet
                int reterned_value = client.Send(bytes, 1024);
                Console.WriteLine("### send packet :{0} ###", reterned_value);
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }
    }
}