namespace CreateODMatrixFromPath
{
    internal class Configuration
    {
        public string FolderPath { get; set; } = string.Empty;
        public string InputOdPairsFileName { get; set; } = string.Empty;

        public string OutputMatrixFileName { get; set; } = string.Empty;

        public string ElementsToKeepFileName { get; set; } = string.Empty;

        /// <summary>
        /// Per TSYS UDA formula: DISTINCT:PUTPATHS\DISTINCT:PUTPATHLEGSWITHOUTWALK\TSYSCODE
        /// Per LINE UDA formula: DISTINCT:PUTPATHS\DISTINCT:PUTPATHLEGSWITHOUTWALK\LINENAME
        /// </summary>
        public string KeepAttributeName { get; set; } = string.Empty;


        public string InputFilePath { get => Path.Combine(FolderPath, InputOdPairsFileName); }
        public string OutputFilePath { get => Path.Combine(FolderPath, OutputMatrixFileName); }
        public string ElementsToKeepFilePath { get => Path.Combine(FolderPath, ElementsToKeepFileName); }

        public bool IsValid => !(string.IsNullOrEmpty(FolderPath)
            || string.IsNullOrEmpty(InputOdPairsFileName)
            || string.IsNullOrEmpty(OutputMatrixFileName)
            || string.IsNullOrEmpty(ElementsToKeepFileName)
            || string.IsNullOrEmpty(KeepAttributeName)
            || !File.Exists(InputFilePath)
            || !File.Exists(ElementsToKeepFilePath));
    }
}
