using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SSWebcamPlayer
{
    /// <summary>
    /// 프로그램내의 기본설정
    /// </summary>
    public class ConfigInfo
    {
        /// <summary>
        /// 프로그램 종류
        /// </summary>
        public int PGM_GBN { get; set; }
        //선택 지자체 정보
        public string SGG_CD { get; set; }
        public string SGG_NM { get; set; }
        /// <summary>
        /// 볼륨
        /// </summary>
        public int VOLUME { get; set; }
        /// <summary>
        /// 대기시간 , 진입시간
        /// </summary>
        public int WAIT_TIME { get; set; }
        public int FONTS_SZ { get; set; }
        public string FONTS_NM { get; set; }
        /// <summary>
        /// 시작지점 X (경도) 
        /// </summary>
        public double INIT_X { get; set; }
        /// <summary>
        /// 시작지점 Y (위도) 
        /// </summary>
        public double INIT_Y { get; set; }
        /// <summary>
        /// 반경거리(단위 미터)
        /// </summary>
        public int RADIUS { get; set; }
        public int ANIME_DELAY { get; set; }
        //--> 프로그램 기본설정 종료


        public bool USE_WEATHER_RSS { get; set; }
        /// <summary>
        /// 날씨 RSS ///http://www.kma.go.kr/weather/lifenindustry/sevice_rss.jsp 에서 검색
        /// </summary>
        public string WEATHER_URL { get; set; }
        public bool USE_NEWS_RSS { get; set; }
        /// <summary>
        /// 뉴스 RSS
        /// </summary>
        public string NEWS_URL { get; set; }

        /// <summary>
        /// 미세먼지 관련 측정소명
        /// </summary>
        public string AIR_PLACE_NM { get; set; }
        public bool USE_AIR_PLACE { get; set; }        
    }
}
