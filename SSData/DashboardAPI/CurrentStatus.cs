namespace SSData.DashboardAPI
{
	public record CurrentStatus
	{
		public int Id { get; set; }
		public string City { get; set; }
		public string StationId { get; set; }
		public string StationName { get; set; }
		public long UpdateUnixTime { get; set; }
		public string UpdateDateTime { get; set; }
		public sbyte Temparature1 { get; set; }
		public sbyte Temparature2 { get; set; }
		public int Humidity { get; set; }
		public int Luminance { get; set; }
		public int Shock1 { get; set; }
		public int Door { get; set; }
		public int Illumination { get; set; }
		public int BattPercent { get; set; }
		public int RemainBat { get; set; }
		public long BitStatusUnixTime { get; set; }
		public int BitStatus { get; set; }
		public string BitStatusDateTime { get; set; }
		public long BusStatusUnixTime { get; set; }
		public int BusStatus { get; set; }
		public string BusStatusDateTime { get; set; }
		public long WeatherStatusUnixTime { get; set; }
		public int WeatherStatus { get; set; }
		public string WeatherStatusDateTime { get; set; }
	}
}
