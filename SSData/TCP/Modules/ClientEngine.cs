using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using SSData.GwachonAPI;
using SSData.DashboardAPI;
using SSCommonNET;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Security.Policy;
using System.Net;
using System.Xml.Linq;
using System.Linq;
using MQTTnet.Internal;
using System.Text.Json.Nodes;

namespace SSData
{
	public class ClientEngine
	{

		Timer gwacheonDataTimer;
		Timer gwacheonAirQualityTimer;
		Timer gwacheonWeatherTimer;
		static string serviceKeyPrivate = "08ixNTCwWo%2BfQRSX6bz8EADns5hSePqPJCSag4%2F1zLOtmwgHI%2F6FPP2fCq%2FZq4p6uzBQdj%2BG1wWhLo%2FmYxcvyg%3D%3D";
		static string serviceKey = "gvJ3930cxcONJYzeeYvlnTcw4mG2dezOE9EPI2NwLqr2mworZ2zC7igmX3Nu1kOUsOtSsNmNojrurmd0KltqFw%3D%3D";

		public ClientEngine()
		{
			InitProc();
		}

		private void InitProc()
		{
			gwacheonDataTimer ??= new Timer(async x => { await GetGwachonBusDataAsync();  }, null, 5000, 30000);    //pjh canceled [change 30sec to 60sec : Insufficient data capacity]
			gwacheonAirQualityTimer ??= new Timer(async x => { await GetGwachonAirQualityData(); }, null, 15000, 60 * 60 * 1000);
			gwacheonWeatherTimer ??= new Timer(async x => { await GetGwacheonWeatherData(); }, null, 10000, 60 * 60 * 1000);
		}

		private static HttpClient _httpClient = new HttpClient();

		public async Task GetBusLocationsForRouteId(int routeId)
		{
			List<int> locationIds = [];
			try
			{
				//string endpoint = $"https://api.gbis.go.kr/ws/rest/buslocationservice?serviceKey=1234567890&routeId={routeId}";
				string endpoint = $"https://apis.data.go.kr/6410000/buslocationservice/v2/getBusLocationListv2?serviceKey={serviceKey}&routeId={routeId}&format=xml";
				var response = await _httpClient!.GetAsync(endpoint);

				if (response.IsSuccessStatusCode)
				{
					var rdata = await response.Content.ReadAsStringAsync();
					XDocument doc = XDocument.Parse(rdata);
					//data = JsonSerializer.Deserialize<GwachonApiResponse>(rdata);
					//HttpService.UpdateBusDataStatus(1);
					locationIds = doc.Descendants("busLocationList")
							 .Select(x => int.Parse(x.Element("stationSeq")?.Value))
							 .ToList();
				}

			}
			catch (Exception ex)
			{
				var error = ex.Message;
			}
			finally
			{
				OnBusLocationReceivedEvent?.Invoke(new BusLocation() { RouteId = routeId, Locations = locationIds});
			}
			
		}
		public static async Task<List<Station>> GetStationsForRouteId(int routeId)
		{
			try
			{
				//no access
				string endpoint = $"https://apis.data.go.kr/6410000/busrouteservice/v2/getBusRouteStationListv2?serviceKey={serviceKey}&routeId={routeId}&format=xml";
				//string endpoint = $"https://api.gbis.go.kr/ws/rest/busrouteservice/station?serviceKey=1234567890&routeId={routeId}";

				var response = await _httpClient!.GetAsync(endpoint);

				if (response.IsSuccessStatusCode)
				{
					var rdata = await response.Content.ReadAsStringAsync();
					var i = 1;
					XDocument doc = XDocument.Parse(rdata);
					//data = JsonSerializer.Deserialize<GwachonApiResponse>(rdata);
					//HttpService.UpdateBusDataStatus(1);
					List<Station> stations = doc.Descendants("busRouteStationList")
							 .Select(x => new Station() { StationName = x.Element("stationName")?.Value, StationSequence = int.Parse(x.Element("stationSeq").Value), StationId = x.Element("stationId")?.Value })
							 .ToList();

				
					return stations;
				}

				return [];
			}
			catch (Exception ex)
			{
				var error = ex.Message;
				return [];
			}
			
		}

		private async Task GetGwachonBusDataAsync()
		{
			List<BusArrivalList> data = [];
			try
			{
				string stationId = BITDataManager.BitSystem.STATION_ID; //"119000074";// BITDataManager.BitSystem.STATION_ID;//"220000112";
				string endpoint = $"https://apis.data.go.kr/6410000/busarrivalservice/v2/getBusArrivalListv2?serviceKey={serviceKey}&stationId={stationId}&format=json";

				var response = await _httpClient!.GetAsync(endpoint);

				if (response.IsSuccessStatusCode)
				{
					var rdata = await response.Content.ReadAsStringAsync();
					var result = JsonSerializer.Deserialize<GwachonApiResponse>(rdata);
					HttpService.UpdateBusDataStatus(1);


					if (result.response.msgBody.busArrivalList.GetType() != typeof(JsonArray))
					{
						var listResult = result.response.msgBody.busArrivalList.Deserialize<BusArrivalList>();
						data.Add(listResult);
					}
					else
					{
						data.AddRange(result.response.msgBody.busArrivalList.Deserialize<List<BusArrivalList>>());
						
					}
					data.ForEach(async a => await GetBusLocationsForRouteId(a.routeId));
				}
			}
			catch (Exception ee)
			{
				var error = ee;

				HttpService.UpdateBusDataStatus(0);
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			finally 
			{
				OnReceivedEvent.Invoke(data);
			}
		}

		private async Task GetGwachonAirQualityData()
		{
			//pjh change stationName to 별양동(gwacheon city) from 영등포구(yoido)
			string endpoint = $"http://apis.data.go.kr/B552584/ArpltnInforInqireSvc/getMsrstnAcctoRltmMesureDnsty?stationName=별양동&dataTerm=DAILY&pageNo=1&numOfRows=1&returnType=json&serviceKey=08ixNTCwWo%2BfQRSX6bz8EADns5hSePqPJCSag4%2F1zLOtmwgHI%2F6FPP2fCq%2FZq4p6uzBQdj%2BG1wWhLo%2FmYxcvyg%3D%3D&ver=1.3";

			try
			{
				var response = await _httpClient!.GetAsync(endpoint);

				if (response.IsSuccessStatusCode)
				{
					string rdata = await response.Content.ReadAsStringAsync();
					GwachonAirQualityResponse gwachonWeatherResponse = JsonSerializer.Deserialize<GwachonAirQualityResponse>(rdata);
					OnAirQualityReceivedEvent.Invoke(gwachonWeatherResponse);
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}			
		}

		private async Task GetGwacheonWeatherData()
		{
			//1. first get total count of items,
			//2. then get all items forecast is 3 days
			//3. get temperatures for the day and show min temperature and max temperature
			GwacheonWeatherResponse gwachonWeatherResponse = new();

			try
			{
				var date = DateTime.Now;
				var baseDate = date.ToString("yyyyMMdd");
				string baseTime = GetBaseTime(date);

				string url = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getVilageFcst"; // URL
				url += $"?ServiceKey={serviceKeyPrivate}"; // Service Key
				url += "&pageNo=1";
				url += "&numOfRows=1000";
				url += "&dataType=JSON";
				url += $"&base_date={baseDate}";
				url += $"&base_time={baseTime}";
				url += "&nx=60";
				url += "&ny=127";

				var response = await _httpClient!.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var rdata = await response.Content.ReadAsStringAsync();
					gwachonWeatherResponse = JsonSerializer.Deserialize<GwacheonWeatherResponse>(rdata);
					HttpService.UpdateWeatherDataStatus(1);
				}
			}
			catch (Exception ee)
			{
				var error = ee;
				HttpService.UpdateWeatherDataStatus(0);
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			finally
			{
				if (gwachonWeatherResponse?.Response?.Body is not null && gwachonWeatherResponse?.Response?.Body?.Items?.Item?.Count > 0)
				{
					OnWeatherReceivedEvent?.Invoke(gwachonWeatherResponse);
				}
			}
		}

		private static string GetBaseTime(DateTime date)
		{
			string baseTime = "";

			switch (date.Hour)
			{
				case 0:
				case 1:
				case 2:
					baseTime = "2300";
					break;
				case 3:
				case 4:
				case 5:
					baseTime = "0200";
					break;
				case 6:
				case 7:
				case 8:
					baseTime = "0500";
					break;
				case 9:
				case 10:
				case 11:
					baseTime = "0800";
					break;
				case 12:
				case 13:
				case 14:
					baseTime = "1100";
					break;
				case 15:
				case 16:
				case 17:
					baseTime = "1400";
					break;
				case 18:
				case 19:
				case 20:
					baseTime = "1700";
					break;
				case 21:
				case 22:
				case 23:
					baseTime = "2000";
					break;
				default:
					break;
			}

			return baseTime;
		}
	

		public delegate void ConnectFailedHandler(Exception ex);
		public event ConnectFailedHandler OnConnectFailedEvent;

		
		public delegate void ReceivedHandler(List<BusArrivalList> busArrivalList);
		public event ReceivedHandler OnReceivedEvent;

		public delegate void BusLocationReceivedHandler(BusLocation busLocation);
		public event BusLocationReceivedHandler OnBusLocationReceivedEvent;

		public delegate void AirQualityReceivedHandler(GwachonAirQualityResponse gwachonWeatherResponse);
		public event AirQualityReceivedHandler OnAirQualityReceivedEvent;

		public delegate void WeatherReceivedHandler(GwacheonWeatherResponse gwachonWeatherResponse);
		public event WeatherReceivedHandler OnWeatherReceivedEvent;


		public record Station
		{
			public int StationSequence { get; set; }
			public string StationName { get; set; }
			public string StationId { get; set; }
		}

		public record BusLocation
		{
			public int RouteId { get; set; }
			public List<int> Locations { get; set; }
		}
	}
}