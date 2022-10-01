using System.Diagnostics;
namespace CreateMainZonesAccordingToStopPoint
{
    internal class Solver
    {

        public static Maps ComputeMaps(string inputFilePath,
                                       HashSet<string> linesToKeep)
        {

            var putPathLegs = ImportPathLegs(inputFilePath, linesToKeep);
            if(putPathLegs is null) return new Maps();

            var elementsToMerge = ComputeElementsToMerger(putPathLegs);
            return MergeElements(elementsToMerge);
        }

        private static PutPathLeg[]? ImportPathLegs(string inputFilePath, HashSet<string> linesToKeep)
        {

            Dictionary<string, PutPathLeg> putPathLegs = new();

            var result = ComputeHeaders(inputFilePath);
            if (result is null) return null;

            int originZoneNumberIndex = result.Value.headers["ORIGZONENO"];
            int destinationZoneNumberIndex = result.Value.headers["DESTZONENO"];
            int pathIndexIndex = result.Value.headers["PATHINDEX"];

            int lineIndex = result.Value.headers["LINENAME"];
            int fromStopPointNoIndex = result.Value.headers["FROMSTOPPOINTNO"];
            int toStopPointNoIndex = result.Value.headers["TOSTOPPOINTNO"];

            PutPathLeg? currentPathLeg = null;
            var lines = File.ReadAllLines(inputFilePath).Skip(result.Value.headerLine).ToArray();
            foreach (var line in lines)
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

            return putPathLegs.Values.ToArray();
        }

        private static (int headerLine, Dictionary<string, int> headers)? ComputeHeaders(string inputFilePath)
        {
            var lines = File.ReadAllLines(inputFilePath);
            int counter = 0;
            foreach (var line in lines)
            {
                if (line.StartsWith("$PUTPATHLEG:"))
                {
                    var headersArray = line.Replace("$PUTPATHLEG:", "").Split(';');
                    var headers = Enumerable.Range(0, headersArray.Length).ToDictionary(x => headersArray[x]);
                    if (headers is null ||
                        !headers.ContainsKey("ORIGZONENO") ||
                        !headers.ContainsKey("DESTZONENO") ||
                        !headers.ContainsKey("PATHINDEX")) return null;

                    return (counter + 1, headers);

                }
                counter++;
            }
            return null;
        }

        internal static string Key(string originZoneNumber, string destinationZoneNumber, string pathIndex)
        {
            return $"{originZoneNumber};{destinationZoneNumber};{pathIndex}";
        }

        private static ElementToMerge[] ComputeElementsToMerger(PutPathLeg[] putPathLegs)
        {
            var elementsToMerge = new Dictionary<int, ElementToMerge>();
            foreach (var putPathLeg in putPathLegs)
            {
                //if (putPathLeg.FromStopPointNumber == int.MaxValue ||
                //    putPathLeg.ToStopPointNumber == int.MaxValue)
                //{
                //    Debugger.Break();
                //}

                if (putPathLeg.FromStopPointNumber != int.MaxValue)
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

        private static Maps MergeElements(ElementToMerge[] elementsToMerge)
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


            var mainZoneToElementMap = new Dictionary<int, string>();
            var zoneToMainZoneMap = new Dictionary<int, int>();

            int counter = 0;
            foreach (var mergedElement in mergedElements.Values)
            {
                counter++;
                mainZoneToElementMap.Add(counter, mergedElement.Key);
                foreach (var connectedZoneNumber in mergedElement.ConnectedZoneNumbers)
                {
                    if (connectedZoneNumber == 0) Debugger.Break();
                    zoneToMainZoneMap.Add(connectedZoneNumber, counter);
                }

            }

            return new Maps() { MainZoneToElementMap = mainZoneToElementMap, ZoneToMainZoneMap = zoneToMainZoneMap };
        }
    }
}
