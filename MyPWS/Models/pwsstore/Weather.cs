using System;
using System.Collections.Generic;

namespace MyPWS.Models.pwsstore
{
    public partial class Weather
    {
        public long Id { get; set; }
        public int IdPws { get; set; }
        public DateTime Dateutc { get; set; }
        public decimal? Tempc { get; set; }
        public short? Humidity { get; set; }
        public decimal? Dewptc { get; set; }
        public decimal? Baromhpa { get; set; }
        public short? Winddir { get; set; }
        public decimal? Windspeedkmh { get; set; }
        public decimal? Windgustkmh { get; set; }        
        public decimal? Uv { get; set; }
        public decimal? Rainmm { get; set; }
        public decimal? Dailyrainmm { get; set; }
        public decimal? Indoortempc { get; set; }
        public short? Indoorhumidity { get; set; }

        public virtual Pws IdPwsNavigation { get; set; }
    }
}
