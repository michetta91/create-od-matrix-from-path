namespace AggregateZonesConnectedToTheSameNode
{
    internal class MergedElements
    {
        private int[] ConnectedNodeNumbers { get; set; }
        internal SortedSet<int> ConnectedZoneNumbers { get; private set; }

        internal string Key { get => string.Join("-", ConnectedNodeNumbers); }

        internal MergedElements(int[] connectedNodeNumbers)
        {
            ConnectedNodeNumbers = connectedNodeNumbers;
            ConnectedZoneNumbers = new SortedSet<int>();
        }

        internal void AddZone(int zoneNumber)
        {
            ConnectedZoneNumbers.Add(zoneNumber);
        }
    }
}
