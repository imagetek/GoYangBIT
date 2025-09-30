using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSData
{
	public class BIT_DISPLAY
	{
		public int SEQ_NO { get; set; }
		public string BIT_ID { get; set; }
		public int DISP_GBN { get; set; }
		public string 표시화면
		{
			get
			{
				DISPLAY구분 item = (DISPLAY구분)DISP_GBN;
				return item.ToString();
			}
		}
		public DISPLAY구분 화면구분
		{
			get
			{
				DISPLAY구분 item = (DISPLAY구분)DISP_GBN;
				return item;
			}
		}

		public string DISP_NM { get; set; }
		public int POS_X { get; set; }
		public int POS_Y { get; set; }
		public int SZ_W { get; set; }
		public int SZ_H { get; set; }
		//public DateTime USE_YMD { get; set; }
		public DateTime REGDATE { get; set; }
	}
}
