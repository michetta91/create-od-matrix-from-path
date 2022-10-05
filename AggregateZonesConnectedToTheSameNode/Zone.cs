using System.Collections.Immutable;

namespace AggregateZonesConnectedToTheSameNode
{
    internal class Zone
    {
        internal int Number { get; private set; }

        internal int[] ConnectedNodesNumbers { get; private set; }

        internal string Key { get => string.Join("-", ConnectedNodesNumbers); }

        internal Zone(string number, string connectedNodes)
        {
            Number = int.Parse(number);
            
            ConnectedNodesNumbers =connectedNodes.Split(",").Select(el => int.Parse(el)).OrderBy(el => el).ToArray();
        }
    }
}
