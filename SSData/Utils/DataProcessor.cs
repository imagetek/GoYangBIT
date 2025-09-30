using SSCommonNET;
using SSData.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace SSData.Utils
{
	public static class DataProcessor
	{
		public static void GetArrivalSoonAndNormalBusData(List<BisArrivalInformation> datas, out List<BisArrivalInformation> items곧도착, out List<BisArrivalInformation> items)
		{
			items곧도착 = new List<BisArrivalInformation>();
			items = new List<BisArrivalInformation>();
			try
			{
				if (datas?.Count > 0)
				{
					items곧도착 = BITDataManager.Get곧도착예정정보(datas);
					items = BITDataManager.Get도착정보목록(datas);// new List<BisArrivalInformation>();
				}
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public static void GetBusImageName(int routeType, ref string 노선종류, ref string 노선색상, string routeNumber)
		{
			switch (routeType)
			{
				//edgar test, remove later or find mapping to actual values
				case 1: 노선종류 = "직행버스.png"; 노선색상 = "#ff492a"; break;
				case 2: 노선종류 = "일반버스.png"; 노선색상 = "#008aec"; break;
				case 3: 노선종류 = "마을버스G.png"; 노선색상 = "#00b223"; break;

				//case 11: 노선종류 = "직행버스.png"; 노선색상 = "#ff492a"; break;
				//case 12: 노선종류 = "좌석버스.png"; 노선색상 = "#008aec"; break;
				//case 13: 노선종류 = "일반버스.png"; 노선색상 = "#00aebc"; break;
				//case 14: 노선종류 = "광역버스.png"; 노선색상 = "#f84729"; break;
				//case 21: 노선종류 = "직행버스.png"; 노선색상 = "#ff492a"; break;
				//case 22: 노선종류 = "시외버스.png"; 노선색상 = "#ff492a"; break;

				//case 23: 노선종류 = "마을버스G.png"; 노선색상 = "#00b223"; break;
				//case 30: 노선종류 = "마을버스Y.png"; 노선색상 = "#FF8000"; break;
				//case 41: 노선종류 = "광역버스.png"; 노선색상 = "#f84729"; break;
				//case 42: 노선종류 = "일반버스.png"; 노선색상 = "#00aebc"; break;
				//case 43: 노선종류 = "시외버스.png"; 노선색상 = "#ff492a"; break;
				//case 99: 노선종류 = "공항버스.png"; 노선색상 = "#8d6c58"; break;

				//case 51: 노선종류 = "공항버스.png"; 노선색상 = "#8d6c58"; break;
				//case 53: 노선종류 = "공항버스.png"; 노선색상 = "#8d6c58"; break;
				default:
					//노선종류 = "일반버스.png"; 노선색상 = "#008aec";
					WriteToLog(routeType, routeNumber);
					break;
			}
		}

		private static void WriteToLog(int routeType, string routeNumber)
		{
			try
			{
				string message = $"Unmapped route type. Route Type: {routeType}, Route Number: {routeNumber}\n";
				string logFileName = "Unmapped_route_type.txt";
				var currentDir = Directory.GetCurrentDirectory();

				string filePath = Path.Combine(currentDir, "LOG", logFileName);

				if (File.Exists(filePath) && File.ReadAllText(filePath).Contains(routeNumber))
				{
					return;
				}
				File.AppendAllText(filePath, message);
			}
			catch (Exception ee)
			{
				LogWriterService.WriteToLog(ee);
			}
		}

		public static void SaveDataToJsonFile<T>(T data, string fileName)
		{
			try
			{
				JsonSerializerOptions options = new()
				{
					Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
				};
				string jsonData = JsonSerializer.Serialize(data, options);
				string pathToJsonFile = Path.Combine(Directory.GetCurrentDirectory(), "data", fileName);
				File.WriteAllTextAsync(pathToJsonFile, jsonData);
			}
			catch (Exception ee)
			{
				LogWriterService.WriteToLog(ee);
			}
			
		}

		public static T GetJsonSettings<T>(string fileName) where T : new()
		{
			try
			{
				var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "data", fileName);
				T settings = JsonSerializer.Deserialize<T>(File.ReadAllText(jsonPath));
				return settings;
			}
			catch (Exception ee)
			{
				LogWriterService.WriteToLog(ee);
				return new T();
			}

		}

		//public static void WriteToJsonSettings(Settings _settings)
		//{
		//	try
		//	{
		//		var json = JsonSerializer.Serialize(_settings);
		//		var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "Settings.json");
		//		File.WriteAllText(jsonPath, json);
		//	}
		//	catch (Exception ee)
		//	{
		//		LogWriterService.WriteToLog(ee);
		//	}

		//}
	}
}
