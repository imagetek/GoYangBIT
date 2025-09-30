using SSCommonNET;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SSData.DashboardAPI
{
	public static class HttpService
	{
		//static HttpClientHandler handler = new HttpClientHandler
		//{
		//	ServerCertificateCustomValidationCallback = (request, cert, chain, errors) =>
		//	{
		//		var c = cert;
		//		Console.WriteLine("SSL error skipped");
		//		return true;
		//	}
		//};
		static HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://192.168.17.210") };
		static CurrentStatus _currentStatus = new() { StationId = BITDataManager.BitSystem.MOBILE_NO.ToString(), StationName = BITDataManager.BitSystem.STATION_NM };
		static string _key = "1234567890123456"; 

		static Timer statusTimer = new Timer(x => { SendStatus(); }, null, 5000, 30000);

		private static void SendStatus()
		{
			Task.Run(async () =>
			{
				await SendCurrentStatus();
			});
		}

		public static async Task LoginUser()
		{
			try
			{
				var user = new UserLogin() { Username = BITDataManager.BitSystem.MOBILE_NO.ToString(), Password = $"{BITDataManager.BitSystem.MOBILE_NO}{BITDataManager.BitSystem.BIT_ID}" };
				Message clientLogin = new()
				{
					EncryptedMessage = EncryptString(JsonSerializer.Serialize(user))
				};
				//var response = await _httpClient.PostAsJsonAsync("/loginclient", clientLogin);
				var response = await _httpClient.PostAsJsonAsync("/api/login", user);
				//var response = await _httpClient.GetAsync("https://192.168.17.212:5177/test");
				if (response.IsSuccessStatusCode)
				{
					await UpdateTokenValues(response);
					//await SendCurrentStatus();
				}
			}
			catch (Exception ee)
			{

				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			
		}

		private static async Task SendCurrentStatus()
		{
			try
			{
				_currentStatus.UpdateUnixTime = TimeProvider.System.GetUtcNow().ToUnixTimeSeconds();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenService.AccessToken);
				var response = await _httpClient.PostAsJsonAsync("/api/currentstatus", _currentStatus);

				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					await RefreshToken();
				}
			}
			catch (Exception ee)
			{

				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public static async Task RefreshToken()
		{
			try
			{
				var response = await _httpClient.PostAsJsonAsync("/api/refreshclient", new { RefreshToken = TokenService.RefreshToken });

				if (response.IsSuccessStatusCode)
				{
					await UpdateTokenValues(response);
				}
			}
			catch (Exception ee)
			{

				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			
		}

		public static void UpdateWeatherDataStatus(int status)
		{
			try
			{
				_currentStatus.WeatherStatus = status;
				_currentStatus.WeatherStatusUnixTime = TimeProvider.System.GetUtcNow().ToUnixTimeSeconds();				
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			
		}		

		public static void UpdateBusDataStatus(int status)
		{
			try
			{
				_currentStatus.BusStatus = status;
				_currentStatus.BusStatusUnixTime = TimeProvider.System.GetUtcNow().ToUnixTimeSeconds();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			
		}

		public static void UpdateSystemStatus(SSNS_ENV2_STATE state, int status)
		{
			try
			{
				_currentStatus.BitStatus = status;
				_currentStatus.BitStatusUnixTime = TimeProvider.System.GetUtcNow().ToUnixTimeSeconds();
				_currentStatus.Temparature1 = state.Temparature1;
				_currentStatus.Temparature2 = state.Temparature2;
				_currentStatus.Humidity = state.Humidity;
				_currentStatus.Luminance = state.Luminance;
				_currentStatus.Shock1 = state.Shock1;
				_currentStatus.Door = state.Door;
				_currentStatus.Illumination = state.Illumination;
				_currentStatus.BattPercent = state.BattPercent;
				_currentStatus.RemainBat = state.RemainBat;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
			
		}

		private static async Task UpdateTokenValues(HttpResponseMessage response)
		{
			var tokenInfo = await response.Content.ReadFromJsonAsync<TokenInfo>();
			TokenService.RefreshToken = tokenInfo.RefreshToken;
			TokenService.AccessToken = tokenInfo.AccessToken;
			var jwtToken = new JwtSecurityToken(tokenInfo.AccessToken);
			TokenService.ValidUntil = jwtToken.ValidTo.ToLocalTime();
		}

		public static string EncryptString(string plainText)
		{
			using (Aes aes = Aes.Create())
			{
				aes.Key = Encoding.UTF8.GetBytes(_key);
				aes.GenerateIV();

				using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
				{
					using (var ms = new MemoryStream())
					{
						ms.Write(aes.IV, 0, aes.IV.Length);
						using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
						{
							using (var sw = new StreamWriter(cs))
							{
								sw.Write(plainText);
							}
						}
						return Convert.ToBase64String(ms.ToArray());
					}
				}
			}
		}

		public record Message
		{
			public string EncryptedMessage { get; set; }
		}
	}
}
