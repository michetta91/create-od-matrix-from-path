using create_od_matrix_from_path;
using System.Text.Json;

string fileName = Path.Combine("Resources","Configuration.json");
string jsonString = File.ReadAllText(fileName);
var configuration = JsonSerializer.Deserialize<Configuration>(jsonString);
if (configuration is null || !configuration.IsValid) return;

var elementsToKeep = File.ReadAllLines(configuration.InputOdPairsFileName).ToHashSet();
if (!elementsToKeep.Any()) return;

Console.WriteLine("Element to keep:");
Console.WriteLine(string.Join(",", elementsToKeep));
Console.WriteLine("Press any key to continue");
Console.ReadLine();

var odPairs = Solver.ComputeOdPairs(configuration.InputFilePath, configuration.KeepAttributeName, elementsToKeep);
Exporter.ExportToMatFile(configuration.OutputFilePath, odPairs);



