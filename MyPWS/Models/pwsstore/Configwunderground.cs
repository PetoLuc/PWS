using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyPWS.Models.pwsstore
{
    public partial class Configwunderground
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public int IdPws { get; set; }

        [Required, StringLength(100)]
        public string Wuid { get; set; }
        
        [Required, StringLength(45)]
        public string Pwd { get; set; }
        public string Desc { get; set; }
        public virtual Pws Pws { get; set; }
    }
}
