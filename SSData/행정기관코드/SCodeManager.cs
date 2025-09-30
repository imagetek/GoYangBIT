using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;

namespace SSData
{
    public class SCodeManager
    {
        static SCodeManager()
        {
        }

        static SCodeFiles SCodeFile { get; set; }
        public static bool Initialize(string 기본DIR)
        {
            try
            {
                DateTime dt = DateTime.Now;

                //if (SCodeFile == null) SCodeFile = new SCodeFiles();

                //SCodeFile = SCodeFile.Load(기본DIR);

                //Console.WriteLine("## SCodeManager Initialize : {0}ms ##", (DateTime.Now - dt).TotalMilliseconds);
                //Console.WriteLine("## SCode VER : {0} ##", SCodeFile.CRE_YMD);

                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        //public static List<DO_CODE> Select전체시도(bool is말소제외 = true)
        //{
        //    try
        //    {
        //        if (SCodeFile == null) return null;

        //        return SCodeFile.DO_ITEMS.Where(data => data.USE_YN.Equals(is말소제외)).OrderBy(data => data.DO_CD).ToList();
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        //public static List<SGG_CODE> Select시군구By시도(string DO_CD , bool is말소제외 = true)
        //{
        //    try
        //    {
        //        return SCodeFile.SGG_ITEMS.Where(data => data.USE_YN.Equals(is말소제외) && data.DO_CD.Equals(DO_CD)).OrderBy(data => data.SGG_CD).ToList();
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        //public static List<SGG_CODE> Select시군구By코드(string S_CD, bool is말소제외 = true)
        //{
        //    try
        //    {
        //        return SCodeFile.SGG_ITEMS.Where(data => data.USE_YN.Equals(is말소제외) && data.SGG_CD.Equals(S_CD)).OrderBy(data => data.SGG_CD).ToList();
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}
        
        //static bool IsYN광역시군(string SGG_CD)
        //{
        //    try
        //    {
        //        string S_CD = SGG_CD.Substring(0, 4);

        //        List<string> items = CommonProc.Get광역시Code;

        //        return items.Contains(S_CD);
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return false;
        //    }
        //}

        //public static List<HJD_CODE> Select읍면동By시군구(string SGG_CD, bool is전체표시 = true, bool is말소제외 = true, bool is광역모드 = false)
        //{
        //    try
        //    {
        //        List<HJD_CODE> items = new List<HJD_CODE>();
        //        List<HJD_CODE> itemsHJD = new List<HJD_CODE>();

        //        if (is광역모드 == true || IsYN광역시군(SGG_CD) == true)
        //        {
        //            itemsHJD = SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == true & data.BJD_CD.Contains(SGG_CD.Substring(0, 4))).ToList();
        //            if (is말소제외 == false) itemsHJD.AddRange(SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == false & data.BJD_CD.Contains(SGG_CD.Substring(0, 4))).ToList());
        //            itemsHJD = itemsHJD.OrderBy(data => data.BJD_CD).ToList();
        //        }
        //        else
        //        {
        //            itemsHJD = SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == true && data.BJD_CD.Contains(SGG_CD)).ToList();
        //            if (is말소제외 == false) itemsHJD.AddRange(SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == true && data.BJD_CD.Contains(SGG_CD)).ToList());
        //            itemsHJD = itemsHJD.OrderBy(data => data.BJD_CD).ToList();
        //        }

        //        items.Clear();
        //        if (is전체표시 == true) items.Add(new HJD_CODE() { BJD_NM = "전체", BJD_CD = "000" });
        //        foreach (HJD_CODE item in itemsHJD)
        //        {
        //            string GBN_NM = item.BJD_NM.Trim().Last().ToString();
        //            switch (GBN_NM)
        //            {
        //                case "읍":
        //                case "면":
        //                    item.DONG_YN = false;
        //                    items.Add(item);
        //                    break;
        //                case "동":
        //                case "가":
        //                case "로":
        //                    item.DONG_YN = true;
        //                    items.Add(item);
        //                    break;
        //            }
        //        }

        //        return items.OrderBy(data => data.BJD_CD).ToList();
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        //public static List<HJD_CODE> Select읍면동By코드(string CODE, bool is말소제외 = true)
        //{
        //    try
        //    {
        //        string S_CD = "";
        //        //if (CODE.Length > 5 && CODE.Substring(0, 4).Equals(MapDataManager.MAP_SGG_CD.Substring(0, 4)) == true)
        //        {
        //            S_CD = CODE;
        //        }
        //        //else
        //        //{
        //        //    S_CD = string.Format("{0}{1}", MapDataManager.MAP_SGG_CD, CODE);
        //        //}

        //        List<HJD_CODE> items = new List<HJD_CODE>();
        //        List<HJD_CODE> itemsHJD = new List<HJD_CODE>();

        //        itemsHJD = SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == true && data.BJD_CD.Contains(S_CD)).ToList();
        //        if (is말소제외 == false) itemsHJD.AddRange(SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == false && data.BJD_CD.Contains(S_CD)).ToList());
        //        itemsHJD = itemsHJD.OrderBy(data => data.BJD_CD).ToList();

        //        items.Clear();
        //        List<string> itemsBJD = new List<string>();
        //        foreach (HJD_CODE item in itemsHJD)
        //        {
        //            if (itemsBJD.Contains(item.BJD_CD) == true) continue;

        //            string GBN_NM = item.BJD_NM.Trim().Last().ToString();
        //            switch (GBN_NM)
        //            {
        //                case "읍":
        //                case "면":
        //                    item.DISPLAY_NM = string.Format("{0} {1}", item.SGG_NM, item.BJD_NM);
        //                    item.DONG_YN = false;
        //                    break;

        //                case "동":
        //                case "가":
        //                case "로":
        //                    item.DISPLAY_NM = string.Format("{0} {1}", item.SGG_NM, item.BJD_NM);
        //                    item.DONG_YN = true;
        //                    break;
        //            }

        //            if (item.DISPLAY_NM != null && item.DISPLAY_NM.Equals("") == false)
        //            {
        //                itemsBJD.Add(item.BJD_CD);
        //                items.Add(item);
        //            }
        //        }
        //        return items.OrderBy(data => data.BJD_CD).ToList();
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        //public static List<HJD_CODE> Select리By읍면동(string UMD_CD, bool is전체표시 = true, bool is말소제외 = true)
        //{
        //    try
        //    {
        //        List<HJD_CODE> items = new List<HJD_CODE>();
        //        List<HJD_CODE> itemsHJD = new List<HJD_CODE>();
        //        if (UMD_CD.Length > 8)
        //        {
        //            UMD_CD = UMD_CD.Substring(0, 8);
        //        }
        //        itemsHJD = SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == true && data.BJD_CD.Contains(UMD_CD)).ToList();
        //        if (is말소제외 == false) itemsHJD.AddRange(SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == false&& data.BJD_CD.Contains(UMD_CD)).ToList());
        //        itemsHJD = itemsHJD.OrderBy(data => data.BJD_CD).ToList();

        //        items.Clear();
        //        if (is전체표시 == true) items.Add(new HJD_CODE() { BJD_NM = "전체", BJD_CD = "00" });
        //        foreach (HJD_CODE item in itemsHJD)
        //        {
        //            string GBN_NM = item.BJD_NM.Trim().Last().ToString();
        //            switch (GBN_NM)
        //            {
        //                case "리":
        //                    items.Add(item);
        //                    break;
        //            }
        //        }

        //        return items.OrderBy(data => data.BJD_CD).ToList();
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        //public static List<HJD_CODE> Select리By코드(string S_CD, bool is전체표시 = true, bool is말소제외 = true)
        //{
        //    try
        //    {
        //        List<HJD_CODE> items = new List<HJD_CODE>();
        //        List<HJD_CODE> itemsHJD = new List<HJD_CODE>();

        //        itemsHJD = SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == true && data.BJD_CD.Equals(S_CD)).ToList();
        //        if (is말소제외 == false) itemsHJD.AddRange(SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == false && data.BJD_CD.Equals(S_CD)).ToList());
        //        itemsHJD = itemsHJD.OrderBy(data => data.BJD_CD).ToList();
                
        //        items.Clear();
        //        if (is전체표시 == true) items.Add(new HJD_CODE() { BJD_NM = "전체", BJD_CD = "00" });
        //        foreach (HJD_CODE item in itemsHJD)
        //        {
        //            string GBN_NM = item.BJD_NM.Trim().Last().ToString();
        //            switch (GBN_NM)
        //            {
        //                case "리":
        //                    items.Add(item);
        //                    break;
        //            }
        //        }

        //        return items.OrderBy(data => data.BJD_CD).ToList();
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        //public static List<HJD_CODE> Select리By시도(string S_CD)
        //{
        //    try
        //    {
        //        List<HJD_CODE> items = new List<HJD_CODE>();
        //        List<HJD_CODE> itemsHJD = new List<HJD_CODE>();

        //        itemsHJD = SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == true && data.BJD_CD.Substring(0,2).Equals(S_CD)).ToList();
        //        //if (is말소제외 == false) itemsHJD.AddRange(SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == false && data.BJD_CD.Equals(S_CD)).ToList());
        //        itemsHJD = itemsHJD.OrderBy(data => data.BJD_CD).ToList();

        //        items.Clear();
        //        foreach (HJD_CODE item in itemsHJD)
        //        {
        //            string GBN_NM = item.BJD_NM.Trim().Last().ToString();
        //            switch (GBN_NM)
        //            {
        //                case "리":
        //                    items.Add(item);
        //                    break;
        //            }
        //        }

        //        return items.OrderBy(data => data.BJD_CD).ToList();
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        //public static List<HJD_CODE> Select읍면동By시도(string CODE)
        //{
        //    try
        //    {
        //        List<HJD_CODE> items = new List<HJD_CODE>();
        //        List<HJD_CODE> itemsHJD = new List<HJD_CODE>();

        //        itemsHJD = SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == true && data.BJD_CD.Substring(0,2).Equals(CODE)== true).ToList();
        //        itemsHJD = itemsHJD.OrderBy(data => data.BJD_CD).ToList();

        //        items.Clear();
        //        List<string> itemsBJD = new List<string>();
        //        foreach (HJD_CODE item in itemsHJD)
        //        {
        //            if (itemsBJD.Contains(item.BJD_CD) == true) continue;

        //            string GBN_NM = item.BJD_NM.Trim().Last().ToString();
        //            switch (GBN_NM)
        //            {
        //                case "읍":
        //                case "면":
        //                    item.DISPLAY_NM = string.Format("{0} {1}", item.SGG_NM, item.BJD_NM);
        //                    item.DONG_YN = false;
        //                    break;

        //                case "동":
        //                case "가":
        //                case "로":
        //                    item.DISPLAY_NM = string.Format("{0} {1}", item.SGG_NM, item.BJD_NM);
        //                    item.DONG_YN = true;
        //                    break;
        //            }

        //            if (item.DISPLAY_NM != null && item.DISPLAY_NM.Equals("") == false)
        //            {
        //                itemsBJD.Add(item.BJD_CD);
        //                items.Add(item);
        //            }
        //        }
        //        return items.OrderBy(data => data.BJD_CD).ToList();
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        //public static List<HJD_CODE> Select법정동By코드(string S_NM, bool is말소제외 = true)
        //{
        //    try
        //    {
        //        string S_CD = "";
        //       // if (S_NM.Length > 5 && S_NM.Substring(0, 4).Equals(MapDataManager.MAP_SGG_CD.Substring(0, 4)) == true)
        //        {
        //            S_CD = S_NM;
        //        }
        //        //else
        //        //{
        //        //    S_CD = string.Format("{0}{1}", MapDataManager.MAP_SGG_CD, S_NM);
        //        //}

        //        List<HJD_CODE> items = new List<HJD_CODE>();
        //        List<HJD_CODE> itemsHJD = new List<HJD_CODE>();

        //        itemsHJD = SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == true && data.BJD_CD.Equals(S_CD)).ToList();
        //        if (is말소제외 == false) itemsHJD.AddRange(SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == false && data.BJD_CD.Equals(S_CD)).ToList());
        //        itemsHJD = itemsHJD.OrderBy(data => data.BJD_CD).ToList();

        //        items.Clear();
        //        List<string> itemsBJD = new List<string>();
        //        foreach (HJD_CODE item in itemsHJD)
        //        {
        //            if (itemsBJD.Contains(item.BJD_CD) == true) continue;

        //            string GBN_NM = item.BJD_NM.Trim().Last().ToString();
        //            switch (GBN_NM)
        //            {
        //                case "읍":
        //                case "면":
        //                case "동":
        //                case "가":
        //                case "로":
        //                    item.DISPLAY_NM = string.Format("{0} {1}", item.SGG_NM, item.BJD_NM);
        //                    break;

        //                case "리":
        //                    item.DISPLAY_NM = string.Format("{0} {1} {2}", item.SGG_NM, item.HJD_NM, item.BJD_NM);
        //                    break;
        //            }

        //            if (item.DISPLAY_NM.Equals("") == false)
        //            {
        //                itemsBJD.Add(item.BJD_CD);
        //                items.Add(item);
        //            }
        //        }
        //        return items.OrderBy(data => data.BJD_CD).ToList();
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        //public static List<HJD_CODE> Select법정동By문자(string S_NM,  bool is말소제외 = true , bool is타시군검색 = false)
        //{
        //    try
        //    {
        //        List<HJD_CODE> items = new List<HJD_CODE>();
        //        List<HJD_CODE> itemsHJD = new List<HJD_CODE>();

        //        itemsHJD = SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == true && (data.BJD_NM.Contains(S_NM) || data.HJD_NM.Contains(S_NM))).ToList();
        //        if (is말소제외 == false) itemsHJD.AddRange(SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == false && (data.BJD_NM.Contains(S_NM) || data.HJD_NM.Contains(S_NM))).ToList());
        //        itemsHJD = itemsHJD.OrderBy(data => data.BJD_CD).ToList();
        //        items.Clear();
        //        List<string> itemsBJD = new List<string>();
        //        foreach (HJD_CODE item in itemsHJD)
        //        {
        //            if (itemsBJD.Contains(item.BJD_CD) == true) continue;
        //            //if (is타시군검색 == false && item.BJD_CD.Substring(0, 4).Equals(MapDataManager.MAP_SGG_CD.Substring(0, 4)) == false) continue;

        //            string GBN_NM = item.BJD_NM.Trim().Last().ToString();
        //            switch (GBN_NM)
        //            {
        //                case "읍":
        //                case "면":
        //                case "동":
        //                case "가":
        //                case "로":
        //                    item.DISPLAY_NM = string.Format("{0} {1}", item.SGG_NM, item.BJD_NM);
        //                    break;

        //                case "리":
        //                    item.DISPLAY_NM = string.Format("{0} {1} {2}", item.SGG_NM, item.HJD_NM, item.BJD_NM);
        //                    break;
        //            }

        //            if (item.DISPLAY_NM.Equals("") == false)
        //            {
        //                itemsBJD.Add(item.BJD_CD);
        //                items.Add(item);
        //            }
        //        }
        //        return items.OrderBy(data => data.BJD_CD).ToList();
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        //public static List<HJD_CODE> Select법정동By문자열(string UMD_NM, string RI_NM, bool is말소제외 = true, bool is타시군검색 = false)
        //{
        //    try
        //    {
        //        List<HJD_CODE> items = new List<HJD_CODE>();
        //        List<HJD_CODE> itemsHJD = new List<HJD_CODE>();

        //        itemsHJD = SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == true && (data.BJD_NM.Contains(RI_NM) || data.HJD_NM.Contains(RI_NM))).ToList();
        //        if (is말소제외 == false) itemsHJD.AddRange(SCodeFile.HJD_ITEMS.Where(data => data.USE_YN == false && (data.BJD_NM.Contains(RI_NM) || data.HJD_NM.Contains(RI_NM))).ToList());

        //        itemsHJD = itemsHJD.Where(data => data.HJD_NM.Equals(UMD_NM)==true).ToList();
        //        itemsHJD = itemsHJD.OrderBy(data => data.BJD_CD).ToList();
                
        //        items.Clear();
        //        List<string> itemsBJD = new List<string>();
        //        foreach (HJD_CODE item in itemsHJD)
        //        {
        //            if (itemsBJD.Contains(item.BJD_CD) == true) continue;
        //            //if (is타시군검색 == false && item.BJD_CD.Substring(0, 4).Equals(MapDataManager.MAP_SGG_CD.Substring(0, 4)) == false) continue;

        //            string GBN_NM = item.BJD_NM.Trim().Last().ToString();
        //            switch (GBN_NM)
        //            {
        //                case "읍":
        //                case "면":
        //                case "동":
        //                case "가":
        //                case "로":
        //                    item.DISPLAY_NM = string.Format("{0} {1}", item.SGG_NM, item.BJD_NM);
        //                    break;

        //                case "리":
        //                    item.DISPLAY_NM = string.Format("{0} {1} {2}", item.SGG_NM, item.HJD_NM, item.BJD_NM);
        //                    break;
        //            }

        //            if (item.DISPLAY_NM.Equals("") == false)
        //            {
        //                itemsBJD.Add(item.BJD_CD);
        //                items.Add(item);
        //            }
        //        }
        //        return items.OrderBy(data => data.BJD_CD).ToList();
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        //public static HJD_CODE CONVERT_DISPLAY_HJD_CODE(HJD_CODE item)
        //{
        //    try
        //    {
        //        string RI_LAST = item.BJD_NM.Last().ToString();
        //        switch (RI_LAST)
        //        {
        //            case "읍":
        //            case "면":
        //            case "동":
        //            case "가":
        //            case "로":
        //                item.DISPLAY_NM = string.Format("{0} {1}", item.SGG_NM, item.BJD_NM);
        //                break;

        //            case "리":
        //                item.DISPLAY_NM = string.Format("{0} {1} {2}", item.SGG_NM, item.HJD_NM, item.BJD_NM);
        //                break;

        //            default:
        //                item.DISPLAY_NM = "";
        //                break;
        //        }

        //        return item;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return item;
        //    }
        //}

        //public static string Get지번주소ByLAND_CD(string pnu)
        //{
        //    try
        //    {
        //        //10자리 컨버터코드 (시군구)/(읍면동)/(리)

        //        //19자리 PNU코드
        //        //12345 678 90 1 2345 6789
        //        //47130 253 25 1 0000 0000
        //        //시군구 읍면동 리 대지(1)/임야(2)  주번 부번

        //        if (!pnu.Length.Equals(19)) return "";
        //        long pnuNumber = Convert.ToInt64(pnu); //순자체크..
        //        string addr = "";

        //        string doCode = pnu.Substring(0, 2); //도
        //        string sggCode = pnu.Substring(0, 5); //시군구
        //        string emdCode = pnu.Substring(5, 3);//읍면동
        //        string riCode = pnu.Substring(8, 2); //리
        //        string mt = (pnu.Substring(10, 1)); //산여부
        //        string mainStr = (pnu.Substring(11, 4));//주번
        //        string subStr = (pnu.Substring(15, 4)); //부번
        //        int mm = Convert.ToInt32("0");
        //        int mainNum = Convert.ToInt32(mainStr);
        //        int subNum = Convert.ToInt32(subStr);

        //        StringBuilder sbBunJi = new StringBuilder();
        //        if (mt.Equals("2")) sbBunJi.Append("산");
        //        if (mainNum >= 0 && subNum > 0)
        //            sbBunJi.Append(string.Format("{0}-{1}", mainNum, subNum));
        //        else if (mainNum > 0 && subNum == 0)
        //            //메인번지만
        //            sbBunJi.Append(string.Format("{0}", mainNum));
        //        else
        //            sbBunJi.Append("0");

        //        string bjdcode = sggCode + emdCode + riCode; //10자리

        //        //경상북도 안동시 송하동 
        //        //서울특별시 관악구 봉천동 

        //        List<HJD_CODE> hjdcodeLists = SCodeFile.HJD_ITEMS.Where(item => item.BJD_CD.Equals(bjdcode) && item.USE_YN == true).ToList();

        //        if (hjdcodeLists.Count <= 0) return "";

        //        HJD_CODE hcode = hjdcodeLists.First();

        //        string RI_LAST = hcode.BJD_NM.Last().ToString();

        //        switch (RI_LAST)
        //        {
        //            case "읍":
        //            case "면":
        //            case "동":
        //            case "가":
        //            case "로":
        //                addr = string.Format("{0} {1} {2}", hcode.DO_NM, hcode.SGG_NM, hcode.BJD_NM);
        //                break;
        //            case "리":
        //                addr = string.Format("{0} {1} {2} {3}", hcode.DO_NM, hcode.SGG_NM, hcode.HJD_NM, hcode.BJD_NM);
        //                break;
        //        }

        //        addr = string.Format("{0} {1}", addr, sbBunJi.ToString());

        //        return addr;
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.Message + ex.StackTrace);
        //        return "";
        //    }
        //}

        //public static string Get도로명주소ByLAND_CD(string pnu)
        //{
        //    try
        //    {
        //        if (!pnu.Length.Equals(19)) return "";

        //        string roadName = "";
        //        int iJibun = Convert.ToInt32(pnu.Substring(11, 4));
        //        int iHo = Convert.ToInt32(pnu.Substring(15, 4));
        //        string selMngCode = "";
        //        List<JusoData> roadInfos = JusoManager.GetRoads();
        //        List<string> dongKinds = new List<string>() { "동", "가" };
        //        foreach (JusoData rd in roadInfos)
        //        {
        //            if (rd.BLDG_BUBN.Trim().Equals("")) rd.BLDG_BUBN = "0";
        //            if (rd.BJD_CD.Equals(pnu.Substring(0, 10)) && Convert.ToInt32(rd.LAND_BOBN).Equals(iJibun) && Convert.ToInt32(rd.LAND_BUBN).Equals(iHo))
        //            {
        //                selMngCode = rd.BD_MST_SN;
        //                roadName = string.Format("{0} {1}{2} {3} {4}{5}",
        //                    rd.DO_NM,
        //                    rd.SGG_NM,
        //                    dongKinds.Contains(rd.UMD_NM.Substring(rd.UMD_NM.Length - 1)) ? "" : " " + rd.UMD_NM,
        //                    rd.RN_NM,
        //                    rd.BLDG_BOBN,
        //                    rd.BLDG_BUBN.Trim().Equals("0") ? "" : string.Format("-{0}", rd.BLDG_BUBN));
        //                break;
        //            }
        //        }

        //        return (roadName.Trim().Equals("") ? "도로명주소가 존재하지 않습니다." : roadName);
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
        //        return "";
        //    }
        //}

        //public static List<string> Convert토지코드들ByNM(string str)
        //{
        //   try
        //    {
        //        List<string> m변환전 = str.Split(',').ToList();
        //        List<string> items = new List<string>();
        //        List<string> items변환전 = Convert변환전주소(m변환전);
        //        foreach (string mstr in items변환전)
        //        {                    
        //            string pnu = Get토지코드ByNM(mstr);
        //            if (pnu.Equals("") == false)
        //            {
        //                items.Add(pnu);
        //            }
        //        }
        //        return items;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        //public static List<string> Convert변환전주소(List<string> items변환전)
        //{
        //    try
        //    {
        //        if (items변환전.Count == 1) return items변환전;

        //        List<string> items = new List<string>();
        //        string 기준주소 = "";
        //        foreach (string item변환전 in items변환전)
        //        {
        //            List<string> 주소변환 = item변환전.Split(' ').ToList();
        //            if (주소변환.Count > 1)
        //            {
        //                bool isYN기준주소 = false;
        //                foreach (string m주소분리 in 주소변환)
        //                {
        //                    if (m주소분리.Equals("") == true) continue;
        //                    string GBN_NM = m주소분리.Last().ToString();
        //                    switch (GBN_NM)
        //                    {
        //                        case "리":
        //                        case "동":
        //                            isYN기준주소 = true;
        //                            int 위치No = 주소변환.IndexOf(m주소분리);
        //                            for (int i = 0; i <= 위치No; i++)
        //                            {
        //                                if (기준주소.Equals("") == true)
        //                                {
        //                                    기준주소 = 주소변환[i];
        //                                }
        //                                else
        //                                {
        //                                    기준주소 += string.Format(" {0}", 주소변환[i]);
        //                                }
        //                            }
        //                            break;
        //                    }
        //                }
        //                if (isYN기준주소 == true)
        //                {
        //                    items.Add(item변환전);
        //                }
        //                else
        //                {
        //                    string 주소2 = string.Format("{0} {1}", 기준주소, item변환전);
        //                    주소2 = 주소2.Replace("  ", " ");
        //                    items.Add(주소2);
        //                }
        //            }
        //            else
        //            {
        //                items.Add(item변환전);
        //            }
        //        }

        //        return items;
                
        //        //if (CHK_STR.Count > 1)
        //        //{
        //        //    foreach (string CHK in CHK_STR)
        //        //    {
        //        //        string GBN = CHK.Last().ToString();
        //        //        if (GBN.Equals("리") == true || GBN.Equals("동") == true)
        //        //        {
        //        //            int idxNo = CHK_STR.IndexOf(CHK);
        //        //            for (int i = 0; i < CHK_STR.Count; i++)
        //        //            {
        //        //           //     BASE_STR += CHK_STR[i];
        //        //            }
        //        //        }
        //        //    }
        //        //}

        //        //return null;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        //public static string Get토지코드ByNM(string str)
        //{
        //    try
        //    {
        //        List<string> m변환전 = str.Split(' ').ToList();
        //        if (m변환전.Count == 0) return "";

        //        Dictionary<string, string> items = new Dictionary<string, string>();
        //        foreach (string mStr in m변환전)
        //        {
        //            if (mStr.Equals("") == true) continue;

        //            string LAST_CHAR = mStr.Last().ToString();
        //            switch (LAST_CHAR)
        //            {
        //                case "도": break;
        //                case "울": break; //서울
        //                case "남": break; //전남 경남...
        //                case "북": break; //전북 경북...
        //                case "원": break; //강원
        //                case "기": break; //경기
        //                case "전": break; //대전
        //                case "주": break; //광주 제주
        //                case "시":
        //                case "군":
        //                case "구":
        //                    if (items.ContainsKey("SGG") == false)
        //                    {
        //                        items.Add("SGG", mStr);
        //                    }
        //                    break;

        //                case "읍":
        //                case "면":
        //                    if (items.ContainsKey("UM") == false)
        //                    {
        //                        items.Add("UM", mStr);
        //                    }
        //                    break;
        //                case "동":
        //                case "가":
        //                case "로":
        //                    if (items.ContainsKey("DONG") == false)
        //                    {
        //                        items.Add("DONG", mStr);
        //                    }
        //                    break;

        //                case "리":
        //                    if (items.ContainsKey("RI") == false)
        //                    {
        //                        items.Add("RI", mStr);
        //                    }
        //                    break;

        //                case "산":
        //                    if (items.ContainsKey("SAN") == false)
        //                    {
        //                        items.Add("SAN", mStr);
        //                    }
        //                    break;

        //                default:
        //                    if (items.ContainsKey("JIBN") == false)
        //                    {
        //                        items.Add("JIBN", mStr);
        //                    }
        //                    break;
        //            }
        //        }

        //        //////////////////////////////////////////////////////////////////////////////                
        //        string LAND_CD = "";
        //        if (items.ContainsKey("JIBN") == true && items.ContainsKey("DONG") == true)
        //         {
        //            List<HJD_CODE> itemsHJD = Select법정동By문자(items["DONG"],true, true);
        //            if (itemsHJD != null && itemsHJD.Count > 0)
        //            {
        //                LAND_CD += itemsHJD.First().BJD_CD;
        //                if (items.ContainsKey("SAN") == true)
        //                {
        //                    LAND_CD += "2";
        //                }

        //                string JIBN_NM = items["JIBN"];
        //                if (items.ContainsKey("SAN") == false)
        //                {
        //                    if (JIBN_NM.Contains("산") == true)
        //                    {
        //                        JIBN_NM = JIBN_NM.Replace("산", "");
        //                        LAND_CD += "2";
        //                    }
        //                    else
        //                    {
        //                        LAND_CD += "1";
        //                    }
        //                }
        //                else
        //                {
        //                    JIBN_NM = JIBN_NM.Replace("산", "");
        //                }

        //                JIBN_NM = JIBN_NM.Replace("번지", "");
        //                string[] JIBN_STR = JIBN_NM.Split('-');
        //                if (JIBN_STR.Length == 2)
        //                {
        //                    int BOBN = Convert.ToInt32(JIBN_STR[0]);
        //                    int BUBN = Convert.ToInt32(JIBN_STR[1]);

        //                    LAND_CD += string.Format("{0:d4}{1:d4}", BOBN, BUBN);
        //                }
        //                else if (JIBN_STR.Length == 1)
        //                {
        //                    int BOBN = Convert.ToInt32(JIBN_STR[0]);

        //                    LAND_CD += string.Format("{0:d4}0000", BOBN);
        //                }
        //            }
        //        }
        //        else if (items.ContainsKey("JIBN") == true && items.ContainsKey("UM") == true && items.ContainsKey("RI") == true)
        //        {
        //            List<HJD_CODE> itemsHJD = Select법정동By문자열(items["UM"], items["RI"], true, true);
        //            if (itemsHJD != null && itemsHJD.Count > 0)
        //            {
        //                LAND_CD += itemsHJD.First().BJD_CD;
        //                if (items.ContainsKey("SAN") == true)
        //                {
        //                    LAND_CD += "2";
        //                }
                                                    
        //                string JIBN_NM = items["JIBN"];
        //                if (items.ContainsKey("SAN") == false)
        //                {
        //                    if (JIBN_NM.Contains("산") == true)
        //                    {
        //                        JIBN_NM = JIBN_NM.Replace("산", "");
        //                        LAND_CD += "2";
        //                    }
        //                    else
        //                    {
        //                        LAND_CD += "1";
        //                    }
        //                }
        //                else
        //                {
        //                    JIBN_NM = JIBN_NM.Replace("산", "");
        //                }

        //                JIBN_NM = JIBN_NM.Replace("번지", "");                         
        //                string[] JIBN_STR = JIBN_NM.Split('-');
        //                if (JIBN_STR.Length == 2)
        //                {
        //                    int BOBN = Convert.ToInt32(JIBN_STR[0]);
        //                    int BUBN = Convert.ToInt32(JIBN_STR[1]);

        //                    LAND_CD += string.Format("{0:d4}{1:d4}", BOBN, BUBN);
        //                }
        //                else if (JIBN_STR.Length == 1)
        //                {
        //                    int BOBN = Convert.ToInt32(JIBN_STR[0]);

        //                    LAND_CD += string.Format("{0:d4}0000", BOBN);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("[주소변환] 필수요소가 존재하지 않습니다. : {0}", str);
        //        }

        //        return LAND_CD;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return "";
        //    }
        //}
    }
}
