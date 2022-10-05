using System.Diagnostics;

namespace AggregateZonesConnectedToTheSameNode
{
    internal class Solver
    {

        public static Maps ComputeMaps(string inputFilePath)
        {

            var zones = ImportZone(inputFilePath);
            if (zones is null) return new Maps();

            var mergedElements = MergeElements(zones);

            return ComputeMaps(mergedElements);
        }

        private static Zone[]? ImportZone(string inputFilePath)
        {

         List< Zone> zones = new();

            var lines = File.ReadAllLines(inputFilePath);

            string[] keys = { "NO" };
            var result = ComputeHeaders(lines, "$ZONE:", keys);
            if (result is null) return null;

            int zoneNumberIndex = result.Value.headers[keys[0]];        
            int connectNodesIndex = result.Value.headers["CONNECTEDNODES"];

            foreach (var line in lines.Skip(result.Value.headerLine).ToArray())
            {
                if (string.IsNullOrEmpty(line)) continue;
                var fields = line.Split(';');

                string zoneNumber = fields[zoneNumberIndex];
                string connectNodes = fields[connectNodesIndex];
              
                if(string.IsNullOrEmpty(connectNodes)) continue;


                var zone = new Zone(zoneNumber, connectNodes);
                zones.Add(zone);
            }

            return zones.ToArray();
        }

        private static (int headerLine, Dictionary<string, int> headers)? ComputeHeaders(string[] lines,
                                                                                         string header,
                                                                                         string[] keys)
        {
            int counter = 0;
            foreach (var line in lines)
            {
                if (line.StartsWith(header))
                {
                    var headersArray = line.Replace(header, "").Split(';');
                    var headers = Enumerable.Range(0, headersArray.Length).ToDictionary(x => headersArray[x]);
                    if (headers is null || !AllKeysExists(headers, keys)) return null;

                    return (counter + 1, headers);

                }
                counter++;
            }
            return null;
        }

        private static bool AllKeysExists(Dictionary<string, int> headers, string[] keys)
        {
            foreach (var key in keys)
            {
                if (!headers.ContainsKey(key)) return false;
            }
            return true;
        }
        internal static string Key(string originZoneNumber, string destinationZoneNumber, string pathIndex)
        {
            return $"{originZoneNumber};{destinationZoneNumber};{pathIndex}";
        }
        
        private static MergedElements[] MergeElements(Zone[] zones)
        {
            var mergedElements = new Dictionary<string, MergedElements>();
            foreach (var zone in zones)
            {
                if (zone.ConnectedNodesNumbers.Length == 0) continue;              
                if (!mergedElements.ContainsKey(zone.Key))
                {
                    mergedElements.Add(zone.Key,
                                       new MergedElements(zone.ConnectedNodesNumbers));
                }

                mergedElements[zone.Key].AddZone(zone.Number);
            }

            return mergedElements.Values.Where(el => el.ConnectedZoneNumbers.Count > 1).ToArray();
        }

        private static Maps ComputeMaps(MergedElements[] mergedElements)
        {

            var mainZoneToElementMap = new Dictionary<int, string>();
            var zoneToMainZoneMap = new Dictionary<int, int>();

            int counter = 0;
            foreach (var mergedElement in mergedElements)
            {
                if (mergedElement.ConnectedZoneNumbers.Count <= 1) continue;

                counter++;
                mainZoneToElementMap.Add(counter, mergedElement.Key);
                foreach (var connectedZoneNumber in mergedElement.ConnectedZoneNumbers)
                {
                    if (connectedZoneNumber == 0) Debugger.Break();
                    zoneToMainZoneMap.Add(connectedZoneNumber, counter);
                }                
            }

            return new Maps() { MainZoneToElementMap = mainZoneToElementMap, ZoneToMainZoneMap = zoneToMainZoneMap};
        }

        private static Dictionary<int, int> ComputeStopPointMap(string stopPointsFilePath)
        {
            var lines = File.ReadAllLines(stopPointsFilePath);

            string[] keys = { "NO" };
            var result = ComputeHeaders(lines, "$STOPPOINT:", keys);
            int stopPointNumberIndex = result.Value.headers[keys[0]];
            int nodeNumberIndex = result.Value.headers["NODENO"];
            int fromNodeNumberIndex = result.Value.headers["FROMNODENO"];

            Dictionary<int, int>? stopPointToNodeMap = new();
            foreach (var line in lines.Skip(result.Value.headerLine).ToArray())
            {
                if (string.IsNullOrEmpty(line)) continue;
                var fields = line.Split(';');

                int stopPointNumber = int.Parse(fields[stopPointNumberIndex]);

                string nodeNumberString = fields[nodeNumberIndex];
                int nodeNumber = string.IsNullOrEmpty(nodeNumberString) ? int.Parse(fields[fromNodeNumberIndex]) : int.Parse(nodeNumberString);
                stopPointToNodeMap.Add(stopPointNumber, nodeNumber);
            }
            return stopPointToNodeMap;
        }
    }
}
