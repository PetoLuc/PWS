using System;
using System.Collections.Generic;

namespace MyPWS.Models.pwsstore
{
    public partial class Configwunderground
    {
        public long Id { get; set; }
        public int IdPws { get; set; }
        public string Wuid { get; set; }
        public string Pwd { get; set; }
        public string Desc { get; set; }

        public virtual Pws IdPwsNavigation { get; set; }
    }
}
