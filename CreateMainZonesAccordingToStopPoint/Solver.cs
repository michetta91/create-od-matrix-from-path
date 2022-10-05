using System.Diagnostics;
namespace CreateMainZonesAccordingToStopPoint
{
    internal class Solver
    {

        public static Maps ComputeMaps(string inputFilePath,
                                       string stopPointsFilePath,
                                       HashSet<string> linesToKeep,
                                       bool mergeAccordingToDestination)
        {

            var putPathLegs = ImportPathLegs(inputFilePath, linesToKeep);
            if (putPathLegs is null) return new Maps();

            var elementsToMerge = ComputeElementsToMerger(putPathLegs, mergeAccordingToDestination);
            var mergedElements = MergeElements(elementsToMerge);

            return ComputeMaps(mergedElements, stopPointsFilePath);
        }

        private static PutPathLeg[]? ImportPathLegs(string inputFilePath, HashSet<string> linesToKeep)
        {

            Dictionary<string, PutPathLeg> putPathLegs = new();

            var lines = File.ReadAllLines(inputFilePath);

            string[] keys = { "ORIGZONENO", "DESTZONENO", "PATHINDEX" };
            var result = ComputeHeaders(lines, "$PUTPATHLEG:", keys);
            if (result is null) return null;

            int originZoneNumberIndex = result.Value.headers[keys[0]];
            int destinationZoneNumberIndex = result.Value.headers[keys[1]];
            int pathIndexIndex = result.Value.headers[keys[2]];

            int lineIndex = result.Value.headers["LINENAME"];
            int fromStopPointNoIndex = result.Value.headers["FROMSTOPPOINTNO"];
            int toStopPointNoIndex = result.Value.headers["TOSTOPPOINTNO"];

            PutPathLeg? currentPathLeg = null;
            foreach (var line in lines.Skip(result.Value.headerLine).ToArray())
            {
                if (string.IsNullOrEmpty(line)) continue;
                var fields = line.Split(';');

                string originZoneNumber = fields[originZoneNumberIndex];
                string destinationZoneNumber = fields[destinationZoneNumberIndex];
                string pathIndex = fields[pathIndexIndex];
                if (!string.IsNullOrEmpty(originZoneNumber) &&
                    !string.IsNullOrEmpty(destinationZoneNumber) &&
                    !string.IsNullOrEmpty(pathIndex))
                {
                    var key = Key(originZoneNumber, destinationZoneNumber, pathIndex);
                    if (!putPathLegs.ContainsKey(key))
                    {
                        var pathLeg = new PutPathLeg(originZoneNumber, destinationZoneNumber, pathIndex);
                        putPathLegs.Add(pathLeg.Key, pathLeg);
                    }
                    currentPathLeg = putPathLegs[key];
                    continue;
                }
                if (currentPathLeg is null) continue;

                var lineName = fields[lineIndex];
                if (linesToKeep.Contains(lineName))
                {
                    var fromStopPointNumber = fields[fromStopPointNoIndex];
                    currentPathLeg.AddStopPoint(int.Parse(fromStopPointNumber), true);
                    var toStopPointNumber = fields[toStopPointNoIndex];
                    currentPathLeg.AddStopPoint(int.Parse(toStopPointNumber), false);
                }
            }

            return putPathLegs.Values.
                Where(putPathLeg => putPathLeg.FromStopPointNumber != int.MaxValue || putPathLeg.ToStopPointNumber != int.MaxValue).
                ToArray();
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

        private static ElementToMerge[] ComputeElementsToMerger(PutPathLeg[] putPathLegs, bool mergeAccordingToDestination)
        {
            var elementsToMerge = new Dictionary<int, ElementToMerge>();
            foreach (var putPathLeg in putPathLegs)
            {
                if (!mergeAccordingToDestination &&
                    putPathLeg.FromStopPointNumber != int.MaxValue)
                {
                    if (!elementsToMerge.ContainsKey(putPathLeg.OriginZoneNumber))
                    {
                        elementsToMerge.Add(putPathLeg.OriginZoneNumber, new ElementToMerge(putPathLeg.OriginZoneNumber));
                    }

                    elementsToMerge[putPathLeg.OriginZoneNumber].AddConnectedTransitStop(putPathLeg.FromStopPointNumber);
                }


                if (putPathLeg.ToStopPointNumber != int.MaxValue)
                {
                    if (!elementsToMerge.ContainsKey(putPathLeg.DestinationZoneNumber))
                    {
                        elementsToMerge.Add(putPathLeg.DestinationZoneNumber, new ElementToMerge(putPathLeg.DestinationZoneNumber));
                    }
                    elementsToMerge[putPathLeg.DestinationZoneNumber].AddConnectedTransitStop(putPathLeg.ToStopPointNumber);

                }
            }

            return elementsToMerge.Values.ToArray();
        }

        private static MergedElements[] MergeElements(ElementToMerge[] elementsToMerge)
        {
            var mergedElements = new Dictionary<string, MergedElements>();
            foreach (var element in elementsToMerge)
            {
                if (element.ConnectedTransitStopNumbers.Count == 0) continue;

                if (!mergedElements.ContainsKey(element.Key))
                {
                    mergedElements.Add(element.Key,
                                       new MergedElements(element.ConnectedTransitStopNumbers));
                }

                mergedElements[element.Key].AddZone(element.ZoneNumber);
            }

            return mergedElements.Values.ToArray();
        }

        private static Maps ComputeMaps(MergedElements[] mergedElements, string stopPointsFilePath)
        {

            var mainZoneToElementMap = new Dictionary<int, string>();
            var zoneToMainZoneMap = new Dictionary<int, int>();

            int counter = 0;
            foreach (var mergedElement in mergedElements)
            {
                counter++;
                mainZoneToElementMap.Add(counter, mergedElement.Key);
                foreach (var connectedZoneNumber in mergedElement.ConnectedZoneNumbers)
                {
                    if (connectedZoneNumber == 0) Debugger.Break();
                    zoneToMainZoneMap.Add(connectedZoneNumber, counter);
                }

            }

            var stopPointToNodeMap = ComputeStopPointMap(stopPointsFilePath);

            return new Maps() { MainZoneToElementMap = mainZoneToElementMap, ZoneToMainZoneMap = zoneToMainZoneMap, StopPointsMap = stopPointToNodeMap };
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
