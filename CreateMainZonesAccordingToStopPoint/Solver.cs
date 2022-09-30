using System.Diagnostics;

namespace CreateMainZonesAccordingToStopPoint
{
    internal class Solver
    {

        public static Maps ComputeMaps(string inputFolder,
                                       HashSet<int> transitStopToKeep)
        {

            var elementsToMerge = ImportPathLegs(inputFolder, transitStopToKeep);
            return MergeElements(elementsToMerge);
        }

        private static List<ElementToMerge> ImportPathLegs(string inputFolder, HashSet<int> transitStopToKeep)
        {

            Dictionary<int, ElementToMerge> elementsToMerge = new();
            var files = Directory.GetFiles(inputFolder).Where(file => Path.GetExtension(file) == ".att");
            foreach (var file in files)
            {
                var result = ComputeHeaders(file);
                if (result is null) continue;


                var linesOrigin = File.ReadAllLines(file).Skip(result.Value.headerLine).ToArray();
                int originZoneNumberIndex = result.Value.headers["ORIGZONENO"];
                int fromStopPointNoIndex = result.Value.headers["FROMSTOPPOINTNO"];
                elementsToMerge.ImportStops(transitStopToKeep, linesOrigin, originZoneNumberIndex, fromStopPointNoIndex, fromOrigin: true);

                var linesDestination = linesOrigin.Reverse().ToArray();
                int destinationZoneNumberIndex = result.Value.headers["DESTZONENO"];
                int toStopPointNoIndex = result.Value.headers["TOSTOPPOINTNO"];
                elementsToMerge.ImportStops(transitStopToKeep, linesDestination, destinationZoneNumberIndex, toStopPointNoIndex, fromOrigin: false);
            }

            return elementsToMerge.Values.ToList();
        }

        private static (int headerLine, Dictionary<string, int> headers)? ComputeHeaders(string file)
        {
            var lines = File.ReadAllLines(file);
            int counter = 0;
            foreach (var line in lines)
            {
                if (line.StartsWith("$PUTPATHLEG:"))
                {
                    var headersArray = line.Replace("$PUTPATHLEG:", "").Split(';');
                    var headers = Enumerable.Range(0, headersArray.Length).ToDictionary(x => headersArray[x]);
                    if (headers is null ||
                        !headers.ContainsKey("ORIGZONENO") || 
                        !headers.ContainsKey("DESTZONENO")) return null;

                    return (counter + 1, headers);

                }
                counter++;
            }
            return null;
        }

        private static Maps MergeElements(List<ElementToMerge> elementsToMerge)
        {
            var mergedElements = new Dictionary<string, MergedElements>();
            foreach (var element in elementsToMerge)
            {
                if (element.ConnectedTransitStopNumbers.Count == 0) continue;

                if (!mergedElements.ContainsKey(element.Key))
                {
                    if (element.Key == "1388") Debugger.Break();
                    mergedElements.Add(element.Key,
                                       new MergedElements(element.ConnectedTransitStopNumbers));
                }

                mergedElements[element.Key].AddZone(element.OriginZoneNumber);
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
