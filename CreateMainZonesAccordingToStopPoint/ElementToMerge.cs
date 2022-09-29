namespace CreateMainZonesAccordingToStopPoint
{
    internal class ElementToMerge
    {
        internal int OriginZoneNumber { get; private set; }
        internal SortedSet<int> ConnectedTransitStopNumbers { get; private set; }

        internal string Key { get => string.Join("-", ConnectedTransitStopNumbers); }

        internal ElementToMerge(int originZoneNumber)
        {
            OriginZoneNumber = originZoneNumber;
            ConnectedTransitStopNumbers = new SortedSet<int>();
        }

        internal void AddConnectedTransitStop(int connectedTransitStopNumber)
        {
            ConnectedTransitStopNumbers.Add(connectedTransitStopNumber);
        }
    }
}
