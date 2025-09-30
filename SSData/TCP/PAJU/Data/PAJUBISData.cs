using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SSData
{

	//DATA TYPE은 다음과 같으며 Dummy 데이터는 Ox00 로 한다.
	//H(Hex)  : n바이트의 인텔 포멧 Binary 데이터
	//C(Char) : n바이트의 ASCII데이터(단위는 바이트) 또는 문자열
	//N(Number) : n바이트의 숫자 데이터

	public struct PAJU_BIS_PROTOCOL
	{
		public byte STX;
		public byte OPCODE;
		/// <summary>
		/// 송수신 카운트 : 2byte
		/// </summary>
		public int SR_CNT;
		/// <summary>
		/// BIT ID : 4byte
		/// </summary>
		public int DeviceID;
		/// <summary>
		/// DATA길이 : 4Byte
		/// </summary>
		public int DataLength;
		/// <summary>
		/// DATA : NByte
		/// </summary>
		public byte[] DATA;
		/// <summary>
		/// HEADER부터 DATA까지 XOR
		/// </summary>
		public byte CheckSum;
		public byte ETX;

		//내부용
		public int RESULT_CD;
		public string RESULT_MSG;
		public PAJU_BIS_OPCODE_TYPE OP_GBN;
	}

	public enum PAJU_BIS_OPCODE_TYPE : byte
	{
		단말기상태정보ACK = 0xC2,
		도착예정정보 = 0xD2,

		파라메터변경 = 0xE5,
		파라메터변경ACK = 0xF5,

		부팅정보ACK = 0xC0,

		단말기상태제어 = 0xA1,
		단말기상태제어수신완료ACK = 0xB1,

		시정홍보자료전송 = 0xA2,
		시정홍보자료수신완료ACK = 0xB2,

		BIT충격영상전송 = 0xD8,
		//내부용
		DEFAULT = 0x00,
	}


	public enum PAJU_BIS_상세제어Type
	{
		NONE = 0,
		볼륨,
		/// <summary>
		/// 잠시후도착조건
		/// </summary>
		잠시후도착조건,
		/// <summary>
		/// 잠시후도착시간조건
		/// </summary>
		잠시후도착시간조건,
		/// <summary>
		/// 잠시후도착정류장조건
		/// </summary>
		잠시후도착정류장조건,
		/// <summary>
		/// 모니터O시간
		/// </summary>
		모니터On시간,
		/// <summary>
		/// 모니터Off시간
		/// </summary>
		모니터Off시간,
		/// <summary>
		/// 상태정보 전송주기
		/// </summary>
		상태정보전송주기,
		/// <summary>
		/// 영상정보전송주기
		/// </summary>
		영상정보전송주기,
		/// <summary>
		/// ScreeCapture 전송주기
		/// </summary>
		ScreeCapture전송주기,
		/// <summary>
		/// BIT정보정렬방식
		/// </summary>
		BIT정보정렬방식,
		/// <summary>
		/// 동작감시센서 사용여부
		/// </summary>
		동작감시센서사용여부,
		/// <summary>
		/// 동작감시센서사용시간
		/// </summary>
		동작감시센서사용시간,
		/// <summary>
		/// FA TEMP MAX
		/// </summary>
		팬동작온도조건MAX,
		/// <summary>
		/// FA TEMP MI
		/// </summary>
		팬동작온도조건MIN,
		/// <summary>
		/// Heater TEMP MAX
		/// </summary>
		히터동작온도조건MAX,
		/// <summary>
		/// Heater TEMP MI
		/// </summary>
		히터동작온도조건MIN,
		/// <summary>
		/// 지하철정보표시여부
		/// </summary>
		지하철정보표시여부,
		/// <summary>
		/// 지하철 호선 코드
		/// </summary>
		지하철호선코드,
		/// <summary>
		/// 지하철 역 코드
		/// </summary>
		지하철역코드,
		/// <summary>
		/// 외국어 정보 표시여부
		/// </summary>
		외국어정보표시여부,
		/// <summary>
		/// 외국어 정보 표출시간
		/// </summary>
		외국어정보표출시간,
		/// <summary>
		/// 충격감지감도
		/// </summary>
		충격감지감도,
		/// <summary>
		/// LCD제어
		/// </summary>
		LCD제어,
		/// <summary>
		/// Reset제어
		/// </summary>
		Reset제어,
		/// <summary>
		/// 시정홍보음성표출
		/// </summary>
		시정홍보음성표출,
		/// <summary>
		/// BIT폰트 크기
		/// </summary>
		BIT표출글씨크기,
		/// <summary>
		/// 시험운영중 표출여부
		/// </summary>
		시험운영중표출여부,
	}
	/// <summary>
	/// 외국어 구분
	/// </summary>
	public enum PAJU_BIS_외국어Type
	{
		NONE,
		한국어 = 1,
		영어,
		중국어,
		일본어,
	}
	//public enum PAJU_BIS_색상Type
	//{
	//    NONE,
	//    한국어 = 1,
	//    영어,
	//    중국어,
	//    일본어,
	//}


	public enum PAJU_BIS_운행상태Type
	{
		NONE = -1,
		차고지대기 = 0,
		운행중 = 1,
		회차지대기,
		운행종료,
	}

	/// <summary>
	/// 파주 노선정보
	/// </summary>
	//public class PAJU_BIS_노선정보
	//{
	//    /// <summary>
	//    /// 노선ID : 4byte
	//    /// </summary>
	//    public int RouteId { get; set; }
	//    /// <summary>
	//    /// RouteNumber C : 16byte 000-0”에서 “-“의 앞자리 수
	//    /// </summary>
	//    public string RouteNumberT { get; set; }
	//    /// <summary>
	//    /// RouteNumber 표출색상 : (1: 빨강 , 2 : 초록 , 3 : 노랑 , 4 : 흰색)
	//    /// </summary>
	//    public int RouteNumberDisplayColor { get; set; }
	//    /// <summary>
	//    /// 노선 방향  0x01: 시점출발 -> 종점도착 0x02: 종점출발 -> 시점도착 0x03: 시점출발 -> 회차지 경유 -> 종점도착
	//    /// </summary>
	//    public int RouteDirection { get; set; }
	//    /// <summary>
	//    /// 행선지표출여부 도착예정정보에 행선지명 표출 여부를 결정 (0 : 미표출 , 1 : 표출)
	//    /// </summary>
	//    public int DisplayDestination { get; set; }
	//}

	/// <summary>
	/// 파주 상세제어코드 모두 2byte int
	/// </summary>
	public class PAJU_BIS_상세제어
	{
		/// <summary>
		/// 볼륨
		/// </summary>
		public int n볼륨 { get; set; }
		/// <summary>
		/// 잠시후도착조건
		/// </summary>
		public int n잠시후도착조건 { get; set; }
		/// <summary>
		/// 잠시후도착시간조건
		/// </summary>
		public int n잠시후도착시간조건 { get; set; }
		/// <summary>
		/// 잠시후도착정류장조건
		/// </summary>
		public int n잠시후도착정류장조건 { get; set; }
		/// <summary>
		/// 모니터On시간
		/// </summary>
		public int n모니터On시간 { get; set; }
		/// <summary>
		/// 모니터Off시간
		/// </summary>
		public int n모니터Off시간 { get; set; }
		/// <summary>
		/// 상태정보 전송주기
		/// </summary>
		public int n상태정보전송주기 { get; set; }
		/// <summary>
		/// 영상정보전송주기
		/// </summary>
		public int n영상정보전송주기 { get; set; }
		/// <summary>
		/// ScreenCapture 전송주기
		/// </summary>
		public int nScreenCapture전송주기 { get; set; }
		/// <summary>
		/// BIT정보정렬방식
		/// </summary>
		public int nBIT정보정렬방식 { get; set; }
		/// <summary>
		/// 동작감시센서 사용여부
		/// </summary>
		public int n동작감시센서사용여부 { get; set; }
		/// <summary>
		/// 동작감시센서사용시간
		/// </summary>
		public int n동작감시센서사용시간 { get; set; }
		/// <summary>
		/// FAN TEMP MAX
		/// </summary>
		public int n팬동작온도조건MAX { get; set; }
		/// <summary>
		/// FAN TEMP MIN
		/// </summary>
		public int n팬동작온도조건MIN { get; set; }
		/// <summary>
		/// Heater TEMP MAX
		/// </summary>
		public int n히터동작온도조건MAX { get; set; }
		/// <summary>
		/// Heater TEMP MIN
		/// </summary>
		public int n히터동작온도조건MIN { get; set; }
		/// <summary>
		/// 지하철정보표시여부
		/// </summary>
		public int n지하철정보표시여부 { get; set; }
		/// <summary>
		/// 지하철 호선 코드
		/// </summary>
		public int n지하철호선코드 { get; set; }
		/// <summary>
		/// 지하철 역 코드
		/// </summary>
		public int n지하철역코드 { get; set; }
		/// <summary>
		/// 외국어 정보 표시여부
		/// </summary>
		public int n외국어정보표시여부 { get; set; }
		/// <summary>
		/// 외국어 정보 표출시간
		/// </summary>
		public int n외국어정보표출시간 { get; set; }
		/// <summary>
		/// 충격감지감도
		/// </summary>
		public int n충격감지감도 { get; set; }
		/// <summary>
		/// LCD제어
		/// </summary>
		public int nLCD제어 { get; set; }
		/// <summary>
		/// Reset제어
		/// </summary>
		public int nReset제어 { get; set; }
		/// <summary>
		/// 시정홍보음성표출
		/// </summary>
		public int n시정홍보음성표출 { get; set; }
		/// <summary>
		/// BIT폰트 크기
		/// </summary>
		public int nBIT표출글씨크기 { get; set; }
		/// <summary>
		/// 시험운영중 표출여부
		/// </summary>
		public int n시험운영중표출여부 { get; set; }
	}

	public class BisArrivalInformation
	{

		/// <summary>
		/// 노선ID : 4byte
		/// </summary>
		[JsonPropertyName("routeId")]
		public int RouteId { get; set; }
		/// <summary>
		/// RouteNumber C : 16byte 000-0”에서 “-“의 앞자리 수
		/// </summary>
		[JsonPropertyName("routeName")]
		public string RouteNumberT { get; set; }

		//[JsonPropertyName("route_name")]
		public string RouteNumber
		{
			get
			{
				return RouteNumberT;
			}
		}
		/// <summary>
		/// RouteNumber 표출색상 : (1: 빨강 , 2 : 초록 , 3 : 노랑 , 4 : 흰색)
		/// </summary>
		public int RouteNumberDisplayColor { get; set; }
		//public ItemData 노선번호색상정보
		//{
		//    get
		//    {
		//        List<ItemData> items = CodeManager.Get색상종류().Where(data => data.nCODE.Equals(RouteNumberDisplayColor) == true).ToList();
		//        if (items != null && items.Count > 0) return items.First();
		//        return null;
		//    }
		//}

		/// <summary>
		/// 노선 방향  0x01: 시점출발 -> 종점도착 0x02: 종점출발 -> 시점도착 0x03: 시점출발 -> 회차지 경유 -> 종점도착
		/// </summary>
		public int RouteDirection { get; set; }
		//public string 노선방향
		//{
		//    get
		//    {
		//        string disp = "";
		//        switch (RouteDirection)
		//        {
		//            case 1: disp = "시점출발→종점도착"; break;
		//            case 2: disp = "종점출발→시점도착"; break;
		//            case 3: disp = "시점출발→회차지경유→종점도착"; break;
		//        }
		//        return disp;
		//    }
		//}
		/// <summary>
		/// 행선지표출여부 도착예정정보에 행선지명 표출 여부를 결정 (0 : 미표출 , 1 : 표출)
		/// </summary>
		public int DisplayDestination { get; set; }
		//public bool 행선지표출YN
		//{
		//    get
		//    {
		//        bool isYN = false;
		//        if (DisplayDestination == 1) isYN = true;
		//        return isYN;
		//    }
		//}
		//public string 행선지표출여부
		//{
		//    get
		//    {
		//        string disp = "";
		//        switch (DisplayDestination)
		//        {
		//            case 0: disp = "미표출"; break;
		//            case 1: disp = "표출"; break;
		//        }
		//        return disp;
		//    }
		//}
		public int StopLocationInformation { get; set; }
		//public string 정류장위치
		//{
		//    get
		//    {
		//        string disp = "";
		//        switch (StopLocationInformation)
		//        {
		//            case 0: disp = "현재정류소"; break;
		//            case 1: disp = "맞은편정류소"; break;
		//            case 2: disp = "인접정류소"; break;
		//            case 3: disp = "불특정정류소"; break;
		//        }
		//        return disp;
		//    }
		//}
		public int StopLocationInformationColorCode { get; set; }
		//public ItemData 정류장위치정보색상
		//{
		//    get
		//    {
		//        List<ItemData> items = CodeManager.Get색상종류().Where(data => data.nCODE.Equals(StopLocationInformationColorCode) == true).ToList();
		//        if (items != null && items.Count > 0) return items.First();
		//        return null;
		//    }
		//}
		[JsonPropertyName("current_stop_id")]
		public int StopId { get; set; }

		[JsonPropertyName("stationNm1")]
		public string StopName { get; set; }
		public int StopNameColorCode { get; set; }
		//public ItemData 정류장명정보색상
		//{
		//    get
		//    {
		//        List<ItemData> items = CodeManager.Get색상종류().Where(data => data.nCODE.Equals(StopLocationInformationColorCode) == true).ToList();
		//        if (items != null && items.Count > 0) return items.First();
		//        return null;
		//    }
		//}       

		//[JsonPropertyName("vehId1")]
		public int VehicleNumber { get; set; }

		//[JsonPropertyName("plateNo1")]
		public string LicensePlateNumber { get; set; }
		public int ForeignLanguageCount { get; set; }
		public int ForeignLanguageTypeCode1 { get; set; }
		//public PAJU_BIS_외국어Type 외국어타입1
		//{
		//    get
		//    {
		//        PAJU_BIS_외국어Type item = (PAJU_BIS_외국어Type)ForeignLanguageTypeCode1;
		//        return item;
		//    }
		//}
		public string ForeignLanguageStopName1 { get; set; }
		public string ForeignLanguageOriginName1 { get; set; }
		public string ForeignLanguageDestinationName1 { get; set; }
		public int ForeignLanguageTypeCode2 { get; set; }
		//public PAJU_BIS_외국어Type 외국어타입2
		//{
		//    get
		//    {
		//        PAJU_BIS_외국어Type item = (PAJU_BIS_외국어Type)ForeignLanguageTypeCode2;
		//        return item;
		//    }
		//}
		public string ForeignLanguageStopName2 { get; set; }
		public string ForeignLanguageOriginName2 { get; set; }
		public string ForeignLanguageDestinationName2 { get; set; }
		public int ForeignLanguageTypeCode3 { get; set; }
		//public PAJU_BIS_외국어Type 외국어타입3
		//{
		//    get
		//    {
		//        PAJU_BIS_외국어Type item = (PAJU_BIS_외국어Type)ForeignLanguageTypeCode3;
		//        return item;
		//    }
		//}
		public string ForeignLanguageStopName3 { get; set; }
		public string ForeignLanguageOriginName3 { get; set; }
		public string ForeignLanguageDestinationName3 { get; set; }
		[JsonPropertyName("locationNo1")]
		public string Location { get; set; }

		
		public int NumberOfRemainingStops {  get; set; }
		
		[JsonPropertyName("operation_stat")]
		public int OperationStatus { get; set; }
       
        //public PAJU_BIS_운행상태Type 운행상태
        //{
        //	get
        //	{
        //		PAJU_BIS_운행상태Type item = (PAJU_BIS_운행상태Type)OperationStatus;
        //		return item;
        //	}
        //}
        [JsonPropertyName("predictTimeSec1")]
		public int EstimatedTimeOfArrival { get; set; }
		/// <summary>
		/// 단위 초 , 단 운행중일경우만 사용
		/// </summary>
		//public string 도착예정시간
		//{
		//    get
		//    {
		//        return "";
		//    }
		//}


		public int EstimatedArrivalTimeDisplayColor { get; set; }

		/// <summary>
		/// 버스유형 0 : 일반 , 1 : 저상 
		/// </summary>
		[JsonPropertyName("lowPlate1")]
		public string LowPlate { get; set; }

		public int BusType {  get; set; }
		
		public bool IsVillageBus
		{
			get
			{
				//bool retYN = false;
				//switch (RouteType)
				//{
				//    //case 23:
				//    case 30: retYN = true; break;
				//}
				//return retYN;
				return RouteType == 3;
			}
		}
		public int FirstAndLastTrainType { get; set; }

		[JsonPropertyName("routeTypeCd")]
		public int RouteType { get; set; }
		public string OriginName { get; set; }
		public int OriginNameExpressionColor { get; set; }

		[JsonPropertyName("routeDestName")]
		public string DestinationName { get; set; }
		public int DestinationNameDisplayColor { get; set; }

		[JsonPropertyName("crowded1")]

		public int BusCongestion {  get; set; }

		
		public int NumberOfRemainingSeats {  get; set; }
		public string Reservation { get; set; }

		//[JsonPropertyName("estimated_arrival_time")] need to change string to datetime

		private DateTime? _timeOfArrival;
		public DateTime TimeOfArrival
		{
			get
			{
				return _timeOfArrival ?? DateTime.Now.AddSeconds(EstimatedTimeOfArrival);
			}
			set
			{
				_timeOfArrival = value;
			}
		}
    }

	public class AirQualityData
	{
		public AirQuality FineDust { get; set; } = new();
		public AirQuality UltraFineDust { get; set; } = new();
		public AirQuality Ozone { get; set; } = new();
	}

	public class AirQuality
	{
		public string BaseValue { get; set; }
		public string ActualValue { get; set; }
		public int Index { get; set; }
	}

	public class WeatherData
	{
		public string WeatherText { get; set; }
		public string CurrentAirTemperature { get; set; }
		public int MaxTemp { get; set; }
		public int MinTemp { get; set; }
		public BitmapSource WeatherImage { get; set; }
	}

	public class PajuBisStatusControl
	{
		public int ControlCode { get; set; }
		public int ControlValue { get; set; }
	}

	#region To CENTER 단말기 → 센터

	/// <summary>
	/// 단말기 상태보고 0xC2 단말 -> 센터
	/// </summary>
	public class PAJU_BIT_단말기상태
	{
		//public int TransmissionDate { get; set; }
		//public int TransmissionTime { get; set; }
		//public int n볼륨{ get; set; }
		public byte bLCDOnOff상태 { get; set; }
		/// <summary>
		/// FAN동작상태 (ON 0)
		/// </summary>
		public byte bFAN동작상태 { get; set; }
		/// <summary>
		/// 웹카메라동작상태 (ON 1)
		/// </summary>
		public byte b웹카메라동작상태 { get; set; }
		/// <summary>
		/// 히터동작상태 (ON 0)
		/// </summary>
		public byte b히터동작상태 { get; set; }
		/// <summary>
		/// 온도
		/// </summary>
		public int n온도 { get; set; }
		/// <summary>
		/// 습도
		/// </summary>
		public int n습도 { get; set; }
		/// <summary>
		/// 도어상태 (ON 0)
		/// </summary>
		public int n도어상태 { get; set; }
		/// <summary>
		/// 동작감지센서 LCD ON/OFF ( 0 : OFF , 1 : ON , 2 미동작중)
		/// </summary>
		public int n동작감지센서LCDOnOFF { get; set; }
		/// <summary>
		/// LCD전류감지 0 :미감지 , 1:전류감지
		/// </summary>
		public int nLCD전류감지 { get; set; }
		/// <summary>
		/// 시험운영중 표출여부 ( 1 표출)
		/// </summary>
		public int n시험운영중표출여부 { get; set; }
	}

	/// <summary>
	/// 6.2 파라메터 변경(0xF5): 단말 -> 센터
	/// </summary>
	public class PAJU_BIT_파라메터변경보고
	{
		/// <summary>
		/// 전송일자 4byte
		/// </summary>
		public int n전송일자 { get; set; }
		/// <summary>
		/// 전송시간 4byte
		/// </summary>
		public int n전송시간 { get; set; }
		/// <summary>
		/// 정상 0 
		/// </summary>
		public int n변경상태 { get; set; }
	}

	/// <summary>
	/// 7.1 부팅정보(0xC0)
	/// </summary>
	public class PAJU_BIT_부팅정보
	{
		/// <summary>
		/// 전송일자 4byte
		/// </summary>
		public int n전송일자 { get; set; }
		/// <summary>
		/// 전송시간 4byte
		/// </summary>
		public int n전송시간 { get; set; }
		/// <summary>
		/// 관리번호
		/// </summary>
		public int nAPP개정번호 { get; set; }
		public int nBIT유형 { get; set; }
		public int sIP { get; set; }
		public string s예약 { get; set; }
	}

	/// <summary>
	/// 7.3 단말기상태제어 수신완료(0xB1): 
	/// </summary>
	public class PAJU_BIT_단말기상태제어수신
	{
		public int n전송일자 { get; set; }
		public int n전송시간 { get; set; }
		public byte b수신상태 { get; set; }
	}

	/// <summary>
	///7.5 시정호보자료 수신 응답(0xB2): 
	/// </summary>
	public class PAJU_BIT_시정홍보자료수신
	{
		public int n전송일자 { get; set; }
		public int n전송시간 { get; set; }
		public byte b변경상태 { get; set; }
	}

	/// <summary>
	///7.6 BIT 충격영상 전송(0xD8):
	/// </summary>
	public class PAJU_BIT_충격영상전송
	{
		public int n충격발생일자 { get; set; }
		public int n전송시간 { get; set; }
		public int n충격시센서값 { get; set; }
		public string s업로드파일명 { get; set; }
	}


	#endregion

	#region From CENTER 센터 -> 단말

	/// <summary>
	/// 5.2 도착예정정보 0xD2 센터 -> 단말
	/// </summary>
	public class BisExpectedArrivalInformation
	{
		public int TransmissionDate { get; set; }
		public int TransmissionTime { get; set; }
		public string BitId { get; set; }
		public int NumberOfOperationInformation { get; set; }

		public string TimeWhenInfoReceived { get; set; }

		public List<BisArrivalInformation> ArrivalInformationList { get; set; }

		///// <summary>
		///// 프로그램 내부용
		///// </summary>
		//public DateTime UPDATE { get; set; }
	}

	/// <summary>
	/// 6.1 파라메터 변경 0xE5 센터 -> 단말 (
	/// </summary>
	//public class PAJU_BIS_파라메터변경
	//{
	//    /// <summary>
	//    /// 전송일자 4byte
	//    /// </summary>
	//    public int TransmissionDate { get; set; }
	//    /// <summary>
	//    /// 전송시간 4byte
	//    /// </summary>
	//    public int TransmissionTime { get; set; }
	//    /// <summary>
	//    /// 볼륨
	//    /// </summary>
	//    public int n볼륨 { get; set; }
	//    /// <summary>
	//    /// 잠시후도착조건
	//    /// </summary>
	//    public int n잠시후도착조건 { get; set; }
	//    /// <summary>
	//    /// 잠시후도착시간조건
	//    /// </summary>
	//    public int n잠시후도착시간조건 { get; set; }
	//    /// <summary>
	//    /// 잠시후도착정류장조건
	//    /// </summary>
	//    public int n잠시후도착정류장조건 { get; set; }
	//    /// <summary>
	//    /// 모니터On시간
	//    /// </summary>
	//    public int n모니터On시간 { get; set; }
	//    /// <summary>
	//    /// 모니터Off시간
	//    /// </summary>
	//    public int n모니터Off시간 { get; set; }

	//    public int DefaultLCD조도값 { get; set; }
	//    public int 시간대별조도값총수 { get; set; }

	//    public List<PAJU_BIS_조도> items조도 { get; set; }

	//    /// <summary>
	//    /// 상태정보 전송주기
	//    /// </summary>
	//    public int n상태정보전송주기 { get; set; }
	//    /// <summary>
	//    /// 영상정보전송주기
	//    /// </summary>
	//    public int n영상정보전송주기 { get; set; }
	//    /// <summary>
	//    /// ScreenCapture 전송주기
	//    /// </summary>
	//    public int nScreenCapture전송주기 { get; set; }
	//    /// <summary>
	//    /// BIT정보정렬방식
	//    /// </summary>
	//    public int nBIT정보정렬방식 { get; set; }
	//    /// <summary>
	//    /// 동작감시센서 사용여부
	//    /// </summary>
	//    public int n동작감시센서사용여부 { get; set; }
	//    /// <summary>
	//    /// 동작감시센서사용시간
	//    /// </summary>
	//    public int n동작감시센서사용시간 { get; set; }
	//    /// <summary>
	//    /// FAN TEMP MAX
	//    /// </summary>
	//    public int n팬동작온도조건MAX { get; set; }
	//    /// <summary>
	//    /// FAN TEMP MIN
	//    /// </summary>
	//    public int n팬동작온도조건MIN { get; set; }
	//    /// <summary>
	//    /// Heater TEMP MAX
	//    /// </summary>
	//    public int n히터동작온도조건MAX { get; set; }
	//    /// <summary>
	//    /// Heater TEMP MIN
	//    /// </summary>
	//    public int n히터동작온도조건MIN { get; set; }
	//    /// <summary>
	//    /// 지하철정보표시여부
	//    /// </summary>
	//    public int n지하철정보표시여부 { get; set; }
	//    /// <summary>
	//    /// 지하철 호선 코드
	//    /// </summary>
	//    public int n지하철호선코드 { get; set; }
	//    /// <summary>
	//    /// 지하철 역 코드
	//    /// </summary>
	//    public int n지하철역코드 { get; set; }
	//    /// <summary>
	//    /// 외국어 정보 표시여부
	//    /// </summary>
	//    public int n외국어정보표시여부 { get; set; }
	//    /// <summary>
	//    /// 외국어 정보 표출시간
	//    /// </summary>
	//    public int n외국어정보표출시간 { get; set; }
	//    /// <summary>
	//    /// 충격감지감도
	//    /// </summary>
	//    public int n충격감지감도 { get; set; }
	//    ///// <summary>
	//    ///// LCD제어
	//    ///// </summary>
	//    //public int nLCD제어 { get; set; }
	//    ///// <summary>
	//    ///// Reset제어
	//    ///// </summary>
	//    //public int nReset제어 { get; set; }
	//    /// <summary>
	//    /// 충격감지감도
	//    /// </summary>
	//    public int n정류소번호 { get; set; }
	//    /// <summary>
	//    /// 충격감지감도
	//    /// </summary>
	//    public string s정류소명칭 { get; set; }
	//    /// <summary>
	//    /// 시정홍보음성표출
	//    /// </summary>
	//    public int n시정홍보음성표출 { get; set; }
	//    /// <summary>
	//    /// BIT폰트 크기
	//    /// </summary>
	//    public int nBIT표출글씨크기 { get; set; }
	//    /// <summary>
	//    /// 시험운영중 표출여부
	//    /// </summary>
	//    public int n시험운영중표출여부 { get; set; }
	//    public int n예비 { get; set; }
	//}

	/// <summary>
	/// 7.2 단말기상태제어(0xA1) :
	/// </summary>
	public class PAJU_BIS_단말기상태제어
	{
		public int n전송일자 { get; set; }
		public int n전송시간 { get; set; }
		public int n총제어수 { get; set; }

		public List<PajuBisStatusControl> items상태제어 { get; set; }
	}

	/// <summary>
	/// 7.4 시정홍보 자료 전송(0xA2)
	/// </summary>
	public class PAJU_BIS_시정홍보자료전송
	{
		public int n전송일자 { get; set; }
		public int n전송시간 { get; set; }
	}


	#endregion
}
