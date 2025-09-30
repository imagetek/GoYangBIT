using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSData.GwachonAPI
{
	public class GwachonAirQualityResponse
	{
		public GwachonResponse response { get; set; }
	}
	public class Body
	{
		public int totalCount { get; set; }
		public List<Item> items { get; set; }
		public int pageNo { get; set; }
		public int numOfRows { get; set; }
	}

	public class Header
	{
		public string resultMsg { get; set; }
		public string resultCode { get; set; }
	}

	public class Item
	{
		public string pm25Grade1h { get; set; }
		public string pm10Value24 { get; set; }
		public string so2Value { get; set; }
		public string pm10Grade1h { get; set; }
		public string o3Grade { get; set; }
		public string pm10Value { get; set; }
		public object pm25Flag { get; set; }
		public string khaiGrade { get; set; }
		public string pm25Value { get; set; }
		public object no2Flag { get; set; }
		public string mangName { get; set; }
		public string no2Value { get; set; }
		public string so2Grade { get; set; }
		public object coFlag { get; set; }
		public string khaiValue { get; set; }
		public string coValue { get; set; }
		public object pm10Flag { get; set; }
		public string no2Grade { get; set; }
		public string pm25Value24 { get; set; }
		public object o3Flag { get; set; }
		public string pm25Grade { get; set; }
		public object so2Flag { get; set; }
		public string coGrade { get; set; }
		public string dataTime { get; set; }
		public string pm10Grade { get; set; }
		public string o3Value { get; set; }
	}

	public class GwachonResponse
	{
		public Body body { get; set; }
		public Header header { get; set; }
	}

	
}
