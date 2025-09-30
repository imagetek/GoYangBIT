using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SSData.GwachonAPI
{
	public class GwacheonWeatherResponse
	{
		[JsonPropertyName("response")]
		public WeatherResponse Response { get; set; }
	}

	public record WeatherResponse
	{
		[JsonPropertyName("header")]
		public WeatherHeader? Header { get; set; }
		[JsonPropertyName("body")]
		public WeatherBody? Body { get; set; }
	}

	public record WeatherHeader
	{
		[JsonPropertyName("resultCode")]
		public string? ResultCode { get; set; }
		[JsonPropertyName("resultMsg")]
		public string? ResultMsg { get; set; }
	}

	public record WeatherBody
	{
		[JsonPropertyName("dataType")]
		public string? DataType { get; set; }

		[JsonPropertyName("items")]
		public WeatherItems? Items { get; set; }

		[JsonPropertyName("pageNo")]
		public int PageNo { get; set; }

		[JsonPropertyName("numOfRows")]
		public int NumOfRows { get; set; }

		[JsonPropertyName("totalCount")]
		public int TotalCount { get; set; }
	}

	public record WeatherItems
	{
		[JsonPropertyName("item")]
		public List<WeatherItem>? Item { get; set; }
	}

	public record WeatherItem
	{
		[JsonPropertyName("baseDate")]
		public string? BaseDate { get; set; }

		[JsonPropertyName("baseTime")]
		public string? BaseTime { get; set; }

		[JsonPropertyName("category")]
		public string? Category { get; set; }

		[JsonPropertyName("fcstDate")]
		public string? FcstDate { get; set; }

		[JsonPropertyName("fcstTime")]
		public string? FcstTime { get; set; }

		[JsonPropertyName("fcstValue")]
		public string? FcstValue { get; set; }

		[JsonPropertyName("nx")]
		public int Nx { get; set; }
		[JsonPropertyName("ny")]
		public int Ny { get; set; }
	}
}
