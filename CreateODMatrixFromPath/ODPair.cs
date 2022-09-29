namespace CreateODMatrixFromPath
{
    internal class ODPair
    {
        internal int OriginZoneNo { get; private set; }
        internal int DestinationZoneNo { get; private set; }
        internal double OdTrips { get; private set; }
        internal HashSet<string> ElementToKeepCodes { get; private set; } = new HashSet<string>();

        internal string Key => $"{OriginZoneNo};{DestinationZoneNo}";

        internal ODPair(int originZoneNo, int destinationZoneNo, double odTrips, HashSet<string> elementToKeepCodes)
        {
            OriginZoneNo = originZoneNo;
            DestinationZoneNo = destinationZoneNo;
            OdTrips = odTrips;
            ElementToKeepCodes = elementToKeepCodes;
        }

    }
}
