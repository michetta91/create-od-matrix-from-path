namespace CreateMainZonesAccordingToStopPoint
{
    internal class PutPathLeg
    {

        internal int OriginZoneNumber { get; private set; }
        internal int DestinationZoneNumber { get; private set; }
        internal int PathIndex { get; private set; }

        internal int FromStopPointNumber { get; private set; }
        internal int ToStopPointNumber { get; private set; }

        internal string Key { get => $"{OriginZoneNumber};{DestinationZoneNumber};{PathIndex}"; }

        internal PutPathLeg(string originZoneNumber,
                            string destinationZoneNumber,
                            string pathIndex)
        {
            OriginZoneNumber = int.Parse(originZoneNumber);
            DestinationZoneNumber = int.Parse(destinationZoneNumber);
            PathIndex = int.Parse(pathIndex);

            FromStopPointNumber = int.MaxValue;
            ToStopPointNumber = int.MaxValue;
        }

        internal void AddStopPoint(int stopPointNumber, bool isFrom)
        {
            if (isFrom)
            {
                if (FromStopPointNumber == int.MaxValue) FromStopPointNumber = stopPointNumber;
            }
            else
            {
                ToStopPointNumber = stopPointNumber;
            }
        }
    }

}
