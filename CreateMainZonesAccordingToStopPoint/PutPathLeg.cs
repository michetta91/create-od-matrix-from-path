using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateMainZonesAccordingToStopPoint
{
    internal class PutPathLeg
    {

        internal int OriginZoneNumber { get; private set; }
        internal int DestinationZoneNumber { get; private set; }
        internal int PathIndex { get; private set; }

        internal int FromStopPointNumber { get; private set; }
        internal int ToStopPointNumber { get; private set; }

        internal string Key { get => $"{OriginZoneNumber};{DestinationZoneNumber};{PathIndex}"; }

        internal PutPathLeg(string originZoneNumber,
                            string destinationZoneNumber,
                            string pathIndex)
        {
            OriginZoneNumber = int.Parse(originZoneNumber);
            DestinationZoneNumber = int.Parse(destinationZoneNumber);
            PathIndex = int.Parse(pathIndex);

            FromStopPointNumber = int.MaxValue;
            ToStopPointNumber = int.MaxValue;
        }

        internal void AddStopPoint(int stopPointNumber, bool isFrom)
        {
            if (isFrom)
            {
                if (FromStopPointNumber != int.MaxValue) FromStopPointNumber = stopPointNumber;
            }
            else
            {
                ToStopPointNumber = stopPointNumber;
            }
        }
    }

    internal static class PutPathLegExtensions
    {
        internal static void AddStopPoints(this PutPathLeg putPathLeg,
                                           HashSet<int> transitStopToKeep,
                                           string fromStopPointNumber,
                                           string toStopPointNumber)
        {
            var fromStopPointNumberInt = int.Parse(fromStopPointNumber);
            if (transitStopToKeep.Contains(fromStopPointNumberInt))
            {
                putPathLeg.AddStopPoint(fromStopPointNumberInt, true);
            }

            var toStopPointNumberInt = int.Parse(toStopPointNumber);
            if (transitStopToKeep.Contains(toStopPointNumberInt))
            {
                putPathLeg.AddStopPoint(toStopPointNumberInt, false);
            }
        }
    }
}
