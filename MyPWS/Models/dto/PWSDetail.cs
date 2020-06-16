using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPWS.API.Models.dto
{
	public class PWSDetail
	{        
        public string Id { get; set; }
        public string Pwd { get; set; }
        public decimal Lat { get; set; }
        public decimal Lon { get; set; }
        public short Alt { get; set; }
        public string Desc { get; set; }
        public string Name { get; set; }
        public long WeatherRecordsCount { get; set; }

        public DateTime LastUpdateDateTime { get; set; }
    }
}
