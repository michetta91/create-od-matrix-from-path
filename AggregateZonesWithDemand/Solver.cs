using System.Text;

namespace AggregateZonesWithDemand
{
    internal class Solver
    {

        internal static HashSet<string> ImportDistinctZones(string InputFileFolder)
        {
            HashSet<string> importedZones = new();

            var files = Directory.GetFiles(InputFileFolder).Where(file => Path.GetExtension(file) == ".att");
            foreach (var file in files)
            {
                using var fileStream = File.OpenRead(file);
                using var streamReader = new StreamReader(fileStream, Encoding.UTF8);
                var counter = 0;
                var line = streamReader.ReadLine();
                while (counter < 10 && !line.StartsWith("$ZONE:"))
                {
                    line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line)) counter++;
                }
                if (string.IsNullOrEmpty(line)) break;

                line = line.Replace("$ZONE:", string.Empty);
                var headersArray = line.Split(';');
                var headers = Enumerable.Range(0, headersArray.Length).ToDictionary(x => headersArray[x]);
                if (!headers.ContainsKey("NO")) break;

                var noIndex = headers["NO"];
                while (true)
                {
                    line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line)) break;

                    var fields = line.Split(';');
                    importedZones.Add(fields[noIndex]);
                }
            }
            return importedZones;
        }
    }
}
