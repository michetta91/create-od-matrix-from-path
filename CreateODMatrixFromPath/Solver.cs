using System.Text;

namespace create_od_matrix_from_path
{
    internal static class Solver
    {
        internal static IEnumerable<ODPair> ComputeOdPairs(string inputFilePath,
                                                           string keepAttributeName,
                                                           HashSet<string> elementsToKeep)
        {
            var inputOdPairs = ImportOdPairs(inputFilePath, keepAttributeName);
            var filteredOdPairs = FilterAndAggregateOdPairs(inputOdPairs, elementsToKeep);
            return filteredOdPairs;
        }

        private static IEnumerable<ODPair> ImportOdPairs(string inputFilePath, string tsysAttributeName)
        {
            var odPairs = new List<ODPair>();

            using (var fileStream = File.OpenRead(inputFilePath))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                var counter = 0;
                var line = streamReader.ReadLine();
                while (counter < 10 && !line.StartsWith("$PUTRELATION:"))
                {
                    line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line)) counter++;
                }
                if (string.IsNullOrEmpty(line)) return odPairs;

                line = line.Replace("$PUTRELATION:", string.Empty);
                var headersArray = line.Split(';');
                var headers = Enumerable.Range(0, headersArray.Length).ToDictionary(x => headersArray[x]);
                if (!headers.ContainsKey(tsysAttributeName)) return odPairs;


                while (!string.IsNullOrEmpty(line) && !string.IsNullOrEmpty(line))
                {
                    line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line)) return odPairs;

                    var fields = line.Split(';');

                    var odPair = new ODPair
                    {
                        OriginZoneNo = int.Parse(fields[headers["ORIGZONENO"]]),
                        DestinationZoneNo = int.Parse(fields[headers["DESTZONENO"]]),
                        OdTrips = double.Parse(fields[headers["ODTRIPSTOTAL"]]),
                        TransportSystemCodes = fields[headers[tsysAttributeName]].Split(',').Where(el => !string.IsNullOrEmpty(el)).ToHashSet()
                    };
                    odPairs.Add(odPair);
                }


            }
            return odPairs;

        }

        private static IEnumerable<ODPair> FilterAndAggregateOdPairs(IEnumerable<ODPair> inputOdPairs,
                                                   HashSet<string> transportSystemToKeep)
        {
            var filteredOdPairs = new List<ODPair>();
            foreach (ODPair odPair in inputOdPairs)
            {
                odPair.TransportSystemCodes.IntersectWith(transportSystemToKeep);
                if (odPair.TransportSystemCodes.Any())
                {

                    filteredOdPairs.Add(odPair);
                }

            }
            return filteredOdPairs;
        }


    }
}
