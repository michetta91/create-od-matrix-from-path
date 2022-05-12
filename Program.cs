// See https://aka.ms/new-console-template for more information
using create_od_matrix_from_path;
using System.Text.Json;

string fileName = Path.Combine("Resources","Configuration.json");
string jsonString = File.ReadAllText(fileName);
var configuration = JsonSerializer.Deserialize<Configuration>(jsonString);
if (configuration is null || !configuration.IsValid) return;

var odPairs = Solver.ComputeOdPairs(configuration);
Exporter.ExportToMatFile(configuration.OutputFilePath, odPairs);

