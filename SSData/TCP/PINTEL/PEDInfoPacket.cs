using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;
using System.Runtime.Serialization.Formatters.Binary;

namespace SSData
{
    [Serializable]
    public class PEDInfoPacket
    {
        public PEDInfoPacket()
        {
            
        }

        const byte STX = 0x02;
        const byte ETX = 0x03;        
        //public static byte[] Convert텍스트출력(string text)
        //{
        //    try
        //    {
        //        List<byte> items = new List<byte>();
        //        //STX
        //        items.Add(STX);
        //        //CMD                
        //        items.Add(Convert.ToByte(CMDType.TEXT출력));
        //        //LUMINANCE
        //        items.Add(Convert.ToByte(LUMType.자동));
        //        //DATA 
        //        byte[] MSG = Encoding.ASCII.GetBytes(text);
                
        //        int Length = MSG.Length;
        //        byte[] btLength = BitConverter.GetBytes(Length).Reverse().ToArray();

        //        items.AddRange(btLength);
        //        items.AddRange(MSG);
        //        items.Add(ETX);

        //        return items.ToArray();
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine("### {0}\r\n{1} ###", ee.StackTrace, ee.Message);
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        /// <summary>
        /// Byte Array to PED 변환
        /// </summary>
        public static PEDInfo ToPEDInfo(byte[] bytes, bool WriteLog)
        {
            PEDInfo item = new PEDInfo();
            try
            {
                //수신 362628
                byte bCMD = bytes[1];
                int CMD = Convert.ToInt16(bCMD);
                item.CMD_GBN = (CMDType)CMD;

                byte bLUM = bytes[2];
                int LUMIN = Convert.ToInt16(bLUM);
                item.LUMIN_GBN = (LUMType)LUMIN;

                byte[] bLEN = new byte[4];
                Buffer.BlockCopy(bytes.ToArray(), 3, bLEN, 0, 4);
                int Length = BitConvertUtils.ToInt32(bLEN);
                //Length : 363620
                if (Length <= 0)
                {
                    Console.WriteLine("길이데이터가 미존재");
                    return null;
                }
                //126976
                byte[] bData = new byte[Length];
                Buffer.BlockCopy(bytes.ToArray(), 7, bData, 0, Length);

                if (item.FILE_BINARY == null) item.FILE_BINARY = new List<byte>();
                item.FILE_BINARY.Clear();

                switch (item.CMD_GBN)
                {
                    case CMDType.텍스트표출:
                        item.FILE_BINARY = bData.ToList();
                        item.DISPTXT = Encoding.ASCII.GetString(bData);
                        if (Length == 20) item.DISPTXT = item.DISPTXT.Replace($"\0", "");
                        break;

                    case CMDType.이미지표출:
                        byte[] bNM2 = new byte[20];
                        byte[] bFILE2 = new byte[Length - 20];
                        //bFILE2 363600  5 2 0 59

                        //20220503 bha 
                        if (bytes.Length - 8 != Length)
                        {
                            Log4Manager.WriteSocketLog(Log4Level.Debug, string.Format("[미일치] 정의 : {0}byte / 실제 : {1}byte"
                               , Length, bytes.Length - 8));

                            bFILE2 = new byte[bytes.Length - 8 - 20];
                            Buffer.BlockCopy(bData, 0, bNM2, 0, 20);
                            Buffer.BlockCopy(bytes.ToArray(), 7+20, bFILE2, 0, bFILE2.Length);

                            //Buffer.BlockCopy(bytes.ToArray(), 7, bData, 0, Length);

                            item.FILE_NM = Encoding.ASCII.GetString(bNM2).Replace($"\0", "");
                            item.FILE_BINARY = bFILE2.ToList();
                        }
                        else
                        {
                            Log4Manager.WriteSocketLog(Log4Level.Debug, string.Format("[일치] 정의 : {0}byte / 실제 : {1}byte"
                               , Length, bytes.Length - 8));

                            Buffer.BlockCopy(bData, 0, bNM2, 0, 20);
                            Buffer.BlockCopy(bData, 20, bFILE2, 0, bFILE2.Length);
                            item.FILE_NM = Encoding.ASCII.GetString(bNM2).Replace($"\0", "");
                            item.FILE_BINARY = bFILE2.ToList();
                        }
                                                
                        try
                        {
                            if (item.FILE_BINARY != null && item.FILE_BINARY.Count > 0)
                            {
                                System.IO.MemoryStream ms = new System.IO.MemoryStream(bFILE2);
                                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                                if (img != null)
                                {
                                    System.IO.FileInfo fi = new System.IO.FileInfo(item.FILE_NM);

                                    string 저장DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "TEMP");
                                    if (System.IO.Directory.Exists(저장DIR) == false) System.IO.Directory.CreateDirectory(저장DIR);
                                    //string 저장FILE = System.IO.Path.Combine(저장DIR, item.FILE_NM);// string.Format("{0}", DateTime.Now.ToString("yyyyMMddHHmmsss")));
                                    //if (System.IO.File.Exists(저장FILE) == true) System.IO.File.Delete(저장FILE);

                                    string 저장FILE = System.IO.Path.Combine(저장DIR, string.Format("{0}_{1}{2}", fi.Name.Replace(fi.Extension,""), DateTime.Now.ToString("yyyyMMddHHmmsss"), fi.Extension));// string.Format("{0}", DateTime.Now.ToString("yyyyMMddHHmmsss")));
                                    if (System.IO.File.Exists(저장FILE) == true) System.IO.File.Delete(저장FILE);

                                    img.Save(저장FILE);
                                    item.FILE_NM = 저장FILE;

                                    fi = null;
                                }
                            }
                        }
                        catch (System.IO.IOException ex)
                        {
                            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                            System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                        }
                        break;

                    case CMDType.파일저장:
                        byte[] bNM = new byte[20];
                        byte[] bFILE = new byte[Length - 20];

                        Buffer.BlockCopy(bData, 0, bNM, 0, 20);
                        Buffer.BlockCopy(bData, 20, bFILE, 0, bFILE.Length);
                        item.FILE_NM = Encoding.ASCII.GetString(bNM).Replace($"\0", "");
                        item.FILE_BINARY = bFILE.ToList();
                        break;

                    case CMDType.파일삭제:
                        item.FILE_BINARY = bData.ToList();
                        item.FILE_NM = Encoding.ASCII.GetString(bData);
                        if (Length == 20) item.FILE_NM = item.FILE_NM.Replace($"\0", "");
                        break;

                    case CMDType.저장파일_화면표출:
                        item.FILE_BINARY = bData.ToList();
                        item.FILE_NM = Encoding.ASCII.GetString(bData);
                        if (Length == 20) item.FILE_NM = item.FILE_NM.Replace($"\0", "");
                        break;

                    case CMDType.표출중지:
                        item.FILE_BINARY = bData.ToList();
                        item.DISPTXT = Encoding.ASCII.GetString(bData);
                        //if (Length == 1) item.DISPTXT = item.DISPTXT.Replace($"\0", "");
                        break;
                }

                if (WriteLog == true)
                {
                    string log = BitConvertUtils.ByteArrayToHexSTR(bytes, "-");
                    Log4Manager.WriteSocketLog(Log4Level.Debug, log);
                }
                return item;
            }
            catch (Exception ee)
            {
                Console.WriteLine("### {0}\r\n{1} ###", ee.StackTrace, ee.Message);
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        /// <summary>
        /// PED to Byte Array 변환 (응답용)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static byte[] ToResponByte(PEDInfo item)
        {
            try
            {
                List<byte> byte회신 = new List<byte>();
                //STX
                byte회신.Add(STX);
                //CMD                
                byte bCMD = Convert.ToByte(item.CMD_GBN);
                bCMD += 0x10;
                byte회신.Add(bCMD);
                byte회신.Add(Convert.ToByte(item.LUMIN_GBN));

                switch (item.CMD_GBN)
                {
                    case CMDType.텍스트표출:
                        byte회신.AddRange(BitConverter.GetBytes(0));
                        break;

                    case CMDType.이미지표출:
                        int nData = item.FILE_BINARY.Count;
                        byte[] bData = BitConverter.GetBytes(nData).Reverse().ToArray();
                        int Length = bData.Length;
                        byte[] btLength = BitConverter.GetBytes(Length).Reverse().ToArray();
                        byte회신.AddRange(btLength);
                        byte회신.AddRange(bData);
                        break;

                    case CMDType.파일저장:
                        int nData2 = item.FILE_BINARY.Count;
                        byte[] bData2 = BitConverter.GetBytes(nData2).Reverse().ToArray();
                        int Length2 = bData2.Length;
                        byte[] btLength2 = BitConverter.GetBytes(Length2).Reverse().ToArray();
                        byte회신.AddRange(btLength2);
                        byte회신.AddRange(bData2);
                        break;

                    case CMDType.파일삭제:
                        byte회신.AddRange(BitConverter.GetBytes(0));
                        break;

                    case CMDType.저장파일_화면표출:
                        byte회신.AddRange(BitConverter.GetBytes(0));
                        break;

                    case CMDType.표출중지:
                        int nLength8 = item.FILE_BINARY.Count;
                        byte[] bLength8 = BitConverter.GetBytes(nLength8).Reverse().ToArray();
                        byte회신.AddRange(bLength8);
                        byte회신.Add(0x00);
                        break;
                }
                byte회신.Add(ETX);

                return byte회신.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine("### {0}\r\n{1} ###", ee.StackTrace, ee.Message);
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        //public static FileInfoPacket ToPacket(byte[] arr)
        //{
        //    try
        //    {
        //        MemoryStream ms = new MemoryStream(arr, 4, arr.Length - 4);
        //        BinaryFormatter bf = new BinaryFormatter();
        //        FileInfoPacket pk = (FileInfoPacket)bf.Deserialize(ms);
        //        return pk;
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine("### {0}\r\n{1} ###", ee.StackTrace, ee.Message);
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}
    }
}

