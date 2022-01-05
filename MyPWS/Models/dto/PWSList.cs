using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPWS.API.Models.dto
{
    public class PWSList
    {
        public string Id { get; set; }
        public Point GpsCoordinates { get; set; }
        public short Alt { get; set; }
        public string Desc { get; set; }
        public string Name { get; set; }
    }
}

