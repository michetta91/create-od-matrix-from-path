using System.Text;

namespace CreateODMatrixFromPath
{
    internal static class Solver
    {
        private const string TABLE_KEY = "$PUTPATH:";
        private const string ORIGIN_KEY = "ORIGZONENO";
        private const string DESTINATION_KEY = "DESTZONENO";
        private const string TRIPS_KEY = "ODTRIPS";
        internal static IEnumerable<ODPair> ComputeOdPairs(string inputFilePath,
                                                           string keepAttributeName,
                                                           HashSet<string> elementsToKeep)
        {
            var inputOdPairs = ImportOdPairs(inputFilePath, keepAttributeName);
            var filteredOdPairs = FilterAndAggregateOdPairs(inputOdPairs, elementsToKeep);
            return filteredOdPairs;
        }

        private static IEnumerable<ODPair> ImportOdPairs(string inputFilePath, string attributeToKeepName)
        {
            var odPairs = new List<ODPair>();

            using (var fileStream = File.OpenRead(inputFilePath))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                var counter = 0;
                var line = streamReader.ReadLine();
                while (counter < 10 && !line.StartsWith(TABLE_KEY))
                {
                    line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line)) counter++;
                }
                if (string.IsNullOrEmpty(line)) return odPairs;

                line = line.Replace(TABLE_KEY, string.Empty);
                var headersArray = line.Split(';');
                var headers = Enumerable.Range(0, headersArray.Length).ToDictionary(x => headersArray[x]);
                if (!headers.ContainsKey(attributeToKeepName)) return odPairs;


                while (!string.IsNullOrEmpty(line) && !string.IsNullOrEmpty(line))
                {
                    line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line)) return odPairs;

                    var fields = line.Split(';');

                    var odPair = new ODPair(
                        int.Parse(fields[headers[ORIGIN_KEY]]),
                        int.Parse(fields[headers[DESTINATION_KEY]]),
                        double.Parse(fields[headers[TRIPS_KEY]]),
                        fields[headers[attributeToKeepName]].Split(',').Where(el => !string.IsNullOrEmpty(el)).ToHashSet()
                    );
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
                odPair.ElementToKeepCodes.IntersectWith(transportSystemToKeep);
                if (odPair.ElementToKeepCodes.Any())
                {
                    if (!filteredOdPairs.ContainsKey(odPair.Key))
                    {
                        filteredOdPairs.Add(odPair.Key, odPair);
                    }
                    else
                    {
                        var oldODPair = filteredOdPairs[odPair.Key];
                        filteredOdPairs[odPair.Key] = oldODPair.Sum(odPair);
                    }
                }
            }
            return filteredOdPairs.Values;
        }


    }
}
