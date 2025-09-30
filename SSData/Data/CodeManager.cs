using System;
using System.Collections.Generic;
using System.Linq;
using SSCommonNET;

namespace SSData
{
    public class CodeManager
    {
        public CodeManager()
        {
            try
            {

            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        public static void Initialize()
        {
            try
            {  
                ITEMS = InitializeCode();
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        //#region DB 구분

        private static List<BC_CODE> InitializeCode()
        {
            try
            {
                List<BC_CODE> datas =
				[
					new BC_CODE { CD_GBN_NO = 1, CD_GBN_NM = "사용자구분", CODE = "0", NAME = "미등록", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 1, CD_GBN_NM = "사용자구분", CODE = "1", NAME = "일반사용자", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 1, CD_GBN_NM = "사용자구분", CODE = "2", NAME = "관리자", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 1, CD_GBN_NM = "사용자구분", CODE = "5", NAME = "임시사용자", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 2, CD_GBN_NM = "인증구분", CODE = "0", NAME = "미인증", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 2, CD_GBN_NM = "인증구분", CODE = "1", NAME = "인증", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 3, CD_GBN_NM = "DB종류", CODE = "0", NAME = "미사용", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 3, CD_GBN_NM = "DB종류", CODE = "1", NAME = "SQLite", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					//new BC_CODE { CD_GBN_NO = 3, CD_GBN_NM = "DB종류", CODE = "2", NAME = "Firebird", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					//new BC_CODE { CD_GBN_NO = 3, CD_GBN_NM = "DB종류", CODE = "3", NAME = "MSSQL", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 3, CD_GBN_NM = "DB종류", CODE = "4", NAME = "MySQL", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 4, CD_GBN_NM = "FTP구분", CODE = "0", NAME = "미사용", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					//new BC_CODE { CD_GBN_NO = 4, CD_GBN_NM = "FTP구분", CODE = "1", NAME = "FTP", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					//new BC_CODE { CD_GBN_NO = 4, CD_GBN_NM = "FTP구분", CODE = "2", NAME = "SFTP", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 5, CD_GBN_NM = "FONTS종류", CODE = "0", NAME = "굴림", S_NM = "gulim", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 5, CD_GBN_NM = "FONTS종류", CODE = "1", NAME = "KoPub돋음 체", S_NM = "KoPubDotum", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 5, CD_GBN_NM = "FONTS종류", CODE = "2", NAME = "나눔바른고딕", S_NM = "NanumBarunGothic", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 5, CD_GBN_NM = "FONTS종류", CODE = "3", NAME = "맑은고딕", S_NM = "malgun", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 6, CD_GBN_NM = "연결종류", CODE = "00", NAME = "미연결", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					//new BC_CODE { CD_GBN_NO = 6, CD_GBN_NM = "연결종류", CODE = "11", NAME = "서울시OpenAPI", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					//new BC_CODE { CD_GBN_NO = 6, CD_GBN_NM = "연결종류", CODE = "12", NAME = "경기도OpenAPI", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					//new BC_CODE { CD_GBN_NO = 6, CD_GBN_NM = "연결종류", CODE = "13", NAME = "인천시OpenAPI", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					//new BC_CODE { CD_GBN_NO = 6, CD_GBN_NM = "연결종류", CODE = "14", NAME = "타고OpenAPI", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					//new BC_CODE { CD_GBN_NO = 6, CD_GBN_NM = "연결종류", CODE = "21", NAME = "파주시BIS", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 7, CD_GBN_NM = "프로그램종류", CODE = "00", NAME = "신성엔에스텍", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					//new BC_CODE { CD_GBN_NO = 7, CD_GBN_NM = "프로그램종류", CODE = "01", NAME = "파주시", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 8, CD_GBN_NM = "환경보드종류", CODE = "0", NAME = "신성환경보드신형", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 8, CD_GBN_NM = "환경보드종류", CODE = "1", NAME = "신성환경보드구형", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 21, CD_GBN_NM = "LCD화면종류", CODE = "00", NAME = "", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 21, CD_GBN_NM = "LCD화면종류", CODE = "01", NAME = "BIS정보", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 21, CD_GBN_NM = "LCD화면종류", CODE = "02", NAME = "BIS정보미존재", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 21, CD_GBN_NM = "LCD화면종류", CODE = "03", NAME = "네트워크오류", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 21, CD_GBN_NM = "LCD화면종류", CODE = "04", NAME = "긴급메세지", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 21, CD_GBN_NM = "LCD화면종류", CODE = "05", NAME = "시스템에러", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 31, CD_GBN_NM = "잠시후도착조건", CODE = "0", NAME = "시간 기준", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 31, CD_GBN_NM = "잠시후도착조건", CODE = "1", NAME = "남은정류장 기준", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 31, CD_GBN_NM = "잠시후도착조건", CODE = "2", NAME = "시간/정류장 모두 만족", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 31, CD_GBN_NM = "잠시후도착조건", CODE = "3", NAME = "시간/정류장 둘중 한 조건 만족", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 32, CD_GBN_NM = "BIT정보정렬방식", CODE = "0", NAME = "시간", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 32, CD_GBN_NM = "BIT정보정렬방식", CODE = "1", NAME = "도착예정시간", S_NM = "", DISP_YN = 1, USE_YN = 1 },
					new BC_CODE { CD_GBN_NO = 32, CD_GBN_NM = "BIT정보정렬방식", CODE = "2", NAME = "시간 및 도착예정시간", S_NM = "", DISP_YN = 1, USE_YN = 1 },
				];

                return datas;
            }
            catch (Exception ee)
            {
                Console.WriteLine(string.Format("# {0}\r\n{1} #", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("# {0}\r\n{1} #", ee.StackTrace, ee.Message));
                return null;
            }
        }

        static List<BC_CODE> ITEMS = null;
        public static List<BC_CODE> Get기초코드(기초코드Type item, bool 코드0제거YN = false, bool 표시YN = true, bool 사용YN = true)
        {
            try
            {
                if (ITEMS == null || ITEMS.Count == 0)
                {
                    Initialize();
                }

                if (ITEMS == null || ITEMS.Count == 0) return null;
                int GBN = Convert.ToInt32(item);
                List<BC_CODE> items = ITEMS.Where(data => data.CD_GBN_NO.Equals(GBN) == true).ToList();
                if (items != null && items.Count > 0)
                {
                    if (사용YN == true)
                    {
                        items = items.Where(data => data.USE_YN.Equals(0) == false).ToList();
                    }

                    if (표시YN == true)
                    {
                        items = items.Where(data => data.DISP_YN.Equals(0) == false).ToList();
                    }

                    if (코드0제거YN == true)
                    {
                        items = items.Where(data => data.nCODE.Equals(0) == false).ToList();
                    }
                    items = items.OrderBy(data => data.CODE).ToList();
                }

                return items;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }
        public static List<BC_CODE> GetDISPLAY구분()
        {
            List<BC_CODE> datas =
			[
				new BC_CODE { CODE = "0", NAME = "미선택", S_NM  = ""},
				//datas.Add(new BC_CODE { CODE = "3", NAME = "LCD가로형(HD)", S_NM = "LCD" });
				new BC_CODE { CODE = "4", NAME = "LCD세로형(HD)", S_NM = "LCD" },
			];

			

            return datas;
        }

        //public static List<BC_CODE> GetLED화면구분()
        //{
        //    List<BC_CODE> datas = new List<BC_CODE>();

        //    datas.Add(new BC_CODE { CODE = "0", NAME = "미선택" });
        //    datas.Add(new BC_CODE { CODE = "1", NAME = "초기화면" });
        //    datas.Add(new BC_CODE { CODE = "2", NAME = "네트워크오류" });
        //    datas.Add(new BC_CODE { CODE = "3", NAME = "시스템오류" });
        //    datas.Add(new BC_CODE { CODE = "4", NAME = "도착정보없슴" });
        //    datas.Add(new BC_CODE { CODE = "5", NAME = "메세지" });
        //    datas.Add(new BC_CODE { CODE = "6", NAME = "미세먼지" });

        //    datas.Add(new BC_CODE { CODE = "10", NAME = "도착정보" });
        //    datas.Add(new BC_CODE { CODE = "11", NAME = "BIS_노선번호" });
        //    datas.Add(new BC_CODE { CODE = "12", NAME = "BIS_버스현위치" });
        //    datas.Add(new BC_CODE { CODE = "13", NAME = "BIS_도착예정시간" });
        //    datas.Add(new BC_CODE { CODE = "14", NAME = "BIS_남은좌석수" });
        //    datas.Add(new BC_CODE { CODE = "15", NAME = "BIS_행선지" });
        //    datas.Add(new BC_CODE { CODE = "16", NAME = "BIS_버스운행상태" });

        //    datas.Add(new BC_CODE { CODE = "20", NAME = "BIS_곧도착정보" });

        //    return datas;
        //}

    }
}
