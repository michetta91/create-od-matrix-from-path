namespace CreateMainZonesAccordingToStopPoint
{
    internal class ElementToMerge
    {
        internal int OriginZoneNumber { get; private set; }
        internal int ConnectedElement { get; private set; } 

        internal ElementToMerge(int originZoneNumber, int connectedElement)
        {
            OriginZoneNumber = originZoneNumber;
            ConnectedElement = connectedElement;
        }
    }
}
