using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SSData.ClientEngine;

namespace SSData.GwachonAPI
{
	public class GwacheonBusStationService
	{
		public static List<RouteInfo> StationRouteMap = [];
		public static List<BusLocation> BusLocations = [];

		public static void AddNewRoute(RouteInfo routeInfo)
		{
			var currentRouteInfo = StationRouteMap.FirstOrDefault(r => r.RouteId == routeInfo.RouteId);
			if (currentRouteInfo is null || currentRouteInfo.StationList?.Count == 0)
			{
				Task.Run(async () =>
				{
					routeInfo.StationList.AddRange(await GetRouteStations(routeInfo.RouteId));
					StationRouteMap?.Add(routeInfo);
				});

			}
		}

		internal static void UpdateBusLocations(BusLocation busLocation)
		{
			var locationInfo = BusLocations.FirstOrDefault(l => l.RouteId == busLocation.RouteId);

			if (locationInfo is null)
			{
				BusLocations.Add(busLocation);
			}
			else
			{
				locationInfo.Locations.Clear();
				locationInfo.Locations.AddRange(busLocation.Locations);
			}
		}

		private static async Task<List<Station>> GetRouteStations(int routeId)
		{
			try
			{
				return await ClientEngine.GetStationsForRouteId(routeId);
			}
			catch (Exception ex)
			{
				return [];
			}
		}
	}

	public class RouteInfo
	{
		public string RouteName { get; set; }
		public int RouteId { get; set; }
		public List<Station> StationList { get; set; } = [];
	}
}
