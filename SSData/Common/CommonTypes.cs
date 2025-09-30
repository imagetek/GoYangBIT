 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSData
{
    public enum 프로그램Type
    {
        NONE,
        AGENT,
        BIT,
        SOUND,
        UPDATER,
        WEBCAM,
        CONFIG_M =11,
    }

    public enum 프로그램메뉴Type
    {
        NONE = -1,
        DEFAULT = 0,
        HOME = 1,
        BIT정보,
        환경보드,
        웹캠,
        BIT설정 = 8,
        환경설정 = 9,
        메뉴표시 = 90,
        메뉴숨기기 = 91,
        이전으로 = 99,
    }

    public enum DISPLAY구분
    {
        NONE = -1,
        DEFAULT = 0,
        LCD가로형 = 1,
        LCD세로형,
        LCD가로형HD,
        LCD세로형HD,
        LED3X8 = 11,
        LED3X6,
        LED4X12 = 21,
    }

    public enum LCD화면Type
    {
        NONE = -1,
        DEFAULT = 0,
        BIS정보 = 1,
        BIS정보미존재,
        네트워크오류,
        긴급메세지,
        시스템에러,
    }

    public enum LED화면Type
    {
        NONE = -1,
        DEFAULT = 0,
        초기화면 = 1,
        네트워크오류,
        시스템오류,
        도착정보없슴,
        메세지,
        미세먼지,

        도착정보 = 10,

        BIS_노선번호 = 11,
        BIS_버스현위치,
        BIS_도착예정시간,
        BIS_남은좌석수,
        BIS_행선지,
        BIS_버스운행상태,

        BIS_곧도착정보 = 20,
    }
}