namespace create_od_matrix_from_path
{
    internal static class ODPairExtensions
    {

        internal static ODPair Sum(this ODPair odPair1, ODPair odPair2)
        {
            if(odPair1.Key !=  odPair2.Key)
            {
                throw new ArgumentException($"Cannot sum od pair with different keys: {odPair1.Key} != {odPair2.Key}");
            }

            var elementsToKeep = odPair1.ElementToKeepCodes;
            elementsToKeep.UnionWith(odPair2.ElementToKeepCodes);

            var sumOdPair = new ODPair(odPair1.OriginZoneNo, odPair1.DestinationZoneNo, odPair1.OdTrips + odPair2.OdTrips, elementsToKeep);
            return sumOdPair;   
        }
    }
}
