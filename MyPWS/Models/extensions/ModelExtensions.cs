using MyPWS.API.Converters;
using MyPWS.API.Models.dto;
using MyPWS.Models.extensions;
using MyPWS.Models.pwsstore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPWS.API.Models.extensions
{
	public static class ModelExtensions
	{
        /// <summary>
        /// convert input protocol class as TDO model to weather entity, round to 1 decimal point        
        /// </summary>
        /// <param name="weatherImperial"></param>
        /// <returns></returns>
        public static Weather ToWeather(this WeatherImperial weatherImperial)
        {
            if (!DateTime.TryParse(weatherImperial.Dateutc, out DateTime date))
            {
                date = DateTime.UtcNow;
            }

            return new Weather
            {
                Baromhpa = ImperialToMetric.MercInchToHpa(weatherImperial.Baromin),
                Dailyrainmm = ImperialToMetric.InchesToMilimeters(weatherImperial.Dailyrainin),
                Dateutc = date,
                Dewptc = ImperialToMetric.FarenheitToCelsius(weatherImperial.Dewptf),
                Humidity = weatherImperial.Humidity,
                //Id = null,
                //IdPws 
                Indoorhumidity = weatherImperial.Indoorhumidity,
                Indoortempc = ImperialToMetric.FarenheitToCelsius(weatherImperial.Indoortempf),
                Rainmm = ImperialToMetric.InchesToMilimeters(weatherImperial.Rainin),
                Tempc = ImperialToMetric.FarenheitToCelsius(weatherImperial.Tempf),
                Uv = weatherImperial.Uv.HasValue ? decimal.Round(weatherImperial.Uv.Value, Constants.DecimalPrecision):weatherImperial.Uv,
                Winddir = weatherImperial.Winddir,                
                Windgustkmh = ImperialToMetric.MphToKmh(weatherImperial.Windgustmph),
                Windspeedkmh = ImperialToMetric.MphToKmh(weatherImperial.Windspeedmph)
            };
        }

        public static WeatherMetric ToWeatherMetric(this Weather pwsUpload)
        {
            return new WeatherMetric
            {
                Baromhpa = pwsUpload.Baromhpa,
                Dailyrainmm = pwsUpload.Dailyrainmm,
                Dateutc = pwsUpload.Dateutc.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),
                Dewptc = pwsUpload.Dewptc,
                Humidity = pwsUpload.Humidity,
                Indoorhumidity = pwsUpload.Indoorhumidity,
                Indoortempc = pwsUpload.Indoortempc,
                Rainmm = pwsUpload.Rainmm,
                Tempc = pwsUpload.Tempc,
                Uv = pwsUpload.Uv,
                Winddir = pwsUpload.Winddir,                
                Windgustkmh = pwsUpload.Windgustkmh,
                Windspeedkmh = pwsUpload.Windspeedkmh
            };
        }

        public static (decimal? speed, short? direction) vectorAvg(this List<Weather> lstWeather, Func<Weather, decimal?> speed, Func<Weather, short?> direction)
        {

            if (lstWeather == null || lstWeather.Count == 0)
            {
                return (null, null);
            }
            double sinSum = 0;
            double cosSum = 0;
            int cnt = 0;

            foreach (Weather weather in lstWeather)
            {
                decimal? spd = speed(weather);
                short? dir = direction(weather);
                //both must by not null
                if (spd != null && dir != null)
                {
                    sinSum += Math.Sin(dir.Value.ConvertToRadians()) * Convert.ToDouble(spd.Value);
                    cosSum += Math.Cos(dir.Value.ConvertToRadians()) * Convert.ToDouble(spd.Value);
                    cnt++;
                }
            }

            decimal avgspeed = (decimal)Math.Round(Math.Sqrt(Math.Pow(sinSum / cnt, 2) + Math.Pow(cosSum / cnt, 2)), Constants.DecimalPrecision);
            short resDir = (short)Math.Round((Math.Atan2(sinSum, cosSum).ConvertToDegress() + 360) % 360, 0);
            return (avgspeed, resDir);
        }
		
	}
}
