using SSCommonNET;
using SSData.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SSData
{
	public static class SettingsManager
	{
		private static readonly Properties.Settings _settings = Properties.Settings.Default;
		private const string _bitSystem = "BitSystem.json";
		private const string _tbSystem = "BitConfig.json";
		private const string _bitEnvSystem = "BitEnvConfig.json";
		public static BIT_SYSTEM GetBitSystemSettings()
		{
			//return ReadBitSettingFromApplicationSettings() ?? ReadBitSettingFromJsonFile();
			return ReadBitSettingFromJsonFile();
		}

		private static BIT_SYSTEM ReadBitSettingFromJsonFile()
		{
			BIT_SYSTEM bitSystem;
			try
			{
				bitSystem = DataProcessor.GetJsonSettings<BIT_SYSTEM>(_bitSystem);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				bitSystem = null;
			}

			return bitSystem;
		}

		private static BIT_SYSTEM ReadBitSettingFromApplicationSettings()
		{
			BIT_SYSTEM bitSystem = new();
			try
			{				
				bitSystem.BIT_ID = _settings.BIT_ID;
				bitSystem.SERVER_URL = _settings.SERVER_URL;
				bitSystem.SEQ_NO = GetIntValue(_settings.SEQ_NO);
				bitSystem.MOBILE_NO = GetIntValue(_settings.MOBILE_NO);
				bitSystem.STATION_NM = _settings.STATION_NM;
				bitSystem.SERVER_TYPE = GetIntValue(_settings.SERVER_TYPE);
				bitSystem.SERVER_PORT = _settings.SERVER_PORT;
				bitSystem.FTP_GBN = GetIntValue(_settings.FTP_GBN);
				bitSystem.FTP_IP = _settings.FTP_IP;
				bitSystem.FTP_PORT = GetIntValue(_settings.FTP_PORT);
				bitSystem.FTP_USERID = _settings.FTP_USERID;
				bitSystem.FTP_USERPWD = _settings.FTP_USERPWD;
				bitSystem.HTTP_URL = _settings.HTTP_URL;
				bitSystem.HTTP_PORT = GetIntValue(_settings.HTTP_PORT);
				bitSystem.ENV_GBN = GetIntValue(_settings.ENV_GBN);
				bitSystem.ENV_PORT_NM = _settings.ENV_PORT_NM;
				bitSystem.ENV_BAUD_RATE = GetIntValue(_settings.ENV_BAUD_RATE);
				bitSystem.CAM_NO1 = GetIntValue(_settings.CAM_NO1);
				bitSystem.CAM_NO1_ROTATE = GetIntValue(_settings.CAM_NO1_ROTATE);
				bitSystem.CAM_NO2 = GetIntValue(_settings.CAM_NO2);
				bitSystem.CAM_NO2_ROTATE = GetIntValue(_settings.CAM_NO2_ROTATE);
				//bitSystem.REGDATE = GetDateTimeValue(_settings.REGDATE);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				bitSystem = null;
			}

			return bitSystem;
		}

		public static bool SaveBitSystemSettings(BIT_SYSTEM bitSystem)
		{
			try
			{
				//SaveBitSystemSettingsToApplicationSettings(bitSystem);
				SaveBitSystemSettingsToJsonFile(bitSystem);
				return true;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			
		}

		private static void SaveBitSystemSettingsToJsonFile(BIT_SYSTEM bitSystem)
		{
			try
			{
				DataProcessor.SaveDataToJsonFile(bitSystem, _bitSystem);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private static void SaveBitSystemSettingsToApplicationSettings(BIT_SYSTEM bitSystem)
		{
			try
			{
				_settings.BIT_ID = bitSystem.BIT_ID.ToString();
				_settings.SERVER_URL = bitSystem.SERVER_URL;
				_settings.SEQ_NO = bitSystem.SEQ_NO.ToString();// = GetIntValue(_settings.SEQ_NO);
				_settings.MOBILE_NO = bitSystem.MOBILE_NO.ToString();
				_settings.STATION_NM = bitSystem.STATION_NM;
				_settings.SERVER_TYPE = bitSystem.SERVER_TYPE.ToString();
				_settings.SERVER_PORT = bitSystem.SERVER_PORT;
				_settings.FTP_GBN = bitSystem.FTP_GBN.ToString();
				_settings.FTP_IP = bitSystem.FTP_IP;
				_settings.FTP_PORT = bitSystem.FTP_PORT.ToString();
				_settings.FTP_USERID = bitSystem.FTP_USERID;
				_settings.FTP_USERPWD = bitSystem.FTP_USERPWD;
				_settings.HTTP_URL = bitSystem.HTTP_URL;
				_settings.HTTP_PORT = bitSystem.HTTP_PORT.ToString();
				_settings.ENV_GBN = bitSystem.ENV_GBN.ToString();
				_settings.ENV_PORT_NM = bitSystem.ENV_PORT_NM;
				_settings.ENV_BAUD_RATE = bitSystem.ENV_BAUD_RATE.ToString();
				_settings.CAM_NO1 = bitSystem.CAM_NO1.ToString();
				_settings.CAM_NO1_ROTATE = bitSystem.CAM_NO1_ROTATE.ToString();
				_settings.CAM_NO2 = bitSystem.CAM_NO2.ToString();
				_settings.CAM_NO2_ROTATE = bitSystem.CAM_NO2_ROTATE.ToString();
				_settings.Save();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public static TB_SYSTEM GetTbSystemSettings()
		{
			//return ReadTbSystemSettingsFromApplicationSettings() ?? ReadTbSystemSettingFromJsonFile();
			return ReadTbSystemSettingFromJsonFile();
		}

		private static TB_SYSTEM ReadTbSystemSettingFromJsonFile()
		{
			TB_SYSTEM tbSystem;
			try
			{
				tbSystem = DataProcessor.GetJsonSettings<TB_SYSTEM>(_tbSystem);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				tbSystem = null;
			}

			return tbSystem;
		}

		private static TB_SYSTEM ReadTbSystemSettingsFromApplicationSettings()
		{
			TB_SYSTEM tbSystem = new();

			try
			{
				tbSystem.LOGSAVE_DAY = GetIntValue(_settings.LOGSAVE_DAY);
				tbSystem.LOGSAVE_PERCENT = GetIntValue(_settings.LOGSAVE_PERCENT);
				tbSystem.ENV_USE_FAN_MANUAL = GetBoolValue(_settings.ENV_USE_FAN_MANUAL);
				tbSystem.ENV_USE_HEATER_MANUAL = GetBoolValue(_settings.ENV_USE_HEATER_MANUAL);
				tbSystem.SHOCK_DETECT_NO = GetIntValue(_settings.SHOCK_DETECT_NO);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				tbSystem = null;
			}

			return tbSystem;
		}

		public static bool SaveTbSystemSettings(TB_SYSTEM tbSystem)
		{
			try
			{
				//SaveTbSystemSettingsToApplicationSettings(tbSystem);
				SaveTbSystemSettingsToJsonFile(tbSystem);
				return true;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}			
		}

		private static void SaveTbSystemSettingsToJsonFile(TB_SYSTEM tbSystem)
		{
			try
			{
				DataProcessor.SaveDataToJsonFile(tbSystem, _tbSystem);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private static void SaveTbSystemSettingsToApplicationSettings(TB_SYSTEM tbSystem)
		{
			try
			{
				_settings.LOGSAVE_DAY = tbSystem.LOGSAVE_DAY.ToString();
				_settings.LOGSAVE_PERCENT = tbSystem.LOGSAVE_PERCENT.ToString();
				_settings.ENV_USE_FAN_MANUAL = tbSystem.ENV_USE_FAN_MANUAL.ToString();
				_settings.ENV_USE_HEATER_MANUAL = tbSystem.ENV_USE_HEATER_MANUAL.ToString();
				_settings.SHOCK_DETECT_NO = tbSystem.SHOCK_DETECT_NO.ToString();
				_settings.Save();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		public static BIT_ENV_SETTING GetBitEnvSettings()
		{
			//return ReadBitEnvSettingFromApplicationSettings() ?? ReadBitEnvSettingFromJsonFile();
			return ReadBitEnvSettingFromJsonFile();
		}

		private static BIT_ENV_SETTING ReadBitEnvSettingFromJsonFile()
		{
			BIT_ENV_SETTING bitEnvSetting;
			try
			{
				bitEnvSetting = DataProcessor.GetJsonSettings<BIT_ENV_SETTING>(_bitEnvSystem);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				bitEnvSetting = null;
			}

			return bitEnvSetting;
		}

		private static BIT_ENV_SETTING ReadBitEnvSettingFromApplicationSettings()
		{
			BIT_ENV_SETTING bitEnvSetting = new();

			try
			{
				bitEnvSetting.Volume = GetIntValue(_settings.Volume);
				bitEnvSetting.ArriveSoonGBN = GetIntValue(_settings.ArriveSoonGBN);
				bitEnvSetting.ArriveSoonTimeGBN = GetIntValue(_settings.ArriveSoonTimeGBN);
				bitEnvSetting.ArriveSoonStationGBN = GetIntValue(_settings.ArriveSoonStationGBN);
				bitEnvSetting.MonitorOnTime = _settings.MonitorOnTime;
				bitEnvSetting.MonitorOffTime = _settings.MonitorOffTime;
				bitEnvSetting.DefaultLCDLux = GetIntValue(_settings.DefaultLCDLux);
				bitEnvSetting.LuxCount = GetIntValue(_settings.LuxCount);
				bitEnvSetting.BITOrderGBN = GetIntValue(_settings.BITOrderGBN);
				bitEnvSetting.UseDetectSensor = GetIntValue(_settings.UseDetectSensor);
				bitEnvSetting.DetectSensorServiceTime = GetIntValue(_settings.DetectSensorServiceTime);
				bitEnvSetting.FANMaxTemp = GetIntValue(_settings.FANMaxTemp);
				bitEnvSetting.FANMinTemp = GetIntValue(_settings.FANMinTemp);
				bitEnvSetting.HeaterMaxTemp = GetIntValue(_settings.HeaterMaxTemp);
				bitEnvSetting.HeaterMinTemp = GetIntValue(_settings.HeaterMinTemp);
				bitEnvSetting.ShockDetectValue = GetIntValue(_settings.ShockDetectValue);
				bitEnvSetting.BIT_ID = _settings.BIT_ID;
				bitEnvSetting.StationMobileNo = GetIntValue(_settings.MOBILE_NO);
				bitEnvSetting.StationName = _settings.STATION_NM;
				bitEnvSetting.BITFontSize = GetIntValue(_settings.BITFontSize);
				bitEnvSetting.TestOperationDisplayYN = GetIntValue(_settings.TestOperationDisplayYN);
				bitEnvSetting.Reserve1 = GetIntValue(_settings.Reserve1);
				bitEnvSetting.BusArrivalInfoRefreshInterval = GetIntValue(_settings.BusArrivalReshreshInterval);
				bitEnvSetting.PromotionRefreshInterval = GetIntValue(_settings.PromotionRefreshInterval);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				bitEnvSetting = null;
			}

			return bitEnvSetting;
		}

		public static bool SaveBitEnvSettings(BIT_ENV_SETTING bitEnvSettings)
		{
			try
			{
				//SaveBitEnvSettingsToApplicationSettings(bitEnvSettings);
				SaveBitEnvSettingsToJsonFile(bitEnvSettings);
				return true;
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				return false;
			}
			
		}

		private static void SaveBitEnvSettingsToJsonFile(BIT_ENV_SETTING bitEnvSettings)
		{
			try
			{
				DataProcessor.SaveDataToJsonFile(bitEnvSettings, _bitEnvSystem);
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private static void SaveBitEnvSettingsToApplicationSettings(BIT_ENV_SETTING bitEnvSettings)
		{
			try
			{
				_settings.Volume = bitEnvSettings.Volume.ToString();
				_settings.ArriveSoonGBN = bitEnvSettings.ArriveSoonGBN.ToString();
				_settings.ArriveSoonTimeGBN = bitEnvSettings.ArriveSoonTimeGBN.ToString();
				_settings.ArriveSoonStationGBN = bitEnvSettings.ArriveSoonStationGBN.ToString();
				_settings.MonitorOnTime = bitEnvSettings.MonitorOnTime;
				_settings.MonitorOffTime = bitEnvSettings.MonitorOffTime;
				_settings.DefaultLCDLux = bitEnvSettings.DefaultLCDLux.ToString();
				_settings.LuxCount = bitEnvSettings.LuxCount.ToString();
				_settings.BITOrderGBN = bitEnvSettings.BITOrderGBN.ToString();
				_settings.UseDetectSensor = bitEnvSettings.UseDetectSensor.ToString();
				_settings.DetectSensorServiceTime = bitEnvSettings.DetectSensorServiceTime.ToString();
				_settings.FANMaxTemp = bitEnvSettings.FANMaxTemp.ToString();
				_settings.FANMinTemp = bitEnvSettings.FANMinTemp.ToString();
				_settings.HeaterMaxTemp = bitEnvSettings.HeaterMaxTemp.ToString();
				_settings.HeaterMinTemp = bitEnvSettings.HeaterMinTemp.ToString();
				_settings.ShockDetectValue = bitEnvSettings.ShockDetectValue.ToString();
				_settings.BIT_ID = bitEnvSettings.BIT_ID;
				_settings.BITFontSize = bitEnvSettings.BITFontSize.ToString();
				_settings.TestOperationDisplayYN = bitEnvSettings.TestOperationDisplayYN.ToString();
				_settings.Reserve1 = bitEnvSettings.Reserve1.ToString();
				_settings.PromotionRefreshInterval = bitEnvSettings.PromotionRefreshInterval.ToString();
				_settings.BusArrivalReshreshInterval = bitEnvSettings.BusArrivalInfoRefreshInterval.ToString();
				_settings.Save();
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}

		private static int GetIntValue(string settingValue)
		{
			return int.TryParse(settingValue, out int value) ? value : 0;
		}

		private static DateTime GetDateTimeValue(string settingValue)
		{
			return DateTime.TryParse(settingValue, out DateTime value) ? value : DateTime.MinValue;
		}

		private static bool GetBoolValue(string setting)
		{
			return bool.TryParse(setting, out bool result) ? result : false;
		}

		private static T GetValue<T>(string settingValue) where T : new()
		{
			var converter = TypeDescriptor.GetConverter(typeof(T));
			var parsedValue = (T)converter.ConvertFromString(settingValue);
			return parsedValue ?? new();
		}
	}
}
