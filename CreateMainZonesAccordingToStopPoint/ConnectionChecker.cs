using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateMainZonesAccordingToStopPoint
{
    internal class ConnectionChecker
    {
        private HashSet<int> TransitStopToKeep { get; set; }
        internal int CurrentZoneNumber { get; private set; }
        internal int PreviousStopPointNumber { get; private set; }
        internal ConnectionChecker(HashSet<int> transitStopToKeep)
        {
            TransitStopToKeep = transitStopToKeep;
        }

        internal void Init(int newOriginZoneNumber)
        {
            CurrentZoneNumber = newOriginZoneNumber;
            PreviousStopPointNumber = 0;
        }

        internal bool ShouldSaveStop(int currentStopPointNumber)
        {
            if (PreviousStopPointNumber != currentStopPointNumber &&
                TransitStopToKeep.Contains(currentStopPointNumber))
            {
                PreviousStopPointNumber = currentStopPointNumber;
                return true;
            }
            return false;
        }
    }
}
