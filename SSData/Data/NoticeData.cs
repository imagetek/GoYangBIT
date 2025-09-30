using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSData
{
    /// <summary>
    /// 미디어정보
    /// </summary>
    [Serializable]
    public class NoticeData
    {
        public bool SELECT_YN { get; set; }
        public int SEQ_NO { get; set; }        
        /// <summary>
        /// 미디어구분 1:이미지,2:동영상,3:공지사항,4:홍보텍스트(LED)전용
        /// </summary>
        public int M_GBN { get; set; }
        /// <summary>
        /// 순번
        /// </summary>
        public int IPC_NO { get; set; }
        public bool USE_YN { get; set; }
        public string USE_YN_DISP
        {
            get
            {
                string item = "미사용";
                if (USE_YN == true) item = "사용";
                return item;
            }
        }
        /// <summary>
        /// 파일명
        /// </summary>
        public string FILE_NM { get; set; }
        /// <summary>
        /// 전체경로
        /// </summary>
        public string FILE_URL { get; set; }
        public string FILE_EXT { get; set; }
        /// <summary>
        /// 표출 텍스트 : 최대6중
        /// </summary>
        public string DISP_TEXT{ get; set; }
        /// <summary>
        /// 볼륨
        /// </summary>
        public int VOLUME { get; set; }
        /// <summary>
        /// 지속시간 단위: 초(이미지일경우)
        /// </summary>
        public int DURATION { get; set; }
        
        public DateTime STA_DATE { get; set; }
        public string STA_DATE_DISP
        {
            get
            {
                return STA_DATE.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        public DateTime END_DATE { get; set; }
        public string END_DATE_DISP
        {
            get
            {
                return END_DATE.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        /// <summary>
        /// 시작위치 X
        /// </summary>
        public double POS_X { get; set; }
        /// <summary>
        /// 시작위치 Y
        /// </summary>
        public double POS_Y { get; set; }
        /// <summary>
        /// 가로 사이즈
        /// </summary>
        public double SZ_W { get; set; }
        /// <summary>
        /// 높이 사이즈
        /// </summary>
        public double SZ_H { get; set; }
        //public object Clone()
        //{
        //    MediaData itemC = new MediaData();

        //    itemC.IS_SELECT = this.IS_SELECT;
        //    itemC.MEDIA_GBN = this.MEDIA_GBN;
        //    itemC.SEQ_NO = this.SEQ_NO;
        //    itemC.USE_YN = this.USE_YN;
        //    itemC.FILE_NM = this.FILE_NM;
        //    itemC.FILE_URL = this.FILE_URL;
        //    itemC.FILE_EXT = this.FILE_EXT;
        //    itemC.VOLUME = this.VOLUME;
        //    itemC.DURATION = this.DURATION;
        //    itemC.POS_X = this.POS_X;
        //    itemC.POS_Y = this.POS_Y;
        //    itemC.SZ_W = this.SZ_W;
        //    itemC.SZ_H = this.SZ_H;
        //    itemC.S_DATE = this.S_DATE;
        //    itemC.E_DATE = this.E_DATE;
        //    itemC.SCREEN_NO = this.SCREEN_NO;

        //    return itemC;
        //}
    }
}
