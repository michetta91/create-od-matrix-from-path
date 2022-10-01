namespace CreateMainZonesAccordingToStopPoint
{
    internal class Configuration
    {
        public string FolderPath { get; set; } = string.Empty;
        public string InputFileName { get; set; } = string.Empty;
        public string OutputFileName { get; set; } = string.Empty;

        public string LinesToKeepFileName { get; set; } = string.Empty;

        public string InputFilePath { get => Path.Combine(FolderPath, InputFileName); }
        public string LinesToKeepFilePath { get => Path.Combine(FolderPath, LinesToKeepFileName); }
        public string OutputFilePathNet { get => Path.Combine(FolderPath, "Output", $"{OutputFileName}.MainZones.net"); }
        public string OutputFilePathAtt { get => Path.Combine(FolderPath, "Output", $"{OutputFileName}.Zones.att"); }

        public bool IsValid => !(string.IsNullOrEmpty(FolderPath)
            || string.IsNullOrEmpty(InputFileName)
            || string.IsNullOrEmpty(LinesToKeepFileName)
            || string.IsNullOrEmpty(OutputFileName)
            || !File.Exists(InputFilePath) 
            || !File.Exists(LinesToKeepFilePath));
    }
}
