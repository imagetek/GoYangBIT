using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSData
{	
	public class BIT_SYSTEM
	{
		public int SEQ_NO { get; set; }
		public string BIT_ID { get; set; }
		public int MOBILE_NO { get; set; }
		public string STATION_ID { get; set; }
		public string STATION_NM { get; set; }
		public string SERVER_URL { get; set; }
		public int SERVER_TYPE{ get; set; }
		public string SERVER_PORT { get; set; }
		public int FTP_GBN { get; set; }
		public string FTP_IP { get; set; }
		public int FTP_PORT { get; set; }
		public string FTP_USERID { get; set; }
		public string FTP_USERPWD { get; set; }
		public string HTTP_URL { get; set; }
		public int HTTP_PORT { get; set; }
		public int ENV_GBN{ get; set; }
		public string ENV_PORT_NM { get; set; }
		public int ENV_BAUD_RATE { get; set; }
		//public string WEBCAM_NM { get; set; }
		//public string SHOCKCAM_NM { get; set; }
		public int CAM_NO1 { get; set; }
		public int CAM_NO1_ROTATE { get; set; }
		public int CAM_NO2 { get; set; }
		public int CAM_NO2_ROTATE { get; set; }
		//public int SHOCKCAM_NO { get; set; }
		public DateTime REGDATE { get; set; }
	}
}