using MyPWS.API.Converters;
using MyPWS.API.Models.dto;
using MyPWS.Models.extensions;
using MyPWS.Models.pwsstore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Net.Http.Headers;
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
            return new Weather
            {
                              
                Baromhpa = ImperialToMetric.MercInchToHpa(weatherImperial.Baromin),
                Dailyrainmm = ImperialToMetric.InchesToMilimeters(weatherImperial.Dailyrainin),                
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
                Windgustkmh = ImperialToMetric.MphToKmh(weatherImperial.Windgustmph),
                Windspeedkmh = ImperialToMetric.MphToKmh(weatherImperial.Windspeedmph)
            };
        }
        
        /// <summary>
        /// check inpyt values ranges if are real
        /// </summary>
        /// <param name="weatherImperial"></param>
        /// <param name="checkErrorDescription"></param>
        /// <returns></returns>
        public static bool CheckRange(this WeatherImperial weatherImperial, ref string checkErrorDescription)        
        {
            List<bool> results = new List<bool>();
            List<string> errors = new List<string>();
            //temperatures
            results.Add(weatherImperial.Tempf.IsWithin(nameof(weatherImperial.Tempf), Constants.RangeTempF, ref errors));
            results.Add(weatherImperial.Indoortempf.IsWithin(nameof(weatherImperial.Indoortempf), Constants.RangeTempF, ref errors));
            results.Add(weatherImperial.Dewptf.IsWithin(nameof(weatherImperial.Dewptf), Constants.RangeTempF, ref errors));
            //humidity
            results.Add(((decimal?)weatherImperial.Humidity).IsWithin(nameof(weatherImperial.Humidity), Constants.RangeHumi, ref errors));
            results.Add(((decimal?)weatherImperial.Indoorhumidity).IsWithin(nameof(weatherImperial.Indoorhumidity), Constants.RangeHumi, ref errors));
            //baro
            results.Add(((decimal?)weatherImperial.Baromin).IsWithin(nameof(weatherImperial.Baromin), Constants.RangeBaroIn, ref errors));
            //wind dir
            results.Add(((decimal?)weatherImperial.Winddir).IsWithin(nameof(weatherImperial.Winddir), Constants.RangeWindDirDeg, ref errors));
            results.Add(((decimal?)weatherImperial.Windgustdir).IsWithin(nameof(weatherImperial.Windgustdir), Constants.RangeWindDirDeg, ref errors));
            //wind speed
            results.Add(((decimal?)weatherImperial.Windspeedmph).IsWithin(nameof(weatherImperial.Windspeedmph), Constants.RangeWindSpeedMpH, ref errors));
            results.Add(((decimal?)weatherImperial.Windgustmph).IsWithin(nameof(weatherImperial.Windgustmph), Constants.RangeWindSpeedMpH, ref errors));
            //uv
            results.Add(((decimal?)weatherImperial.Uv).IsWithin(nameof(weatherImperial.Uv), Constants.RangeUV, ref errors));
            //rain  
            results.Add(((decimal?)weatherImperial.Rainin).IsWithin(nameof(weatherImperial.Rainin), Constants.RangeRainIn, ref errors));
            results.Add(((decimal?)weatherImperial.Dailyrainin).IsWithin(nameof(weatherImperial.Dailyrainin), Constants.RangeRainIn, ref errors));
            

            checkErrorDescription = JsonSerializer.Serialize(errors);
            return results.All(r => r ==true);                        
        }

        //private static string addRangeErrorDesc(object checkedObject, Constants.range range)
        //{
        //    return $"{nameof(checkedObject)}: {checkedObject} is out of range from:  {range.MinValue} to: {range.MaxValue}";
        //}

        public static WeatherMetric ToWeatherMetric(this Weather weather)
        {
            return new WeatherMetric
            {             
                DateUtc = weather.Dateutc,
                Baromhpa = weather.Baromhpa,
                Dailyrainmm = weather.Dailyrainmm,                
                Dewptc = weather.Dewptc,
                Humidity = weather.Humidity,
                Indoorhumidity = weather.Indoorhumidity,
                Indoortempc = weather.Indoortempc,
                Rainmm = weather.Rainmm,
                Tempc = weather.Tempc,
                Uv = weather.Uv,
                Winddir = weather.Winddir,                
                Windgustkmh = weather.Windgustkmh,
                Windspeedkmh = weather.Windspeedkmh
            };
        }

        public static (decimal? speed, short? direction) vectorAvg(this IEnumerable<Weather> lstWeather, Func<Weather, decimal?> speed, Func<Weather, short?> direction)
        {

            if (lstWeather == null || lstWeather.Count() == 0)
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

            decimal avgspeed = (decimal)Math.Sqrt(Math.Pow(sinSum / cnt, 2) + Math.Pow(cosSum / cnt, 2));
            short resDir = (short)Math.Round((Math.Atan2(sinSum, cosSum).ConvertToDegress() + 360) % 360, 0);
            return (avgspeed, resDir);
        }

        /// <summary>
        /// calculate average values 
        /// </summary>
        /// <param name="weathers"></param>
        /// /// <param name="windgustMax">true - return max gust, false - return average gust</param>
        /// <returns></returns>
        public static IEnumerable<Weather> GetAverage(this IEnumerable<IGrouping<int, Weather>> weathers, bool windgustMax = false)
        {
            foreach (IEnumerable<Weather> weatherByDay in weathers)
            {
                Weather insWeather = new Weather()
                {
                    //rount safe
                    Baromhpa = weatherByDay.Average(w => w.Baromhpa),
                    Dailyrainmm = weatherByDay.Max(w => w.Dailyrainmm),
                    //date of last record 
                    Dateutc = weatherByDay.Max(w=>w.Dateutc),
                    Dewptc = weatherByDay.Average(w => w.Dewptc),
                    Humidity = (short?)weatherByDay.Average(w => w.Humidity),
                    Indoorhumidity = (short?)weatherByDay.Average(w => w.Indoorhumidity),
                    Indoortempc = weatherByDay.Average(w => w.Indoortempc),
                    Rainmm = weatherByDay.Max(w => w.Rainmm),
                    Tempc = weatherByDay.Average(w => w.Tempc),
                    Uv = weatherByDay.Average(w => w.Uv),                    
                };

                if (!windgustMax) //average gust speed
                {
                    short? dir;
                    (insWeather.Windgustkmh, dir) = weatherByDay.vectorAvg(w => w.Windgustkmh, w => w.Winddir);
                }
                else //max gust speed
                {
                    insWeather.Windgustkmh = weatherByDay.Max(w => w.Windgustkmh);
                }

                (insWeather.Windspeedkmh, insWeather.Winddir) = weatherByDay.vectorAvg(w => w.Windspeedkmh, w => w.Winddir);
                yield return insWeather;
            }
        }

    }
}
