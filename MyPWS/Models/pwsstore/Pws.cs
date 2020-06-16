using System;
using System.Collections.Generic;

namespace MyPWS.Models.pwsstore
{
    public partial class Pws
    {
        public Pws()
        {
            Configwunderground = new HashSet<Configwunderground>();
            Weather = new HashSet<Weather>();
        }

        public int IdPws { get; set; }
        public string Id { get; set; }
        public string Pwd { get; set; }
        public decimal Lat { get; set; }
        public decimal Lon { get; set; }
        public short Alt { get; set; }
        public string Desc { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Configwunderground> Configwunderground { get; set; }
        public virtual ICollection<Weather> Weather { get; set; }
    }
}
