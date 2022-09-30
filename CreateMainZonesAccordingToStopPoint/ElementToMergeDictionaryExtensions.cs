using System.Diagnostics;

namespace CreateMainZonesAccordingToStopPoint
{
    internal static class ElementToMergeDictionaryExtensions
    {

        internal static void ImportStops(this Dictionary<int, ElementToMerge> elementsToMerge,
                                               HashSet<int> transitStopToKeep,
                                               string[] lines,
                                               int zoneNumberIndex,
                                               int stopPointNoIndex,
                                               bool fromOrigin)
        {
            var connectionChecker = new ConnectionChecker(transitStopToKeep);         
            for(int i = 0; i< lines.Length; i++) 
            {
                if(i == 562047) Debugger.Break();

                var line = lines[i];
                if (string.IsNullOrEmpty(line)) continue;
                var fields = line.Split(';');

                string zoneNumber = fields[zoneNumberIndex];
                if (fromOrigin && !string.IsNullOrEmpty(zoneNumber))
                {
                    var newZoneNumber = int.Parse(zoneNumber);
                    connectionChecker.Init(newZoneNumber);
                }

                if (!fromOrigin && connectionChecker.CurrentZoneNumber == 0 && string.IsNullOrEmpty(zoneNumber))
                {
                    zoneNumber = GetFirstZoneNumber(lines, i, zoneNumberIndex);
                    var newZoneNumber = int.Parse(zoneNumber);
                    connectionChecker.Init(newZoneNumber);
                }
               
                var connectedStopPoint = fields[stopPointNoIndex];
                if (string.IsNullOrEmpty(connectedStopPoint)) continue;
                if (!elementsToMerge.ContainsKey(connectionChecker.CurrentZoneNumber))
                {
                    if (connectionChecker.CurrentZoneNumber == 0) Debugger.Break();
                    elementsToMerge.Add(connectionChecker.CurrentZoneNumber, new ElementToMerge(connectionChecker.CurrentZoneNumber));
                }

                if (!string.IsNullOrEmpty(connectedStopPoint))
                {
                    var currentStopPointNumberOrigin = int.Parse(connectedStopPoint);
                    if (connectionChecker.ShouldSaveStop(currentStopPointNumberOrigin))
                    {
                        elementsToMerge[connectionChecker.CurrentZoneNumber].AddConnectedTransitStop(currentStopPointNumberOrigin);
                    }
                }

                if(!fromOrigin && !string.IsNullOrEmpty(zoneNumber))
                {
                    connectionChecker.Init(0);
                }
            }
        }

        private static string GetFirstZoneNumber(string[] lines, int i, int zoneNumberIndex)
        {
            for(int i1 = i; i1<lines.Length; i1++)
            {
                var line = lines[i1];
                var fields = line.Split(';');
                var zoneNumber = fields[zoneNumberIndex];
                if (!string.IsNullOrEmpty(zoneNumber)) return zoneNumber;
            }
            return string.Empty;
        }
    }
}
