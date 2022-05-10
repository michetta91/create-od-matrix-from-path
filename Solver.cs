using System.Text;

namespace create_od_matrix_from_path
{
    internal static class Solver
    {
        internal static IEnumerable<ODPair> ComputeOdPairs(Configuration configuration)
        {
            var inputOdPairs = ImportOdPairs(configuration.InputFilePath);
            var filteredOdPairs = FilterAndAggregateOdPairs(inputOdPairs, configuration.TransportSystemToKeep);
            return filteredOdPairs;
        }

        private static IEnumerable<ODPair> ImportOdPairs(string inputFilePath)
        {
            var odPairs = new List<ODPair>();

            using (var fileStream = File.OpenRead(inputFilePath))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                var line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line) && !line.StartsWith("$PUTPATH:"))
                {
                    line = streamReader.ReadLine();             
                }
                if (line == null) return odPairs;

                line = line.Replace("$PUTPATH:", string.Empty);
                var headersArray = line.Split(';');
                var headers = Enumerable.Range(0, headersArray.Length).ToDictionary(x => headersArray[x]);
                while (!string.IsNullOrEmpty(line) && !string.IsNullOrEmpty(line))
                {
                    line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line)) return odPairs;

                    var fields = line.Split(';');

                    var odPair = new ODPair
                    {
                        OriginZoneNo = int.Parse(fields[headers["ORIGZONENO"]]),
                        DestinationZoneNo = int.Parse(fields[headers["DESTZONENO"]]),
                        OdTrips = double.Parse(fields[headers["ODTRIPS"]]),
                        TransportSystemCodes = fields[headers["TRANSPORTSYSTEMCODES"]].Split(',').Where(el => !string.IsNullOrEmpty(el)).ToHashSet()
                    };
                    odPairs.Add(odPair);
                }


            }
            return odPairs;

        }

        private static IEnumerable<ODPair> FilterAndAggregateOdPairs(IEnumerable<ODPair> inputOdPairs,
                                                   HashSet<string> transportSystemToKeep)
        {
            var filteredOdPairs = new Dictionary<string, ODPair>();

            foreach (ODPair odPair in inputOdPairs)
            {
                odPair.TransportSystemCodes.IntersectWith(transportSystemToKeep);
                if (odPair.TransportSystemCodes.Any())
                {
                    
                    if (filteredOdPairs.TryGetValue(odPair.Key, out ODPair oldOdPair))
                    {
                        oldOdPair.TransportSystemCodes.UnionWith(odPair.TransportSystemCodes);
                        oldOdPair.OdTrips += odPair.OdTrips;
                    }
                    else
                    {
                        filteredOdPairs.Add(odPair.Key, odPair);
                    }
                }

            }
            return filteredOdPairs.Values;
        }


    }
}
