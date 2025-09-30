using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using SSCommonNET;
using System.Net.Http;
using MQTTnet.Client;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SSData.MQTT;
public class MQTTService
{
	public static IMqttClient mqttClient;

	private static HttpClient sharedClient = new()
	{
		BaseAddress = new Uri($"http://{BITDataManager.BitSystem.HTTP_URL}/bitdn/public/"),
	};

	public static void ReconnectUsingTimer(MqttClientOptions mqttClientOptions)
	{
		/*
         * This sample shows how to reconnect when the connection was dropped.
         * This approach uses a custom Task/Thread which will monitor the connection status.
         * This is the recommended way but requires more custom code!
         */

		var mqttFactory = new MqttFactory();

		mqttClient = mqttFactory.CreateMqttClient();

		_ = Task.Run(
			async () =>
			{
				// User proper cancellation and no while(true).
				while (true)
				{
					try
					{
						// This code will also do the very first connect! So no call to _ConnectAsync_ is required in the first place.
						if (!await mqttClient.TryPingAsync())
						{
							var result = await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

							if (result.ResultCode == MqttClientConnectResultCode.Success)
							{
								// Subscribe to topics when session is clean etc.
								Log4Manager.WriteSocketLog(Log4Level.Debug, string.Format("서버에 접속되었습니다. {0}", BITDataManager.BitSystem.SERVER_URL));

								SubscribeToTopic();
							}
							else
							{
								BISManager.MQTTconnectionFailed();
							}
						}
					}
					catch (Exception ee)
					{
						BISManager.MQTTconnectionFailed();
					}
					finally
					{
						// Check the connection state every 60 seconds and perform a reconnect if required.
						await Task.Delay(TimeSpan.FromSeconds(60));
					}
				}
			});
	}

	private static void SubscribeToTopic()
	{
		BISManager pAJUBISManager = new BISManager();
		string topicAll = $"+/bitctl/BIT0000{BITDataManager.BIT_ID}/#";
		string topic4400 = "+/bitctl/0x4400";
		var resultAll = mqttClient.SubscribeAsync(topicAll).Result;

		var result4400 = mqttClient.SubscribeAsync(topic4400).Result;

		//mqttClient.ApplicationMessageReceivedAsync += e =>
		//{
		//	Console.WriteLine(JsonSerializer.Serialize(e));

		//	var segment = e.ApplicationMessage.PayloadSegment;
		//	string topic = e.ApplicationMessage.Topic;


		//	if (topic.Contains("3400"))
		//	{
		//		pAJUBISManager.ProcessMQTTMessage(segment.ToArray(), 3400);
		//	}
		//	else if (topic.Contains("3300"))
		//	{
		//		pAJUBISManager.ProcessMQTTMessage(segment.ToArray(), 3300);
		//	}
		//	else if (topic.Contains("3100"))
		//	{
		//		pAJUBISManager.ProcessMQTTMessage(segment.ToArray(), 3100);
		//	}

		//	return Task.CompletedTask;
		//};
	}

	public static void SaveImageDataFromBytes(byte[] messageBytes)
	{
		List<ImageData> imageDataList = [];
		byte[][] startBytes = [[20, 0, 0, 0]]; //[[255, 255]];
		var imageIndexes = SearchForStart(messageBytes, startBytes);

		if (imageIndexes?.Count == 0)
		{
			return;
		}
		int imageIndex = 0;
		foreach (var index in imageIndexes!)
		{
			int startIndex = index;

			if (index == 0)
			{
				continue;
			}

			ImageData imageData = new ImageData();
			imageData.Index = imageIndex;
			//display_index
			startIndex += 7; //11;
			imageData.DisplayMethod = messageBytes[startIndex];

			//display_time
			startIndex++;
			imageData.DisplayTime = messageBytes[startIndex];

			//public_type
			startIndex += 3;
			imageData.PublicType = messageBytes[startIndex];

			//file_name
			startIndex += 9; //buffer size for image file name

			int bufferSize = messageBytes[startIndex];
			byte[] buffer = new byte[bufferSize];
			startIndex += 4; //beginning of file name
			Buffer.BlockCopy(messageBytes, startIndex, buffer, 0, bufferSize);
			imageData.ImageName = Encoding.UTF8.GetString(buffer);

			//end date
			startIndex += bufferSize;
			while (messageBytes[startIndex] == 0) { startIndex++; }

			bufferSize = messageBytes[startIndex];
			buffer = new byte[bufferSize];
			startIndex += 4;
			Buffer.BlockCopy(messageBytes, startIndex, buffer, 0, bufferSize);
			imageData.DisplayEndTime = Encoding.UTF8.GetString(buffer);

			//start date
			startIndex += bufferSize;
			while (messageBytes[startIndex] == 0) { startIndex++; }

			bufferSize = messageBytes[startIndex];
			buffer = new byte[bufferSize];
			startIndex += 4;
			Buffer.BlockCopy(messageBytes, startIndex, buffer, 0, bufferSize);
			imageData.DisplayStartTime = Encoding.UTF8.GetString(buffer);

			Console.WriteLine(imageData);
			imageDataList.Add(imageData);
			imageIndex++;
		}

		SaveImageDataToFile(imageDataList);

		DownLoadMissingImages(imageDataList);
	}

	public static void DownLoadMissingImages(List<ImageData> imageDataList)
	{
		var tasks = new List<Task>();

		string promotionFolder = Path.Combine(Directory.GetCurrentDirectory(), "Promotion");
		foreach (ImageData imageData in imageDataList)
		{

			string imagePath = Path.Combine(promotionFolder, imageData.ImageName);
			if (!File.Exists(imagePath))
			{
				tasks.Add(Task.Run(async () =>
				{
					await DownloadImage(imageData.ImageName, promotionFolder);
				}));
			}
		}

		Task task = Task.WhenAll(tasks);

		try
		{
			task.Wait();

		}
		catch (Exception ee)
		{
			TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
		}
	}

	private static async Task DownloadImage(string imageName, string promotionFolder)
	{
		var imageBytes = await sharedClient.GetByteArrayAsync(imageName);

		string path = Path.Combine(promotionFolder, imageName);
		await File.WriteAllBytesAsync(path, imageBytes);
	}

	private static void SaveImageDataToFile(List<ImageData> imageDataList)
	{
		string data = JsonSerializer.Serialize(imageDataList);
		string fileName = "image_data.json";
		string folderName = "Promotion";
		string path = Path.Combine(Directory.GetCurrentDirectory(), folderName);
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		string imagePath = Path.Combine(path, fileName);

		File.WriteAllText(imagePath, data);
	}

	public static List<BisArrivalInformation> GetArrivalInfoFromBytes(byte[] messageBytes)
	{
		List<BisArrivalInformation> arrivalInfo = [];
		byte[][] posibbleStartsShort = [[42, 0, 0, 0], [255, 255]];
		var indexOfStarts = SearchForStart(messageBytes, posibbleStartsShort);

		if (indexOfStarts.Count == 0)
		{
			return arrivalInfo;
		}

		foreach (var index in indexOfStarts)
		{
			if (index == 0)
			{
				continue;
			}

			int startIndex = index;

			byte singleByte;
			byte[] uint16Byte = new byte[2];
			byte[] uint32Byte = new byte[4];
			byte[] timeBuffer = new byte[8];

			BisArrivalInformation info = new();
			//car_interval
			startIndex += 6;
			Buffer.BlockCopy(messageBytes, startIndex, uint16Byte, 0, 2);
			ushort carInterval = BitConverter.ToUInt16(uint16Byte);

			startIndex += 2;
			int indexOfArrivalStatus = messageBytes[startIndex];

			//seat_type
			startIndex += indexOfArrivalStatus == 52 ? 11 : indexOfArrivalStatus == 48 ? 9 : 0; //52 normal status, 48 arriving soon, 44/24 not operating
			singleByte = messageBytes[startIndex];

			info.BusCongestion = indexOfArrivalStatus == 40 || indexOfArrivalStatus == 24 ? 0 : singleByte;
			info.NumberOfRemainingSeats = singleByte == 5 ? messageBytes[startIndex - 1] : 255;

			//remain_time
			startIndex += indexOfArrivalStatus == 52 ? 1 : indexOfArrivalStatus == 48 ? 2 : 0; //52 normal status, 48 arriving soon, 44/24 not operating
			Buffer.BlockCopy(messageBytes, startIndex, uint16Byte, 0, 2);
			ushort remainingTime = indexOfArrivalStatus == 52 ? BitConverter.ToUInt16(uint16Byte) : (ushort)0;

			info.EstimatedTimeOfArrival = remainingTime;

			//remain_sttn_count
			ushort remainingStops = 0;
			if (indexOfArrivalStatus == 52)
			{
				startIndex = startIndex + 2;
				Buffer.BlockCopy(messageBytes, startIndex, uint16Byte, 0, 2);
				remainingStops = BitConverter.ToUInt16(uint16Byte);

			}
			else if (indexOfArrivalStatus == 48)
			{
				startIndex = startIndex - 1;
				remainingStops = messageBytes[startIndex];
			}

			info.NumberOfRemainingStops = remainingStops;

			//sttn_id
			if (indexOfArrivalStatus != 24)
			{
				startIndex += indexOfArrivalStatus == 44 || indexOfArrivalStatus == 40 ? 12 : 6;
				Buffer.BlockCopy(messageBytes, startIndex, uint32Byte, 0, 4);
				uint stationId = BitConverter.ToUInt32(uint32Byte);

				info.StopId = (int)stationId;
			}



			//operate_mode
			if (indexOfArrivalStatus != 24)
			{
				if (indexOfArrivalStatus != 40)
				{
					startIndex += 7;
					singleByte = messageBytes[startIndex];

					if (singleByte == 1)
					{
						info.BusType = 1;
						info.OperationStatus = messageBytes[startIndex - 1];
					}
					else
					{
						info.BusType = 0;
						info.OperationStatus = singleByte;
					}
				}
			}
			else
			{
				info.BusType = 0;
				info.OperationStatus = -1;
			}

			if (info.OperationStatus == 255)
			{
				info.OperationStatus = 0;
			}

			//veh_id
			if (indexOfArrivalStatus != 24)
			{
				startIndex += indexOfArrivalStatus == 40 ? 8 : 5;
				Buffer.BlockCopy(messageBytes, startIndex, uint32Byte, 0, 4);
				uint vehId = BitConverter.ToUInt32(uint32Byte);

				info.VehicleNumber = (int)vehId;
			}


			//route_type
			startIndex += indexOfArrivalStatus == 24 ? 15 : 11;
			singleByte = messageBytes[startIndex];

			info.RouteType = singleByte;

			//route_id
			startIndex += 5;
			Buffer.BlockCopy(messageBytes, startIndex, uint32Byte, 0, 4);
			uint routeId = BitConverter.ToUInt32(uint32Byte);

			info.RouteId = (int)routeId;

			//last_car_time		
			startIndex += 8;
			Buffer.BlockCopy(messageBytes, startIndex, timeBuffer, 0, 8);
			string lastCarTime = Encoding.UTF8.GetString(timeBuffer).Trim();



			//first_car_time
			startIndex += 12;
			Buffer.BlockCopy(messageBytes, startIndex, timeBuffer, 0, 8);
			string firstCarTime = Encoding.UTF8.GetString(timeBuffer).Trim();

			//sttn_name
			startIndex += 12;
			int stringByteSize = messageBytes[startIndex - 4];


			byte[] stringBuffer = new byte[stringByteSize];
			Buffer.BlockCopy(messageBytes, startIndex, stringBuffer, 0, stringByteSize);
			string stationName = Encoding.UTF8.GetString(stringBuffer).Trim();

			info.StopName = stationName;

			if (indexOfArrivalStatus != 24)
			{
				//veh_no
				stringBuffer = UpdateStartIndexAndTempBuffer(messageBytes, ref startIndex, ref stringByteSize);
				startIndex += 4;
				Buffer.BlockCopy(messageBytes, startIndex, stringBuffer, 0, stringByteSize);
				string vehNo = Encoding.UTF8.GetString(stringBuffer).Trim();

				//route_direction				
				stringBuffer = UpdateStartIndexAndTempBuffer(messageBytes, ref startIndex, ref stringByteSize);
				startIndex += 4;
				Buffer.BlockCopy(messageBytes, startIndex, stringBuffer, 0, stringByteSize);
				string routeDirection = Encoding.UTF8.GetString(stringBuffer).Trim();

				info.DestinationName = routeDirection;
			}


			//route_name
			stringBuffer = UpdateStartIndexAndTempBuffer(messageBytes, ref startIndex, ref stringByteSize);
			startIndex += 4;
			Buffer.BlockCopy(messageBytes, startIndex, stringBuffer, 0, stringByteSize);
			string routeName = Encoding.UTF8.GetString(stringBuffer).Trim();

			info.RouteNumberT = routeName;
			arrivalInfo.Add(info);
		}

		return arrivalInfo;
	}

	public static string GetTimeInfoFromBytes(byte[] messageBytes)
	{
		byte[] startBytes = [17, 0, 0, 0];
		int startIndex = SearchBytes(messageBytes, startBytes).FirstOrDefault();

		int bufferSize = messageBytes[startIndex];
		byte[] buffer = new byte[bufferSize];

		startIndex += 4;
		Buffer.BlockCopy(messageBytes, startIndex, buffer, 0, bufferSize);

		string time = Encoding.UTF8.GetString(buffer);
		return time;
	}

	public static AirQualityData GetWeatherInfoFromBytes(byte[] messageBytes)
	{
		AirQualityData weatherData = new();
		try
		{
			//TODO need to check weather data more. if all is good below startBytes don't ex
			byte[] startBytes = [0, 0, 0, 6];
			int bufferSize = 0;
			byte[] buffer = new byte[bufferSize];
			int startIndex = SearchBytes(messageBytes, startBytes).FirstOrDefault();

			if (startIndex == 0)
			{
				return null;
			}

			startIndex += 8;
			int offset = messageBytes[startIndex];

			startIndex += offset;

			byte[] o3Index = [26, 0, 0, 0, 0, 0, 0];
			byte[] pm10Index = [44, 0, 0, 0, 0, 0, 0];
			byte[] pm25Index = [40, 0, 0, 0, 0, 0, 0];


			int o3Start = SearchBytes(messageBytes, o3Index).FirstOrDefault();
			weatherData.Ozone.Index = o3Start == 0 ? 0 : messageBytes[o3Start + 7];

			int pm10Start = SearchBytes(messageBytes, pm10Index).FirstOrDefault();
			weatherData.FineDust.Index = pm10Start == 0 ? 0 : messageBytes[pm10Start + 7];

			int pm25Start = SearchBytes(messageBytes, pm25Index).FirstOrDefault();
			weatherData.UltraFineDust.Index = pm25Start == 0 ? 0 : messageBytes[pm25Start + 7];

			var start = messageBytes[startIndex];
			var start4 = messageBytes[startIndex + 4];
			var start8 = messageBytes[startIndex + 8];

			startIndex += messageBytes[startIndex] == 24 ? messageBytes[startIndex + 4] + 4 : messageBytes[startIndex + 8] + 8;

			//o3_value
			bufferSize = messageBytes[startIndex];
			buffer = new byte[bufferSize];
			startIndex += 4;
			Buffer.BlockCopy(messageBytes, startIndex, buffer, 0, bufferSize);
			string o3Value = Encoding.UTF8.GetString(buffer).Trim();
			//Console.WriteLine($"o3_value: {o3Value}");
			weatherData.Ozone.ActualValue = o3Value;

			//o3_base
			buffer = UpdateStartIndexAndTempBuffer(messageBytes, ref startIndex, ref bufferSize);
			startIndex += 4;
			Buffer.BlockCopy(messageBytes, startIndex, buffer, 0, bufferSize);
			string o3Base = Encoding.UTF8.GetString(buffer).Trim();
			//Console.WriteLine($"o3_base: {o3Base}");
			weatherData.Ozone.BaseValue = o3Base;

			//pm25_value
			buffer = UpdateStartIndexAndTempBuffer(messageBytes, ref startIndex, ref bufferSize);
			startIndex += 4;
			Buffer.BlockCopy(messageBytes, startIndex, buffer, 0, bufferSize);
			string pm25_value = Encoding.UTF8.GetString(buffer).Trim();
			//Console.WriteLine($"pm25_value: {pm25_value}");
			weatherData.UltraFineDust.ActualValue = pm25_value;

			//pm25_base
			buffer = UpdateStartIndexAndTempBuffer(messageBytes, ref startIndex, ref bufferSize);
			startIndex += 4;
			Buffer.BlockCopy(messageBytes, startIndex, buffer, 0, bufferSize);
			string pm25_base = Encoding.UTF8.GetString(buffer).Trim();
			//Console.WriteLine($"pm25_base: {pm25_base}");
			weatherData.UltraFineDust.BaseValue = pm25_base;

			//pm10_value
			buffer = UpdateStartIndexAndTempBuffer(messageBytes, ref startIndex, ref bufferSize);
			startIndex += 4;
			Buffer.BlockCopy(messageBytes, startIndex, buffer, 0, bufferSize);
			string pm10_value = Encoding.UTF8.GetString(buffer).Trim();
			//Console.WriteLine($"o3_base: {pm10_value}");
			weatherData.FineDust.ActualValue = pm10_value;

			//pm10_base
			buffer = UpdateStartIndexAndTempBuffer(messageBytes, ref startIndex, ref bufferSize);
			startIndex += 4;
			Buffer.BlockCopy(messageBytes, startIndex, buffer, 0, bufferSize);
			string pm10_base = Encoding.UTF8.GetString(buffer).Trim();
			//Console.WriteLine($"o3_base: {pm10_base}");
			weatherData.FineDust.BaseValue = pm10_base;

			//if (tempExists)
			//{
			//	//edgar, need to think if we should add it
			//	//temperature
			//	buffer = UpdateStartIndexAndTempBuffer(messageBytes, ref startIndex, ref bufferSize);
			//	startIndex += 4;
			//	Buffer.BlockCopy(messageBytes, startIndex, buffer, 0, bufferSize);
			//	string temperature = Encoding.UTF8.GetString(buffer).Trim();
			//	//Console.WriteLine($"temperature: {temperature}");            
			//}


		}
		catch (Exception ee)
		{
			TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			return null;
		}

		return weatherData;
	}

	static byte[] UpdateStartIndexAndTempBuffer(byte[] messageBytes, ref int startIndex, ref int stringByteSize)
	{
		startIndex = startIndex + stringByteSize;
		startIndex = SkipZeroValues(messageBytes, startIndex);
		stringByteSize = messageBytes[startIndex];
		return new byte[stringByteSize];
	}

	static int SkipZeroValues(byte[] messageBytes, int startIndex)
	{
		while (messageBytes[startIndex] == 0) { startIndex++; }

		return startIndex;
	}

	static int[] SearchBytes(byte[] haystack, byte[] needle)
	{
		List<int> found = [];
		int foundIndex = 0;
		var len = needle.Length;
		var limit = haystack.Length - len;
		for (var i = 0; i <= limit; i++)
		{
			var k = 0;
			for (; k < len; k++)
			{
				if (needle[k] != haystack[i + k]) break;
			}
			if (k == len)
			{
				//found[foundIndex] = i;           
				//         foundIndex++;
				found.Add(i);
			}
		}
		return found.ToArray();
	}

	static List<int> SearchForStart(byte[] haystack, byte[][] needles)
	{
		List<int> found = [];
		var limit = haystack.Length;
		foreach (var needle in needles)
		{
			for (int i = 0; i < limit; i += 4)
			{
				if (needle.Length == 4)
				{
					if (haystack[i] == needle[0]
						&& haystack[i + 1] == needle[1]
						&& haystack[i + 2] == needle[2]
						&& haystack[i + 3] == needle[3])
					{
						found.Add(i);
					}
				}
				else if (needle.Length == 2)
				{
					if (haystack[i + 2] == needle[0]
						&& haystack[i + 3] == needle[1])
					{
						found.Add(i);
					}
				}
			}
		}

		return found;
	}
}
