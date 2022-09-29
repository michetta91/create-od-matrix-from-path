namespace CreateMainZonesAccordingToStopPoint
{
    internal class Configuration
    {
        public string FolderPath { get; set; } = string.Empty;

        public string OutputFileName { get; set; } = string.Empty;

        public string ElementsToMergeFileName { get; set; } = string.Empty;

        /// <summary>
        /// Per TSTP: FROMSTOPPOINTNO
        /// </summary>
        public string MergeAttributeName { get; set; } = string.Empty;

        public string ElementsToKeepFilePath { get => Path.Combine(FolderPath, ElementsToMergeFileName); }

        public string OutputFilePathNet { get => Path.Combine(FolderPath, OutputFileName, ".MainZones.net"); }
        public string OutputFilePathAtt { get => Path.Combine(FolderPath, OutputFileName, ".Zones.att"); }     
    
        public bool IsValid => !(string.IsNullOrEmpty(FolderPath)
            || string.IsNullOrEmpty(OutputFileName)
            || string.IsNullOrEmpty(ElementsToMergeFileName)
            || string.IsNullOrEmpty(MergeAttributeName)
            || !File.Exists(ElementsToKeepFilePath));
    }
}
