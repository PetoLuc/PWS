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

		/// <summary>
		/// PWS maximum chache period s afrer this period can by stored same data as previous 
		/// </summary>
		public static int PWSTimeout  = 60; //1 miunute

		public static string DateNow = "now";
		public static int DecimalPrecision = 1;

	}
	public static class CacheKeys
	{
		public static string PWS { get { return "_PWS_"; } }
		public static string PWSUpload { get { return "_PWS_"; } }

		
	}
}
