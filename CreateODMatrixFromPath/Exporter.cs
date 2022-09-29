using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateODMatrixFromPath
{
    internal static class Exporter
    {

        public static void ExportToMatFile(string exportFilePath, IEnumerable<ODPair> odPairs)
        {
            if (File.Exists(exportFilePath)) File.Delete(exportFilePath);

            var myLines = new List<string>();
            myLines.Add("$O;D3");
            myLines.Add("* From To");
            myLines.Add("- -");
            myLines.Add("* Factor");
            myLines.Add("1.00");
            myLines.Add("*");

            foreach (var odPair in odPairs)
            {
                myLines.Add($"{odPair.OriginZoneNo} {odPair.DestinationZoneNo} {odPair.OdTrips}");
            }

            myLines.Add("* Network object names");
            myLines.Add("$NAMES");

            File.WriteAllLines(exportFilePath, myLines);
        }

        public static void ExportToMatToKeepMultiplier(string exportFilePath, IEnumerable<ODPair> odPairs)
        {
            if (File.Exists(exportFilePath)) File.Delete(exportFilePath);

            var myLines = new List<string>();
            myLines.Add("$O;D3");
            myLines.Add("* From To");
            myLines.Add("- -");
            myLines.Add("* Factor");
            myLines.Add("1.00");
            myLines.Add("*");

            foreach (var odPair in odPairs)
            {
                myLines.Add($"{odPair.OriginZoneNo} {odPair.DestinationZoneNo} {1}");
            }

            myLines.Add("* Network object names");
            myLines.Add("$NAMES");

            File.WriteAllLines(exportFilePath, myLines);
        }
    }
}
