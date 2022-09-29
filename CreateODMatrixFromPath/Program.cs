using CreateODMatrixFromPath;
using System.Text.Json;

string fileName = Path.Combine("Resources","Configuration.json");
string jsonString = File.ReadAllText(fileName);
var configuration = JsonSerializer.Deserialize<Configuration>(jsonString);
if (configuration is null || !configuration.IsValid)
{
    Console.WriteLine("Invalid configuration");
    return;
}

var elementsToKeep = File.ReadAllLines(configuration.ElementsToKeepFilePath).ToHashSet();
if (!elementsToKeep.Any()) return;

Console.WriteLine("Element to keep:");
Console.WriteLine(string.Join(",", elementsToKeep));
Console.WriteLine("Press ENTER to continue");
Console.ReadLine();

var odPairs = Solver.ComputeOdPairs(configuration.InputFilePath, configuration.KeepAttributeName, elementsToKeep);
Exporter.ExportToMatToKeepMultiplier(configuration.OutputFilePath, odPairs);



