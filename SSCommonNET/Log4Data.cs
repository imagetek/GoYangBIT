namespace SSCommonNET
{
	public class Log4Data
	{
		public Log4Level Level { get; set; }

		public string 오류LV
		{
			get
			{
				string 오류Lv = "";
				switch (this.Level)
				{
					case Log4Level.Trace:
						오류Lv = "미비한오류";
						break;
					case Log4Level.Error:
						오류Lv = "오류";
						break;
					case Log4Level.Fatal:
						오류Lv = "치명적오류";
						break;
					case Log4Level.Info:
						오류Lv = "정보";
						break;
					case Log4Level.Debug:
						오류Lv = "디버그";
						break;
				}
				return 오류Lv;
			}
		}

		public DateTime REGDATE { get; set; }

		public string 등록일시 => this.REGDATE.ToString("yyyy-MM-dd HH:mm:sss");

		public string 로그내용 { get; set; }

		public LogSource LogSource { get; set; }
	}

	public enum LogSource
	{
		Other,
		BusInfo,
		Board
	}
}
