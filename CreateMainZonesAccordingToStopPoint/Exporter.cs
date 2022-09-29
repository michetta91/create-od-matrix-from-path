using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateMainZonesAccordingToStopPoint
{
    internal class Exporter
    {
        internal static void ExportToNetFile(string outputFilePathNet, Dictionary<int, string> mainZoneToElementMap)
        {

            if (File.Exists(outputFilePathNet)) File.Delete(outputFilePathNet);

            var myLines = new List<string>();
            myLines.Add("$VISION");
            myLines.Add("$VERSION:VERSNR;FILETYPE;LANGUAGE;UNIT");
            myLines.Add("13.00;Net;ENG;KM");
            myLines.Add("*");
            myLines.Add("* Table: Main zones");
            myLines.Add("*");
            myLines.Add("MAINZONE:NO;CODE;NAME");
            foreach (var mainZoneMap in mainZoneToElementMap)
            {
                myLines.Add($"{mainZoneMap.Key};{mainZoneMap.Value};{mainZoneMap.Value}");
            }
            File.WriteAllLines(outputFilePathNet, myLines);
        }

        internal static void ExportToAttFile(string outputFilePathAtt, Dictionary<int, int> zoneToMainZoneMap)
        {
            if (File.Exists(outputFilePathAtt)) File.Delete(outputFilePathAtt);

            var myLines = new List<string>();
            myLines.Add("$VISION");
            myLines.Add("$VERSION:VERSNR;FILETYPE;LANGUAGE;UNIT");
            myLines.Add("13.00;Att;ENG;KM");
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
