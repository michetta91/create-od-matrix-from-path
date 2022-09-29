namespace CreateMainZonesAccordingToStopPoint
{
    internal class MergedElements
    {
        private SortedSet<int> StopPointNumbers { get; set; }
        internal SortedSet<int> ConnectedZoneNumbers { get; private set; }

        internal string Key { get => string.Join("-", StopPointNumbers); }

        internal MergedElements(SortedSet<int> stopPointNumberspPoints)
        {
            StopPointNumbers = stopPointNumberspPoints;
            ConnectedZoneNumbers = new SortedSet<int>();
        }

        internal void AddZone(int zoneNumber)
        {
            ConnectedZoneNumbers.Add(zoneNumber);
        }
    }
}
