using MyPWS.API.Converters;
using MyPWS.API.Models.dto;
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
        /// convert input protocol class as TDO model to Pwsupload entity
        /// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio#over-post
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
                Uv = weatherImperial.Uv,
                Winddir = weatherImperial.Winddir,
                Windgustdir = weatherImperial.Windgustdir,
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
                Windgustdir = pwsUpload.Windgustdir,
                Windgustkmh = pwsUpload.Windgustkmh,
                Windspeedkmh = pwsUpload.Windspeedkmh
            };
        }
    }
}
