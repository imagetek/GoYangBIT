using System;
using System.Collections.Generic;
using System.Linq;
using SSCommonNET;
using SSData.GwachonAPI;
using static SSData.ClientEngine;

namespace SSData
{
	/// <summary>
	/// 기존 파주 TCP/IP베이스 
	/// </summary>
	public class BISManager : PAJUBISPacket
	{
		//weather constants
		public const string WeatherTemperature = "TMP";
		public const string WeatherSkyCode = "SKY";
		public const string WeatherPtyCode = "PTY";
		public BISManager()
		{

		}

		internal ClientEngine mSocketPlayer = null;
		internal bool IsRecvState = false;
		/// <summary>
		/// 접속된 클라이언트
		/// </summary>
		internal Dictionary<string, ClientEngine> arrPlayerGroup = new Dictionary<string, ClientEngine>();
		/// <summary>
		/// 접속 상태
		/// </summary>
		internal bool IsAccessState = true;


		public void DoFinal()
		{
			try
			{
                GC.Collect();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public void Connet서버()
		{
			try
			{		
				IsAccessState = false;

				if (mSocketPlayer == null)
				{
					mSocketPlayer = new ClientEngine();
					mSocketPlayer.OnReceivedEvent += MSocketPlayer_OnReceivedEvent;
					mSocketPlayer.OnAirQualityReceivedEvent += MSocketPlayer_OnAirQualityReceivedEvent;
					mSocketPlayer.OnWeatherReceivedEvent += MSocketPlayer_OnWeatherReceivedEvent;
					mSocketPlayer.OnBusLocationReceivedEvent += MSocketPlayer_OnBusLocationReceivedEvent;
				}				
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private void MSocketPlayer_OnBusLocationReceivedEvent(BusLocation busLocation)
		{
			GwacheonBusStationService.UpdateBusLocations(busLocation);
		}

		private void MSocketPlayer_OnWeatherReceivedEvent(GwacheonWeatherResponse gwachonWeatherResponse)
		{
			OnWeatherDataReceivedEvent.Invoke(gwachonWeatherResponse);
		}


		private void MSocketPlayer_OnAirQualityReceivedEvent(GwachonAirQualityResponse gwachonWeatherResponse)
		{
			
			AirQualityData weatherData = GetAirQualityDataFromResponse(gwachonWeatherResponse.response.body.items.FirstOrDefault());

			if (weatherData == null)
			{
				return;
			}

			OnAirQualityDataReceivedEvent?.Invoke(weatherData);
		}

		private AirQualityData GetAirQualityDataFromResponse(Item item)
		{
			try
			{
				// base value change to 100, 50, 0.1 from 10, 35, 0.1
				AirQualityData weatherData = new()
				{
					Ozone = new()
				};
				weatherData.Ozone.ActualValue = item.o3Value;
				weatherData.Ozone.Index = string.IsNullOrWhiteSpace(item.o3Grade) ? -1 : int.Parse(item.o3Grade);
				weatherData.Ozone.BaseValue = "0.1";
				weatherData.FineDust = new()
				{
					ActualValue = item.pm10Value,
					Index = string.IsNullOrWhiteSpace(item.pm10Grade) ? -1 : int.Parse(item.pm10Grade),
					BaseValue = "100"
				};
				weatherData.UltraFineDust = new()
				{
					ActualValue = item.pm25Value,
					Index = string.IsNullOrWhiteSpace(item.pm25Grade) ? -1 : int.Parse(item.pm25Grade),
					BaseValue = "35"
				};

				return weatherData;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return null;
			}
			
		}

		private void MSocketPlayer_OnReceivedEvent(List<BusArrivalList> busArrivals)
		{
			List<BisArrivalInformation> arrivalInfo = MapValues(busArrivals);


			BisExpectedArrivalInformation arrivalPacketInfo = new BisExpectedArrivalInformation
			{
				BitId = BITDataManager.BIT_ID,
				ArrivalInformationList = arrivalInfo,
				TimeWhenInfoReceived = DateTime.Now.ToShortDateString(),
			};

			if (arrivalPacketInfo != null && arrivalPacketInfo.ArrivalInformationList != null)
			{
				string log = string.Format("[도착정보] {0} BIT_ID:{1} {2}건의 데이터를 수신했습니다.", arrivalPacketInfo.TimeWhenInfoReceived, arrivalPacketInfo.BitId, arrivalPacketInfo.ArrivalInformationList.Count);

				Log4Manager.WriteSocketLog(Log4Level.Info, log);
				On도착정보수신Event?.Invoke(arrivalPacketInfo);

				//add routes for station
				arrivalPacketInfo.ArrivalInformationList.ForEach(AddStationInfo);
				
			}
			else
			{
				string log = string.Format("[도착정보] {0} BIT_ID:{1} 버스운행정보가 없습니다.", arrivalPacketInfo.TimeWhenInfoReceived, arrivalPacketInfo.BitId);

				Log4Manager.WriteSocketLog(Log4Level.Info, log);

				On도착정보수신Event?.Invoke(arrivalPacketInfo);
			}
		}

		private void AddStationInfo(BisArrivalInformation info)
		{
			if (!GwacheonBusStationService.StationRouteMap.Exists(ri => ri.RouteId == info.RouteId))
			{
				GwacheonBusStationService.AddNewRoute(new RouteInfo { RouteId = info.RouteId, RouteName = info.RouteNumber });
			}
		}

		private List<BisArrivalInformation> MapValues(List<BusArrivalList> busArrivalList)
		{
			List<BisArrivalInformation> bisArrivalInformation = [];
			try
			{
				foreach (var item in busArrivalList)
				{
					BisArrivalInformation busInfo = new();
					busInfo.RouteId = item.routeId;
					busInfo.RouteNumberT = item.routeName.ToString();
					busInfo.StopName = item.stationNm1;
					busInfo.RouteType = item.routeTypeCd;
					busInfo.EstimatedTimeOfArrival = item.predictTimeSec1 is null ? 0 : (int)item.predictTimeSec1;
					busInfo.OperationStatus = GetOperationStatus(item.flag, item.predictTimeSec1);
					busInfo.NumberOfRemainingStops = string.IsNullOrWhiteSpace(item.locationNo1.ToString()) ? 0 : int.Parse(item.locationNo1.ToString());
					busInfo.NumberOfRemainingSeats = string.IsNullOrWhiteSpace(item.remainSeatCnt1.ToString()) ? 0 : int.Parse(item.remainSeatCnt1.ToString());
					busInfo.BusCongestion = string.IsNullOrWhiteSpace(item.crowded1.ToString()) ? 0 : int.Parse(item.crowded1.ToString());
					busInfo.DestinationName = item.routeDestName;
					busInfo.LowPlate = item.lowPlate1.ToString();
					//busInfo. = ((System.Text.Json.JsonElement)item.crowded1).ValueKind;

					bisArrivalInformation.Add(busInfo);
				}

				return bisArrivalInformation;
			}
			catch (Exception err)
			{
				var e = err;
				return bisArrivalInformation;
			}	
			
		}		

		private int GetOperationStatus(string flag, int? predictTimeSec1)
		{
			if (predictTimeSec1 is null)
			{
				return 1;
			}

			return flag switch
			{
				"RUN" => 4,
				"PASS" => 3,
				"STOP" => 2,
				"WAIT" => 1,
				_ => 1
			};
		}		

		public delegate void Socket접속상태Handler(string ip, bool connectYN);
		public static event Socket접속상태Handler OnSocket접속상태Event;		

		public delegate void Socket수신Handler(PAJU_BIS_OPCODE_TYPE type);
		public event Socket수신Handler OnSocket수신Event;

		public delegate void 도착정보수신Handler(BisExpectedArrivalInformation _item);
		public static event 도착정보수신Handler On도착정보수신Event;

		public delegate void 단말기제어수신Handler(List<PajuBisStatusControl> _item);
		public event 단말기제어수신Handler On단말기제어수신Event;

		//edgar adding weather data
		public delegate void AirQualityDataReceivedHandler(AirQualityData weatherInfo);
		public event AirQualityDataReceivedHandler OnAirQualityDataReceivedEvent;

		public delegate void WeatherDataReceivedHandler(GwacheonWeatherResponse weatherData);
		public event WeatherDataReceivedHandler OnWeatherDataReceivedEvent;
		public static void MQTTconnectionFailed()
		{			
			OnSocket접속상태Event?.Invoke(string.Empty, false);
		}		
	}
}
