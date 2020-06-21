using System;

namespace MyPWS.Models.pwsstore
{
	public partial class Weather: IEquatable<Weather>
	{
		public bool Equals(Weather other)
		{
			if (other == null) return false;
			if (this.Baromhpa != other.Baromhpa ||
				this.Dailyrainmm != other.Dailyrainmm ||
				this.Dewptc != other.Dewptc ||
				this.Humidity != other.Humidity ||
				this.Indoorhumidity != other.Indoorhumidity ||
				this.Indoortempc != other.Indoortempc ||
				this.Rainmm != other.Rainmm ||
				this.Tempc != other.Tempc ||
				this.Uv != other.Uv ||
				this.Winddir != other.Winddir ||				
				this.Windgustkmh != other.Windgustkmh ||
				this.Windspeedkmh != other.Windspeedkmh
				)
			{
				return false;
			}
			return true;
		}
	}
}
