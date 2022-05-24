// See https://aka.ms/new-console-template for more information
using AggregateZonesWithDemand;
using System.Text.Json;

string fileName = Path.Combine("Resources", "Configuration.json");
string jsonString = File.ReadAllText(fileName);
var configuration = JsonSerializer.Deserialize<Configuration>(jsonString);
if (configuration is null || !configuration.IsValid) return;

var zonesToKeep = Solver.ImportDistinctZones(configuration.InputFileFolder);
Exporter.ExportToAttFile(Path.Combine(configuration.InputFileFolder, "ZonesToKeep.att"), zonesToKeep);
