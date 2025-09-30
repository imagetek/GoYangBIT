using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;

namespace SSData
{
    public enum 기초코드Type
    {
        NONE = -1,
        버전관리 = 0,
        사용자구분 = 1, //기본
        인증구분,
        DB종류,
        FTP구분,
        FONTS종류,
        연결종류,
        프로그램종류,
        환경보드종류,

        LCD화면종류 = 21,

        잠시후도착조건 = 31,
        BIT정보정렬방식,
        //LED화면종류,
    }

    public class BC_CODE
    {
        public int CD_GBN_NO { get; set; }
        public string CD_GBN_NM { get; set; }
        public string CODE { get; set; }
        public int nCODE
        {
            get
            {
                int data = 0;
                if (CODE.Equals("") == false && CommonUtils.IsNumeric(CODE) == true)
                {
                    data = Convert.ToInt32(CODE);
                }
                return data;
            }
        }
        public string NAME { get; set; }
        public string S_NM { get; set; }
        public int DISP_YN { get; set; }
        public string DISP_YN_DISP
        {
            get
            {
                string DISP = "미표시";
                if (DISP_YN.Equals(1) == true)
                {
                    DISP = "표시";
                }
                return DISP;
            }
        }
        public int USE_YN { get; set; }
        public string USE_YN_DISP
        {
            get
            {
                string DISP = "미사용";
                if (USE_YN.Equals(1) == true)
                {
                    DISP = "사용";
                }
                return DISP;
            }
        }

        //주정차관리시스템용
        //public enum 기초구분Type
        //{
        //    NONE = -1,
        //    버전관리 = 0,
        //    사용자구분 = 1, //기본
        //    인증구분,
        //    CLIENT구분 = 11, //LPR기본
        //    방향구분,
        //    번호판색상구분,
        //    인식결과구분,

        //    차량용도 = 21,
        //    차종기호,
        //    차량종류,
        //    차량분류, //국토교통부 차종분류조사 (AVC)

        //    법령구분 = 31,
        //    단속구분,

        //    로그인결과 = 41, //40번대이후 오류관련        
        //}
    }
}
