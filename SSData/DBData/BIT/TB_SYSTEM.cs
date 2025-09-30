using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSData
{
	public class TB_SYSTEM
	{
		public int SEQ_NO { get; set; }
		public int LOGSAVE_DAY { get; set; }

		private int _logSavePercent;
		public int LOGSAVE_PERCENT { 
			get => _logSavePercent == 0 ? 10 : _logSavePercent; 
			set => _logSavePercent = value;
		}
		public int LED_PAGE_CHGTIME { get; set; }
		public bool LED_USE_BUSNO_COLOR { get; set; }
		public bool LED_USE_BUSNO_LOW { get; set; }
		public bool LED_USE_ARRIVESOON_LOW { get; set; }
		public bool LED_USE_ARRIVESOON_COLOR { get; set; }

		//20220718 BHA
		public bool ENV_USE_FAN_MANUAL { get; set; }
		public bool ENV_USE_HEATER_MANUAL { get; set; }

		//20221102 bha
		public int SUBWAY_DISPLAY_YN { get; set; }
		public int SUBWAY_LINE_NO { get; set; }
		public int SUBWAY_STATION_NO { get; set; }
		//20221129 
		public int SHOCK_DETECT_NO { get; set; }
		public DateTime REGDATE { get; set; }

		public bool IS_VERTICAL_LAYOUT { get; set; }
		public bool IS_SOLAR_MODEL { get; set; }
		public bool SHOW_BUS_ROUTE { get; set; }
		public int MAX_ROUTE_COUNT { get; set; }
	}
}