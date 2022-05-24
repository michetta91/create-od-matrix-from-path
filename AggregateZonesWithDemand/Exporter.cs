using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregateZonesWithDemand
{
    internal class Exporter
    {
        internal static void ExportToAttFile(string outputFilePath, HashSet<string> zonesToKeep)
        {
            if (File.Exists(outputFilePath)) File.Delete(outputFilePath);

            var myLines = new List<string>();
            myLines.Add("$VISION");
            myLines.Add("$VERSION:VERSNR;FILETYPE;LANGUAGE;UNIT");
            myLines.Add("13.00;Att;ENG;KM");
            myLines.Add("*");
            myLines.Add("* Table: Zones");
            myLines.Add("*");
            myLines.Add("$ZONE:NO;WITHDEMAND");
            foreach (var zone in zonesToKeep)
            {
                myLines.Add($"{zone};1");
            }
            File.WriteAllLines(outputFilePath, myLines);
        }
    }
}
