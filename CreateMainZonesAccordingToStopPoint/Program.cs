using CreateMainZonesAccordingToStopPoint;
using System.Text.Json;

string fileName = Path.Combine("Resources", "Configuration.json");
string jsonString = File.ReadAllText(fileName);
var configuration = JsonSerializer.Deserialize<Configuration>(jsonString);
if (configuration is null || !configuration.IsValid)
{
    Console.WriteLine("Invalid configuration");
    return;
}

var transitStopToKeep = File.ReadAllLines(configuration.TransitStopsToKeepFilePath).ToHashSet();
if (!transitStopToKeep.Any()) return;

Console.WriteLine("Element to merge:");
Console.WriteLine(string.Join(",", transitStopToKeep));
Console.WriteLine("Press ENTER to continue");
Console.ReadLine();

var maps = Solver.ComputeMaps(configuration.FolderPath, transitStopToKeep.Select(el => int.Parse(el)).ToHashSet());
if (maps.ZoneToMainZoneMap is null || maps.MainZoneToElementMap is null)
{
    Console.WriteLine("Invalid maps");
    return;
}

var outputDirectory = Path.GetDirectoryName(configuration.OutputFilePathAtt);
if (!Directory.Exists(outputDirectory)) Directory.CreateDirectory(outputDirectory);
Exporter.ExportToAttFile(configuration.OutputFilePathAtt, maps.ZoneToMainZoneMap);
Exporter.ExportToNetFile(configuration.OutputFilePathNet, maps.MainZoneToElementMap);



