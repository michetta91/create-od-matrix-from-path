namespace CreateMainZonesAccordingToStopPoint
{
    internal class Exporter
    {
        internal static void ExportToNetFile(string outputFilePathNet, Maps maps)
        {
            if (File.Exists(outputFilePathNet)) File.Delete(outputFilePathNet);

            var myLines = new List<string>();
            myLines.Add("$VISION");
            myLines.Add("$VERSION:VERSNR;FILETYPE;LANGUAGE;UNIT");
            myLines.Add("13.000;Net;ENG;KM");
            myLines.Add("*");
            myLines.Add("* Table: Main zones");
            myLines.Add("*");
            myLines.Add("$MAINZONE:NO;CODE;NAME;XCOORD;YCOORD");
            foreach (var mainZoneMap in maps.MainZoneToElementMap)
            {    
                myLines.Add($"{mainZoneMap.Key};{mainZoneMap.Value};{mainZoneMap.Value};0;0");
            }
            myLines.Add("");

            myLines.Add("*");
            myLines.Add("* Table: Connectors");
            myLines.Add("*");
            myLines.Add("$CONNECTOR:ZONENO;NODENO;DIRECTION;TYPENO;TSYSSET");
            foreach (var zoneToMainZone in maps.ZoneToMainZoneMap)
            {
                var stopPointNumbersString = maps.MainZoneToElementMap[zoneToMainZone.Value];
                string[] stopPointNumbers = stopPointNumbersString.Split("-");
             
                foreach(var stopPointNumber in stopPointNumbers)
                {
                    var nodeNumer = maps.StopPointsMap[int.Parse(stopPointNumber)];
                    myLines.Add($"{zoneToMainZone.Key};{nodeNumer};O;9;");
                    myLines.Add($"{zoneToMainZone.Key};{nodeNumer};D;9;");
                }
            }

            File.WriteAllLines(outputFilePathNet, myLines);
        }

        internal static void ExportToAttFile(string outputFilePathAtt, Dictionary<int, int> zoneToMainZoneMap)
        {
            if (File.Exists(outputFilePathAtt)) File.Delete(outputFilePathAtt);

            var myLines = new List<string>();
            myLines.Add("$VISION");
            myLines.Add("$VERSION:VERSNR;FILETYPE;LANGUAGE;UNIT");
            myLines.Add("13.000;Att;ENG;KM");
            myLines.Add("*");
            myLines.Add("* Table: Zones");
            myLines.Add("*");
            myLines.Add("$ZONE:NO;MAINZONENO");
            foreach (var zoneMap in zoneToMainZoneMap)
            {
                myLines.Add($"{zoneMap.Key};{zoneMap.Value}");
            }
            File.WriteAllLines(outputFilePathAtt, myLines);
        }

    }
}
