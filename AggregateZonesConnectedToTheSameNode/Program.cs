using AggregateZonesConnectedToTheSameNode;
using System.Text.Json;

string fileName = Path.Combine("Resources", "Configuration.json");
string jsonString = File.ReadAllText(fileName);
var configuration = JsonSerializer.Deserialize<Configuration>(jsonString);
if (configuration is null || !configuration.IsValid)
{
    Console.WriteLine("Invalid configuration");
    return;
}

var maps = Solver.ComputeMaps(configuration.InputFilePath);
if (maps.ZoneToMainZoneMap is null || maps.MainZoneToElementMap is null)
{
    Console.WriteLine("Invalid maps");
    return;
}

var outputDirectory = Path.GetDirectoryName(configuration.OutputFilePathAtt);
if (!Directory.Exists(outputDirectory)) Directory.CreateDirectory(outputDirectory);
Exporter.ExportToNetFile(configuration.OutputFilePathNet, maps);
Exporter.ExportToAttFile(configuration.OutputFilePathAtt, maps.ZoneToMainZoneMap);



