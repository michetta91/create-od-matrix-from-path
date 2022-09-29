namespace CreateMainZonesAccordingToStopPoint
{
    internal class MergedElements
    {
        internal int OriginZoneNumber { get; private set; }
        private HashSet<int> ConnectedStopPointNumbers { get; set; }

        internal MergedElements(int originZoneNumber)
        {
            OriginZoneNumber = originZoneNumber;
            ConnectedStopPointNumbers = new HashSet<int>();
        }

        internal void AddElement(int connectedStopPointNumber)
        {
            ConnectedStopPointNumbers.Add(connectedStopPointNumber);
        }

        internal HashSet<int> GetConnectedStopPointNumbers()
        {
            return ConnectedStopPointNumbers;
        }

    }
}
