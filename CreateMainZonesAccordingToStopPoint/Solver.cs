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
            var mergedElements = new Dictionary<int, MergedElements>();
            foreach (var element in elementsToMerge)
            {
                if (!mergedElements.ContainsKey(element.OriginZoneNumber))
                {
                    mergedElements.Add(element.OriginZoneNumber,
                                       new MergedElements(element.OriginZoneNumber));
                }

                if (transitStopToKeep.Contains(element.ConnectedElement))
                {
                    mergedElements[element.OriginZoneNumber].AddElement(element.ConnectedElement);
                }
            }
            return ConvertMergedElementsToMap(mergedElements);
        }

        private static List<ElementToMerge> ImportPathLegs(string inputFolder)
        {
            List<ElementToMerge> elementsToMerge = new();

            var files = Directory.GetFiles(inputFolder).Where(file => Path.GetExtension(file) == ".att");
            foreach (var file in files)
            {
                using var fileStream = File.OpenRead(file);
                using var streamReader = new StreamReader(fileStream, Encoding.UTF8);
                var counter = 0;
                var line = streamReader.ReadLine();
                while (counter < 5 && !line.StartsWith("$PUTPATHLEG"))
                {
                    line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line)) counter++;
                }
                if (string.IsNullOrEmpty(line)) break;

                line = line.Replace("$PUTPATHLEG", string.Empty);
                var headersArray = line.Split(';');
                var headers = Enumerable.Range(0, headersArray.Length).ToDictionary(x => headersArray[x]);
                if (!headers.ContainsKey("ORIGZONENO")) break;
                               

                int originZoneNumberIndex = headers["ORIGZONENO"];
                int fromStopPointNoIndex = headers["FROMSTOPPOINTNO"];
                string currentOriginZoneNumber = string.Empty;
                while (true)
                {
                    line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line)) break;
                    var fields = line.Split(' ');

                    var originZoneNumber = fields[originZoneNumberIndex];
                    if (!string.IsNullOrEmpty(originZoneNumber)) currentOriginZoneNumber = originZoneNumber;

                    var connectedStopPoint = fields[originZoneNumberIndex];
                    if (!string.IsNullOrEmpty(connectedStopPoint)) break;

                    var elementToMerge = new ElementToMerge(int.Parse(currentOriginZoneNumber),
                                                            int.Parse(connectedStopPoint));
                    elementsToMerge.Add(elementToMerge);                    
                }
            }

            return elementsToMerge;
        }

        private static Maps ConvertMergedElementsToMap(Dictionary<int, MergedElements> mergedElements)
        {

            var mainZoneToElementMap = new Dictionary<int, HashSet<int>>();
            var zoneToMainZoneMap = new Dictionary<int, int>();
            foreach (var mergedElement in mergedElements)
            {

            }

            return new Maps() { MainZoneToElementMap = mainZoneToElementMap, ZoneToMainZoneMap = zoneToMainZoneMap};
        }
    }
}
