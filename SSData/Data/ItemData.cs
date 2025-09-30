using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSData
{
    public class ItemData
    {
        public bool SELECT_YN { get; set; }

        public int SEQNO { get; set; }
        /// <summary>
        /// CODE가 숫자로 구성되어 있을경우 Int형
        /// </summary>
        public int nCODE{ get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string NAME_S { get; set; }
        /// <summary>
        /// 그룹
        /// </summary>
        public string GRP_NM { get; set; }

        public object TAG { get; set; }
        /// <summary>
        /// X좌표 , 경도(lng)
        /// </summary>
        public double POS_X { get; set; }
        /// <summary>
        /// Y좌표 , 위도(lat)
        /// </summary>
        public double POS_Y { get; set; }
        public double SZ_W { get; set; }
        public double SZ_H { get; set; }
    }
}
