namespace CreateMainZonesAccordingToStopPoint
{
    internal class ElementToMerge
    {
        internal int  ZoneNumber { get; private set; }
        internal SortedSet<int> ConnectedTransitStopNumbers { get; private set; }

        internal string Key { get => string.Join("-", ConnectedTransitStopNumbers); }

        internal ElementToMerge(int zoneNumber)
        {
            ZoneNumber = zoneNumber;
            ConnectedTransitStopNumbers = new SortedSet<int>();
        }

        internal void AddConnectedTransitStop(int connectedTransitStopNumber)
        {
            ConnectedTransitStopNumbers.Add(connectedTransitStopNumber);
        }
    }
}
