using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateMainZonesAccordingToStopPoint
{
    internal class Maps
    {
        internal Dictionary<int, string>? MainZoneToElementMap { get; private set; } = null;
        internal Dictionary<int, int>? ZoneToMainZoneMap { get; private set; } = null;
    }
}
