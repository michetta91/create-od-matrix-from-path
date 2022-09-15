using System.Text;

namespace AggregateZonesWithDemand
{
    internal class Solver
    {

        internal static HashSet<string> ImportDistinctZones(string InputFileFolder)
        {
            HashSet<string> importedZones = new();

            var files = Directory.GetFiles(InputFileFolder).Where(file => Path.GetExtension(file) == ".mtx");
            foreach (var file in files)
            {
                using var fileStream = File.OpenRead(file);
                using var streamReader = new StreamReader(fileStream, Encoding.UTF8);
                var counter = 0;
                var line = streamReader.ReadLine();
                while (counter < 5 && !line.StartsWith("$"))
                {
                    line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line)) counter++;
                }
                if (string.IsNullOrEmpty(line)) break;

                line = line.Replace("$", string.Empty);
                var headersArray = line.Split(';');
                var headers = Enumerable.Range(0, headersArray.Length).ToDictionary(x => headersArray[x]);
                if (!headers.ContainsKey("O")) break;


                line = streamReader.ReadLine();
                while (counter < 4 || !line.StartsWith("*"))
                {
                    line = streamReader.ReadLine();
                    counter++;
                }

                var counter2 = 0;
                int oIndex = headers["O"];
                int dIndex = headers["D3"];
                while (true)
                {
                    line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line) || line.StartsWith("*")) break;
                    counter2++;
                    var fields = line.Split(' ');
                    importedZones.Add(fields[oIndex]);
                    importedZones.Add(fields[dIndex]);
                }
            }
            return importedZones;
        }
    }
}
