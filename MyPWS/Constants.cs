using MySql.Data.EntityFrameworkCore.Query.Expressions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPWS
{	
	public static class Constants
	{
		public const string NoPWS = "No PWS found";
		public const string NoPWSData = "No PWS data found";
		public const string OutOfRangePattern = "{0} isn´t in range {1}";

		/// <summary>
		/// PWS maximum chache period s afrer this period can by stored same data as previous 
		/// </summary>
		public static TimeSpan weatherPostCacheTimeout = new TimeSpan(0, 1, 0);

		/// <summary>
		/// shortest PWSWeather interval for PWS in ms
		/// </summary>
		public static TimeSpan ShortestUploadInterval = new TimeSpan(0,0,0,0,0);
		public struct range
		{
			public short MinValue { get; private set; }
			public short MaxValue { get; private set; }
			public range(short minVal, short maxVal)
			{
				MinValue = minVal;
				MaxValue = maxVal;
			}
			public override string ToString()
			{
				return $"from: {MinValue} to: {MaxValue}";
			}
		}


		/// <summary>
		/// temperature in F range
		/// </summary>
		public static range RangeTempF = new range(-125, 140);

		/// <summary>
		/// barometric pressure in Inch range
		/// </summary>
		public static range RangeBaroIn = new range(0, 40);

		/// <summary>
		/// humidity percentage range
		/// </summary>
		public static range RangeHumi = new range(0, 100);

		/// <summary>
		/// wind direction degrees range 
		/// </summary>
		public static range RangeWindDirDeg = new range(0, 360);

		/// <summary>
		/// UV index range
		/// </summary>
		public static range RangeUV = new range(0, 50);

		/// <summary>
		/// Wind speed MPH range
		/// </summary>
		public static range RangeWindSpeedMpH = new range(0, 300);

		/// <summary>
		/// Rain in In
		/// </summary>
		public static range RangeRainIn = new range(0, 300);		


	}
	public static class CacheKeys
	{
		public static string PWS { get { return "_PWS_"; } }		
	}

	
}


