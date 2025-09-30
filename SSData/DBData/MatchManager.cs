using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;
using System.Data;

namespace SSData
{
    public class MatchManager
    {
        static MatchManager()
        {
        }

        #region BC_CODE 관련

        public static BC_CODE BC_CODE_BY_ROW(DataRow row)
        {
            try
            {
                BC_CODE item = new BC_CODE();

                item.CD_GBN_NO = ConvertUtils.IntByObject(row["CD_GBN_NO"]);
                item.CD_GBN_NM = ConvertUtils.StringByObject(row["CD_GBN_NM"]);
                item.CODE = ConvertUtils.StringByObject(row["CODE"]);
                item.NAME = ConvertUtils.StringByObject(row["NAME"]);
                item.S_NM = ConvertUtils.StringByObject(row["S_NM"]);
                item.DISP_YN = ConvertUtils.IntByObject(row["DISP_YN"]);
                item.USE_YN = ConvertUtils.IntByObject(row["USE_YN"]);

                return item;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        #endregion

        public static BIT_SYSTEM BIT_SYSTEM_BY_ROW(DataRow row)
        {
            try
            {
                BIT_SYSTEM item = new BIT_SYSTEM();

                item.SEQ_NO = ConvertUtils.IntByObject(row["SEQ_NO"]);

                item.BIT_ID = ConvertUtils.StringByObject(row["BIT_ID"]);
                item.MOBILE_NO = ConvertUtils.IntByObject(row["MOBILE_NO"]);
                item.STATION_NM = ConvertUtils.StringByObject(row["STATION_NM"]);

                //item.DB_GBN = ConvertUtils.IntByObject(row["DB_GBN"]);
                //item.DB_IP = ConvertUtils.StringByObject(row["DB_IP"]);
                //item.DB_URL = ConvertUtils.StringByObject(row["DB_URL"]);
                //item.DB_PORT = ConvertUtils.IntByObject(row["DB_PORT"]);
                //item.DB_USERID = ConvertUtils.StringByObject(row["DB_USERID"]);
                //item.DB_USERPWD = ConvertUtils.StringByObject(row["DB_USERPWD"]);

                item.SERVER_URL = ConvertUtils.StringByObject(row["SERVER_URL"]);
                item.SERVER_PORT = ConvertUtils.StringByObject(row["SERVER_PORT"]);

                item.FTP_GBN = ConvertUtils.IntByObject(row["FTP_GBN"]);
                item.FTP_IP = ConvertUtils.StringByObject(row["FTP_IP"]);
                item.FTP_PORT = ConvertUtils.IntByObject(row["FTP_PORT"]);
                item.FTP_USERID = ConvertUtils.StringByObject(row["FTP_USERID"]);
                item.FTP_USERPWD = ConvertUtils.StringByObject(row["FTP_USERPWD"]);

                item.HTTP_URL = ConvertUtils.StringByObject(row["HTTP_URL"]);
                item.HTTP_PORT = ConvertUtils.IntByObject(row["HTTP_PORT"]);

                item.ENV_GBN = ConvertUtils.IntByObject(row["ENV_GBN"]); //20220729
                item.ENV_PORT_NM = ConvertUtils.StringByObject(row["ENV_PORT_NM"]);
                item.ENV_BAUD_RATE = ConvertUtils.IntByObject(row["ENV_BAUD_RATE"]);
                //item.WEBCAM_NM = ConvertUtils.StringByObject(row["WEBCAM_NM"]);
                //item.SHOCKCAM_NM = ConvertUtils.StringByObject(row["SHOCKCAM_NM"]);
                item.CAM_NO1 = ConvertUtils.IntByObject(row["CAM_NO1"]);
                item.CAM_NO1_ROTATE = ConvertUtils.IntByObject(row["CAM_NO1_ROTATE"]);
                item.CAM_NO2 = ConvertUtils.IntByObject(row["CAM_NO2"]);
                item.CAM_NO2_ROTATE = ConvertUtils.IntByObject(row["CAM_NO2_ROTATE"]);

                item.REGDATE = ConvertUtils.DateTimeByObject(row["REGDATE"]);

                item.SERVER_TYPE = 1;
				if (row.Table.Columns["SERVER_TYPE"] != null && row.IsNull("SERVER_TYPE") == false)
				{
					item.SERVER_TYPE = ConvertUtils.IntByObject(row["SERVER_TYPE"]);
				}

				return item;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        #region ENV_CONFIG

        public static BIT_ENV_SETTING BIT_ENV_SETTING_BY_ROW(DataRow row)
        {
            try
            {
                BIT_ENV_SETTING item = new BIT_ENV_SETTING();

                item.SEQ_NO= ConvertUtils.IntByObject(row["SEQ_NO"]);
                item.BIT_ID = ConvertUtils.StringByObject(row["BIT_ID"]);
                item.Volume = ConvertUtils.IntByObject(row["Volume"]);
                item.ArriveSoonGBN = ConvertUtils.IntByObject(row["ArriveSoonGBN"]);
                item.ArriveSoonTimeGBN = ConvertUtils.IntByObject(row["ArriveSoonTimeGBN"]);
                item.ArriveSoonStationGBN = ConvertUtils.IntByObject(row["ArriveSoonStationGBN"]);
                item.MonitorOnTime = ConvertUtils.StringByObject(row["MonitorOnTime"]);
                item.MonitorOffTime = ConvertUtils.StringByObject(row["MonitorOffTime"]);
                item.DefaultLCDLux = ConvertUtils.IntByObject(row["DefaultLCDLux"]);
                item.LuxCount = ConvertUtils.IntByObject(row["LuxCount"]);
                item.StateSendPeriod = ConvertUtils.IntByObject(row["StateSendPeriod"]);
                item.WebcamSendPeriod = ConvertUtils.IntByObject(row["WebcamSendPeriod"]);
                item.ScreenCaptureSendPeriod = ConvertUtils.IntByObject(row["ScreenCaptureSendPeriod"]);
                item.BITOrderGBN = ConvertUtils.IntByObject(row["BITOrderGBN"]);
                item.UseDetectSensor = ConvertUtils.IntByObject(row["UseDetectSensor"]);
                item.DetectSensorServiceTime = ConvertUtils.IntByObject(row["DetectSensorServiceTime"]);
                item.FANMaxTemp = ConvertUtils.IntByObject(row["FANMaxTemp"]);
                item.FANMinTemp = ConvertUtils.IntByObject(row["FANMinTemp"]);
                item.HeaterMaxTemp = ConvertUtils.IntByObject(row["HeaterMaxTemp"]);
                item.HeaterMinTemp = ConvertUtils.IntByObject(row["HeaterMinTemp"]);
                item.SubwayDisplayYN = ConvertUtils.IntByObject(row["SubwayDisplayYN"]);
                item.SubwayLineNo = ConvertUtils.IntByObject(row["SubwayLineNo"]);
                item.SubwayStationNo = ConvertUtils.IntByObject(row["SubwayStationNo"]);
                item.ForeignDisplayYN = ConvertUtils.IntByObject(row["ForeignDisplayYN"]);
                item.ForeignDisplayTime = ConvertUtils.IntByObject(row["ForeignDisplayTime"]);
                item.ShockDetectValue = ConvertUtils.IntByObject(row["ShockDetectValue"]);
                item.StationMobileNo = ConvertUtils.IntByObject(row["StationMobileNo"]);
                item.StationName = ConvertUtils.StringByObject(row["StationName"]);
                item.PromoteSoundPlayYN = ConvertUtils.IntByObject(row["PromoteSoundPlayYN"]);
                item.BITFontSize = ConvertUtils.IntByObject(row["BITFontSize"]);
                item.TestOperationDisplayYN = ConvertUtils.IntByObject(row["TestOperationDisplayYN"]);
                item.Reserve1 = ConvertUtils.IntByObject(row["Reserve1"]);
                //item.USE_YMD = ConvertUtils.DateTimeByObject(row["USE_YMD"]);
                item.REGDATE = ConvertUtils.DateTimeByObject(row["REGDATE"]);


                return item;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        public static BIT_ENV_LUX BIT_ENV_LUX_BY_ROW(DataRow row)
        {
            try
            {
                BIT_ENV_LUX item = new BIT_ENV_LUX();

                item.SEQ_NO = ConvertUtils.IntByObject(row["SEQ_NO"]);
                item.BIT_ID = ConvertUtils.IntByObject(row["BIT_ID"]);
                item.ENV_CONFIG_NO= ConvertUtils.IntByObject(row["ENV_CONFIG_NO"]);
                item.S_TIME = ConvertUtils.StringByObject(row["S_TIME"]);
                item.E_TIME = ConvertUtils.StringByObject(row["E_TIME"]);
                item.LUX = ConvertUtils.IntByObject(row["LUX"]);
                //item.USE_YMD = ConvertUtils.DateTimeByObject(row["USE_YMD"]);
                item.REGDATE = ConvertUtils.DateTimeByObject(row["REGDATE"]);

                return item;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        #endregion

        public static BIT_DISPLAY BIT_DISPLAY_BY_ROW(DataRow row)
        {
            try
            {
                BIT_DISPLAY item = new BIT_DISPLAY();

                item.SEQ_NO = ConvertUtils.IntByObject(row["SEQ_NO"]);
                item.BIT_ID = ConvertUtils.StringByObject(row["BIT_ID"]);
                item.DISP_GBN = ConvertUtils.IntByObject(row["DISP_GBN"]);
                item.DISP_NM= ConvertUtils.StringByObject(row["DISP_NM"]);
                item.POS_X = ConvertUtils.IntByObject(row["POS_X"]);
                item.POS_Y = ConvertUtils.IntByObject(row["POS_Y"]);
                item.SZ_W = ConvertUtils.IntByObject(row["SZ_W"]);
                item.SZ_H = ConvertUtils.IntByObject(row["SZ_H"]);
                //item.USE_YMD = ConvertUtils.DateTimeByObject(row["USE_YMD"]);
                item.REGDATE = ConvertUtils.DateTimeByObject(row["REGDATE"]);

                return item;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        public static LED_ITEM LED_ITEM_BY_ROW(DataRow row)
        {
            try
            {
                LED_ITEM item = new LED_ITEM();

                item.SEQ_NO = ConvertUtils.IntByObject(row["SEQ_NO"]);
                item.BIT_ID = ConvertUtils.IntByObject(row["BIT_ID"]);
                item.DISP_GBN = ConvertUtils.IntByObject(row["DISP_GBN"]);                
                item.USE_YN = ConvertUtils.IntByObject(row["USE_YN"]);
                item.CELL_NO = ConvertUtils.StringByObject(row["CELL_NO"]);
                item.POS_X = ConvertUtils.IntByObject(row["POS_X"]);
                item.POS_Y = ConvertUtils.IntByObject(row["POS_Y"]);
                item.SZ_W = ConvertUtils.IntByObject(row["SZ_W"]);
                item.SZ_H = ConvertUtils.IntByObject(row["SZ_H"]);
                item.DISP_TEXT= ConvertUtils.StringByObject(row["DISP_TEXT"]);
                item.FONT_NM = ConvertUtils.StringByObject(row["FONT_NM"]);
                item.FONT_SZ = ConvertUtils.DoubleByObject(row["FONT_SZ"]);
                item.FONT_ARGB = ConvertUtils.StringByObject(row["FONT_ARGB"]);
                item.FONT_STYLE_GBN = ConvertUtils.IntByObject(row["FONT_STYLE_GBN"]);
                item.FONT_WEIGHT_GBN = ConvertUtils.IntByObject(row["FONT_WEIGHT_GBN"]);
                item.FONT_ALIGN_GBN = ConvertUtils.IntByObject(row["FONT_ALIGN_GBN"]);

                item.REGDATE = ConvertUtils.DateTimeByObject(row["REGDATE"]);

                return item;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        public static LED_BUS_ARRIVESOON LED_BUS_ARRIVESOON_BY_ROW(DataRow row)
        {
            try
            {
                LED_BUS_ARRIVESOON item = new LED_BUS_ARRIVESOON();

                item.SEQ_NO = ConvertUtils.IntByObject(row["SEQ_NO"]);
                item.BIT_ID = ConvertUtils.IntByObject(row["BIT_ID"]);
                item.DISP_GBN = ConvertUtils.IntByObject(row["DISP_GBN"]);
                item.USE_YN = ConvertUtils.IntByObject(row["USE_YN"]);
                item.TITLE_TEXT = ConvertUtils.StringByObject(row["TITLE_TEXT"]);
                item.TITLE_POS_X = ConvertUtils.IntByObject(row["TITLE_POS_X"]);
                item.TITLE_POS_Y = ConvertUtils.IntByObject(row["TITLE_POS_Y"]);
                item.TITLE_SZ_W = ConvertUtils.IntByObject(row["TITLE_SZ_W"]);
                item.TITLE_SZ_H = ConvertUtils.IntByObject(row["TITLE_SZ_H"]);
                item.TITLE_FONT_NM = ConvertUtils.StringByObject(row["TITLE_FONT_NM"]);
                item.TITLE_FONT_SZ = ConvertUtils.DoubleByObject(row["TITLE_FONT_SZ"]);
                item.TITLE_FONT_ARGB = ConvertUtils.StringByObject(row["TITLE_FONT_ARGB"]);
                item.TITLE_FONT_STYLE_GBN = ConvertUtils.IntByObject(row["TITLE_FONT_STYLE_GBN"]);
                item.TITLE_FONT_WEIGHT_GBN = ConvertUtils.IntByObject(row["TITLE_FONT_WEIGHT_GBN"]);
                item.TITLE_FONT_ALIGN_GBN = ConvertUtils.IntByObject(row["TITLE_FONT_ALIGN_GBN"]);
                item.CONT_POS_X = ConvertUtils.IntByObject(row["CONT_POS_X"]);
                item.CONT_POS_Y = ConvertUtils.IntByObject(row["CONT_POS_Y"]);
                item.CONT_SZ_W = ConvertUtils.IntByObject(row["CONT_SZ_W"]);
                item.CONT_SZ_H = ConvertUtils.IntByObject(row["CONT_SZ_H"]);
                item.CONT_FONT_NM = ConvertUtils.StringByObject(row["CONT_FONT_NM"]);
                item.CONT_FONT_SZ = ConvertUtils.DoubleByObject(row["CONT_FONT_SZ"]);
                item.CONT_FONT_ARGB = ConvertUtils.StringByObject(row["CONT_FONT_ARGB"]);
                item.CONT_FONT_STYLE_GBN = ConvertUtils.IntByObject(row["CONT_FONT_STYLE_GBN"]);
                item.CONT_FONT_WEIGHT_GBN = ConvertUtils.IntByObject(row["CONT_FONT_WEIGHT_GBN"]);
                //item.CONT_FONT_ALIGN_GBN = ConvertUtils.IntByObject(row["CONT_FONT_ALIGN_GBN"]);
                item.REGDATE = ConvertUtils.DateTimeByObject(row["REGDATE"]);

                return item;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        public static TB_SYSTEM TB_SYSTEM_BY_ROW(DataRow row)
        {
            try
            {
                TB_SYSTEM item = new TB_SYSTEM();

                item.SEQ_NO = ConvertUtils.IntByObject(row["SEQ_NO"]);
                item.LOGSAVE_DAY = ConvertUtils.IntByObject(row["LOGSAVE_DAY"]);
                item.LOGSAVE_PERCENT = ConvertUtils.IntByObject(row["LOGSAVE_PERCENT"]);
                item.LED_PAGE_CHGTIME= ConvertUtils.IntByObject(row["LED_PAGE_CHGTIME"]);
                item.LED_USE_BUSNO_COLOR = ConvertUtils.IntByObject(row["LED_USE_BUSNO_COLOR"]).Equals(1);
                item.LED_USE_BUSNO_LOW = ConvertUtils.IntByObject(row["LED_USE_BUSNO_LOW"]).Equals(1);
                item.LED_USE_ARRIVESOON_COLOR = ConvertUtils.IntByObject(row["LED_USE_ARRIVESOON_COLOR"]).Equals(1);
                item.LED_USE_ARRIVESOON_LOW = ConvertUtils.IntByObject(row["LED_USE_ARRIVESOON_LOW"]).Equals(1);

                item.ENV_USE_FAN_MANUAL= ConvertUtils.IntByObject(row["ENV_USE_FAN_MANUAL"]).Equals(1);
                item.ENV_USE_HEATER_MANUAL = ConvertUtils.IntByObject(row["ENV_USE_HEATER_MANUAL"]).Equals(1);

                item.SUBWAY_DISPLAY_YN = ConvertUtils.IntByObject(row["SUBWAY_DISPLAY_YN"]);
                item.SUBWAY_LINE_NO = ConvertUtils.IntByObject(row["SUBWAY_LINE_NO"]);
                item.SUBWAY_STATION_NO = ConvertUtils.IntByObject(row["SUBWAY_STATION_NO"]);

                //20221129 bha
				item.SHOCK_DETECT_NO = 0;
                if (row.Table.Columns["SHOCK_DETECT_NO"] != null && row.IsNull("SHOCK_DETECT_NO") == false)
                {
                    item.SHOCK_DETECT_NO = ConvertUtils.IntByObject(row["SHOCK_DETECT_NO"]);
                }
                else
                {
                    item.SHOCK_DETECT_NO = BITDataManager.BitENVConfig.ITEM.ShockDetectValue;
                }

				item.REGDATE = ConvertUtils.DateTimeByObject(row["REGDATE"]);

                return item;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }
    }
}