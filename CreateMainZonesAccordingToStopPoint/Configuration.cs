namespace CreateMainZonesAccordingToStopPoint
{
    internal class Configuration
    {
        public string FolderPath { get; set; } = string.Empty;

        public string OutputFileName { get; set; } = string.Empty;

        public string TransitStopsToKeepFileName { get; set; } = string.Empty;  

        public string TransitStopsToKeepFilePath { get => Path.Combine(FolderPath, TransitStopsToKeepFileName); }

        public string OutputFilePathNet { get => Path.Combine(FolderPath, OutputFileName, ".MainZones.net"); }
        public string OutputFilePathAtt { get => Path.Combine(FolderPath, OutputFileName, ".Zones.att"); }     
    
        public bool IsValid => !(string.IsNullOrEmpty(FolderPath)
            || string.IsNullOrEmpty(OutputFileName)
            || string.IsNullOrEmpty(TransitStopsToKeepFileName)
            || !File.Exists(TransitStopsToKeepFilePath));
    }
}
