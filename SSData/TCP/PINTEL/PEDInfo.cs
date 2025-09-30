using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SSData
{
    public class PEDInfo
    {
        public byte STX = 0x02;
        public CMDType CMD_GBN { get; set; }
        public LUMType LUMIN_GBN { get; set; }
        public int LENGTH { get; set; }
        public string FILE_NM { get; set; }
        public string FILE_EXT { get; set; }
        public List<byte> FILE_BINARY { get; set; }
        //public System.Drawing.Image IMG { get; set; }
        //public System.Windows.Media.Imaging.BitmapImage bIMG { get; set; }
        public string DISPTXT { get; set; }
        public byte ETX = 0x03;
    }

    public enum CMDType
    {
        NONE,
        텍스트표출 = 3,
        이미지표출,
        파일저장,
        파일삭제,
        저장파일_화면표출,
        표출중지,

        //RES_TEXT출력 = 13,
        //RES_이미지출력,
        //RES_파일저장,
        //RES_파일삭제,
        //RES_저장파일_VMS출력,
        //RES_표출중지,
    }

    public enum LUMType
    {
        NONE = -1,
        자동 = 0,
        주간,
        야간,
    }
}
