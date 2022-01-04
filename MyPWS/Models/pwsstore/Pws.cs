using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyPWS.Models.pwsstore
{
    public partial class Pws
    {
        public Pws()
        {
            Configwunderground = new HashSet<Configwunderground>();
            Weathers = new HashSet<Weather>();
        }
        [Key]
        public int IdPws { get; set; }

        [Required, MaxLength(100)]        
        public string Id { get; set; }

        [Required, MaxLength(45)]        
        public string Pwd { get; set; }
        
        [Required]
        public decimal Lat { get; set; }

        [Required]
        public decimal Lon { get; set; }

        [Required]
        public short Alt { get; set; }
        public string Desc { get; set; }

        [Required, MaxLength(45)]        
        public string Name { get; set; }

        public virtual ICollection<Configwunderground> Configwunderground { get; set; }
        public virtual ICollection<Weather> Weathers { get; set; }
    }
}
