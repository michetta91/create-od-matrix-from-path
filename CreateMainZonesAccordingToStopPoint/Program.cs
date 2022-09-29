using CreateMainZonesAccordingToStopPoint;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

string fileName = Path.Combine("Resources", "Configuration.json");
string jsonString = File.ReadAllText(fileName);
var configuration = JsonSerializer.Deserialize<Configuration>(jsonString);
if (configuration is null || !configuration.IsValid)
{
    Console.WriteLine("Invalid configuration");
    return;
}

var elementsToMerge = File.ReadAllLines(configuration.ElementsToMergeFileName).ToHashSet();
if (!elementsToMerge.Any()) return;

Console.WriteLine("Element to merge:");
Console.WriteLine(string.Join(",", elementsToMerge));
Console.WriteLine("Press ENTER to continue");
Console.ReadLine();

var odPairs = Solver.ComputeOdPairs(configuration.InputFilePath, configuration.KeepAttributeName, elementsToMerge);
Exporter.ExportToMatToKeepMultiplier(configuration.OutputFilePath, odPairs);




