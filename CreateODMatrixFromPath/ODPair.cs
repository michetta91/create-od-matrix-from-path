using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace create_od_matrix_from_path
{
    internal class ODPair
    {
        internal int OriginZoneNo { get; set; }
        internal int DestinationZoneNo { get; set; }
        internal double OdTrips { get; set; }
        internal HashSet<string> TransportSystemCodes { get; set; } = new HashSet<string>();

        internal string Key => $"{OriginZoneNo};{DestinationZoneNo}";
    }
}
