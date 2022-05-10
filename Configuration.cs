namespace create_od_matrix_from_path
{
    internal class Configuration
    {
        public string InputFilePath { get; set; } = string.Empty;

        public HashSet<string> TransportSystemToKeep { get; set; } = new();

        public string OutputFilePath { get; set; } = string.Empty;

        public bool IsValid => !(string.IsNullOrEmpty(InputFilePath) || !File.Exists(InputFilePath) || string.IsNullOrEmpty(OutputFilePath) || !TransportSystemToKeep.Any());
    }
}
