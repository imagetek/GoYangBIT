using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSData
{
	/// <summary>
	/// 기본 LED 설정
	/// </summary>
	public class LED_ITEM
	{
		public int SEQ_NO { get; set; }
		public int BIT_ID { get; set; }
		/// <summary>
		/// LED 화면종류  3X6 , 3X8....
		/// </summary>
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
		public int USE_YN { get; set; }
		public string CELL_NO { get; set; }
		public int POS_X { get; set; }
		public int POS_Y { get; set; }
		public int SZ_W { get; set; }
		public int SZ_H { get; set; }
		public string DISP_TEXT { get; set; }
		public string FONT_NM { get; set; }
		public double FONT_SZ { get; set; }
		public string FONT_ARGB { get; set; }
		public int FONT_STYLE_GBN { get; set; }
		public int FONT_ALIGN_GBN { get; set; }
		public int FONT_WEIGHT_GBN { get; set; }
		public DateTime REGDATE { get; set; }
	}

	/// <summary>
	/// 곧도착정보 설정
	/// </summary>
	public class LED_BUS_ARRIVESOON
	{
		public int SEQ_NO { get; set; }
		public int BIT_ID { get; set; }
		/// <summary>
		/// LED 화면종류  3X6 , 3X8....
		/// </summary>
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
		public int USE_YN { get; set; }
		public string TITLE_TEXT { get; set; }
		public int TITLE_POS_X { get; set; }
		public int TITLE_POS_Y { get; set; }
		public int TITLE_SZ_W { get; set; }
		public int TITLE_SZ_H { get; set; }
		public string TITLE_FONT_NM { get; set; }
		public double TITLE_FONT_SZ { get; set; }
		public string TITLE_FONT_ARGB { get; set; }
		public int TITLE_FONT_STYLE_GBN { get; set; }
		public int TITLE_FONT_WEIGHT_GBN { get; set; }
		public int TITLE_FONT_ALIGN_GBN { get; set; }

		public int CONT_POS_X { get; set; }
		public int CONT_POS_Y { get; set; }
		public int CONT_SZ_W { get; set; }
		public int CONT_SZ_H { get; set; }
		public string CONT_FONT_NM { get; set; }
		public double CONT_FONT_SZ { get; set; }
		public string CONT_FONT_ARGB { get; set; }
		public int CONT_FONT_STYLE_GBN { get; set; }
		public int CONT_FONT_WEIGHT_GBN { get; set; }
		//public int CONT_FONT_ALIGN_GBN { get; set; }

		public DateTime REGDATE { get; set; }
	}

	
}
