using System.Collections.Generic;
using System.Text;

namespace CreateMainZonesAccordingToStopPoint
{
    internal class Solver
    {

        public static Maps ComputeMaps(string inputFolder,
                                       HashSet<int> transitStopToKeep)
        {

            var elementsToMerge = ImportPathLegs(inputFolder);
            return MergeElements(transitStopToKeep, elementsToMerge);
        }

        private static List<ElementToMerge> ImportPathLegs(string inputFolder)
        {
            Dictionary<int, ElementToMerge> elementsToMerge = new();
            var files = Directory.GetFiles(inputFolder).Where(file => Path.GetExtension(file) == ".att");
            foreach (var file in files)
            {
                using var fileStream = File.OpenRead(file);
                using var streamReader = new StreamReader(fileStream, Encoding.UTF8);
                var counter = 0;
                var line = streamReader.ReadLine();
                while (!line.StartsWith("$PUTPATHLEG:"))
                {
                    line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line)) counter++;
                }
                if (string.IsNullOrEmpty(line)) break;

                line = line.Replace("$PUTPATHLEG:", string.Empty);
                var headersArray = line.Split(';');
                var headers = Enumerable.Range(0, headersArray.Length).ToDictionary(x => headersArray[x]);
                if (!headers.ContainsKey("ORIGZONENO")) break;


                int originZoneNumberIndex = headers["ORIGZONENO"];
                int fromStopPointNoIndex = headers["FROMSTOPPOINTNO"];
                int currentOriginZoneNumber = 0;

                while (true)
                {
                    line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line)) break;
                    var fields = line.Split(';');

                    var originZoneNumber = fields[originZoneNumberIndex];
                    if (!string.IsNullOrEmpty(originZoneNumber)) currentOriginZoneNumber = int.Parse(originZoneNumber);

                    var connectedStopPoint = fields[fromStopPointNoIndex];
                    if (string.IsNullOrEmpty(connectedStopPoint)) continue;

                    if (!elementsToMerge.ContainsKey(currentOriginZoneNumber))
                    {
                        elementsToMerge.Add(currentOriginZoneNumber, new ElementToMerge(currentOriginZoneNumber));
                    }
                    elementsToMerge[currentOriginZoneNumber].AddConnectedTransitStop(int.Parse(connectedStopPoint));
                }
            }

            return elementsToMerge.Values.ToList();
        }

        private static Maps MergeElements(HashSet<int> transitStopToKeep,
                                          List<ElementToMerge> elementsToMerge)
        {

            var mergedElements = new Dictionary<string, MergedElements>();
            foreach (var element in elementsToMerge)
            {
                element.ConnectedTransitStopNumbers.IntersectWith(transitStopToKeep);
                if (element.ConnectedTransitStopNumbers.Count == 0) continue;

                if (!mergedElements.ContainsKey(element.Key))
                {
                    mergedElements.Add(element.Key,
                                       new MergedElements(element.ConnectedTransitStopNumbers));
                }

                mergedElements[element.Key].AddZone(element.OriginZoneNumber);
            }


            var mainZoneToElementMap = new Dictionary<int, string>();
            var zoneToMainZoneMap = new Dictionary<int, int>();

            int counter =0;
            foreach (var mergedElement in mergedElements.Values)
            {
                counter++;
                mainZoneToElementMap.Add(counter, mergedElement.Key);
                foreach (var connectedZoneNumber in mergedElement.ConnectedZoneNumbers)
                {
                    zoneToMainZoneMap.Add(connectedZoneNumber, counter);
                }

            }

            return new Maps() { MainZoneToElementMap = mainZoneToElementMap, ZoneToMainZoneMap = zoneToMainZoneMap };
        }
    }
}
