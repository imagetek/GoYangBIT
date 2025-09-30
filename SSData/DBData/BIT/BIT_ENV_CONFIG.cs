using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSData
{
	public class BIT_ENV_SETTING
	{
		public int SEQ_NO { get; set; }
		public string BIT_ID { get; set; }
		public int Volume { get; set; }
		public int ArriveSoonGBN { get; set; }

		private int _arriveSoonTimeGBN;
		public int ArriveSoonTimeGBN { 
			get => _arriveSoonTimeGBN == 0 ? 180 : _arriveSoonTimeGBN; 
			set => _arriveSoonTimeGBN = value; 
		}

		private int _arriveSoonStationGBN;
		public int ArriveSoonStationGBN { 
			get => _arriveSoonStationGBN == 0 ? 3 : _arriveSoonStationGBN; 
			set => _arriveSoonStationGBN = value; 
		}
		private string _monitorOnTime;
		public string MonitorOnTime { 
			get => string.IsNullOrWhiteSpace(_monitorOnTime) ? "0430" : _monitorOnTime; 
			set => _monitorOnTime = value;
		}

		private string _monitorOffTime;
		public string MonitorOffTime { 
			get => string.IsNullOrWhiteSpace(_monitorOffTime) ? "0100" : _monitorOffTime; 
			set => _monitorOffTime = value;
		}
		public int DefaultLCDLux { get; set; }
		public int LuxCount { get; set; }
		public List<BIT_ENV_LUX> itemsLux { get; set; }
		public int StateSendPeriod { get; set; }
		public int WebcamSendPeriod { get; set; }
		public int ScreenCaptureSendPeriod { get; set; }
		public int BITOrderGBN { get; set; }
		public int UseDetectSensor { get; set; }
		public int DetectSensorServiceTime { get; set; }
		public int FANMaxTemp { get; set; }
		public int FANMinTemp { get; set; }
		public int HeaterMaxTemp { get; set; }
		public int HeaterMinTemp { get; set; }
		//20221102 BHA 미사용
		public int SubwayDisplayYN { get; set; }
		//20221102 BHA 미사용
		public int SubwayLineNo { get; set; }
		//20221102 BHA 미사용
		public int SubwayStationNo { get; set; }
		public int ForeignDisplayYN { get; set; }
		public int ForeignDisplayTime { get; set; }
		public int ShockDetectValue { get; set; }
		public int StationMobileNo { get; set; }
		public string StationName { get; set; }
		public int PromoteSoundPlayYN { get; set; }
		public int BITFontSize { get; set; }
		public int TestOperationDisplayYN { get; set; }
		public int Reserve1 { get; set; }
		//public DateTime USE_YMD { get; set; }
		public DateTime REGDATE { get; set; }

		public int PromotionRefreshInterval { get; set; }
		public int BusArrivalInfoRefreshInterval { get; set; }
	}

	public class BIT_ENV_LUX
	{
		public int SEQ_NO { get; set; }
		public int BIT_ID { get; set; }
		public int ENV_CONFIG_NO { get; set; }
		public string S_TIME { get; set; }
		public string E_TIME{ get; set; }
		public int LUX { get; set; }
		//public DateTime USE_YMD { get; set; }
		public DateTime REGDATE { get; set; }
	}
}
