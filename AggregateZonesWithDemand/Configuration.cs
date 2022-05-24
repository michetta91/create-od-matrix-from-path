namespace AggregateZonesWithDemand
{
    internal class Configuration
    {
        public string InputFileFolder { get; set; } = string.Empty;    
     
        public bool IsValid => !(string.IsNullOrEmpty(InputFileFolder)
            || !Directory.Exists(InputFileFolder));
    }
}
